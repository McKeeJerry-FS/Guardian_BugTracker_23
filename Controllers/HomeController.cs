using Guardian_BugTracker_23.Data.Enums;
using Guardian_BugTracker_23.Extensions;
using Guardian_BugTracker_23.Models;
using Guardian_BugTracker_23.Models.ChartModels;
using Guardian_BugTracker_23.Services;
using Guardian_BugTracker_23.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Guardian_BugTracker_23.Controllers
{
    public class HomeController : BTBaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IBTCompanyService _btCompanyService;
        private readonly IBTProjectService _btProjectService;
        private readonly IBTTicketService _btTicketService;
        private readonly UserManager<BTUser> _userManager;

        public HomeController(ILogger<HomeController> logger,
                              IConfiguration configuration,
                              IBTCompanyService bTCompanyService,
                              IBTProjectService bTProjectService,
                              IBTTicketService bTTicketService,
                              UserManager<BTUser> userManager)
        {
            _logger = logger;
            _configuration = configuration;
            _btCompanyService = bTCompanyService;
            _btProjectService = bTProjectService;
            _btTicketService = bTTicketService;
            _userManager = userManager;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

		public IActionResult Landing()
		{
			return View();
		}

		public IActionResult FileManager()
		{
			return View();
		}

		public IActionResult About()
		{
			return View();
		}

		public IActionResult Calendar()
		{
			return View();
		}

		public IActionResult ToDo()
		{
			return View();
		}

		public IActionResult Mail()
		{
			return View();
		}

		[HttpPost]
		public async Task<JsonResult> AmCharts()
		
		{

			AmChartData amChartData = new();
			List<AmItem> amItems = new();

            int companyId = _companyId;

			List<Project> projects = (await _btCompanyService.GetProjectsAsync(_companyId)).Where(p => p.Archived == false).ToList();

			foreach (Project project in projects)
			{
				AmItem item = new();

				item.Project = project.Name;
				item.Tickets = project.Tickets.Count;
				item.Developers = (await _btProjectService.GetProjectMembersByRoleAsync(project.Id, nameof(BTRoles.Developer), _companyId)).Count;

				amItems.Add(item);
			}

			amChartData.Data = amItems.ToArray();


			return Json(amChartData.Data);
		}

		[HttpPost]
		public async Task<JsonResult> GglProjectTickets()
		{
            int companyId = _companyId;

			List<Project> projects = await _btProjectService.GetAllProjectsByCompanyIdAsync(_companyId);

			List<object> chartData = new();
			chartData.Add(new object[] { "ProjectName", "TicketCount" });

			foreach (Project prj in projects)
			{
				chartData.Add(new object[] { prj.Name!, prj.Tickets.Count() });
			}

			return Json(chartData);
		}

		[HttpPost]
		public async Task<JsonResult> GglProjectPriority()
		{
			

			List<Project> projects = await _btProjectService.GetAllProjectsByCompanyIdAsync(_companyId);

			List<object> chartData = new();
			chartData.Add(new object[] { "Priority", "Count" });


			foreach (string priority in Enum.GetNames(typeof(BTProjectPriorities)))
			{
				int priorityCount = (await _btProjectService.GetAllProjectsByPriorityAsync(_companyId, priority)).Count();
				chartData.Add(new object[] { priority, priorityCount });
			}

			return Json(chartData);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
