﻿using System;

namespace VAR.Focus.Web.Code.Entities
{
    public class Card
    {
        public int IDCard { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        public bool Active { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}