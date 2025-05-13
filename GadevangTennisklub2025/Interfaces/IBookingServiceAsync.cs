using GadevangTennisklub2025.Models;

namespace GadevangTennisklub2025.Interfaces
{
    public interface IBookingServiceAsync
    {
        public Task<List<Booking>> GetBookingsByUser(int uid);
        public Task CreateBooking(Booking b);
        public Task CreateTeamBooking(Booking b, int TeamId);
        public Task CreateEventBooking(Booking b, int EventId);
        public Task UpdateBooking(Booking b);
        public Task DeleteBooking(Booking b);
        public Task<List<Booking>> GetAllBookings();
        public Task<List<DateTime>> TeamCreation(Team T);
    }
}
