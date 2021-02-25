using System;
using System.Collections.Generic;
using System.Text;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class OrderInfo
    {
        /// <summary>
        /// Merchant OrderId
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// Payment amount
        /// </summary>
        public decimal Amount { get; set; }
        public string OrderDescription { get; set; }

        public string BankCode { get; set; }

        /// <summary>
        /// Order Status
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Creaed date
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// VNPAY Transaction Id
        /// </summary>
        public decimal vnp_TransactionNo { get; set; }
        public string vpn_Message { get; set; }
        public string vpn_TxnResponseCode { get; set; }
    }
}
