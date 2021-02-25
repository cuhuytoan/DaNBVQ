using HNM.DataNC.ModelsNC.ModelsUtility;
using System;
using System.Collections.Generic;
using System.Text;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class SubscriptionDTO
    {
        public int Subscription_ID { get; set; }
        public string Email { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? Active { get; set; }
    }
    public class SubscriptionRequestDTO
    {
        public string Email { get; set; }
    }
    public class SubscriptionResultDTO : ModelBaseStatus
    {
    }
}
