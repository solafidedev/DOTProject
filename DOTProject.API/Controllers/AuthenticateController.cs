using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DOTProject.API.Configuration;
using DOTProject.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DOTProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private const string _username = "admin";
        private const string _password = "TGU18q35EKcshos9eEwp6TJ9PpTIzKtka2cOFDuYZB4=";//Admin#1234
        private readonly IConfiguration _configuration;
        public AuthenticateController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            bool isValid = (model.Username == _username) && (Helper.Hash(model.Password) == _password);

            if (!isValid)
            {
                return BadRequest("User Not Found or incorrect Password");
            }

            var authClaims = new List<Claim>
            {
                new Claim("username", _username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = GetToken(authClaims);

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            });
        }
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration?["JWT:Secret"] ?? ""));

            var token = new JwtSecurityToken(
                issuer: _configuration?["JWT:ValidIssuer"],
                audience: _configuration?["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
    }
}