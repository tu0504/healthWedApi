using HEALTH_SUPPORT.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HEALTH_SUPPORT.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public IActionResult Index()
        {
            return View();
        }
    }
}
