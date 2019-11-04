using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EConsultingTest.Models
{
	public class AuthOptions
	{
		public const string ISSUER = "MyAuthServer"; 
		public const string AUDIENCE = "https://localhost:44376/"; 
		const string KEY = "mysupersecret_secretkey!123";   
		public const int LIFETIME = 1; 
		public static SymmetricSecurityKey GetSymmetricSecurityKey()
		{
			return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
		}
	}
}
