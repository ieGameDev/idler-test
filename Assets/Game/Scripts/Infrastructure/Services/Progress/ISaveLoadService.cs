using Game.Scripts.Data;
using Game.Scripts.Infrastructure.DI;

namespace Game.Scripts.Infrastructure.Services.Progress
{
    public interface ISaveLoadService : IService
    {
        void SaveProgress();
        PlayerProgress LoadProgress();
    }
}