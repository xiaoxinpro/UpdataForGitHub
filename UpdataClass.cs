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
        #endregion

        #region 初始化
        public string Username { get; set; }
        public string Project { get; set; }
        public string Title { get; set; }
        public string Run { get; set; }
        public Version Version { get; set; }
        public Boolean IsAutoRun { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public UpdataClass()
        {
            Username = "";
            Project = "";
            Title = "";
            Run = "";
            Version = Version.Parse("0.0.0.1");
            IsAutoRun = false;
            InitDownLoadFile();
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
        private void GetGitHubInfo()
        {
            string strCode = HttpHelper.Send(JsonGitHub.URL);

            JsonGitHub jsonGitHub = new JsonGitHub(strCode);

            StringWriter swInfo = new StringWriter();

            swInfo.WriteLine("项目名：" + jsonGitHub.versionInfo.Name);
            swInfo.WriteLine("版本号：" + jsonGitHub.versionInfo.Version.ToString());
            swInfo.WriteLine("强制性：" + jsonGitHub.versionInfo.Prerelease.ToString());
            swInfo.WriteLine("日期：" + jsonGitHub.versionInfo.Published_at.ToString());

            foreach (Assets item in jsonGitHub.versionInfo.ArrAssets)
            {
                if (item.Name == "updata.zip")
                {
                    swInfo.WriteLine("文件名：" + item.Name);
                    swInfo.WriteLine("下载数：" + item.Download_count);
                    swInfo.WriteLine("大小：" + item.Size);
                    //下载
                    AddDownloadFile(item.Name, item.Browser_download_url);
                }
            }
            swInfo.WriteLine("更新记录：\r\n" + jsonGitHub.versionInfo.Body);

            ActiveUpdataInfo(EnumUpdataStatus.Done, swInfo.ToString());

            //this.Invoke((MethodInvoker)delegate ()
            //{
            //    txtUpdataInfo.Text = swInfo.ToString();
            //    labelInfo.Text = "获取更新完成";
            //    btnUpdata.Enabled = true;
            //});
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
                    ActiveUpdataInfo(EnumUpdataStatus.DownloadFile, "正在连接服务器");
                    //this.Invoke((MethodInvoker)delegate ()
                    //{
                    //    Console.WriteLine("开始下载：" + DateTime.Now.ToString());
                    //    labelInfo.Text = "正在连接服务器";
                    //});
                    break;
                case DownStatus.GetLength:
                    ActiveUpdataInfo(EnumUpdataStatus.DownloadFile, "正在连接服务器");
                    //this.Invoke((MethodInvoker)delegate ()
                    //{
                    //    Console.WriteLine("连接成功：" + msg.LengthInfo);
                    //    labelInfo.Text = "连接成功";
                    //});
                    break;
                case DownStatus.DownLoad:
                    if (msg.Tag == DownStatus.DownLoad)
                    {
                        ActiveUpdataInfo(EnumUpdataStatus.DownloadFile, "下载中" + msg.Progress.ToString() + "%");
                    }
                    else
                    {
                        ActiveUpdataInfo(EnumUpdataStatus.DownloadFile, "下载完成！");
                    }
                    //this.Invoke(new MethodInvoker(() =>
                    //{
                    //    this.Invoke((MethodInvoker)delegate ()
                    //    {
                    //        if (msg.Tag == DownStatus.DownLoad)
                    //        {
                    //            Console.Write("下载中：");
                    //            labelInfo.Text = "下载中" + msg.Progress.ToString() + "%";
                    //            proUpdata.Value = Convert.ToInt32(msg.Progress);
                    //        }
                    //        else
                    //        {
                    //            Console.Write("下载完成：");
                    //            proUpdata.Value = proUpdata.Maximum;
                    //        }
                    //        Console.WriteLine(msg.SizeInfo + "\t" + msg.Progress.ToString() + "%\t" + msg.SpeedInfo + "\t" + msg.SurplusInfo);
                    //        Application.DoEvents();
                    //    });
                    //}));
                    break;
                case DownStatus.End:
                    ActiveUpdataInfo(EnumUpdataStatus.DownloadFile, "下载完成！");
                    //this.Invoke((MethodInvoker)delegate ()
                    //{
                    //    Console.WriteLine("下载完成！！！");
                    //    labelInfo.Text = "下载完成";
                    //    proUpdata.Value = proUpdata.Maximum;
                    //    //System.Diagnostics.Process.Start(strDownFilePath);
                    //});
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

                        ActiveUpdataInfo(EnumUpdataStatus.Updata, "更新完成");
                        //this.Invoke((MethodInvoker)delegate ()
                        //{
                        //    Console.WriteLine("更新完成");
                        //    labelInfo.Text = "更新完成";
                        //    proUpdata.Value = proUpdata.Maximum;
                        //    if (MessageBox.Show("是否运行更新后的程序？", "更新完成", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        //    {
                        //        Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "BleTestTool.exe");
                        //    }
                        //    Application.Exit();
                        //});
                    }
                    break;
                case DownStatus.Error:
                    ActiveUpdataInfo(EnumUpdataStatus.Error, "下载失败：" + msg.ErrMessage);
                    //this.Invoke((MethodInvoker)delegate ()
                    //{
                    //    Console.WriteLine("下载失败：" + msg.ErrMessage);
                    //    labelInfo.Text = "下载失败：" + msg.ErrMessage;
                    //});
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region 公共函数
        /// <summary>
        /// 获取更新信息
        /// </summary>
        public void Get()
        {
            Task.Factory.StartNew(() =>
            {
                GetGitHubInfo();
            });
        }
        
        /// <summary>
        /// 开始运行更新程序
        /// </summary>
        /// <param name="e"></param>
        public void Start()
        {
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
