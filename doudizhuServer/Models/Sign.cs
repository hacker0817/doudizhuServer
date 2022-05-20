using System;
using System.Collections.Generic;

namespace doudizhuServer.Models
{
    public partial class Sign
    {
        /// <summary>
        /// 签到时间
        /// </summary>
        public DateTime SignTime { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
    }
}
