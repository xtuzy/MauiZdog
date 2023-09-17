using Zdog.Maui.Extensions;
using Vector3 = Zdog.Maui.Render.Vector;

namespace Zdog.Maui.Render
{
    public class Anchor : IProperty
    {
        public string DEBUGID;

        static readonly Vector3 onePoint = new Vector3(1.0f, 1.0f, 1.0f);

        public readonly Vector3 _translate = new Vector3();
        public readonly Vector3 _rotate = new Vector3();

        public readonly Vector3 _scale = onePoint;

        public virtual string color
        {
            set
            {
                if(!string.IsNullOrEmpty(value))
                    colour = Color.Parse(value);
            }
            get => colour.ToArgbHex();
        }

        public float _alpha = 1f;

        public Anchor? addTo;

        public Color colour = Color.Parse("#333");

        readonly Vector3 _renderOrigin = new Vector3();
        public Vector3 renderOrigin => _renderOrigin;

        public float sortValue = 0;

        public readonly List<Anchor> children = new List<Anchor>();

        protected readonly Vector3 origin;

        List<Anchor>? mflatGraph;
        protected List<Anchor>? _flatGraph
        {
            get
            {
                if (mflatGraph == null)
                    updateFlatGraph();
                return mflatGraph;
            }

            set { mflatGraph = value; }
        }

        protected int renderAlpha = 255;

        public enum AnimatorType
        {
            Custom,
            Translate,
            Scale,
            Rotate,
            Alpha,
            Color
        }

        Dictionary<AnimatorType, ValueAnimator> _animators;
        Dictionary<AnimatorType, ValueAnimator> animators
        {
            get
            {
                if(_animators == null)
                    _animators = new Dictionary<AnimatorType, ValueAnimator>();
                return _animators;
            }
        }

        public override void onCreate()
        {
            addTo?.addChild(this);
        }

        public virtual void addChild(Anchor shape)
        {
            if (children.IndexOf(shape) != -1)
            {
                return;
            }
            shape.remove();
            shape.addTo = this;
            this.children.Add(shape);
        }

        public void addChilds(params Anchor[] shapes)
        {
            foreach (var shape in shapes)
                addChild(shape);
        }

        protected void removeChild(Anchor shape)
        {
            children.Remove(shape);
        }

        public void remove()
        {
            addTo?.removeChild(this);
            clearAnimators();
        }

        void update()
        {
            // update self
            reset();
            // update children
            children.ForEach((it) =>
            {
                it.update();
                if (_alpha < it._alpha)
                {
                    it.renderAlpha = renderAlpha;
                }
                else
                {
                    it.renderAlpha = ((int)(it._alpha * 255)).Bound(0, 255);
                }
            });
            transform(_translate, _rotate, _scale);
        }

        public virtual void reset()
        {
            renderOrigin.set(origin);
            renderAlpha = ((int)(_alpha * 255)).Bound(0, 255);
        }

        public void translate(float x = 0f, float y = 0f, float z = 0f)
        {
            this._translate.set(x, y, z);
        }

        protected void translate(Action<Vector> block)
        {
            block?.Invoke(this._translate);
        }

        public void rotate(float x = 0f, float y = 0f, float z = 0f) =>
            this._rotate.set(x, y, z);

        public void rotate(Action<Vector> block)
        {
            block?.Invoke(this._rotate);
        }

        public Vector scale(float scale) {
            return this._scale.set(scale, scale, scale);
        }

        public void scale(Action<Vector> block)
        {
            block?.Invoke(this._scale);
        }

        public virtual void transform(Vector3 translation, Vector3 rotation, Vector3 scale)
        {
            renderOrigin.transform(translation, rotation, scale);
            children.ForEach((it) =>
            {
                it.transform(translation, rotation, scale);
            });
        }

        protected virtual void updateGraph()
        {
            update();
            updateFlatGraph();
            _flatGraph?.ForEach((it) =>
            {
                it.updateSortValue();
            });
            sortFlatGraph();
        }

        protected void sortFlatGraph()
        {
            _flatGraph = _flatGraph?.OrderBy((a) =>
            {
                return a.sortValue;
            }).ToList();
        }

        protected virtual void updateFlatGraph()
        {
            _flatGraph = flatGraph();
        }

        protected virtual List<Anchor> flatGraph()
        {
            return addChildFlatGraph(new List<Anchor>() { this });
        }

        protected List<Anchor> addChildFlatGraph(List<Anchor> flatGraph)
        {
            children.ForEach((it) =>
            {
                flatGraph.AddRange(it.flatGraph());
            });
            return flatGraph;
        }

        public virtual void updateSortValue()
        {
            sortValue = renderOrigin.z;
        }

        public virtual void render(Renderer renderer) { }

        public void renderGraph(Renderer renderer)
        {
            _flatGraph?.ForEach((it) =>
            {
                it.render(renderer);
            });
        }

        public virtual Anchor Copy() => Copy(new Anchor());
        protected virtual Anchor Copy(Anchor shape)
        {
            shape._translate.set(_translate);
            shape._rotate.set(_rotate);
            shape._scale.set(_scale);
            shape.colour = colour;
            shape._alpha = _alpha;
            shape.addTo = addTo;
            return shape;
        }

        public virtual T copyGraph<T>(Action<T> block) where T : Anchor
        {
            var clone = (Copy() as T).set(block);
            children.ForEach((it) =>
            {
                it.copyGraph<T>((t) =>
                {
                    addTo = clone;
                });
            });
            return clone;
        }

        public void normalizeRotate()
        {
            var tau = Utils.TAU;
            _rotate.x = Utils.modulo(_rotate.x, tau);
            _rotate.y = Utils.modulo(_rotate.y, tau);
            _rotate.z = Utils.modulo(_rotate.x, tau);
        }

        public void clearAnimators()
        {
            foreach (var animator in animators)
            {
                if (animator.Value != null)
                {
                    animator.Value.Pause();
                    animator.Value.Cancel();
                }
            }
            animators.Clear();
            children.ForEach((it) => { it.clearAnimators(); });
        }

        public void addAnimator(AnimatorType type, ValueAnimator animator)
        {
            var old = animators[type];
            if (old != null)
            {
                old.Pause();
                old.Cancel();
            }
            animators[type] = animator;
        }

        public ValueAnimator? getAnimator(AnimatorType type) => animators[type];
    }
}
