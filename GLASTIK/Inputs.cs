using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLASTIK
{
    public static class Inputs
    {
        public enum MouseButton
        {
            Left,
            Right,
            Middle,
            Forward,
            Backward
        }

        public enum GamepadAxis
        {
            LeftXPlus,
            LeftXMinus,
            LeftYPlus,
            LeftYMinus,
            RightXPlus,
            RightXMinus,
            RightYPlus,
            RightYMinus,
            LeftTrigger,
            RightTrigger
        }

        private static readonly List<BaseInput> inputs = new();

        public static BaseInput GetInput(Keys key)
        {
            BaseInput input = inputs.Where(x => x is KeyboardInput).FirstOrDefault(x => (x as KeyboardInput).Key == key);
            if (input == null)
            {
                KeyboardInput i = new(key);
                inputs.Add(i);
                return i;
            }
            else
            {
                return input;
            }
        }

        public static BaseInput GetInput(Buttons button)
        {
            BaseInput input = inputs.Where(x => x is GamepadButtonInput).FirstOrDefault(x => (x as GamepadButtonInput).Button == button);
            if (input == null)
            {
                GamepadButtonInput i = new(button);
                inputs.Add(i);
                return i;
            }
            else
            {
                return input;
            }
        }

        public static BaseInput GetInput(MouseButton button)
        {
            BaseInput input = inputs.Where(x => x is MouseButtonInput).FirstOrDefault(x => (x as MouseButtonInput).Button == button);
            if (input == null)
            {
                MouseButtonInput i = new(button);
                inputs.Add(i);
                return i;
            }
            else
            {
                return input;
            }
        }

        public static BaseInput GetInput(GamepadAxis axis)
        {
            BaseInput input = inputs.Where(x => x is GamepadAxisInput).FirstOrDefault(x => (x as GamepadAxisInput).Axis == axis);
            if (input == null)
            {
                GamepadAxisInput i = new(axis);
                inputs.Add(i);
                return i;
            }
            else
            {
                return input;
            }
        }


        public abstract class BaseInput
        {
            public abstract void Update();
        }

        public abstract class BaseButtonInput : BaseInput
        {
            public bool Pressed { get; protected set; }
        }

        public abstract class BaseAxisInput : BaseInput
        {
            public double Magnitude { get; protected set; }
        }

        protected class KeyboardInput : BaseButtonInput
        {
            public Keys Key { get; }

            public KeyboardInput(Keys key)
            {
                Key = key;
            }

            public override void Update()
            {
                Pressed = Keyboard.GetState().IsKeyDown(Key);
            }
        }

        protected class GamepadButtonInput : BaseButtonInput
        {
            public Buttons Button { get; }

            public GamepadButtonInput(Buttons button)
            {
                Button = button;
            }

            public override void Update()
            {
                Pressed = GamePad.GetState(PlayerIndex.One).IsButtonDown(Button);
            }
        }

        protected class MouseButtonInput : BaseButtonInput
        {
            public MouseButton Button { get; }

            public MouseButtonInput(MouseButton button)
            {
                Button = button;
            }

            public override void Update()
            {
                Pressed = Button switch
                {
                    MouseButton.Left => Mouse.GetState().LeftButton == ButtonState.Pressed,
                    MouseButton.Right => Mouse.GetState().RightButton == ButtonState.Pressed,
                    MouseButton.Middle => Mouse.GetState().MiddleButton == ButtonState.Pressed,
                    MouseButton.Forward => Mouse.GetState().XButton1 == ButtonState.Pressed,
                    MouseButton.Backward => Mouse.GetState().XButton2 == ButtonState.Pressed,
                    _ => false
                };
            }
        }

        protected class GamepadAxisInput : BaseAxisInput
        {
            public GamepadAxis Axis { get; }

            public GamepadAxisInput(GamepadAxis axis)
            {
                Axis = axis;
            }

            public override void Update()
            {
                Magnitude = 0.0;
                GamePadState state = GamePad.GetState(PlayerIndex.One);

                switch (Axis)
                {
                    case GamepadAxis.LeftXPlus:
                        {
                            double x = state.ThumbSticks.Left.X;
                            if (x < 0.0) Magnitude = 0.0;
                            else Magnitude = x;
                        }
                        break;
                    case GamepadAxis.LeftXMinus:
                        {
                            double x = state.ThumbSticks.Left.X;
                            if (x > 0.0) Magnitude = 0.0;
                            else Magnitude = -x;
                        }
                        break;
                    case GamepadAxis.LeftYPlus:
                        {
                            double y = state.ThumbSticks.Left.Y;
                            if (y > 0.0) Magnitude = 0.0;
                            else Magnitude = -y;
                        }
                        break;
                    case GamepadAxis.LeftYMinus:
                        {
                            double y = state.ThumbSticks.Left.Y;
                            if (y < 0.0) Magnitude = 0.0;
                            else Magnitude = y;
                        }
                        break;
                    case GamepadAxis.RightXPlus:
                        {
                            double x = state.ThumbSticks.Left.X;
                            if (x < 0.0) Magnitude = 0.0;
                            else Magnitude = x;
                        }
                        break;
                    case GamepadAxis.RightXMinus:
                        {
                            double x = state.ThumbSticks.Left.X;
                            if (x > 0.0) Magnitude = 0.0;
                            else Magnitude = -x;
                        }
                        break;
                    case GamepadAxis.RightYPlus:
                        {
                            double y = state.ThumbSticks.Left.Y;
                            if (y > 0.0) Magnitude = 0.0;
                            else Magnitude = -y;
                        }
                        break;
                    case GamepadAxis.RightYMinus:
                        {
                            double y = state.ThumbSticks.Left.Y;
                            if (y < 0.0) Magnitude = 0.0;
                            else Magnitude = y;
                        }
                        break;
                    case GamepadAxis.LeftTrigger:
                        Magnitude = state.Triggers.Left;
                        break;
                    case GamepadAxis.RightTrigger:
                        Magnitude = state.Triggers.Right;
                        break;
                }
            }
        }
    }

}
