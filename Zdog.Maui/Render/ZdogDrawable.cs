using SkiaSharp;

namespace Zdog.Maui.Render
{
    public abstract class IAndroidDrawable
    {
        public abstract void draw(SKCanvas Canvas);
        public abstract void onBoundsChange(SKRect bounds);
        public abstract void setAlpha(int alpha);
    }

    public class ZdogDrawable : IAndroidDrawable
    {
        public Illustration illo = new Illustration();
        //protected Animation animator = null;
        public override void draw(SKCanvas canvas)
        {
            illo.updateRenderGraph(canvas);
        }

        public override void onBoundsChange(SKRect bounds)
        {
            changeSize(bounds.Width, bounds.Height);
        }

        public override void setAlpha(int alpha)
        {
            illo._alpha = alpha / 255f;
        }

        private void changeSize(float width, float height)
        {
            illo.zoom = Math.Min(width, height) / 240f;
            illo.onResize(width, height);
        }

        public void addChild(Anchor shape)
        {
            illo.addChild(shape);
        }

        public void removeChild(Anchor shape)
        {
            shape.remove();
        }
    }
}
