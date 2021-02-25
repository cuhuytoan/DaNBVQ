using HNM.DataNC.ModelsPayment;
using HNM.RepositoriesNC.PaymentRepositoriesBase;
using HNM.RepositoriesNC.RepositoriesBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.PaymentRepositories
{
    public interface IPaymentTypeRepository : IPaymentRepositoryBase<PaymentType>
    {
        Task<List<PaymentType>> GetlstPaymentType();
    }
    public class PaymentTypeRepository : PaymentRepositoryBase<PaymentType>, IPaymentTypeRepository
    {
        public PaymentTypeRepository(PaymentContext PaymentContext) : base(PaymentContext)
        {

        }

        public async Task<List<PaymentType>> GetlstPaymentType()
        {
            var output = new List<PaymentType>();
            try
            {
                output = await PaymentContext.PaymentType.ToListAsync();

            }
            catch(Exception ex)
            {
                return output;
            }
            return output;
        }
    }
}
