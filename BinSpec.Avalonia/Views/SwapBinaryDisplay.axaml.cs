using System;
using System.Linq;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

using BinSpec.Avalonia.MVVM;

namespace BinSpec.Avalonia.Views;

public partial class SwapBinaryDisplay : UserControl, IScrollable
{
    public BinaryTextReader? Source
    {
        get => (BinaryTextReader?)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }
    
    public static readonly AvaloniaProperty<BinaryTextReader?> SourceProperty =
        AvaloniaProperty.Register<SwapBinaryDisplay, BinaryTextReader?>("Source");

    public ObservableProperty<Thickness> TextMargin { get; } // TODO
    public ObservableProperty<int> LineSpacing { get; }
    public ObservableProperty<int> CharacterSpacing { get; }
    
    public Vector Offset { get; set; } // TODO
    public Size Extent { get; } // TODO
    public Size Viewport => new(Width, Height);

    static SwapBinaryDisplay()
    {
        AffectsRender<SwapBinaryDisplay>(SourceProperty);
    }
    
    public SwapBinaryDisplay()
    {
        TextMargin = new ObservableProperty<Thickness>();
        LineSpacing = new ObservableProperty<int>();
        CharacterSpacing = new ObservableProperty<int>();

        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);
        
        if (Source == null)
            return;
        
        var measure = new FormattedText
        {
            TextWrapping = TextWrapping.Wrap,
            FontSize = FontSize,
            Typeface = new Typeface(FontFamily.Name),
            Text = "0",
        };

        var singleCharacterRect = measure.HitTestTextPosition(0);
        var totalCharacterWidth = singleCharacterRect.Width + CharacterSpacing.Value;
        var totalLineHeight = singleCharacterRect.Height + LineSpacing.Value;

        var lineCount = (int)Math.Ceiling(Height / totalLineHeight);
        
        if (lineCount <= 0)
            return;

        var lineCharacterWidth = (int)Math.Ceiling(Width / totalCharacterWidth);
        
        if (lineCharacterWidth == 0)
            return;

        var startIndex = (int)Math.Floor(Offset.Y / totalLineHeight) * lineCharacterWidth;

        Source.BaseStream.Position = startIndex;

        var bufferSize = lineCount * lineCharacterWidth;
        var buffer = new string[bufferSize];

        // Source.Read(buffer);
        
        var text = new FormattedText
        {
            TextWrapping = TextWrapping.Wrap,
            FontSize = FontSize,
            Typeface = new Typeface(FontFamily.Name),
            Text = string.Concat(buffer.SelectMany(c => c))
        };
        
        context.DrawText(Brushes.Black, new Point(0, 0), text);
    }
}