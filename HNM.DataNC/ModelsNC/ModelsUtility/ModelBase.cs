namespace HNM.DataNC.ModelsNC.ModelsUtility
{
    public class ModelBase
    {
        /// <summary>
        /// Json web token
        /// </summary>
        public string JWT { get; set; }
        /// <summary>
        /// Error code
        /// </summary>
        public string ErrorCode { get; set; }
        /// <summary>
        /// Error Message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// User ID (if already login always push userId on each request)
        /// </summary>
        public string UserId { get; set; }

    }

    public class ModelBaseStatus
    {
        public string ErrorCode { get; set; } = "00";

        public string Message { get; set; } = "Successed";
    }
    public class ModelPaymentStatus
    {
        public string RspCode { get; set; } = "00";

        public string Message { get; set; } = "Successed";
    }

}
