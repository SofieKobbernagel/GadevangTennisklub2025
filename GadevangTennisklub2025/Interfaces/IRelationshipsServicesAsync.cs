namespace GadevangTennisklub2025.Interfaces
{
    public interface IRelationshipsServicesAsync
    {
        public Task EventMemberRelation(int EventId, int MemberId);
    }
}
