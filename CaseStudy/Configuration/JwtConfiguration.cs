﻿namespace CaseStudy.Configuration
{
    public class JwtConfiguration
    {
        public string Subject { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
    }
}
