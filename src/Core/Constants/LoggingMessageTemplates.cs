namespace Core.Constants;

internal static class LoggingMessageTemplates
{
    public const string ServiceCollectionExtensionsMigratingDatabaseContext = "Migrating database context: {Context}";
    public const string ServiceCollectionExtensionsDatabaseMigrationComplete = "Database context migration completed: {Context}";
    public const string ServiceCollectionExtensionsDatabaseMigrationUnableToConnect = "Unable to connect to database, trying again in 5 seconds..";
    public const string ServiceCollectionExtensionsDatabaseMigrationFail = "Failed to migrate database context: {Context}";
    public const string FunctionServiceHandleFunctionConsumedFunction = "Consumed function: {Function}";
    public const string FunctionServiceHandleFunctionHandlingAnyFunction = "Handling ANY function";
    public const string FunctionServiceHandleFunctionProcessedAnyFunction = "Processed ANY function";
    public const string FunctionServiceHandleFunctionIgnoreNotAttachedFunction = "Function ignored due to not attached: {Function}";
    public const string FunctionServiceHandleFunctionHandlingFunctionAndPayload = "Handling function: {Function}, Payload: {Payload}";
    public const string FunctionServiceHandleFunctionHandlingInvalidFunctionTypeIgnored = "Function ignored due to invalid function type: {Function}";
    public const string FunctionServiceHandleFunctionHandlingInvalidFunctionPayloadIgnored = "Function '{Function}' ignored due to invalid function payload: {Payload}";
    public const string FunctionServiceHandleFunctionHandlingProcessedFunction = "Function processed: {Function}";
    public const string FunctionServiceHandleFunctionHandlingUnableHandleFunction = "Unable to handle function: {Function}";
    public const string FunctionServiceRegisterFunctionRegistering = "Registering function: {Function}";
    public const string FunctionServiceRegisterFunctionRegistered = "Function registered: {Function}";
    public const string FunctionServiceRegisterAddToListenersFailed = "Failed to add function to listeners: {Function}";
    public const string FunctionProgramBaseStartServiceStarting = "Starting service";
    public const string ServiceBuilderStartSigtermSignalReceived = "SIGTERM signal received";
    public const string ProgramBaseServiceProviderSetRunningOnCoreVersion = "Running on core version: {Version}";
    public const string ProgramBaseServiceProviderSetServiceMetadataValues = "Service metadata values: {@Metadata}";
    public const string ProgramBaseStartServiceStarted = "Service started (PID: {Pid})";
    public const string ProgramBaseStartServiceStopping = "Stopping service";
    public const string ProgramBaseStartTimeoutOccuredWhenStoppingService = "Timeout occurred when trying to stop service";
    public const string ProgramBaseStartServiceStoppedGracefully = "Service stopped gracefully";
    public const string ProgramBaseStartServiceStopGracefullyFailed = "Failed to stop service gracefully";
}