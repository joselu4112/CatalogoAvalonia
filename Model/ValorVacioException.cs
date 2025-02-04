using System;

namespace CatalogoVista.Model
{
    [Serializable]
    internal class ValorVacioException : Exception
    {
        public ValorVacioException()
        {
        }

        public ValorVacioException(string? message) : base(message)
        {
        }

        public ValorVacioException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}