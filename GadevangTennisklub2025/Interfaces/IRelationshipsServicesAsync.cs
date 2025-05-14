using GadevangTennisklub2025.Models;

namespace GadevangTennisklub2025.Interfaces
{
    public interface IRelationshipsServicesAsync
    {
        public Task EventMemberRelation(int EventId, int MemberId);
        public Task BookingMemberRelation(int MemberId, int BookingId);

        public Task<bool> MemberAvailible(int memberID, DateTime start, DateTime end);
        public Task<bool> CourtAvailible(int courtID, DateTime start, DateTime end);
        public Task TeamMemberRelation(int TeamId, int MemberId);
        public Task TeamCoachRelation(int TeamId, int CoachId);

        public Task<List<Booking?>> GetBookingsByMemberId(int memberID);
        public Task<TennisField> GetTennisFieldById(int court_Id);
        public Task<string> GetBookingPartnerName(int memberId, int bookingId);
        public Task<List<Event?>> GetEventsByMemberId(int memberID);
        public Task<Event?> GetEventById(int event_Id);
    }
}
