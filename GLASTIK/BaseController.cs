using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLASTIK
{
    public abstract class BaseController : ITickable, IOwned
    {
        public static BindingManager Input { get; set; }

        public BaseEntity Owner { get; set; }

        public double Speed { get; set; } = 1.0;

        protected double xOffset = 0.0;
        protected double yOffset = 0.0;

        public virtual void Tick()
        {
            if (xOffset != 0.0) Owner.Position.X += xOffset;
            if (yOffset != 0.0) Owner.Position.Y += yOffset;

            xOffset = 0.0;
            yOffset = 0.0;
        }
    }
}
