using TranslaterServiceBL.Common.Exceptions;

namespace TranslaterServiceBL.Common
{
    public abstract class ValidatorDto<InputObjectDto, TResult>
         where InputObjectDto : class
    {
        private Lazy<DtoVereficationException> _exception;
        public string Message { get; set; } = string.Empty;
        public DtoVereficationException Exception { get { return _exception.Value; } }

        public ValidatorDto()
        {
            _exception = new Lazy<DtoVereficationException>(() => new DtoVereficationException(Message));
        }

        public abstract Task<TResult> IsValid(InputObjectDto inputObjectDto);
    }
}
