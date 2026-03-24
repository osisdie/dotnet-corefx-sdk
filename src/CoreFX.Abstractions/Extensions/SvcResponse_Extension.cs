using System;
using CoreFX.Abstractions.Bases.Interfaces;
using CoreFX.Abstractions.Enums;

namespace CoreFX.Abstractions.Extensions
{
    public static class SvcResponse_Extension
    {
        public static bool Any<T>(this ISvcResponseBaseDto<T> res) =>
           true == res?.IsSuccess &&
           null != res.Data;

        public static bool FailOrEmpty<T>(this ISvcResponseBaseDto<T> res) =>
            true != res?.IsSuccess ||
            null == res.Data;

        public static ISvcResponseBaseDto<T> SetStatusCode<T>(this ISvcResponseBaseDto<T> res, bool isSuccess)
        {
            _ = isSuccess ? res.Success() : res.Error();
            return res;
        }

        public static ISvcResponseBaseDto<T> SetStatusCode<T, E>(this ISvcResponseBaseDto<T> res, E code)
            where E : struct, IConvertible
        {
            if (typeof(E).IsEnum)
            {
                res.Code = Convert.ToInt32(code);
            }

            return res;
        }

        public static ISvcResponseBaseDto<T> SetData<T>(this ISvcResponseBaseDto<T> res, T data)
        {
            res.Data = data;
            return res;
        }

        public static ISvcResponseBaseDto<T> SetMsg<T>(this ISvcResponseBaseDto<T> res, string msg)
        {
            res.Msg = msg;
            return res;
        }

        public static ISvcResponseBaseDto<T> SetSubCode<T, E>(this ISvcResponseBaseDto<T> res, E code)
            where E : struct, IConvertible
        {
            if (typeof(E).IsEnum)
            {
                res.SubCode = Convert.ToInt32(code).ToString();
                res.SubMsg = code.ToString();
            }

            return res;
        }

        public static ISvcResponseBaseDto Error(this ISvcResponseBaseDto res)
        {
            res.Error(SvcCodeEnum.Error, SvcCodeEnum.Error.ToString());
            return res;
        }

        public static ISvcResponseBaseDto Error<E>(this ISvcResponseBaseDto res, E code, Exception ex)
            where E : struct, IConvertible

        {
            res.Error(code, ex?.Message);
            return res;
        }

        public static ISvcResponseBaseDto Error<E>(this ISvcResponseBaseDto res, E code, string errMsg)
            where E : struct, IConvertible
        {
            if (typeof(E).IsEnum)
            {
                res.Code = Convert.ToInt32(code);
                res.Msg = string.IsNullOrWhiteSpace(errMsg) ? code.ToString() : errMsg;
            }
            else
            {
                res.Msg = errMsg;
            }

            res.IsSuccess = false;
            return res;
        }

        public static ISvcResponseBaseDto<T> Error<T>(this ISvcResponseBaseDto<T> res)
        {
            res.Error(SvcCodeEnum.Error, SvcCodeEnum.Error.ToString());
            return res;
        }

        public static ISvcResponseBaseDto<T> Error<T, E>(this ISvcResponseBaseDto<T> res, E code, Exception ex)
            where E : struct, IConvertible
        {
            res.Error(code, ex?.Message);
            return res;
        }

        public static ISvcResponseBaseDto<T> Error<T, E>(this ISvcResponseBaseDto<T> res, E code, string errMsg)
            where E : struct, IConvertible
        {
            if (typeof(E).IsEnum)
            {
                res.Code = Convert.ToInt32(code);
                res.Msg = string.IsNullOrWhiteSpace(errMsg) ? code.ToString() : errMsg;
            }
            else
            {
                res.Msg = errMsg;
            }

            res.IsSuccess = false;
            return res;
        }

        public static ISvcResponseBaseDto Success(this ISvcResponseBaseDto res)
        {
            res.Success(SvcCodeEnum.Success, SvcCodeEnum.Success.ToString());
            return res;
        }

        public static ISvcResponseBaseDto Success<E>(this ISvcResponseBaseDto res, E code, string msg = null)
            where E : struct, IConvertible
        {
            if (typeof(E).IsEnum)
            {
                res.Code = Convert.ToInt32(code);
                res.Msg = string.IsNullOrWhiteSpace(msg) ? code.ToString() : msg;
            }
            else
            {
                res.Msg = msg;
            }

            res.IsSuccess = true;
            return res;
        }

        public static ISvcResponseBaseDto<T> Success<T>(this ISvcResponseBaseDto<T> res)
        {
            res.Success(SvcCodeEnum.Success, SvcCodeEnum.Success.ToString());
            return res;
        }

        public static ISvcResponseBaseDto<T> Success<T, E>(this ISvcResponseBaseDto<T> res, E code, string msg = null)
            where E : struct, IConvertible
        {
            if (typeof(E).IsEnum)
            {
                res.Code = Convert.ToInt32(code);
                res.Msg = string.IsNullOrWhiteSpace(msg) ? code.ToString() : msg;
            }
            else
            {
                res.Msg = msg;
            }

            res.IsSuccess = true;
            return res;
        }
    }
}
