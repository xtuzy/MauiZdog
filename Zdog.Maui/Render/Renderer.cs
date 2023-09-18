using Zdog.Maui.Extensions;
using SkiaSharp;
using Microsoft.Maui.Graphics.Skia;

namespace Zdog.Maui.Render
{
    public class Renderer
    {
        private Path path = new Path();
        private Path dest = new Path();
        private Paint paint = new Paint()
        {
            IsAntialias = true,
            StrokeCap = SKStrokeCap.Round,
            StrokeJoin = SKStrokeJoin.Round,
        };
        internal Canvas canvas;
        internal Segment segment;
        private PathMeasure measure = new PathMeasure();

        public void setLineCap(SKStrokeCap value)
        {
            paint.StrokeCap = value;
        }

        public void clearRect(float x, float y, float width, float height, int color)
        {
            var oldStyle = paint.Style;
            FillStyle = color;
            canvas.DrawRect(x, y, width, height, paint);
            paint.Style = oldStyle;
        }

        public void save()
        {
            canvas.Save();
        }

        public void restore()
        {
            canvas.Restore();
        }

        public void translate(float x, float y)
        {
            canvas.Translate(x, y);
        }

        public void scale(float x, float y)
        {
            canvas.Scale(x, y);
        }

        public void rotate(float degree)
        {
            canvas.RotateDegrees(degree);
        }

        public void begin()
        {
            path.Reset();
        }

        public void move(Vector point)
        {
            path.MoveTo(point.x, point.y);
        }

        public void line(Vector point)
        {
            path.LineTo(point.x, point.y);
        }

        public void bezier(Vector cp0, Vector cp1, Vector end)
        {
            path.CubicTo(cp0.x, cp0.y, cp1.x, cp1.y, end.x, end.y);
        }

        public void arc(float x, float y, float radius, float start, float end)
        {
            path.ArcTo(new SKRect(x - radius, y - radius, x + radius, y + radius), start.Angle(), (end - start).Angle(), false);
        }

        public void circle(float x, float y, float radius)
        {
            path.AddCircle(x, y, radius, SKPathDirection.Clockwise);
        }

        public void closePath()
        {
            path.Close();
        }

        public void setPath()
        {
        }

        public void renderToPath(Path path, List<PathCommand> pathCommands, bool isClosed)
        {
            var previousPath = this.path;
            this.path = path;
            renderPath(pathCommands, isClosed);
            this.path = previousPath;
        }

        public void renderPath(List<PathCommand> pathCommands)
        {
            begin();
            foreach (var it in pathCommands)
            {
                it.render(this);
            }
        }

        public void renderPath(List<PathCommand> pathCommands, bool isClosed)
        {
            renderPath(pathCommands);
            if (isClosed)
            {
                closePath();
            }
        }

        public int StrokeStyle
        {
            get
            {
                return paint.Color.ToMauiColor().ToInt();
            }
            set
            {
                paint.Color = Color.FromInt(value).AsSKColor();
                paint.Style = SKPaintStyle.Stroke;
            }
        }

        public void Stroke(bool isStroke, int color, float lineWidth, int alpha, PathEffect effect = null, Shader shader = null, ShaderLayer layer = null)
        {
            if (!isStroke)
            {
                return;
            }

            paint.Color = Color.FromInt(color).ToSKColor();
            paint.StrokeWidth = lineWidth;
            Stroke(alpha, effect, shader, layer);
        }

        public void Stroke(int alpha, PathEffect effect = null, Shader shader = null, ShaderLayer layer = null)
        {
            paint.Style = SKPaintStyle.Stroke;
            paint.Color = paint.Color.ToMauiColor().WithAlpha(alpha).ToSKColor(); //paint.Alpha = alpha;
            paint.Shader =  shader;
            paint.PathEffect = effect;
            if (layer != null)
            {
                paint.SetShadowLayer(layer.Radius, layer.Dx, layer.Dy, Color.FromInt( layer.Color).ToSKColor());
                Draw();
                paint.ClearShadowLayer();
            }
            else
            {
                Draw();
            }
        }

        public int FillStyle
        {
            get
            {
                return paint.Color.ToMauiColor().ToInt();
            }
            set
            {
                paint.Color = Color.FromInt(value).ToSKColor();
                paint.Style = SKPaintStyle.Fill;
            }
        }

        public void Fill(bool isFill, int color, int alpha, Shader shader = null,
                         ShaderLayer layer = null)
        {
            if (!isFill)
            {
                return;
            }
            paint.Color = Color.FromInt(color).ToSKColor();
            Fill(alpha, shader, layer);
        }

        public void Fill(int alpha, Shader shader = null, ShaderLayer layer = null)
        {
            paint.Style = SKPaintStyle.Fill;
            paint.Color = paint.Color.ToMauiColor().WithAlpha(alpha).ToSKColor();//paint.Alpha = alpha;
            paint.Shader = shader;
            paint.PathEffect = null;
            if (layer != null)
            {
                paint.SetShadowLayer(layer.Radius, layer.Dx, layer.Dy, Color.FromInt(layer.Color).ToSKColor());
                Draw();
                paint.ClearShadowLayer();
            }
            else
            {
                Draw();
            }
        }

        public void Text(string text, float textSize, float centerX, float centerY, int color,
        int alpha, SKTypeface typeface)
        {
            paint.Style = SKPaintStyle.Fill;
            //paint.Alpha = alpha;
            paint.TextSize = textSize;
            paint.Typeface = typeface;
            paint.Color = Color.FromInt(color).WithAlpha(alpha).ToSKColor();
            canvas.DrawText(text, -centerX, centerY, paint);
        }

        public void End() { }

        private void Draw()
        {
            if (segment != null)
            {
                if (segment.Unbroken())
                {
                    canvas.DrawPath(path, paint);
                }
                else
                {
                    measure.SetPath(path, false);
                    var length = measure.Length;
                    if (length != 0f)
                    {
                        DrawSegment(segment, length * segment.Start, length * segment.End, length);
                    }
                }
            }
            else
            {
                canvas.DrawPath(path, paint);
            }
        }

        private void DrawSegment(Segment segment, float start, float end, float length)
        {
            dest.Reset();
            if (start > end)
            {
                measure.GetSegment(start, length, dest, true);
                measure.GetSegment(0f, end, dest, false);
            }
            else
            {
                measure.GetSegment(start, end, dest, true);
            }
            segment.Begin(dest);
            canvas.DrawPath(dest, paint);
        }
    }
}
