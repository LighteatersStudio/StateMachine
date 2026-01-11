using System.Threading;
using Zenject;

namespace Services.AI.StateMachine.States
{
    public interface IInitAIState : IAIState
    {
        public class Factory : PlaceholderFactory<CancellationToken, IInitAIState>
        {}
    }
}