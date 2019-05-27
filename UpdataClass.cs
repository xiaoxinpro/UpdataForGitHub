using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdataApp
{
    public class UpdataClass
    {
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
        }
        #endregion

        #region 事件函数
        public delegate void DelegateUpdataInfo(object sender);

        public event DelegateUpdataInfo EventUpdataInfo;

        private void ActiveUpdataInfo()
        {
            EventUpdataInfo?.Invoke(this);
        }
        #endregion

        #region 公共函数
        public void Start(DelegateUpdataInfo e)
        {
            EventUpdataInfo += e;
            Task.Factory.StartNew(() =>
            {

            });
        }
        #endregion

    }
}
