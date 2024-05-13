using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;


namespace TaskHub.Business
{
    public class TokenServiceExtension
    {
        private SymmetricSecurityKey _SymmetricSecurityKey;
        private string _audience;
        private string _issuer;
        private string _cookieName;

        public TokenServiceExtension(IConfiguration configuration)
        {
            var TokTokenAuthentication = configuration.GetSection("TokenAuthentication");
            _audience = TokTokenAuthentication.GetValue<string>("Audience");
            _issuer = TokTokenAuthentication.GetValue<string>("Issuer");
            _cookieName = TokTokenAuthentication.GetValue<string>("CookieName");
            _SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(TokTokenAuthentication.GetValue<string>("SecretKey")));

        }

        public Action<JwtBearerOptions> Validation()
        {

            Action<JwtBearerOptions> options = o =>
            {

                o.Audience = _audience;
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _SymmetricSecurityKey,
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };
                o.Events = new JwtBearerEvents
                {
                    OnChallenge = (context) =>
                    {
                        string result = "";
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";

                        if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
                        {
                            result = JsonConvert.SerializeObject(new
                            {
                                errorNumber = 2,
                                value = "non authorisé",
                                message = "mauvais paramètres d'authorization"
                            });
                        }
                        else
                        {
                            result = JsonConvert.SerializeObject(new
                            {
                                errorNumber = 2,
                                value = "non authorisé",
                                detail = "mauvais paramètres d'authorization"
                            });
                        }
                        return context.Response.WriteAsync(result);
                    },
                };
            };

            return options;
        }

        public Action<CookieAuthenticationOptions> RefreshOptions()
        {
            Action<CookieAuthenticationOptions> options = option =>
            {
                option.Cookie.Name = _cookieName;
                option.ExpireTimeSpan = TimeSpan.FromDays(30);
                option.SlidingExpiration = true;
            };

            return options;
        }
    }
}
