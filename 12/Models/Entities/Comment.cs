using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        public string UserEmail { get; set; }

        public string ServiceName { get; set; }

        public string Text { get; set; }
    }
}