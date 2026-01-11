using System.Collections.Generic;
using System.Threading;
using Services.AI.StateMachine.Transitions;

namespace Services.AI.StateMachine.States
{
    public abstract class SimpleBaseState : BaseState
    {
        protected SimpleBaseState(CancellationToken mainToken, IEnumerable<IStateTransitionFactory> transitionsFactories)
            : base(mainToken, transitionsFactories)
        {
        }

        protected override void BeginInternal()
        {
        }
        
        protected override void EndInternal()
        {
        }
    }
}