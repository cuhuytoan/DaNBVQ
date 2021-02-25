
using HNM.DataNC.Models;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesNC.RepositoriesBase;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.Repositories
{
    public interface IFCMMessageRepository : IRepositoryBase<FCMMessage>
    {
        Task<IEnumerable<FCMMessage>> GetListMsgByUser(string UserID, int? Top, int? NotiSpecType = 0);
        Task<FCMNumberUnread> GetNumberFCMUnread(string UserId);
        Task<FCMMessage> GetDetailMessage(string UserID, int FCMMessage_ID);
        int AddNewData(string UserID);
        Task UpdateData(FCMMessage Content);
        Task<int> UpdateHasRead(string UserId, int HasRead, int? FCMMessageID = 0, int? NotiSpecType = 0);
        Task PushNotificationToRabitMQ(NotificationRabitMQModel model);
        Task AddNewFCMMessage(FCMMessage model);

    }
    public class FCMMessageRepository : RepositoryBase<FCMMessage>, IFCMMessageRepository
    {
        public FCMMessageRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }

        public int AddNewData(string UserID)
        {
            FCMMessage CurrentFCMMessage = new FCMMessage();
            CurrentFCMMessage.CreateDate = DateTime.Now;
            CurrentFCMMessage.CreateBy = Guid.Parse(UserID);
            HanomaContext.FCMMessage.Add(CurrentFCMMessage);
            HanomaContext.SaveChangesAsync();
            return CurrentFCMMessage.FCMMessage_ID;
        }

        public async Task<FCMMessage> GetDetailMessage(string UserID, int FCMMessage_ID)
        {
            var userId = Guid.Parse(UserID);
            var result = await HanomaContext.FCMMessage.FirstOrDefaultAsync(p => p.UserID == userId && p.FCMMessage_ID == FCMMessage_ID);
            return result;
        }

        public async Task<IEnumerable<FCMMessage>> GetListMsgByUser(string UserID, int? Top = 20, int? NotiSpecType = 0)
        {
            if (!String.IsNullOrEmpty(UserID))
            {
                var userId = Guid.Parse(UserID);
                if (NotiSpecType != 0)
                {
                    return await HanomaContext.FCMMessage.Where(p => (p.Topic == UserID || p.Topic == "Global") && p.NotiSpecType == NotiSpecType)
                        .OrderByDescending(x => x.CreateDate)
                        .Take(Top ?? 20)
                   .ToListAsync();
                }
                else
                {
                    return await HanomaContext.FCMMessage.Where(p => (p.Topic == UserID || p.Topic == "Global"))
                        .OrderByDescending(x => x.CreateDate)
                        .Take(Top ?? 20)
                  .ToListAsync();
                }

            }
            else
            {
                if (NotiSpecType != 0)
                {
                    return await HanomaContext.FCMMessage.Where(p => p.Topic == "Global" && p.NotiSpecType == NotiSpecType).Take(Top ?? 20)
                    .ToListAsync();
                }
                else
                {
                    return await HanomaContext.FCMMessage.Where(p => p.Topic == "Global").Take(Top ?? 20)
                   .ToListAsync();
                }

            }
        }

        public async Task<FCMNumberUnread> GetNumberFCMUnread(string UserId)
        {
            var output = new FCMNumberUnread();
            output.TotalUnRead = HanomaContext.FCMMessage.Count(p => p.Topic == UserId  && p.HasRead  == 0);
            output.UserUnRead = HanomaContext.FCMMessage.Count(p => p.Topic == UserId  && p.NotiSpecType == 1 && p.HasRead == 0);
            output.CommunityUnRead = HanomaContext.FCMMessage.Count(p => p.Topic == UserId  && p.NotiSpecType == 2 && p.HasRead == 0);
            output.PromotionUnRead = HanomaContext.FCMMessage.Count(p => p.Topic == UserId  && p.NotiSpecType == 3 && p.HasRead == 0);
            return output;
        }

        public Task PushNotificationToRabitMQ(NotificationRabitMQModel model)
        {
            Send("ondemand-notifications", JsonConvert.SerializeObject(model));
            return Task.CompletedTask;
        }
        public IConnection GetConnection()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory
            {
                HostName = "202.134.18.185",
                Port = 5672,
                UserName = "hanoma",
                Password = "hanoma",
                VirtualHost = "/",
            };
            return connectionFactory.CreateConnection();
        }
        public void Send(string queue, string data)
        {
            using (IConnection connection = GetConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue, true, false, false, null);
                    channel.BasicPublish(string.Empty, queue, null, Encoding.UTF8.GetBytes(data));
                }
            }
        }
        public Task UpdateData(FCMMessage Content)
        {
            //FCMMessage CurrentFCMMessage = HanomaContext.FCMMessage.FirstOrDefault(p => p.FCMMessage_ID == FCMMessage_ID);
            FCMMessage CurrentFCMMessage = new FCMMessage();
            CurrentFCMMessage.Title = Content.Title;
            CurrentFCMMessage.Body = Content.Body;
            CurrentFCMMessage.Content = Content.Content;
            CurrentFCMMessage.UserName = "";
            CurrentFCMMessage.UserID = Content.UserID;
            CurrentFCMMessage.NotificationType = Content.NotificationType;
            CurrentFCMMessage.Form_ID = Content.Form_ID;
            CurrentFCMMessage.ParameterId = Content.ParameterId ?? 0;
            CurrentFCMMessage.FullUrl = Content.FullUrl;
            CurrentFCMMessage.Topic = Content.Topic;
            CurrentFCMMessage.CreateDate = DateTime.Now;
            CurrentFCMMessage.CreateBy = Content.UserID;
            CurrentFCMMessage.UserID = Content.UserID;
            HanomaContext.FCMMessage.Add(CurrentFCMMessage);
            HanomaContext.SaveChangesAsync();


            //Insert FCMMessageToken
            //if (Content.Topic == "Global")
            //{
            //    var ItemClient = HanomaContext.FCMClient.Select(p => p.UserID).Distinct().ToList();
            //    foreach (var p in ItemClient)
            //    {
            //        FCMMessageForTokken item = new FCMMessageForTokken();
            //        if (p != null) item.UserID = p.Value;
            //        item.FCMMessage_ID = CurrentFCMMessage.FCMMessage_ID;
            //        item.CreateBy = Guid.Parse(User_ID);
            //        item.CreateDate = DateTime.Now;
            //        HanomaContext.FCMMessageForTokken.Add(item);
            //        HanomaContext.SaveChangesAsync();                    
            //    }
            //}
            //else
            //{
            //    var ItemClient = HanomaContext.FCMClient.Where(p => p.Topic == Content.Topic).Select(i => i.UserID).Distinct().ToList();
            //    if (ItemClient != null)
            //    {
            //        foreach (var p in ItemClient)
            //        {
            //            FCMMessageForTokken item = new FCMMessageForTokken();
            //            if (p != null) item.UserID = p.Value;
            //            item.FCMMessage_ID = CurrentFCMMessage.FCMMessage_ID;
            //            item.CreateBy = Guid.Parse(User_ID);
            //            item.CreateDate = DateTime.Now;
            //            HanomaContext.FCMMessageForTokken.Add(item);
            //            HanomaContext.SaveChangesAsync();
            //        }
            //    }
            //}

            return Task.CompletedTask;
        }

        public async Task<int> UpdateHasRead(string UserId, int HasRead, int? FCMMessageID = 0, int? NotiSpecType = 0)
        {
            try
            {
                if (FCMMessageID != 0)
                {
                    var fcmMessage = HanomaContext.FCMMessage.FirstOrDefault(p => p.FCMMessage_ID == FCMMessageID && p.Topic == UserId);
                    if (fcmMessage != null)
                    {
                        fcmMessage.HasRead = HasRead;
                        HanomaContext.SaveChanges();
                    }
                }
                else
                {
                    if(NotiSpecType == 0)
                    {
                        var lstFcmMessage = HanomaContext.FCMMessage.Where(p => p.Topic == UserId);
                        await lstFcmMessage.ForEachAsync(p => p.HasRead = HasRead);
                        HanomaContext.SaveChanges();
                    }
                    else
                    {
                        var lstFcmMessage = HanomaContext.FCMMessage.Where(p => p.Topic == UserId && p.NotiSpecType == NotiSpecType);
                        await lstFcmMessage.ForEachAsync(p => p.HasRead = HasRead);
                        HanomaContext.SaveChanges();
                    }
         
                    
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 1;
        }

        public async Task AddNewFCMMessage(FCMMessage model)
        {
            FCMMessage item = new FCMMessage();
            item = model;
            HanomaContext.Add(item);
            await HanomaContext.SaveChangesAsync();
        }
    }
}
