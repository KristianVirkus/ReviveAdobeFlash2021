using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace CustomFlashViewer
{
    public partial class MainForm : Form
    {
        private string _configFilePath;
        private bool _configLoaded;
        private bool _dontUpdateConfig;
        private string _url;
        private string _windowTitle;
        private int? _windowWidth;
        private int? _windowHeight;
        private int? _windowLeft;
        private int? _windowTop;
        private FormWindowState? _windowState;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var timer = new Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 50;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            ((Timer)sender).Stop();
            this._configFilePath = Path.Combine(Path.GetDirectoryName(typeof(MainForm).Assembly.Location), Path.GetFileNameWithoutExtension(typeof(MainForm).Assembly.Location) + ".cfg");
            this.loadConfig();
            this._configLoaded = true;

            if (this._url == null || string.IsNullOrEmpty(this._url.Trim()))
            {
                MessageBox.Show(Resources.UrlMissing);
                Environment.Exit(1);
            }

            this.LoadingLabel.Text = "'" + this._url + "'" + Resources.UrlNotLoaded;

            this.Text = this._windowTitle ?? Resources.DefaultWindowTitle;

            this.ClientSize = new Size(this._windowWidth ?? this.ClientSize.Width, this._windowHeight ?? this.ClientSize.Height);
            if (this._windowLeft != null) this.Left = this._windowLeft.Value;
            if (this._windowTop != null) this.Top = this._windowTop.Value;
            this.WindowState = this._windowState ?? FormWindowState.Normal;

            this.Flash.OnReadyStateChange += new AxShockwaveFlashObjects._IShockwaveFlashEvents_OnReadyStateChangeEventHandler(Flash_OnReadyStateChange);
            this.Flash.Movie = this._url;
            this.Flash.Play();
            this.Flash.Focus();
        }

        private void Flash_OnReadyStateChange(object sender, AxShockwaveFlashObjects._IShockwaveFlashEvents_OnReadyStateChangeEvent e)
        {
            // MessageBox.Show(e.newState.ToString());
            this.LoadingLabel.Visible = this.Flash.TotalFrames == 0;
        }

        private void loadConfig()
        {
            if (!File.Exists(this._configFilePath))
            {
                MessageBox.Show(Resources.ConfigFileMissing);
                Environment.Exit(1);
            }

            try
            {
                using (var fileReader = new StreamReader(this._configFilePath))
                {
                    string line;
                    while ((line = fileReader.ReadLine()) != null)
                    {
                        if (line.Equals("DontUpdateConfig", StringComparison.OrdinalIgnoreCase))
                            this._dontUpdateConfig = true;
                        else if (line.StartsWith("Url=", StringComparison.OrdinalIgnoreCase))
                            this._url = line.Substring("Url=".Length);
                        else if (line.StartsWith("WindowTitle=", StringComparison.OrdinalIgnoreCase))
                            this._windowTitle = line.Substring("WindowTitle=".Length);
                        else if (line.StartsWith("WindowWidth=", StringComparison.OrdinalIgnoreCase))
                            this._windowWidth = int.Parse(line.Substring("WindowWidth=".Length));
                        else if (line.StartsWith("WindowHeight=", StringComparison.OrdinalIgnoreCase))
                            this._windowHeight = int.Parse(line.Substring("WindowHeight=".Length));
                        else if (line.StartsWith("WindowLeft=", StringComparison.OrdinalIgnoreCase))
                            this._windowLeft = int.Parse(line.Substring("WindowLeft=".Length));
                        else if (line.StartsWith("WindowTop=", StringComparison.OrdinalIgnoreCase))
                            this._windowTop = int.Parse(line.Substring("WindowTop=".Length));
                        else if (line.StartsWith("WindowState=", StringComparison.OrdinalIgnoreCase))
                            this._windowState = (FormWindowState)Enum.Parse(typeof(FormWindowState), line.Substring("WindowState=".Length));
                    }
                }

                this._configLoaded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources.ReadConfigError + ex.Message);
                Environment.Exit(1);
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this._configLoaded && !this._dontUpdateConfig) saveConfig();
        }

        private void saveConfig()
        {
            try
            {
                using (var fileWriter = new StreamWriter(this._configFilePath))
                {
                    if (this._dontUpdateConfig == true)
                        fileWriter.WriteLine("DontUpdateConfig");
                    fileWriter.WriteLine("Url=" + this._url ?? "");
                    if (this._windowTitle != null)
                        fileWriter.WriteLine("WindowTitle=" + this._windowTitle);
                    fileWriter.WriteLine("WindowWidth=" + this.ClientSize.Width);
                    fileWriter.WriteLine("WindowHeight=" + this.ClientSize.Height);
                    fileWriter.WriteLine("WindowLeft=" + this.Left);
                    fileWriter.WriteLine("WindowTop=" + this.Top);
                    fileWriter.WriteLine("WindowState=" + this.WindowState.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources.WriteConfigError + ex.Message);
                Environment.Exit(1);
            }
        }
    }
}
