using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toutokaz.Domain.Models;

namespace Toutokaz.Data.Interfaces
{
    public abstract class EntityRepository<T> : IEntityRepository<T> where T : class
    {
        internal toutokaz_dbEntities _db;
        internal DbSet<T> dbSet;

        public EntityRepository()
        {
            _db = new toutokaz_dbEntities();
            dbSet = _db.Set<T>();
        }

        public virtual void Add(T Entity)
        {
            dbSet.Add(Entity);
        }

        public virtual void Update(T Entity)
        {
            _db.Entry(Entity).State = EntityState.Modified;
        }

        public virtual void Delete(T Entity)
        {
            dbSet.Remove(Entity);
        }

        public virtual void Delete(Func<T, Boolean> where)
        {
            IEnumerable<T> objects = dbSet.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
            {
                dbSet.Remove(obj);

            }
        }

        public virtual T GetById(long Id)
        {
            return dbSet.Find(Id);
        }

        public virtual IEnumerable<T> GetAll()
        {

            return dbSet.ToList();

        }

        public virtual IEnumerable<T> GetMany(Func<T, Boolean> where)
        {
            return dbSet.Where<T>(where).ToList();
        }

        public virtual T Get(Func<T, Boolean> where)
        {
            return dbSet.Where<T>(where).FirstOrDefault();
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
