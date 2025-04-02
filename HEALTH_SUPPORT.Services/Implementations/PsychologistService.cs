using HEALTH_SUPPORT.Repositories.Entities;
using HEALTH_SUPPORT.Repositories.Repository;
using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.RequestModel;
using HEALTH_SUPPORT.Services.ResponseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static HEALTH_SUPPORT.Services.RequestModel.AccountRequest;

namespace HEALTH_SUPPORT.Services.Implementations
{
    public class PsychologistService : IPsychologistService
    {
        private readonly IBaseRepository<Psychologist, Guid> _psychologistRepository;
        private readonly IBaseRepository<Account, Guid> _accountRepository;
        private readonly IBaseRepository<Role, Guid> _roleRepository;
        private readonly IHostEnvironment _environment;
        private readonly IAvatarRepository<Psychologist, Guid> _avatarRepository;

        public PsychologistService(IBaseRepository<Psychologist, Guid> psychologistRepository, IBaseRepository<Role, Guid> roleRepository, IBaseRepository<Account, Guid> accountRepository, IHostEnvironment environment, IAvatarRepository<Psychologist,Guid> avatarRepository)
        {
            _psychologistRepository = psychologistRepository;
            _environment = environment;
            _avatarRepository = avatarRepository;
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
        }

        public async Task AddPsychologist(PsychologistRequest.CreatePsychologistModel model)
        {
            var existingPsychologist = await _psychologistRepository.GetAll()
                .AnyAsync(p => p.Name == model.Name && !p.IsDeleted);

            if (existingPsychologist)
            {
                throw new InvalidOperationException("A psychologist with the same name already exists.");
            }

            var psychologist = new Psychologist
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Specialization = model.Specialization,
                Description = model.Description,
                Achievements = model.Achievements,
                CreateAt = DateTimeOffset.UtcNow
            };

            await _psychologistRepository.Add(psychologist);
            await _psychologistRepository.SaveChangesAsync();
        }


        public async Task<PsychologistResponse.GetPsychologistModel?> GetPsychologistById(Guid id)
        {
            var psychologist = await _psychologistRepository.GetById(id);
            if (psychologist == null || psychologist.IsDeleted)
            {
                return null;
            }

            return new PsychologistResponse.GetPsychologistModel
            {
                Id = psychologist.Id,
                Name = psychologist.Name,
                Email = psychologist.Email,
                PhoneNumber = psychologist.PhoneNumber,
                Specialization = psychologist.Specialization,
                Description = psychologist.Description,
                Achievements = psychologist.Achievements,
                ImgUrl = psychologist.ImgUrl,
                IsDeleted = psychologist.IsDeleted
            };
        }

        public async Task<List<PsychologistResponse.GetPsychologistModel>> GetPsychologists()
        {
            return await _psychologistRepository.GetAll()
            .Where(p => !p.IsDeleted)
            .Select(p => new PsychologistResponse.GetPsychologistModel
            {
                Id = p.Id,
                Name = p.Name,
                Email = p.Email,
                PhoneNumber = p.PhoneNumber,
                Specialization = p.Specialization,
                Description = p.Description,
                Achievements = p.Achievements,
                ImgUrl = p.ImgUrl,
                IsDeleted = p.IsDeleted
            })
    .ToListAsync();
        }

        public async Task UpdatePsychologist(Guid id, PsychologistRequest.UpdatePsychologistModel model)
        {
            var psychologist = await _psychologistRepository.GetById(id);
            if (psychologist == null || psychologist.IsDeleted)
            {
                throw new InvalidOperationException("Psychologist not found.");
            }

            psychologist.Name = model.Name ?? psychologist.Name;
            psychologist.Email = model.Email ?? psychologist.Email;
            psychologist.PhoneNumber = model.PhoneNumber ?? psychologist.PhoneNumber;
            psychologist.Specialization = model.Specialization ?? psychologist.Specialization;
            psychologist.Description = model.Description ?? psychologist.Description;
            psychologist.Achievements = model.Achievements ?? psychologist.Achievements;
            psychologist.ModifiedAt = DateTimeOffset.UtcNow;

            await _psychologistRepository.Update(psychologist);
            await _psychologistRepository.SaveChangesAsync();
        }

