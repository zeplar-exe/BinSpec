using System;
using System.Reactive;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

using DynamicData.Binding;

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
        InitializeComponent();
        #if DEBUG
        this.AttachDevTools();
        #endif
    }

    private void InitializeComponent()
    {
        DataContext = this;
        
        AvaloniaXamlLoader.Load(this);
    }

    private void Confirm(object? sender, RoutedEventArgs e)
    {
        var box = this.FindControl<CheckBox>("v_DontAskAgain");
        
        Close(new ConfirmDialogResult(true, box.IsChecked ?? false));
    }
    
    private void Cancel(object? sender, RoutedEventArgs e)
    {
        var box = this.FindControl<CheckBox>("v_DontAskAgain");
        
        Close(new ConfirmDialogResult(false, box.IsChecked ?? false));
    }
}