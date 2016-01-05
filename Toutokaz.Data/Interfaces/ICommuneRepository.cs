using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toutokaz.Domain.Models;

namespace Toutokaz.Data.Interfaces
{
    public interface ICommuneRepository: IEntityRepository<tb_commune>
    {
        IEnumerable<tb_commune> GetSelectedCommune(string searchTerm, int pageSize, int pageNum);
        int GetSelectedCommuneCount(string searchTerm, int pageSize, int pageNum);
        IEnumerable<tb_departement> GetAllDepartement();
        IEnumerable<tb_commune> GetCommuneByDepartement(int id);
        IEnumerable<tb_commune>  GetAllCommune();
        IEnumerable<tb_commune> GetDistrictCommune();
    }
}
