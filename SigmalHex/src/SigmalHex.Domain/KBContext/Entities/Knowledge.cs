using System;

namespace SigmalHex.Domain.KBContext.Entities
{
    public class Knowledge
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Tags { get; set; }
    }
}
