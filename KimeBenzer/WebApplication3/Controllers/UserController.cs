using KimeBenzerRest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KimeBenzerRest.Response;
using KimeBenzerRest.services;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        // /User
        [HttpGet]
        public IEnumerable<UserTable> Get()
        {
            using (var context = new KimeBenzerContext())
            {
                // to get all user
                return context.UserTable.ToList();
            }
        }
        // /User/{id}
        [HttpGet("{id:int}")]
        public IEnumerable<UserTable> GetById(int id)
        {
            using (var context = new KimeBenzerContext())
            {
                // to get  user by ıd
                return context.UserTable.Where(auth => auth.UserId == id).ToList();
            }
        }
        // /User/AddUser/{name}/{surname}/{email}/{password}
        [HttpPost("AddUser/{name}/{surname}/{email}/{password}")]
        public object AddUser(string name, string surname,string email,string password)
        {
            using (var context = new KimeBenzerContext())
            {
                // to add user 
                UserTable user = new UserTable();

                user.UserName = name;
                user.UserSurname = surname;
                user.UserEmail = email;
                user.UserPassword = password;
                
                context.UserTable.Add(user);
                context.SaveChanges();
                return new ResponseUser
                {
                    Status = "Success",
                    Message = "Record SuccessFully Saved."
                };
            }
        }
        [HttpDelete("{id:int}")]
        public object RemoveUser(int id)
        {
            using (var context = new KimeBenzerContext())
            {
                // to remove user 
                UserTable user = context.UserTable.Where(auth => auth.UserId == id).FirstOrDefault();
                context.UserTable.Remove(user);
                context.SaveChanges();

                return new ResponseUser
                {
                    Status = "Success",
                    Message = "Record SuccessFully Removed."
                };
            }
        }
        [HttpGet("{token}")]
        public  UserTable GetByToken(string token)
        {
            TokenController tokenController = new TokenController();
            if (tokenController.Validate(token))
            {
               var id = tokenController.getJWTTokenClaim(token, "nameid");
                using (var context = new KimeBenzerContext())
                {
                    // to remove user 
                    UserTable user = context.UserTable.Where(auth => auth.UserId == Int32.Parse(id)).FirstOrDefault();
                    UserTable user2 = new UserTable();
                    user2.UserName = user.UserName;
                    user2.UserSurname = user.UserSurname;
                    user2.UserEmail = user.UserEmail;
                    user2.UserId = user.UserId;
                    return user2;
                }
               

            }
            return null;
        }
    }
}
