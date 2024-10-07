using BeymenCase.Configurations.ConfigurationReader;
using BeymenCase.Core.Interfaces;
using BeymenCase.SERVICE_A.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BeymenCase.SERVICE_A.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfigurationReader _configurationReader;
        private readonly IMessageConsumer _consumer;

        public HomeController(ILogger<HomeController> logger, IConfigurationReader configurationReader, IMessageConsumer consumer)
        {
            _logger = logger;
            _configurationReader = configurationReader;
            _consumer = consumer;
        }
        public async Task<IActionResult> Index()
        {
            var viewModel = new ConfigVM();

            var siteName = await _configurationReader.GetValueAsync<string>("SiteName");
            var maxItemCount = await _configurationReader.GetValueAsync<int?>("MaxItemCount");
            var isBasketEnabled = await _configurationReader.GetValueAsync<string>("IsBasketEnabled");

            viewModel.SiteName = siteName;
            viewModel.MaxItemCount = maxItemCount;
            viewModel.IsBasketEnabled = isBasketEnabled;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<JsonResult> GetLatestMessage()
        {
            var message = await _consumer.ReceiveMessageAsync();
            return Json(new { message = message });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
