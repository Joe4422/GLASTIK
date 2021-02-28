using System.Collections.Generic;
using System.Linq;

namespace GLASTIK
{
    public abstract class BaseBinding
    {
        public string Name { get; }

        public BaseBinding(string name)
        {
            Name = name;
        }

        public abstract void Update();
    }

    public class ButtonBinding : BaseBinding
    {
        protected bool lastPressed = false;
        protected bool nowPressed = false;

        public List<Inputs.BaseInput[]> Inputs { get; }

        public ButtonBinding(string name, params Inputs.BaseInput[][] inputs) : base(name)
        {
            Inputs = inputs.ToList();
        }

        public ButtonBinding(string name, params Inputs.BaseInput[] inputs) : base(name)
        {
            Inputs = new();

            foreach (var input in inputs)
            {
                Inputs.Add(new Inputs.BaseInput[] { input });
            }
        }

        public bool Pressed { get; protected set; } = false;

        public override void Update()
        {
            nowPressed = true;

            if (Inputs.Count == 0) nowPressed = false;

            foreach (var combo in Inputs)
            {
                nowPressed = true;

                foreach (var input in combo)
                {
                    input.Update();

                    if (input is Inputs.BaseButtonInput)
                    {
                        Inputs.BaseButtonInput i = input as Inputs.BaseButtonInput;

                        if (i.Pressed == false) nowPressed = false;
                        continue;
                    }
                    else if (input is Inputs.BaseAxisInput)
                    {
                        Inputs.BaseAxisInput i = input as Inputs.BaseAxisInput;

                        if (i.Magnitude < 0.01) nowPressed = false;
                        continue;
                    }
                }

                if (nowPressed == true) break;
            }

            if (nowPressed == true && lastPressed == false)
            {
                Pressed = true;
            }
            else
            {
                Pressed = false;
            }

            lastPressed = nowPressed;
        }
    }

    public class AxisBinding : BaseBinding
    {
        public List<Inputs.BaseInput> Inputs { get; }

        public AxisBinding(string name, params Inputs.BaseInput[] inputs) : base(name)
        {
            Inputs = inputs.ToList();
        }

        public double Magnitude { get; protected set; } = 0.0;

        public double Deadzone { get; set; } = 0.01;

        public override void Update()
        {
            Magnitude = 0.0;

            foreach (var input in Inputs)
            {
                input.Update();

                if (input is Inputs.BaseButtonInput)
                {
                    Inputs.BaseButtonInput i = input as Inputs.BaseButtonInput;

                    if (i.Pressed)
                    {
                        Magnitude = 1.0;
                        break;
                    }
                }
                else if (input is Inputs.BaseAxisInput)
                {
                    Inputs.BaseAxisInput i = input as Inputs.BaseAxisInput;

                    if (i.Magnitude > Deadzone)
                    {
                        Magnitude = i.Magnitude;
                        break;
                    }
                }
            }
        }
    }
}
