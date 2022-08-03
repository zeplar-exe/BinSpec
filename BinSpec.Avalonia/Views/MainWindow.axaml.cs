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
using Avalonia.Interactivity;
using Avalonia.ReactiveUI;

using BinSpec.Avalonia.ViewModels;

namespace BinSpec.Avalonia.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            ViewModel = new MainWindowViewModel();
            
            InitializeComponent();
        }

        private async void OpenBinaryFileClick(object? sender, RoutedEventArgs e)
        {
            var files = await OpenFileDialog();

            if (files.Length == 0)
                return;

            var file = files.First();
            await using var stream = File.OpenRead(file);

            var builder = new StringBuilder();

            await foreach (var b in ReadAllBytesAsync(stream))
            {
                var bits = new BitArray(new[] { b });

                for (var i = 0; i < bits.Length; i++)
                {
                    var bit = bits[i];

                    builder.Append(bit ? '1' : '0');
                }
            }

            ViewModel.DisplayText.Value = builder.ToString();
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