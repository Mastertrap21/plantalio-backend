namespace Core.Service;

public interface IServiceBuilder
{
    IServiceBuilder Configure();

    IServiceBuilder Build();
        
    void Start();

    void Stop();
}