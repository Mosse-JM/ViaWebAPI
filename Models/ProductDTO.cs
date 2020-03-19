using System.Collections.Generic;
//using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace WebApi.Models
{
    public class ProductDTO {
        public int Id { get; set;}
        public string Name { get; set;}
        public string Description { get; set;}
        public string BillingAddress { get; set;}
        public int UnitPrice { get; set;}
        public string Category { get; set;}
        public int Quantity { get; set;}
        public Byte[] Images { get; set;}//IFormFile 
        public string TC { get; set;}
        public int SellerId { get; set;}
        public string SellerName { get; set;}

    }


}