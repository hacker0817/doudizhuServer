using codebase;
using doudizhuServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace doudizhuServer
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly gameContext _dbContext;
        public UserController(gameContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [HttpGet]
        public JObject Get(string action)
        {
            JObject result = new JObject();
            try
            {
                switch (action)
                {
                    case "action1":
                        {
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
                result.Add("state", "0");
                result.Add("msg", "");
            }
            catch (Exception ex)
            {
                result.Add("state", "1");
                result.Add("msg", ex.Message);
            }
            return result;
        }

        [HttpPost]
        public JObject Post(string action, [FromBody] JObject input)
        {
            JObject result = new JObject();
            try
            {
                string msg = "";
                switch (action)
                {
                    case "register":
                        {
                            string name = input["name"].ToString().Trim();
                            string password = input["password"].ToString().Trim();
                            string email = input["email"].ToString().Trim();
                            var user = _dbContext.Users
                                .FirstOrDefault(user => user.Username == name || user.Email == email);
                            if (user == null)
                            {
                                _dbContext.Users.Add(new User()
                                {
                                    Username = name,
                                    Password = EncryptionMD5.EncryptStringMD5(password),
                                    Email = email
                                });
                                _dbContext.SaveChanges();
                            }
                            else
                                msg = "the username or email already exists";
                            break;
                        }
                    case "login":
                        {
                            string name = input["name"].ToString().Trim();
                            string password = input["password"].ToString().Trim();
                            var user = _dbContext.Users.FirstOrDefault(user => (user.Username == name || user.Email == name));
                            if (user == null)
                            {
                                msg = "the username or email not exists";
                            }
                            else if (user.Password != EncryptionMD5.EncryptStringMD5(password))
                            {
                                msg = "the password is wrong";
                            }
                            else
                                result.Add("token", CreateToken(user));
                            break;
                        }
                }
                result.Add("msg", msg);
            }
            catch (Exception ex)
            {
                result.Add("msg", ex.Message);
            }
            return result;
        }

        public string CreateToken(User user)
        {
            // 1. 定义需要使用到的Claims
            var claims = new[]
            {
                new Claim("UserId", user.UserId.ToString()),
                new Claim("Username", user.Username),
                new Claim("Email", user.Email)
            };

            // 2. 从 appsettings.json 中读取SecretKey
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

            // 3. 选择加密算法
            var algorithm = SecurityAlgorithms.HmacSha256;

            // 4. 生成Credentials
            var signingCredentials = new SigningCredentials(secretKey, algorithm);

            // 5. 从 appsettings.json 中读取Expires
            var expires = Convert.ToDouble(_configuration["JWT:Expires"]);

            // 6. 根据以上，生成token
            var token = new JwtSecurityToken(
                _configuration["JWT:Issuer"],     //Issuer
                _configuration["JWT:Audience"],   //Audience
                claims,                          //Claims,
                DateTime.Now,                    //notBefore
                DateTime.Now.AddDays(expires),   //expires
                signingCredentials               //Credentials
            );

            // 7. 将token变为string
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
    }
}
