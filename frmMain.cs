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
        private UpdataClass UpdataInfo;

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
            UpdataInfo = new UpdataClass(UpdataInfo_Event);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            ReflashGitHubInfo();
        }

        private void UpdataInfo_Event(object sender, EnumUpdataStatus status, string msg)
        {
            Console.WriteLine(status.ToString() + "\t->\t" + msg);
            this.Invoke(new Action(() =>
            {
                switch (status)
                {
                    case EnumUpdataStatus.Start:
                        break;
                    case EnumUpdataStatus.GetJson:
                        break;
                    case EnumUpdataStatus.DownloadFile:
                        break;
                    case EnumUpdataStatus.Updata:
                        break;
                    case EnumUpdataStatus.Done:
                        txtUpdataInfo.Text = msg;
                        break;
                    case EnumUpdataStatus.Error:
                        break;
                    default:
                        break;
                }
            }));
        }

        private void InitCmdData(string strName = null, string strData = null)
        {
            if (strName == null)
            {
                //UpdataInfo属性复位
            }
            else
            {
                switch (strName)
                {
                    case "username":
                    case "u":
                        UpdataInfo.Username = strData.Trim();
                        break;
                    case "project":
                    case "p":
                        UpdataInfo.Project = strData.Trim();
                        break;
                    case "title":
                    case "t":
                        UpdataInfo.Title = strData.Trim();
                        break;
                    case "version":
                    case "v":
                        UpdataInfo.Version = System.Version.Parse(strData.Trim());
                        break;
                    case "run":
                    case "r":
                        UpdataInfo.Run = strData.Trim();
                        break;
                    case "auto_run":
                    case "ar":
                        UpdataInfo.IsAutoRun = Convert.ToBoolean(strData.Trim());
                        break;
                    default:
                        break;
                }
            }
        }

        private void ReflashGitHubInfo()
        {
            labelInfo.Text = "获取更新中";
            proUpdata.Value = 0;
            txtUpdataInfo.Clear();
            btnUpdata.Enabled = false;
            UpdataInfo.Get();
        }

        private void btnReflash_Click(object sender, EventArgs e)
        {
            ReflashGitHubInfo();
        }

        private void btnUpdata_Click(object sender, EventArgs e)
        {
            btnUpdata.Enabled = false;
            UpdataInfo.Start();
        }
    }
}
