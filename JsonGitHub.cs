using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpdataApp
{
    public class JsonGitHub
    {
        public const string URL = "https://api.github.com/repos/xiaoxinpro/BleTestTool/releases/latest";

        public VersionInfo versionInfo = new VersionInfo();

        public JsonGitHub(JObject jo)
        {
            try
            {
                versionInfo.Name = jo["name"].ToString();
                versionInfo.Version = new Version(jo["tag_name"].ToString().ToUpper().Replace('V', '0'));
                versionInfo.Created_at = Convert.ToDateTime(jo["created_at"].ToString());
                versionInfo.Published_at = Convert.ToDateTime(jo["published_at"].ToString());
                versionInfo.Prerelease = Convert.ToBoolean(jo["prerelease"].ToString());
                versionInfo.ArrAssets = new Assets[jo["assets"].Count()];
                int i = 0;
                foreach (var item in jo["assets"])
                {
                    versionInfo.ArrAssets[i] = new Assets();
                    versionInfo.ArrAssets[i].Name = item["name"].ToString();
                    versionInfo.ArrAssets[i].Size = Convert.ToInt64(item["size"].ToString());
                    versionInfo.ArrAssets[i].Download_count = Convert.ToInt64(item["download_count"].ToString());
                    versionInfo.ArrAssets[i].Browser_download_url = item["browser_download_url"].ToString();
                    i++;
                }
                versionInfo.Body = jo["body"].ToString();
            }
            catch (Exception)
            {
                return;
            }

            Console.WriteLine("输出版本信息：");
            Console.WriteLine("版本名称：" + versionInfo.Name);
            Console.WriteLine("版本号：" + versionInfo.Version);
            Console.WriteLine("创建时间：" + versionInfo.Created_at);
            Console.WriteLine("修改时间：" + versionInfo.Published_at);
            Console.WriteLine("是否适合生产：" + versionInfo.Prerelease);
            Console.WriteLine("附件：");
            foreach (var item in versionInfo.ArrAssets)
            {
                Console.WriteLine("  附件名称：" + item.Name);
                Console.WriteLine("  附件大小：" + item.Size);
                Console.WriteLine("  下载次数：" + item.Download_count);
                Console.WriteLine("  下载链接：" + item.Browser_download_url);
            }
            Console.WriteLine("说明：" + versionInfo.Body);
        }

        public JsonGitHub(string strJson):this((JObject)JsonConvert.DeserializeObject(strJson))
        {
            
        }
    }

    public class VersionInfo
    {
        private string name;
        private Version version;
        private DateTime created_at;
        private DateTime published_at;
        private bool prerelease;
        private Assets[] arrAssets;
        private string body;

        public string Name { get => name; set => name = value; }
        public Version Version { get => version; set => version = value; }
        public DateTime Created_at { get => created_at; set => created_at = value; }
        public DateTime Published_at { get => published_at; set => published_at = value; }
        public bool Prerelease { get => prerelease; set => prerelease = value; }
        public Assets[] ArrAssets { get => arrAssets; set => arrAssets = value; }
        public string Body { get => body; set => body = value; }
    }

    public class Assets
    {
        private string name;
        private long size;
        private long download_count;
        private string browser_download_url;

        public string Name { get => name; set => name = value; }
        public long Size { get => size; set => size = value; }
        public long Download_count { get => download_count; set => download_count = value; }
        public string Browser_download_url { get => browser_download_url; set => browser_download_url = value; }
    }
}
