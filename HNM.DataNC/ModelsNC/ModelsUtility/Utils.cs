using System;
using System.Globalization;

namespace HNM.DataNC.ModelsNC.ModelsUtility
{
    public class Utils
    {
        public static string RenderPriceRecruitment(object PriceFrom, object PriceTo)
        {
            string Result = "Thỏa thuận";
            if (PriceFrom != null && PriceTo != null)
            {
                double PriceNumberFrom = Convert.ToDouble(PriceFrom);
                double PriceNumberTo = Convert.ToDouble(PriceTo);
                if (PriceNumberFrom > 0 && PriceNumberTo > 0)
                {
                    Result = ShowNumber(PriceNumberFrom, 0) + " ~ " + ShowNumber(PriceNumberTo, 0);
                }
            }
            else if (PriceFrom != null && PriceTo == null)
            {
                double PriceNumberFrom = Convert.ToDouble(PriceFrom);
                if (PriceNumberFrom > 0)
                {
                    Result = ShowNumber(PriceNumberFrom, 0) + " ~ " + Result;
                }
            }
            return Result;
        }
        public static string ShowNumber(object Number, int NumberDecimalDigits)
        {
            string NumberString = "0";
            if (Number != null && Number.ToString() != string.Empty)
            {
                NumberFormatInfo myNumberFormat = new CultureInfo("vi-VN").NumberFormat;
                myNumberFormat.NumberGroupSeparator = ".";
                myNumberFormat.NumberDecimalDigits = NumberDecimalDigits;

                NumberString = double.Parse(Number.ToString()).ToString("N", myNumberFormat);
            }
            return NumberString;
        }

        public static string CloudPath()
        {
            return "https://cdn.hanoma.vn";
        }

        public static string CdnPath()
        {
            return "https://cdn.hanoma.vn";
        }
    }
}
