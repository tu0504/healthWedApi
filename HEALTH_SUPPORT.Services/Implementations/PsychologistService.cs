using HEALTH_SUPPORT.Repositories.Entities;
using HEALTH_SUPPORT.Repositories.Repository;
using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.RequestModel;
using HEALTH_SUPPORT.Services.ResponseModel;
using Microsoft.EntityFrameworkCore;
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
        public PsychologistService(IBaseRepository<Psychologist, Guid> psychologistRepository)
        {
            _psychologistRepository = psychologistRepository;
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
                CreateAt = DateTimeOffset.UtcNow
            };

            await _psychologistRepository.Add(psychologist);
            await _psychologistRepository.SaveChangesAsync();
        }


        public async Task<PsychologistResponse.GetPsychologistsModel?> GetPsychologistById(Guid id)
        {
            var psychologist = await _psychologistRepository.GetById(id);
            if (psychologist == null || psychologist.IsDeleted)
            {
                return null;
            }

            return new PsychologistResponse.GetPsychologistsModel(
                psychologist.Id,
                psychologist.Name,
                psychologist.Email,
                psychologist.PhoneNumber,
                psychologist.Specialization,
                psychologist.CreateAt
            );
        }

        public async Task<List<PsychologistResponse.GetPsychologistsModel>> GetPsychologists()
        {
            return await _psychologistRepository.GetAll()
                .Where(p => !p.IsDeleted)
                .Select(p => new PsychologistResponse.GetPsychologistsModel(
                    p.Id,
                    p.Name,
                    p.Email,
                    p.PhoneNumber,
                    p.Specialization,
                    p.CreateAt
                ))
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
    }
}
