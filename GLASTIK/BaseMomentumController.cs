using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLASTIK
{
    public abstract class BaseMomentumController : BaseController
    {
        public double XVelocity { get; set; }
        public double YVelocity { get; set; }

        public override void Tick()
        {
            XVelocity *= 0.75;
            YVelocity *= 0.75;

            if (Math.Abs(XVelocity) < 0.1) XVelocity = 0.0;
            if (Math.Abs(YVelocity) < 0.1) YVelocity = 0.0;

            xOffset = XVelocity;
            yOffset = YVelocity;

            base.Tick();
        }
    }
}
