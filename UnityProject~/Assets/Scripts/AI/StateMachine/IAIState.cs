using System.Threading.Tasks;

namespace Services.AI.StateMachine
{
    public interface IAIState
    {
        void Begin();
        Task<IAIState> Launch();
        void Release();
    }
}