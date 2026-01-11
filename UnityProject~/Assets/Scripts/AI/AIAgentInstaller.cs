using UnityEngine;
using Zenject;

namespace Services.AI
{
    public class AIAgentInstaller : MonoInstaller
    {
        [SerializeField] private AIAgent _aiAgent;

        [SerializeField] private Component _behaviourInstaller;

        public override void InstallBindings()
        {
            BindSystems();
            BindSelf();

            if (_behaviourInstaller.TryGetComponent(out IAIBehaviourInstaller installer))
            {
                BindStates(installer);    
            }
            else
            {
                Debug.LogError($"{nameof(AIAgentInstaller)} requires an installer component.");
            }
        }

        private void BindSelf()
        { 
            Container.Bind<AIAgent>()
                .FromInstance(_aiAgent)
                .AsCached();

            Container.Bind<IAIAgentStop>()
                .To<AIAgent>()
                .FromResolve()
                .AsCached();
        }

        private void BindSystems()
        {
        }
        
        private void BindStates(IAIBehaviourInstaller installer)
        {
            installer.InstallBindings(Container);
        }
    }
}