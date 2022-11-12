using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.ReactiveUI;

using BinSpec.Avalonia.Resources;
using BinSpec.Avalonia.ViewModels;
using BinSpec.Avalonia.Views.Dialogs;

namespace BinSpec.Avalonia.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        private bool EditEnabled { get; set; }
        
        public MainWindow()
        {
            ViewModel = new MainWindowViewModel();
            
            InitializeComponent();
            
            v_TextDisplay.AddHandler(TextInputEvent, TextDisplayInput, RoutingStrategies.Tunnel);
            // see https://github.com/AvaloniaUI/Avalonia/issues/3491#issuecomment-771151642
        }
        
        private async void TextDisplayInput(object? sender, TextInputEventArgs e)
        {
            if (EditEnabled)
            {
                return;
            }

            if (Settings.Get<bool>("ALLOW_DISPLAY_EDIT_DONT_ASK_AGAIN"))
            {
                EditEnabled = true;
                
                return;
            }
            
            if (Settings.Get<bool>("DISALLOW_DISPLAY_EDIT_DONT_ASK_AGAIN"))
            {
                EditEnabled = false;
                e.Handled = true;
                
                return;
            }

            e.Handled = true; // Stop the input from getting inserted during the dialog await

            var confirm = new ConfirmDialog
            {
                Message = DialogMsg.FileEditConfirm,
                ConfirmText = "Yes",
                CancelText = "No"
            };

            var result = await confirm.ShowDialog<ConfirmDialogResult>(this);
            
            if (result.Confirmed)
            {
                EditEnabled = true;

                if (result.DontAskAgain)
                {
                    Settings.Set("ALLOW_DISPLAY_EDIT_DONT_ASK_AGAIN", true);
                }
            }
            else
            {
                if (result.DontAskAgain)
                {
                    Settings.Set("DISALLOW_DISPLAY_EDIT_DONT_ASK_AGAIN", true);
                }
            }
        }

        private async void OpenBinaryFileClick(object? sender, RoutedEventArgs e)
        {
            var files = await OpenFileDialog();

            if (files.Length == 0)
                return;

            var file = files.First();
            await using var stream = File.OpenRead(file);
            using var reader = new BinaryTextReader(stream);
            
            ViewModel.SourceText.Value = reader.ReadToEnd();
        }
        
        private async Task<string[]> OpenFileDialog()
        {
            var dialog = new OpenFileDialog();
            var result = await dialog.ShowAsync(this);

            return result ?? Array.Empty<string>();
        }

        private async IAsyncEnumerable<byte> ReadAllBytesAsync(Stream stream, 
            [EnumeratorCancellation] CancellationToken cancel = default)
        {
            var buffer = new byte[1];

            while (await stream.ReadAsync(buffer, cancel) != 0)
            {
                cancel.ThrowIfCancellationRequested();
                
                yield return buffer[0];
            }
        }
    }
}