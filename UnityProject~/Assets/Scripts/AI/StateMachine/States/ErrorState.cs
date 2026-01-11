using System;
using System.Threading;
using System.Threading.Tasks;
using Services.AI.StateMachine.Transitions;

namespace Services.AI.StateMachine.States
{
    public class ErrorState : SimpleBaseState
    {
        private readonly TaskCompletionSource<StateResult> _taskCompletionSource;

        public ErrorState(CancellationToken token)
            : base(token, Array.Empty<IStateTransitionFactory>())
        {
            _taskCompletionSource = new TaskCompletionSource<StateResult>();
        }
        
        protected override async Task<IAIState> LaunchInternal(CancellationToken token)
        {
            await _taskCompletionSource.Task;
            return new EmptyState();
        }
    }
}