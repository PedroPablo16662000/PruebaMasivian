using Backend.RuletaMasivian.Entities.ModelsAdmin;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Backend.RuletaMasivian.Entities.Interface.RepositoryAdmin
{
    public interface IRolRepository
    {
        Task<List<Rol>> GetRolesIds(Expression<Func<Rol, bool>> expression);
    }
}
