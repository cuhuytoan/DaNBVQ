using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace HNM.CommonNC
{
    public class Utils
    {

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
  
        /// <summary>
        /// Render Price For Product
        /// </summary>
        /// <param name="Price"></param>
        /// <returns></returns>
        public static string RenderPrice(object Price)
        {
            string Result = "Liên hệ";
            if (Price != null)
            {
                double PriceNumber = Convert.ToDouble(Price);
                if (PriceNumber > 0)
                {
                    Result = ShowNumber(PriceNumber, 0);
                }
            }
            return Result;
        }
        public static string RenderTimeSince(DateTime datetime)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;
            var ts = new TimeSpan(DateTime.Now.Ticks - datetime.Ticks);
            if (ts.Seconds < 0 ||ts.Minutes < 0 || ts.Hours < 0 || ts.Days< 0)
            {
                ts = -(ts);
            }
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "1 giây trước" : ts.Seconds + " giây trước";

            if (delta < 2 * MINUTE)
                return "1 phút trước";

            if (delta < 45 * MINUTE)
                return ts.Minutes + " phút trước";

            if (delta < 90 * MINUTE)
                return "1 giờ trước";

            if (delta < 24 * HOUR)
                return ts.Hours + " giờ trước";

            if (delta < 48 * HOUR)
                return "hôm qua";

            if (delta < 30 * DAY)
                return ts.Days + " ngày trước";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "1 tháng trước" : months + " tháng trước";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "1 năm trước" : years + " năm trước";
            }
        }
        /// <summary>
        /// Return ID from Url
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static int RegexRouteIdFromUrl(string Url)
        {
            int Id = 0;
            try
            {
              Id=  Int32.Parse(Url.Remove(0, Url.LastIndexOf("-") + 1));
            }
            catch(Exception ex)
            {
                
            }
            return Id;
        }

        public static string RegexRouteStringFromUrl(string Url)
        {
            string text = null;
            try
            {
                text = Url.Replace("-", " ");
            }
            catch (Exception ex)
            {

            }
            return text;
        }

        public static int YearJoinHanoma(DateTime createDate)
        {
            return DateTime.Now.Year - DateTime.Parse(createDate.ToString()).Year + 1;
        }
        public static bool IsEmail(string email)
        {
            try
            {
                string pattern = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-\.]{1,}$";
                var regex = new Regex(pattern, RegexOptions.IgnoreCase);

                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }
    }
}
