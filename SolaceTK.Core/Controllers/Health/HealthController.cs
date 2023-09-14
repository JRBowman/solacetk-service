using Microsoft.AspNetCore.Mvc;
using SolaceTK.Core.Contexts;
using SolaceTK.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace SolaceTK.Core.Controllers.Health
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly CoreContext _context;
        private readonly BehaviorContext _behaviorContext;
        private readonly SolaceTK.Core.Contexts.ControllerContext _controllerContext;
        private readonly EnvironmentContext _envContext;
        private readonly SoundContext _soundContext;
        private readonly WorkContext _workContext;

        public AsepriteService AsepriteService { get; private set; }

        public HealthController(CoreContext context, BehaviorContext behaviorContext, 
        SolaceTK.Core.Contexts.ControllerContext controllerContext, EnvironmentContext envContext,
        SoundContext soundContext, WorkContext workContext, AsepriteService aseService)
        {
            _context = context;
            _behaviorContext = behaviorContext;
            _controllerContext = controllerContext;
            _envContext = envContext;
            _soundContext = soundContext;
            _workContext = workContext;
            AsepriteService = aseService;
        }

        // GET: api/v1/Health
        [HttpGet]
        public async Task<ActionResult<ServiceHealthReport>> GetStatus()
        {
            var shr = new ServiceHealthReport();

            shr.ServiceStatus.Add($"Serving Status is Online: {this.Request.Host}");

            // If Connection can reach the API - then the Service is Alive:
            shr.DataStatus.Add($"Core Service - {_context.Database.CanConnect()} - using Provider: {_context.Database.ProviderName}");
            shr.DataStatus.Add($"Behavior Service - {_behaviorContext.Database.CanConnect()} - using Provider: {_behaviorContext.Database.ProviderName}");
            shr.DataStatus.Add($"Controller Service - {_controllerContext.Database.CanConnect()} - using Provider: {_controllerContext.Database.ProviderName}");
            shr.DataStatus.Add($"Environment Service - {_envContext.Database.CanConnect()} - using Provider: {_envContext.Database.ProviderName}");
            shr.DataStatus.Add($"Sound Service - {_soundContext.Database.CanConnect()} - using Provider: {_soundContext.Database.ProviderName}");
            shr.DataStatus.Add($"Work Service - {_workContext.Database.CanConnect()} - using Provider: {_workContext.Database.ProviderName}");

            // Aseprite Integration Present and Working:
            shr.AsepriteStatus = await CheckAseprite();

            return Ok(shr);
        }

        [HttpGet("Inventory")]
        public async Task<ActionResult<InventoryReport>> GetInventory()
        {
            var inventory = new InventoryReport()
            {
                Projects = (await _workContext.Projects.ToListAsync()).Count,
                Controllers = (await _controllerContext.Controllers.ToListAsync()).Count,
                Behaviors = (await _behaviorContext.Systems.ToListAsync()).Count,
                States = (await _behaviorContext.States.ToListAsync()).Count,
                Animations = (await _behaviorContext.Animations.ToListAsync()).Count,
                Events = (await _behaviorContext.Events.ToListAsync()).Count,
                Maps = (await _envContext.Maps.ToListAsync()).Count,
                Tilesets = (await _envContext.TileSets.ToListAsync()).Count,
                Timelines = (await _context.Timelines.ToListAsync()).Count,
                StoryCards = (await _context.StoryCards.ToListAsync()).Count,
                ResourceCollections = (await _context.Collections.ToListAsync()).Count,
                SoundSets = (await _soundContext.SoundSets.ToListAsync()).Count
                //Interfaces = (await _interfaceContext.Interfaces.ToListAsync()).Count
            };

            // Collect Inventory:

            return inventory;
        }

        private async Task<List<string>> CheckAseprite()
        {
            List<string> aseChecks = new();
            var aseVersion = "";
            var aseEnabled = false;
            var aseEnabledMessage = "";

            // Check if Aseprite is Present:
            var asePresent = FileService.FileExists(Program.AseCli);
            aseChecks.Add(asePresent ? "Aseprite is present." : "Aseprite is not present.");

            if (asePresent)
            {
                aseVersion = await AsepriteService.GetVersion();
                aseEnabled = aseVersion.Contains("Aseprite ");
                if (aseEnabled) aseChecks.Add(aseVersion);
                else  
                {
                    aseChecks.Add("Aseprite is Present but not currently usable.");
                    aseChecks.Add("Validate that the current Service Account can RWX Aseprite.");
                }
            }
            
            aseEnabledMessage = asePresent && aseEnabled ? "Enabled." : "Disabled.";
            aseChecks.Add($"Aseprite is {aseEnabledMessage}");

            return aseChecks;
        }
    }

    public class ServiceHealthReport
    {
        public List<string> ServiceStatus { get; set; } = new();
        public List<string> DataStatus { get; set; } = new();
        public List<string> AsepriteStatus { get; set; } = new();
    }

    public class InventoryReport
    {
        public int Projects { get; set; }
        public int Behaviors { get; set; }
        public int States { get; set; }
        public int Animations { get; set; }
        public int Controllers { get; set; }
        public int Events { get; set; }
        public int Maps { get; set; }
        public int Tilesets { get; set; }
        public int Interfaces { get; set; }
        public int SoundSets { get; set; }
        public int ResourceCollections { get; set; }
        public int Timelines { get; set; }
        public int StoryCards { get; set; }

    }
}