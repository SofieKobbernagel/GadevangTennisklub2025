using GadevangTennisklub2025.Models;

namespace GadevangTennisklub2025.Interfaces
{
    public interface IRelationshipsServicesAsync
    {
        public Task EventMemberRelation(int EventId, int MemberId);
        public Task BookingMemberRelation(int MemberId, int BookingId);

        public Task<bool> MemberAvailible(int memberID,DateTime start, DateTime end);
        public Task<bool> CourtAvailible(int courtID,DateTime start, DateTime end);
        public  Task TeamMemberRelation(int TeamId, int MemberId);
        public Task<List<Booking>> GetBookingsByUser(int memberID);
    }
}
