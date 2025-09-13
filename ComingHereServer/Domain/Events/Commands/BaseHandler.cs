using ComingHereServer.Data.Interfaces;
using ComingHereServer.Interfaces;

namespace ComingHereServer.Domain.Events.Commands
{
    public abstract class BaseHandler
    {
        protected readonly IUnitOfWork _uow;
        protected readonly IHtmlSanitizingService _sanitizer;

        protected BaseHandler(IUnitOfWork uow, IHtmlSanitizingService sanitizer)
        {
            _uow = uow;
            _sanitizer = sanitizer;
        }
    }
}
