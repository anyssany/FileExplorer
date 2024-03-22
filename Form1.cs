using System.Configuration;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace FileExplorer
{
    public partial class Form1 : Form
    {
        private string startDirectory;
        private string filePattern;
        private List<string> filesFound;
        private TreeNodeCollection nodes;
        private Thread searchThread;
        private ManualResetEvent pauseEvent = new ManualResetEvent(true); // ������� ManualResetEvent � �������������� ��� � ���������� ��������� (true)
        private Stopwatch stopwatch = new Stopwatch();

        public Form1()
        {
            InitializeComponent();

            //  ��������� ��������� ���� ������������ ����������
            startDirectoryTextBox.Text = GetSetting("startDirectory");
            filePatternTextBox.Text = GetSetting("filePattern");

            statusLabel.Visible = false;

            UpdateUI(true);
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            statusLabel.Visible = true;
            startDirectoryTextBox.Enabled = false;
            filePatternTextBox.Enabled = false;
            if (searchThread != null) // ���� ����� ��� ����������, �.�. ��� ���������� �� ������ ����
            {
                stopwatch.Start(); // ������� ������ �������
                pauseEvent.Set(); // ������� ���������� � ������
                UpdateUI(false);
            }
            else
            {
                filesTreeView.Nodes.Clear();
                stopwatch.Restart();

                startDirectory = startDirectoryTextBox.Text;
                filePattern = filePatternTextBox.Text;

                SetSetting("startDirectory", startDirectory);
                SetSetting("filePattern", filePattern);

                filesFound = new List<string>();
                searchThread = new Thread(new ThreadStart(SearchFiles));
                searchThread.Start(); // �������� ������ �������
                UpdateUI(false);
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            stopwatch.Stop(); // ������������� ������ �������
            pauseEvent.Reset(); // ��������� �����
            UpdateUI(true);
        }

        private void SearchFiles()
        {
            try
            {
                SearchDirectory(startDirectory);

                stopwatch.Stop();

                UpdateStatus($"����� ��������. ������� {filesFound.Count} ������ �� {stopwatch.Elapsed.TotalSeconds:F2} ������");
            }
            catch (Exception ex)
            {
                UpdateStatus($"������: {ex.Message}");
            }
            finally
            {
                searchThread = null;
                Invoke(new Action(() =>
                {
                    filePatternTextBox.Enabled = true;
                    startDirectoryTextBox.Enabled = true;
                }));
                filesFound.Clear();
                nodes.Clear();
                UpdateUI(true);
            }
        }

        private void SearchDirectory(string directory)
        {
            UpdateStatus($"����� �: {directory}");
            try
            {                
                foreach (string file in Directory.GetFiles(directory)) // ����� ���� ���������� � ��������
                {
                    pauseEvent.WaitOne(); // ���� ������������� ������

                    if (Regex.IsMatch(Path.GetFileName(file), filePattern)) // ���� ���� ������������� �����
                    {
                        filesFound.Add(file); // ��������� ���� � ������
                        AddFileToTreeView(file); // ��������� ���� � ������
                    }
                }

                foreach (string subDir in Directory.GetDirectories(directory)) // �� �� �����, �� ��� �������� ���������� ��������
                {
                    SearchDirectory(subDir);
                }
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        private void AddFileToTreeView(string filePath) // ���������� ������
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AddFileToTreeView), filePath);
                return;
            }
            string[] pathParts = filePath.Split('\\');
            nodes = filesTreeView.Nodes;
            TreeNode lastNode = null;
            foreach (string part in pathParts)
            {
                lastNode = nodes[part] ?? nodes.Add(part, part);
                nodes = lastNode.Nodes;
            }
        }

        private void UpdateStatus(string status) // ���������� ���������� � ���� ������
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(UpdateStatus), status);
                return;
            }

            statusLabel.Text = status;            
        }

        private void UpdateUI(bool flag) // ��������� ��������� ������ �� ���� ������
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool>(UpdateUI), flag);
                return;
            }

            startButton.Enabled = flag;
            stopButton.Enabled = !flag;
        }

        private static string GetSetting(string key) // ��������� ����������� ���������������� ���������
        {
            return ConfigurationManager.AppSettings[key];
        }

        private static void SetSetting(string key, string value) // ���������� ���������������� ���������
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings.Remove(key);
            configuration.AppSettings.Settings.Add(key, value);
            configuration.Save(ConfigurationSaveMode.Full, true);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
