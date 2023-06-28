using Microsoft.Extensions.Options;
using SyncClipboard.Core.Interfaces;
using SyncClipboard.Core.Options;
using System.Text.Json;

namespace SyncClipboard.Core.Commons
{
    public class UserConfig
    {
        public event Action? ConfigChanged;

        public const string CONFIG_FILE = "SyncClipboard.json";

        public class Configuration
        {
            public class CProgram
            {
                public int IntervalTime { get; set; } = 3;
                public int RetryTimes { get; set; } = 3;
                public int TimeOut { get; set; } = 10;
                public string Proxy { get; set; } = "";
                public bool DeleteTempFilesOnStartUp { get; set; } = false;
                public int LogRemainDays { get; set; } = 30;
            }

            public class CSyncService
            {
                public string RemoteURL { get; set; } = "";
                public string UserName { get; set; } = "";
                public string Password { get; set; } = "";
                public bool PullSwitchOn { get; set; } = false;
                public bool PushSwitchOn { get; set; } = false;
                public bool DeletePreviousFilesOnPush { get; set; } = true;
                public bool EasyCopyImageSwitchOn { get; set; } = false;
                public int MaxFileByte { get; set; } = 1024 * 1024 * 20;  // 10MB
            }

            public class CCommandService
            {
                public bool SwitchOn { get; set; } = false;
                public int Shutdowntime { get; set; } = 30;
            }

            public class CClipboardService
            {
                public bool ConvertSwitchOn { get; set; } = false;
            }

            public class CServerService
            {
                public bool SwitchOn { get; set; } = false;
                public short Port { get; set; } = 5033;
                public string UserName { get; set; } = "admin";
                public string Password { get; set; } = "admin";
            }

            public CSyncService SyncService { get; set; } = new CSyncService();
            public CCommandService CommandService { get; set; } = new CCommandService();
            public CClipboardService ClipboardService { get; set; } = new CClipboardService();
            public CProgram Program { get; set; } = new CProgram();
            public CServerService ServerService { get; set; } = new CServerService();
        }

        public Configuration Config = new();

        private readonly ILogger _logger;
        private readonly string _path;

        public UserConfig(IOptions<UserConfigOption> option, ILogger logger)
        {
            _logger = logger;
            _path = option.Value.Path ?? throw new ArgumentNullException(nameof(option.Value.Path), "�����ļ�·��Ϊnull");
        }

        public void Save()
        {
            var configStr = JsonSerializer.Serialize(Config, new JsonSerializerOptions { WriteIndented = true });
            _logger.Write("Save user config");
            File.WriteAllText(_path, configStr);
            ConfigChanged?.Invoke();
        }

        public void Load()
        {
            string text;
            try
            {
                text = File.ReadAllText(_path);
            }
            catch (FileNotFoundException)
            {
                WriteDefaultConfigFile();
                return;
            }

            try
            {
                var config = JsonSerializer.Deserialize<Configuration>(text);
                if (config is null)
                {
                    WriteDefaultConfigFile();
                }
                else
                {
                    Config = config;
                }
                ConfigChanged?.Invoke();
            }
            catch
            {
                WriteDefaultConfigFile();
            }
        }

        private void WriteDefaultConfigFile()
        {
            _logger.Write("Write new default file.");
            Config = new Configuration();
            Save();
        }
    }
}