using HEALTH_SUPPORT.Repositories;
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
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IBaseRepository<Account, Guid> _accountRepository;
        private readonly IBaseRepository<Role, Guid> _roleRepository;

        public AccountService(IBaseRepository<Account, Guid> accountRepository, IBaseRepository<Role, Guid> roleRepository)
        {
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
        }

        public async Task AddAccount(AccountRequest.CreateAccountModel model)
        {
            var role = await _roleRepository.GetAll().FirstOrDefaultAsync(r => r.Name == model.RoleName);
            if (role == null)
            {
                throw new Exception("Invalid Role Name");
            }
            try
            {

                var acc = new Account()
                {
                    Id = Guid.NewGuid(),
                    UseName = model.UserName,
                    Fullname = model.Fullname,
                    Email = model.Email,
                    Phone = model.Phone,
                    Address = model.Address,
                    PasswordHash = model.PasswordHash,
                    RoleId = role.Id,
                    CreateAt = DateTimeOffset.UtcNow,
                    LoginDate = DateTimeOffset.UtcNow
                };

                await _accountRepository.Add(acc);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<AccountResponse.GetAccountsModel?> GetAccountById(Guid id)
        {
            var account = await _accountRepository.GetById(id);
            if (account == null || account.IsDeleted)
            {
                return null;
            }
            return new AccountResponse.GetAccountsModel(
                account.Id,
                account.UseName,
                account.Fullname,
                account.Email,
                account.Phone,
                account.Address,
                account.Role?.Name ?? "Unknown"
            );
        }

        public async Task<List<AccountResponse.GetAccountsModel>> GetAccounts()
        {
            return await _accountRepository.GetAll()
                .Where(a => !a.IsDeleted)
                .AsNoTracking()
                .Select(a => new AccountResponse.GetAccountsModel(
                    a.Id,
                    a.UseName,
                    a.Fullname,
                    a.Email,
                    a.Phone,
                    a.Address,
                    a.Role.Name
                ))
                .ToListAsync();
        }

        public async Task RemoveAccount(Guid id)
        {
            var account = await _accountRepository.GetById(id);
            if (account == null)
            {
                throw new InvalidOperationException("Account not found");
            }

            account.IsDeleted = true;
            account.ModifiedAt = DateTimeOffset.UtcNow;

            await _accountRepository.Update(account);
            await _accountRepository.SaveChangesAsync();
        }

        public async Task UpdateAccount(AccountRequest.UpdateAccountModel model)
        {
            try
            {
                var existedAcc = await _accountRepository.GetById(model.AccountId);
                if (existedAcc is null)
                {
                    throw new Exception("Not exist account!");
                }

                //Tracking
                existedAcc.UseName = string.IsNullOrWhiteSpace(model.Username) ? existedAcc.UseName : model.Username;
                existedAcc.Fullname = string.IsNullOrWhiteSpace(model.Fullname) ? existedAcc.Fullname : model.Fullname;
                existedAcc.Email = string.IsNullOrWhiteSpace(model.Email) ? existedAcc.Email : model.Email;
                existedAcc.Phone = string.IsNullOrWhiteSpace(model.Phone) ? existedAcc.Phone : model.Phone;
                existedAcc.Address = string.IsNullOrWhiteSpace(model.Address) ? existedAcc.Address : model.Address;
                existedAcc.PasswordHash = string.IsNullOrWhiteSpace(model.PasswordHash) ? existedAcc.PasswordHash : model.PasswordHash;
                //AsNoTracking
                await _accountRepository.Update(existedAcc);
                await _accountRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
