using HNM.DataNC.Models;
using HNM.DataNC.ModelsPayment;
using HNM.RepositoriesNC.PaymentRepositories;
using HNM.RepositoriesNC.Repositories;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.PaymentRepositoriesBase
{
    public class PaymentRepositoryWrapper : IPaymentRepositoryWrapper
    {
        private PaymentContext _paymentContext;
        private IPaymentTypeRepository _paymentType;
        private IPaymentDiscountRepository _paymentDiscount;
        private IPaymentServiceRepository _paymentService;
        private IPaymentBankingRepository _paymentBanking;
        private IPaymentSettingRepository _paymentSetting;
        private IPaymentUpgradeRepository _paymentUpgrade;
        public PaymentRepositoryWrapper(PaymentContext PaymentContext)
        {
            _paymentContext = PaymentContext;
        }

        public IPaymentTypeRepository PaymentType
        {
            get
            {
                if (_paymentType == null)
                {
                    _paymentType = new PaymentTypeRepository(_paymentContext);
                }

                return _paymentType;
            }
        }

        public IPaymentDiscountRepository PaymentDisCount
        {
            get
            {
                if (_paymentDiscount == null)
                {
                    _paymentDiscount = new PaymentDiscountRepository(_paymentContext);
                }

                return _paymentDiscount;
            }
        }

        public IPaymentServiceRepository PaymentServcie
        {
            get
            {
                if (_paymentService == null)
                {
                    _paymentService = new PaymentServiceRepository(_paymentContext);
                }

                return _paymentService;
            }
        }

        public IPaymentBankingRepository PaymentBanking
        {
            get
            {
                if (_paymentBanking == null)
                {
                    _paymentBanking = new PaymentBankingRepository(_paymentContext);
                }

                return _paymentBanking;
            }
        }

        public IPaymentSettingRepository PaymentSetting
        {
            get
            {
                if (_paymentSetting == null)
                {
                    _paymentSetting = new PaymentSettingRepository(_paymentContext);
                }

                return _paymentSetting;
            }
        }

        public IPaymentUpgradeRepository PaymentUpgrade
        {
            get
            {
                if (_paymentUpgrade == null)
                {
                    _paymentUpgrade = new PaymentUpgradeRepository(_paymentContext);
                }

                return _paymentUpgrade;
            }
        }

        public void Save()
        {
            _paymentContext.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _paymentContext.SaveChangesAsync();
        }
    }
}
