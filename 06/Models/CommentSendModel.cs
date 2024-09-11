using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models
{
    public class CommentSendModel
    {
        public string Username { get; set; }
        public string Text { get; set; }

        public CommentSendModel() { }
    }
}