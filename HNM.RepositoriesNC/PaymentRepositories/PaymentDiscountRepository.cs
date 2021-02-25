using HNM.DataNC.ModelsPayment;
using HNM.RepositoriesNC.PaymentRepositoriesBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.PaymentRepositories
{
    public interface IPaymentDiscountRepository :IPaymentRepositoryBase<DiscountConfig>
    {
        Task<List<DiscountConfig>> GetlstDiscountConfig();
    }
    public class PaymentDiscountRepository : PaymentRepositoryBase<DiscountConfig>, IPaymentDiscountRepository
    {
        public PaymentDiscountRepository(PaymentContext PaymentContext) : base(PaymentContext)
        {

        }
        public async Task<List<DiscountConfig>> GetlstDiscountConfig()
        {
            var output = new List<DiscountConfig>();
            try
            {
                output = await PaymentContext.DiscountConfig.ToListAsync();
            }
            catch (Exception ex)
            {
                return output;
            }
            return output;
        }
    }
}
