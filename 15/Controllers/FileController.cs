using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RentApp.Models.Entities;
using RentApp.Persistance;
using RentApp.Persistance.UnitOfWork;
using System.Drawing;
using System.Web;
using System.IO;
using System.Threading.Tasks;

namespace RentApp.Controllers
{
    public class FileController : ApiController
    {
        //api/File
        [AllowAnonymous]
        public async Task<HttpResponseMessage> PostImage()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png", ".img", ".jpeg" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {

                            var message = string.Format("Please Upload image of type .jpg,.gif,.png,.img,.jpeg.");

                            return Request.CreateResponse(HttpStatusCode.BadRequest, message);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {
                            var message = string.Format("Please Upload a file upto 1 mb.");

                            return Request.CreateResponse(HttpStatusCode.BadRequest, message);
                        }
                        else
                        {
                            var filePath = HttpContext.Current.Server.MapPath("/Content/Picture/" + postedFile.FileName);
                            postedFile.SaveAs(filePath);

                            var message = string.Format("/Content/Picture/" + postedFile.FileName);
                            return Request.CreateResponse(HttpStatusCode.OK, message);
                        }
                    }

                    var message1 = string.Format("Image Updated Successfully.");
                    return Request.CreateErrorResponse(HttpStatusCode.Created, message1);
                }

                var res = string.Format("Please Upload a image.");
                return Request.CreateResponse(HttpStatusCode.NotFound, res);
            }
            catch (Exception ex)
            {
                var res = string.Format("some Message");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }
    }
}