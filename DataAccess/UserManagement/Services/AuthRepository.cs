using DataAccess.UserManagement.Interfaces;
using DataAccess.UserManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.UserManagement.Services
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataAccessContext _context;
        private readonly IConfiguration _configuration;
        public AuthRepository(DataAccessContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async  Task<ServiceResponse<string>> Login(string email, string password)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower()); //Get user from database.
            if (user == null)
            {
                response.Success = false;
                response.Message = "Invalid Credentials";
            }
            else if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Invalid Credentials";
            }
            else
            {
                response.Model = CreateToken(user);
            }
            return response;
        }
        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                { 
                    if (computedHash[i] != passwordHash[i]) return false; 
                }
            }
            return true; 
        }
        public async  Task<ServiceResponse<ApplicationUser>> Register(ApplicationUser user, string password)
        {
            ServiceResponse<ApplicationUser> response = new ServiceResponse<ApplicationUser>();
         
          
        
                try
                {
                ApplicationUser userFromEmail = await _context.Users.Where(u => u.Email == user.Email).FirstOrDefaultAsync();
                if (userFromEmail != null)
                {
                    response.Success = false;
                    response.Message = "Uneseni email već postoji";
                }
                else
                {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash(password, out passwordHash, out passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;

                    await _context.Users.AddAsync(user); // Adding the user to context of users.
                    await _context.SaveChangesAsync(); // Save changes to database.
                    response.Model = user;
                }
                
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Message = ex.Message;
                }

            
           
            return response;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        public async  Task<bool> UserExists(string email)
        {
            if (await _context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower()))
                return true;
            return false;
        }

        private string CreateToken(ApplicationUser user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.PkUserId.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name,user.Ime)
            };
            SymmetricSecurityKey key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(5),
                SigningCredentials = creds,
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
