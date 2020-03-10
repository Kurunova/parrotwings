using System;

namespace BusinessLogicLayer.Exceptions
{
    public class ValidationException : Exception
    {
        public new string Message { get; private set; }

        public string FiledName { get; private set; }

        public string Error { get; private set; }

        public ValidationException(string fieldName, string error) : base()
        {
            FiledName = fieldName;
            Error = error;
        }
    }
}