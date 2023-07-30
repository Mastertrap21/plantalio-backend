namespace UserService.Model;

public interface IRegisterResponse
{
    bool Success { get; set; }
    ErrorCodes Error { get; set; }
}