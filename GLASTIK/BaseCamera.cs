using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLASTIK
{
    public abstract class BaseCamera : BaseEntity
    {
        public BaseCamera(double x = 0.0, double y = 0.0) : base(x, y)
        {
        }
    }
}
