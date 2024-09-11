using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
//using System.Web.Mvc;

namespace RentApp.Controllers
{
    [System.Web.Http.RoutePrefix("api/Upload")]
    public class FileUploadController : ApiController
    {

        [Route("PostImage")]
        [AllowAnonymous]
        public async Task<string>  PostImage()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                var httpRequest = HttpContext.Current.Request;
                var filePath = "";

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {

                            var message = string.Format("Please Upload image of type .jpg,.gif,.png.");

                            dict.Add("error", message);
                            return "Bad request " + message;
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("Please Upload a file upto 1 mb.");

                            dict.Add("error", message);
                            return "Bad request " + message;
                        }
                        else
                        {

                            filePath = HttpContext.Current.Server.MapPath("~/Content/Images/" + postedFile.FileName);

                            postedFile.SaveAs(filePath);
                            filePath = @"http://localhost:51680/Content/Images/" + postedFile.FileName ;
                        }
                    }

                    return filePath;
                }

                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return "Not found " + res;
            }
            catch (Exception ex)
            {
                var res = string.Format("some Message");
                dict.Add("error", res);
                return "Not found " + res;
            }
        }
    }
}