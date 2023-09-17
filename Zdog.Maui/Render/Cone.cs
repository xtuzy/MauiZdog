using Zdog.Maui.Extensions;  

namespace Zdog.Maui.Render
{
    public class Cone : Ellipse
    {
        public float length = 1;
        Anchor apex;
        Vector renderApex;
        Vector renderCentroid;
        Vector tangentA;
        Vector tangentB;
        List<PathCommand> surfacePathCommands;

        public Cone()
        {
            fill = true;
        }

        public override void onCreate()
        {
            base.onCreate();
            apex = new Anchor().setup((it) =>
            {
                it.addTo = this;
                it.translate(z: length);
            });

            renderApex = new Vector();
            renderCentroid = new Vector();
            tangentA = new Vector();
            tangentB = new Vector();

            surfacePathCommands = Extensions.EnumerableExtensions.listOf<PathCommand>(
                new Move(new Vector()),
                new Line(new Vector()),
                new Line(new Vector())
            );
        }

        public override void updateSortValue()
        {
            renderCentroid.set(renderOrigin)
            .lerp(apex.renderOrigin, 1 / 3f);
            sortValue = renderCentroid.z;
        }

        public override void render(Renderer renderer)
        {
            renderConeSurface(renderer);
            base.render(renderer);
        }

        void renderConeSurface(Renderer renderer)
        {
            if (!visible)
            {
                return;
            }
            renderApex.set(apex.renderOrigin).
                    subtract(renderOrigin);
            var scale = renderNormal.magnitude();
            var apexDistance = renderApex.magnitude2d();
            var normalDistance = renderNormal.magnitude2d();

            var eccenAngle = Math.Acos((normalDistance / scale));
            var eccen = Math.Sin(eccenAngle);
            var radius = diameter / 2 * scale;
            var isApexVisible = radius * eccen < apexDistance;
            if (!isApexVisible)
            {
                return;
            }

            var apexAngle = (Math.Atan2(renderNormal.y, renderNormal.x)
                + Utils.TAU / 2);
            var projectLength = apexDistance / eccen;
            var projectAngle = Math.Acos(radius / projectLength);

            tangentA.x = ((float)(Math.Cos(projectAngle) * radius * eccen));
            tangentA.y = ((float)(Math.Sin(projectAngle) * radius));

            tangentB.set(tangentA);
            tangentB.y *= -1;

            tangentA.rotateZ((float)apexAngle);
            tangentB.rotateZ((float)apexAngle);
            tangentA.add(renderOrigin);
            tangentB.add(renderOrigin);

            setSurfaceRenderPoint(0, tangentA);
            setSurfaceRenderPoint(1, apex.renderOrigin);
            setSurfaceRenderPoint(2, tangentB);

            renderer.renderPath(surfacePathCommands);
            renderer.Stroke(stroke > 0, colour.ToInt(), stroke, renderAlpha);
            renderer.Fill(fill, colour.ToInt(), renderAlpha);
            renderer.End();
        }

        void setSurfaceRenderPoint(int index, Vector point)
        {
            surfacePathCommands[index].renderPoint().set(point);
        }

        public override Cone Copy()
        {
            return Copy(new Cone());
        }

        protected override Cone Copy(Anchor shape)
        {
            return (base.Copy(shape) as Cone).Also((it) =>
            {
                it.length = length;
            });
        }
    }
}