using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace SigmalHex.WebApi.Controllers
{
    [Route("api/{language:regex(^[[a-z]]{{2}}(?:-[[A-Z]]{{2}})?$)}/[controller]")]
    [Route("api/[controller]")]
    [EnableCors(SigmalHexConstant.DefaultCorsPolicy)]
    public class BaseApiController : Controller
    {
    }
}
