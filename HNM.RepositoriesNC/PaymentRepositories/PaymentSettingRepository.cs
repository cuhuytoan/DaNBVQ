using HNM.DataNC.ModelsPayment;
using HNM.RepositoriesNC.PaymentRepositoriesBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.PaymentRepositories
{
    public interface IPaymentSettingRepository : IPaymentRepositoryBase<PaymentSetting>
    {
        Task<PaymentSetting> GetPaymentSetting();
    }
    public class PaymentSettingRepository : PaymentRepositoryBase<PaymentSetting>, IPaymentSettingRepository
    {
        public PaymentSettingRepository(PaymentContext PaymentContext) : base(PaymentContext)
        {

        }
        public async Task<PaymentSetting> GetPaymentSetting()
        {
            var output = new PaymentSetting();
            try
            {
                output = await PaymentContext.PaymentSetting.FirstOrDefaultAsync(p => p.Id == 1);
            }
            catch(Exception ex)
            {
                return output;
            }
            return output;
        }
    }
}
