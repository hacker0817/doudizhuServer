using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace doudizhuServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DoudizhuController : ControllerBase
    {
        public DoudizhuController()
        {
        }

        [HttpPost]
        [Authorize]
        public JObject Post(string action, [FromBody] JObject input)
        {
            JObject result = new JObject();
            try
            {
                string msg = "";
                switch (action)
                {
                    case "readyGame":
                        {
                            var UserId = HttpContext.User.Claims.Where(c => c.Type == "UserId").FirstOrDefault().Value;
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

        public bool IsCanReadToken(ref string tokenStr)
        {
            if (string.IsNullOrWhiteSpace(tokenStr) || tokenStr.Length < 7)
                return false;
            if (!tokenStr.Substring(0, 6).Equals(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme))
                return false;
            tokenStr = tokenStr.Substring(7);
            bool isCan = new JwtSecurityTokenHandler().CanReadToken(tokenStr);

            return isCan;
        }
    }
}
