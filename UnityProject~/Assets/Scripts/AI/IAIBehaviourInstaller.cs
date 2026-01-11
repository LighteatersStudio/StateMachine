using Zenject;

namespace Services.AI
{
    public interface IAIBehaviourInstaller
    {
        public void InstallBindings(DiContainer container);
    }
}