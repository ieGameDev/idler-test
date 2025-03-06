using Game.Scripts.Data;

namespace Game.Scripts.Infrastructure.Services.Progress
{
    public interface ISavedProgress : ISavedProgressReader
    {
        void SaveProgress(PlayerProgress progress);
    }
    
    public interface ISavedProgressReader
    {
        void LoadProgress(PlayerProgress progress);
    }
}