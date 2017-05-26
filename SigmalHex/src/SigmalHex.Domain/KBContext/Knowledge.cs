using System.Collections.Generic;

namespace SigmalHex.Domain.KBContext
{
    public class Knowledge
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<Tag> Tags { get; set; }
    }
}
