using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace WebApi.Models
{
    public class OrderDetail{
        [Key]
        public int OrderDetailId { get; set;}
        public string CustomerId { get; set;}
        public string CustomerName{ get; set;}
        public string DeliveryAddress { get; set;}
        public string Phone { get; set;}
        public string Email { get; set;}
        public string Message{ get; set;}
        public DateTime OrderDate { get; set;}
        public string IP { get; set;}
        public string OrderPayMethod { get; set;}
        private string PaymentRefrenceId { get; set;}
        public bool IsConfirmed { get; set;}
        public string Status { get; set;}
        public ICollection<OrderItem> OrderItems { get; set;}
    }
        public class OrderItem {
        [Key]
        public int OrderItemId { get; set;}
        public int ProductID { get; set;}
        public int SellerID { get; set;}
        public string ProductName { get; set;}
        public int PerUnitPrice { get; set;}
        public int OrderedQuantity { get; set;}
        public int OrderDetailId { get; set;}
        //public OrderDetail OrderDetail { get; set;}
    }
    
}