using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLASTIK
{
    public interface IOwned
    {
        public BaseEntity Owner { get; set; }
    }
}
