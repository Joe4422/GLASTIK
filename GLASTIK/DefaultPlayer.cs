using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLASTIK
{
    public class DefaultPlayer : BasePlayer
    {
        public DefaultPlayer(ushort spriteIndex, double x = 0, double y = 0) : base(spriteIndex, x, y)
        {
            Controller = new WalkController();
            Camera = new FollowCamera
            {
                Parent = this
            };

            (Controller as WalkController).Speed = 4.0;
        }
    }
}
