using Plugin.Multilingual;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagePicker
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            AppResource.Culture = CrossMultilingual.Current.DeviceCultureInfo;


            MainPage = new training();
        }

        protected  override void OnStart()
        {



        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
