using RentApp.Models.Entities;
using RentApp.Persistance.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace RentApp.Controllers
{
    [RoutePrefix("api/File")]
    public class FileController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public FileController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [AllowAnonymous]
        [Route("PostImage")]
        public async Task<HttpResponseMessage> PostImage()
        {
            string jwt = Request.Headers.Authorization.Parameter.ToString();
            var decodedToken = unitOfWork.AppUserRepository.DecodeJwt(jwt);

            string userEmail = decodedToken.Claims.First(claim => claim.Type == "unique_name").Value;
            var user = unitOfWork.AppUserRepository.GetAll().ToList().Where(u => u.Email == userEmail).ToList().FirstOrDefault();

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
                            if (!Directory.Exists(HttpContext.Current.Server.MapPath("/Content/Images/Users/" + user.Id)))
                                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/Content/Images/Users/" + user.Id));

                            var filePath = HttpContext.Current.Server.MapPath("/Content/Images/Users/" + user.Id + "/profilePic." + postedFile.FileName.Split('.').LastOrDefault());
                            //postedFile as .SaveAs(filePath);

                            Bitmap bmp = new Bitmap(postedFile.InputStream);
                            Image img = (Image)bmp;
                            byte[] imagebytes = ImageToByteArray(img);
                            byte[] cryptedBytes = EncryptBytes(imagebytes, "password", "asdasd");
                            File.WriteAllBytes(filePath, cryptedBytes);

                            user.Image = "/Content/Images/Users/" + user.Id + "/profilePic." + postedFile.FileName.Split('.').LastOrDefault();
                            var message = string.Format("/Content/Images/" + postedFile.FileName);
                        }
                    }

                    var message1 = string.Format("Image Updated Successfully.");
                    //return Request.CreateErrorResponse(HttpStatusCode.Created, message1);
                }

                var res = string.Format("Please Upload a image.");
                //return Request.CreateResponse(HttpStatusCode.NotFound, res);
            }
            catch (Exception)
            {
                var res = string.Format("some Message");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }

            unitOfWork.AppUserRepository.Update(user);
            unitOfWork.Complete();
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [AllowAnonymous]
        [Route("PostServiceImage")]
        public async Task<HttpResponseMessage> PostServiceImage(string serviceEmail)
        {
            var service = unitOfWork.Services.GetAll().ToList().Where(s => s.Email == serviceEmail).ToList().FirstOrDefault();
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
                            if (!Directory.Exists(HttpContext.Current.Server.MapPath("/Content/Images/Services/" + service.Id)))
                                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/Content/Images/Services/" + service.Id));

                            var filePath = HttpContext.Current.Server.MapPath("/Content/Images/Services/" + service.Id + "/serviceLogo." + postedFile.FileName.Split('.').LastOrDefault());
                            postedFile.SaveAs(filePath);

                            service.Logo = "/Content/Images/Services/" + service.Id + "/serviceLogo." + postedFile.FileName.Split('.').LastOrDefault();
                            var message = string.Format("/Content/Images/" + postedFile.FileName);
                        }
                    }

                    var message1 = string.Format("Image Updated Successfully.");
                    //return Request.CreateErrorResponse(HttpStatusCode.Created, message1);
                }

                var res = string.Format("Please Upload a image.");
                //return Request.CreateResponse(HttpStatusCode.NotFound, res);
            }
            catch (Exception)
            {
                var res = string.Format("some Message");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }

            unitOfWork.Services.Update(service);
            unitOfWork.Complete();
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [AllowAnonymous]
        [Route("PostBranchImage")]
        public async Task<HttpResponseMessage> PostBranchImage(string bId)
        {
            var branch = unitOfWork.BranchOffices.GetAll().ToList().Where(b => b.Id == Int32.Parse(bId)).ToList().FirstOrDefault();

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
                            if (!Directory.Exists(HttpContext.Current.Server.MapPath("/Content/Images/BranchOffices/" + branch.Id)))
                                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/Content/Images/BranchOffices/" + branch.Id));

                            var filePath = HttpContext.Current.Server.MapPath("/Content/Images/BranchOffices/" + branch.Id + "/branchPic." + postedFile.FileName.Split('.').LastOrDefault());
                            postedFile.SaveAs(filePath);

                            branch.Logo = "/Content/Images/BranchOffices/" + branch.Id + "/branchPic." + postedFile.FileName.Split('.').LastOrDefault();
                            var message = string.Format("/Content/Images/" + postedFile.FileName);
                        }
                    }

                    var message1 = string.Format("Image Updated Successfully.");
                    //return Request.CreateErrorResponse(HttpStatusCode.Created, message1);
                }

                var res = string.Format("Please Upload a image.");
                //return Request.CreateResponse(HttpStatusCode.NotFound, res);
            }
            catch (Exception)
            {
                var res = string.Format("some Message");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }

            unitOfWork.BranchOffices.Update(branch);
            unitOfWork.Complete();
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [AllowAnonymous]
        [Route("PostVehicleImages")]
        public async Task<HttpResponseMessage> PostVehicleImages(string vehicleId)
        {
            int i = 0;
            var vehicle = unitOfWork.Vehicles.GetAll().ToList().Where(v => v.Id == Int32.Parse(vehicleId)).ToList().FirstOrDefault();

            Dictionary<string, object> dict = new Dictionary<string, object>();
            String imgs = "";
            try
            {
                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    i++;
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
                            if (!Directory.Exists(HttpContext.Current.Server.MapPath("/Content/Images/Vehicles/" + vehicle.Id)))
                                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/Content/Images/Vehicles/" + vehicle.Id));

                            var filePath = HttpContext.Current.Server.MapPath("/Content/Images/Vehicles/" + vehicle.Id + "/vehiclePic" + i + "." + postedFile.FileName.Split('.').LastOrDefault());
                            postedFile.SaveAs(filePath);

                            imgs += (String.Format("/Content/Images/Vehicles/" + vehicle.Id + "/vehiclePic" + i + "." + postedFile.FileName.Split('.').LastOrDefault())) + ";";
                            var message = string.Format("/Content/Images/" + postedFile.FileName);
                        }
                    }

                    var message1 = string.Format("Image Updated Successfully.");
                    //return Request.CreateErrorResponse(HttpStatusCode.Created, message1);
                }

                var res = string.Format("Please Upload a image.");
                //return Request.CreateResponse(HttpStatusCode.NotFound, res);
            }
            catch (Exception e)
            {
                var res = string.Format("some Message");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }

            vehicle.Images = imgs;
            unitOfWork.Vehicles.Update(vehicle);
            unitOfWork.Complete();
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public byte[] ImageToByteArray(Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }

        public static byte[] EncryptBytes(byte[] inputBytes, string passPhrase, string saltValue)
        {
            RijndaelManaged RijndaelCipher = new RijndaelManaged();

            RijndaelCipher.Mode = CipherMode.CBC;
            byte[] salt = Encoding.ASCII.GetBytes(saltValue);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, salt, "SHA1", 2);

            ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(password.GetBytes(32), password.GetBytes(16));

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(inputBytes, 0, inputBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] CipherBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();

            return CipherBytes;
        }

        public static byte[] DecryptBytes(byte[] encryptedBytes, string passPhrase, string saltValue)
        {
            RijndaelManaged RijndaelCipher = new RijndaelManaged();

            RijndaelCipher.Mode = CipherMode.CBC;
            byte[] salt = Encoding.ASCII.GetBytes(saltValue);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, salt, "SHA1", 2);

            ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(password.GetBytes(32), password.GetBytes(16));

            MemoryStream memoryStream = new MemoryStream(encryptedBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);
            byte[] plainBytes = new byte[encryptedBytes.Length];

            int DecryptedCount = cryptoStream.Read(plainBytes, 0, plainBytes.Length);

            memoryStream.Close();
            cryptoStream.Close();

            return plainBytes;
        }

        [AllowAnonymous]
        [Route("GetUserImage")]
        public async Task<byte[]> GetUserImage(string email)
        {
            int userId = unitOfWork.AppUserRepository.Find(u => u.Email == email).FirstOrDefault().Id;
            var filePath = HttpContext.Current.Server.MapPath("/Content/Images/Users/" + userId + "/profilePic.jpg" /*+ postedFile.FileName.Split('.').LastOrDefault()*/);

            if (File.Exists(filePath))
            {
                byte[] bytes = File.ReadAllBytes(filePath);
                byte[] decryptedBytes = DecryptBytes(bytes, "password", "asdasd");
                return decryptedBytes;
            }

            return null;
        }

        [AllowAnonymous]
        [Route("PostUserImages")]
        public async Task<List<byte[]>> PostUserImages(List<AppUser> list)
        {
            List<byte[]> returnList = new List<byte[]>();
            foreach(var uid in list)
            {
                var filePath = HttpContext.Current.Server.MapPath("/Content/Images/Users/" + uid.Id.ToString() + "/profilePic.jpg" /*+ postedFile.FileName.Split('.').LastOrDefault()*/);
                
                if (File.Exists(filePath))
                {
                    byte[] bytes = File.ReadAllBytes(filePath);
                    byte[] decryptedBytes = DecryptBytes(bytes, "password", "asdasd");
                    returnList.Add(decryptedBytes);
                }
            }

            return returnList;
        }
    }
}
