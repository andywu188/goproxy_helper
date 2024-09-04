using Microsoft.Win32;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace goproxy_helper
{
    public partial class main : Form
    {
        private static string serviceName = "proxyadmin";
        private static string binName = "proxy-admin";
        private static string titleType = "社区版";
        public main()
        {
            InitializeComponent();
            this.Text += " " + titleType;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Service.Exists(serviceName))
            {
                MessageBox.Show("服务已经安装！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            MessageBox.Show(exec(binName, "install", true, false, true) ? "安装成功" : "安装失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private string chromePath()
        {
            object path2;
            path2 = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe", "", null);
            if (path2 != null)
            {
                return path2.ToString();
            }
            return "";
        }
        private string firefoxPath()
        {
            object path2;
            path2 = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\firefox.exe", "", null);
            if (path2 != null)
            {
                return path2.ToString();
            }
            return "";
        }
        private string edgePath()
        {
            object path2;
            path2 = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\msedge.exe", "", null);
            if (path2 != null)
            {
                return path2.ToString();
            }
            return "";
        }
        private string otherPath()
        {
            object path2;
            path2 = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\360se6.exe", "", null);
            if (path2 != null)
            {
                return path2.ToString();
            }
            return "";
        }


        private void main_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.chromePath()) && string.IsNullOrEmpty(this.firefoxPath()) && string.IsNullOrEmpty(this.edgePath()) && string.IsNullOrEmpty(this.otherPath()))
            {
                lblTips.Text = "请先安装最新版chrome、edge或者firefox等现代浏览器，才能正常使用goproxy控制面板。\n如果已安装请手动打开使用：http://127.0.0.1:32080访问";
            }
            else
            {
                lblTips.Text = string.Empty;
            }
            refreshStatus();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!Service.Exists(serviceName))
            {
                MessageBox.Show("服务未安装！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Service.Stop(serviceName);
            //exec(binName, "stop", true, false);
            MessageBox.Show(Service.Start(serviceName) ? "重启成功" : "重启失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void refreshStatus()
        {
            Thread demoThread = new Thread(new ThreadStart(threadMethod));
            demoThread.IsBackground = true;
            demoThread.Start();//启动线程 

        }
        void threadMethod()
        {
            string statusText = "";
            while (true)
            {
                bool isExists = Service.Exists(serviceName);
                if (isExists)
                {
                    if (Service.IsRunning(serviceName))
                    {
                        status.ForeColor = Color.LimeGreen;
                        statusText = "运行中";
                    }
                    else
                    {
                        status.ForeColor = Color.Red;
                        statusText = "已停止";
                    }
                }
                else
                {
                    status.ForeColor = Color.Red;
                    statusText = "未安装";
                }
                Action<String> AsyncUIDelegate = delegate (string n) { status.Text = n; };
                status.Invoke(AsyncUIDelegate, new object[] { statusText });
                Thread.Sleep(1000);
            }
        }
        private bool exec(string cmd, string args, bool CreateNoWindow, bool UseShellExecute, bool wait)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = cmd;
            p.StartInfo.CreateNoWindow = CreateNoWindow;
            p.StartInfo.UseShellExecute = UseShellExecute;
            p.StartInfo.Arguments = args;
            try
            {
                bool ok = p.Start();
                if (wait)
                {
                    p.WaitForExit();
                }
                return ok;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!Service.Exists(serviceName))
            {
                MessageBox.Show("服务未安装！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            MessageBox.Show(exec(binName, "uninstall", true, false, true) ? "卸载成功" : "卸载失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            /**
            if (!Service.Exists(serviceName))
            {
                MessageBox.Show("服务未安装！");
                return;
            }**/
            exec("explorer.exe", "C:\\gpa", true, false, false);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!Service.Exists(serviceName))
            {
                MessageBox.Show("服务未安装！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string chromebinpath = this.chromePath();
            string firefoxbinpath = this.firefoxPath();
            string edgebinpath = this.edgePath();
            string otherbinpath = this.otherPath();
            string browser = "";

            if (chromebinpath != "")
            {
                browser = "chrome.exe";
            }
            else if (firefoxbinpath != "")
            {
                browser = "firefox.exe";
            }
            else if (edgebinpath != "")
            {
                browser = "msedge.exe";
            }
            else if (otherbinpath != "")
            {
                browser = "360se6.exe";
            }
            else
            {
                MessageBox.Show("请先安装最新版 Google Chrome 、Microsoft Edge 或者 Firefox 等现代浏览器！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            System.Diagnostics.Process.Start(browser, "http://127.0.0.1:32080");
            //exec("cmd.exe", "start " + browser + " http://127.0.0.1:32080", true, true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("太难了，最近压力很大", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (!Service.Exists(serviceName))
            {
                MessageBox.Show("服务未安装！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!Service.IsRunning(serviceName))
            {
                MessageBox.Show("服务未启动！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            MessageBox.Show(Service.Stop(serviceName) ? "停止成功" : "停止失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (!Service.Exists(serviceName))
            {
                MessageBox.Show("服务未安装！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (Service.IsRunning(serviceName))
            {
                MessageBox.Show("服务运行中！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            MessageBox.Show(Service.Start(serviceName) ? "启动成功" : "启动失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://127.0.0.1:32080");
        }
    }
}
