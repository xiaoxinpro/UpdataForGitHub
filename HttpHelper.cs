using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace UpdataApp
{
    public class HttpHelper
    {
        public static string Send(string sUrl, string sParam = "", string method = "get")
        {
            //定义安全传输协议（TLS1.2=3702, TLS1.1=765, TLS1.0=192, SSL3=48）
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;//TLS1.2=3702

            string result = "";
            HttpWebRequest req = WebRequest.Create(sUrl) as HttpWebRequest;
            HttpWebResponse res = null;
            if (req != null)
            {
                req.Method = method;
                req.ContentType = @"application/octet-stream";
                req.UserAgent = @"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36";
                byte[] postData = Encoding.GetEncoding("UTF-8").GetBytes(sParam);
                if (postData.Length > 0)
                {
                    req.ContentLength = postData.Length;
                    req.Timeout = 15000;
                    Stream outputStream = req.GetRequestStream();
                    outputStream.Write(postData, 0, postData.Length);
                    outputStream.Flush();
                    outputStream.Close();
                    try
                    {
                        res = (HttpWebResponse)req.GetResponse();
                        System.IO.Stream InputStream = res.GetResponseStream();
                        Encoding encoding = Encoding.GetEncoding("UTF-8");
                        StreamReader sr = new StreamReader(InputStream, encoding);
                        result = sr.ReadToEnd();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return result;
                    }
                }
                else
                {
                    try
                    {
                        res = (HttpWebResponse)req.GetResponse();
                        System.IO.Stream InputStream = res.GetResponseStream();
                        Encoding encoding = Encoding.GetEncoding("UTF-8");
                        StreamReader sr = new StreamReader(InputStream, encoding);
                        result = sr.ReadToEnd();
                        sr.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return result;
                    }
                }
            }
            return result;
        }
        
    }
}
