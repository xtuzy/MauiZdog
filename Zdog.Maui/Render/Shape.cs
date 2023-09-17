using Zdog.Maui.Extensions;
namespace Zdog.Maui.Render
{
    public class Shape : Anchor
    {
        public virtual float stroke { get; set; } = 1f;
        public virtual bool fill { get; set; } = false;
        public virtual bool visible { get; set; } = true;
        public bool closed { get; set; } = true;
        public Vector front { get; set; } = new Vector(z: 1f);
        private string _backface = String.Empty;
        public string backface
        {
            get { return _backface; }
            set
            {
                _backface = value;
                if (!String.IsNullOrEmpty(value))
                {
                    backfaceColor = Color.Parse(value).ToInt();
                }
            }
        }

        public Vector renderNormal { get; set; } = new Vector();
        public PathEffect effect { get; set; }
        public Shader shader { get; set; }
        public Segment segment { get; set; }
        public ShaderLayer layer { get; set; }

        private bool isFacingBack = false;
        private Vector _renderFront;
        private Vector renderFront
        {
            get
            {
                if (_renderFront == null)
                    _renderFront = new Vector(front);
                return _renderFront;
            }
        }
        public List<PathCommand> _path { get; private set; } = new List<PathCommand>();

        internal int backfaceColor { get; set; }

        private int renderColor
        {
            get
            {
                bool isBackFaceColor = !String.IsNullOrEmpty(backface) && isFacingBack;
                return isBackFaceColor ? backfaceColor : colour.ToInt();
            }
        }

        public override void onCreate()
        {
            base.onCreate();
            updatePath();
        }

        public List<PathCommand> Path(params PathCommand[] commands)
        {
            _path.Clear();
            foreach (var command in commands)
            {
                _path.Add(command);
            }
            return _path;
        }

        public List<PathCommand> Path(List<PathCommand> commands)
        {
            _path.Clear();
            foreach (var command in commands)
            {
                _path.Add(command);
            }
            return _path;
        }

        internal void updatePath()
        {
            SetPath();
            UpdatePathCommands();
        }

        protected virtual void SetPath()
        {

        }

        public void UpdatePathCommands()
        {
            Vector previousPoint = null;
            if (_path.Count == 0)
            {
                _path.Add(Utils.move());
            }
            else
            {
                var first = _path.First();
                if (!(first is Move))
                {
                    _path[0] = Utils.move(first.point());
                }
                foreach (var command in _path)
                {
                    command.previous(previousPoint);
                    previousPoint = command.endRenderPoint;
                }
            }
        }

        public void UpdateSegment(float start, float end, Action<Path> block = null)
        {
            segment = (segment ?? new Segment()).Set(start, end, block);
        }

        public override void reset()
        {
            base.reset();
            renderFront.set(front);
            _path.ForEach(it => it.reset());
        }

        public override void transform(Vector translation, Vector rotation, Vector scale)
        {
            renderOrigin.transform(translation, rotation, scale);
            renderFront.transform(translation, rotation, scale);
            renderNormal.set(renderOrigin).subtract(renderFront);

            _path.ForEach(it => it.transform(translation, rotation, scale));
            children.ForEach(it => it.transform(translation, rotation, scale));
        }

        public override void updateSortValue()
        {
            int pointCount = _path.Count;
            var firstPoint = _path[0].endRenderPoint;
            var lastPoint = _path.Last().endRenderPoint;
            bool isSelfClosing = pointCount > 2 && firstPoint.isSame(lastPoint);
            if (isSelfClosing)
            {
                pointCount -= 1;
            }

            float sortValueTotal = 0f;
            for (int i = 0; i < pointCount; i++)
            {
                sortValueTotal += _path[i].endRenderPoint.z;
            }
            sortValue = sortValueTotal / pointCount;
        }

        public override void render(Renderer renderer)
        {
            int length = _path.Count;
            if (!visible || length == 0)
            {
                return;
            }
            isFacingBack = renderNormal.z > 0;
            if (_backface == null && isFacingBack)
            {
                return;
            }

            bool isDot = length == 1;
            var oldSegment = renderer.segment;
            if (oldSegment == null)
            {
                renderer.segment = segment;
            }
            if (isDot)
            {
                renderDot(renderer);
            }
            else
            {
                RenderPath(renderer);
            }
            renderer.segment = oldSegment;
        }

        private void renderDot(Renderer renderer)
        {
            if (stroke == 0f)
            {
                return;
            }
            renderer.FillStyle = renderColor;
            var point = _path[0].endRenderPoint;
            renderer.begin();
            var radius = stroke / 2;
            renderer.move(point);
            renderer.circle(point.x, point.y, radius);
            renderer.Fill(renderAlpha, shader, layer);
        }

        private void RenderPath(Renderer renderer)
        {
            bool isTwoPoints = _path.Count == 2 && (_path[1] is Line);
            bool isClosed = !isTwoPoints && closed;
            var color = renderColor;
            renderer.renderPath(_path, isClosed);
            renderer.Stroke(stroke > 0f, color, stroke, renderAlpha, effect, shader, layer);
            renderer.Fill(fill, color, renderAlpha, shader);
            renderer.End();
        }

        public Path ToPath(Renderer renderer)
        {
            Path result = new Path();
            if (_path.Count > 0)
            {
                bool isTwoPoints = _path.Count == 2 && (_path[1] is Line);
                bool isClosed = !isTwoPoints && closed;
                renderer.renderToPath(result, _path, isClosed);
            }
            return result;
        }

        public override Shape Copy()
        {
            return Copy(new Shape());
        }

        protected override Shape Copy(Anchor shape)
        {
            return ((Shape)base.Copy(shape)).Also((it)=>
            {
                it.stroke = stroke;
                it.fill = fill;
                it.closed = closed;
                it.visible = visible;
                it._path = _path.Select(it => it.clone()).ToList();
                it.front.set(front);
                it._backface = _backface;
            });
        }
    }
}