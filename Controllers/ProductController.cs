using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Entities;
using WebApi.Helpers;
using System;
using System.Drawing;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web;



//using System.Web.Http;


namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private DataContext _dataContext;

        public ProductController(DataContext context){
            _dataContext = context;
        }

        IQueryable<ProductDTO> MapProducts()
        {
            return from p in _dataContext.Products
                join I in _dataContext.Images on p.Id equals I.ProductId 
                select new ProductDTO() 
                { 
                    Id = p.Id,
                    Name = p.Name, 
                    Quantity = p.Quantity,
                    SellerId = p.SellerId, 
                    SellerName = p.SellerName, 
                    TC = p.TC, 
                    UnitPrice = p.UnitPrice, 
                    BillingAddress = p.BillingAddress, 
                    Category = p.Category, 
                    Description = p.Description, 
                    Images = //System.Convert.ToBase64String()
                        (from q in _dataContext.Images
                                //group q by q.ProductId into productImages
                                //from pI in productImages
                                where q.ProductId == p.Id
                                select q.ImageFilePath.ToArray()).FirstOrDefault()  
                    
                };
        }

        // GET api/
        [HttpGet]
        public IEnumerable<ProductDTO> GetProducts()
        {
            return MapProducts().AsEnumerable();
        }

        // GET api/values/5
        [HttpGet]
        [Route("GetProduct/{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            if(id <= 0){
                return NotFound("Product id must be grater than 0");
            }
            var product = (from p in MapProducts() 
                           where p.Id == id
                           select p).FirstOrDefault();
            if (product == null)
            {
                return NotFound("Product not found");
            }
            return Ok(product);
        }

        

        // POST api/
        [HttpPost]
        public async Task<ActionResult> PostProduct([FromForm]Product product,[FromForm(Name = "testImage.jpg")] IFormFile files)// [FromForm]List<IFormFile>
        {
            //a method to save on db
            byte[] p0 = new byte[0];
            byte[] p1 = null;
            //var files = Request.Form.Files;
            Product newProduct = new Product();
            var imageList= new List<Image>();
            //foreach (var formFile in files)
            {
                //you can first copy it to a stream
                using (var memoryStream = new MemoryStream())
                {
                    await files.CopyToAsync(memoryStream);

                    p1 = memoryStream.ToArray();
                    
                    // Upload the file if less than 2 MB
                    if ( p1.Length< 2097152)
                    {
                        int i = p0.Length;
                        int k = p1.Length;
                        Array.Resize(ref p0, p0.Length + p1.Length);

                        for (int j = 0; j < k; j++)
                        {
                            p0[i] = p1[j];
                            i++;
                        } 
                    }
                    else
                    {
                        ModelState.AddModelError("File", "The file is too large.");
                    }
                    var image = new Image{
                        ProductId = newProduct.Id
                    };
                    image.ImageFilePath = p0;
                    imageList.Add(image);
                }
                
            } 
  
            newProduct.Images = imageList;
            newProduct.Name = product.Name;
            newProduct.Description = product.Description;
            newProduct.BillingAddress = product.BillingAddress;
            newProduct.UnitPrice = product.UnitPrice;
            newProduct.Category = product.Category;
            newProduct.Quantity = product.Quantity;
            newProduct.TC = product.TC;
            newProduct.SellerId = product.SellerId;
            newProduct.SellerName = product.SellerName;

            //db.Products.Add(product);
            await _dataContext.Products.AddAsync(newProduct);
            //db.SaveShanges();
            await _dataContext.SaveChangesAsync();

            return Ok(newProduct);
            
            /* 
            //to write on disk
            long size = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    //Create custom filename
                    var ImageFilePath = new String(Path.GetFileNameWithoutExtension(formFile.FileName).Take(10).ToArray()).Replace(" ", "-");
                    ImageFilePath = ImageFilePath + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(formFile.FileName);
                    //var uniqueFileName = $"{product.SellerId}_profilepic.png";
                    var filePath = Path.Combine("~/Images/" + ImageFilePath);
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    //using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(fileStream);
                    }
                }
            }
            */
 
        }


        // PUT api/
        [HttpPut]
        public async Task<ActionResult> PutProduct([FromBody]Product product)
        {
            if(product == null){
                return NotFound("Product data is not supplied");
            }
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            Product existingProduct = _dataContext.Products.FirstOrDefault(s => s.Id == product.Id);
            if(existingProduct == null){
                return NotFound("Product doesn't exist in db");
            }
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.BillingAddress = product.BillingAddress;
            existingProduct.UnitPrice = product.UnitPrice;
            existingProduct.Category = product.Category;
            existingProduct.Quantity = product.Quantity;
            
            existingProduct.TC = product.TC;
            existingProduct.SellerId = product.SellerId;
            existingProduct.SellerName = product.SellerName;

            _dataContext.Attach(existingProduct).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _dataContext.SaveChangesAsync();
            return Ok(existingProduct);
        }

        // Delete api/
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int? id)
        {
            if(id == null){
                return NotFound("Id is not supplied");
            }
            Product product = _dataContext.Products.FirstOrDefault(s => s.Id == id);
            if(product == null){
                return NotFound("Product with this id doesn't exist");
            }
            _dataContext.Products.Remove(product);
            await _dataContext.SaveChangesAsync();
            return Ok("product was deleted");
        }

        ~ProductController(){
            _dataContext.Dispose();
        }
        
    }
}