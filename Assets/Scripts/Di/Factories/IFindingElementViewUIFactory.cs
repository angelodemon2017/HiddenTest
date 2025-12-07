public interface IFindingElementViewUIFactory
{
    FindingElementViewUI Create();
    void Despawn(FindingElementViewUI view);
}