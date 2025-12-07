using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private BaseMonoLevel _baseMonoLevel;

    public override void InstallBindings()
    {
        var ls = Container.Resolve<SceneService>();
        ls.SetLevel(_baseMonoLevel);
    }
}