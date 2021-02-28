using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLASTIK
{
    public class NPCEntity : LivingEntity
    {
        public NPCEntity(ushort spriteIndex, double x = 0, double y = 0) : base(spriteIndex, x, y)
        {
        }
    }
}
