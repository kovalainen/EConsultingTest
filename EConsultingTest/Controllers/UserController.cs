using EConsultingTest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TokenApp.Controllers
{
	public class UserController : Controller
	{
		private readonly User m_user = new User() { Login = "admin", Password = "admin" };

		[HttpPost("/api/values/token")]
		public async Task Token([FromBody]User user)
		{
			string username = user.Login;
			string password = user.Password;

			ClaimsIdentity identity = GetIdentity(username, password);
			if (identity == null)
			{
				Response.StatusCode = 400;
				await Response.WriteAsync("Invalid username or password.");
				return;
			}

			DateTime now = DateTime.UtcNow;

			JwtSecurityToken jwt = new JwtSecurityToken(
					issuer: AuthOptions.ISSUER,
					audience: AuthOptions.AUDIENCE,
					notBefore: now,
					claims: identity.Claims,
					expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
					signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

			string encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

			var response = new
			{
				accessToken = encodedJwt,
				username = identity.Name
			};
			Response.ContentType = "application/json";
			await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
		}

		private ClaimsIdentity GetIdentity(string username, string password)
		{
			if (m_user.Login == username && m_user.Password == password)
			{
				List<Claim> claims = new List<Claim>
				{
					new Claim(ClaimsIdentity.DefaultNameClaimType, m_user.Login)
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
