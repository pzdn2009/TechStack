using SigmalHex.Domain.KBContext.Entities;
using System;
using System.Collections.Generic;

namespace SigmalHex.Domain.KBContext.ApplicationServices
{
    public class KnowledgeApplicationService : IKnowledgeApplicationService
    {
        Guid id = Guid.NewGuid();

        public IEnumerable<Knowledge> GetAll()
        {
            return new List<Knowledge>()
            {
                new Knowledge()
                {
                    Id = id,
                    Name=".NET CORE",
                    Description = "跨平台",
                    Tags = new string[]{"DEV","LINUX"}
                },
                new Knowledge()
                {
                    Id = id,
                    Name="C#",
                    Description = "跨平台",
                    Tags = new string[]{"DEV","LINUX"}
                }
            };
        }
    }
}
