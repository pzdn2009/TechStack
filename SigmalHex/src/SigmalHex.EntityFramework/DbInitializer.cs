using SigmalHex.Domain.KBContext.Entities;
using System.Linq;

namespace SigmalHex.EntityFramework
{
    public static class DbInitializer
    {
        public static void Initialize(SigmalHexContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Knowledges.Any())
            {
                return;   // DB has been seeded
            }

            var kbs = new Knowledge[]
            {
                new Knowledge{ Description = "Records sth.", Name = "asp.net core", Tags =".net core,C# 7" },
            };

            foreach (var s in kbs)
            {
                context.Knowledges.Add(s);
            }

            context.SaveChanges();
        }
    }
}
