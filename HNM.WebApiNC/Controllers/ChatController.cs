using AutoMapper;
using HNM.DataNC.Models;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsStore;
using HNM.RepositoriesNC.RepositoriesBase;
using HNM.WebApiNC.Logging;
using HNM.WebApiNC.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
namespace HNM.WebApiNC.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class ChatController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        public ChatController(IRepositoryWrapper repoWrapper, ILoggerManager logger, IMapper mapper, IDistributedCache distributedCache)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }
        /// <summary>
        /// UpLoad file chat
        /// Path File Ảnh "chat/images" 
        /// Path File "chat/files"
        /// </summary>
        /// <param name="model"></param>
        /// <param name="pathMain"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<UrlReturnUploadDTO>> UploadChatFile(List<ImageUploadDTO> model)
        {
            var output = new List<UrlReturnUploadDTO>();
            foreach( var p in model)
            {
                var item = new UrlReturnUploadDTO();
                if (p.Base64 != null)
                {
                    var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                    p.FileName = $"{p.FileName.Split(".").First()}-{timestamp}.{p.FileName.Split(".").Last()}";
                    await UploadChatFileS3(p, p.PathSave);
                    item.UrlFile = "https://hanoma-cdn.s3.cloud.cmctelecom.vn/" + p.PathSave + "/" + p.FileName;
                    item.FileName = p.FileName;
                    item.FileSize = p.FileSize;
                    item.ChatType = p.ChatType;
                }
                else
                {
                    item.ErrorCode = "01";
                    item.Message = "File upload rỗng";
                }
                output.Add(item);
            }    
            
            return output;

        }


        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<bool> UploadChatFileS3(ImageUploadDTO model, string pathMain)
        {
            try
            {
                var imageDataByteArray = Convert.FromBase64String(model.Base64);
                var imageDataStream = new MemoryStream(imageDataByteArray);
                imageDataStream.Position = 0;
              
                var file = File(imageDataByteArray, model.ExtensionType);
                var fileName = model.FileName;
                if (file.FileContents.Length > 0)
                {
                    Util.UploadS3(model.FileName, pathMain, imageDataStream, model.ExtensionType);                    
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Upload Product Image: " + ex.ToString());
                return false;
            }
        }

        public class ContentTypeS3
        {
            public string fileType { get; set; }
            public string ContentType { get; set; }
        }
        private List<ContentTypeS3> GetListContentType()
        {
            var output = new List<ContentTypeS3>();
            output.Add(new ContentTypeS3 { fileType = "3g2", ContentType = "video/3gpp2" });
            output.Add(new ContentTypeS3 { fileType = "3gp", ContentType = "video/3gpp" });
            output.Add(new ContentTypeS3 { fileType = "3gp2", ContentType = "video/3gpp2" });
            output.Add(new ContentTypeS3 { fileType = "3gpp", ContentType = "video/3gpp" });
            output.Add(new ContentTypeS3 { fileType = "aa", ContentType = "audio/audible" });
            output.Add(new ContentTypeS3 { fileType = "aac", ContentType = "audio/vnd.dlna.adts" });
            output.Add(new ContentTypeS3 { fileType = "aax", ContentType = "audio/vnd.audible.aax" });
            output.Add(new ContentTypeS3 { fileType = "addin", ContentType = "text/xml" });
            output.Add(new ContentTypeS3 { fileType = "adt", ContentType = "audio/vnd.dlna.adts" });
            output.Add(new ContentTypeS3 { fileType = "adts", ContentType = "audio/vnd.dlna.adts" });
            output.Add(new ContentTypeS3 { fileType = "ai", ContentType = "application/postscript" });
            output.Add(new ContentTypeS3 { fileType = "aif", ContentType = "audio/aiff" });
            output.Add(new ContentTypeS3 { fileType = "aifc", ContentType = "audio/aiff" });
            output.Add(new ContentTypeS3 { fileType = "aiff", ContentType = "audio/aiff" });
            output.Add(new ContentTypeS3 { fileType = "application", ContentType = "application/x-ms-application" });
            output.Add(new ContentTypeS3 { fileType = "asax", ContentType = "application/xml" });
            output.Add(new ContentTypeS3 { fileType = "ascx", ContentType = "application/xml" });
            output.Add(new ContentTypeS3 { fileType = "asf", ContentType = "video/x-ms-asf" });
            output.Add(new ContentTypeS3 { fileType = "ashx", ContentType = "application/xml" });
            output.Add(new ContentTypeS3 { fileType = "asmx", ContentType = "application/xml" });
            output.Add(new ContentTypeS3 { fileType = "aspx", ContentType = "application/xml" });
            output.Add(new ContentTypeS3 { fileType = "asx", ContentType = "video/x-ms-asf" });
            output.Add(new ContentTypeS3 { fileType = "au", ContentType = "audio/basic" });
            output.Add(new ContentTypeS3 { fileType = "avi", ContentType = "video/avi" });
            output.Add(new ContentTypeS3 { fileType = "bmp", ContentType = "image/bmp" });
            output.Add(new ContentTypeS3 { fileType = "btapp", ContentType = "application/x-bittorrent-app" });
            output.Add(new ContentTypeS3 { fileType = "btinstall", ContentType = "application/x-bittorrent-appinst" });
            output.Add(new ContentTypeS3 { fileType = "btkey", ContentType = "application/x-bittorrent-key" });
            output.Add(new ContentTypeS3 { fileType = "btsearch", ContentType = "application/x-bittorrentsearchdescription+xml" });
            output.Add(new ContentTypeS3 { fileType = "btskin", ContentType = "application/x-bittorrent-skin" });
            output.Add(new ContentTypeS3 { fileType = "cat", ContentType = "application/vnd.ms-pki.seccat" });
            output.Add(new ContentTypeS3 { fileType = "cd", ContentType = "text/plain" });
            output.Add(new ContentTypeS3 { fileType = "cer", ContentType = "application/x-x509-ca-cert" });
            output.Add(new ContentTypeS3 { fileType = "config", ContentType = "application/xml" });
            output.Add(new ContentTypeS3 { fileType = "contact", ContentType = "text/x-ms-contact" });
            output.Add(new ContentTypeS3 { fileType = "crl", ContentType = "application/pkix-crl" });
            output.Add(new ContentTypeS3 { fileType = "crt", ContentType = "application/x-x509-ca-cert" });
            output.Add(new ContentTypeS3 { fileType = "cs", ContentType = "text/plain" });
            output.Add(new ContentTypeS3 { fileType = "csproj", ContentType = "text/plain" });
            output.Add(new ContentTypeS3 { fileType = "css", ContentType = "text/css" });
            output.Add(new ContentTypeS3 { fileType = "csv", ContentType = "application/vnd.ms-excel" });
            output.Add(new ContentTypeS3 { fileType = "datasource", ContentType = "application/xml" });
            output.Add(new ContentTypeS3 { fileType = "der", ContentType = "application/x-x509-ca-cert" });
            output.Add(new ContentTypeS3 { fileType = "dib", ContentType = "image/bmp" });
            output.Add(new ContentTypeS3 { fileType = "dll", ContentType = "application/x-msdownload" });
            output.Add(new ContentTypeS3 { fileType = "doc", ContentType = "application/msword" });
            output.Add(new ContentTypeS3 { fileType = "docm", ContentType = "application/vnd.ms-word.document.macroEnabled.12" });
            output.Add(new ContentTypeS3 { fileType = "docx", ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document" });
            output.Add(new ContentTypeS3 { fileType = "dot", ContentType = "application/msword" });
            output.Add(new ContentTypeS3 { fileType = "dotm", ContentType = "application/vnd.ms-word.template.macroEnabled.12" });
            output.Add(new ContentTypeS3 { fileType = "dotx", ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.template" });
            output.Add(new ContentTypeS3 { fileType = "dtd", ContentType = "application/xml-dtd" });
            output.Add(new ContentTypeS3 { fileType = "dtsconfig", ContentType = "text/xml" });
            output.Add(new ContentTypeS3 { fileType = "eps", ContentType = "application/postscript" });
            output.Add(new ContentTypeS3 { fileType = "exe", ContentType = "application/x-msdownload" });
            output.Add(new ContentTypeS3 { fileType = "fdf", ContentType = "application/vnd.fdf" });
            output.Add(new ContentTypeS3 { fileType = "fif", ContentType = "application/fractals" });
            output.Add(new ContentTypeS3 { fileType = "gif", ContentType = "image/gif" });
            output.Add(new ContentTypeS3 { fileType = "group", ContentType = "text/x-ms-group" });
            output.Add(new ContentTypeS3 { fileType = "hdd", ContentType = "application/x-virtualbox-hdd" });
            output.Add(new ContentTypeS3 { fileType = "hqx", ContentType = "application/mac-binhex40" });
            output.Add(new ContentTypeS3 { fileType = "hta", ContentType = "application/hta" });
            output.Add(new ContentTypeS3 { fileType = "htc", ContentType = "text/x-component" });
            output.Add(new ContentTypeS3 { fileType = "htm", ContentType = "text/html" });
            output.Add(new ContentTypeS3 { fileType = "html", ContentType = "text/html" });
            output.Add(new ContentTypeS3 { fileType = "hxa", ContentType = "application/xml" });
            output.Add(new ContentTypeS3 { fileType = "hxc", ContentType = "application/xml" });
            output.Add(new ContentTypeS3 { fileType = "hxd", ContentType = "application/octet-stream" });
            output.Add(new ContentTypeS3 { fileType = "hxe", ContentType = "application/xml" });
            output.Add(new ContentTypeS3 { fileType = "hxf", ContentType = "application/xml" });
            output.Add(new ContentTypeS3 { fileType = "hxh", ContentType = "application/octet-stream" });
            output.Add(new ContentTypeS3 { fileType = "hxi", ContentType = "application/octet-stream" });
            output.Add(new ContentTypeS3 { fileType = "hxk", ContentType = "application/xml" });
            output.Add(new ContentTypeS3 { fileType = "hxq", ContentType = "application/octet-stream" });
            output.Add(new ContentTypeS3 { fileType = "hxr", ContentType = "application/octet-stream" });
            output.Add(new ContentTypeS3 { fileType = "hxs", ContentType = "application/octet-stream" });
            output.Add(new ContentTypeS3 { fileType = "hxt", ContentType = "application/xml" });
            output.Add(new ContentTypeS3 { fileType = "hxv", ContentType = "application/xml" });
            output.Add(new ContentTypeS3 { fileType = "hxw", ContentType = "application/octet-stream" });
            output.Add(new ContentTypeS3 { fileType = "ico", ContentType = "image/x-icon" });
            output.Add(new ContentTypeS3 { fileType = "ics", ContentType = "text/calendar" });
            output.Add(new ContentTypeS3 { fileType = "ipa", ContentType = "application/x-itunes-ipa" });
            output.Add(new ContentTypeS3 { fileType = "ipg", ContentType = "application/x-itunes-ipg" });
            output.Add(new ContentTypeS3 { fileType = "ipsw", ContentType = "application/x-itunes-ipsw" });
            output.Add(new ContentTypeS3 { fileType = "iqy", ContentType = "text/x-ms-iqy" });
            output.Add(new ContentTypeS3 { fileType = "iss", ContentType = "text/plain" });
            output.Add(new ContentTypeS3 { fileType = "ite", ContentType = "application/x-itunes-ite" });
            output.Add(new ContentTypeS3 { fileType = "itlp", ContentType = "application/x-itunes-itlp" });
            output.Add(new ContentTypeS3 { fileType = "itls", ContentType = "application/x-itunes-itls" });
            output.Add(new ContentTypeS3 { fileType = "itms", ContentType = "application/x-itunes-itms" });
            output.Add(new ContentTypeS3 { fileType = "itpc", ContentType = "application/x-itunes-itpc" });
            output.Add(new ContentTypeS3 { fileType = "jfif", ContentType = "image/jpeg" });
            output.Add(new ContentTypeS3 { fileType = "jnlp", ContentType = "application/x-java-jnlp-file" });
            output.Add(new ContentTypeS3 { fileType = "jpe", ContentType = "image/jpeg" });
            output.Add(new ContentTypeS3 { fileType = "jpeg", ContentType = "image/jpeg" });
            output.Add(new ContentTypeS3 { fileType = "jpg", ContentType = "image/jpeg" });
            output.Add(new ContentTypeS3 { fileType = "js", ContentType = "application/javascript" });
            output.Add(new ContentTypeS3 { fileType = "latex", ContentType = "application/x-latex" });
            output.Add(new ContentTypeS3 { fileType = "library-ms", ContentType = "application/windows-library+xml" });
            output.Add(new ContentTypeS3 { fileType = "m1v", ContentType = "video/mpeg" });
            output.Add(new ContentTypeS3 { fileType = "m2t", ContentType = "video/vnd.dlna.mpeg-tts" });
            output.Add(new ContentTypeS3 { fileType = "m2ts", ContentType = "video/vnd.dlna.mpeg-tts" });
            output.Add(new ContentTypeS3 { fileType = "m2v", ContentType = "video/mpeg" });
            output.Add(new ContentTypeS3 { fileType = "m3u", ContentType = "audio/mpegurl" });
            output.Add(new ContentTypeS3 { fileType = "m3u8", ContentType = "audio/x-mpegurl" });
            output.Add(new ContentTypeS3 { fileType = "m4a", ContentType = "audio/m4a" });
            output.Add(new ContentTypeS3 { fileType = "m4b", ContentType = "audio/m4b" });
            output.Add(new ContentTypeS3 { fileType = "m4p", ContentType = "audio/m4p" });
            output.Add(new ContentTypeS3 { fileType = "m4r", ContentType = "audio/x-m4r" });
            output.Add(new ContentTypeS3 { fileType = "m4v", ContentType = "video/x-m4v" });
            output.Add(new ContentTypeS3 { fileType = "magnet", ContentType = "application/x-magnet" });
            output.Add(new ContentTypeS3 { fileType = "man", ContentType = "application/x-troff-man" });
            output.Add(new ContentTypeS3 { fileType = "master", ContentType = "application/xml" });
            output.Add(new ContentTypeS3 { fileType = "mht", ContentType = "message/rfc822" });
            output.Add(new ContentTypeS3 { fileType = "mhtml", ContentType = "message/rfc822" });
            output.Add(new ContentTypeS3 { fileType = "mid", ContentType = "audio/mid" });
            output.Add(new ContentTypeS3 { fileType = "midi", ContentType = "audio/mid" });
            output.Add(new ContentTypeS3 { fileType = "mod", ContentType = "video/mpeg" });
            output.Add(new ContentTypeS3 { fileType = "mov", ContentType = "video/quicktime" });
            output.Add(new ContentTypeS3 { fileType = "mp2", ContentType = "audio/mpeg" });
            output.Add(new ContentTypeS3 { fileType = "mp2v", ContentType = "video/mpeg" });
            output.Add(new ContentTypeS3 { fileType = "mp3", ContentType = "audio/mpeg" });
            output.Add(new ContentTypeS3 { fileType = "mp4", ContentType = "video/mp4" });
            output.Add(new ContentTypeS3 { fileType = "mp4v", ContentType = "video/mp4" });
            output.Add(new ContentTypeS3 { fileType = "mpa", ContentType = "video/mpeg" });
            output.Add(new ContentTypeS3 { fileType = "mpe", ContentType = "video/mpeg" });
            output.Add(new ContentTypeS3 { fileType = "mpeg", ContentType = "video/mpeg" });
            output.Add(new ContentTypeS3 { fileType = "mpf", ContentType = "application/vnd.ms-mediapackage" });
            output.Add(new ContentTypeS3 { fileType = "mpg", ContentType = "video/mpeg" });
            output.Add(new ContentTypeS3 { fileType = "mpv2", ContentType = "video/mpeg" });
            output.Add(new ContentTypeS3 { fileType = "mts", ContentType = "video/vnd.dlna.mpeg-tts" });
            output.Add(new ContentTypeS3 { fileType = "odc", ContentType = "text/x-ms-odc" });
            output.Add(new ContentTypeS3 { fileType = "odg", ContentType = "application/vnd.oasis.opendocument.graphics" });
            output.Add(new ContentTypeS3 { fileType = "odm", ContentType = "application/vnd.oasis.opendocument.text-master" });
            output.Add(new ContentTypeS3 { fileType = "odp", ContentType = "application/vnd.oasis.opendocument.presentation" });
            output.Add(new ContentTypeS3 { fileType = "ods", ContentType = "application/vnd.oasis.opendocument.spreadsheet" });
            output.Add(new ContentTypeS3 { fileType = "odt", ContentType = "application/vnd.oasis.opendocument.text" });
            output.Add(new ContentTypeS3 { fileType = "otg", ContentType = "application/vnd.oasis.opendocument.graphics-template" });
            output.Add(new ContentTypeS3 { fileType = "oth", ContentType = "application/vnd.oasis.opendocument.text-web" });
            output.Add(new ContentTypeS3 { fileType = "ots", ContentType = "application/vnd.oasis.opendocument.spreadsheet-template" });
            output.Add(new ContentTypeS3 { fileType = "ott", ContentType = "application/vnd.oasis.opendocument.text-template" });
            output.Add(new ContentTypeS3 { fileType = "ova", ContentType = "application/x-virtualbox-ova" });
            output.Add(new ContentTypeS3 { fileType = "ovf", ContentType = "application/x-virtualbox-ovf" });
            output.Add(new ContentTypeS3 { fileType = "oxt", ContentType = "application/vnd.openofficeorg.extension" });
            output.Add(new ContentTypeS3 { fileType = "p10", ContentType = "application/pkcs10" });
            output.Add(new ContentTypeS3 { fileType = "p12", ContentType = "application/x-pkcs12" });
            output.Add(new ContentTypeS3 { fileType = "p7b", ContentType = "application/x-pkcs7-certificates" });
            output.Add(new ContentTypeS3 { fileType = "p7c", ContentType = "application/pkcs7-mime" });
            output.Add(new ContentTypeS3 { fileType = "p7m", ContentType = "application/pkcs7-mime" });
            output.Add(new ContentTypeS3 { fileType = "p7r", ContentType = "application/x-pkcs7-certreqresp" });
            output.Add(new ContentTypeS3 { fileType = "p7s", ContentType = "application/pkcs7-signature" });
            output.Add(new ContentTypeS3 { fileType = "pcast", ContentType = "application/x-podcast" });
            output.Add(new ContentTypeS3 { fileType = "pdf", ContentType = "application/pdf" });
            output.Add(new ContentTypeS3 { fileType = "pdfxml", ContentType = "application/vnd.adobe.pdfxml" });
            output.Add(new ContentTypeS3 { fileType = "pdx", ContentType = "application/vnd.adobe.pdx" });
            output.Add(new ContentTypeS3 { fileType = "pfx", ContentType = "application/x-pkcs12" });
            output.Add(new ContentTypeS3 { fileType = "pko", ContentType = "application/vnd.ms-pki.pko" });
            output.Add(new ContentTypeS3 { fileType = "pls", ContentType = "audio/scpls" });
            output.Add(new ContentTypeS3 { fileType = "png", ContentType = "image/png" });
            output.Add(new ContentTypeS3 { fileType = "pot", ContentType = "application/vnd.ms-powerpoint" });
            output.Add(new ContentTypeS3 { fileType = "potm", ContentType = "application/vnd.ms-powerpoint.template.macroEnabled.12" });
            output.Add(new ContentTypeS3 { fileType = "potx", ContentType = "application/vnd.openxmlformats-officedocument.presentationml.template" });
            output.Add(new ContentTypeS3 { fileType = "ppa", ContentType = "application/vnd.ms-powerpoint" });
            output.Add(new ContentTypeS3 { fileType = "ppam", ContentType = "application/vnd.ms-powerpoint.addin.macroEnabled.12" });
            output.Add(new ContentTypeS3 { fileType = "pps", ContentType = "application/vnd.ms-powerpoint" });
            output.Add(new ContentTypeS3 { fileType = "ppsm", ContentType = "application/vnd.ms-powerpoint.slideshow.macroEnabled.12" });
            output.Add(new ContentTypeS3 { fileType = "ppsx", ContentType = "application/vnd.openxmlformats-officedocument.presentationml.slideshow" });
            output.Add(new ContentTypeS3 { fileType = "ppt", ContentType = "application/vnd.ms-powerpoint" });
            output.Add(new ContentTypeS3 { fileType = "pptm", ContentType = "application/vnd.ms-powerpoint.presentation.macroEnabled.12" });
            output.Add(new ContentTypeS3 { fileType = "pptx", ContentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation" });
            output.Add(new ContentTypeS3 { fileType = "prf", ContentType = "application/pics-rules" });
            output.Add(new ContentTypeS3 { fileType = "ps", ContentType = "application/postscript" });
            output.Add(new ContentTypeS3 { fileType = "psc1", ContentType = "application/PowerShell" });
            output.Add(new ContentTypeS3 { fileType = "pwz", ContentType = "application/vnd.ms-powerpoint" });
            output.Add(new ContentTypeS3 { fileType = "py", ContentType = "text/plain" });
            output.Add(new ContentTypeS3 { fileType = "pyw", ContentType = "text/plain" });
            output.Add(new ContentTypeS3 { fileType = "rat", ContentType = "application/rat-file" });
            output.Add(new ContentTypeS3 { fileType = "rc", ContentType = "text/plain" });
            output.Add(new ContentTypeS3 { fileType = "rc2", ContentType = "text/plain" });
            output.Add(new ContentTypeS3 { fileType = "rct", ContentType = "text/plain" });
            output.Add(new ContentTypeS3 { fileType = "rdlc", ContentType = "application/xml" });
            output.Add(new ContentTypeS3 { fileType = "resx", ContentType = "application/xml" });
            output.Add(new ContentTypeS3 { fileType = "rmi", ContentType = "audio/mid" });
            output.Add(new ContentTypeS3 { fileType = "rmp", ContentType = "application/vnd.rn-rn_music_package" });
            output.Add(new ContentTypeS3 { fileType = "rqy", ContentType = "text/x-ms-rqy" });
            output.Add(new ContentTypeS3 { fileType = "rtf", ContentType = "application/msword" });
            output.Add(new ContentTypeS3 { fileType = "sct", ContentType = "text/scriptlet" });
            output.Add(new ContentTypeS3 { fileType = "settings", ContentType = "application/xml" });
            output.Add(new ContentTypeS3 { fileType = "shtml", ContentType = "text/html" });
            output.Add(new ContentTypeS3 { fileType = "sit", ContentType = "application/x-stuffit" });
            output.Add(new ContentTypeS3 { fileType = "sitemap", ContentType = "application/xml" });
            output.Add(new ContentTypeS3 { fileType = "skin", ContentType = "application/xml" });
            output.Add(new ContentTypeS3 { fileType = "sldm", ContentType = "application/vnd.ms-powerpoint.slide.macroEnabled.12" });
            output.Add(new ContentTypeS3 { fileType = "sldx", ContentType = "application/vnd.openxmlformats-officedocument.presentationml.slide" });
            output.Add(new ContentTypeS3 { fileType = "slk", ContentType = "application/vnd.ms-excel" });
            output.Add(new ContentTypeS3 { fileType = "sln", ContentType = "text/plain" });
            output.Add(new ContentTypeS3 { fileType = "slupkg-ms", ContentType = "application/x-ms-license" });
            output.Add(new ContentTypeS3 { fileType = "snd", ContentType = "audio/basic" });
            output.Add(new ContentTypeS3 { fileType = "snippet", ContentType = "application/xml" });
            output.Add(new ContentTypeS3 { fileType = "spc", ContentType = "application/x-pkcs7-certificates" });
            output.Add(new ContentTypeS3 { fileType = "sst", ContentType = "application/vnd.ms-pki.certstore" });
            output.Add(new ContentTypeS3 { fileType = "stc", ContentType = "application/vnd.sun.xml.calc.template" });
            output.Add(new ContentTypeS3 { fileType = "std", ContentType = "application/vnd.sun.xml.draw.template" });
            output.Add(new ContentTypeS3 { fileType = "stl", ContentType = "application/vnd.ms-pki.stl" });
            output.Add(new ContentTypeS3 { fileType = "stw", ContentType = "application/vnd.sun.xml.writer.template" });
            output.Add(new ContentTypeS3 { fileType = "svg", ContentType = "image/svg+xml" });
            output.Add(new ContentTypeS3 { fileType = "sxc", ContentType = "application/vnd.sun.xml.calc" });
            output.Add(new ContentTypeS3 { fileType = "sxd", ContentType = "application/vnd.sun.xml.draw" });
            output.Add(new ContentTypeS3 { fileType = "sxg", ContentType = "application/vnd.sun.xml.writer.global" });
            output.Add(new ContentTypeS3 { fileType = "sxw", ContentType = "application/vnd.sun.xml.writer" });
            output.Add(new ContentTypeS3 { fileType = "tga", ContentType = "image/targa" });
            output.Add(new ContentTypeS3 { fileType = "thmx", ContentType = "application/vnd.ms-officetheme" });
            output.Add(new ContentTypeS3 { fileType = "tif", ContentType = "image/tiff" });
            output.Add(new ContentTypeS3 { fileType = "tiff", ContentType = "image/tiff" });
            output.Add(new ContentTypeS3 { fileType = "torrent", ContentType = "application/x-bittorrent" });
            output.Add(new ContentTypeS3 { fileType = "ts", ContentType = "video/vnd.dlna.mpeg-tts" });
            output.Add(new ContentTypeS3 { fileType = "tts", ContentType = "video/vnd.dlna.mpeg-tts" });
            output.Add(new ContentTypeS3 { fileType = "txt", ContentType = "text/plain" });
            output.Add(new ContentTypeS3 { fileType = "user", ContentType = "text/plain" });
            output.Add(new ContentTypeS3 { fileType = "vb", ContentType = "text/plain" });
            output.Add(new ContentTypeS3 { fileType = "vbox", ContentType = "application/x-virtualbox-vbox" });
            output.Add(new ContentTypeS3 { fileType = "vbox-extpack", ContentType = "application/x-virtualbox-vbox-extpack" });
            output.Add(new ContentTypeS3 { fileType = "vbproj", ContentType = "text/plain" });
            output.Add(new ContentTypeS3 { fileType = "vcf", ContentType = "text/x-vcard" });
            output.Add(new ContentTypeS3 { fileType = "vdi", ContentType = "application/x-virtualbox-vdi" });
            output.Add(new ContentTypeS3 { fileType = "vdp", ContentType = "text/plain" });
            output.Add(new ContentTypeS3 { fileType = "vdproj", ContentType = "text/plain" });
            output.Add(new ContentTypeS3 { fileType = "vhd", ContentType = "application/x-virtualbox-vhd" });
            output.Add(new ContentTypeS3 { fileType = "vmdk", ContentType = "application/x-virtualbox-vmdk" });
            output.Add(new ContentTypeS3 { fileType = "vor", ContentType = "application/vnd.stardivision.writer" });
            output.Add(new ContentTypeS3 { fileType = "vscontent", ContentType = "application/xml" });
            output.Add(new ContentTypeS3 { fileType = "vsi", ContentType = "application/ms-vsi" });
            output.Add(new ContentTypeS3 { fileType = "vspolicy", ContentType = "application/xml" });
            output.Add(new ContentTypeS3 { fileType = "vspolicydef", ContentType = "application/xml" });
            output.Add(new ContentTypeS3 { fileType = "vspscc", ContentType = "text/plain" });
            output.Add(new ContentTypeS3 { fileType = "vsscc", ContentType = "text/plain" });
            output.Add(new ContentTypeS3 { fileType = "vssettings", ContentType = "text/xml" });
            output.Add(new ContentTypeS3 { fileType = "vssscc", ContentType = "text/plain" });
            output.Add(new ContentTypeS3 { fileType = "vstemplate", ContentType = "text/xml" });
            output.Add(new ContentTypeS3 { fileType = "vsto", ContentType = "application/x-ms-vsto" });
            output.Add(new ContentTypeS3 { fileType = "wal", ContentType = "interface/x-winamp3-skin" });
            output.Add(new ContentTypeS3 { fileType = "wav", ContentType = "audio/wav" });
            output.Add(new ContentTypeS3 { fileType = "wave", ContentType = "audio/wav" });
            output.Add(new ContentTypeS3 { fileType = "wax", ContentType = "audio/x-ms-wax" });
            output.Add(new ContentTypeS3 { fileType = "wbk", ContentType = "application/msword" });
            output.Add(new ContentTypeS3 { fileType = "wdp", ContentType = "image/vnd.ms-photo" });
            output.Add(new ContentTypeS3 { fileType = "website", ContentType = "application/x-mswebsite" });
            output.Add(new ContentTypeS3 { fileType = "wiz", ContentType = "application/msword" });
            output.Add(new ContentTypeS3 { fileType = "wlz", ContentType = "interface/x-winamp-lang" });
            output.Add(new ContentTypeS3 { fileType = "wm", ContentType = "video/x-ms-wm" });
            output.Add(new ContentTypeS3 { fileType = "wma", ContentType = "audio/x-ms-wma" });
            output.Add(new ContentTypeS3 { fileType = "wmd", ContentType = "application/x-ms-wmd" });
            output.Add(new ContentTypeS3 { fileType = "wmv", ContentType = "video/x-ms-wmv" });
            output.Add(new ContentTypeS3 { fileType = "wmx", ContentType = "video/x-ms-wmx" });
            output.Add(new ContentTypeS3 { fileType = "wmz", ContentType = "application/x-ms-wmz" });
            output.Add(new ContentTypeS3 { fileType = "wpl", ContentType = "application/vnd.ms-wpl" });
            output.Add(new ContentTypeS3 { fileType = "wsc", ContentType = "text/scriptlet" });
            output.Add(new ContentTypeS3 { fileType = "wsdl", ContentType = "application/xml" });
            output.Add(new ContentTypeS3 { fileType = "wsz", ContentType = "interface/x-winamp-skin" });
            output.Add(new ContentTypeS3 { fileType = "wvx", ContentType = "video/x-ms-wvx" });
            output.Add(new ContentTypeS3 { fileType = "xaml", ContentType = "application/xaml+xml" });
            output.Add(new ContentTypeS3 { fileType = "xbap", ContentType = "application/x-ms-xbap" });
            output.Add(new ContentTypeS3 { fileType = "xdp", ContentType = "application/vnd.adobe.xdp+xml" });
            output.Add(new ContentTypeS3 { fileType = "xdr", ContentType = "application/xml" });
            output.Add(new ContentTypeS3 { fileType = "xfdf", ContentType = "application/vnd.adobe.xfdf" });
            output.Add(new ContentTypeS3 { fileType = "xht", ContentType = "application/xhtml+xml" });
            output.Add(new ContentTypeS3 { fileType = "xhtml", ContentType = "application/xhtml+xml" });
            output.Add(new ContentTypeS3 { fileType = "xla", ContentType = "application/vnd.ms-excel" });
            output.Add(new ContentTypeS3 { fileType = "xlam", ContentType = "application/vnd.ms-excel.addin.macroEnabled.12" });
            output.Add(new ContentTypeS3 { fileType = "xld", ContentType = "application/vnd.ms-excel" });
            output.Add(new ContentTypeS3 { fileType = "xlk", ContentType = "application/vnd.ms-excel" });
            output.Add(new ContentTypeS3 { fileType = "xll", ContentType = "application/vnd.ms-excel" });
            output.Add(new ContentTypeS3 { fileType = "xlm", ContentType = "application/vnd.ms-excel" });
            output.Add(new ContentTypeS3 { fileType = "xls", ContentType = "application/vnd.ms-excel" });
            output.Add(new ContentTypeS3 { fileType = "xlsb", ContentType = "application/vnd.ms-excel.sheet.binary.macroEnabled.12" });
            output.Add(new ContentTypeS3 { fileType = "xlsm", ContentType = "application/vnd.ms-excel.sheet.macroEnabled.12" });
            output.Add(new ContentTypeS3 { fileType = "xlsx", ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
            output.Add(new ContentTypeS3 { fileType = "xlt", ContentType = "application/vnd.ms-excel" });
            output.Add(new ContentTypeS3 { fileType = "xltm", ContentType = "application/vnd.ms-excel.template.macroEnabled.12" });
            output.Add(new ContentTypeS3 { fileType = "xltx", ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.template" });
            output.Add(new ContentTypeS3 { fileType = "xlw", ContentType = "application/vnd.ms-excel" });
            output.Add(new ContentTypeS3 { fileType = "xml", ContentType = "text/xml" });
            output.Add(new ContentTypeS3 { fileType = "xrm-ms", ContentType = "text/xml" });
            output.Add(new ContentTypeS3 { fileType = "xsc", ContentType = "application/xml" });
            output.Add(new ContentTypeS3 { fileType = "xsd", ContentType = "application/xml" });
            output.Add(new ContentTypeS3 { fileType = "xsl", ContentType = "text/xml" });
            output.Add(new ContentTypeS3 { fileType = "xslt", ContentType = "application/xml" });
            output.Add(new ContentTypeS3 { fileType = "xss", ContentType = "application/xml" });

            return output;

        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
