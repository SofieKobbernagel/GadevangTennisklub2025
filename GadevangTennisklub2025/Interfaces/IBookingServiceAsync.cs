using GadevangTennisklub2025.Models;

namespace GadevangTennisklub2025.Interfaces
{
    public interface IBookingServiceAsync
    {
        public Task GetBookingsByUser(int uid);
        public Task CreateBooking(Booking b);
        public Task UpdateBooking(Booking b);
        public Task DeleteBooking(Booking b);
        public Task<List<Booking>> GetAllBookings();
    }
}
