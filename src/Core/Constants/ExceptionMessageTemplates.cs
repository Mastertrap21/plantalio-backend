namespace Core.Constants;

internal static class ExceptionMessageTemplates
{
    public const string FunctionProgramBaseConfigureServiceFunctionServiceNotFound = "FunctionService not found";
    public const string FunctionProgramBaseStartServiceProviderNotSet = "ServiceProvider not set";
    public const string FunctionServiceRegisterAnyHandlerAnyFunctionAlreadyHandled = "Any function is already handled";
    public const string FunctionServiceRegisterAnyHandlerAnyFunctionRegisterFailed = "Failed to register Any function";
    public const string ServiceBuilderStopServiceWithinTimeoutFailed = "Failed to stop service within timeout";
    public const string ProgramBaseServiceProviderSetAlreadySet = "ServiceProvider was already set";
}