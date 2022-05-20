using System;
using System.Collections.Generic;

namespace doudizhuServer.Models
{
    public partial class User
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = null!;
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; } = null!;
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; } = null!;
    }
}
