using BinSpec.Avalonia.MVVM;
using BinSpec.Avalonia.Views;

namespace BinSpec.Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableProperty<SwapTextReader> OpenReader { get; }

        public MainWindowViewModel()
        {
            OpenReader = new ObservableProperty<SwapTextReader>();
        }
    }
}