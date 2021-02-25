using HNM.DataNC.ModelsPayment;
using HNM.RepositoriesNC.PaymentRepositoriesBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.PaymentRepositories
{
    public interface IPaymentBankingRepository : IPaymentRepositoryBase<PaymentBanking>
    {
        Task<List<PaymentBanking>> GetlstPaymentBankingByType(int PaymentTypeId);
    }
    public class PaymentBankingRepository : PaymentRepositoryBase<PaymentBanking>, IPaymentBankingRepository
    {
        public PaymentBankingRepository(PaymentContext PaymentContext) : base(PaymentContext)
        {

        }

        public async Task<List<PaymentBanking>> GetlstPaymentBankingByType(int PaymentTypeId)
        {
            var output = new List<PaymentBanking>();
            try
            {
                output = await PaymentContext.PaymentBanking.Where(p => p.PaymentTypeId == PaymentTypeId).ToListAsync();
            }
            catch(Exception ex)
            {
                return output;
            }
            return output;
        }
    }
}
