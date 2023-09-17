using Zdog.Maui.Render;
using SkiaSharp;
using SkiaSharp.Views.Maui.Controls;
using Line = Zdog.Maui.Render.Line;

namespace Zdog.Maui.Demo
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            List<Anchor> shapeList = new List<Anchor>()
            {
                GetDot(),
                GetLine(),
                GetZShape(),
                Get3DShape(),
                GetArc(),
                GetBezier(),
                GetClosed(),
                GetUnclosed(),
                GetEllipse(),
                GetHemisphere(),
                GetCone(),
                GetCylinder1(),
                GetCylinder2(),
                GetBox(),
                GetZfighting(),
            };
            CreateView(shapeList);
        }

        void CreateView(List<Anchor> shapeList)
        {
            foreach (var shape in shapeList)
            {
                var skView = new SKCanvasView() { HeightRequest = 300, Margin = new Thickness(0, 10, 0, 0) };
                var bar = new XYZChangeBar();
                var drawable = new ZdogDrawable();
                shape.addTo = drawable.illo;
                drawable.illo.color = "#eeeeee";
                drawable.addChild(shape);
                skView.PaintSurface += (sender, e) =>
                {
                    e.Surface.Canvas.Clear(SKColors.White);
                    drawable.onBoundsChange(new SKRect(0, 0, e.Info.Width, e.Info.Height));
                    drawable.draw(e.Surface.Canvas);
                };
                bar.SKCanvasView = skView;
                bar.ZdogDrawable = drawable;
                bar.Title = shape.DEBUGID;
                layout.Add(skView);
                layout.Add(bar);
            }
        }

        Anchor GetDot()
        {
            return new Shape()
            {
                stroke = 20,
                color = "#636",
                DEBUGID = "Dot"
            }.setup();
        }

        Anchor GetLine()
        {
            return new Shape()
            {
                stroke = 20,
                color = "#636",
                DEBUGID = "Line"
            }.setup((it) =>
            {
                it.Path(new Move(new Vector(-40, 0, 0)),
                    new Line(new Vector(40, 0, 0)));
            });
        }

        Anchor GetZShape()
        {
            return new Shape()
            {
                closed = false,
                stroke = 20,
                color = "#636",
                DEBUGID = "ZShape"
            }.setup((it) =>
            {
                it.Path(
                    new Move(new Vector(-32, 0, -40)),
                    new Line(new Vector(32, 0, -40)),
                    new Line(new Vector(-32, 0, 40)),
                    new Line(new Vector(32, 0, 40))
                    );
            });
        }

        Anchor Get3DShape()
        {
            return new Shape()
            {
                closed = false,
                stroke = 20,
                color = "#636",
                DEBUGID = "3DShape"
            }.setup((it) =>
            {
                it.Path(new Move(new Vector(-32, -40, 40)),
                    new Line(new Vector(32, -40, 0)),
                    new Line(new Vector(32, 40, 40)),
                    new Line(new Vector(32, 40, -40)));
            });
        }

        Anchor GetArc()
        {
            return new Shape()
            {
                closed = false,
                stroke = 20,
                color = "#636",
                DEBUGID = "Arc"
            }.setup((it) =>
            {
                it.Path(new Move(new Vector(-60, -60, 0)),
                    new Arc(new[] { new Vector(20, -60, 0), new Vector(20, 20, 0) }),
                    new Arc(new[] { new Vector(20, 60, 0), new Vector(60, 60, 0) })
                    );
            });
        }

        Anchor GetBezier()
        {
            return new Shape()
            {
                closed = false,
                stroke = 20,
                color = "#636",
                DEBUGID = "Bezier"
            }.setup((it) =>
            {
                it.Path(new Move(new Vector(-60, -60, 0)),
                    new Bezier(new[]{
                        new Vector(20 ,-60, 0),
                        new Vector(20 ,60, 0),
                        new Vector(60 ,60, 0)
                    }));
            });
        }

        Anchor GetClosed()
        {
            return new Shape()
            {
                closed = true,
                stroke = 20,
                color = "#636",
                DEBUGID = "Closed"
            }.setup((it) =>
            {
                it.Path(new Move(new Vector(0, -40, 0)),
                    new Line(new Vector(40, 40, 0)),
                    new Line(new Vector(-40, 40, 0)));
            });
        }

        Anchor GetUnclosed()
        {
            return new Shape()
            {
                closed = false,
                stroke = 20,
                color = "#636",
                DEBUGID = "Unclosed"
            }.setup((it) =>
            {
                it.Path(new Move(new Vector(0, -40, 0)),
                    new Line(new Vector(40, 40, 0)),
                    new Line(new Vector(-40, 40, 0)));
            });
        }

        Anchor GetEllipse()
        {
            return new Ellipse()
            {
                diameter = 120,
                width = 200,
                height = 100,
                stroke = 0,
                color = "#C25",
                backface = "#EA0",
                DEBUGID = "Ellipse"
            }.setup();
        }

        Anchor GetHemisphere()
        {
            return new Hemisphere()
            {
                diameter = 120,
                stroke = 0,
                color = "#C25",
                backface = "#EA0",
                DEBUGID = "Hemisphere"
            }.setup();
        }

        Anchor GetCone()
        {
            return new Cone()
            {
                diameter = 70,
                length = 90,
                stroke = 0,
                color = "#636",
                backface = "#C25",
                DEBUGID = "Cone"
            }.setup();
        }

        Anchor GetCylinder1()
        {
            return new Cylinder()
            {
                diameter = 80,
                length = 120,
                stroke = 0,
                color = "#C25",
                backface = "#E62",
                DEBUGID = "Cylinder1"
            }.setup();
        }

        Anchor GetCylinder2()
        {
            return new Cylinder()
            {
                diameter = 80,
                length = 120,
                stroke = 0,
                color = "#C25",
                frontFace = "#EA0",
                backface = "#636",
                DEBUGID = "Cylinder2"
            }.setup();
        }

        Anchor GetBox()
        {
            return new Box()
            {
                width = 120,
                height = 120,
                depth = 80,
                stroke = 0,
                color = "#636",
                frontFace = "#636",
                leftFace = "#EA0",
                topFace = "#ED0",
                rightFace = "#E62",
                bottomFace = "#8fce00",
                rearFace = "#cc0000",
                fill = true,
                DEBUGID = "Box"
            }.setup();
        }

        Anchor GetZfighting()
        {
            var distance = 40;

            var group = new Shape()
            {
                DEBUGID = "Zfighting"
            }.setup((shape) =>
            {
                var dot = new Shape()
                {
                    stroke = 80,
                    color = "#636",
                };
                shape.addChilds(
                    //dot,//why error
                    dot.copy((it) =>
                    {
                        it.translate(y: -distance);
                    }),
                    dot.copy((it) =>
                    {
                        it.translate(x: -distance);
                        it.color = "#EA0";
                    }),
                    dot.copy((it) =>
                    {
                        it.translate(z: distance);
                        it.color = "#C25";
                    }),
                    dot.copy((it) =>
                    {
                        it.translate(x: distance);
                        it.color = "#E62";
                    }),
                    dot.copy((it) =>
                    {
                        it.translate(z: -distance);
                        it.color = "#C25";
                    }),
                    dot.copy((it) =>
                    {
                        it.translate(y: distance);
                    })
                 );
            });
            return group;
        }
    }
}