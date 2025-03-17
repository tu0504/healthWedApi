using HEALTH_SUPPORT.Repositories.Entities;
using HEALTH_SUPPORT.Repositories.Repository;
using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.RequestModel;
using HEALTH_SUPPORT.Services.ResponseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.Implementations
{
    public class PsychologistService : IPsychologistService
    {
        private readonly IBaseRepository<Psychologist, Guid> _psychologistRepository;

        private readonly IHostEnvironment _environment;
        private readonly IAvatarRepository<Psychologist, Guid> _avatarRepository;

        public PsychologistService(IBaseRepository<Psychologist, Guid> psychologistRepository, IHostEnvironment environment, IAvatarRepository<Psychologist,Guid> avatarRepository)
        {
            _psychologistRepository = psychologistRepository;
            _environment = environment;
            _avatarRepository = avatarRepository;
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
    }
}
