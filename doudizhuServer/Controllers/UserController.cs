using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace doudizhuServer
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MySqlDbContext _dbContext;
        public UserController(MySqlDbContext dbContext)
        {
            _dbContext = dbContext;
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
                switch (action)
                {
                    case "register":
                    {
                        string email = input["email"].ToString().Trim();
                        string password = input["password"].ToString().Trim();
                        string nickname = input["nickname"].ToString().Trim();
                        break;
                    }
                    case "login":
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
    }
}
