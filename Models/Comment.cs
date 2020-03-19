using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace WebApi.Models
{
    public class Comment {
        [Key]
        public int Id { get; set;}
        public string UserName { get; set;}
        public string Link { get; set;}
        public string Text { get; set;}
        public string RComment { get; set;}
    }

}