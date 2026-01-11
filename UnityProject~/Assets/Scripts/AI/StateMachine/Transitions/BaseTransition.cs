using System;
using System.Threading;
using Zenject;

namespace Services.AI.StateMachine.Transitions
{
    public abstract class BaseTransition : IStateTransition
    {
        private readonly CancellationToken _token;
        
        public event Action<IAIState> Activated;

        protected BaseTransition(CancellationToken token)
        {
            _token = token;
        }

        public abstract void Initialize();
        public abstract void Release();

        protected TState ActivateState<TState>(PlaceholderFactory<CancellationToken, TState> factory) where TState : IAIState
        {
            return factory.Create(_token);
        }
        
        protected TState ActivateState<TState>(Func<CancellationToken, TState> factoryMethod) where TState : IAIState
        {
            return factoryMethod.Invoke(_token);
        }
        
        protected void OnActivated(IAIState state)
        {
            Activated?.Invoke(state);
        }
    }
}