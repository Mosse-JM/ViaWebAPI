using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace WebApi.Models
{
    public class Message {
        [Key]
        public int Id { get; set;}
        public string Name { get; set;}
        public string Email { get; set;}
        public string Text { get; set;}
        public Boolean Subscribed { get; set;}


    }

}