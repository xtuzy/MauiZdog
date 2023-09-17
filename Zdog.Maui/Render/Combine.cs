namespace Zdog.Maui.Render
{
    public class Combine : Group
    {
        public Combine()
        {
            updateSort = true;
        }

        public override void reset()
        {
            base.reset();
            children.ForEach((it) => { it.colour = colour; });
        }

        public override Combine Copy()
        {
            return Copy(new Combine());
        }

        protected override Combine Copy(Anchor shape)
        {
            return base.Copy(shape) as Combine;
        }
    }
}