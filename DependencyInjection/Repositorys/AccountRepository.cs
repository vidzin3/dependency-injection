using Contract;
using Entities.Auth;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Repositorys
{
    public class AccountRepository : IAccountInterface
    {
        protected readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;

        public AccountRepository(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<object> Login(LoginRequestDto request)
        {
            var user = await userManager.FindByNameAsync(request.UserName);
            var userrole = await userManager.GetRolesAsync(user);   
            if(user == null)
            {
                throw new Exception(@$"This ${request.UserName} is not exist");
            }
            if(user == null && !await userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new Exception("Username or Password is invalid");
            }
            var authClaim = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach(var role in userrole)
            {
                authClaim.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = GetToken(authClaim);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpired = DateTime.UtcNow.AddMinutes(10);
            await userManager.UpdateAsync(user);

            var tokenHandler = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenHandler;
        }
        public async Task<object> Register(RegisterRequestDto request)
        {
            var userByName = await userManager.FindByNameAsync(request.UserName);
            var userByEmail = await userManager.FindByEmailAsync(request.Email);
            if(userByName != null || userByEmail != null) 
            {
                throw new Exception("this user is already exist");
            }
            User user = new()
            {
                UserName = request.UserName,
                Email = request.Email,
                SecurityStamp = Guid.NewGuid().ToString(),  
            };
            var result = await userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception("Register Failed");
            }
            if (!await roleManager.RoleExistsAsync(RoleType.User))
            {
                await roleManager.CreateAsync(new Role() { Name = RoleType.User });
            }
            if (await roleManager.RoleExistsAsync(RoleType.User))
            {
                await userManager.AddToRoleAsync(user, RoleType.User);
            }
            return await Login(new LoginRequestDto { UserName = request.UserName, Password = request.Password });
        }
        public async Task<object> RegisterAdmin(RegisterRequestDto request)
        {
            var userByName = await userManager.FindByNameAsync(request.UserName);
            var userByEmail = await userManager.FindByEmailAsync(request.Email);
            if(userByName != null || userByEmail != null)
            {
                throw new Exception("this user is not exist");
            }
            User user = new()
            {
                UserName = request.UserName,
                Email = request.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            var result = await userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception("Register Failed");
            }
            if(!await roleManager.RoleExistsAsync(RoleType.Admin))
            {
                await roleManager.CreateAsync(new Role() { Name = RoleType.Admin });
            }
            if(!await roleManager.RoleExistsAsync(RoleType.User))
            {
                await roleManager.CreateAsync(new Role() { Name = RoleType.User });
            }
            if (await roleManager.RoleExistsAsync(RoleType.Admin))
            {
                await userManager.AddToRoleAsync(user, RoleType.Admin);
            }
            if (await roleManager.RoleExistsAsync(RoleType.User))
            {
                await userManager.AddToRoleAsync(user, RoleType.User);
            }

            return await Login(new LoginRequestDto { UserName = request.UserName, Password = request.Password });
        }
        public async Task<object> Logout()
        {
            return "";
        }
        public async Task<object> RefreshToken(TokenModel token)
        {
            if(token == null)
            {
                throw new Exception("Token is invalid");
            }
            string? accessToken = token.AccessToken;
            string? refreshToken = token.RefreshToken;

            var principle = GetClaimsPrincipal(accessToken);
            if(principle == null)
            {
                throw new Exception("token Principle in invalid");
            }

            string? username = principle.Identity.Name;

            var user = await userManager.FindByNameAsync(username);
            var NewAccessToken = GetToken(principle.Claims.ToList());
            var NewRefreshToken = GenerateRefreshToken();

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpired < DateTime.Now)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpired = DateTime.MinValue;
                await userManager.UpdateAsync(user);
                throw new Exception("Invalid access token or refresh token");
            }
            else
            {
                user.RefreshToken = NewRefreshToken;
                user.RefreshTokenExpired = DateTime.UtcNow.AddMinutes(10);
                await userManager.UpdateAsync(user);

                return new OkObjectResult(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(NewAccessToken),
                    refreshtoken = NewRefreshToken
                });
            }
        }
        private JwtSecurityToken GetToken(IEnumerable<Claim> authClaim)
        {
            var securityAuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthContext.Secret));
            var securityAlgorithm = SecurityAlgorithms.HmacSha256;
            var token = new JwtSecurityToken(
                    issuer: AuthContext.Issure,
                    audience: AuthContext.Audience,
                    claims:authClaim,
                    expires: DateTime.UtcNow.AddMinutes(5),
                    signingCredentials: new SigningCredentials(securityAuthKey,securityAlgorithm)
                );

            return token;
        }
        private ClaimsPrincipal? GetClaimsPrincipal(string token)
        {
            var tokenValidateParameter = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthContext.Secret))
            };

            var tokenValidate = new JwtSecurityTokenHandler();
            var principle = tokenValidate.ValidateToken(token, tokenValidateParameter, out SecurityToken securityToken);
            if(securityToken is not JwtSecurityToken jwtSecurity || !jwtSecurity.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception("Token is invalid");
            }
            return principle;      
        }
        private string GenerateRefreshToken()
        {
            var RandomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(RandomNumber);
            return Convert.ToBase64String(RandomNumber);
        }
    }
}
