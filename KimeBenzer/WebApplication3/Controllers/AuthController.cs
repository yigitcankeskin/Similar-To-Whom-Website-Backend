using KimeBenzerRest.Models;
using Microsoft.AspNetCore.Mvc;
using KimeBenzerRest.services;
using System.Linq;
using System.Threading.Tasks;

namespace KimeBenzerRest.Controllers
{
    public class AuthController : ControllerBase
    {
        TokenController tokenController = new TokenController();
        [HttpGet("/login")]
        public async Task<IActionResult> LoginControl(string Email,string password)
        {
            using (var context = new KimeBenzerContext())
            {
                
                UserTable user = context.UserTable.Where(auth => auth.UserEmail == Email && auth.UserPassword == password).FirstOrDefault();
                if (user != null)
                {
                    var token = tokenController.generateJwtToken(user);
                    return Ok(token);

                }
                else
                {
                    return Unauthorized("Kullanıcı veya Şifre doğru değil");
                }


            }

        }
        [HttpPost("/register")]
        public async Task<IActionResult> Register(UserTable User)
        {
            using (var context = new KimeBenzerContext())
            {
                if (context.UserTable.Where(auth => auth.UserEmail == User.UserEmail).FirstOrDefault() != null)
                {
                    return Conflict("Kullanıcı zaten Bulunmaktadır");//conflict olmalı forbid değil
                }
                else
                {
                    context.UserTable.Add(User);
                    context.SaveChanges();
                    var token = tokenController.generateJwtToken(User);
                    return Ok(token);
                }   

            }

        }
        [HttpGet("/SessionControl")]
        public async Task<IActionResult> SessionControl(string token)
        {
            TokenController tk = new TokenController();
           if(tk.Validate(token))
                return Ok(token);
            else
                return Unauthorized("Token tanınamadı");
        }

    }
}
