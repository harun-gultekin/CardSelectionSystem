using CardSelectionSystem.Core.Models;

namespace CardSelectionSystem.Core.Persistence
{
    public interface ISaveService
    {
        void Save(CycleState state);
        CycleState Load();
        void Delete();
    }
}
