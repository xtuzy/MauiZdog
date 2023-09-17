namespace Zdog.Maui.Render
{
    public class Segment
    {
        public float Start { get; set; }
        public float End { get; set; }

        private Action<Path> onBegin;

        public Segment(float start = 0f, float end = 1f)
        {
            Start = start;
            End = end;
        }

        public bool Unbroken()
        {
            return Start == 0f && End == 1f;
        }

        public Segment Set(float start, float end, Action<Path> block = null)
        {
            Start = start;
            End = end;
            onBegin = block ?? onBegin;
            return this;
        }

        public Segment Reset()
        {
            Start = 0f;
            End = 1f;
            onBegin = null;
            return this;
        }

        public void Begin(Path path)
        {
            onBegin?.Invoke(path);
        }
    }
}