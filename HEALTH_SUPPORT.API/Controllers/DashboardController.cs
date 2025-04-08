using HEALTH_SUPPORT.Services.Implementations;
using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.ResponseModel;
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

        [HttpGet("stats")]
        public async Task<ActionResult<DashboardResponse.DashboardStats>> GetDashboardStats()
        {
            try
            {
                var stats = await _dashboardService.GetDashboardStats();
                return Ok(stats);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dashboard stats");
                return StatusCode(500, "An error occurred while retrieving dashboard statistics");
            }
        }

        [HttpGet("subscriptions/monthly")]
        public async Task<ActionResult<List<DashboardResponse.MonthlySubscriptionStats>>> GetMonthlySubscriptionStats()
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
        public async Task<ActionResult<List<DashboardResponse.MonthlyRevenueStats>>> GetMonthlyRevenueStats()
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

        [HttpGet("totalMonthly")]
        public async Task<IActionResult> GetTotalMonthlyStats()
        {
            var stats = await _dashboardService.GetTotalMonthlyStats();
            return Ok(stats);
        }

        [HttpGet("totalAccounts")]
        public async Task<IActionResult> GetTotalStudentAndParentAccounts()
        {
            var total = await _dashboardService.GetTotalAccountsAsync();
            return Ok(total);
        }

        [HttpGet("ordersToday")]
        public async Task<IActionResult> GetTodayOrders()
        {
            var total = await _dashboardService.GetTodayOrderCountAsync();
            return Ok(total);
        }
    }
} 