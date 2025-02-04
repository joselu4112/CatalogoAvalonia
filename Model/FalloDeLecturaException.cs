using System;

namespace CatalogoVista.Model
{
    [Serializable]
    internal class FalloDeLecturaException : Exception
    {
        public FalloDeLecturaException()
        {
        }

        public FalloDeLecturaException(string? message) : base(message)
        {
        }

        public FalloDeLecturaException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}