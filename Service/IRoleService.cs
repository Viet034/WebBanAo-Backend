using WebBanAoo.Models.DTO.Response;
using static WebBanAoo.Models.Status.Status;
using WebBanAoo.Models.DTO.Request.Role;

namespace WebBanAoo.Service
{
    public interface IRoleService
    {
        public Task<IEnumerable<RoleResopnse>> GetAllRoleAsync();
        public Task<IEnumerable<RoleResopnse>> SearchRoleByKeyAsync(string key);
        public Task<RoleResopnse> UpdateRoleAsync(int id, RoleUpdate update);
        public Task<RoleResopnse> CreateRoleAsync(RoleCreate create);
        public Task<bool> HardDeleteRoleAsync(int id);
        public Task<RoleResopnse> SoftDeleteRoleAsync(int id, RoleStatus newStatus);
        public Task<RoleResopnse> FindRoleByIdAsync(int id);
        public Task<string> CheckUniqueCodeAsync();
    }
}
