using System;

namespace Services.AI.StateMachine
{
    public interface IStateTransition
    {
        event Action<IAIState> Activated;

        void Initialize();
        void Release();

    }
}