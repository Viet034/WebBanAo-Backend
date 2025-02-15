using Microsoft.EntityFrameworkCore;
using WebBanAoo.Data;
using WebBanAoo.Mapper;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Role;
using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Models.Status;
using WebBanAoo.Models.ultility;
using System.Data;

namespace WebBanAoo.Service.impl
{
    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext _context;
        private IRoleMapper _mapper;

        public RoleService(ApplicationDbContext context, IRoleMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<string> CheckUniqueCodeAsync()
        {
            string newCode;
            bool isExist;

            do
            {
                newCode = GenerateCode.GenerateRoleCode();
                _context.ChangeTracker.Clear();
                isExist = await _context.Roles.AnyAsync(p => p.Code == newCode);
            }
            while (isExist);

            return newCode;
        }

        public async Task<RoleResopnse> CreateRoleAsync(RoleCreate create)
        {
            Role entity = _mapper.CreateToEntity(create);

            if (string.IsNullOrEmpty(entity.Code) || entity.Code == "string")
            {
                entity.Code = await CheckUniqueCodeAsync();
            }
            while (await _context.Roles.AnyAsync(p => p.Code == entity.Code))
            {
                entity.Code = await CheckUniqueCodeAsync();
            }

            await _context.Roles.AddAsync(entity);

            await _context.SaveChangesAsync();
            var response = _mapper.EntityToResponse(entity);
            return response;
        }

        public async Task<RoleResopnse> FindRoleByIdAsync(int id)
        {
            var coId = _context.Roles.FirstOrDefault(co => co.Id == id);
            if (coId == null)
            {
                throw new Exception($" Khong co Id {id} ton tai");
            }
            var response = _mapper.EntityToResponse(coId);
            return response;
        }

        public async Task<IEnumerable<RoleResopnse>> GetAllRoleAsync()
        {
            var co = await _context.Roles.ToListAsync();
            if (co == null) throw new Exception($"Khong co ban ghi nao");

            var response = _mapper.ListEntityToResponse(co);

            return response;
        }

        public async Task<bool> HardDeleteRoleAsync(int id)
        {
            var co = _context.Roles.FirstOrDefault(co => co.Id == id);
            if (co == null)
            {
                throw new Exception($" Khong co Id {id} ton tai");
            }
            _context.Roles.Remove(co);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<RoleResopnse>> SearchRoleByKeyAsync(string key)
        {
            var coKey = await _context.Roles
               .FromSqlRaw("Select * from Roles where Name like {0}", "%" + key + "%").ToListAsync();

            if (coKey == null) throw new Exception($"Khong co Code {key} nao");
            var response = _mapper.ListEntityToResponse(coKey);
            return response;
        }

        public async Task<RoleResopnse> SoftDeleteRoleAsync(int id, Status.RoleStatus newStatus)
        {
            var coId = await _context.Roles.FindAsync(id);
            if (coId == null) throw new Exception($"Khong co Id {id} ton tai");

            coId.Status = newStatus;

            await _context.SaveChangesAsync();

            var response = _mapper.EntityToResponse(coId);
            return response;
        }

        public async Task<RoleResopnse> UpdateRoleAsync(int id, RoleUpdate update)
        {
            var coId = await _context.Roles.FirstOrDefaultAsync(co => co.Id == id);
            if (coId == null)
            {
                throw new Exception($" Khong co Id {id} ton tai");
            }
            if (!string.IsNullOrEmpty(update.Code) && update.Code != "string" && update.Code != coId.Code)
            {
                bool isExist = await _context.Roles.AnyAsync(p => p.Code == update.Code);
                if (isExist)
                {
                    throw new Exception();
                }
                coId.Code = update.Code;
            }
            else
            {
                update.Code = coId.Code;
            }
            var result = _mapper.UpdateToEntity(update);
            
            coId.Status = result.Status;
            coId.Name = result.Name;
            coId.Description = result.Description;
            
            coId.CreateDate = result.CreateDate;
            coId.UpdateDate = result.UpdateDate;
            coId.CreatedBy = result.CreatedBy;
            coId.UpdateBy = result.UpdateBy;


            await _context.SaveChangesAsync();

            var response = _mapper.EntityToResponse(coId);
            return response;
        }
    }
}
