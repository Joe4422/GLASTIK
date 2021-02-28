using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GLASTIK
{
    public abstract class BaseEntity : ITickable, IOwned
    {
        [AttributeUsage(AttributeTargets.Property)]
        public class ConsoleModifiableAttribute : Attribute { }

        [ConsoleModifiable]
        public string Name { get; set; } = "";
        [ConsoleModifiable]
        public Point2D Position { get; set; } = new();
        public BaseController Controller { get; set; } = null;
        [ConsoleModifiable]
        public BaseEntity Owner { get; set; } = null;

        protected BaseEntity parent = null;
        [ConsoleModifiable]
        public BaseEntity Parent
        {
            get => parent;
            set
            {
                if (value != null)
                {
                    relativeToParent = Position.VectorTowards(value.Position);
                    parent = value;
                }
                else
                {
                    relativeToParent = null;
                    parent = null;
                }
            }
        }
        protected Vector2D relativeToParent = null;
        public Level Level { get; set; } = null;

        [ConsoleModifiable]
        public double Speed
        {
            get => Controller == null ? 0.0 : Controller.Speed;
            set
            {
                if (Controller != null) Controller.Speed = value;
            }
        }

        public BaseEntity(double x = 0.0, double y = 0.0)
        {
            Position = new(x, y);
        }

        public virtual void Tick()
        {
            if (Parent != null)
            {
                Position.X = Parent.Position.X - relativeToParent.X;
                Position.Y = Parent.Position.Y - relativeToParent.Y;
            }
            else if (Controller != null)
            {
                if (Controller.Owner != this) Controller.Owner = this;

                Controller.Tick();
            }
        }

        public override string ToString() => $"{(Name == "" ? "Unnamed" : Name)} (instance of {GetType()})";
    }
}
