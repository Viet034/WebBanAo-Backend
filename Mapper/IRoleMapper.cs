using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Role;

namespace WebBanAoo.Mapper
{
    public interface IRoleMapper
    {
        // request => Entity(DTO)
        Role CreateToEntity(RoleCreate create);
        Role UpdateToEntity(RoleUpdate update);
        Role DeleteToEntity(RoleDelete delete);

        // Entity(DTO) => Response
        RoleResopnse EntityToResponse(Role entity);
        IEnumerable<RoleResopnse> ListEntityToResponse(IEnumerable<Role> entities);
    }
}
