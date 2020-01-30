using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public static class ConstantData
    { 
        public static int intTrue = 1;
        public static int intFalse = 0;
        public static DateTime fechaActual = DateTime.Now;
        public static bool boolTrue = true;
        public static bool boolFalse = false;
        //Variables a usar para generar JWT Token
        public static string authSecretKey = "hellmann-api-security";
        public static string forgotPasswordSecretKey = "reset-password-key";
        public static string audienceToken = "https://localhost:44355";
        public static string issuerToken = "https://localhost:44355";
        public static string expireAuthTimeToken = "60";
        public static string expireForgotTimeToken = "30";

        //Credenciales servidor de correos
        public static string fromEmail = "admin@hplperu.com";
        public static string usernameEmail = "jose.cristobal@tecsup.edu.pe";
        public static string passwordEmail = "T3csup3020";
        public static string emailHost = "smtp.gmail.com";
        public static int emailPort = 25;
        public static bool enableSsl = true;
    }
}
