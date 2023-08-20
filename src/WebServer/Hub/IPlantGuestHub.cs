namespace WebServer.Hub;

public interface IPlantGuestHub
{
    Task<Model.IPlant> GetPlant(Guid plantId);
}