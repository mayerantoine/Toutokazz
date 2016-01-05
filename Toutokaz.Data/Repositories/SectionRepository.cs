using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toutokaz.Data.Interfaces;
using Toutokaz.Domain.Models;

namespace Toutokaz.Data.Repositories
{
    public class SectionRepository:EntityRepository<tb_section>, ISectionRepository
    {
        public IEnumerable<tb_category> GetCategoryBySectionId(int Id)
        {
            /*var q = from c in _db.tb_category.Include("tb_section")
                    orderby c.category_order
                    where c.id_section == Id
                    select c;*/

            IEnumerable<tb_category> q  = _db.tb_category.Include("tb_section").Where(x => x.id_section == Id).OrderBy(c => c.category_order);
            return q;
        }
    }
}
