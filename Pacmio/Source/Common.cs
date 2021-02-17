/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;

namespace Pacmio
{
    public static class Log
    {
        //private static string DebugLogFile => Root.AppPath + "Debug.log";
        //private static string UnknownExchangeFile => Program.ResourcePath + "UnknownExchangeCode.Json";
        //private static string UnknownSecurityTypeFile => Program.ResourcePath + "UnknownSecurityTypeCode.Json";

        public static void SaveLogs()
        {
            /*
            if (File.Exists(UnknownExchangeFile))
                ExchangeInfo.UnknownExchangeCode = Util.DeserializeJsonFile<HashSet<string>>(UnknownExchangeFile);
            else
                ExchangeInfo.UnknownExchangeCode = new HashSet<string>();

            if (File.Exists(UnknownSecurityTypeFile))
                SecurityTypeInfo.UnknownSecurityTypeCode = Util.DeserializeJsonFile<HashSet<string>>(UnknownSecurityTypeFile);
            else
                SecurityTypeInfo.UnknownSecurityTypeCode = new HashSet<string>();
            

            lock (ExchangeInfo.UnknownExchangeCode)
                ExchangeInfo.UnknownExchangeCode.SerializeJsonFile(UnknownExchangeFile);

            lock (SecurityTypeInfo.UnknownSecurityTypeCode)
                SecurityTypeInfo.UnknownSecurityTypeCode.SerializeJsonFile(UnknownSecurityTypeFile);
            */
        }

        internal static List<string> OutputMessages = new List<string>();

        [Conditional("DEBUG")]
        public static void Print(string str)
        {
            Console.WriteLine(str);
            OutputMessages.Add(str);

            while (OutputMessages.Count > 500)
            {
                OutputMessages.RemoveAt(0);
            }
        }
    }

    [DesignerCategory("Code")]
    public class PacmWebClient : WebClient
    {
        public Uri ResponseUri { get; private set; }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            /*
            WebResponse response = base.GetWebResponse(request);
            ResponseUri = response.ResponseUri;*/
            (request as HttpWebRequest).AllowAutoRedirect = true;
            WebResponse response = base.GetWebResponse(request);
            return response;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://www.google.com");// base.GetWebRequest(address) as HttpWebRequest;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-us,en;q=0.5");

            Log.Print(request.RequestUri.AbsoluteUri);

            return request;
        }
    }
}
