namespace Core.Service;

public interface IServiceBuilderBase
{
    IServiceBuilderBase Configure();

    IServiceBuilderBase Build();
        
    void Start();

    void Stop();
}