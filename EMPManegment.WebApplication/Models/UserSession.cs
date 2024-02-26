﻿using Microsoft.AspNetCore.Http;

namespace EMPManegment.Web.Models
{
    public class UserSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static IHttpContextAccessor _staticHttpContextAccessor;

        public UserSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _staticHttpContextAccessor = httpContextAccessor;
        }

        HttpContext HttpContext => _httpContextAccessor.HttpContext;
        static HttpContext StaticHttpContext => _staticHttpContextAccessor.HttpContext;

        public Guid UserId
        {
            get
            {
                var userid = StaticHttpContext.User.Claims.FirstOrDefault(x => string.Compare(x.Type, "UserId", true) == 0);
                return userid != null ? Guid.Parse(userid.Value) : Guid.Empty;
            }
        }



        public string FirstName
        {
            get
            {
                return HttpContext.User.Claims.FirstOrDefault(x => string.Compare(x.Type, "FirstName", true) == 0)?.Value;
            }
        }

        public string FullName
        {
            get
            {
                return HttpContext.User.Claims.FirstOrDefault(x => string.Compare(x.Type, "FullName", true) == 0)?.Value;
            }
        }

        public string UserName
        {
            get
            {
                return HttpContext.User.Claims.FirstOrDefault(x => string.Compare(x.Type, "UserName", true) == 0)?.Value;
            }
        }

        public static string ProfilePhoto
        {
            get
            {
                if (StaticHttpContext.Session.GetString("ProfilePhoto") == null)
                    return null;
                else
                    return StaticHttpContext.Session.GetString("ProfilePhoto");
            }
            set
            {
                StaticHttpContext.Session.SetString("ProfilePhoto", value);
            }
        }

        public string UserRoll
        {
            get
            {
                return HttpContext.User.Claims.FirstOrDefault(x => string.Compare(x.Type, "IsAdmin", true) == 0)?.Value;
            }
        }

        public static string ProjectId
        {
            get
            {
                if (StaticHttpContext.Session.GetString("ProjectId") == null)
                    return null;
                else
                    return StaticHttpContext.Session.GetString("ProjectId");
            }
            set
            {
                StaticHttpContext.Session.SetString("ProjectId", value);
            }
        }

        public static string ProjectName
        {
            get
            {
                if (StaticHttpContext.Session.GetString("ProjectName") == null)
                    return null;
                else
                    return StaticHttpContext.Session.GetString("ProjectName");
            }
            set
            {
                StaticHttpContext.Session.SetString("ProjectName", value);
            }
        }
    }
}
