﻿using SyncClipboard.Core.Interfaces;

namespace SyncClipboard.Desktop;

internal class AppConfig : IAppConfig
{
    public string AppId => Env.AppId;
    public string AppStringId => "SyncClipboard.Desktop";
    public string AppVersion => "0.4.1";
    public string UpdateApiUrl => "https://api.github.com/repos/Jeric-X/SyncClipboard.Desktop/releases/latest";
    public string UpdateUrl => "https://github.com/Jeric-X/SyncClipboard.Desktop/releases/latest";
}
