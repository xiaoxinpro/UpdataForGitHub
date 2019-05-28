using Gac;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdataApp
{
    public class UpdataClass
    {
        #region 静态字段
        private DownLoadFile downLoadFile;
        private Boolean isRunGet = false;
        private Boolean isRunStart = false;
        #endregion

        #region 初始化
        public string Username { get; set; }
        public string Project { get; set; }
        public string Title { get; set; }
        public string Run { get; set; }
        public Version Version { get; set; }
        public Boolean IsAutoRun { get; set; }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public void InitData()
        {
            Username = "xiaoxinpro";
            Project = "BleTestTool";
            Title = "软件升级工具";
            Run = "";
            Version = Version.Parse("0.0.0.1");
            IsAutoRun = false;
            InitDownLoadFile();
        }

        /// <summary>
        /// 获取URL
        /// </summary>
        private string GetUrl()
        {
            return JsonGitHub.URL.Replace("Username", Username.Trim()).Replace("Project", Project.Trim());
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public UpdataClass()
        {
            InitData();
        }

        public UpdataClass(DelegateUpdataInfo e):this()
        {
            EventUpdataInfo += e;
        }
        #endregion

        #region 事件函数
        /// <summary>
        /// 升级信息事件
        /// </summary>
        /// <param name="sender">UpdataClass对象</param>
        /// <param name="status">状态枚举类型</param>
        /// <param name="msg">消息字符串</param>
        public delegate void DelegateUpdataInfo(object sender, EnumUpdataStatus status, string msg);

        public event DelegateUpdataInfo EventUpdataInfo;

        private void ActiveUpdataInfo(EnumUpdataStatus status, string msg = "")
        {
            EventUpdataInfo?.Invoke(this, status, msg);
        }
        #endregion

        #region 内部函数
        /// <summary>
        /// 获取GitHub信息
        /// </summary>
        private void GetGitHubInfo(string fileName = "updata.zip")
        {
            string strCode = HttpHelper.Send(GetUrl());

            JsonGitHub jsonGitHub = new JsonGitHub(strCode);

            StringWriter swInfo = new StringWriter();

            try
            {
                swInfo.WriteLine("项目名：" + jsonGitHub.versionInfo.Name);
                swInfo.WriteLine("版本号：" + jsonGitHub.versionInfo.Version.ToString());
                swInfo.WriteLine("强制性：" + jsonGitHub.versionInfo.Prerelease.ToString());
                swInfo.WriteLine("日期：" + jsonGitHub.versionInfo.Published_at.ToString());
            }
            catch (Exception err)
            {
                ActiveUpdataInfo(EnumUpdataStatus.Error, "获取更新信息错误");
            }

            foreach (Assets item in jsonGitHub.versionInfo.ArrAssets)
            {
                if (item.Name == fileName)
                {
                    swInfo.WriteLine("文件名：" + item.Name);
                    swInfo.WriteLine("下载数：" + item.Download_count);
                    swInfo.WriteLine("大小：" + item.Size);
                    //下载
                    AddDownloadFile(item.Name, item.Browser_download_url);
                }
            }
            swInfo.WriteLine("更新记录：\r\n" + jsonGitHub.versionInfo.Body);

            ActiveUpdataInfo(EnumUpdataStatus.GetJson, swInfo.ToString());

            isRunGet = false;
        }

        /// <summary>
        /// 初始化下载
        /// </summary>
        private void InitDownLoadFile()
        {
            downLoadFile = new DownLoadFile();
            downLoadFile.doSendMsg += SendMsgHander;
        }

        /// <summary>
        /// 添加下载文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="url">下载地址</param>
        private string strDownFilePath;
        private void AddDownloadFile(string name, string url)
        {
            string dir = System.AppDomain.CurrentDomain.BaseDirectory;
            strDownFilePath = dir + name;
            downLoadFile.AddDown(url, dir, 0, name);
            Console.WriteLine("准备下载文件：" + name);
        }

        /// <summary>
        /// 下载文件回调
        /// </summary>
        /// <param name="msg">消息字符串</param>
        private void SendMsgHander(DownMsg msg)
        {
            switch (msg.Tag)
            {
                case DownStatus.Start:
                    Console.WriteLine("开始下载：" + DateTime.Now.ToString());
                    ActiveUpdataInfo(EnumUpdataStatus.DownloadFile, "正在连接服务器");
                    break;
                case DownStatus.GetLength:
                    Console.WriteLine("连接成功：" + msg.LengthInfo);
                    ActiveUpdataInfo(EnumUpdataStatus.DownloadFile, "连接成功");
                    break;
                case DownStatus.DownLoad:
                    if (msg.Tag == DownStatus.DownLoad)
                    {
                        Console.Write("下载中：");
                        ActiveUpdataInfo(EnumUpdataStatus.DownloadFile, "下载中" + msg.Progress.ToString() + "%");
                        //proUpdata.Value = Convert.ToInt32(msg.Progress);
                    }
                    else
                    {
                        Console.Write("下载完成：");
                        ActiveUpdataInfo(EnumUpdataStatus.DownloadFile, "下载完成！");
                        //proUpdata.Value = proUpdata.Maximum;
                        isRunStart = false;
                    }
                    Console.WriteLine(msg.SizeInfo + "\t" + msg.Progress.ToString() + "%\t" + msg.SpeedInfo + "\t" + msg.SurplusInfo);
                    break;
                case DownStatus.End:
                    Console.WriteLine("下载完成！！！");
                    ActiveUpdataInfo(EnumUpdataStatus.DownloadFile, "下载完成！");

                    //开始更新
                    if (File.Exists(strDownFilePath))
                    {
                        System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
                        watch.Start();
                        try
                        {
                            //解压缩文件
                            ActiveUpdataInfo(EnumUpdataStatus.Updata, "解压缩文件中");

                            //ZipFile.ExtractToDirectory(strDownFilePath, System.AppDomain.CurrentDomain.BaseDirectory);
                            ZipHelper.UnZip(strDownFilePath, System.AppDomain.CurrentDomain.BaseDirectory, "", true);

                            //删除压缩包
                            File.Delete(strDownFilePath);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("更新压缩包操作错误：" + ex.Message);
                            ActiveUpdataInfo(EnumUpdataStatus.Error, "更新压缩包操作错误：" + ex.Message);
                        }
                        watch.Stop();
                        TimeSpan timespan = watch.Elapsed;
                        Console.WriteLine("解压缩执行时间：{0}(毫秒)", timespan.TotalMilliseconds);  //总毫秒数

                        Console.WriteLine("更新完成");
                        ActiveUpdataInfo(EnumUpdataStatus.Done, "更新完成");
                        //proUpdata.Value = proUpdata.Maximum;
                        isRunStart = false;
                    }
                    break;
                case DownStatus.Error:
                    ActiveUpdataInfo(EnumUpdataStatus.Error, "下载失败：" + msg.ErrMessage);
                    Console.WriteLine("下载失败：" + msg.ErrMessage);
                    isRunStart = false;
                    break;
                default:
                    isRunStart = false;
                    break;
            }
        }
        #endregion

        #region 公共函数
        /// <summary>
        /// 获取更新信息
        /// </summary>
        /// <param name="fileName">需要跟新的文件名.zip</param>
        public void Get(string fileName = "updata.zip")
        {
            if (isRunGet)
            {
                return;
            }
            isRunGet = true;
            Task.Factory.StartNew(() =>
            {
                GetGitHubInfo(fileName);
            });
        }
        
        /// <summary>
        /// 开始运行更新程序
        /// </summary>
        /// <param name="e"></param>
        public void Start()
        {
            if (isRunStart)
            {
                return;
            }
            isRunStart = true;
            downLoadFile.StartDown();
        }
        #endregion

    }

    /// <summary>
    /// 升级状态枚举
    /// </summary>
    public enum EnumUpdataStatus
    {
        Start,
        GetJson,
        DownloadFile,
        Updata,
        Done,
        Error,
    }
}
