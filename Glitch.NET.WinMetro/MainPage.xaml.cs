using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Glitch.NET.WinMD;
using Glitch.NET.WinMD.ResponseStructs;


namespace Glitch.NET.WinMetro
{
    partial class MainPage
    {
        GlitchAPI API;
        bool TestsRunning = false;

        public MainPage()
        {
            InitializeComponent();
            API = new GlitchAPI();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TestsRunning)
                return;

            TestsRunning = true;
            var button = (Button)sender;
            button.Content = "Running";

            if(!API.IsSignedIn)
                await API.authCheck("read");

            var PlayerInfo = await API.playersFullInfo(API.PlayerTSID);
            button.Content = string.Format("You logged in as: {0}: with tsid: {1}", PlayerInfo.player_name, PlayerInfo.player_tsid);
            TestsRunning = false;
        }
    }
}
