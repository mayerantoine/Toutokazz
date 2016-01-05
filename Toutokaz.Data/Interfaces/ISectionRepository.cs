using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toutokaz.Domain.Models;

namespace Toutokaz.Data.Interfaces
{
    public interface ISectionRepository:IEntityRepository<tb_section>
    {
        IEnumerable<tb_category> GetCategoryBySectionId(int Id);
    }
}