        public async Task RemovePsychologist(Guid id)
        {
            var psychologist = await _psychologistRepository.GetById(id);
            if (psychologist == null || psychologist.IsDeleted)
            {
                throw new InvalidOperationException("Psychologist not found.");
            }

            psychologist.IsDeleted = true;
            psychologist.ModifiedAt = DateTimeOffset.UtcNow;

            await _psychologistRepository.Update(psychologist);
            await _psychologistRepository.SaveChangesAsync();
        }

        public async Task<PsychologistResponse.AvatarResponseModel> UploadAvatarAsync(Guid id, PsychologistRequest.UploadAvatarModel model)
        {
            var p = await _psychologistRepository.GetById(id);
            if (p == null || p.IsDeleted) throw new Exception("Psychologist not found");

            // Vì IHostEnvironment không có WebRootPath, cần tự tạo đường dẫn wwwroot
            string uploadsFolder = Path.Combine(_environment.ContentRootPath, "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            string fileName = $"{Guid.NewGuid()}_{model.File.FileName}";
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            string relativePath = $"/uploads/{fileName}";
            await _avatarRepository.UpdateAvatarAsync(id, relativePath);

            return new PsychologistResponse.AvatarResponseModel { AvatarUrl = relativePath };
        }

        public async Task<PsychologistResponse.AvatarResponseModel> UpdateAvatarAsync(Guid id, PsychologistRequest.UploadAvatarModel model)
        {
            var p = await _psychologistRepository.GetById(id);
            if (p == null || p.IsDeleted) throw new Exception("Psychologist not found");

            if (!string.IsNullOrEmpty(p.ImgUrl))
            {
                string oldFilePath = Path.Combine(_environment.ContentRootPath, "wwwroot", p.ImgUrl.TrimStart('/'));
                if (File.Exists(oldFilePath)) File.Delete(oldFilePath);
            }
            p.ModifiedAt = DateTimeOffset.UtcNow;
            return await UploadAvatarAsync(id, model);
        }

        public async Task RemoveAvatarAsync(Guid id)
        {
            var p = await _psychologistRepository.GetById(id);
            if (p == null || p.IsDeleted) throw new Exception("Psychologist not found");

            if (!string.IsNullOrEmpty(p.ImgUrl))
            {
                string filePath = Path.Combine(_environment.ContentRootPath, "wwwroot", p.ImgUrl.TrimStart('/'));
                if (File.Exists(filePath)) File.Delete(filePath);

                await _avatarRepository.UpdateAvatarAsync(id, null);
            }
        }
        public async Task<List<PsychologistResponse.GetPsychologistModel>> GetPsychologistDetailByManager()
        {
            var psychologists = await _psychologistRepository.GetAll()
                .Where(p => !p.IsDeleted) 
                .Select(p => new PsychologistResponse.GetPsychologistModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Email = p.Email,
                    PhoneNumber = p.PhoneNumber,
                    Specialization = p.Specialization,
                    Description = p.Description,
                    Achievements = p.Achievements,
                    ImgUrl = p.ImgUrl,
                    IsDeleted = p.IsDeleted
                })
                .ToListAsync();

            return psychologists;
        }
        public async Task<List<PsychologistResponse.GetPsychologistWithAccountModel>> GetPsychologistProfileByManager()
        {
            var psychologists = await _psychologistRepository.GetAll()
                .Where(p => !p.IsDeleted)
                .Include(p => p.Account) // Include thông tin Account
                .Select(p => new PsychologistResponse.GetPsychologistWithAccountModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Email = p.Email,
                    PhoneNumber = p.PhoneNumber,
                    Specialization = p.Specialization,
                    Description = p.Description,
                    Achievements = p.Achievements,
                    ImgUrl = p.ImgUrl,
                    IsDeleted = p.IsDeleted,

                   
                    Account = p.Account != null ? new PsychologistResponse.AccountModel
                    {
                        Id = p.Account.Id,
                        Username = p.Account.UserName,
                        Email = p.Account.Email,
                        Phone = p.Account.Phone,
                        Address = p.Account.Address
                    } : null
                })
                .ToListAsync();

