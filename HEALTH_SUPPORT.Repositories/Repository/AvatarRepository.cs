using HEALTH_SUPPORT.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Repositories.Repository
{
    public class AvatarRepository<TEntity, TKey> : IAvatarRepository<TEntity, TKey> where TEntity : Entity<TKey>
    {
        private readonly ApplicationDbContext _context;

        public AvatarRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task UpdateAvatarAsync(TKey id, string avatarPath)
        {
            var entity = await _context.Accounts.FindAsync(id);
            if (entity != null)
            {
                entity.ImgUrl = avatarPath;
                await _context.SaveChangesAsync();
            }
        }
    }
    
}
