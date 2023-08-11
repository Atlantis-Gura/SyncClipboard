﻿using Microsoft.Extensions.DependencyInjection;
using SyncClipboard.Control;
using SyncClipboard.Core;
using SyncClipboard.Core.Commons;
using SyncClipboard.Core.Interfaces;
using System;
using System.Windows.Forms;

namespace SyncClipboard
{
    public static class Global
    {
        private static ContextMenu Menu;
        private static ProgramWorkflow ProgramWorkflow;

        internal static IHttp Http;
        internal static ConfigManager ConfigManager;
        internal static ILogger Logger;

        public static void StartUp()
        {
            var services = ConfigurateServices();

            Http = services.GetRequiredService<IHttp>();
            ConfigManager = services.GetRequiredService<ConfigManager>();
            Logger = services.GetRequiredService<ILogger>();

            ProgramWorkflow = new ProgramWorkflow(services);
            ProgramWorkflow.Run();
            StartUpUI();
        }

        public static IServiceProvider ConfigurateServices()
        {
            var services = new ServiceCollection();
            ProgramWorkflow.ConfigCommonService(services);

            var notifyer = new Notifyer();
            Menu = new ContextMenu(notifyer);
            services.AddSingleton<IContextMenu>(Menu);
            services.AddSingleton<ITrayIcon>(notifyer);
            services.AddSingleton<IMainWindow, SettingsForm>();

            return services.BuildServiceProvider();
        }

        internal static void EndUp()
        {
            ProgramWorkflow.Stop();
        }

        private static void StartUpUI()
        {
            MenuItem[] menuItems =
            {
                //new ToggleMenuItem("开机启动", StartUpHelper.Status(), StartUpHelper.Set),
                //new MenuItem("从Nextcloud登录", Nextcloud.LogWithNextcloud),
                //new MenuItem("检查更新", Module.UpdateChecker.Check),
                new MenuItem("退出", Application.Exit)
            };
            Menu.AddMenuItemGroup(menuItems);
        }
    }
}
