using System.Collections.Generic;
//using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace WebApi.Models
{
    public class Product {
        [Key]
        public int Id { get; set;}
        public string Name { get; set;}
        public string Description { get; set;}
        public string BillingAddress { get; set;}
        public int UnitPrice { get; set;}
        public string Category { get; set;}
        public int Quantity { get; set;}
        public ICollection<Image> Images { get; set;}//IFormFile 
        public string TC { get; set;}
        public int SellerId { get; set;}
        public string SellerName { get; set;}

        //public virtual ICollection<ProductCategory> ProductCategories { get; set; }
    }

    public partial class Image{
        [Key]
        public int ID { get; set; }
        public Byte[] ImageFilePath { get; set;}
        
        //[ForeignKey("Product")]
        public int ProductId  { get; set; }
        //public Product Product  { get; set; }
    }


/* 
    public enum Category
    {
        A, B, C, D, F
    }
    public class ProductCategory {
        public Category? Category { get; set;}
        public virtual <Product> Product { get; set;}
    }
    */
}