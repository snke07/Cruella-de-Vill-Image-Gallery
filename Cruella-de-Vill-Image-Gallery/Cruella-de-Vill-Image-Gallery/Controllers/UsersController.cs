using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using CruellaDeVillImageGallery.Models;
using CruellaDeVillImageGallery.Repositories;

namespace CruellaDeVillImageGallery.Controllers
{
    public class UsersController : BaseApiController
    {
        [HttpGet]
        [ActionName("test")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        [ActionName("register")]
        public HttpResponseMessage RegisterUser(UserRegisterModel user)
        {
            var responseMsg = this.PerformOperation(() =>
            {
                UsersRepository.CreateUser(user.Email, user.Nickname, user.AuthCode);

                string nickname = string.Empty;
                var sessionKey = UsersRepository.LoginUser(user.Email, user.AuthCode, out nickname);
                return new UserLoggedModel()
                {
                    Nickname = nickname,
                    SessionKey = sessionKey
                };
            });
            return responseMsg;
        }

        [HttpPost]
        [ActionName("login")]
        public HttpResponseMessage LoginUser(UserLoginModel user)
        {
            var responseMsg = this.PerformOperation(() =>
            {
                string nickname = string.Empty;
                var sessionKey = UsersRepository.LoginUser(user.Email, user.AuthCode, out nickname);
                return new UserLoggedModel()
                {
                    Nickname = nickname,
                    SessionKey = sessionKey
                };
            });
            return responseMsg;
        }

        [HttpGet]
        [ActionName("logout")]
        public HttpResponseMessage LogoutUser(string sessionKey)
        {
            var responseMsg = this.PerformOperation(() =>
            {
                UsersRepository.LogoutUser(sessionKey);
            });
            return responseMsg;
        }
    }
}
