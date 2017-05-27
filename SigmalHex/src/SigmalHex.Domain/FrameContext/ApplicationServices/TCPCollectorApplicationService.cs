using SigmalHex.Domain.FrameContext.Entities;
using System.Collections.Generic;

namespace SigmalHex.Domain.FrameContext.ApplicationServices
{
    public class TCPCollectorApplicationService : ITCPCollectorApplicationService
    {
        public IEnumerable<TCPCollector> GetAll()
        {
            return new List<TCPCollector>()
            {
                new TCPCollector(){ Name = "rmq"},
                new TCPCollector(){ Name = "zk"}
            };
        }
    }
}
