using CoreFX.Abstractions.Bases.Interfaces;
using CoreFX.Abstractions.Logging;
using Microsoft.Extensions.Logging;

namespace CoreFX.Abstractions.Bases
{
    public abstract class DomainServiceBase : IDomainServiceBase
    {
        protected DomainServiceBase() : this(null) { }

        protected DomainServiceBase(ILogger logger)
        {
            _logger = logger ?? LogMgr.CreateLogger(GetType());
        }

        protected readonly ILogger _logger;
        public ILogger GetLogger() => _logger;
    }
}
