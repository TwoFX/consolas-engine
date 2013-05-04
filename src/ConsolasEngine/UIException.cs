using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolasEngine
{
    public class UIException : Exception
    {
        public UIException()
        { }

        public UIException(string message)
            : base(message)
        { }

        public UIException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
