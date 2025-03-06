using Game.Scripts.Data;
using Game.Scripts.Infrastructure.DI;

namespace Game.Scripts.Infrastructure.Services.Progress
{
    public interface IProgressService : IService
    {
        PlayerProgress Progress { get; set; }
    }
}