using Zenject;

public class CarInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        //Container.Bind<IMovable>().To<CarEngine>().FromNew().AsSingle();
    }
}
