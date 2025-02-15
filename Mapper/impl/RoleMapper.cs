using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Role;
using WebBanAoo.Models.DTO.Response;

namespace WebBanAoo.Mapper.impl
{
    public class RoleMapper : IRoleMapper
    {
        private readonly Role role = new Role();
        public Role CreateToEntity(RoleCreate create)
        {
            role.Code = create.Code;
            role.Name = create.Name;
            role.Description = create.Description;
            role.Status = create.Status;
            role.CreatedBy = "System";
            role.CreateDate = DateTime.Now.AddHours(7);
            role.UpdateDate = DateTime.Now.AddHours(7);
            role.UpdateBy = "System";
            return role;
        }

        public Role DeleteToEntity(RoleDelete delete)
        {
            role.Id = delete.Id;
            role.Code = delete.Code;
            role.Name = delete.Name;
            role.Description = delete.Description;
            role.Status = delete.Status;
            role.CreatedBy = "System";
            role.CreateDate = DateTime.Now.AddHours(7);
            role.UpdateDate = DateTime.Now.AddHours(7);
            role.UpdateBy = "System";
            return role;
        }

        public RoleResopnse EntityToResponse(Role entity)
        {
           RoleResopnse response = new RoleResopnse();
            response.Id = entity.Id;
            response.Code = entity.Code;
            response.Name = entity.Name;
            response.Description = entity.Description;
            response.Status = entity.Status;
            role.CreatedBy = "System";
            role.CreateDate = DateTime.Now.AddHours(7);
            role.UpdateDate = DateTime.Now.AddHours(7);
            role.UpdateBy = "System";
            return response;
        }

        public IEnumerable<RoleResopnse> ListEntityToResponse(IEnumerable<Role> entities)
        {
            return entities.Select(x => EntityToResponse(x)).ToList();
        }

        public Role UpdateToEntity(RoleUpdate update)
        {
            role.Code = update.Code;
            role.Name = update.Name;
            role.Description = update.Description;
            role.Status = update.Status;
            role.CreatedBy = "System";
            role.CreateDate = DateTime.Now.AddHours(7);
            role.UpdateDate = DateTime.Now.AddHours(7);
            role.UpdateBy = "System";
            return role;
        }
    }
}
