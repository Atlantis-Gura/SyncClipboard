using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using SyncClipboard.Core.Models.UserConfigs;
using SyncClipboard.Core.ViewModels;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SyncClipboard.WinUI3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SyncSettingPage : Page
    {
        private readonly SyncSettingViewModel _viewModel;

        public SyncSettingPage()
        {
            this.InitializeComponent();
            _viewModel = App.Current.Services.GetRequiredService<SyncSettingViewModel>();
            this.DataContext = _viewModel;
        }

        [RelayCommand]
        private async void SetServerAsync()
        {
            _ServerSettingDialog.Password = _viewModel.ServerConfig.Password;
            _ServerSettingDialog.UserName = _viewModel.ServerConfig.Password;
            _ServerSettingDialog.Url = _viewModel.ServerConfig.Port.ToString();
            await _ServerSettingDialog.ShowAsync();
        }

        [RelayCommand]
        private async void SetSyncAsync()
        {
            _SyncSettingDialog.Password = _viewModel.ClientConfig.Password;
            _SyncSettingDialog.UserName = _viewModel.ClientConfig.UserName;
            _SyncSettingDialog.Url = _viewModel.ClientConfig.RemoteURL;
            await _SyncSettingDialog.ShowAsync();
        }

        private void ServerSettingDialog_OkClick(ContentDialog _, ContentDialogButtonClickEventArgs args)
        {
            var res = _viewModel.SetServerConfig(_ServerSettingDialog.Url, _ServerSettingDialog.UserName, _ServerSettingDialog.Password);
            if (string.IsNullOrEmpty(res))
            {
                _ServerSettingDialog.ErrorTip = "";
                return;
            }

            _ServerSettingDialog.ErrorTip = res;
            args.Cancel = true;
            return;
        }

        private void SyncSettingDialog_OkClick(ContentDialog _, ContentDialogButtonClickEventArgs args)
        {
            var res = _viewModel.SetClientConfig(_SyncSettingDialog.Url, _SyncSettingDialog.UserName, _SyncSettingDialog.Password);
            if (string.IsNullOrEmpty(res))
            {
                _SyncSettingDialog.ErrorTip = "";
                return;
            }

            _SyncSettingDialog.ErrorTip = res;
            args.Cancel = true;
            return;
        }

        private string GetPasswordString(string origin, bool? show)
        {
            return show ?? false ? origin : "*********";
        }

        private bool Not(bool value) => !value;

        private string ServerConfigDescription(ServerConfig config, bool? show)
        {
            return
@$"�ˡ��ڣ�{config.Port}
�û�����{config.UserName}
�ܡ��룺{GetPasswordString(config.Password, show)}";
        }

        private string ClientConfigDescription(SyncConfig config, bool? show)
        {
            return
@$"�ء�ַ��{config.RemoteURL}
�û�����{config.UserName}
�ܡ��룺{GetPasswordString(config.Password, show)}";
        }
    }
}
