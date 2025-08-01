﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020052.DomainModels
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = "";
        public string ProductDescription { get; set; } = "";
        public int SupplierID { get; set;}
        public int CategoryID {  get; set; }
        public string Unit { get; set; } = "";
        public decimal Price { get; set; }
        public string Photo { get; set; } = "";
        public bool IsSelling { get; set; }
        public string Description { get; set; }
    }
}
