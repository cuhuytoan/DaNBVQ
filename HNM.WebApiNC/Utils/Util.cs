using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using HNM.DataNC.Models;
using HNM.DataNC.ModelsNC.ModelsDTO;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace HNM.WebApiNC.Utils
{
    public class Util
    {
        public static Dictionary<int, string> LocationDictionary = new Dictionary<int, string> { { -1, "Tất cả tỉnh thành" }, { 1, "An Giang" }, { 2, "Bà Rịa - Vũng Tàu" }, { 3, "Bắc Giang" }, { 4, "Bắc Kạn" }, { 5, "Bạc Liêu" }, { 6, "Bắc Ninh" }, { 7, "Bến Tre" }, { 8, "Bình Định" }, { 9, "Bình Dương" }, { 10, "Bình Phước" }, { 11, "Bình Thuận" }, { 12, "Cà Mau" }, { 13, "Cao Bằng" }, { 14, "Đắk Lắk" }, { 15, "Đắk Nông" }, { 16, "Điện Biên" }, { 17, "Đồng Nai" }, { 18, "Đồng Tháp" }, { 19, "Gia Lai" }, { 20, "Hà Giang" }, { 21, "Hà Nam" }, { 22, "Hà Tĩnh" }, { 23, "Hải Dương" }, { 24, "Hậu Giang" }, { 25, "Hòa Bình" }, { 26, "Hưng Yên" }, { 27, "Khánh Hòa" }, { 28, "Kiên Giang" }, { 29, "Kon Tum" }, { 30, "Lai Châu" }, { 31, "Lâm Đồng" }, { 32, "Lạng Sơn" }, { 33, "Lào Cai" }, { 34, "Long An" }, { 35, "Nam Định" }, { 36, "Nghệ An" }, { 37, "Ninh Bình" }, { 38, "Ninh Thuận" }, { 39, "Phú Thọ" }, { 40, "Quảng Bình" }, { 41, "Quảng Nam" }, { 42, "Quảng Ngãi" }, { 43, "Quảng Ninh" }, { 44, "Quảng Trị" }, { 45, "Sóc Trăng" }, { 46, "Sơn La" }, { 47, "Tây Ninh" }, { 48, "Thái Bình" }, { 49, "Thái Nguyên" }, { 50, "Thanh Hóa" }, { 51, "Thừa Thiên Huế" }, { 52, "Tiền Giang" }, { 53, "Trà Vinh" }, { 54, "Tuyên Quang" }, { 55, "Vĩnh Long" }, { 56, "Vĩnh Phúc" }, { 57, "Yên Bái" }, { 58, "Phú Yên" }, { 59, "Cần Thơ" }, { 60, "Đà Nẵng" }, { 61, "Hải Phòng" }, { 62, "Hà Nội" }, { 63, "TP HCM" } };
        public static string accessKey = "W1H820TV8PYDK6I3C3K0";
        public static string secretKey = "HwSPUnmegVksiOXN8GvRe1Lv2m40kKRDHGqayv9v";
        public static string ServiceURL = "https://s3.cloud.cmctelecom.vn";
        public static string BucketName = "hanoma-cdn";
        public static bool IsEmailOrPhone(string input)
        {
            if (IsEmail(input)) return true;
            if (IsPhoneNumber(input)) return true;
            return false;
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

        //public static async Task<string> SendMessageAndroid(string Title, string Body, string Content, int? NotifyType,  string Topic, int? FormId, int? ParameterId, string FullUrl,string FullUrlImage, int? ProductTypeId)
        //{
        //    WebRequest tRequest;
        //    //thiết lập FCM send
        //    tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
        //    tRequest.Method = "POST";
        //    tRequest.UseDefaultCredentials = true;
        //    tRequest.PreAuthenticate = true;
        //    tRequest.Credentials = CredentialCache.DefaultNetworkCredentials;

        //    //định dạng JSON
        //    tRequest.ContentType = "application/json";
        //    tRequest.Headers.Add(string.Format("Authorization: key={0}", "AIzaSyChBVls7mr2oXkXuIQlkfZze6MZb6sXU3w"));
        //    tRequest.Headers.Add(string.Format("Sender: id={0}", "636270236020"));

        //    // Generate Json
        //    string PostData = GenerateJsonContentToppic(Title, Body, Content, NotifyType, Topic,FormId,ParameterId,FullUrl,FullUrlImage,ProductTypeId);

        //    Byte[] byteArray = Encoding.UTF8.GetBytes(PostData);
        //    tRequest.ContentLength = byteArray.Length;

        //    Stream dataStream = await tRequest.GetRequestStreamAsync();
        //    dataStream.Write(byteArray, 0, byteArray.Length);
        //    dataStream.Close();

        //    WebResponse tResponse = tRequest.GetResponse();

        //    dataStream =  tResponse.GetResponseStream();

        //    StreamReader tReader = new StreamReader(dataStream);

        //    string sResponseFromServer =  tReader.ReadToEnd();

        //    tReader.Close();
        //    dataStream.Close();
        //    tResponse.Close();
        //    return  sResponseFromServer;
        //}
        public static async Task<string> SendMessageFirebase(FCMMessageOutputDTO model)
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                var defaultApp = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FBkey.json")),
                });
            }

            //Convert Data to List Dictionnary
            var jsonData = JsonConvert.SerializeObject(model.Data);
            var dictionaryData = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonData);
            var message = new Message()
            {
                Data = dictionaryData,
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = model.Notification.Title,
                    Body = model.Notification.Body
                },
                // Token = model.Token
                Topic = model.Topic
            };
            var messaging = FirebaseMessaging.DefaultInstance;
            var result = await messaging.SendAsync(message);
            return result;
        }

        public static async Task<BatchResponse> SendMessageFirebaseChatViaTokken(FCMMessageChatDTO model)
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                var defaultApp = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FBkey.json")),
                });
            }

            //Convert Data to List Dictionnary
            var jsonData = JsonConvert.SerializeObject(model.Data);
            var dictionaryData = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonData);
            var message = new MulticastMessage()
            {
                Data = dictionaryData,
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = model.Notification.Title,
                    Body = model.Notification.Body
                },
                Tokens = model.Token                
            };
            var messaging = FirebaseMessaging.DefaultInstance;
            var result = await messaging.SendMulticastAsync(message);
            return result;
        }

        public static string GenerateJsonContentToppic(string Title, string Body, string Content, int? NotifyType, string Topic, string FormId, int? ParameterId, string FullUrl, string FullUrlImage, int? ProductTypeId)
        {
            string Result = "";
            FCMMessageOutputDTO FCMMessageV2Output_Item = new FCMMessageOutputDTO();

            FCMMessageV2Output_Item.Notification.Title = Title;
            FCMMessageV2Output_Item.Notification.Body = Body;
            FCMMessageV2Output_Item.Data.notifyType = NotifyType ?? 0;
            FCMMessageV2Output_Item.Data.formId = FormId;
            FCMMessageV2Output_Item.Data.id = ParameterId ?? 0;
            FCMMessageV2Output_Item.Data.fullUrl = FullUrl;
            FCMMessageV2Output_Item.Data.fullUrlImage = FullUrlImage;
            FCMMessageV2Output_Item.Data.typeId = ProductTypeId;
            Result = JsonConvert.SerializeObject(FCMMessageV2Output_Item);

            return Result;
        }
        public static string SendSMS(string SMSContent, string PhoneNumber)
        {
            string post_response;
            Dictionary<string, string> post_values = new Dictionary<string, string>();
            post_values.Add("action", "create");
            post_values.Add("token", "1c73c0b3b86e6f64ffd40d998ef1baf8");
            post_values.Add("title", "Tin nhan tu he thong");
            post_values.Add("type", "cus");
            post_values.Add("brandName", "HANOMA.VN");
            post_values.Add("scheduleDate", "");
            post_values.Add("message", SMSContent);
            post_values.Add("phones", PhoneNumber);

            String post_string = "";
            foreach (var post_value in post_values)
            {
                post_string += post_value.Key + "=" + HttpUtility.UrlEncode(post_value.Value) + "&";
            }
            post_string = post_string.TrimEnd('&');

            string VOID_URL = "https://crm.pavietnam.vn/api/sms/campaign";
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(VOID_URL);
            objRequest.Method = "POST";
            objRequest.ContentLength = post_string.Length;
            objRequest.ContentType = "application/x-www-form-urlencoded";

            // post data is sent as a stream
            StreamWriter myWriter = null;
            myWriter = new StreamWriter(objRequest.GetRequestStream());
            myWriter.Write(post_string);
            myWriter.Close();

            // returned values are returned as a stream, then read into a string
            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            using (StreamReader responseStream = new StreamReader(objResponse.GetResponseStream()))
            {
                post_response = responseStream.ReadToEnd();
                responseStream.Close();
            }

            return post_response;
        }

        public static void SendMail(string FromName, string ToEmail, string ToName, string Subject, string Body, Setting setting)
        {


            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = setting.EmailSenderSMTP;
            smtpClient.Port = int.Parse(setting.EmailSenderPort);
            smtpClient.EnableSsl = setting.EmailSenderSSL.Value;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            smtpClient.Credentials = new NetworkCredential(setting.EmailSender, setting.EmailSenderPassword);
            smtpClient.Timeout = 20000;

            MailAddress FromAddress = new MailAddress(setting.EmailSender, setting.AdminName);
            MailAddress ToAddress = new MailAddress(ToEmail, ToName);
            MailMessage Mailer = new MailMessage(FromAddress, ToAddress);
            Mailer.IsBodyHtml = true;
            Mailer.BodyEncoding = System.Text.Encoding.UTF8;
            Mailer.Subject = Subject;
            Mailer.IsBodyHtml = false;
            Mailer.Body = Body;
            smtpClient.Send(Mailer);
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
        public static void WaterMark(string filePath, string imageName)
        {
            try
            {


                var PatchAbsolute = @"E:\";
                var filePathTemp = Path.Combine(PatchAbsolute, "Temp", imageName);
                var filePathWaterMark = Path.Combine(PatchAbsolute, "watermark/watermark_1_499.png");


                using (System.Drawing.Image image = System.Drawing.Image.FromFile(filePath))
                {
                    var switchValue = image.Width / 513;

                    switch (switchValue)
                    {
                        case 0: // 1 - 499                            
                            filePathWaterMark = Path.Combine(PatchAbsolute, "watermark/watermark_1_499.png");
                            break;
                        case 1: // 500 999                            
                            filePathWaterMark = Path.Combine(PatchAbsolute, "watermark/watermark_500_999.png");
                            break;
                        case 2: // 1000 1499                            
                            filePathWaterMark = Path.Combine(PatchAbsolute, "watermark/watermark_1000_1499.png");
                            break;
                        case 3: //1500 1999                            
                            filePathWaterMark = Path.Combine(PatchAbsolute, "watermark/watermark_1500_1999.png");
                            break;
                        case 4: // 2000 2499                            
                            filePathWaterMark = Path.Combine(PatchAbsolute, "watermark/watermark_2000_2499.png");
                            break;
                        case 5: // 2500 2999
                            filePathWaterMark = Path.Combine(PatchAbsolute, "watermark/watermark_2500_2999.png");
                            break;
                        case 6: // 3000 3499
                            filePathWaterMark = Path.Combine(PatchAbsolute, "watermark/watermark_3000_3499.png");
                            break;
                        case 7: // 3500 3999
                            filePathWaterMark = Path.Combine(PatchAbsolute, "watermark/watermark_3500_3999.png");
                            break;
                        default: // > 4000
                            filePathWaterMark = Path.Combine(PatchAbsolute, "watermark/watermark_1000.png");
                            break;
                    }
                }


                using (System.Drawing.Image image = System.Drawing.Image.FromFile(filePath))
                using (System.Drawing.Image watermarkImage = System.Drawing.Image.FromFile(filePathWaterMark))
                using (Graphics imageGraphics = Graphics.FromImage(image))
                using (TextureBrush watermarkBrush = new TextureBrush(watermarkImage))
                {
                    int x = (image.Width / 2 - watermarkImage.Width / 2);
                    int y = (image.Height / 2 - watermarkImage.Height / 2);
                    watermarkBrush.TranslateTransform(x, y);
                    imageGraphics.FillRectangle(watermarkBrush, new Rectangle(new Point(x, y), new Size(watermarkImage.Width + 1, watermarkImage.Height)));



                    image.Save(filePathTemp);

                }
                MoveFile(filePathTemp, filePath);
            }
            catch (Exception ex)
            {
                var res = ex;
            }
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

                _amazonS3Client.PutObjectAsync(request);
            }
            catch (Exception ex)
            {
                // log or throw;
            }

        }
               public static Stream ResizeImageStream(Stream inputStream, int WidthLimit, int HeightLimit)
        {

            try
            {
                var PatchAbsolute = @"E:\";
                var filePathWaterMark = Path.Combine(PatchAbsolute, "watermark/watermark_1_499.png");

                using (System.Drawing.Image image = System.Drawing.Image.FromStream(inputStream))
                {
                    var switchValue = image.Width / 513;

                    switch (switchValue)
                    {
                        case 0: // 1 - 499                            
                            filePathWaterMark = Path.Combine(PatchAbsolute, "watermark/watermark_1_499.png");
                            break;
                        case 1: // 500 999                            
                            filePathWaterMark = Path.Combine(PatchAbsolute, "watermark/watermark_500_999.png");
                            break;
                        case 2: // 1000 1499                            
                            filePathWaterMark = Path.Combine(PatchAbsolute, "watermark/watermark_1000_1499.png");
                            break;
                        case 3: //1500 1999                            
                            filePathWaterMark = Path.Combine(PatchAbsolute, "watermark/watermark_1500_1999.png");
                            break;
                        case 4: // 2000 2499                            
                            filePathWaterMark = Path.Combine(PatchAbsolute, "watermark/watermark_2000_2499.png");
                            break;
                        case 5: // 2500 2999
                            filePathWaterMark = Path.Combine(PatchAbsolute, "watermark/watermark_2500_2999.png");
                            break;
                        case 6: // 3000 3499
                            filePathWaterMark = Path.Combine(PatchAbsolute, "watermark/watermark_3000_3499.png");
                            break;
                        case 7: // 3500 3999
                            filePathWaterMark = Path.Combine(PatchAbsolute, "watermark/watermark_3500_3999.png");
                            break;
                        default: // > 4000
                            filePathWaterMark = Path.Combine(PatchAbsolute, "watermark/watermark_1000.png");
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
                throw ex;
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
        public static Stream WaterMarkStream(Stream inputStream)
        {
            try
            {
                var PatchAbsolute = @"E:\";
                var filePathWaterMark = Path.Combine(Directory.GetCurrentDirectory(), "Data", "watermark", "watermark_1_499.png");

                using (System.Drawing.Image image = System.Drawing.Image.FromStream(inputStream))
                {
                    var switchValue = image.Width / 513;

                    switch (switchValue)
                    {
                        case 0: // 1 - 499                            
                            filePathWaterMark = Path.Combine(PatchAbsolute, "watermark/watermark_1_499.png");
                            break;
                        case 1: // 500 999                            
                            filePathWaterMark = Path.Combine(PatchAbsolute, "watermark/watermark_500_999.png");
                            break;
                        case 2: // 1000 1499                            
                            filePathWaterMark = Path.Combine(PatchAbsolute, "watermark/watermark_1000_1499.png");
                            break;
                        case 3: //1500 1999                            
                            filePathWaterMark = Path.Combine(PatchAbsolute, "watermark/watermark_1500_1999.png");
                            break;
                        case 4: // 2000 2499                            
                            filePathWaterMark = Path.Combine(PatchAbsolute, "watermark/watermark_2000_2499.png");
                            break;
                        case 5: // 2500 2999
                            filePathWaterMark = Path.Combine(PatchAbsolute, "watermark/watermark_2500_2999.png");
                            break;
                        case 6: // 3000 3499
                            filePathWaterMark = Path.Combine(PatchAbsolute, "watermark/watermark_3000_3499.png");
                            break;
                        case 7: // 3500 3999
                            filePathWaterMark = Path.Combine(PatchAbsolute, "watermark/watermark_3500_3999.png");
                            break;
                        default: // > 4000
                            filePathWaterMark = Path.Combine(PatchAbsolute, "watermark/watermark_1000.png");
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
                throw ex;
            }
        }
          public static Stream GetStream(Image img, ImageFormat format)
        {
            var ms = new MemoryStream();
            img.Save(ms, format);
            return ms;
        }
        public static DistributedCacheEntryOptions RedisOptions()
        {
            return new DistributedCacheEntryOptions()
             .SetSlidingExpiration(TimeSpan.FromMinutes(5))
             .SetAbsoluteExpiration(TimeSpan.FromHours(3));
        }

    }

}
