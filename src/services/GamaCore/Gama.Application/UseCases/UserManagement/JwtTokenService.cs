using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Gama.Application.Contracts.Repositories;
using Gama.Application.Contracts.UserManagement;
using Gama.Application.DataContracts.Commands;
using Gama.Domain.Exceptions;
using Gama.Domain.ValueTypes;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Gama.Application.UseCases.UserManagement;

public class JwtTokenService : ITokenService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    
    public JwtTokenService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<Result<string>> Generate(TokenCreationCommand tokenCreationCommand)
    {
        var user = await _userRepository.GetByLoginAsync(tokenCreationCommand.Email, tokenCreationCommand.Username);

        if (user is null)
        {
            return new Result<string>(new ValidationException(new ValidationError()
                { PropertyName = "user", ErrorMessage = "Usuário ou senha inválidos" }));
        }

        var validPassword = user.IsValidPassword(tokenCreationCommand.Password);
        if (!validPassword)
        {
            return new Result<string>(new ValidationException(new ValidationError()
                { PropertyName = "user", ErrorMessage = "Usuário ou senha inválidos" }));
        }
        
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var key = Encoding.ASCII.GetBytes
            (_configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString()),
                new Claim("Role", user.Role.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(5),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        var stringToken = tokenHandler.WriteToken(token);
        return stringToken;
    }
}