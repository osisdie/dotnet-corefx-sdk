using System.ComponentModel.DataAnnotations;

namespace CoreFX.Abstractions.Enums
{
    /// <summary>
    /// Platform Error Code
    /// </summary>
    public enum SvcCodeEnum
    {
        [Display(Name = "Unset")]
        UnSet = -0001,

        [Display(Name = "Error")]
        Error = 0000,

        [Display(Name = "Success")]
        Success = 0001,

        [Display(Name = "Exception")]
        Exception = 0003,

        [Display(Name = "Invalid contract")]
        InvalidContract = 0004,

        [Display(Name = "Internal HTTP Error")]
        InternalHttpError = 0051,

        [Display(Name = "Internal DB Error")]
        InternalDBError = 0052,

        [Display(Name = "Internal Cache Error")]
        InternalCacheError = 0053,

        [Display(Name = "Internal Storage Error")]
        InternalStorageError = 0054,

        [Display(Name = "Internal MQ Error")]
        InternalMQError = 0055,

        [Display(Name = "Internal Socket Error")]
        InternalSocketError = 0056,

        [Display(Name = "JWT token failed to create")]
        TokenCreateFailed = 0101,

        [Display(Name = "JWT token failed to refresh")]
        TokenRefreshFailed = 0102,

        [Display(Name = "JWT token expired")]
        TokenExpired = 0103,

        [Display(Name = "JWT token invalid")]
        TokenInvalid = 0104,

        [Display(Name = "Not login yet")]
        NotLoginYet = 0301,

        [Display(Name = "Account login failed")]
        AccountLoginFailed = 0302,

        [Display(Name = "Account permission denied")]
        AccountPermissionDenied = 0303,

        [Display(Name = "Account password expired")]
        AccountPasswordExpired = 0304,

        [Display(Name = "Account password too weak")]
        AccountPasswordTooWeak = 0305,

        ErrorCode_Max = 1000
    }
}
