using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using P320.AuthenticationService.Contracts;
using P320.AuthenticationService.Models;
using P320.DomainModels.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace P320.AuthenticationService
{
    public class AuthService : IAuthService
    {
        private readonly JwtSetting _jwtSetting;
        private readonly List<User> _users;

        public AuthService(IOptions<JwtSetting> jwtSetting)
        {
            _jwtSetting = jwtSetting.Value;
            _users = new List<User>
            {
                new User {Username = "Aydin", Password = "123", Fullname = "Aydin Yusubov"},
                new User {Username = "Aydin2", Password = "123", Fullname = "Aydin2 Yusubov2"},
                new User {Username = "Aydin3", Password = "123", Fullname = "Aydin3 Yusubov3"},
            };
        }

        public string GetToken(CredentialModel credentialModel)
        {
            var user = _users.Find(x => x.Username == credentialModel.Username);
            if (user == null)
            {
                throw new Exception("Invalid Credentials");
            }

            if (user.Password != credentialModel.Password)
            {
                throw new Exception("Invalid Credentials");
            }

            var jwtSecurityToken = CreateJwtToken(user);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return token;
        }
        private JwtSecurityToken CreateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("roles", "Admin"),
                new Claim("roles", "Member"),
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            };

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                _jwtSetting.Issuer,
                _jwtSetting.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSetting.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }
    }
}
