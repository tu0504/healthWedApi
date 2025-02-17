using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Repositories.Abstractions
{
<<<<<<< HEAD
    public abstract class Entity<T>
=======
    public class Entity<T>
>>>>>>> 9bf8b07 (update entity)
    {
        public T Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
