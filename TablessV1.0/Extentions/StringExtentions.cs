using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace TablessV1._0.Extentions
{
    public static class StringExtension
    {
        public static string generateAppSecretProof(this String access_token)
        {
            using (HMACSHA256 algorithm = new HMACSHA256(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["Facebook_AppSecret"])))
            {
                byte[] hash = algorithm.ComputeHash(Encoding.ASCII.GetBytes(access_token));
                StringBuilder bulder = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    bulder.Append(hash[i].ToString("x2", CultureInfo.InvariantCulture));
                }
                return Convert.ToString(bulder);
            }

        }

        public static string GraphAPICall(this string baseGraphApiCall, params object[] args)
        {
            //returns a formatted Graph Api Call with a version prefix and appends a query string parameter containing the appsecret_proof value
            if (!string.IsNullOrEmpty(baseGraphApiCall))
            {
                if (args != null &&
                    args.Count() > 0)
                {
                    //Determine if we need to concatenate appsecret_proof query string parameter or inject it as a single query string paramter
                    string _graphApiCall = string.Empty;
                    if (baseGraphApiCall.Contains("?"))
                        _graphApiCall = string.Format(baseGraphApiCall + "&appsecret_proof={" + (args.Count() - 1) + "}", args);
                    else
                        _graphApiCall = string.Format(baseGraphApiCall + "?appsecret_proof={" + (args.Count() - 1) + "}", args);
                    //prefix with Graph API Version
                    return string.Format("v2.1/{0}", _graphApiCall);
                }
                else
                    throw new Exception("GraphAPICall requires at least one string parameter that contains the appsecret_proof value.");
            }
            else
                return string.Empty;
        }
    }
}