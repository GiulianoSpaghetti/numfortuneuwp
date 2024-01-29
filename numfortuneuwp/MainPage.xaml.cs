using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Profile.SystemManufacturers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x410

namespace numfortuneuwp
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private HttpResponseMessage httpResponse;
        private HttpClient client;
        private MessageDialog d;
        public MainPage()
        {
            this.InitializeComponent();
            if (!SystemSupportInfo.LocalDeviceInfo.SystemProductName.Contains("Xbox")) {
                d = new MessageDialog("Unsupported Platform");
                d.Commands.Add(new UICommand("Exit", new UICommandInvokedHandler(exit)));
                IAsyncOperation<IUICommand> asyncOperation = d.ShowAsync();
            }
            client = new HttpClient();
            tick();
        }

        private void exit(IUICommand command)
        {
            Application.Current.Exit();
        }
        private async void tick()
        {
            try
            {
                httpResponse = await client.GetAsync("https://api.justyy.workers.dev/api/fortune");
            }
            catch (Exception ex)
            {
                cookie.Text = ex.Message;
                return;
            }

            if (httpResponse.IsSuccessStatusCode)
            {
                String s = await httpResponse.Content.ReadAsStringAsync();
                s = s.Substring(1, s.Length - 2);
                s = s.Replace("\\n", System.Environment.NewLine);
                s = s.Replace("\\t", "	");
                s = s.Replace("\\\"", "\"");
                cookie.Text = s;
            }
            else
            {
                cookie.Text = $"The HTTP status code is ${httpResponse.StatusCode}";
            }

        }

        public void OnTick_Click(Object obj, RoutedEventArgs args)
        {
            tick();
        }
    }
}
