using System.Collections.Generic;
using System.ComponentModel;
using CoreFX.Abstractions.Contracts;

namespace CoreFX.Abstractions.Bases.Interfaces
{
    public interface ISvcResponseBaseDto
    {
        /// <summary>
        /// Custom status code
        /// @See SvcCodeEnum
        /// </summary>
        int Code { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        string Msg { get; set; }

        /// <summary>
        /// Message Id
        /// </summary>
        string MsgId { get; set; }

        /// <summary>
        /// The most important Successful flag after function, procedure, service call
        /// </summary>
        [DefaultValue(false)]
        bool IsSuccess { get; set; }

        /// <summary>
        /// Internal status code
        /// </summary>
        string SubCode { get; set; }

        /// <summary>
        /// Internal message or error message
        /// </summary>
        string SubMsg { get; set; }

        /// <summary>
        /// Extra error detail if exists
        /// </summary>
        ErrorDetailDto Errors { get; set; }

        /// <summary>
        /// Extension Key,Value Map
        /// </summary>
        Dictionary<string, string> ExtMap { get; set; }
    }

    public interface ISvcResponseBaseDto<T> : ISvcResponseBaseDto
    {
        T Data { get; set; }
    }
}
