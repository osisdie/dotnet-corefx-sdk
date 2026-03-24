using System;
using System.Threading.Tasks;
using CoreFX.Abstractions.Contracts;
using CoreFX.Abstractions.Extensions;
using CoreFX.Abstractions.Logging;
using CoreFX.Auth.Interfaces;
using CoreFX.Auth.Models;
using CoreFX.Auth.Utils;
using Microsoft.Extensions.Logging;

namespace CoreFX.Auth.Services
{
    public class SessionAdmin : ISessionAdmin
    {
        protected readonly ILogger _logger;

        public SessionAdmin(ILogger<SessionAdmin> logger)
        {
            _logger = logger ?? LogMgr.CreateLogger(GetType());
        }

        /// <summary>
        /// Authentication with JWT
        /// </summary>
        /// <param name="username">DA</param>
        /// <param name="password">CH</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<SvcResponseDto<JwtTokenDto>> Authentication(string username, string password)
        {
            var res = new SvcResponseDto<JwtTokenDto>();

            try
            {
                var jwtTokenDto = new JwtTokenDto
                {
                    UserId = username.ToMD5(),
                    UserName = username
                    //FullName = username,
                    //Email = adminInfo.Email,
                };

                var accessToken = JwtUtil.GenTokenkey(jwtTokenDto, 60);

                jwtTokenDto._id = Guid.NewGuid().ToString();
                var newRefreshToken = JwtUtil.GenTokenkey(jwtTokenDto, 10800);

                jwtTokenDto.Exp = jwtTokenDto.Exp;
                jwtTokenDto.Token = accessToken;
                jwtTokenDto.RefreshToken = newRefreshToken;

                res.SetData(jwtTokenDto).Success();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.ToString());
            }

            await Task.CompletedTask;
            return res;
        }

        /// <summary>
        /// Refresh token for user
        /// </summary>
        /// <param name="refreshToken">Current refresh token</param>
        /// <returns></returns>
        public SvcResponseDto<JwtTokenDto> RefeshToken(string refreshToken)
        {
            var response = new SvcResponseDto<JwtTokenDto>();
            try
            {
                //Step 1: Check null & validate token 
                if (string.IsNullOrEmpty(refreshToken))
                {
                    response.Msg = "token does not exist.";
                    return response;
                }

                var isTokenValid = JwtUtil.IsTokenValid(refreshToken);
                if (!isTokenValid)
                {
                    response.Msg = "token invalid.";
                    return response;
                }

                // Step 2: Create new token & return
                var payload = JwtUtil.ExtracToken(refreshToken);
                var newAccessToken = JwtUtil.GenTokenkey(payload, 60);
                var newRefreshToken = JwtUtil.GenTokenkey(payload, 10800);
                payload.Token = newAccessToken;
                payload.RefreshToken = newRefreshToken;

                response.Success().SetData(payload);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                response.Msg = "Session error.";
                return response;
            }
        }
    }
}
