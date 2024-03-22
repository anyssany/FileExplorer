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
        private ManualResetEvent pauseEvent = new ManualResetEvent(true); // Создаем ManualResetEvent и инициализируем его в сигнальном состоянии (true)
        private Stopwatch stopwatch = new Stopwatch();

        public Form1()
        {
            InitializeComponent();

            //  Заполняем текстовые поля сохраненными критериями
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
            if (searchThread != null) // Если поток уже существует, т.е. был остановлен по кнопке Стоп
            {
                stopwatch.Start(); // Начинем отсчет времени
                pauseEvent.Set(); // Снимаем блокировку с потока
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
                searchThread.Start(); // Начинаем отсчет времени
                UpdateUI(false);
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            stopwatch.Stop(); // Останавливаем отсчет времени
            pauseEvent.Reset(); // Блокируем поток
            UpdateUI(true);
        }

        private void SearchFiles()
        {
            try
            {
                SearchDirectory(startDirectory);

                stopwatch.Stop();

                UpdateStatus($"Поиск закончен. Нашлось {filesFound.Count} файлов за {stopwatch.Elapsed.TotalSeconds:F2} секунд");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Ошибка: {ex.Message}");
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
            UpdateStatus($"Поиск в: {directory}");
            try
            {                
                foreach (string file in Directory.GetFiles(directory)) // Обход всех директорий в исходной
                {
                    pauseEvent.WaitOne(); // Ждем разблокировки потока

                    if (Regex.IsMatch(Path.GetFileName(file), filePattern)) // Если файл соответствует маске
                    {
                        filesFound.Add(file); // Добавляем файл в список
                        AddFileToTreeView(file); // Добавляем файл в дерево
                    }
                }

                foreach (string subDir in Directory.GetDirectories(directory)) // То же самое, но для дочерних директорий исходной
                {
                    SearchDirectory(subDir);
                }
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        private void AddFileToTreeView(string filePath) // Построение дерева
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

        private void UpdateStatus(string status) // ОБновление информации о ходе поиска
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(UpdateStatus), status);
                return;
            }

            statusLabel.Text = status;            
        }

        private void UpdateUI(bool flag) // Изменение видимости кнопок по ходу поиска
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool>(UpdateUI), flag);
                return;
            }

            startButton.Enabled = flag;
            stopButton.Enabled = !flag;
        }

        private static string GetSetting(string key) // Получение сохраненных пользовательских критериев
        {
            return ConfigurationManager.AppSettings[key];
        }

        private static void SetSetting(string key, string value) // Сохранение пользовательских критериев
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings.Remove(key);
            configuration.AppSettings.Settings.Add(key, value);
            configuration.Save(ConfigurationSaveMode.Full, true);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
