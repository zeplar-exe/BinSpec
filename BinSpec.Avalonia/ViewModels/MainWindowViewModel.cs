using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

using BinSpec.Avalonia.MVVM;
using BinSpec.Avalonia.Views;

using ReactiveUI;

namespace BinSpec.Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<byte> SourceBytes { get; }
        public string SourceText => "";
        public ObservableProperty<string> FilePath { get; }
        public ICommand SaveCommand { get; }
        public ObservableProperty<bool> EditEnabled { get; }
        
        public ICommand? TextDisplayCut { get; set; }
        public ICommand? TextDisplayCopy { get; set; }
        public ICommand? TextDisplayPaste { get; set; }

        public MainWindowViewModel()
        {
            SourceBytes = new ObservableCollection<byte>();
            FilePath = new ObservableProperty<string>();
            EditEnabled = new ObservableProperty<bool>();
            
            SaveCommand = ReactiveCommand.Create(Save, 
                this.WhenAnyValue(vm => vm.EditEnabled.Value));
        }

        private void Save()
        {
            File.WriteAllBytes(FilePath.Value, SourceBytes.ToArray());
        }
    }
}