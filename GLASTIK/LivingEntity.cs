using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLASTIK
{
    public abstract class LivingEntity : SpriteEntity
    {
        public LivingEntity(ushort spriteIndex, double x = 0.0, double y = 0.0) : base(spriteIndex, x, y)
        {
        }

        [ConsoleModifiable]
        public int MaxHealth { get; set; } = 100;
        [ConsoleModifiable]
        public int Health { get; set; } = 100;
        public bool Alive => Health <= 0;
    }
}
