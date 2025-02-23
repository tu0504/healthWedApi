using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.ResponseModel
{
    public static class AccountResponse
    {
        public record GetAccountsModel(
            Guid Id,
            string UserName,
            string Fullname,
            string Email, 
            string Phone, 
            string Address,
            //string PasswordHash,
            string RoleName
        );
    }
}
