using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLASTIK
{
    public class Point2D
    {
        public double X { get; set; }
        public double Y { get; set; }

        public static Point2D Origin { get; } = new Point2D(0, 0);

        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Point2D(Point2D point)
        {
            X = point.X;
            Y = point.Y;
        }

        public Point2D() : this(0.0, 0.0)
        {

        }

        public Vector2D VectorTowards(Point2D point)
        {
            double dx = point.X - X;
            double dy = point.Y - Y;

            return new(dx, dy);
        }

        public Vector2D UnitVectorTowards(Point2D point)
        {
            Vector2D vec = VectorTowards(point);

            return vec.ToUnitVector();
        }

        public override string ToString() => $"{{{X}, {Y}}}";
    }
}
