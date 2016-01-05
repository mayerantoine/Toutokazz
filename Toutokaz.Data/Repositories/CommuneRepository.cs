using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toutokaz.Data.Interfaces;
using Toutokaz.Domain.Models;

namespace Toutokaz.Data.Repositories
{
    public class CommuneRepository : EntityRepository<tb_commune>,ICommuneRepository
    {
        public IEnumerable<tb_commune> GetSelectedCommune(string searchTerm, int pageSize, int pageNum)
        {
            var result = _db.tb_commune
                               .Where(c => c.commune.Contains(searchTerm))
                               .Take(pageSize);
            return result.ToList();

        }

        public int GetSelectedCommuneCount(string searchTerm, int pageSize, int pageNum)
        {
            var result = _db.tb_commune
                                .Where(c => c.commune.Contains(searchTerm))
                                .Take(pageSize);

            if (result == null)
            {
                return 0;
            }
            
            return result.Count();
                                
        }


        public IEnumerable<tb_commune> GetAllCommune()
        {
            var result = _db.tb_commune.ToList();

            return result;

        }

        public IEnumerable<tb_commune> GetDistrictCommune()
        {
            var result = _db.tb_commune.Where(x => x.district == 1);

            return result.ToList();
        }

        public IEnumerable<tb_departement> GetAllDepartement()
        {
            var result = _db.tb_departement.ToList();

            return result.ToList();

        }

        public IEnumerable<tb_commune> GetCommuneByDepartement(int id)
        {
            var result = _db.tb_commune
                              .Where(c => c.id_departement == id);

            return result.ToList();
        }
    }
}
