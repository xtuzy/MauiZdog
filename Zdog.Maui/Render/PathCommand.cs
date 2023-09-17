using Zdog.Maui.Extensions;

namespace Zdog.Maui.Render
{
    public interface PathCommand
    {
        Vector endRenderPoint { get; }
        void reset();
        void transform(Vector translation, Vector rotate, Vector scale);
        void render(Renderer renderer);
        Vector point(int index = 0);
        Vector renderPoint(int index = 0);
        void previous(Vector? previousPoint) { }
        PathCommand clone();
    }

    public partial class Move : PathCommand
    {
        readonly Vector _point;
        readonly Vector _renderPoint;
        public Move(Vector point)
        {
            _point = point;
            _renderPoint = point.copy();
            endRenderPoint = _renderPoint;
        }

        public Vector endRenderPoint { get; }

        public void reset()
        {
            _renderPoint.set(_point);
        }

        public void transform(Vector translation, Vector rotate, Vector scale)
        {
            _renderPoint.transform(translation, rotate, scale);
        }

        public void render(Renderer renderer)
        {
            renderer.move(_renderPoint);
        }

        public Vector point(int index = 0)
        {
            return _point;
        }

        public Vector renderPoint(int index = 0)
        {
            return _renderPoint;
        }

        public PathCommand clone()
        {
            return new Move(this.endRenderPoint);
        }
    }

    public partial class Line : PathCommand
    {
        readonly Vector _point;
        readonly Vector _renderPoint;
        public Line(Vector point)
        {
            _point = point;
            _renderPoint = point.copy();
            endRenderPoint = _renderPoint;
        }

        public Vector endRenderPoint { get; private set; }

        public Vector point(int index = 0)
        {
            return _point;
        }

        public void render(Renderer renderer)
        {
            renderer.line(_renderPoint);
        }

        public Vector renderPoint(int index = 0)
        {
            return _renderPoint;
        }

        public void reset()
        {
            _renderPoint.set(_point);
        }

        public void transform(Vector translation, Vector rotate, Vector scale)
        {
            _renderPoint.transform(translation, rotate, scale);
        }

        public PathCommand clone()
        {
            return new Line(this.endRenderPoint);
        }
    }

    public partial class Bezier : PathCommand
    {
        readonly Vector[] _points;
        readonly Vector[] _renderPoints;
        public Bezier(Vector[] points)
        {
            _points = points;
            _renderPoints = points.map<Vector, Vector>((it) => { return it.copy(); });
            endRenderPoint = _renderPoints.Last();
        }

        public Vector endRenderPoint { get; }

        public void reset()
        {
            _renderPoints.Each((index, vector)
                => vector.set(_points[index])
            );
        }

        public void transform(Vector translation, Vector rotate, Vector scale)
        {
            _renderPoints.Each((index, it) =>
                it.transform(translation, rotate, scale));
        }

        public void render(Renderer renderer)
        {
            renderer.bezier(_renderPoints[0], _renderPoints[1], _renderPoints[2]);
        }

        public Vector point(int index = 0)
        {
            return _points[index];
        }

        public Vector renderPoint(int index = 0)
        {
            return _renderPoints[index];
        }

        public PathCommand clone()
        {
            return new Line(this.endRenderPoint);
        }

        public override int GetHashCode()
        {
            return _points.GetHashCode();
        }
    }

    public partial class Arc : PathCommand
    {
        readonly Vector[] _points;
        Vector? _previous;
        readonly Vector[] _renderPoints;

        Vector[] _controlPoints = new Vector[2] { new Vector(), new Vector() };
        public Arc(Vector[] points, Vector? previous = null)
        {
            _points = points;
            _previous = previous;
            _renderPoints = points.map<Vector, Vector>((it) => { return it.copy(); });
            endRenderPoint = _renderPoints.Last();
        }

        public Vector endRenderPoint { get; }

        public void reset()
        {
            _renderPoints.Each((index, vector)
                => vector.set(_points[index])
            );
        }

        public void transform(Vector translation, Vector rotate, Vector scale)
        {
            _renderPoints.Each((index, it) =>
                it.transform(translation, rotate, scale));
        }

        public void render(Renderer renderer)
        {
            var prev = _previous;
            var corner = _renderPoints[0];
            var end = _renderPoints[1];
            var cp0 = _controlPoints[0];
            var cp1 = _controlPoints[1];

            cp0.set(prev).lerp(corner, Utils.arcHandleLength);
            cp1.set(end).lerp(corner, Utils.arcHandleLength);
            renderer.bezier(cp0, cp1, end);
        }

        public Vector point(int index = 0)
        {
            return _points[index];
        }

        public Vector renderPoint(int index = 0)
        {
            return _renderPoints[index];
        }

        public void previous(Vector? previousPoint)
        {
            _previous = previousPoint;
        }

        public PathCommand clone()
        {
            return new Arc(this._renderPoints, _previous);
        }

        public override int GetHashCode()
        {
            var result = _points.GetHashCode();
            result = 31 * result + (_previous?.GetHashCode() ?? 0);
            return result;
        }
    }
}

