using HEALTH_SUPPORT.Services.DTOs.Dashboard;
using HEALTH_SUPPORT.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(
            IDashboardService dashboardService,
            ILogger<DashboardController> logger)
        {
            _dashboardService = dashboardService;
            _logger = logger;
        }

        [HttpGet("subscriptions/monthly")]
        public async Task<ActionResult<List<MonthlySubscriptionStats>>> GetMonthlySubscriptionStats()
        {
            try
            {
                var stats = await _dashboardService.GetMonthlySubscriptionStats();
                return Ok(stats);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error retrieving monthly subscription stats");
                return StatusCode(500, "An error occurred while retrieving subscription statistics");
            }
        }

        [HttpGet("revenue/monthly")]
        public async Task<ActionResult<List<MonthlyRevenueStats>>> GetMonthlyRevenueStats()
        {
            try
            {
                var stats = await _dashboardService.GetMonthlyRevenueStats();
                return Ok(stats);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error retrieving monthly revenue stats");
                return StatusCode(500, "An error occurred while retrieving revenue statistics");
            }
        }
    }
} 