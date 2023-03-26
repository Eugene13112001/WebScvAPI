using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebScvAPI.Models;
using WebScvAPI.Settings;

namespace WebScvAPI.Containers
{
    public interface IAuthData
    {
        public  Task<string> GetToken(ClaimsIdentity identity);
        public ClaimsIdentity? GetIdentity(string username, string password);
    }
    public class AuthData: IAuthData
    {
        private List<Person> people = new List<Person>
        {
            new Person {Login="Eugene", Password="12345"},

        };
        public async Task<string> GetToken(ClaimsIdentity identity)
        {
           

            
            var key = Encoding.ASCII.GetBytes("This is a sample secret key - please don't use in production environment.");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
            {
              
                new Claim(JwtRegisteredClaimNames.Sub, "Eugene"),
                
               
             }),
                Expires = DateTime.UtcNow.AddMinutes(5),
               
                SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            var stringToken = tokenHandler.WriteToken(token);

            return stringToken;
        }
        public ClaimsIdentity? GetIdentity(string username, string password)
        {
            Person? person = people.FirstOrDefault(x => x.Login == username && x.Password == password);
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, person.Login)

                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }
    }
}
