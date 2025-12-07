using Zenject;

public class FindingElementViewUIFactory : IFindingElementViewUIFactory
{
    private readonly DiContainer _container;
    private readonly FindingElementViewUI.Pool _pool;

    public FindingElementViewUIFactory(
        DiContainer container,
        FindingElementViewUI.Pool pool)
    {
        _container = container;
        _pool = pool;
    }

    public FindingElementViewUI Create()
    {
        return _pool.Spawn();
    }

    public void Despawn(FindingElementViewUI view)
    {
        _pool.Despawn(view);
    }
}