using BinSpec.Avalonia.MVVM;

namespace BinSpec.Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableProperty<string> DisplayText { get; }

        public MainWindowViewModel()
        {
            DisplayText = new ObservableProperty<string>();
        }
    }
}