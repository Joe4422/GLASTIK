using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLASTIK
{
    public abstract class SpriteEntity : BaseEntity
    {
        [ConsoleModifiable]
        public ushort SpriteIndex { get; set; }

        public SpriteEntity(ushort spriteIndex, double x = 0.0, double y = 0.0) : base(x, y)
        {
            SpriteIndex = spriteIndex;
        }
    }
}
