using Web.Models;

namespace Web.Services
{
    public interface IWarSessionManager
    {
        void Add(WarSession session);
        void Clean();
        void Remove(string userId);
        WarSession Find(string userId);
    }
}