using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Localization;
using SigmalHex.Domain.FrameContext.ApplicationServices;
using SigmalHex.Domain.KBContext.ApplicationServices;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SigmalHex.WebApi.Controllers
{
    public class TodoController : BaseApiController
    {
        private readonly IStringLocalizer<TodoController> _localizer;

        IFileProvider fileProvider;
        private log4net.ILog log = log4net.LogManager.GetLogger(Startup.repository.Name, typeof(TodoController));

        public TodoController(IKnowledgeApplicationService knowledgeApplicationService, ITCPCollectorApplicationService tcpCollectorApplicationService, IStringLocalizer<TodoController> localizer, IServiceProvider serviceProvider)
        {
            KnowledgeApplicationService = knowledgeApplicationService;
            TCPCollectorApplicationService = tcpCollectorApplicationService;

            _localizer = localizer;
            ServiceProvider = serviceProvider;
        }

        public IKnowledgeApplicationService KnowledgeApplicationService { get; set; }
        public ITCPCollectorApplicationService TCPCollectorApplicationService { get; set; }


        public IServiceProvider ServiceProvider { get; set; }
        // GET api/todo
        [HttpGet]
        public async Task<string> GetAsync()
        {
            log.Info(_localizer["Get Async information."]);
            log.Info(_localizer["Hello"]);

            fileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
            var fileInfo = fileProvider.GetFileInfo("Features.txt");

            var stream = fileInfo.CreateReadStream();

            byte[] buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer, 0, buffer.Length);

            return Encoding.UTF8.GetString(buffer);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            if (id == 1)
            {
                return Ok(KnowledgeApplicationService.GetAll());
            }
            if (id == 2)
            {
                var tcpSvc = ServiceProvider.GetService(typeof(ITCPCollectorApplicationService)) as ITCPCollectorApplicationService;
                return Ok(tcpSvc.GetAll());
            }
            else
            {
                return Ok(TCPCollectorApplicationService.GetAll());
            }
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
