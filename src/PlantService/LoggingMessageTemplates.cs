namespace PlantService;

internal static class LoggingMessageTemplates
{
    public const string GetPlantRequestHandleFailed = "Failed to handle get plant request. Request: {@Request}";
    public const string GetPlantRequestHandlePlantIdCheckInfo = "Handling get plant request. Checking plant id: {PlantId}";
    public const string GetPlantRequestHandlePlantIdFoundInfo = "Plant found: {PlantId}";
    public const string GetPlantRequestHandlePlantIdNotFoundInfo = "Plant not found: {PlantId}";
}