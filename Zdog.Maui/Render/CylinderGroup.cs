using Zdog.Maui.Extensions;

namespace Zdog.Maui.Render
{
    public class CylinderGroup : Group
    {
        public float stroke = 1;
        public bool fill = true;

        List<PathCommand> pathCommands;
        internal Ellipse frontBase;
        internal Ellipse rearBase;

        public CylinderGroup()
        {
            updateSort = true;
        }

        public override void onCreate()
        {
            base.onCreate();
            pathCommands = Extensions.EnumerableExtensions.listOf<PathCommand>(new Move(new Vector()), new Line(new Vector()));
        }

        public override void render(Renderer renderer)
        {
            renderCylinderSurface(renderer);
            base.render(renderer);
        }

        void renderCylinderSurface(Renderer renderer)
        {
            if (!visible)
            {
                return;
            }
            var scale = frontBase.renderNormal.magnitude();
            var strokeWidth = frontBase.diameter * scale + frontBase.stroke;
            pathCommands[0].renderPoint().set(frontBase.renderOrigin);
            pathCommands[1].renderPoint().set(rearBase.renderOrigin);

            renderer.setLineCap(SkiaSharp.SKStrokeCap.Butt);
            renderer.renderPath(pathCommands);
            renderer.Stroke(true, colour.ToInt(), strokeWidth, renderAlpha);
            renderer.End();
            renderer.setLineCap(SkiaSharp.SKStrokeCap.Round);
        }

        public override CylinderGroup Copy()
        {
            return Copy(new CylinderGroup());
        }

        protected override CylinderGroup Copy(Anchor shape)
        {
            return (base.Copy(shape) as CylinderGroup).Also((it) =>
            {
                it.stroke = stroke;
                it.fill = fill;
            });
        }

        public override T copyGraph<T>(Action<T> block)
        {
            throw new InvalidOperationException("Couldn't copy graph for CylinderGroup");
        }
    }
}
