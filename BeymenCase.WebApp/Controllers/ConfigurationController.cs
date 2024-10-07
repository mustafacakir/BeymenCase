using BeymenCase.Core.Entities;
using BeymenCase.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BeymenCase.WebApp.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly IBaseRepository<ConfigurationRecord> _repository;
        private readonly IQueueService _queueService;

        public ConfigurationController(IBaseRepository<ConfigurationRecord> repository, IQueueService queueService)
        {
            _repository = repository;
            _queueService = queueService;
        }

        public async Task<IActionResult> Index()
        {
            var configurations = await _repository.GetListAsync();
            return View(configurations);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ConfigurationRecord configuration)
        {
            if (configuration == null)
            {
                return BadRequest("Configuration cannot be null.");
            }

            await _repository.CreateAsync(configuration);
            _queueService.SendMessage(configuration.Name);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(string applicationName, string id)
        {
            var configuration = await _repository.GetByIdAsync($"{applicationName}:{id}");
            if (configuration == null)
            {
                return NotFound();
            }
            return View(configuration);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ConfigurationRecord configuration)
        {
            if (configuration == null)
            {
                return BadRequest("Configuration cannot be null.");
            }

            await _repository.UpdateAsync(configuration);
            return RedirectToAction(nameof(Index));
        }

    }
}
