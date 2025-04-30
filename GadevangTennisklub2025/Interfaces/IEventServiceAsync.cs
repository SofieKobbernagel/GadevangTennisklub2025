using GadevangTennisklub2025.Models;

namespace GadevangTennisklub2025.Interfaces
{
    public interface IEventServiceAsync
    {
        public Task UpdateEventAsync(Event ev);
        public Task DeleteEventAsync(Event ev);
        public Task CreateEventAsync(Event ev);
        public Task<Event> returnEventAsync(int eid);
        public Task<List<Event>> GetEventsAsync();
        public Task<List<Event>> GetEventspermonthInYearAsync(int month, int year);
    }
}
