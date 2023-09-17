using Zdog.Maui.Extensions;

namespace Zdog.Maui.Render
{
    public class Illustration : Anchor
    {
        bool centered = true;
        internal float zoom = 1f;
        bool resize = false;
        Action<Renderer> onPrerender = null;
        Action<float, float> _onResize = null;
        float width = 0f;
        float height = 0f;

        float right => _translate.x + width;
        float bottom => _translate.y + height;

        Renderer renderer = new Renderer();

        public void alpha(float alpha)
        {
            colour = colour.WithAlpha(alpha);
        }

        public void setSize(float width, float height)
        {
            this.width = width;
            this.height = height;
        }

        public void onResize(float width, float height)
        {
            setSize(width, height);
            _onResize?.Invoke(width, height);
        }

        public void renderGraph(Canvas canvas, Anchor item = null)
        {
            setCanvas(canvas);
            preRender();
            (item ?? this).renderGraph(renderer);
            postRender();
        }

        void preRender()
        {
            renderer.clearRect(_translate.x, _translate.y, width, height, colour.ToInt());
            renderer.save();
            if (centered)
            {
                var centerX = width / 2;
                var centerY = height / 2;
                renderer.translate(centerX, centerY);
            }
            var scale = zoom;
            renderer.scale(scale, scale);
            onPrerender?.Invoke(renderer);
        }

        void postRender()
        {
            renderer.restore();
        }

        public void updateRenderGraph(Canvas canvas, Anchor item = null)
        {
            updateGraph();
            renderGraph(canvas, item);
        }

        void setCanvas(Canvas canvas)
        {
            renderer.canvas = canvas;
            if (width == 0f || height == 0f)
            {
                //setSize(canvas.Width, canvas.Height);
            }
        }

        public Path renderToPath(Shape item)
        {
            return item.ToPath(renderer);
        }

        public override Illustration Copy()
        {
            return Copy(new Illustration());
        }

        protected override Illustration Copy(Anchor shape)
        {
            return (base.Copy(shape) as Illustration).Also((it) =>
            {
                it.centered = centered;
                it.zoom = zoom;
                it.resize = resize;
                it.onPrerender = onPrerender;
                it._onResize = _onResize;
                it.width = width;
                it.height = height;
            });
        }
    }
}