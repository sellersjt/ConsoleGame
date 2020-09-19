using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MySyrniaGame
{
    public class SpinnerRepo
    {

        public class Spinner : IDisposable
        {
            private const string _sequence = @"/-\|";
            private int _counter = 0;
            private readonly int _left;
            private readonly int _top;
            private readonly int _delay;
            private bool _active;
            private readonly Thread _thread;

            public Spinner(int left, int top, int delay = 100)
            {
                this._left = left;
                this._top = top;
                this._delay = delay;
                _thread = new Thread(Spin);
            }

            public void Start()
            {
                _active = true;
                if (!_thread.IsAlive)
                    _thread.Start();
            }

            public void Stop()
            {
                _active = false;
                Draw(' ');
            }

            private void Spin()
            {
                while (_active)
                {
                    Turn();
                    Thread.Sleep(_delay);
                }
            }

            private void Draw(char c)
            {
                Console.SetCursorPosition(_left, _top);
                // Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(c);
            }

            private void Turn()
            {
                Draw(_sequence[++_counter % _sequence.Length]);
            }

            public void Dispose()
            {
                Stop();
            }
        }
    }
}
