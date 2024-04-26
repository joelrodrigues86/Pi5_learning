using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Infrastructure.Controllers
{
    public class XboxGamepad
    {

        public delegate void testEventHandler(object source, string message);
        public event testEventHandler testEvento;

        private int MovementDivider = 2_000;
        private int _refreshRate = 60;

        //Deadzones
        private int _leftDeadzone = 200;
        private int _rightDeadzone = 200;

        private Timer _timer;
        private Controller _controller;

        private bool _wasADown;
        private bool _wasBDown;

        public XboxGamepad(int RefreshRate = 60)
        {
            _controller = getConnectedController();
            _timer = new Timer(obj => Update());
            _refreshRate = RefreshRate;
        }

        public void Start()
        {
            _timer.Change(0, 1000 / _refreshRate);
        }

        private Controller getConnectedController()
        {
            var controllers = new[] { 
                new Controller(UserIndex.One), 
                new Controller(UserIndex.Two), 
                new Controller(UserIndex.Three), 
                new Controller(UserIndex.Four)
            };
            return controllers?.FirstOrDefault(x => x.IsConnected) ?? new Controller(UserIndex.One);
        }


        private void Update()
        {
            _controller.GetState(out var state);
            Movement(state);

            //LeftButton(state);
            //RightButton(state);
        }

        private void Movement(State state)
        {
            float LX = state.Gamepad.LeftThumbX;
            float LY = state.Gamepad.LeftThumbY;

            //determine how far the controller is pushed
            double magnitude = Math.Sqrt(LX * LX + LY * LY);

            //determine the direction the controller is pushed
            double normalizedLX = LX / magnitude;
            double normalizedLY = LY / magnitude;

            double normalizedMagnitude = 0;

            //Console.Write("normalizedLX - " + normalizedLX);
            //Console.Write("normalizedLY - " + normalizedLY);

            //check if the controller is outside a circular dead zone
            if (magnitude > Gamepad.LeftThumbDeadZone)
            {
                //clip the magnitude at its expected maximum value
                if (magnitude > 32767) magnitude = 32767;

                //adjust magnitude relative to the end of the dead zone
                magnitude -= Gamepad.LeftThumbDeadZone;

                //optionally normalize the magnitude with respect to its expected range
                //giving a magnitude value of 0.0 to 1.0
                normalizedMagnitude = magnitude / (32767 - Gamepad.LeftThumbDeadZone);
                testEvento(this, normalizedMagnitude.ToString());
            }
            else //if the controller is in the deadzone zero out the magnitude
            {
                magnitude = 0.0;
                normalizedMagnitude = 0.0;
            }



            var x = state.Gamepad.LeftThumbX / MovementDivider;
            var y = state.Gamepad.LeftThumbY / MovementDivider;
            //Console.Write("LEFT X - " + state.Gamepad.LeftThumbX);
            //Console.Write("LEFT Y - " + state.Gamepad.LeftThumbY);
            //Console.Write(y);
            //_mouseSimulator.MoveMouseBy(x, -y);
            //32768
        }

        private void RightButton(State state)
        {
            var isBDown = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.B);
            //if (isBDown && !_wasBDown) _mouseSimulator.RightButtonDown();
            //if (!isBDown && _wasBDown) _mouseSimulator.RightButtonUp();
            _wasBDown = isBDown;
        }

        private void LeftButton(State state)
        {
            var isADown = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.A);
            //if (isADown && !_wasADown) _mouseSimulator.LeftButtonDown();
            //if (!isADown && _wasADown) _mouseSimulator.LeftButtonUp();
            _wasADown = isADown;
        }


    }
}
