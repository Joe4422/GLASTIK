using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GLASTIK
{
    public class InputBinding
    {
        public enum MouseButton
        {
            Left,
            Right,
            Middle,
            Forward,
            Backward
        }

        public enum Axis
        {
            LeftXPlus,
            LeftXMinus,
            LeftYPlus,
            LeftYMinus,
            RightXPlus,
            RightXMinus,
            RightYPlus,
            RightYMinus
        }

        public string Name { get; }
        public List<List<Keys>> KeyboardKeys { get;  }
        public List<MouseButton> MouseButtons { get; }
        public List<Buttons> GamepadButtons { get; }
        public List<Axis> GamepadAxis { get; }

        public bool Pressed { get; private set; }
        public double Magnitude { get; private set; }

        public InputBinding(string name, List<List<Keys>> keyboardKeys = null, List<MouseButton> mouseButtons = null, List<Buttons> gamepadButtons = null, List<Axis> gamepadAxis = null)
        {
            Name = name;

            KeyboardKeys = keyboardKeys ?? new();
            MouseButtons = mouseButtons ?? new();
            GamepadButtons = gamepadButtons ?? new();
            GamepadAxis = gamepadAxis ?? new();
        }

        public InputBinding(string name, Keys? keyboardKey = null, MouseButton? mouseButton = null, Buttons? gamepadButton = null, Axis? gamepadAxis = null)
        {
            Name = name;

            KeyboardKeys = new();
            if (keyboardKey != null) KeyboardKeys.Add(new() { keyboardKey.Value });
            MouseButtons = new();
            if (mouseButton != null) MouseButtons.Add(mouseButton.Value);
            GamepadButtons = new();
            if (gamepadButton != null) GamepadButtons.Add(gamepadButton.Value);
            GamepadAxis = new();
            if (gamepadAxis != null) GamepadAxis.Add(gamepadAxis.Value);
        }

        public void Poll()
        {
            Pressed = false;
            Magnitude = 0.0;

            // Check keyboard keys
            foreach (var keyCombos in KeyboardKeys)
            {
                bool isPressed = true;

                foreach (var key in keyCombos)
                {
                    if (Keyboard.GetState().IsKeyDown(key) == false)
                    {
                        isPressed = false;
                    }
                }

                if (isPressed)
                {
                    Pressed = true;
                    Magnitude = 1.0;
                }
            }

            // Check mouse buttons
            if (Pressed == false)
            {
                foreach (var mb in MouseButtons)
                {
                    bool isPressed = false;

                    switch (mb)
                    {
                        case MouseButton.Left:
                            if (Mouse.GetState().LeftButton == ButtonState.Pressed) isPressed = true;
                            break;
                        case MouseButton.Right:
                            if (Mouse.GetState().RightButton == ButtonState.Pressed) isPressed = true;
                            break;
                        case MouseButton.Middle:
                            if (Mouse.GetState().MiddleButton == ButtonState.Pressed) isPressed = true;
                            break;
                        case MouseButton.Forward:
                            if (Mouse.GetState().XButton1 == ButtonState.Pressed) isPressed = true;
                            break;
                        case MouseButton.Backward:
                            if (Mouse.GetState().XButton2 == ButtonState.Pressed) isPressed = true;
                            break;
                    }

                    if (isPressed)
                    {
                        Pressed = true;
                        Magnitude = 1.0;
                    }
                }
            }

            // Check gamepad buttons
            if (Pressed == false)
            {
                foreach (var btn in GamepadButtons)
                {
                    if (GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One).IsButtonDown(btn))
                    {
                        Pressed = true;
                        Magnitude = 1.0;
                    }
                }
            }

            // Check gamepad axis
            if (Pressed == false)
            {
                foreach(var axis in GamepadAxis)
                {
                    const float deadzone = 0.0f;
                    GamePadState state = GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One);

                    switch (axis)
                    {
                        case Axis.LeftXPlus:
                            if (state.ThumbSticks.Left.X > deadzone)
                            {
                                Pressed = true;
                                Magnitude = state.ThumbSticks.Left.X;
                            }
                            break;
                        case Axis.LeftXMinus:
                            if (state.ThumbSticks.Left.X < -deadzone)
                            {
                                Pressed = true;
                                Magnitude = state.ThumbSticks.Left.X;
                            }
                            break;
                        case Axis.LeftYPlus:
                            if (state.ThumbSticks.Left.Y > deadzone)
                            {
                                Pressed = true;
                                Magnitude = state.ThumbSticks.Left.Y;
                            }
                            break;
                        case Axis.LeftYMinus:
                            if (state.ThumbSticks.Left.Y < -deadzone)
                            {
                                Pressed = true;
                                Magnitude = state.ThumbSticks.Left.Y;
                            }
                            break;
                        case Axis.RightXPlus:
                            if (state.ThumbSticks.Right.X > deadzone)
                            {
                                Pressed = true;
                                Magnitude = state.ThumbSticks.Right.X;
                            }
                            break;
                        case Axis.RightXMinus:
                            if (state.ThumbSticks.Right.X < -deadzone)
                            {
                                Pressed = true;
                                Magnitude = state.ThumbSticks.Right.X;
                            }
                            break;
                        case Axis.RightYPlus:
                            if (state.ThumbSticks.Right.Y > deadzone)
                            {
                                Pressed = true;
                                Magnitude = state.ThumbSticks.Right.Y;
                            }
                            break;
                        case Axis.RightYMinus:
                            if (state.ThumbSticks.Right.Y < -deadzone)
                            {
                                Pressed = true;
                                Magnitude = state.ThumbSticks.Right.Y;
                            }
                            break;
                    }

                    Magnitude = Math.Abs(Magnitude);
                }
            }
        }
    }
}
