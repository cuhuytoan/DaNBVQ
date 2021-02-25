using System;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class FCMClientDTO
    {
        public Guid? FCMClientID { get; set; }
        public string DeviceID { get; set; }
        public string OS { get; set; }
        public string Token { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastSeen { get; set; }
        public string UserName { get; set; }
        public string Topic { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; }
        public Guid? UserID { get; set; }
    }
}
