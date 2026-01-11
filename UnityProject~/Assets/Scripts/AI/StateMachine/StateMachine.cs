using System;
using System.Threading;
using UnityEngine;

namespace Services.AI.StateMachine
{
    public class StateMachine
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly Func<CancellationToken, IAIState> _startStateFactory;
        private readonly string _name;
        private readonly bool _debugTrace;

        public string CurrentStateName { get; private set; }


        public StateMachine(Func<CancellationToken, IAIState>  startStateFactory, string name, bool debugTrace)
        {
            _startStateFactory = startStateFactory;
            _name = name;
            _debugTrace = debugTrace;
        }

        public void Launch()
        {
            ProcessLoop(_cancellationTokenSource.Token);
        }
        
        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }
        
        private async void ProcessLoop(CancellationToken token)
        {
            var currentState = _startStateFactory(token);
            
            while (!token.IsCancellationRequested)
            {
                CurrentStateName = currentState.GetType().Name;
                
                if (_debugTrace)
                {
                    Debug.Log($"[{_name}] AI launch state: {CurrentStateName}");   
                }
                
                currentState.Begin();
                var nextState = await currentState.Launch();
                currentState.Release();
                
                currentState = nextState;
            }
        }
    }
}