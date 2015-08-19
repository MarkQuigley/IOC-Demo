using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twioc.Tests.TestClasses
{
    public interface IComplex
    {
        ITimer Timer { get; }
        void Delay(int seconds);
    }

    public class Complex : IComplex
    {
        private ITimer _timer;

        public ITimer Timer { get { return _timer; } }

        public Complex(ITimer timer)
        {
            _timer = timer;
        }

        public void Delay(int seconds)
        {
            _timer.Start(seconds*1000);
        }
    }
}
