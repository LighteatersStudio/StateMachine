using Services.AI.StateMachine.States;
using UnityEngine;
using Zenject;

namespace Services.AI
{
    public class AIAgent : MonoBehaviour, IAIAgentStop
    {
        [SerializeField] private bool _debugTrace;
        
        private bool _isInitialized;
        
        private IInitAIState.Factory _initStateFactory;
        private StateMachine.StateMachine _stateMachine;
        
        public string CurrentStateName => _stateMachine.CurrentStateName;
        
        [Inject]
        public void Construct(IInitAIState.Factory initStateFactory)
        {
            _initStateFactory = initStateFactory;
        }

        private void Start()
        {
            LaunchStateMachine();
            _isInitialized = true;
        }

        private void OnEnable()
        {
            if (_isInitialized)
            {
                _stateMachine?.Stop();
                LaunchStateMachine();
            }
        }

        private void LaunchStateMachine()
        {
            _stateMachine = new StateMachine.StateMachine(_initStateFactory.Create, gameObject.name, _debugTrace);
            _stateMachine.Launch();
        }
        
        private void OnDisable()
        {
            _stateMachine?.Stop();
        }

        public void Stop()
        {
            _stateMachine?.Stop();
        }
    }
}