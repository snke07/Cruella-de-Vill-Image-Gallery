using CruellaDeVillImageGallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace CruellaDeVillImageGallery.Controllers
{
    public class BaseApiController : ApiController
    {
        private static Dictionary<string, HttpStatusCode> ErrorToStatusCodes = new Dictionary<string, HttpStatusCode>();

        static BaseApiController()
        {
            // General error codes
            ErrorToStatusCodes["ERR_GEN_SVR"] = HttpStatusCode.InternalServerError;
            // User error codes
            ErrorToStatusCodes["INV_EMAIL_LEN"] = HttpStatusCode.BadRequest;
            ErrorToStatusCodes["INV_EMAIL_CHARS"] = HttpStatusCode.BadRequest;
            ErrorToStatusCodes["INV_NICK_LEN"] = HttpStatusCode.BadRequest;
            ErrorToStatusCodes["INV_NICK_CHARS"] = HttpStatusCode.BadRequest;
            ErrorToStatusCodes["INV_EMAIL_AUTH_LEN"] = HttpStatusCode.BadRequest;
            ErrorToStatusCodes["ERR_DUP_EMAIL"] = HttpStatusCode.Conflict;
            ErrorToStatusCodes["ERR_DUP_NICK"] = HttpStatusCode.Conflict;
            ErrorToStatusCodes["INV_USR_AUTH"] = HttpStatusCode.BadRequest;
            ErrorToStatusCodes["ERR_INV_USR"] = HttpStatusCode.BadRequest;
        }

        public BaseApiController()
        {
        }

        protected HttpResponseMessage PerformOperation(Action action)
        {
            try
            {
                action();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ServerErrorException ex)
            {
                return BuildErrorResponse(ex.Message, ex.ErrorCode);
            }
            catch (Exception ex)
            {
                var errCode = "ERR_GEN_SVR";
                return BuildErrorResponse(ex.Message, errCode);
            }
        }

        protected HttpResponseMessage PerformOperation<T>(Func<T> action)
        {
            try
            {
                var result = action();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (ServerErrorException ex)
            {
                return BuildErrorResponse(ex.Message, ex.ErrorCode);
            }
            catch (Exception ex)
            {
                var errCode = "ERR_GEN_SVR";
                return BuildErrorResponse(ex.Message, errCode);
            }
        }

        private HttpResponseMessage BuildErrorResponse(string message, string errCode)
        {
            var httpError = new HttpError(message);
            httpError["errCode"] = errCode;
            var statusCode = ErrorToStatusCodes[errCode];
            return Request.CreateErrorResponse(statusCode, httpError);
        }
    }
}
