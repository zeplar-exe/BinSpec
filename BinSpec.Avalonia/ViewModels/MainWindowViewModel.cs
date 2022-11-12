using System.IO;
using System.Windows.Input;

using BinSpec.Avalonia.MVVM;
using BinSpec.Avalonia.Views;

using ReactiveUI;

namespace BinSpec.Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableProperty<string> SourceText { get; }
        public ObservableProperty<string> FilePath { get; }
        public ICommand SaveCommand { get; }
        public ObservableProperty<bool> EditEnabled { get; }

        public MainWindowViewModel()
        {
            SourceText = new ObservableProperty<string>();
            FilePath = new ObservableProperty<string>();
            EditEnabled = new ObservableProperty<bool>();
            
            SaveCommand = ReactiveCommand.Create(Save, 
                this.WhenAnyValue(vm => vm.EditEnabled.Value));
        }

        private void Save()
        {
            File.WriteAllText(FilePath.Value, SourceText.Value);
        }
    }
}