using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class UtilitariesResponse<T>
    {
        public ResponseBase<T> SetResponseBaseForExecuteSQLCommand(int result)
        {
            ResponseBase<T> response = new ResponseBase<T>();
            if (result >= 0)
            {
                response.Code = ConfigurationLib.SuccessCode;
                response.Message = ConfigurationLib.SuccessMessageES;
            }
            else
            {
                response.Code = ConfigurationLib.DataNotFoundErrorCode;
                response.Message = ConfigurationLib.DataNotFoundMessageES;
            }
            return response;
        }
        public ResponseBase<T> SetResponseBaseForList(IQueryable<T> query)
        {
            ResponseBase<T> response = new ResponseBase<T>();
            if (query == null)
            {
                response.Code = ConfigurationLib.DataNotFoundErrorCode;
                response.Message = ConfigurationLib.DataNotFoundMessageES;
            }
            else
            {
                if (query.Any())
                {
                    response.Code = ConfigurationLib.SuccessCode;
                    response.Message = ConfigurationLib.SuccessMessageES;
                    response.List = query.ToList();
                    response.IsResultList = true;
                }
                else
                {
                    response.Code = ConfigurationLib.DataNotFoundErrorCode;
                    response.Message = ConfigurationLib.DataNotFoundMessageES;
                }
            }
            return response;
        }
        public ResponseBase<T> SetResponseBaseForObj(T obj)
        {
            ResponseBase<T> response = new ResponseBase<T>();
            if (obj != null)
            {
                response.Code = ConfigurationLib.SuccessCode;
                response.Message = ConfigurationLib.SuccessMessageES;
                response.Object = obj;
            }
            else
            {
                response.Code = ConfigurationLib.DataNotFoundErrorCode;
                response.Message = ConfigurationLib.DataNotFoundMessageES;
            }
            return response;
        }

        public ResponseBase<T> SetResponseBaseForValidationExceptionString(IList<string> errors)
        {
            ResponseBase<T> response = new ResponseBase<T>();
            response.Code = ConfigurationLib.InvalidParametersCode;
            response.Message = ConfigurationLib.InvalidParametersMessageES;
            response.FunctionalErrors = errors.ToList();
            return response;
        }
        //public ResponseBase<T> SetResponseBaseForOK()
        //{
        //    ResponseBase<T> response = new ResponseBase<T>();
        //    response.Code = ConfigurationLib.SuccessCode;
        //    response.Message = ConfigurationLib.SuccessMessageES;
        //    return response;
        //}
        public ResponseBase<T> SetResponseBaseForOK(T obj)
        {
            ResponseBase<T> response = new ResponseBase<T>();
            response.Code = ConfigurationLib.SuccessCode;
            response.Message = ConfigurationLib.SuccessMessageES;
            if (obj != null) response.Object = obj;
            return response;
        }
        public ResponseBase<T> SetResponseBaseForOK(IEnumerable<T> obj)
        {
            ResponseBase<T> response = new ResponseBase<T>();
            response.Code = ConfigurationLib.SuccessCode;
            response.Message = ConfigurationLib.SuccessMessageES;
            if (obj.Any()) { response.List = obj; response.IsResultList = true; }
            return response;
        }
        public ResponseBase<T> SetResponseBaseForExceptionUnexpected()
        {
            ResponseBase<T> response = new ResponseBase<T>();
            response.Code = ConfigurationLib.NotSpecifiedErrorCode;
            response.Message = ConfigurationLib.NotSpecifiedErrorMessageES;
            return response;
        }
        public ResponseBase<T> SetResponseBaseForException(Exception ex)
        {
            ResponseBase<T> response = new ResponseBase<T>();
            if (ex is TimeoutException)
            {
                response.Code = ConfigurationLib.TimeoutErrorCode;
                response.Message = ConfigurationLib.TimeoutErrorMessageES;
            }
            else if (ex is HttpRequestException)
            {
                response.Code = ConfigurationLib.TimeoutErrorCode;
                response.Message = ConfigurationLib.TimeoutErrorMessageES;
            }
            else if (ex is WSNotAuthorized)
            {
                response.Code = ConfigurationLib.UnauthorizedErrorCode;
                response.Message = ConfigurationLib.UnauthorizedErrorMessageES;
            }
            else if (ex is WSNotFoundException)
            {
                response.Code = ConfigurationLib.NotFoundErrorCode;
                response.Message = ConfigurationLib.NotFoundErrorMessageES;
            }
            else
            {
                response.Code = ConfigurationLib.NotSpecifiedErrorCode;
                response.Message = ConfigurationLib.NotSpecifiedErrorMessageES;
            }
            response.TechnicalErrors = ex;
            return response;
        }
        public ResponseBase<T> SetResponseBaseForNoAuthorized()
        {
            ResponseBase<T> response = new ResponseBase<T>();
            response.Code = ConfigurationLib.UnauthorizedErrorCode;
            response.Message = ConfigurationLib.UnauthorizedErrorMessageES;
            //response.List = new List<T>();
            return response;
        }
        //public ResponseBase<T> SetResponseBaseForNoAuthorized(TokenErrorResponse error)
        //{
        //    ResponseBase<T> response = new ResponseBase<T>();
        //    response.Code = ConfigurationLib.CodigoErrorNoAuthorized;
        //    response.Message = ConfigurationLib.MensajeErrorNoAuthorizedES;
        //    response.MessageEN = ConfigurationLib.MensajeErrorNoAuthorizedEN;
        //    response.List = new List<T>();
        //    var errorResponse = new List<string>();
        //    errorResponse.Add(error.ErrorDescription);
        //    response.FunctionalErrors = errorResponse;
        //    return response;
        //}
        public ResponseBase<T> SetResponseBaseForNoDataFound()
        {
            ResponseBase<T> response = new ResponseBase<T>();
            response.Code = ConfigurationLib.DataNotFoundErrorCode;
            response.Message = ConfigurationLib.DataNotFoundMessageES;
            response.List = new List<T>();
            return response;
        }
        public ResponseBase<T> SetResponseBaseForParameterNoValid()
        {
            ResponseBase<T> response = new ResponseBase<T>();
            response.Code = ConfigurationLib.InvalidParametersCode;
            response.Message = ConfigurationLib.InvalidParametersMessageES;
            response.List = new List<T>();
            return response;
        }
        public ResponseBase<T> SetResponseBaseForUniqueValidation()
        {
            ResponseBase<T> response = new ResponseBase<T>();
            response.Code = ConfigurationLib.DataExistsCode;
            response.Message = ConfigurationLib.DataExistsMessage;
            return response;
        }

        public ResponseBase<T> SetResponseBaseFunctionalErrors(ValidationResult results)
        {
            ResponseBase<T> response = new ResponseBase<T>();
            response.Code = ConfigurationLib.InvalidParametersCode;
            response.Message = ConfigurationLib.InvalidParametersMessageES;
            List<string> listErrors = new List<string>(); 
            foreach (var failure in results.Errors)
            {
                listErrors.Add(failure.ErrorMessage);
            }
            response.FunctionalErrors = listErrors;
            return response;
        }
    }
}
