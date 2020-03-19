using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Entities;
using WebApi.Helpers;
using System;

namespace shoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunicationController : Controller
    {
        private DataContext _dataContext;

        public CommunicationController(DataContext context){
            _dataContext = context;
        }

        // GET api/
        [HttpGet]
        [Route("Message")]
        public IEnumerable<Message> GetMessages()
        { 
            return _dataContext.Messages;
        }

        // GET api/values/5
        [HttpGet]
        [Route("Message/{id}")]
        public ActionResult<Message> GetMessagesById(int id)
        {
            if(id <= 0){
                return NotFound("Message id must be grater than 0");
            }
            var message = (from p in _dataContext.Messages
                           where p.Id == id
                           select p).FirstOrDefault();
            if (message == null)
            {
                return NotFound("Message not found");
            }
            return Ok(message);
        }


        // POST api/
        [HttpPost]
        [Route("Message")]
        public async Task<ActionResult> SaveMessage([FromForm]Message msg)
        {

            Message message = new Message(); 
            message.Name = msg.Name;
            message.Email = msg.Email;
            message.Text = msg.Text;
            message.Subscribed = msg.Subscribed;

            //db.Products.Add(product);
            await _dataContext.Messages.AddAsync(message);
            //db.SaveShanges();
            await _dataContext.SaveChangesAsync();

            return Ok(message);
        }

         // Delete api/
        [HttpDelete]
        [Route("Message/{id}")]
        public async Task<ActionResult> DeleteMessage(int? id)
        {
            if(id == null){
                return NotFound("Id is not supplied");
            }
            Message message = _dataContext.Messages.FirstOrDefault(s => s.Id == id);
            if(message == null){
                return NotFound("Message with this id doesn't exist");
            }
            _dataContext.Messages.Remove(message);
            await _dataContext.SaveChangesAsync();
            return Ok();
        }

        // GET api/
        [HttpGet]
        [Route("Comments")]
        public IEnumerable<Comment> GetComments()
        { 
            return _dataContext.Comments;
        }

        // GET api/values/5
        [HttpGet]
        [Route("Comments/{id}")]
        public ActionResult<Comment> GetCommentById(int id)
        {
            if(id <= 0){
                return NotFound("Comment id must be grater than 0");
            }
            var comment = (from p in _dataContext.Comments
                           where p.Id == id
                           select p).FirstOrDefault();
            if (comment == null)
            {
                return NotFound("Comment not found");
            }
            return Ok(comment);
        }

        [HttpPost]
        [Route("Comments")]
        public async Task<ActionResult> SaveComments([FromForm]Comment cmt)
        {

            Comment comment = new Comment(); 
            comment.UserName = cmt.UserName;
            comment.Link = cmt.Link;
            comment.Text = cmt.Text;
            comment.RComment = cmt.RComment;

            //db.Products.Add(product);
            await _dataContext.Comments.AddAsync(comment);
            //db.SaveShanges();
            await _dataContext.SaveChangesAsync();

            return Ok(comment);
        }

        // PUT api/
        [HttpPut]
        [Route("Comments")]
        public async Task<ActionResult> PutProduct([FromBody]Comment comment)
        {
            if(comment == null){
                return NotFound("Post data is not supplied");
            }
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            Comment existingComment = _dataContext.Comments.FirstOrDefault(s => s.Id == comment.Id);
            if(existingComment == null){
                return NotFound("Product doesn't exist in db");
            }
            existingComment.UserName = comment.UserName;
            existingComment.Link = comment.Link;
            existingComment.Text = comment.Text;
            existingComment.RComment = comment.RComment;

            _dataContext.Attach(existingComment).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _dataContext.SaveChangesAsync();
            return Ok(existingComment);
        }

         // Delete api/
        [HttpDelete]
        [Route("Comments/{id}")]
        public async Task<ActionResult> DeleteComment(int? id)
        {
            if(id == null){
                return NotFound("Id is not supplied");
            }
            Comment comment = _dataContext.Comments.FirstOrDefault(s => s.Id == id);
            if(comment == null){
                return NotFound("Comment with this id doesn't exist");
            }
            _dataContext.Comments.Remove(comment);
            await _dataContext.SaveChangesAsync();
            return Ok();
        }

       
        
    }
}