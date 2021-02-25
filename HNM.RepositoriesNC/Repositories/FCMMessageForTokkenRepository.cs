using HNM.DataNC.Models;
using HNM.RepositoriesNC.RepositoriesBase;

namespace HNM.RepositoriesNC.Repositories
{
    public interface IFCMMessageForTokkenRepository : IRepositoryBase<FCMMessageForTokken>
    {

    }
    public class FCMMessageForTokkenRepository : RepositoryBase<FCMMessageForTokken>, IFCMMessageForTokkenRepository
    {
        public FCMMessageForTokkenRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }


    }
}
