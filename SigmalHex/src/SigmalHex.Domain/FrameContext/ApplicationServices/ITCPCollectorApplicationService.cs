using SigmalHex.Domain.FrameContext.Entities;
using System.Collections.Generic;

namespace SigmalHex.Domain.FrameContext.ApplicationServices
{
    public interface ITCPCollectorApplicationService
    {
        IEnumerable<TCPCollector> GetAll();
    }
}
