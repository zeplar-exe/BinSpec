using BinSpec.Avalonia.MVVM;
using BinSpec.Avalonia.Views;

namespace BinSpec.Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableProperty<string> SourceText { get; }

        public MainWindowViewModel()
        {
            SourceText = new ObservableProperty<string>();
        }
    }
}