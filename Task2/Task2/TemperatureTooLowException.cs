using System.Runtime.Serialization;

namespace Task2
{
    internal class TemperatureTooLowException : Exception

    {
        public TemperatureTooLowException()
        {
        }

        public TemperatureTooLowException(string? message) : base(message)
        {
        }

        public TemperatureTooLowException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected TemperatureTooLowException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}