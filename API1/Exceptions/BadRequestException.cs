namespace API1.Exceptions
{

    class BadRequestException : Exception

    {
        public BadRequestException(string message) : base(message) { }
    }
}

