﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuth.Core.Auth
{
    public class VerifyEmailRequest
    {
        public string Username { get; set; }
        public string Token { get; set; }
    }
}
