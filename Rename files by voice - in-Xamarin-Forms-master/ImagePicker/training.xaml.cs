using ImagePicker;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using XFSpeechDemo;

namespace ImagePicker
{
    public partial class training : ContentPage
    {
        private ISpeechToText2 _speechRecongnitionInstance2;
        public training()
        {
            InitializeComponent();




            

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await GetPermissions();
        }
        public async Task GetPermissions()
        {


            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (storageStatus != PermissionStatus.Granted)
            {

                //if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                //{
                //    await Application.Current.MainPage.DisplayAlert("Storage Permission", "OK?", "OK");
                //}

                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                cameraStatus = results[Permission.Camera];
                storageStatus = results[Permission.Storage];
                var storageStatus2 = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

                if (storageStatus2 != PermissionStatus.Granted)
                {

                    await DisplayAlert("Warning", "Please open App again and accept Permissions", "OK");
                    await Navigation.PushModalAsync(new Exit());

                }
                //else
                //{
                //    //await DisplayAlert(" grant = alow", "Gunna need that location", "OK");
                //    await Navigation.PushModalAsync(new MainPage());

                //}
            }
            if (storageStatus == PermissionStatus.Granted)
            {


                                    await Navigation.PushModalAsync(new MainPage());


            }

        }

    }
}
