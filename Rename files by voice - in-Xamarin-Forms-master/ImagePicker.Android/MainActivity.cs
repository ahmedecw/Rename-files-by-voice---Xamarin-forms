using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using ImagePicker.Droid.DependencyServiceImplementation;
using Android.Content;
using FFImageLoading.Forms.Platform;
using Android.Speech;
using Xamarin.Forms;
using Plugin.Media;
using Plugin.CurrentActivity;
//using Plugin.InAppBilling;
using Android.Gms.Ads;
using Plugin.Permissions;

namespace ImagePicker.Droid
{



    [Activity(Label = "Delete and Rename Files", Icon = "@drawable/icon2", Theme = "@style/MainTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity , IMessageSender
    {

        public static Activity FormsActivity { get; set; }
        public static Context FormsContext { get; set; }

        private readonly int VOICE = 10;

        protected override   void OnCreate(Bundle savedInstanceState)
        {
            FormsActivity = this;
            FormsContext = this;
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            CachedImageRenderer.Init(true);
            base.OnCreate(savedInstanceState);

            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            MobileAds.Initialize(ApplicationContext, "ca-app-pub-2572035864509774~2850414065");


            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity = this;
            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());


        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            try
            {

                PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
            catch { }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {

            if (requestCode == VOICE)
            {
                if (resultCode == Result.Ok)
                {
                    var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                    if (matches.Count != 0)
                    {
                        string textInput = matches[0];
                        MessagingCenter.Send<IMessageSender, string>(this, "STT", textInput);
                    }
                    else
                    {
                        MessagingCenter.Send<IMessageSender, string>(this, "STT", "No input");
                    }

                }
            }






            



            MediaServiceImplementation.OnActivityResult(requestCode, resultCode, data);
            base.OnActivityResult(requestCode, resultCode, data);
            //InAppBillingImplementation.HandleActivityResult(requestCode, resultCode, data);


        }

    }
}