using SigmalHex.Domain.KBContext.Entities;
using System.Collections.Generic;

namespace SigmalHex.Domain.KBContext.ApplicationServices
{
    public interface IKnowledgeApplicationService
    {
        IEnumerable<Knowledge> GetAll();
    }
}
