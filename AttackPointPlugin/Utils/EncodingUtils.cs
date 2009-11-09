using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace GK.Utils
{
    public static class EncodingUtils
    {
        private static Encoding latin1Encoding = Encoding.GetEncoding("iso-8859-1");

        public static string ConvertForLatin1Html(string s) {
            var sb = new StringBuilder();

            // AttackPoint web site uses ISO-8859-1 encoding to display pages.
            // So, I HTML-encode all characters whose codepoint is greater than 255.
            foreach (var c in s.ToCharArray()) {
                if (c > 255) {
                    sb.AppendFormat("&#{0};", (int)c);
                }
                else {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        public static string UrlEncodeForLatin1(string s) {
            return HttpUtility.UrlEncode(s, latin1Encoding);
        }

        public static string UrlDecodeForLatin1(string s) {
            return HttpUtility.UrlDecode(s, latin1Encoding);
        }

        public static string UrlEncode(string s) {
            return HttpUtility.UrlEncode(s, Encoding.UTF8);
        }
    }
}
