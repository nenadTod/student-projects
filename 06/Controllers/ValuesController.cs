using System.Collections.Generic;
using System.Web.Http;
using RentApp.Services;

namespace RentApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ValuesController : ApiController
    {
        private readonly ISmtpService _smtp;

        public ValuesController(ISmtpService smtp)
        {
            _smtp = smtp;
        }


        // GET api/values
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post(U u)
        {
            _smtp.SendMail(u.Subject,u.Content, u.MailTo);
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        public class U
        {
            public string Subject { get; set; }
            public string Content { get; set; }
            public string MailTo { get; set; }
        }
    }
}
