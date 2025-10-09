// IGroupRepository.cs (SLUTLIG VERSION)
using AlltOmHundar.Core.Interfaces.Repositories;
using AlltOmHundar.Core.Models;

public interface IGroupRepository : IRepository<Group>
{
    Task<IEnumerable<Group>> GetUserGroupsAsync(int userId);
    Task<Group?> GetGroupWithMembersAsync(int groupId);
    Task<bool> IsUserMemberAsync(int groupId, int userId);
    Task<bool> IsAdminOrOwnerAsync(int groupId, int userId);
    Task AddMemberAsync(int groupId, int userId, bool isAdmin = false);
    Task RemoveMemberAsync(int groupId, int targetUserId);

    // Inbjudningar
    Task<GroupInvitation> UpsertInviteAsync(int groupId, int invitedUserId, int invitedByUserId);
    Task<GroupInvitation?> GetInviteAsync(int invitationId);
    Task<IReadOnlyList<GroupInvitation>> GetInvitesForUserAsync(int userId);

    Task SaveAsync();
}
