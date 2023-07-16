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
            this._logger.LogInformation($"Client {Context.ConnectionId} is viewing {plantId}");
            return new Model.Plant()
            {
                Id = Guid.Parse("b00c58c0-df00-49ac-ae85-0a135f75e01b"),
                Name = "Name",
                Species = "Species",
            };
        }
    }
    
}