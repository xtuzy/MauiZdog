using Zdog.Maui.Render;
using SkiaSharp.Views.Maui.Controls;

namespace Zdog.Maui.Demo;

public partial class XYZChangeBar : ContentView
{
    public XYZChangeBar()
    {
        InitializeComponent();
    }
    public SKCanvasView SKCanvasView { get; set; }
    public ZdogDrawable ZdogDrawable { get; set; }
    public string Title
    {
        set
        {
            title.Text = value;
        }
    }

    private void x_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        ZdogDrawable?.illo.rotate((float)e.NewValue, ZdogDrawable.illo._rotate.y, ZdogDrawable.illo._rotate.z);
        SKCanvasView?.InvalidateSurface();
    }

    private void y_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        ZdogDrawable?.illo.rotate(ZdogDrawable.illo._rotate.x, (float)e.NewValue, ZdogDrawable.illo._rotate.z);
        SKCanvasView?.InvalidateSurface();
    }

    private void z_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        ZdogDrawable?.illo.rotate(ZdogDrawable.illo._rotate.x, ZdogDrawable.illo._rotate.y, (float)e.NewValue);
        SKCanvasView?.InvalidateSurface();
    }
}