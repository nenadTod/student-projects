﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        public int User_Id { get; set; }

        public int Service_Id { get; set; }

        public string Text { get; set; }

       
    }
}