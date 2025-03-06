using Game.Scripts.Data;

namespace Game.Scripts.Infrastructure.Services.Progress
{
    public class ProgressService : IProgressService
    {
        public PlayerProgress Progress { get; set; }
    }
}