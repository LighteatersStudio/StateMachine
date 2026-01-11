using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Services.AI.StateMachine.Transitions;
using Zenject;

namespace Services.AI.StateMachine.States
{
    public abstract class BaseState : IAIState
    {
        private readonly CancellationToken _mainToken;
        private readonly List<IStateTransitionFactory> _transitionFactory;
        
        private readonly CancellationTokenSource _selfTokenSource;
        private readonly List<IStateTransition> _transitions = new ();
        private CancellationTokenRegistration _mainTokenHandler;
        
        
        private IAIState _transitionState;

        protected BaseState(CancellationToken mainToken, IEnumerable<IStateTransitionFactory> transitionsFactories)
        {
            _mainToken = mainToken;
            _transitionFactory = transitionsFactories.ToList();
            _selfTokenSource = new CancellationTokenSource();
        }
        
        public void Begin()
        {
            _mainTokenHandler = _mainToken.Register(_selfTokenSource.Cancel);

            foreach (var factory in _transitionFactory)
            {
                var transition = factory.Create(_mainToken);
                transition.Activated += OnTransitionActivated;
                transition.Initialize();
                _transitions.Add(transition);
            }
            
            BeginInternal();
        }
        protected abstract void BeginInternal();

        public async Task<IAIState> Launch()
        {
            var commonResult = await LaunchInternal(_selfTokenSource.Token);
            return _transitionState ?? commonResult;
        }
        
        protected abstract Task<IAIState> LaunchInternal(CancellationToken token);

        public void Release()
        {
            _mainTokenHandler.Dispose();
            _selfTokenSource.Cancel();
            
            foreach (var transition in _transitions)
            {
                transition.Activated -= OnTransitionActivated;
                transition.Release();
            }
            
            EndInternal();
        }
        protected abstract void EndInternal();
        
        protected TState ActivateState<TState>(PlaceholderFactory<CancellationToken, TState> factory) where TState : IAIState
        {
            return factory.Create(_mainToken);
        }
        
        protected TState ActivateState<TParameter, TState>(PlaceholderFactory<CancellationToken, TParameter, TState> factory, TParameter parameter) where TState : IAIState
        {
            return factory.Create(_mainToken, parameter);
        }

        protected TState ActivateState<TState>(Func<CancellationToken, TState> factoryMethod) where TState : IAIState
        {
            return factoryMethod.Invoke(_mainToken);
        }

        protected TState ActivateState<TParameter, TState>(Func<CancellationToken, TParameter, TState> factoryMethod,
            TParameter parameter) where TState : IAIState
        {
            return factoryMethod.Invoke(_mainToken, parameter);
        }
        
        private void OnTransitionActivated(IAIState nextState)
        {
            _transitionState = nextState;
            _selfTokenSource.Cancel();
        }
    }
}