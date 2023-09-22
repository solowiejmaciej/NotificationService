﻿using System;

namespace AuthService.Application.Models.Responses
{
    public class TokenResponse
    {
        public string Token { get; set; }
        
        public string RefreshToken { get; set; }
        public int StatusCode { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string UserId { get; set; }
    }
}