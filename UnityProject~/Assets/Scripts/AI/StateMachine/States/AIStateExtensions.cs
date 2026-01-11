using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace Services.AI.StateMachine.States
{
    public static class AIStateExtensions
    {
        public static Task Throttling(this IAIState state, CancellationToken token)
        {
            return state.WaitWhile(() => true, token);
        }

        public static Task WaitWhile(this IAIState state, Func<bool> condition, CancellationToken token)
        {
            return UniTask.WaitWhile(condition, PlayerLoopTiming.Update, token).SuppressCancellationThrow().AsTask();
        }
        
        public static Task Delay(this IAIState state, float duration, CancellationToken token)
        {
            return UniTask.WaitForSeconds(duration, false, PlayerLoopTiming.Update, token)
                .SuppressCancellationThrow().AsTask(); 
        }
    }
}