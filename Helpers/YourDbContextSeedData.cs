using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using WebApi.Entities;
using System.Linq;
using WebApi.Services;

namespace WebApi.Helpers
{
    public class YourDbContextSeedData
    {
        private DataContext _Context;

        public YourDbContextSeedData(DataContext context)
        {
            _Context = context;
        }

        public async void SeedAdminUser()
        {
            var user = new User
            {
                Id = 1,
                FirstName = "Admin",
                LastName = "Admin",
                Username = "Admin1",
                Role = "Admin",
            };

            string password = "AdminPass";


            if (!_Context.Users.Any(x => x.Id == user.Id))
            {
                using (var hmac = new System.Security.Cryptography.HMACSHA512())
                {
                    user.PasswordSalt = hmac.Key;
                    user.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                }
                
                _Context.Users.Add(user);
                _Context.SaveChanges(); 
            }

            await _Context.SaveChangesAsync();
        }
/* 
        public async void UpdateSeedAdminUser()
        {
            _Context.Users.Update(user);
            _Context.SaveChanges();
        }*/
    }
}