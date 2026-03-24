using System.Threading.Tasks;
using CoreFX.Abstractions.Contracts;
using CoreFX.Auth.Models;

namespace CoreFX.Auth.Interfaces
{
    public interface ISessionAdmin
    {
        // Help check authentication
        Task<SvcResponseDto<JwtTokenDto>> Authentication(string username, string password);

        // Help refresh token
        SvcResponseDto<JwtTokenDto> RefeshToken(string refreshToken);
    }
}
