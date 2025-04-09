using HEALTH_SUPPORT.Repositories.Entities;
using HEALTH_SUPPORT.Repositories.Repository;
using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.RequestModel;
using HEALTH_SUPPORT.Services.ResponseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IBaseRepository<Appointment, Guid> _appointmentRepository;
        private readonly IBaseRepository<Psychologist, Guid> _psychologistRepository;
        private readonly IBaseRepository<Account, Guid> _accountRepository;

        public AppointmentService(IBaseRepository<Appointment, Guid> appointmentRepository, IBaseRepository<Psychologist, Guid> psychologistRepository, IBaseRepository<Account, Guid> accountRepository)
        {
            _appointmentRepository = appointmentRepository;
            _psychologistRepository = psychologistRepository;
            _accountRepository = accountRepository;
        }

        public async Task AddAppointment(AppointmentRequest.AddAppointmentRequestRequest model)
        {
            var psychologist = await _psychologistRepository.GetById(model.PsychologistId);
            if (psychologist is null || psychologist.IsDeleted)
            {
                throw new Exception("Không tìm thấy bác sĩ tâm lý.");
            }
            if ((model.AppointmentDate - DateTime.Now).TotalHours < 24)
            {
                throw new Exception("Lịch hẹn phải được đặt trước ít nhất 24 giờ.");
            }

            bool isBooked = await _appointmentRepository.GetAll().AnyAsync(a => a.PsychologistId ==model.PsychologistId && a.AppointmentDate == model.AppointmentDate);
            if (isBooked)
            {
                throw new Exception("Khung giờ này đã có người đặt lịch với bác sĩ.");
            }

            Appointment appointment = new Appointment
            {
                AccountId = model.AccountId,
                CreateAt = DateTime.Now,
                PsychologistId = model.PsychologistId,
                AppointmentDate = model.AppointmentDate,
                Content = model.Content,
                Status = model.Status
            };
            await _appointmentRepository.Add(appointment);
            await _appointmentRepository.SaveChangesAsync();
        }

        public async Task<List<AppointmentResponse.GetAppointmentModel>> GetAppointment()
        {

            return await _appointmentRepository.GetAll().Where(s => !s.IsDeleted).AsNoTracking().
                    Select(s => new AppointmentResponse.GetAppointmentModel
                    {
                        Id = s.Id,
                        AccountId = s.AccountId,
                        Account = new AppointmentResponse.GetAccountsForAppointmentModel
                        {
                            Id = s.AccountId,
                            Address = s.Account.Address,
                            Email = s.Account.Email,
                            Fullname = s.Account.Fullname,
                            Phone = s.Account.Phone,
                            UserName = s.Account.UserName
                        },
                        CreateAt = s.CreateAt,
                        ModifiedAt = s.ModifiedAt,
                        AppointmentDate = s.AppointmentDate,
                        Content = s.Content,
                        IsDelete = s.IsDeleted,
                        PsychologistId = s.PsychologistId,
                        Psychologist = new PsychologistResponse.GetPsychologistModel
                        {
                            IsDeleted = s.Psychologist.IsDeleted,
                            Email = s.Psychologist.Email,
                            Id = s.Psychologist.Id,
                            Name = s.Psychologist.Name,
                            PhoneNumber = s.Psychologist.PhoneNumber,
                            UrlMeet = s.Psychologist.UrlMeet,
                            Specialization = s.Psychologist.Specialization
                        },
                        Status = s.Status
                    }).ToListAsync(); 
        }

        public async Task<AppointmentResponse.GetAppointmentModel?> GetAppointmentById(Guid id)
        {
            var appointment = await _appointmentRepository.GetById(id);
            if (appointment is null || appointment.IsDeleted)
            {
                throw new Exception("Không tìm thấy lịch hẹn.");
            }
            var psychologist = await _psychologistRepository.GetById(appointment.PsychologistId);
            if (psychologist is null || psychologist.IsDeleted)
            {
                throw new Exception("Không tìm thấy bác sĩ tâm lý.");
            }
            var account = await _accountRepository.GetById(appointment.AccountId);
            if (account is null || account.IsDeleted)
            {
                throw new Exception("Không tìm thấy người dùng.");
            }
            return new AppointmentResponse.GetAppointmentModel
            {
                Id = id,
                AccountId = appointment.AccountId,
                Account = new AppointmentResponse.GetAccountsForAppointmentModel
                {
                    Id = appointment.AccountId,
                    Address = account.Address,
                    Email = account.Email,
                    Fullname = account.Fullname,
                    Phone = account.Phone,
                    UserName = account.UserName
                },
                CreateAt = appointment.CreateAt,
                ModifiedAt = appointment.ModifiedAt,
                AppointmentDate = appointment.AppointmentDate,
                Content = appointment.Content,
                IsDelete = appointment.IsDeleted,
                PsychologistId = appointment.PsychologistId,
                Status = appointment.Status,
                Psychologist = new PsychologistResponse.GetPsychologistModel
                {
                    IsDeleted = psychologist.IsDeleted,
                    Email = psychologist.Email,
                    Id = psychologist.Id,
                    Name = psychologist.Name,
                    PhoneNumber = psychologist.PhoneNumber,
                    UrlMeet = psychologist.UrlMeet,
                    Specialization = psychologist.Specialization
                }
            };
        }

        public async Task<List<AppointmentResponse.GetAppointmentModel>> GetAppointmentsByPsychologistName(string psychologistName)
        {
            // Lấy danh sách tất cả bác sĩ tâm lý
            var allPsychologists = await _psychologistRepository.GetAll().ToListAsync();

            // Lọc danh sách bác sĩ theo tên
            var psychologists = allPsychologists
                .Where(p => p.Name.ToLower().Contains(psychologistName.ToLower()) && !p.IsDeleted)
                .ToList();

            if (!psychologists.Any())
            {
                throw new Exception("Không tìm thấy bác sĩ tâm lý.");
            }

            // Lấy danh sách ID của các bác sĩ đã lọc
            var psychologistIds = psychologists.Select(p => p.Id).ToList();

            // Lấy tất cả lịch hẹn, sau đó lọc theo danh sách ID bác sĩ
            var allAppointments = await _appointmentRepository.GetAll().ToListAsync();
            var appointments = allAppointments
                .Where(a => psychologistIds.Contains(a.PsychologistId) && !a.IsDeleted)
                .ToList();

            if (!appointments.Any())
            {
                throw new Exception("Không tìm thấy lịch hẹn.");
            }

            var response = new List<AppointmentResponse.GetAppointmentModel>();

            foreach (var appointment in appointments)
            {
                var psychologist = psychologists.FirstOrDefault(p => p.Id == appointment.PsychologistId);
                if (psychologist == null) continue;

                var account = await _accountRepository.GetById(appointment.AccountId);
                if (account == null || account.IsDeleted) continue;

                response.Add(new AppointmentResponse.GetAppointmentModel
                {
                    Id = appointment.Id,
                    AccountId = appointment.AccountId,
                    Account = new AppointmentResponse.GetAccountsForAppointmentModel
                    {
                        Id = account.Id,
                        Address = account.Address,
                        Email = account.Email,
                        Fullname = account.Fullname,
                        Phone = account.Phone,
                        UserName = account.UserName
                    },
                    CreateAt = appointment.CreateAt,
                    ModifiedAt = appointment.ModifiedAt,
                    AppointmentDate = appointment.AppointmentDate,
                    Content = appointment.Content,
                    IsDelete = appointment.IsDeleted,
                    PsychologistId = appointment.PsychologistId,
                    Status = appointment.Status,
                    Psychologist = new PsychologistResponse.GetPsychologistModel
                    {
                        IsDeleted = psychologist.IsDeleted,
                        Email = psychologist.Email,
                        Id = psychologist.Id,
                        Name = psychologist.Name,
                        PhoneNumber = psychologist.PhoneNumber,
                        UrlMeet = psychologist.UrlMeet,
                        Specialization = psychologist.Specialization
                    }
                });
            }

            return response;
        }


        public async Task<List<AppointmentResponse.GetAppointmentModel>> GetAppointmentsForAccount(Guid accountId)
        {
            var appointment = _appointmentRepository.GetAll().Where(s => s.AccountId == accountId && s.IsDeleted == false).Select(s => new AppointmentResponse.GetAppointmentModel
            {
                Id = s.Id,
                AccountId = s.AccountId,
                CreateAt = s.CreateAt,
                ModifiedAt = s.ModifiedAt,
                AppointmentDate = s.AppointmentDate,
                Content = s.Content,
                IsDelete = s.IsDeleted,
                PsychologistId = s.PsychologistId,
                Status = s.Status
            }).ToList();
            if (!appointment.Any())
            {
                throw new Exception("Không tìm thấy lịch hẹn.");
            }
            var psychologistIdList = appointment.Select(s => s.PsychologistId).ToList();
            var psychologist = _psychologistRepository.GetAll().Where(s => psychologistIdList.Contains(s.Id)).Distinct().Select(s => new PsychologistResponse.GetPsychologistModel
            {
                IsDeleted = s.IsDeleted,
                Email = s.Email,
                Id = s.Id,
                Name = s.Name,
                PhoneNumber = s.PhoneNumber,
                UrlMeet = s.UrlMeet,
                Specialization = s.Specialization
            }).ToList();
            if (!psychologist.Any())
            {
                throw new Exception("Không tìm thấy bác sĩ tâm lý.");
            }
            foreach(var item in appointment)
            {
                item.Psychologist = psychologist.FirstOrDefault(s => s.Id == item.PsychologistId);
            }
            return appointment;
        }

        public async Task<List<AppointmentResponse.GetAppointmentModel>> GetAppointmentsForPsychologist(Guid psychologistId)
        {
            var appointment = _appointmentRepository.GetAll().Where(s => s.PsychologistId == psychologistId && s.IsDeleted==false).Select(s => new AppointmentResponse.GetAppointmentModel
            {
                Id = s.Id,
                AccountId = s.AccountId,
                CreateAt = s.CreateAt,
                ModifiedAt = s.ModifiedAt,
                AppointmentDate = s.AppointmentDate,
                IsDelete = s.IsDeleted,
                PsychologistId = s.PsychologistId,
                Content = s.Content,
                Status = s.Status
            }).ToList();
            if (!appointment.Any())
            {
                throw new Exception("Không tìm thấy lịch hẹn.");
            }
            var accountIdList = appointment.Select(s => s.AccountId).ToList();
            var account = _accountRepository.GetAll().Where(s => accountIdList.Contains(s.Id)).Distinct().Select(s => new AppointmentResponse.GetAccountsForAppointmentModel
            {
                Id = s.Id,
                Address = s.Address,
                Email = s.Email,
                Fullname = s.Fullname,
                Phone = s.Phone,
                UserName = s.UserName
            }).ToList();
            if (!account.Any())
            {
                throw new Exception("Không tìm thấy bác sĩ tâm lý.");
            }
            foreach (var item in appointment)
            {
                item.Account = account.FirstOrDefault(s => s.Id == item.AccountId);
            }
            return appointment;
        }

        public async Task RemoveAppointment(Guid id)
        {
            var appointment = await _appointmentRepository.GetById(id);
            if (appointment is null)
            {
                throw new Exception("Không tìm thấy lịch hẹn.");
            }
            appointment.IsDeleted = true;
            appointment.ModifiedAt = DateTime.Now;
            await _appointmentRepository.Update(appointment);
            await _appointmentRepository.SaveChangesAsync();
        }

        public async Task UpdateAppointment(Guid id, AppointmentRequest.EditAppointmentRequestRequest model)
        {
            var appointment = await _appointmentRepository.GetById(id);
            if (appointment is null)
            {
                throw new Exception("Không tìm thấy lịch hẹn.");
            }
            appointment.AccountId = model.AccountId;
            appointment.PsychologistId = model.PsychologistId;
            appointment.ModifiedAt = DateTime.Now;
            appointment.AppointmentDate = model.AppointmentDate;
            appointment.Content = model.Content;
            appointment.Status = model.Status;
            await _appointmentRepository.Update(appointment);
            await _appointmentRepository.SaveChangesAsync();
        }
    }
}
