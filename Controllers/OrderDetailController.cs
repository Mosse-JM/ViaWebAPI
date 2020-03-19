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
    public class OrderDetailController : Controller
    {
        private DataContext _dataContext;

        public OrderDetailController(DataContext context){
            _dataContext = context;
        }

        // GET api/
        [HttpGet]
        public IEnumerable<OrderDetail> GetOrderDetails()
        {
             return (from od in _dataContext.OrderDetail
                //join oi in _dataContext.OrderItems on od.OrderDetailId equals oi.OrderDetailId
                //where oi.OrderDetailId == od.OrderDetailId
                select new OrderDetail{
                    OrderDetailId = od.OrderDetailId,
                    CustomerId = od.CustomerId,
                    CustomerName= od.CustomerName,
                    DeliveryAddress= od.DeliveryAddress,
                    Phone= od.Phone,
                    Email= od.Email,
                    Message= od.Message,
                    OrderDate= od.OrderDate,
                    IP= od.IP,
                    OrderPayMethod= od.OrderPayMethod,
                    IsConfirmed= od.IsConfirmed,
                    Status= od.Status,
                    OrderItems = (from i in _dataContext.OrderItems 
                                where i.OrderDetailId == od.OrderDetailId
                                select new OrderItem{
                                    OrderItemId = i.OrderItemId,
                                    ProductID = i.ProductID,
                                    SellerID = i.SellerID,
                                    ProductName = i.ProductName,
                                    PerUnitPrice = i.PerUnitPrice,
                                    OrderedQuantity = i.OrderedQuantity,
                                }).ToList()
                    }).AsEnumerable();
                
            //return _dataContext.OrderDetail;
        }

        // GET api/
        [HttpGet]
        [Route("GetOrderDetail/{id}")]
        public ActionResult<OrderDetail> GetOrderDetail(int id)
        {
            if(id <= 0){
                return NotFound("OrderiD must be grater than 0");
            }
            OrderDetail orderDetail = _dataContext.OrderDetail.FirstOrDefault(s => s.OrderDetailId == id);
            if (orderDetail == null){
                return NotFound("User not found");
            }
            return Ok(orderDetail);
        }

        // POST api/
        [HttpPost]
        public async Task<ActionResult> PostOrderDetail([FromBody]OrderDetail orderDetail)
        {
            if(orderDetail== null){
                return NotFound("OrderDetaildata is not supplied");
            }
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            orderDetail.IP = "";
            orderDetail.IsConfirmed = false;
            orderDetail.OrderDate = DateTime.Now;
            orderDetail.Status = "Placed";
            await _dataContext.OrderDetail.AddAsync(orderDetail);
            await _dataContext.SaveChangesAsync();
            return Ok(orderDetail.OrderDetailId);
        }


        // PUT api
        [HttpPut]
        public async Task<ActionResult> PutOrderDetail([FromBody]OrderDetail orderDetail)
        {
            if(orderDetail == null){
                return NotFound("orderDetail data is not supplied");
            }
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            OrderDetail existingOrderDetail = _dataContext.OrderDetail.FirstOrDefault(s => s.OrderDetailId == orderDetail.OrderDetailId);
            if(existingOrderDetail == null){
                return NotFound("OrderDetail doesn't exist in db");
            }
            existingOrderDetail.CustomerId = orderDetail.CustomerId;
            existingOrderDetail.CustomerName= orderDetail.CustomerName;
            existingOrderDetail.DeliveryAddress= orderDetail.DeliveryAddress;
            existingOrderDetail.Phone= orderDetail.Phone;
            existingOrderDetail.OrderDate= orderDetail.OrderDate;
            existingOrderDetail.IP= orderDetail.IP;
            existingOrderDetail.OrderPayMethod= orderDetail.OrderPayMethod;
            existingOrderDetail.IsConfirmed= orderDetail.IsConfirmed;
            existingOrderDetail.Status= orderDetail.Status;
            //existingOrderDetail.OrderItems= orderDetail.OrderItems;
            

            _dataContext.Attach(existingOrderDetail).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _dataContext.SaveChangesAsync();
            return Ok(existingOrderDetail);
        }

        // Delete api/
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrderDetail(int? id)
        {
            if(id == null){
                return NotFound("Id is not supplied");
            }
            OrderDetail orderDetail = _dataContext.OrderDetail.FirstOrDefault(s => s.OrderDetailId == id);
            if(orderDetail == null){
                return NotFound("OrderDetail with this id doesn't exist");
            }
            _dataContext.OrderDetail.Remove(orderDetail);
            await _dataContext.SaveChangesAsync();
            return Ok("OrderDetail was deleted");
        }

        ~OrderDetailController(){
            _dataContext.Dispose();
        }
        
    }
}