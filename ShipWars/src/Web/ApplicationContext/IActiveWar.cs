using Web.Models;

namespace Web.ApplicationContext
{
    public interface IActiveWar
    {
        War TheWar { get; }
        bool Exists { get; }
        string CompetitorId { get; }
    }
}
