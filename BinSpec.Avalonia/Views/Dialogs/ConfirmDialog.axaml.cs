using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace BinSpec.Avalonia.Views.Dialogs;

public partial class ConfirmDialog : Window
{
    public static readonly StyledProperty<string> MessageProperty = 
        AvaloniaProperty.Register<ConfirmDialog, string>(nameof(Message));

    public string Message
    {
        get => GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public static readonly StyledProperty<string> ConfirmTextProperty = 
        AvaloniaProperty.Register<ConfirmDialog, string>(nameof(ConfirmText));

    public string ConfirmText
    {
        get => GetValue(ConfirmTextProperty);
        set => SetValue(ConfirmTextProperty, value);
    }

    public static readonly StyledProperty<string> CancelTextProperty = 
        AvaloniaProperty.Register<ConfirmDialog, string>(nameof(CancelText));

    public string CancelText
    {
        get => GetValue(CancelTextProperty);
        set => SetValue(CancelTextProperty, value);
    }

    public ConfirmDialog()
    {
        DataContext = this;
        
        InitializeComponent();
    }

    private void Confirm(object? sender, RoutedEventArgs e)
    {
        Close(new ConfirmDialogResult(true, v_DontAskAgain.IsChecked ?? false));
    }
    
    private void Cancel(object? sender, RoutedEventArgs e)
    {
        Close(new ConfirmDialogResult(false, v_DontAskAgain.IsChecked ?? false));
    }
}