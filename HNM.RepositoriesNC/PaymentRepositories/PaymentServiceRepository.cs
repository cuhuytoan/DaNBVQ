using HNM.DataNC.ModelsPayment;
using HNM.RepositoriesNC.PaymentRepositoriesBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.PaymentRepositories
{
    public interface IPaymentServiceRepository : IPaymentRepositoryBase<HNM.DataNC.ModelsPayment.Services>
    {
        Task<List<HNM.DataNC.ModelsPayment.Services>> GetServices();
    }
    public class PaymentServiceRepository : PaymentRepositoryBase<HNM.DataNC.ModelsPayment.Services>, IPaymentServiceRepository
    {
        public PaymentServiceRepository(PaymentContext PaymentContext) : base(PaymentContext)
        {

        }
        public async Task<List<Services>> GetServices()
        {
            var output = new List<Services>();
            try
            {
                output = await PaymentContext.Services.ToListAsync();
            }
            catch(Exception ex)
            {
                return output;
            }
            return output;
        }
    }
}
