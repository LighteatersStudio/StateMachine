using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Services.AI.StateMachine.Transitions;

namespace Services.AI.StateMachine.States
{
    public abstract class ThrottlingState : SimpleBaseState
    {
        protected ThrottlingState(CancellationToken mainToken, IEnumerable<IStateTransitionFactory> transitionsFactories)
            : base(mainToken, transitionsFactories)
        {
        }
        
        protected override async Task<IAIState> LaunchInternal(CancellationToken token)
        {
            await this.Throttling(token);
            return new EmptyState();
        }
    }
}