using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SigmalHex.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        IFileProvider fileProvider;

        // GET api/todo
        [HttpGet]
        public async Task<string> GetAsync()
        {
            fileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
            var fileInfo = fileProvider.GetFileInfo("Features.txt");

            var stream = fileInfo.CreateReadStream();

            byte[] buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer, 0, buffer.Length);

            return Encoding.UTF8.GetString(buffer);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
