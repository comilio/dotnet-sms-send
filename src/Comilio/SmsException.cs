using System;
using System.Collections.Generic;
using System.Text;

namespace Comilio
{
    public class SmsException : Exception
    {
        public SmsException(string message) 
            : base(message)
        {
        }
    }
}
