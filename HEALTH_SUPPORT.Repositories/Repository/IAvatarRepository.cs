using HEALTH_SUPPORT.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Repositories.Repository
{
    public interface IAvatarRepository<TEntity, TKey> where TEntity : Entity<TKey>
    {
        Task UpdateAvatarAsync(TKey id, string avatarPath);
    }
}
