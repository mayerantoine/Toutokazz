using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toutokaz.Data.Interfaces;
using Toutokaz.Domain.Models;

namespace Toutokaz.Data.Repositories
{
    public class AccountRepository : EntityRepository<tb_account>,IAccountRepository
    {
        public tb_account GetUserProfile(Guid userkey)
        {
            var result = (from c in _db.tb_account
                         where c.id_user == userkey
                         select c).SingleOrDefault();
            return result;
        }

        public tb_account GetAccountByUserId(int id) 
        {
            var result = (from c in _db.tb_account
                          where c.UserId == id
                          select c).SingleOrDefault();
            return result;
        }
    }
}
