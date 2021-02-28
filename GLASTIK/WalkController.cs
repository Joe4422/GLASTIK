using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLASTIK
{
    public class WalkController : BaseMomentumController
    {
        public override void Tick()
        {
            double upMag = Input.Up.Magnitude;
            double downMag = Input.Down.Magnitude;
            double leftMag = Input.Left.Magnitude;
            double rightMag = Input.Right.Magnitude;

            double dx = rightMag - leftMag;
            double dy = downMag - upMag;

            if (Math.Abs(dx) > 0.1 || Math.Abs(dy) > 0.1)
            {
                Point2D next = new(dx, dy);
                Vector2D nextVec = Point2D.Origin.VectorTowards(next);

                if (nextVec.Magnitude > 1.0)
                {
                    nextVec = nextVec.ToUnitVector();
                }

                XVelocity = nextVec.X * Speed;
                YVelocity = nextVec.Y * Speed;
            }

            base.Tick();
        }
    }
}
