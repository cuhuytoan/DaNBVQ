using HNM.DataNC.ModelsNC.ModelsUtility;
using System;
using System.Collections.Generic;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class FCMMessageV2OutputDTO
    {
        public FCMMessageV2OutputDTO()
        {

        }
        private notificationType _notification = new notificationType();
        private dataType _data = new dataType();
        private string _to;
        public class notificationType
        {
            private string _title;
            private string _body;

            public string title
            {
                get { return _title; }
                set { _title = value; }
            }
            public string body
            {
                get { return _body; }
                set { _body = value; }
            }
        }
        public class dataType
        {
            private string _content;

            public string content
            {
                get { return _content; }
                set { _content = value; }
            }
            public int? NotifyType { get; set; }
            public int? FormId { get; set; }
            public int? ParameterId { get; set; }
            public string FullUrl { get; set; }
            public string FullUrlImage { get; set; }
            public int? ProductTypeId { get; set; }
        }
        // Các thuộc tính của FCMMessageV2Output
        public notificationType notification
        {
            get { return _notification; }
            set { _notification = value; }
        }
        public dataType data
        {
            get { return _data; }
            set { _data = value; }
        }
        public string to
        {
            get { return _to; }
            set { _to = value; }
        }
        //public string priority
        //{
        //    get { return _priority; }
        //    set { _priority = value; }
        //}
        //public string content_available
        //{
        //    get { return _content_available; }
        //    set { _content_available = value; }
        //}
        // Kết thúc các thuộc tính của FCMMessageV2Output
    }


    public class FCMMessageOutputDTO
    {
        public Notification Notification { get; set; }
        public DataNotify Data { get; set; }
        //public string Token { get; set; }
        public string Topic { get; set; }
        public FCMMessageOutputDTO()
        {
            Notification = new Notification();
            Data = new DataNotify();
        }

    }
    public class Notification
    {
        public string Title { get; set; }
        public string Body { get; set; }
    }
    public class DataNotify
    {
        /// <summary>
        /// 0 : View 1 : In APP
        /// </summary>
        public int? notifyType { get; set; }
        /// <summary>
        /// Refer List     
        /// </summary>
        public string formId { get; set; }
        /// <summary>
        /// Parameter
        /// </summary>
        public int? id { get; set; }
        public int? categoryId { get; set; }
        /// <summary>
        /// FullUrl image View
        /// </summary>
        public string fullUrl { get; set; }
        /// <summary>
        /// Full Main Image
        /// </summary>
        public string fullUrlImage { get; set; }
        public int? typeId { get; set; }
        public int? notiSpecType { get; set; }
        public int? isPinTop { get; set; }
        public string formAppName { get; set; }
        public string OrderCode { get; set; }

    }
    public class LstFCMMessage 
    {
        public int FCMMessageId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        /// <summary>
        /// 0 : View 1 : In APP
        /// </summary>
        public int? notifyType { get; set; }
        /// <summary>
        /// Refer List     
        /// </summary>
        public string formId { get; set; }
        /// <summary>
        /// Parameter
        /// </summary>
        public int? id { get; set; }
        public int? categoryId { get; set; }
        /// <summary>
        /// FullUrl image View
        /// </summary>
        public string fullUrl { get; set; }
        /// <summary>
        /// Full Main Image
        /// </summary>
        public string fullUrlImage { get; set; }
        public int? typeId { get; set; }
        public int? notiSpecType { get; set; }
        public int? isPinTop { get; set; }
        public string formAppName { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? HasRead { get; set; }
    }
    public class FCMHasReadDTO  :ModelBaseStatus
    {
        public FCMNumberUnread NumberUnread { get; set; }
    }

    public class FCMMessageChatDTO:ModelBase
    {
        public Notification Notification { get; set; }
        public DataChatNotify Data { get; set; }
        public List<string> Token { get; set; }
        //public string Topic { get; set; }
        public FCMMessageChatDTO()
        {
            Notification = new Notification();            
            Data = new DataChatNotify();
        }
    }
    public class DataChatNotify
    {
        /// <summary>
        /// Room Chat
        /// </summary>
        public string roomId { get; set; }
        /// <summary>
        /// avatar
        /// </summary>
        public string avatar { get; set; }
        /// <summary>
        /// tên người gửi
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// tin nhắn
        /// </summary>
        public string message { get; set; }
        public string formId { get; set; } = "CHAT";

    }
}
