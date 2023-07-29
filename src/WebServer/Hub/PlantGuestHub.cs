using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace WebServer.Hub
{
    
    public class PlantGuestHub : GuestHub
    {
        private readonly ILogger _logger;
        public PlantGuestHub(ILogger<PlantGuestHub> logger)
        {
            this._logger = logger;
        }
        
        public async Task<Model.Plant> GetPlant(Guid plantId)
        {
            this._logger.LogInformation("Client {ContextConnectionId} is viewing {PlantId}", Context.ConnectionId, plantId);
            return new Model.Plant()
            {
                Id = Guid.Parse("b00c58c0-df00-49ac-ae85-0a135f75e01b"),
                Name = "Name",
                Species = "Species",
            };
        }
    }
    
}