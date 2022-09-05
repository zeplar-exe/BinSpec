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

using BinSpec.Avalonia.ViewModels;
using BinSpec.Avalonia.Views.Dialogs;

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
        }
        
        private async void TextDisplayInput(object? sender, TextInputEventArgs e)
        {
            var confirm = new ConfirmDialog
            {
                Message = new StringBuilder()
                    .Append("File editing is currently disabled, and thus disables any editing of this TextBox. ")
                    .Append("When enabled, you can directly edit the opened binary file.")
                    .AppendLine()
                    .AppendLine()
                    .Append("Would you like to enable it now? Be weary of the risks involved with editing ")
                    .Append("certain files (like executables).")
                    .ToString(),
                ConfirmText = "Yes",
                CancelText = "No"
            };
            
            if (!await confirm.ShowDialog<bool>(this))
                return;
        }

        private async void OpenBinaryFileClick(object? sender, RoutedEventArgs e)
        {
            var files = await OpenFileDialog();

            if (files.Length == 0)
                return;

            var file = files.First();
            using var stream = File.OpenRead(file);
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