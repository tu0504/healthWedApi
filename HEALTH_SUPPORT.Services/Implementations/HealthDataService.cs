using HEALTH_SUPPORT.Repositories.Entities;
using HEALTH_SUPPORT.Repositories.Repository;
using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.RequestModel;
using HEALTH_SUPPORT.Services.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace HEALTH_SUPPORT.Services.Implementations
{
    public class HealthDataService : IHealthDataService
    {
        private readonly IBaseRepository<HealthData, Guid> _healthDataRepository;
        private readonly IBaseRepository<Account, Guid> _accountRepository;
        private readonly IBaseRepository<Psychologist, Guid> _psychologistRepository;

        public HealthDataService(IBaseRepository<HealthData, Guid> healthDataRepository, IBaseRepository<Account, Guid> accountRepository, IBaseRepository<Psychologist, Guid> psychologistRepository)
        {
            _healthDataRepository = healthDataRepository;
            _accountRepository = accountRepository;
            _psychologistRepository = psychologistRepository;
        }

        public async Task AddHealthData(HealthDataRequest.AddHealthDataRequest model)
        {
            var account = await _accountRepository.GetById(model.AccountId);
            if (account is null)
            {
                throw new Exception("Không tìm thấy người dùng.");
            }
            var psychologist = await _psychologistRepository.GetById(model.PsychologistId);
            if (psychologist is null)
            {
                throw new Exception("Không tìm thấy bác sĩ tâm lý.");
            }

            var healthData = new HealthData
            {
                PsychologistId = model.PsychologistId,
                AccountId = model.AccountId,
                description = model.description,
                FollowUpAppoint = model.FollowUpAppoint,
                level = model.level
            };
            await _healthDataRepository.Add(healthData);
            await _healthDataRepository.SaveChangesAsync();
        }

        public async Task<HealthDataResponse.GetHealthDataModel?> GetHealthDataById(Guid id)
        {
            var healthData = await _healthDataRepository.GetAll().Include(s => s.Account).ThenInclude(s => s.Role).Include(s => s.Psychologist).FirstOrDefaultAsync(s => s.Id == id);
            if (healthData is null)
            {
                throw new Exception("Không tìm thấy dữ liệu.");
            }
            return new HealthDataResponse.GetHealthDataModel
            {
                Id = id,
                description = healthData.description,
                level = healthData.level,
                FollowUpAppoint = healthData.FollowUpAppoint,
                IsDeleted = healthData.IsDeleted,
                Account = new AccountResponse.GetAccountsModel(
                    healthData.Account.Id,
                    healthData.Account.UserName,
                    healthData.Account.Fullname,
                    healthData.Account.Email,
                    healthData.Account.Phone,
                    healthData.Account.Address,
                    healthData.Account.PasswordHash,
                    healthData.Account.Role.Name ?? "Unknown",
                    healthData.Account.ImgUrl
                ),
                Psychologist = new PsychologistResponse.GetPsychologistModel
                {
                    Id = healthData.Psychologist.Id,
                    Name = healthData.Psychologist.Name,
                    Email = healthData.Psychologist.Email,
                    PhoneNumber = healthData.Psychologist.PhoneNumber,
                    Specialization = healthData.Psychologist.Specialization,
                    IsDelete = healthData.Psychologist.IsDeleted
                }
            };

        }

        public async Task<List<HealthDataResponse.GetHealthDataModel>> GetHealthDataByPsychologistId(Guid psychologistId)
        {
            var healthData = await _healthDataRepository.GetAll().Where(s => s.PsychologistId == psychologistId)
                .Include(s => s.Account).ThenInclude(s => s.Role)
                .Include(s => s.Psychologist)
                .Select(s => new HealthDataResponse.GetHealthDataModel
                {
                    Id = s.Id,
                    description = s.description,
                    level = s.level,
                    FollowUpAppoint = s.FollowUpAppoint,
                    IsDeleted = s.IsDeleted,
                    Account = new AccountResponse.GetAccountsModel(
                        s.Account.Id,
                        s.Account.UserName,
                        s.Account.Fullname,
                        s.Account.Email,
                        s.Account.Phone,
                        s.Account.Address,
                        s.Account.PasswordHash,
                        s.Account.Role.Name ?? "Unknown",
                        s.Account.ImgUrl
                    ),
                    Psychologist = new PsychologistResponse.GetPsychologistModel
                    {
                        Id = s.Psychologist.Id,
                        Name = s.Psychologist.Name,
                        Email = s.Psychologist.Email,
                        PhoneNumber = s.Psychologist.PhoneNumber,
                        Specialization = s.Psychologist.Specialization,
                        IsDelete = s.Psychologist.IsDeleted
                    }
                }).ToListAsync();
            return healthData;
        }

        public async Task<List<HealthDataResponse.GetHealthDataModel>> GetHealthDataByAccountId(Guid accountId)
        {
            var healthData = await _healthDataRepository.GetAll().Where(s => s.AccountId == accountId)
                .Include(s => s.Account).ThenInclude(s => s.Role)
                .Include(s => s.Psychologist)
                .Select(s => new HealthDataResponse.GetHealthDataModel
                {
                    Id = s.Id,
                    description = s.description,
                    level = s.level,
                    FollowUpAppoint = s.FollowUpAppoint,
                    IsDeleted = s.IsDeleted,
                    Account = new AccountResponse.GetAccountsModel(
                        s.Account.Id,
                        s.Account.UserName,
                        s.Account.Fullname,
                        s.Account.Email,
                        s.Account.Phone,
                        s.Account.Address,
                        s.Account.PasswordHash,
                        s.Account.Role.Name ?? "Unknown",
                        s.Account.ImgUrl
                    ),
                    
                    Psychologist = new PsychologistResponse.GetPsychologistModel
                    {
                        Id = s.Psychologist.Id,
                        Name = s.Psychologist.Name,
                        Email = s.Psychologist.Email,
                        PhoneNumber = s.Psychologist.PhoneNumber,
                        Specialization = s.Psychologist.Specialization,
                        IsDelete = s.Psychologist.IsDeleted
                    }
                }).ToListAsync();
            return healthData;
        }

        public async Task<List<HealthDataResponse.GetHealthDataModel>> GetHealthDatas()
        {
            var healthData = await _healthDataRepository.GetAll()
                .Include(s => s.Account).ThenInclude(s => s.Role)
                .Include(s => s.Psychologist)
                .Select(s => new HealthDataResponse.GetHealthDataModel
                {
                    Id = s.Id,
                    description = s.description,
                    level = s.level,
                    FollowUpAppoint = s.FollowUpAppoint,
                    IsDeleted = s.IsDeleted,
                    Account = new AccountResponse.GetAccountsModel(
                        s.Account.Id,
                        s.Account.UserName,
                        s.Account.Fullname,
                        s.Account.Email,
                        s.Account.Phone,
                        s.Account.Address,
                        s.Account.PasswordHash,
                        s.Account.Role.Name ?? "Unknown",
                        s.Account.ImgUrl
                    ),
                    Psychologist = new PsychologistResponse.GetPsychologistModel
                    {
                        Id = s.Psychologist.Id,
                        Name = s.Psychologist.Name,
                        Email = s.Psychologist.Email,
                        PhoneNumber = s.Psychologist.PhoneNumber,
                        Specialization = s.Psychologist.Specialization,
                        IsDelete = s.Psychologist.IsDeleted
                    }
                }).ToListAsync();
            return healthData;
        }


        public async Task RemoveHealthData(Guid id)
        {
            var healthData = await _healthDataRepository.GetById(id);
            if (healthData is null)
            {
                throw new Exception("Không tìm thấy dữ liệu.");
            }
            healthData.IsDeleted = true;
            await _healthDataRepository.Update(healthData);
            await _healthDataRepository.SaveChangesAsync();
        }

        public async Task UpdateHealthData(Guid id, HealthDataRequest.UpdateHealthDataRequest model)
        {
            var healthData = await _healthDataRepository.GetById(id);
            if (healthData is null)
            {
                throw new Exception("Không tìm thấy dữ liệu.");
            }
            healthData.description = model.description;
            healthData.level = model.level;
            healthData.FollowUpAppoint = model.FollowUpAppoint;
            await _healthDataRepository.Update(healthData);
            await _healthDataRepository.SaveChangesAsync();
        }
    }
}
