using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLASTIK
{
    public abstract class BasePlayer : LivingEntity
    {
        public static BindingManager Input { get; set; }
        public BaseCamera Camera { get; set; }

        public BasePlayer(ushort spriteIndex, double x = 0, double y = 0) : base(spriteIndex, x, y)
        {
        }

        public override void Tick()
        {
            base.Tick();

            if (Camera != null)
            {
                Camera.Tick();
            }

            if (Input.Respawn.Pressed)
            {
                Respawn();
            }
        }

        public virtual void Respawn()
        {
            Health = MaxHealth;

            if (Level != null)
            {
                Point2D spawn = new(Level.Map.Spawns[new Random().Next(Level.Map.Spawns.Count)]);

                Position = spawn;

                if (Controller is BaseMomentumController ctrl)
                {
                    ctrl.XVelocity = 0.0;
                    ctrl.YVelocity = 0.0;
                }
            }
        }
    }
}
