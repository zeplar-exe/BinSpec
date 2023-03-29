using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.ReactiveUI;

using BinSpec.Avalonia.Resources;
using BinSpec.Avalonia.ViewModels;
using BinSpec.Avalonia.Views.Dialogs;

using ReactiveUI;

namespace BinSpec.Avalonia.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            ViewModel = new MainWindowViewModel();
            
            InitializeComponent();
            
            v_TextDisplay.AddHandler(TextInputEvent, TextDisplayInput, RoutingStrategies.Tunnel);
            // see https://github.com/AvaloniaUI/Avalonia/issues/3491#issuecomment-771151642

            ViewModel.TextDisplayCut = ReactiveCommand.Create(TextDisplayCutAction,
                this.WhenAnyValue(w => w.v_TextDisplay.CanCut));
            ViewModel.TextDisplayCopy = ReactiveCommand.Create(TextDisplayCopyAction,
                    this.WhenAnyValue(w => w.v_TextDisplay.CanCopy));
            ViewModel.TextDisplayPaste = ReactiveCommand.Create(TextDisplayPasteAction,
                    this.WhenAnyValue(w => w.v_TextDisplay.CanPaste));
        }
        
        private void TextDisplayCutAction() => v_TextDisplay.Cut();
        private void TextDisplayCopyAction() => v_TextDisplay.Copy();
        private void TextDisplayPasteAction() => v_TextDisplay.Paste();
        
        private async void TextDisplayInput(object? sender, TextInputEventArgs e)
        {
            if (!e.Text?.All(c => c is '0' or '1') ?? false)
            {
                e.Handled = true;
                return;
            }
            
            if (ViewModel.EditEnabled.Value)
            {
                return;
            }

            if (Settings.Get<bool>("ALLOW_DISPLAY_EDIT_DONT_ASK_AGAIN"))
            {
                ViewModel.EditEnabled.Value = true;
                
                return;
            }
            
            if (Settings.Get<bool>("DISALLOW_DISPLAY_EDIT_DONT_ASK_AGAIN"))
            {
                ViewModel.EditEnabled.Value = false;
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
                ViewModel.EditEnabled.Value = true;

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
            ViewModel.SourceBytes.Clear();
            
            var files = await OpenFileDialog();

            if (files.Length == 0)
                return;

            var file = files.First();
            await using var stream = File.OpenRead(file);
            using var reader = new BinaryTextReader(stream);

            //await foreach (var b in ReadAllBytesAsync(stream))
            //{
                //ViewModel.SourceBytes.Add(b);
            //}

            v_TextDisplay.Text = await reader.ReadToEndAsync();
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