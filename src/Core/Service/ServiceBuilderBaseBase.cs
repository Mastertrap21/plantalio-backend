namespace Core.Service;

public abstract class ServiceBuilderBaseBase<TProgram> : IServiceBuilderBase where TProgram : IProgramBase, new()
{
    protected IProgramBase Program { get; } = new TProgram();

    public abstract IServiceBuilderBase Configure();
    public abstract IServiceBuilderBase Build();
    public abstract void Start();
    public abstract void Stop();
}