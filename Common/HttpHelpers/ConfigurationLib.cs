using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class ConfigurationLib
    {
        public static int SuccessCode { get { return 200; } }
        public static string SuccessMessageES { get { return "Operación Exitosa"; } }
        public static int DataNotFoundErrorCode { get { return 201; } }
        public static string DataNotFoundMessageES { get { return "No se encontraron registros"; } }

        public static int LoginCode { get { return 202; } }
        public static string LoginMessageES { get { return "Usuario y password incorrecto"; } }

        public static int DataExistsCode { get { return 203; } }
        public static string DataExistsMessage { get { return "Lo sentimos, ya existen los registros"; } }

        public static int UserExistsCode { get { return 203; } }

        public static string UserExistsMessageES { get { return "El usuario ya existe"; } }

        public static int InvalidParametersCode { get { return 204; } }
        public static string InvalidParametersMessageES { get { return "Parámetros Ingresados Incorrectos"; } }



        public static int DBErrorCode { get { return 501; } }
        public static string DBErrorMessage { get { return "Error de conexión"; } }

        public static int TimeoutErrorCode { get { return 502; } }
        public static string TimeoutErrorMessageES { get { return "Error de tiempo de espera"; } }


        public static int NotSpecifiedErrorCode { get { return 500; } }

        public static string NotSpecifiedErrorMessageES { get { return "Error no especificado"; } }

        public static int NotFoundErrorCode { get { return 400; } }
        public static string NotFoundErrorMessageES { get { return "Error No encontrado"; } }

        public static int UnauthorizedErrorCode { get { return 401; } }
        public static string UnauthorizedErrorMessageES { get { return "No Autorizado"; } }



    }
}
