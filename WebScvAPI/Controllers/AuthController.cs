using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebScvAPI.Containers;
using WebScvAPI.Features.AddFileFeature;
using WebScvAPI.Features.ChangeFeature;
using WebScvAPI.Features.DeleteFeature;
using WebScvAPI.Features.GetAllFeature;
using WebScvAPI.Features.GetByIdFeature;
using WebScvAPI.Models;
using WebScvAPI.Settings;

namespace WebScvAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthtController : ControllerBase
    {

        IAuthData auth;
        private List<Person> people = new List<Person>
        {
            new Person {Login="Eugene", Password="12345"},
           
        };

        public AuthtController(IAuthData auth)
        {
            this.auth = auth ;
        }
        /// <summary>
        /// Получить Jwt-токен 
        /// </summary>
        ///  <remarks>
        /// Метод принимает логин и пароль
        /// 
        /// Единственый зарегистрированный пользователь - логин: Eugene  пароль: 12345
        /// 
        /// Метод Jwt токен для аутентификации
        /// 
        /// </remarks>
        [HttpGet]
        [Route("api/Authenificate/{username}&{password}")]
        public async Task<IActionResult> GetToken(string username, string password)
        {
            var identity = this.auth.GetIdentity(username, password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var jwt = await this.auth.GetToken(identity);
            Response.Headers.Add("x-access-token", jwt);

            return new JsonResult(jwt);
        }
        

    }
}
