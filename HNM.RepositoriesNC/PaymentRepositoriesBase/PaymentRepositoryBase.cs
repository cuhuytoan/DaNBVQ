using HNM.DataNC.ModelsPayment;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.PaymentRepositoriesBase
{
    public abstract class PaymentRepositoryBase<T> : IPaymentRepositoryBase<T> where T : class
    {
        protected PaymentContext PaymentContext { get; set; }
        private DbSet<T> table = null;
        public PaymentRepositoryBase(PaymentContext PaymentContext)
        {
            this.PaymentContext = PaymentContext;
            table = PaymentContext.Set<T>();
        }

        public IQueryable<T> FindAll()
        {
            return this.PaymentContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.PaymentContext.Set<T>().Where(expression).AsNoTracking();
        }

        public void Create(T entity)
        {
            this.PaymentContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            this.PaymentContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            this.PaymentContext.Set<T>().Remove(entity);
        }

        public ValueTask<T> FindAsync(object id)
        {
            return this.PaymentContext.Set<T>().FindAsync(id);
        }

        public T GetById(object id)
        {
            return table.Find(id);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> expression)
        {
            return this.PaymentContext.Set<T>().FirstOrDefault(expression);
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await this.PaymentContext.Set<T>().SingleOrDefaultAsync(expression);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await this.PaymentContext.Set<T>().FirstOrDefaultAsync(expression);
        }
    }
}
