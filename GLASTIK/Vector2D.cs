using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLASTIK
{
    public class Vector2D
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Magnitude
        {
            get
            {
                return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
            }
        }

        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vector2D() : this(0.0, 0.0)
        {
        }

        public Vector2D ToUnitVector()
        {
            return new Vector2D(X / Magnitude, Y / Magnitude);
        }
    }
}
