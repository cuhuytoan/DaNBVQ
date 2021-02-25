using HNM.DataNC.Models;
using HNM.RepositoriesNC.RepositoriesBase;
using System;

namespace HNM.RepositoriesNC.Repositories
{
    public interface IFCMClientRepository : IRepositoryBase<FCMClient>
    {
        void UpdateFCMClient(Guid fcmClientId, FCMClient fcmClient);
    }

    public class FCMClientRepository : RepositoryBase<FCMClient>, IFCMClientRepository
    {
        public FCMClientRepository(HanomaContext hanomaContext)
            : base(hanomaContext)
        {
        }


        public void UpdateFCMClient(Guid fcmClientId, FCMClient fcmClient)
        {
            var fcmClientItem = HanomaContext.FCMClient.Find(fcmClientId);
            if (fcmClientItem != null)
            {
                fcmClientItem.LastSeen = DateTime.Now;
                fcmClientItem.UserID = fcmClient.UserID;
                fcmClientItem.UserName = fcmClient.UserName;
                fcmClientItem.Topic = fcmClient.UserID.ToString();
                HanomaContext.SaveChanges();
            }
        }
    }
}
