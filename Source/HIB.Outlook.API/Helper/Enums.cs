using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIB.Outlook.API.Helper
{
    public enum ResponseStatus
    {
        Sucess = 200,
        BadRequest = 400,
        Unauthorized = 401,
        NotFound = 404,
        InternalError = 500,
        NoContent = 204,          //The server successfully processed the request and is not returning any content.
        UnprocessableEntity = 422, //The request was well-formed but was unable to be followed due to semantic errors
        UnSupportedMediaType = 415
    }

}