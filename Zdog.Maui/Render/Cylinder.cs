using Zdog.Maui.Extensions;

namespace Zdog.Maui.Render
{
    public class Cylinder : Shape
    {
        public float diameter = 1;
        public float length = 1;
        public string frontFace;

        float _stroke = 1;
        public override float stroke
        {
            get => _stroke;
            set
            {
                _stroke = value;
                if (frontBase != null) frontBase.stroke = value;
                if (rearBase != null) frontBase.stroke = value;
                if (group != null) group.stroke = value;
            }
        }

        bool _fill = true;
        public override bool fill
        {
            get => _fill;
            set
            {
                _fill = value;
                if (frontBase != null) frontBase.fill = value;
                if (rearBase != null) frontBase.fill = value;
                if (group != null) group.fill = value;
            }
        }
        
        string _color;
        public override string color
        {
            get => colour.ToArgbHex();
            set
            {
                colour = Color.Parse(value);
                if (frontBase != null) frontBase.color = value;
                if (rearBase != null) frontBase.color = value;
                if (group != null) group.color = value;
            }
        }

        bool _visible = true;
        public override bool visible
        {
            get => _visible;
            set
            {
                _visible = value;
                if (frontBase != null) frontBase.visible = value;
                if (rearBase != null) frontBase.visible = value;
                if (group != null) group.visible = value;
            }
        }


        CylinderGroup group;
        Ellipse frontBase;
        Ellipse rearBase;

        public override void onCreate()
        {
            base.onCreate();

            var group = new CylinderGroup().setup((it) =>
            {
                it.addTo = this;
                it.color = color;
                it.visible = visible;
            });

            var baseZ = length / 2;
            var baseColor = backface != null ? backface : null;
            group.frontBase = new Ellipse().setup((it) =>
            {
                it.addTo = group;
                it.diameter = diameter;
                it.translate(z: baseZ);
                it.rotate(y: (Utils.TAU / 2));
                it.color = color;
                it.stroke = stroke;
                it.fill = fill;
                it.backface = (frontFace != null) ? frontFace : baseColor;
                it.visible = visible;
            });

            frontBase = group.frontBase;

            group.rearBase = group.frontBase.copy((it) =>
            {
                it.translate(z: -baseZ);
                it.rotate(y: 0f);
                it.backface = baseColor;
            });
            rearBase = group.rearBase;

            this.group = group;
        }

        public override void render(Renderer renderer)
        {
            //base.render(renderer);
        }

        public override Cylinder Copy()
        {
            return Copy(new Cylinder());
        }

        protected override Cylinder Copy(Anchor shape)
        {
            return (base.Copy(shape) as Cylinder).Also((it) =>
            {
                it.diameter = diameter;
                it.length = length;
                it.frontFace = frontFace;
            });
        }
    }
}