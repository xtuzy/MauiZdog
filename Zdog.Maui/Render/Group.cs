using Zdog.Maui.Extensions;

namespace Zdog.Maui.Render
{
    public class Group : Anchor
    {
        public bool updateSort = false;
        public bool visible = true;
        Segment segment;

        public override void updateSortValue()
        {
            sortValue = _flatGraph?.let<List<Anchor>, float>((list) =>
            {
                var values = list.map<Anchor, float>((it) =>
                {
                    it.updateSortValue();

                    return it.sortValue;
                });
                return values.Sum() / list.Count;
            }) ?? renderOrigin.z;

            if (updateSort)
            {
                sortFlatGraph();
            }
        }

        public override void addChild(Anchor shape)
        {
            base.addChild(shape);
            updateFlatGraph();
        }

        public override void render(Renderer renderer)
        {
            if (!visible)
            {
                return;
            }

            var oldSegment = renderer.segment;
            if (oldSegment == null)
            {
                renderer.segment = segment;
            }
            base.renderGraph(renderer);
            renderer.segment = oldSegment;
        }

        protected override void updateFlatGraph()
        {
            _flatGraph = addChildFlatGraph(new List<Anchor>());
        }

        void updateSegment(float start, float end, Action<Path> block)
        {
            segment = (segment ?? new Segment()).Set(start, end, block);
        }

        protected override List<Anchor> flatGraph()
        {
            return Extensions.EnumerableExtensions.listOf<Anchor>(this);
        }

        public override Group Copy()
        {
            return Copy(new Group());
        }

        protected override Group Copy(Anchor shape)
        {
            return (base.Copy(shape) as Group).Also((it) =>
            {
                it.updateSort = updateSort;
                it.visible = visible;
            });
        }
    }
}