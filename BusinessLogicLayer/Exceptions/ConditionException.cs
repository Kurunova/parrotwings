using System;

namespace BusinessLogicLayer.Exceptions
{
    public class ConditionException : Exception
    {
        public new string Message { get; private set; }

        public ConditionException(string message) : base()
        {
            Message = message;
        }
    }
}