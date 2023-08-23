namespace UserService;

internal static class LoggingMessageTemplates
{
    public const string LoginRequestHandleUsernameCheck = "Handling login request. Checking username: {Username}";
    public const string LoginRequestHandleUsersNullError = "Authentication failed for user: {Username} as users is null";
    public const string LoginRequestHandleAuthFailedError = "Authentication failed for user: {Username}";
    public const string LoginRequestHandleAuthSuccess = "Authentication succeeded for user: {Username}";
    public const string LoginRequestHandleFailError = "Failed to handle login request. Request: {@Request}";
    public const string RegisterRequestHandleUsernameCheck = "Handling register request. Checking username: {Username}";
    public const string RegisterRequestHandleFailError = "Failed to create user. Request: {@Request}";
}