            return psychologists;
        }

        public async Task AddPsychologistToManager(CreateAccountAndPsychologistModel model)
        {
            var existingUser = await _accountRepository.GetAll().AnyAsync(r => r.Email == model.Email);
            if (existingUser)
            {
                throw new Exception("Email đã được sử dụng.");
            }

         
            var role = await _roleRepository.GetAll().FirstOrDefaultAsync(r => r.Name == model.RoleName);
            if (role == null)
            {
                throw new Exception("Vui lòng chọn vai trò.");
            }

         
            if (model.PasswordHash != model.ConfirmPassword)
            {
                throw new Exception("Mật khẩu nhập lại không khớp!");
            }

           
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.PasswordHash);

            var account = new Account()
            {
                Id = Guid.NewGuid(),
                UserName = model.UserName,
                Fullname = model.Fullname,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                PasswordHash = passwordHash,
                RoleId = role.Id,
                CreateAt = DateTimeOffset.UtcNow,
            };

            await _accountRepository.Add(account);
            await _accountRepository.SaveChangesAsync();

            var psychologist = new Psychologist
            {
                Id = Guid.NewGuid(),
                Name = account.UserName,
                Email = account.Email,
                PhoneNumber = account.Phone,
                Specialization = model.Specialization,
                Description = model.Description,
                Achievements = model.Achievements,
                CreateAt = DateTimeOffset.UtcNow,
                AccountId = account.Id, 
            };

            await _psychologistRepository.Add(psychologist);
            await _psychologistRepository.SaveChangesAsync();
        }

        public async Task UpdatePsychologistToManager(Guid id, PsychologistRequest.UpdatePsychologistModel model)
        {
            var psychologist = await _psychologistRepository.GetById(id);
            if (psychologist == null || psychologist.IsDeleted)
            {
                throw new InvalidOperationException("Psychologist not found.");
            }

            var account = await _accountRepository.GetById(psychologist.AccountId);

            // Kiểm tra email đã tồn tại (nếu có cập nhật email)
            if (!string.IsNullOrEmpty(model.Email) && account != null)
            {
                var existingEmail = await _accountRepository.GetAll()
                    .AnyAsync(a => a.Email == model.Email && a.Id != account.Id);
                if (existingEmail)
                {
                    throw new InvalidOperationException("Email đã được sử dụng.");
                }
            }

            
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                psychologist.Name = model.Name;
                if (account != null) account.UserName = model.Name; 
            }

            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                psychologist.Email = model.Email;
                if (account != null) account.Email = model.Email;
            }

            if (!string.IsNullOrWhiteSpace(model.PhoneNumber))
            {
                psychologist.PhoneNumber = model.PhoneNumber;
                if (account != null) account.Phone = model.PhoneNumber;
            }

           
            psychologist.Specialization = model.Specialization ?? psychologist.Specialization;
            psychologist.Description = model.Description ?? psychologist.Description;
            psychologist.Achievements = model.Achievements ?? psychologist.Achievements;
            psychologist.ModifiedAt = DateTimeOffset.UtcNow;

            if (account != null)
            {
                account.ModifiedAt = DateTimeOffset.UtcNow;
                await _accountRepository.Update(account);
                await _accountRepository.SaveChangesAsync();
            }

            await _psychologistRepository.Update(psychologist);
            await _psychologistRepository.SaveChangesAsync();
        }

        public async Task DeletePsychologistById(Guid id)
        {
            // Lấy thông tin psychologist
            var psychologist = await _psychologistRepository.GetById(id);
            if (psychologist == null || psychologist.IsDeleted)
            {
                throw new InvalidOperationException("Psychologist not found or already deleted.");
            }

           
            var account = await _accountRepository.GetById(psychologist.AccountId);

          
            psychologist.IsDeleted = true;
            psychologist.ModifiedAt = DateTimeOffset.UtcNow;
            await _psychologistRepository.Update(psychologist);

            if (account != null && !account.IsDeleted) 
            {
                account.IsDeleted = true;
                account.ModifiedAt = DateTimeOffset.UtcNow;
                await _accountRepository.Update(account);
            }

           
            await _psychologistRepository.SaveChangesAsync();
            if (account != null)
            {
                await _accountRepository.SaveChangesAsync();
            }
        }


    }
}
