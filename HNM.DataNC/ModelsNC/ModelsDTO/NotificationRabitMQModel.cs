namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class NotificationRabitMQModel
    {
        public string Type { get; set; }
        public string NotificationCode { get; set; }
        public string ChannelSend { get; set; }
        public bool? UsingTemplate { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string UserId { get; set; }
        public int ObjectId { get; set; }
        public string ObjectType { get; set; }
        public string OrderId { get; set; }
    }
}
