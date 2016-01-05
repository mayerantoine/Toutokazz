using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toutokaz.Domain.Models;

namespace Toutokaz.Data.Interfaces
{
    public interface IAccountRepository: IEntityRepository<tb_account>
    {
        tb_account GetUserProfile(Guid userkey);
        tb_account GetAccountByUserId(int id);
    }
}
