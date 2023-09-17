using Zdog.Maui.Extensions;

namespace Zdog.Maui.Render
{
    public class Hemisphere : Ellipse
    {
        public Hemisphere() { fill = true; }

        Anchor apex;
        Vector renderCentroid;

        public override void onCreate()
        {
            base.onCreate();
            apex = new Anchor
            {
                addTo = this,

            }.Also((it) =>
            {
                it.translate(z: diameter / 2);
            });
            renderCentroid = new Vector();
        }

        public override void updateSortValue()
        {
            renderCentroid.set(renderOrigin)
             .lerp(apex.renderOrigin, 3 / 8f);
            sortValue = renderCentroid.z;
        }

        public override void render(Renderer renderer)
        {
            renderDemo(renderer);
            base.render(renderer);
        }

        void renderDemo(Renderer renderer)
        {
            if (!visible)
            {
                return;
            }

            var contourAngle = Math.Atan2(renderNormal.y, renderNormal.x);
            var demoRadius = diameter / 2 * renderNormal.magnitude();
            var x = renderOrigin.x;
            var y = renderOrigin.y;

            var startAngle = contourAngle + Utils.TAU / 4;
            var endAnchor = contourAngle - Utils.TAU / 4;
            renderer.begin();
            renderer.arc(x, y, demoRadius, (float)startAngle, (float)endAnchor);
            renderer.Stroke(stroke > 0, colour.ToInt(), stroke, renderAlpha);
            renderer.Fill(fill, colour.ToInt(), renderAlpha);
            renderer.End();
        }
    }
}