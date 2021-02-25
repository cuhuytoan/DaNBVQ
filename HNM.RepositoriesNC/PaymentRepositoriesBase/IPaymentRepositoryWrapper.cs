using HNM.RepositoriesNC.PaymentRepositories;
using HNM.RepositoriesNC.Repositories;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.PaymentRepositoriesBase
{
    public interface IPaymentRepositoryWrapper
    {    
        IPaymentTypeRepository PaymentType { get; }
        IPaymentDiscountRepository PaymentDisCount { get; }
        IPaymentServiceRepository PaymentServcie { get; }
        IPaymentBankingRepository PaymentBanking { get; }
        IPaymentSettingRepository PaymentSetting { get; }
        IPaymentUpgradeRepository PaymentUpgrade { get; }
        void Save();
        Task<int> SaveChangesAsync();
    }
}
