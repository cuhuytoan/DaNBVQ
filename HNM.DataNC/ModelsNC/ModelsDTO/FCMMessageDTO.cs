using HNM.DataNC.Models;
using HNM.DataNC.ModelsNC.ModelsUtility;
using System;
using System.Collections.Generic;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class FCMMessageDTO : ModelBaseStatus
    {
        public int FCMMessageId { get; set; }
        public FCMMessageOutputDTO FCMMsg { get; set; }
        public Guid? UserId { get; set; }
    }
    public class LstFCMMessageUser
    {
        public List<LstFCMMessage> LstFCMMessage { get; set; }
        public FCMNumberUnread NumberUnread { get; set; }
    }
    public class FCMNumberUnread
    {
        public int? TotalUnRead { get; set; }
        public int? UserUnRead { get; set; }
        public int? CommunityUnRead { get; set; }
        public int? PromotionUnRead { get; set; }
    } 
 
}
