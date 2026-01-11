using System.Threading.Tasks;

namespace Services.AI.StateMachine.States
{
    public class EmptyState : IAIState
    {
        private readonly TaskCompletionSource<StateResult> _taskCompletionSource;

        public EmptyState()
        {
            _taskCompletionSource = new TaskCompletionSource<StateResult>();
        }

        public void Begin()
        {
        }

        public async Task<IAIState> Launch()
        {
            await _taskCompletionSource.Task;
            
            return new EmptyState();
        }

        public void Release()
        {
        }
    }
}