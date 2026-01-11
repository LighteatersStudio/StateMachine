using System.Threading;

namespace Services.AI.StateMachine.Transitions
{
    public interface IStateTransitionFactory
    {
        IStateTransition Create(CancellationToken token);
    }
}