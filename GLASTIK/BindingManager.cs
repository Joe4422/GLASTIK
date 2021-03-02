using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace GLASTIK
{
    public class BindingManager : ITickable
    {
        protected List<BaseBinding> bindings = new()
        {
            new AxisBinding("Up", Inputs.GetInput(Keys.W), Inputs.GetInput(Keys.Up), Inputs.GetInput(Inputs.GamepadAxis.RightYMinus)),
            new AxisBinding("Down", Inputs.GetInput(Keys.S), Inputs.GetInput(Keys.Down), Inputs.GetInput(Inputs.GamepadAxis.RightYPlus)),
            new AxisBinding("Left", Inputs.GetInput(Keys.A), Inputs.GetInput(Keys.Left), Inputs.GetInput(Inputs.GamepadAxis.RightXMinus)),
            new AxisBinding("Right", Inputs.GetInput(Keys.D), Inputs.GetInput(Keys.Right), Inputs.GetInput(Inputs.GamepadAxis.RightXPlus)),
            new ButtonBinding("Menu", Inputs.GetInput(Keys.Escape), Inputs.GetInput(Buttons.Start)),
            new ButtonBinding("Respawn", Inputs.GetInput(Keys.R), Inputs.GetInput(Buttons.Back)),
            new ButtonBinding("Show Console", Inputs.GetInput(Keys.OemTilde))
        };

        public AxisBinding Up => (AxisBinding)bindings[0];
        public AxisBinding Down => (AxisBinding)bindings[1];
        public AxisBinding Left => (AxisBinding)bindings[2];
        public AxisBinding Right => (AxisBinding)bindings[3];
        public ButtonBinding Menu => (ButtonBinding)bindings[4];
        public ButtonBinding Respawn => (ButtonBinding)bindings[5];
        public ButtonBinding ShowConsole => (ButtonBinding)bindings[6];

        public void Tick()
        {
            foreach (var binding in bindings)
            {
                binding.Update();
            }
        }
    }
}
