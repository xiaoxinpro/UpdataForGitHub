using Gac;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace UpdataApp
{
    public partial class frmMain : Form
    {
        private DownLoadFile downLoadFile;

        private Thread threadGetUpdataInfo;

        private CmdType cmdData;

        public frmMain(string[] args)
        {
            InitializeComponent();
            InitCmdData();
            Console.WriteLine("命令行参数：");
            foreach (string item in args)
            {
                Console.WriteLine("  " + item);
                string[] arrStr = item.Trim().Split('=');
                if (arrStr.Length == 2)
                {
                    InitCmdData(arrStr[0], arrStr[1]);
                }
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            InitDownLoadFile();
            ReflashGitHubInfo();
        }

        private void InitCmdData(string strName = null, string strData = null)
        {
            if (strName == null)
            {
                cmdData.Username = cmdData.Username == null ? cmdData.Username : "";
                cmdData.Project = cmdData.Project == null ? cmdData.Project : "";
                cmdData.Title = cmdData.Title == null ? cmdData.Title : "";
                cmdData.Version = cmdData.Version == null ? cmdData.Version : Version.Parse("0.0.0.1");
                cmdData.Run = cmdData.Run == null ? cmdData.Run : "";
            }
            else
            {
                switch (strName)
                {
                    case "username":
                    case "u":
                        cmdData.Username = strData.Trim();
                        break;
                    case "project":
                    case "p":
                        cmdData.Project = strData.Trim();
                        break;
                    case "title":
                    case "t":
                        cmdData.Title = strData.Trim();
                        break;
                    case "version":
                    case "v":
                        cmdData.Version = System.Version.Parse(strData.Trim());
                        break;
                    case "run":
                    case "r":
                        cmdData.Run = strData.Trim();
                        break;
                    case "auto_run":
                    case "ar":
                        cmdData.IsAutoRun = Convert.ToBoolean(strData.Trim());
                        break;
                    default:
                        break;
                }
            }
        }

        private void StartGetUpdataInfoThread()
        {
            threadGetUpdataInfo = new Thread(new ThreadStart(GetGitHubInfo));
            threadGetUpdataInfo.IsBackground = true;
            threadGetUpdataInfo.Start();
        }

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

            this.Invoke((MethodInvoker)delegate ()
            {
                txtUpdataInfo.Text = swInfo.ToString();
                labelInfo.Text = "获取更新完成";
                btnUpdata.Enabled = true;
            });
        }

        private void InitDownLoadFile()
        {
            downLoadFile = new DownLoadFile();
            downLoadFile.doSendMsg += SendMsgHander;
        }

        private string strDownFilePath;
        private void AddDownloadFile(string name, string url)
        {
            string dir = System.AppDomain.CurrentDomain.BaseDirectory;
            strDownFilePath = dir + name;
            downLoadFile.AddDown(url, dir, 0, name);
            Console.WriteLine("准备下载文件：" + name);
        }

        private void SendMsgHander(DownMsg msg)
        {
            switch (msg.Tag)
            {
                case DownStatus.Start:
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        Console.WriteLine("开始下载：" + DateTime.Now.ToString());
                        labelInfo.Text = "正在连接服务器";
                    });
                    break;
                case DownStatus.GetLength:
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        Console.WriteLine("连接成功：" + msg.LengthInfo);
                        labelInfo.Text = "连接成功";
                    });
                    break;
                case DownStatus.DownLoad:
                    this.Invoke(new MethodInvoker(() =>
                    {
                        this.Invoke((MethodInvoker)delegate ()
                        {
                            if (msg.Tag == DownStatus.DownLoad)
                            {
                                Console.Write("下载中：");
                                labelInfo.Text = "下载中" + msg.Progress.ToString() + "%";
                                proUpdata.Value = Convert.ToInt32(msg.Progress);
                            }
                            else
                            {
                                Console.Write("下载完成：");
                                proUpdata.Value = proUpdata.Maximum;
                            }
                            Console.WriteLine(msg.SizeInfo + "\t" + msg.Progress.ToString() + "%\t" + msg.SpeedInfo + "\t" + msg.SurplusInfo);
                            Application.DoEvents();
                        });
                    }));
                    break;
                case DownStatus.End:
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        Console.WriteLine("下载完成！！！");
                        labelInfo.Text = "下载完成";
                        proUpdata.Value = proUpdata.Maximum;
                        //System.Diagnostics.Process.Start(strDownFilePath);
                    });
                    //开始更新
                    if (File.Exists(strDownFilePath))
                    {
                        System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
                        watch.Start();
                        try
                        {
                            //解压缩文件
                            //ZipFile.ExtractToDirectory(strDownFilePath, System.AppDomain.CurrentDomain.BaseDirectory);
                            ZipHelper.UnZip(strDownFilePath, System.AppDomain.CurrentDomain.BaseDirectory, "", true);

                            //删除压缩包
                            File.Delete(strDownFilePath);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("更新压缩包操作错误：" + ex.Message);
                        }
                        watch.Stop();
                        TimeSpan timespan = watch.Elapsed;
                        Console.WriteLine("解压缩执行时间：{0}(毫秒)", timespan.TotalMilliseconds);  //总毫秒数
                        this.Invoke((MethodInvoker)delegate ()
                        {
                            Console.WriteLine("更新完成");
                            labelInfo.Text = "更新完成";
                            proUpdata.Value = proUpdata.Maximum;
                            if (MessageBox.Show("是否运行更新后的程序？","更新完成",MessageBoxButtons.YesNo,MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "BleTestTool.exe");
                            }
                            Application.Exit();
                        });
                    }
                    break;
                case DownStatus.Error:
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        Console.WriteLine("下载失败：" + msg.ErrMessage);
                        labelInfo.Text = "下载失败：" + msg.ErrMessage;
                    });
                    break;
                default:
                    break;
            }
        }

        private void ReflashGitHubInfo()
        {
            labelInfo.Text = "获取更新中";
            proUpdata.Value = 0;
            txtUpdataInfo.Clear();
            btnUpdata.Enabled = false;
            StartGetUpdataInfoThread();
        }

        private void btnReflash_Click(object sender, EventArgs e)
        {
            ReflashGitHubInfo();
        }

        private void btnUpdata_Click(object sender, EventArgs e)
        {
            btnUpdata.Enabled = false;
            downLoadFile.StartDown();
        }
    }

    public struct CmdType
    {
        public string Username;
        public string Project;
        public string Title;
        public Version Version;
        public string Run;
        public bool IsAutoRun;
    }
}
