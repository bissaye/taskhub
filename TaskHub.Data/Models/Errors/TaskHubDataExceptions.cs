
namespace TaskHub.Data.Models.Errors
{
    public class ReadErrorException : Exception
    {
        public ReadErrorException() : base() { }
        public ReadErrorException(string msg) : base(msg) { }
    }
    public class SavingErrorException : Exception
    {
        public SavingErrorException() : base() { }
        public SavingErrorException(string msg) : base(msg) { }
    }
    public class UpdateErrorException : Exception
    {
        public UpdateErrorException() : base() { }
        public UpdateErrorException(string msg) : base(msg) { }
    }
    public class NotFoundException : Exception
    {
        public NotFoundException() : base() { }
        public NotFoundException(string msg) : base(msg) { }
    }

    public class AlreadyExistException : Exception
    {
        public AlreadyExistException() : base() { }
        public AlreadyExistException(string msg) : base(msg) { }
    }

    public class DeleteErrorException : Exception
    {
        public DeleteErrorException() : base() { }
        public DeleteErrorException(string msg) : base(msg) { }
    }

}
