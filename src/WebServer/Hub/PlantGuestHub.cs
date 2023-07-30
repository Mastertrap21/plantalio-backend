using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace WebServer.Hub
{
    
    public class PlantGuestHub : GuestHub, IPlantGuestHub
    {
        private readonly ILogger<IPlantGuestHub> _logger;
        public PlantGuestHub(ILogger<PlantGuestHub> logger)
        {
            _logger = logger;
        }
        
        public async Task<Model.IPlant> GetPlant(Guid plantId)
        {
            // TODO: do actual async
            await Task.Run(() => { });
            _logger.LogInformation("Client {ContextConnectionId} is viewing {PlantId}", Context.ConnectionId, plantId);
            return new Model.Plant()
            {
                Id = Guid.Parse("b00c58c0-df00-49ac-ae85-0a135f75e01b"),
                Name = "Name",
                Species = "Species",
            };
        }
    }
    
}