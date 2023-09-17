using Zdog.Maui.Extensions;
using FaceOptions = System.Action<Zdog.Maui.Render.BoxRect>;
namespace Zdog.Maui.Render
{
    public class Box : Anchor
    {
        private float _stroke = 1f;
        public float stroke
        {
            get { return _stroke; }
            set
            {
                _stroke = value;
                setProperty((_, rect) => rect.stroke = _stroke);
            }
        }

        private bool _fill = true;
        public bool fill
        {
            get { return _fill; }
            set
            {
                _fill = value;
                setProperty((_, rect) => rect.fill = _fill);
            }
        }

        public override string color
        {
            get { return colour.ToArgbHex(); }
            set
            {
                colour = Color.Parse(value);
                setProperty((face, rect) =>
                {
                    if (face == null) rect.colour = colour;
                });
            }
        }

        private bool _visible = true;
        public bool visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                setProperty((_, rect) => rect.visible = _visible);
            }
        }

        private Vector _front = new Vector(z: 1f);
        public Vector front
        {
            get { return _front; }
            set
            {
                _front = value;
                setProperty((_, rect) => rect.front = _front);
            }
        }

        private string _backface = String.Empty;
        public string backface
        {
            get { return _backface; }
            set
            {
                _backface = value;
                setProperty((_, rect) => rect.backface = _backface);
            }
        }

        public float width { get; set; } = 1f;

        public float height { get; set; } = 1f;

        public float depth { get; set; } = 1f;

        private FaceOptions frontFaceOptions => (it) =>
        {
            it.width = width;
            it.height = height;
            it.translate(0, 0, z: depth / 2);
        };

        private FaceOptions rearFaceOptions => (it) =>
        {
            it.width = width;
            it.height = height;
            it.translate(0, 0, z: -depth / 2);
            it.rotate(0, y: (float)(Utils.TAU / 2), 0);
        };

        private FaceOptions leftFaceOptions => (it) =>
        {
            it.width = depth;
            it.height = height;
            it.translate(x: -width / 2, 0, 0);
            it.rotate(0, y: (float)(-Utils.TAU / 4), 0);
        };

        private FaceOptions rightFaceOptions => (it) =>
        {
            it.width = depth;
            it.height = height;
            it.translate(x: width / 2, 0, 0);
            it.rotate(0, y: (float)(Utils.TAU / 4), 0);
        };

        private FaceOptions topFaceOptions => (it) =>
        {
            it.width = width;
            it.height = depth; 
            it.translate(0, y: -height / 2, 0);
            it.rotate(x: (float)(-Utils.TAU / 4), 0, 0);
        };

        private FaceOptions bottomFaceOptions => (it) =>
        {
            it.width = width;
            it.height = depth;
            it.translate(0, y: height / 2, 0);
            it.rotate(x: (float)(Utils.TAU / 4), 0, 0);
        };

        String? _frontFace = String.Empty;
        public String? frontFace
        {
            set
            {
                _frontFace = value;
                frontRect = initRect(frontRect, value, frontFaceOptions);
            }
            get { return _frontFace; }
        }
        BoxRect? frontRect;

        String? _rearFace = String.Empty;
        public String? rearFace
        {
            set
            {
                _rearFace = value;
                rearRect = initRect(rearRect, value, rearFaceOptions);
            }
            get { return _rearFace; }
        }
        BoxRect? rearRect;


        String? _leftFace = String.Empty;
        public String? leftFace
        {
            set
            {
                _leftFace = value;
                leftRect = initRect(leftRect, value, leftFaceOptions);
            }
            get { return _leftFace; }
        }
        BoxRect? leftRect;

        String? _rightFace = String.Empty;
        public String? rightFace
        {
            set
            {
                _rightFace = value;
                rightRect = initRect(rightRect, value, rightFaceOptions);
            }
            get { return _rightFace; }
        }
        BoxRect? rightRect;

        String? _topFace = String.Empty;
        public String? topFace
        {
            set
            {
                _topFace = value;
                topRect = initRect(topRect, value, topFaceOptions);
            }
            get { return _topFace; }
        }
        BoxRect? topRect;

        String? _bottomFace = String.Empty;
        public String? bottomFace
        {
            set
            {
                _bottomFace = value;
                bottomRect = initRect(bottomRect, value, bottomFaceOptions);
            }
            get { return _bottomFace; }
        }
        BoxRect? bottomRect;

        private BoxRect? initRect(BoxRect? face, String? value, Action<BoxRect> block)
        {
            var rect = face;
            if (value == null)
            {
                if (rect != null)
                {
                    removeChild(rect);
                }
                return null;
            }

            var colour = string.IsNullOrEmpty(value) ? this.colour : Color.Parse(value);

            rect = rect != null ? rect.Also((it) =>
            {
                block(it);
                it.colour = colour;
            }) : new BoxRect().setup((it) =>
                {
                    block(it);
                    it.colour = colour;
                });
            rect.updatePath();
            addChild(rect);

            return rect;
        }

        public override void onCreate()
        {
            initRect();
            base.onCreate();
            updatePath();
        }

        private void updatePath()
        {
            setProperty((face, rect) =>
            {
                rect.colour = string.IsNullOrEmpty(face) ? colour : Color.Parse(face);
                rect.stroke = stroke;
                rect.fill = fill;
                rect.backface = backface;
                rect.front = front;
                rect.visible = visible;
            });
        }

        private void initRect()
        {
            if (frontFace != null)
            {
                frontRect = frontRect ?? newRect(frontFaceOptions);
            }
            if (rearFace != null)
            {
                rearRect = rearRect ?? newRect(rearFaceOptions);
            }
            if (leftFace != null)
            {
                leftRect = leftRect ?? newRect(leftFaceOptions);
            }
            if (rightFace != null)
            {
                rightRect = rightRect ?? newRect(rightFaceOptions);
            }
            if (topFace != null)
            {
                topRect = topRect ?? newRect(topFaceOptions);
            }
            if (bottomFace != null)
            {
                bottomRect = bottomRect ?? newRect(bottomFaceOptions);
            }
        }

        private BoxRect newRect(FaceOptions options) =>
        new BoxRect().setup(options).Also((it) =>
        {
            it.updatePath();
            addChild(it);
        });

        private void setProperty(Action<String?, BoxRect> block)
        {
            frontRect?.let((it) => { block(frontFace, it); return it; });
            rearRect?.let((it) => {block(rearFace, it); return it; });
            leftRect?.let((it) =>{ block(leftFace, it); return it; });
            rightRect?.let((it) => {block(rightFace, it); return it; });
            topRect?.let((it) => {block(topFace, it); return it; });
            bottomRect?.let((it) => {block(bottomFace, it); return it; });
        }

        public override Box Copy()
        {
            return Copy(new Box());
        }

        protected override Box Copy(Anchor shape)
        {
            return (base.Copy(shape) as Box).Also((it) =>
            {
                it.stroke = stroke;
                it.fill = fill;
                it.color = color;
                it.visible = visible;
                it.front = front;
                it.backface = backface;
                it.width = width;
                it.height = height;
                it.depth = depth;

                it.frontFace = frontFace;
                it.rearFace = rearFace;
                it.leftFace = leftFace;
                it.rightFace = rightFace;
                it.topFace = topFace;
                it.bottomFace = bottomFace;
            });
        }
    }
}

