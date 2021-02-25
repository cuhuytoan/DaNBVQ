using System;
using System.Data;
using System.Linq;
using System.Globalization;
using System.Resources;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Amazon.S3.Model;
using Amazon.S3;
using Amazon.S3.Util;
using Amazon.S3.Transfer;
using System.Threading.Tasks;

namespace HNM.Common
{
    public class CommonUtil
    {

        public static Dictionary<int, string> LocationDictionary = new Dictionary<int, string> { { -1, "Tất cả tỉnh thành" }, { 1, "An Giang" }, { 2, "Bà Rịa - Vũng Tàu" }, { 3, "Bắc Giang" }, { 4, "Bắc Kạn" }, { 5, "Bạc Liêu" }, { 6, "Bắc Ninh" }, { 7, "Bến Tre" }, { 8, "Bình Định" }, { 9, "Bình Dương" }, { 10, "Bình Phước" }, { 11, "Bình Thuận" }, { 12, "Cà Mau" }, { 13, "Cao Bằng" }, { 14, "Đắk Lắk" }, { 15, "Đắk Nông" }, { 16, "Điện Biên" }, { 17, "Đồng Nai" }, { 18, "Đồng Tháp" }, { 19, "Gia Lai" }, { 20, "Hà Giang" }, { 21, "Hà Nam" }, { 22, "Hà Tĩnh" }, { 23, "Hải Dương" }, { 24, "Hậu Giang" }, { 25, "Hòa Bình" }, { 26, "Hưng Yên" }, { 27, "Khánh Hòa" }, { 28, "Kiên Giang" }, { 29, "Kon Tum" }, { 30, "Lai Châu" }, { 31, "Lâm Đồng" }, { 32, "Lạng Sơn" }, { 33, "Lào Cai" }, { 34, "Long An" }, { 35, "Nam Định" }, { 36, "Nghệ An" }, { 37, "Ninh Bình" }, { 38, "Ninh Thuận" }, { 39, "Phú Thọ" }, { 40, "Quảng Bình" }, { 41, "Quảng Nam" }, { 42, "Quảng Ngãi" }, { 43, "Quảng Ninh" }, { 44, "Quảng Trị" }, { 45, "Sóc Trăng" }, { 46, "Sơn La" }, { 47, "Tây Ninh" }, { 48, "Thái Bình" }, { 49, "Thái Nguyên" }, { 50, "Thanh Hóa" }, { 51, "Thừa Thiên Huế" }, { 52, "Tiền Giang" }, { 53, "Trà Vinh" }, { 54, "Tuyên Quang" }, { 55, "Vĩnh Long" }, { 56, "Vĩnh Phúc" }, { 57, "Yên Bái" }, { 58, "Phú Yên" }, { 59, "Cần Thơ" }, { 60, "Đà Nẵng" }, { 61, "Hải Phòng" }, { 62, "Hà Nội" }, { 63, "TP HCM" } };
        public static Dictionary<string, string> MetaSettings = new Dictionary<string, string>() { };
        private static Random random = new Random();
        private static readonly Regex NonExplicitLines = new Regex("\r|\n", RegexOptions.Multiline | RegexOptions.Compiled);
        private static readonly Regex DivEndings = new Regex("</div>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex LineBreaks = new Regex("</br\\s*>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex Tags = new Regex("<[^>]*>", RegexOptions.Compiled);

        public static string accessKey = "W1H820TV8PYDK6I3C3K0";
        public static string secretKey = "HwSPUnmegVksiOXN8GvRe1Lv2m40kKRDHGqayv9v";
        public static string ServiceURL = "https://s3.cloud.cmctelecom.vn";
        public static string BucketName = "hanoma-cdn";
        public static string GetResources(string ResourceName, string Key)
        {
            ResourceManager Resources = new ResourceManager("Resources." + ResourceName, System.Reflection.Assembly.Load("App_GlobalResources"));
            return Resources.GetString(Key);
        }
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RandomNumber(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string GetRandomText()
        {
            StringBuilder randomText = new StringBuilder();
            string alphabets = "2345679ABCEGHKMNPSXZabceghkmnpqstuvxz";
            Random r = new Random();
            for (int j = 0; j <= 2; j++)
            {
                randomText.Append(alphabets[r.Next(alphabets.Length)]);
            }
            return randomText.ToString();
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

        public static int CounterWebsite
        {
            get
            {
                return 0;
            }
        }

        public static void AddCounterWebsite()
        {

        }

        public static string CutText(string TextInput, int NumberCharacter)
        {
            string Result = "";
            if (string.IsNullOrEmpty(TextInput)) return TextInput;
            TextInput = StripHTML(TextInput);
            TextInput = Regex.Replace(TextInput, @"\r\n?|\n|\t", " ");
            TextInput = Regex.Replace(TextInput, @"\s+", " ");

            int Length = TextInput.Length;
            if (Length < NumberCharacter)
                return TextInput;

            // Process
            TextInput = TextInput.Substring(0, NumberCharacter);
            int index = TextInput.LastIndexOfAny(new char[] { ' ' });
            Result = TextInput.Substring(0, index);
            return Result;
        }
        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public static bool DeleteFile(string xPath, string xFileName)
        {
            if (File.Exists(xPath + xFileName))
            {
                File.Delete(xPath + xFileName);
                return true;
            }
            else
                return false;
        }



        public static string GetFileNameFromURL(string URL)
        {
            return URL.Substring(URL.LastIndexOf("/") + 1, URL.Length - URL.LastIndexOf("/") - 1);
        }

        public static string GetFileNameNotExFromUrl(string Url)
        {
            string tmp = GetFileNameFromURL(Url);
            return tmp.Substring(0, tmp.Length - 4);
        }

        public static bool IsImageFile(string Extention)
        {
            foreach (string iEx in new string[5] { ".jpg", ".gif", ".png", ".bmp", ".jpeg" })
            {
                if (Extention.ToLower() == iEx)
                    return true;
            }
            return false;
        }

        public static void EditSize(string OrgFileName, string DesFileName, int WidthLimit, int HeightLimit)
        {
            if (System.IO.File.Exists(Path.Combine(DesFileName)))
            {
                System.IO.File.Delete(Path.Combine(DesFileName));
            }

            ImageFormat Format;
            System.Drawing.Image OrgImg = System.Drawing.Image.FromFile(OrgFileName);
            int DesWidth = OrgImg.Width;
            int DesHeight = OrgImg.Height;
            double ratio = (double)DesWidth / (double)DesHeight;
            Format = OrgImg.RawFormat;
            if (DesWidth <= WidthLimit && DesHeight <= HeightLimit)
            {
                File.Copy(OrgFileName, DesFileName);
                return;
            }

            if (DesWidth > WidthLimit)
            {
                DesWidth = WidthLimit;
                DesHeight = (int)Math.Round(DesWidth / ratio);
            }

            if (DesHeight > HeightLimit)
            {
                DesHeight = HeightLimit;
                DesWidth = (int)Math.Round(DesHeight * ratio);
            }

            Bitmap DesImg = new System.Drawing.Bitmap(DesWidth, DesHeight, PixelFormat.Format24bppRgb);
            DesImg.SetResolution(96, 96);

            Graphics GraphicImg = Graphics.FromImage(DesImg);
            GraphicImg.SmoothingMode = SmoothingMode.AntiAlias;
            GraphicImg.InterpolationMode = InterpolationMode.HighQualityBicubic;
            GraphicImg.PixelOffsetMode = PixelOffsetMode.HighQuality;
            System.Drawing.Rectangle oRectangle = new Rectangle(0, 0, DesWidth, DesHeight);
            GraphicImg.DrawImage(OrgImg, oRectangle);
            OrgImg.Dispose();
            DesImg.Save(DesFileName, Format);
        }

        public static string GetFileSize(string FileName)
        {
            long Bytes = 0;
            if (File.Exists(FileName))
            {
                System.IO.FileInfo f = new System.IO.FileInfo(FileName);
                Bytes = f.Length;
            }

            if (Bytes >= 1073741824)
            {
                Decimal size = Decimal.Divide(Bytes, 1073741824);
                return String.Format("{0:##.##} GB", size);
            }
            else if (Bytes >= 1048576)
            {
                Decimal size = Decimal.Divide(Bytes, 1048576);
                return String.Format("{0:##.##} MB", size);
            }
            else if (Bytes >= 1024)
            {
                Decimal size = Decimal.Divide(Bytes, 1024);
                return String.Format("{0:##.##} KB", size);
            }
            else if (Bytes > 0 & Bytes < 1024)
            {
                Decimal size = Bytes;
                return String.Format("{0:##.##} Bytes", size);
            }
            else
            {
                return "0 Bytes";
            }
        }

        public static string FormatDateTimeToShortDate(DateTime TimeInput)
        {
            string Result = "";
            double DaySpan = (DateTime.Now - TimeInput).TotalDays;
            double HourSpan = (DateTime.Now - TimeInput).TotalHours;
            double MinuteSpan = (DateTime.Now - TimeInput).TotalMinutes;

            if (MinuteSpan < 1)
                return "Vừa xong";

            if (MinuteSpan < 60)
                return Math.Truncate(MinuteSpan) + " phút";

            if (MinuteSpan >= 60 && HourSpan < 24)
                return Math.Truncate(HourSpan) + " giờ";

            if (DaySpan < 2)
                return "Hôm qua";

            Result = String.Format("{0:dd/MM/yyyy}", TimeInput);
            return Result;
        }
        public static int CheckTimeToRenew(DateTime dateTime)
        {
            int spaceMinutes = 0;
            try
            {
                Int32 unixTimestampInput = (Int32)(dateTime.Subtract(new DateTime(1970, 1, 1))).TotalMinutes;
                Int32 unixTimestampCurrent = (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalMinutes;
                spaceMinutes = (unixTimestampCurrent - unixTimestampInput);


                return spaceMinutes;
            }
            catch (Exception ex)
            {

                return spaceMinutes;
            }
        }
        public static string ConvertTimeSpaceToString(DateTime dateTimeFrom, DateTime dateTimeEnd)
        {
            string res = "";
            Int32 unixTimestampInput = (Int32)(dateTimeFrom.Subtract(new DateTime(1970, 1, 1))).TotalMinutes;
            Int32 unixTimestampCurrent = (Int32)(dateTimeEnd.Subtract(new DateTime(1970, 1, 1))).TotalMinutes;
            var spaceMinutes = (unixTimestampCurrent - unixTimestampInput);
            var hours = spaceMinutes / 60;
            var minutes = spaceMinutes - hours * 60;
            res = String.Format("{0}h {1} phút", hours, minutes);
            return res;
        }



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

        public static string RenderNumber(object Number)
        {
            string Result = "Không xác định";
            if (Number != null)
            {
                double PriceNumber = Convert.ToDouble(Number);
                if (PriceNumber > 0)
                {
                    Result = ShowNumber(PriceNumber, 0);
                }
            }
            return Result;
        }

        public static string RenderNumberYear(object Number)
        {
            string Result = "Không xác định";
            if (Number != null)
            {
                double PriceNumber = Convert.ToDouble(Number);
                if (PriceNumber > 0)
                {
                    Result = PriceNumber.ToString();
                }
            }
            return Result;
        }

        public static string RenderString(object InputObject)
        {
            string Result = "Không xác định";
            if (InputObject != null)
            {
                string InputText = InputObject.ToString();
                if (!string.IsNullOrEmpty(InputText))
                {
                    Result = InputText;
                }
            }
            return Result;
        }
        public static bool IsEmail(string email)
        {
            try
            {
                //var addr = new System.Net.Mail.MailAddress(email);
                //return addr.Address == email;
                string pattern1 = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
                string pattern = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-\.]{1,}$";
                var regex = new Regex(pattern, RegexOptions.IgnoreCase);

                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }
        public static bool IsPhoneNumber(string number)
        {
            if (String.IsNullOrEmpty(number)) return false;
            string RegexE;
            List<string> lst = PhoneList();
            foreach (var p in lst)
            {
                RegexE = @"^" + p + @"([0-9]{1,7})$";
                if (Regex.Match(number, RegexE).Success)
                {
                    return true;
                }
            }

            return false;
        }

        public static List<string> PhoneList()
        {
            List<string> lst = new List<string>();
            //Viettel
            lst.Add("032");
            lst.Add("033");
            lst.Add("034");
            lst.Add("035");
            lst.Add("036");
            lst.Add("037");
            lst.Add("038");
            lst.Add("039");
            lst.Add("086");
            lst.Add("096");
            lst.Add("097");
            lst.Add("098");
            //MobilePhone
            lst.Add("070");
            lst.Add("079");
            lst.Add("077");
            lst.Add("076");
            lst.Add("078");
            lst.Add("089");
            lst.Add("090");
            lst.Add("093");
            //VinaPhone
            lst.Add("083");
            lst.Add("084");
            lst.Add("085");
            lst.Add("081");
            lst.Add("082");
            lst.Add("088");
            lst.Add("091");
            lst.Add("094");
            //VietNamMobile
            lst.Add("092");
            lst.Add("056");
            lst.Add("058");
            //GPhone
            lst.Add("099");
            lst.Add("059");
            return lst;

        }
        public static bool IsEmailOrPhone(string input)
        {
            if (IsEmail(input)) return true;
            if (IsPhoneNumber(input)) return true;
            return false;
        }

        //PrefixTitle
        public static string GetPrefixTitle(int? productTypeId = 1)
        {
            var PrefixTitle = "";
            switch (productTypeId)
            {
                case 1:
                    PrefixTitle = "Bán";
                    break;
                case 2:
                    PrefixTitle = "Cần mua";
                    break;
                case 3:
                    PrefixTitle = "Cho thuê";
                    break;
                case 4:
                    PrefixTitle = "Cần thuê";
                    break;
                case 5:
                    PrefixTitle = "Bán";
                    break;
                case 6:
                    PrefixTitle = "Cần mua";
                    break;
            }
            return PrefixTitle;
        }
        public static string GetPrefixURL(int? productTypeId = 1)
        {
            var PrefixURL = "";
            switch (productTypeId)
            {
                case 1:
                    PrefixURL = "ban";
                    break;
                case 2:
                    PrefixURL = "can-mua";
                    break;
                case 3:
                    PrefixURL = "cho-thue";
                    break;
                case 4:
                    PrefixURL = "can-thue";
                    break;
                case 5:
                    PrefixURL = "ban-phu-tung";
                    break;
                case 6:
                    PrefixURL = "can-mua-phu-tung";
                    break;
                case 7:
                    PrefixURL = "can-ban-vat-tu";
                    break;
                case 8:
                    PrefixURL = "can-mua-vat-tu";
                    break;
                case 11:
                    PrefixURL = "dich-vu";
                    break;
            }
            return PrefixURL;
        }
        public static string GetNameByTypeId(int? productTypeId = 1)
        {
            var typeName = "";
            switch (productTypeId)
            {
                case 1:
                    typeName = "Máy để bán";
                    break;
                case 2:
                    typeName = "Cần mua máy";
                    break;
                case 3:
                    typeName = "Cho thuê máy";
                    break;
                case 4:
                    typeName = "Cần thuê máy";
                    break;
                case 5:
                    typeName = "Bán phụ tùng";
                    break;
                case 6:
                    typeName = "Cần mua phụ tùng";
                    break;
                case 7:
                    typeName = "Cần bán vật tư";
                    break;
                case 8:
                    typeName = "Cần mua vật tư";
                    break;
                case 11:
                    typeName = "Dịch vụ";
                    break;
            }
            return typeName;
        }
        public static MemoryStream GetMemoryStreamCaptchaImage(string text)
        {
            //first, create a dummy bitmap just to get a graphics object
            System.Drawing.Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            Font font = new Font("Arial", 15);
            //measure the string to see how big the image needs to be
            SizeF textSize = drawing.MeasureString(text, font);

            //free up the dummy image and old graphics object
            img.Dispose();
            drawing.Dispose();

            //create a new image of the right size
            img = new Bitmap((int)textSize.Width + 40, (int)textSize.Height + 20);
            drawing = Graphics.FromImage(img);

            Color backColor = Color.Black;
            Color textColor = Color.White;
            //paint the background
            drawing.Clear(backColor);

            //create a brush for the text
            Brush textBrush = new SolidBrush(textColor);

            drawing.DrawString(text, font, textBrush, 20, 10);

            drawing.Save();

            font.Dispose();
            textBrush.Dispose();
            drawing.Dispose();

            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            img.Dispose();
            return ms;
        }

        public static string Decode(string html)
        {
            if (string.IsNullOrEmpty(html))
                return html;
            var decoded = html.Trim();
            if (!HasTags(decoded))
                return html;
            decoded = NonExplicitLines.Replace(decoded, string.Empty);
            decoded = DivEndings.Replace(decoded, Environment.NewLine);
            decoded = LineBreaks.Replace(decoded, Environment.NewLine);
            decoded = Tags.Replace(decoded, string.Empty).Trim();
            return WebUtility.HtmlDecode(decoded);
        }
        private static bool HasTags(string str)
        {
            return str.StartsWith("<") && str.EndsWith(">");
        }

        public static void MoveFile(string filePathSource, string filePathTarget)
        {
            try
            {

                if (System.IO.File.Exists(filePathTarget))
                    System.IO.File.Delete(filePathTarget);
                System.IO.File.Move(filePathSource, filePathTarget);

            }
            catch (Exception ex)
            {
                var res = ex;
            }
        }
        public static void WaterMark(string filePath, string imageName)
        {
            try
            {
                var filePathTemp = Path.Combine(HttpContext.Current.Server.MapPath("~/Data/Temp"), imageName);
                var filePathWaterMark = HttpContext.Current.Server.MapPath("~/Data/watermark/watermark_1_499.png");

                using (System.Drawing.Image image = System.Drawing.Image.FromFile(filePath))
                {
                    var switchValue = image.Width / 513;

                    switch (switchValue)
                    {
                        case 0: // 1 - 499
                            filePathWaterMark = HttpContext.Current.Server.MapPath("~/Data/watermark/watermark_1_499.png");
                            break;
                        case 1: // 500 999
                            filePathWaterMark = HttpContext.Current.Server.MapPath("~/Data/watermark/watermark_500_999.png");
                            break;
                        case 2: // 1000 1499
                            filePathWaterMark = HttpContext.Current.Server.MapPath("~/Data/watermark/watermark_1000_1499.png");
                            break;
                        case 3: //1500 1999
                            filePathWaterMark = HttpContext.Current.Server.MapPath("~/Data/watermark/watermark_1500_1999.png");
                            break;
                        case 4: // 2000 2499
                            filePathWaterMark = HttpContext.Current.Server.MapPath("~/Data/watermark/watermark_2000_2499.png");
                            break;
                        case 5: // 2500 2999
                            filePathWaterMark = HttpContext.Current.Server.MapPath("~/Data/watermark/watermark_2500_2999.png");
                            break;
                        case 6: // 3000 3499
                            filePathWaterMark = HttpContext.Current.Server.MapPath("~/Data/watermark/watermark_3000_3499.png");
                            break;
                        case 7: // 3500 3999
                            filePathWaterMark = HttpContext.Current.Server.MapPath("~/Data/watermark/watermark_3500_3999.png");
                            break;
                        default: // > 4000
                            filePathWaterMark = HttpContext.Current.Server.MapPath("~/Data/watermark/watermark_1000.png");
                            break;
                    }
                }


                using (System.Drawing.Image image = System.Drawing.Image.FromFile(filePath))
                using (System.Drawing.Image watermarkImage = System.Drawing.Image.FromFile(filePathWaterMark))
                using (Graphics imageGraphics = Graphics.FromImage(image))
                using (TextureBrush watermarkBrush = new TextureBrush(watermarkImage))
                {
                    int x = (image.Width * 3 / 4 - watermarkImage.Width / 2);
                    int y = (image.Height / 4 - watermarkImage.Height / 2);
                    watermarkBrush.TranslateTransform(x, y);
                    imageGraphics.FillRectangle(watermarkBrush, new Rectangle(new Point(x, y), new Size(watermarkImage.Width + 1, watermarkImage.Height)));



                    image.Save(filePathTemp);

                }
                MoveFile(filePathTemp, filePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async Task UploadS3(string fileName, string pathSave, Stream inputStream, string contentType)
        {
            try
            {
                AmazonS3Config config = new AmazonS3Config();
                config.ServiceURL = ServiceURL;
                AmazonS3Client _amazonS3Client = new AmazonS3Client(
                        accessKey,
                        secretKey,
                        config
                        );
                var fileTransferUtility = new TransferUtility(_amazonS3Client);

                var request = new PutObjectRequest();
                string _bucketName = String.Format($"{BucketName}/{pathSave}");

                request.BucketName = _bucketName;
                request.CannedACL = S3CannedACL.PublicRead;                
                request.InputStream = inputStream;
                request.Key = fileName;
                request.Headers.ContentType = contentType;

                await _amazonS3Client.PutObjectAsync(request);
            }
            catch (Exception ex)
            {
                // log or throw;
            }

        }
        public static async Task UploadProductImageToS3(string fileName, string pathSave1, string pathSave2, string pathSave3, Stream inputStreamOri, Stream inputStreamSmall, Stream inputStreamThumb, string contentType)
        {
            try
            {
                AmazonS3Config config = new AmazonS3Config();
                config.ServiceURL = ServiceURL;
                AmazonS3Client _amazonS3Client = new AmazonS3Client(
                        accessKey,
                        secretKey,
                        config
                        );
                var fileTransferUtility = new TransferUtility(_amazonS3Client);

                // original
                var request = new PutObjectRequest();
                string _bucketName = String.Format($"{BucketName}/{pathSave1}");

                request.BucketName = _bucketName;
                request.CannedACL = S3CannedACL.PublicRead;
                request.InputStream = inputStreamOri;
                request.Key = fileName;
                request.Headers.ContentType = contentType;

                var res = await _amazonS3Client.PutObjectAsync(request);
                // small
                var request2 = new PutObjectRequest();
                string _bucketName2 = String.Format($"{BucketName}/{pathSave2}");

                request.BucketName = _bucketName2;
                request.CannedACL = S3CannedACL.PublicRead;
                request.InputStream = inputStreamSmall;
                request.Key = fileName;
                request.Headers.ContentType = contentType;
                var res2 = await _amazonS3Client.PutObjectAsync(request2);

                // thumb
                var request3 = new PutObjectRequest();
                string _bucketName3 = String.Format($"{BucketName}/{pathSave3}");

                request.BucketName = _bucketName3;
                request.CannedACL = S3CannedACL.PublicRead;
                request.InputStream = inputStreamThumb;
                request.Key = fileName;
                request.Headers.ContentType = contentType;

                var res3 = await _amazonS3Client.PutObjectAsync(request3);


            }
            catch (Exception ex)
            {
                // log or throw;
            }

        }
        public static async Task UploadS3Base64(string fileName, string pathSave, byte[] baseImage, string contentType)
        {
           
                AmazonS3Config config = new AmazonS3Config();
                config.ServiceURL = ServiceURL;
                AmazonS3Client _amazonS3Client = new AmazonS3Client(
                        accessKey,
                        secretKey,
                        config
                        );
                var fileTransferUtility = new TransferUtility(_amazonS3Client);

                var request = new PutObjectRequest();
                string _bucketName = String.Format($"{BucketName}/{pathSave}");

                request.BucketName = _bucketName;
                request.CannedACL = S3CannedACL.PublicRead;
                request.Key = fileName;
                request.Headers.ContentType = contentType;
                var inputStream = new MemoryStream(baseImage);                
                inputStream.Position = 0;                    
                request.InputStream = inputStream;
            try
            {
                await _amazonS3Client.PutObjectAsync(request);
            }
            catch (Exception ex)
            {
                // log or throw;
            }
            finally
            {
                inputStream.Dispose();
            }

        }
        public static Stream ResizeImageStream(Stream inputStream, int WidthLimit, int HeightLimit)
        {

            try
            {
                var filePathWaterMark = HttpContext.Current.Server.MapPath("~/Data/watermark/watermark_1_499.png");

                using (System.Drawing.Image image = System.Drawing.Image.FromStream(inputStream))
                {
                    var switchValue = image.Width / 513;

                    switch (switchValue)
                    {
                        case 0: // 1 - 499
                            filePathWaterMark = HttpContext.Current.Server.MapPath("~/Data/watermark/watermark_1_499.png");
                            break;
                        case 1: // 500 999
                            filePathWaterMark = HttpContext.Current.Server.MapPath("~/Data/watermark/watermark_500_999.png");
                            break;
                        case 2: // 1000 1499
                            filePathWaterMark = HttpContext.Current.Server.MapPath("~/Data/watermark/watermark_1000_1499.png");
                            break;
                        case 3: //1500 1999
                            filePathWaterMark = HttpContext.Current.Server.MapPath("~/Data/watermark/watermark_1500_1999.png");
                            break;
                        case 4: // 2000 2499
                            filePathWaterMark = HttpContext.Current.Server.MapPath("~/Data/watermark/watermark_2000_2499.png");
                            break;
                        case 5: // 2500 2999
                            filePathWaterMark = HttpContext.Current.Server.MapPath("~/Data/watermark/watermark_2500_2999.png");
                            break;
                        case 6: // 3000 3499
                            filePathWaterMark = HttpContext.Current.Server.MapPath("~/Data/watermark/watermark_3000_3499.png");
                            break;
                        case 7: // 3500 3999
                            filePathWaterMark = HttpContext.Current.Server.MapPath("~/Data/watermark/watermark_3500_3999.png");
                            break;
                        default: // > 4000
                            filePathWaterMark = HttpContext.Current.Server.MapPath("~/Data/watermark/watermark_1000.png");
                            break;
                    }
                }


                using (System.Drawing.Image image = System.Drawing.Image.FromStream(inputStream))
                using (System.Drawing.Image watermarkImage = System.Drawing.Image.FromFile(filePathWaterMark))
                using (Graphics imageGraphics = Graphics.FromImage(image))
                using (TextureBrush watermarkBrush = new TextureBrush(watermarkImage))
                {
                    int x = (image.Width * 3 / 4 - watermarkImage.Width / 2);
                    int y = (image.Height / 4 - watermarkImage.Height / 2);
                    watermarkBrush.TranslateTransform(x, y);
                    imageGraphics.FillRectangle(watermarkBrush, new Rectangle(new Point(x, y), new Size(watermarkImage.Width + 1, watermarkImage.Height)));

                    inputStream =  GetStream(image, System.Drawing.Imaging.ImageFormat.Png);


                }
            }
            catch (Exception ex)
            {
                return inputStream;
            }

            ImageFormat Format;
            System.Drawing.Image OrgImg = System.Drawing.Image.FromStream(inputStream);
            int DesWidth = OrgImg.Width;
            int DesHeight = OrgImg.Height;
            double ratio = (double)DesWidth / (double)DesHeight;
            Format = OrgImg.RawFormat;
            if (DesWidth <= WidthLimit && DesHeight <= HeightLimit)
            {
                return inputStream;
            }

            if (DesWidth > WidthLimit)
            {
                DesWidth = WidthLimit;
                DesHeight = (int)Math.Round(DesWidth / ratio);
            }

            if (DesHeight > HeightLimit)
            {
                DesHeight = HeightLimit;
                DesWidth = (int)Math.Round(DesHeight * ratio);
            }

            Bitmap DesImg = new System.Drawing.Bitmap(DesWidth, DesHeight, PixelFormat.Format24bppRgb);
            DesImg.SetResolution(96, 96);

            Graphics GraphicImg = Graphics.FromImage(DesImg);
            GraphicImg.SmoothingMode = SmoothingMode.AntiAlias;
            GraphicImg.InterpolationMode = InterpolationMode.HighQualityBicubic;
            GraphicImg.PixelOffsetMode = PixelOffsetMode.HighQuality;
            System.Drawing.Rectangle oRectangle = new Rectangle(0, 0, DesWidth, DesHeight);
            GraphicImg.DrawImage(OrgImg, oRectangle);
            OrgImg.Dispose();
            MemoryStream memoryStream = new MemoryStream();
            DesImg.Save(memoryStream, Format);
            return memoryStream;
        }
        public static Stream ResizeImageStreamNotWatermark(Stream inputStream, int WidthLimit, int HeightLimit)
        {

            ImageFormat Format;
            System.Drawing.Image OrgImg = System.Drawing.Image.FromStream(inputStream);
            int DesWidth = OrgImg.Width;
            int DesHeight = OrgImg.Height;
            double ratio = (double)DesWidth / (double)DesHeight;
            Format = OrgImg.RawFormat;
            if (DesWidth <= WidthLimit && DesHeight <= HeightLimit)
            {
                return inputStream;
            }

            if (DesWidth > WidthLimit)
            {
                DesWidth = WidthLimit;
                DesHeight = (int)Math.Round(DesWidth / ratio);
            }

            if (DesHeight > HeightLimit)
            {
                DesHeight = HeightLimit;
                DesWidth = (int)Math.Round(DesHeight * ratio);
            }

            Bitmap DesImg = new System.Drawing.Bitmap(DesWidth, DesHeight, PixelFormat.Format24bppRgb);
            DesImg.SetResolution(96, 96);

            Graphics GraphicImg = Graphics.FromImage(DesImg);
            GraphicImg.SmoothingMode = SmoothingMode.AntiAlias;
            GraphicImg.InterpolationMode = InterpolationMode.HighQualityBicubic;
            GraphicImg.PixelOffsetMode = PixelOffsetMode.HighQuality;
            System.Drawing.Rectangle oRectangle = new Rectangle(0, 0, DesWidth, DesHeight);
            GraphicImg.DrawImage(OrgImg, oRectangle);
            OrgImg.Dispose();
            MemoryStream memoryStream = new MemoryStream();
            DesImg.Save(memoryStream, Format);
            return memoryStream;
        }
        public static Stream WaterMarkStream(Stream inputStream)
        {
            try
            {
                var filePathWaterMark = HttpContext.Current.Server.MapPath("~/Data/watermark/watermark_1_499.png");

                using (System.Drawing.Image image = System.Drawing.Image.FromStream(inputStream))
                {
                    var switchValue = image.Width / 513;

                    switch (switchValue)
                    {
                        case 0: // 1 - 499
                            filePathWaterMark = HttpContext.Current.Server.MapPath("~/Data/watermark/watermark_1_499.png");
                            break;
                        case 1: // 500 999
                            filePathWaterMark = HttpContext.Current.Server.MapPath("~/Data/watermark/watermark_500_999.png");
                            break;
                        case 2: // 1000 1499
                            filePathWaterMark = HttpContext.Current.Server.MapPath("~/Data/watermark/watermark_1000_1499.png");
                            break;
                        case 3: //1500 1999
                            filePathWaterMark = HttpContext.Current.Server.MapPath("~/Data/watermark/watermark_1500_1999.png");
                            break;
                        case 4: // 2000 2499
                            filePathWaterMark = HttpContext.Current.Server.MapPath("~/Data/watermark/watermark_2000_2499.png");
                            break;
                        case 5: // 2500 2999
                            filePathWaterMark = HttpContext.Current.Server.MapPath("~/Data/watermark/watermark_2500_2999.png");
                            break;
                        case 6: // 3000 3499
                            filePathWaterMark = HttpContext.Current.Server.MapPath("~/Data/watermark/watermark_3000_3499.png");
                            break;
                        case 7: // 3500 3999
                            filePathWaterMark = HttpContext.Current.Server.MapPath("~/Data/watermark/watermark_3500_3999.png");
                            break;
                        default: // > 4000
                            filePathWaterMark = HttpContext.Current.Server.MapPath("~/Data/watermark/watermark_1000.png");
                            break;
                    }
                }

                using (System.Drawing.Image image = System.Drawing.Image.FromStream(inputStream))
                using (System.Drawing.Image watermarkImage = System.Drawing.Image.FromFile(filePathWaterMark))
                using (Graphics imageGraphics = Graphics.FromImage(image))
                using (TextureBrush watermarkBrush = new TextureBrush(watermarkImage))
                {
                    int x = (image.Width * 3 / 4 - watermarkImage.Width / 2);
                    int y = (image.Height / 4 - watermarkImage.Height / 2);
                    watermarkBrush.TranslateTransform(x, y);
                    imageGraphics.FillRectangle(watermarkBrush, new Rectangle(new Point(x, y), new Size(watermarkImage.Width + 1, watermarkImage.Height)));

                    return GetStream(image, System.Drawing.Imaging.ImageFormat.Png);
                   

                }
            }
            catch (Exception ex)
            {
                return inputStream;
            }
        }

        public static Stream ResizeImageStreamNoWaterMark(Stream inputStream, int WidthLimit, int HeightLimit)
        {
            ImageFormat Format;
            System.Drawing.Image OrgImg = System.Drawing.Image.FromStream(inputStream);
            int DesWidth = OrgImg.Width;
            int DesHeight = OrgImg.Height;
            double ratio = (double)DesWidth / (double)DesHeight;
            Format = OrgImg.RawFormat;
            if (DesWidth <= WidthLimit && DesHeight <= HeightLimit)
            {
                return inputStream;
            }

            if (DesWidth > WidthLimit)
            {
                DesWidth = WidthLimit;
                DesHeight = (int)Math.Round(DesWidth / ratio);
            }

            if (DesHeight > HeightLimit)
            {
                DesHeight = HeightLimit;
                DesWidth = (int)Math.Round(DesHeight * ratio);
            }

            Bitmap DesImg = new System.Drawing.Bitmap(DesWidth, DesHeight, PixelFormat.Format24bppRgb);
            DesImg.SetResolution(96, 96);

            Graphics GraphicImg = Graphics.FromImage(DesImg);
            GraphicImg.SmoothingMode = SmoothingMode.AntiAlias;
            GraphicImg.InterpolationMode = InterpolationMode.HighQualityBicubic;
            GraphicImg.PixelOffsetMode = PixelOffsetMode.HighQuality;
            System.Drawing.Rectangle oRectangle = new Rectangle(0, 0, DesWidth, DesHeight);
            GraphicImg.DrawImage(OrgImg, oRectangle);
            OrgImg.Dispose();
            MemoryStream memoryStream = new MemoryStream();
            DesImg.Save(memoryStream, Format);
            return memoryStream;
        }
        public static Stream GetStream(Image img, ImageFormat format)
        {
            var ms = new MemoryStream();
            img.Save(ms, format);
            return ms;
        }

    }
}
