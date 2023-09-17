using Zdog.Maui.Extensions;
using SkiaSharp;

namespace Zdog.Maui.Render
{
    public class Text : Anchor
    {
        Paint paint = new Paint()
        {
            IsAntialias = true,
        };

        string text = string.Empty;
        int textSize = 12;//sp
        SKTypeface typeface = SKTypeface.Default;
        public float centerX { get; private set; }
        public float centerY { get; private set; }

        public override void onCreate()
        {
            base.onCreate();
            updateDimention();
        }

        void updateDimention()
        {
            paint.TextSize = textSize;
            paint.Typeface = typeface;
            centerX = paint.MeasureText(text) / 2;
            centerY = (paint.FontMetrics.Bottom - paint.FontMetrics.Top) / 4;
        }

        public override void render(Renderer renderer)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            renderer.save();
            renderer.translate(_translate.x, _translate.y);
            renderer.scale(_scale.x, _scale.y);
            renderer.rotate(_rotate.z);
            renderer.Text(text, textSize, centerX, centerY, colour.ToInt(), renderAlpha, typeface);
            renderer.restore();
        }

        public override Text Copy()
        {
            return Copy(new Text());
        }

        protected override Text Copy(Anchor shape)
        {
            return (base.Copy(shape) as Text).Also((it) =>
            {
                it.text = text;
                it.textSize = textSize;
                it.typeface = typeface;
            });
        }
    }
}
