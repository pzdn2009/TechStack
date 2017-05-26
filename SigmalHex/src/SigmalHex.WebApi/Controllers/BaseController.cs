using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace SigmalHex.WebApi.Controllers
{
    [EnableCors(SigmalHexConstant.DefaultCorsPolicy)]
    public class BaseController : Controller
    {
    }
}
