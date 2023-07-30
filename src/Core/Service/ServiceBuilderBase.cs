namespace Core.Service;

public abstract class ServiceBuilderBase<TProgram> : IServiceBuilder where TProgram : IProgramBase, new()
{
    protected IProgramBase Program { get; } = new TProgram();

    public abstract IServiceBuilder Configure();
    public abstract IServiceBuilder Build();
    public abstract void Start();
    public abstract void Stop();
}