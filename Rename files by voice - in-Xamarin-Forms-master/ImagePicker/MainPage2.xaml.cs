using ImagePicker.Model;
using ImagePicker.ViewModel;
//using Plugin.InAppBilling;
//using Plugin.InAppBilling.Abstractions;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using XFSpeechDemo;
using PermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;
using ImagePicker.Helpers;
using System.Resources;
using System.Reflection;
using Plugin.Multilingual;

namespace ImagePicker
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]










    public partial class MainPage2 : ContentPage
    {


        private ISpeechToText _speechRecongnitionInstance;


        MainPageViewModel mainPageViewModel;
        Frame currentSelectedFrame; //store the selected image frame from collection view
        bool isSelected; // determines is selected of the image
        Frame selectedFrame; //stores the previous selected image frame of collection view
        MediaAssest selectedMediaAsset; //this holds the selected image details
        IMediaService mediaService;



        //protected override async void OnAppearing()
        //{
        //    base.OnAppearing();

        //    await GetPermissions();
        //}


        public MainPage2()
        {


            InitializeComponent();

            

            try
                {
                    _speechRecongnitionInstance = DependencyService.Get<ISpeechToText>();
                }
                catch (Exception ex)
                {
                    recon.Text = ex.Message;
                }


                MessagingCenter.Subscribe<ISpeechToText, string>(this, "STT", (sender, args) =>
                {
                    SpeechToTextFinalResultRecieved(args);
                });

                MessagingCenter.Subscribe<ISpeechToText>(this, "Final", (sender) =>
                {
                    start.IsEnabled = true;
                });

                MessagingCenter.Subscribe<IMessageSender, string>(this, "STT", (sender, args) =>
                {
                    SpeechToTextFinalResultRecieved(args);
                });






                mediaService = Xamarin.Forms.DependencyService.Get<IMediaService>();
                mainPageViewModel = new MainPageViewModel(mediaService);
                BindingContext = mainPageViewModel;




          


                // ImageSkipOrSelectImageClickEvent();// check preference already image selected if selected load the profile picture or else defult image
            imageselector.IsVisible = true; //make image picker stack layout visible
                                                //  bodyContent.Opacity = 0.3; //make behing content opacity so that image picker gets attention
                bodyContent.InputTransparent = true; // disable any user interaction on main content
                imageNext.IsVisible = false;
                //  imageSkip.IsVisible = true;
                imageselectorFrame.TranslateTo(0, imageselectorFrame.Y + 50, 300);
                mainPageViewModel.LoadMediaAssets(); //load the image from phone storage


        }


        async void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            imageselector.IsVisible = true; //make image picker stack layout visible
            bodyContent.Opacity = 0.3; //make behing content opacity so that image picker gets attention
            bodyContent.InputTransparent = true; // disable any user interaction on main content
            imageNext.IsVisible = false;
            //imageSkip.IsVisible = true;
            await imageselectorFrame.TranslateTo(0, imageselectorFrame.Y + 50, 300);
            await mainPageViewModel.LoadMediaAssets(); //load the image from phone storage
        }

      


        public void imageNextTapped(System.Object sender, System.EventArgs e)
        {
        }

            private void ImageSkipOrSelectImageClickEvent()
        {

            //common method to handle to display default image or seleceted image
            if (Preferences.ContainsKey("ProfileImage"))
            {
                string path = Preferences.Get("ProfileImage", null);
                if (path == "default")
                {
                    profilePicture.Source = "default5.png";
                }


                if (path == "")
                {

                    return;

                }
                if (path.Contains(".mp4"))
                {

                    profilePicture.Source = DependencyService.Get<IMediaService>().GenerateThumbnailImageSource(path,0);
                    string filename = path.Substring(path.LastIndexOf("/") + 1);

                    path_image.Text = filename;
                    path_image2.Text = path;

                }

                else
                {


                    profilePicture.Source = path;
                    string filename = path.Substring(path.LastIndexOf("/") + 1);
                    
                    path_image.Text = filename;
                    path_image2.Text = path;


                }
            }


        }


















       


        async void imageTapped(System.Object sender, System.EventArgs e)
        {
            
            var s = (StackLayout)sender;
            var ss = s.Children[0] as Grid;
            var sss = ss.Children[0] as Frame;
            selectedFrame = ss.Children[1] as Frame;
            var clicked = (TappedEventArgs)e;
            var mediaAssest = (MediaAssest)clicked.Parameter;
            selectedMediaAsset = mediaAssest;



            if (mediaAssest.PreviewPath == "group.png")
            {



                }




            else
            {


                   

                        if (currentSelectedFrame != null)
                {
                    if (selectedFrame != currentSelectedFrame)
                    {
                        selectedFrame.BackgroundColor = Color.Green;
                        currentSelectedFrame.BackgroundColor = Color.Transparent;
                        currentSelectedFrame = selectedFrame;
                        isSelected = true;
                    }
                    else
                    {
                        if (selectedFrame.BackgroundColor == Color.Green)
                        {
                            selectedFrame.BackgroundColor = Color.Transparent;

                            currentSelectedFrame = selectedFrame;
                            isSelected = false;
                        }
                        else
                        {
                            selectedFrame.BackgroundColor = Color.Green;
                            isSelected = true;
                        }
                    }
                }
                else
                {
                    selectedFrame.BackgroundColor = Color.Green;
                    currentSelectedFrame = selectedFrame;
                    isSelected = true;
                }

                //display next button
                if (isSelected)
                {


                    //display next button
                    imageNext.IsVisible = false;
                    await imageNext.TranslateTo(0, 0, 300);


                }
                else
                {
                    //display skip options
                    imageNext.IsVisible = false;
                    await imageNext.TranslateTo(0, 0, 300);
                }

            }



            try
            {

                //if (currentSelectedFrame == null)
                //{
                //    return;
                //}

                bodyContent.Opacity = 1;
                bodyContent.InputTransparent = false;
                // imageselector.IsVisible = false;

                Preferences.Set("ProfileImage", selectedMediaAsset.Path);


                //set the path of the image in preferences

                ImageSkipOrSelectImageClickEvent();
                isSelected = false;
                selectedMediaAsset = null;
                selectedFrame.BackgroundColor = Color.Transparent;

            }
            catch
            {

                return;

            }


        }








        async void SpeechToTextFinalResultRecieved(string args)
        {

            recon.Text = args;

            const string ResourceId = "ImagePicker.AppResource";
            var resmgr = new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly);
            var ci = CrossMultilingual.Current.CurrentCultureInfo;





            var answer = await DisplayAlert(resmgr.GetString("confirm1", ci), resmgr.GetString("question_rename", ci)  + args + "", resmgr.GetString("yes", ci), resmgr.GetString("no", ci));
            if (answer)
            {
                
                
                
                recon.Text = args;




                string filename = path_image2.Text.Substring(path_image2.Text.LastIndexOf("/") + 1);

                string extension = Path.GetExtension(path_image2.Text);

                string directory = Path.GetDirectoryName(path_image2.Text);

                string without = Path.GetFileNameWithoutExtension(path_image2.Text);

                if (File.Exists(directory + "/" + recon.Text + extension))
                {

                    await DisplayAlert(resmgr.GetString("error", ci), resmgr.GetString("file_available", ci), "OK");

                    return;

                }

                System.IO.File.Move(path_image2.Text, directory + "/" + recon.Text + extension);



                DependencyService.Get<IMediaService>().UpdateGallery(path_image2.Text);


                DependencyService.Get<IMediaService>().UpdateGallery(directory + "/" + recon.Text + extension);

                string path2 = directory + "/" + recon.Text + extension;
                string filename2 = path2.Substring(path2.LastIndexOf("/") + 1);

                path_image.Text = filename2;
                recon.Text = "";



            }
            else
            {
                recon.Text = "";

            }



          

        }



        async void refresh_Clicked(object sender, EventArgs e)
        {


            imageselector.IsVisible = false;
            var btn = sender as Button;
            btn.IsEnabled = false;
            activity.IsEnabled = true;
            activity.IsRunning = true;
            activity.IsVisible = true;
            await mainPageViewModel.LoadMediaAssets(); //load the image from phone storage
            activity.IsEnabled = false;
            activity.IsRunning = false;
            activity.IsVisible = false;
            imageselector.IsVisible = true;
            btn.IsEnabled = true;


            //Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            //{

            //    btn.IsEnabled = true;
            //    return false;
            //});

        }






        //async void Camera_Clicked(object sender, EventArgs e)
        //{






        //    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
        //    {
        //        await DisplayAlert("Error", ":( Camera Not Available.", "OK");
        //        return;
        //    }

        //    var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
        //    var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

        //    if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
        //    {
        //        var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
        //        cameraStatus = results[Permission.Camera];
        //        storageStatus = results[Permission.Storage];




        //        var storageStatus2 = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);

        //        if (storageStatus2 != PermissionStatus.Granted)
        //        {

        //            await DisplayAlert("Warning", "Please accept Permissions", "OK");
        //            return;

        //        }
              







        //    }

        //    if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
        //    {

        //        //===HERE IS THE PROBLEM, READ METHOD BUT NOT OPEN CAMERA! NOT ERRORS, NOT EXCEPTION, NOTHING===
        //        var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
        //        {
        //            SaveToAlbum = true,
        //            //Directory = "Sample",
        //            //Name = "test.jpg"
        //        });


        //        if (file == null)
        //            return;

        //        // await DisplayAlert("File Location", file.Path, "OK");



        //        bodyContent.Opacity = 1;
        //        bodyContent.InputTransparent = false;
        //        // imageselector.IsVisible = false;

        //        string location = file.Path;

        //        string camera_image = location.Substring(location.LastIndexOf("/") + 1);
        //        //  await DisplayAlert("sdsdfsd", camera_image, "ok");
        //        string path3 = "storage/emulated/0/DCIM/Camera/" + camera_image;


        //        System.IO.File.Move(location, path3);



        //        DependencyService.Get<IMediaService>().UpdateGallery(location);


        //        DependencyService.Get<IMediaService>().UpdateGallery(path3);
        //        string temp = "storage/emulated/0/Pictures/temp/" + camera_image;

        //        System.IO.File.Delete(temp);
        //        DependencyService.Get<IMediaService>().UpdateGallery(temp);


        //        path_image2.Text = path3;
        //        path_image.Text = camera_image;



        //        profilePicture.Source = path3;


        //    }





        //}





            public  void Start_Clicked(object sender, EventArgs e)
        {

            task2();
         


        }

    async void Subscribe_Clicked(object sender, EventArgs e)
        {


            const string ResourceId = "ImagePicker.AppResource";
            var resmgr = new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly);
            var ci = CrossMultilingual.Current.CurrentCultureInfo;


            if (path_image.Text == "")
            {

                await DisplayAlert(resmgr.GetString("error", ci), resmgr.GetString("choose_image", ci), "ok");
                return;


            }
        

            //await PurchaseItem("product_2", "payload");question_delete
            var answer = await DisplayAlert(resmgr.GetString("confirm1", ci) , resmgr.GetString("question_delete", ci)  + path_image2.Text + "", resmgr.GetString("yes", ci), resmgr.GetString("no", ci));
            if (answer)
            {

                System.IO.File.Delete(path_image2.Text);
                DependencyService.Get<IMediaService>().UpdateGallery(path_image2.Text);
                await DisplayAlert(resmgr.GetString("confirm1", ci), resmgr.GetString("confirm_delete", ci) , "OK");
            }
        }


        public async void task2()
        {

  const string ResourceId = "ImagePicker.AppResource";
                var resmgr = new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly);
                var ci = CrossMultilingual.Current.CurrentCultureInfo;


            if (path_image.Text == "")
            {
              


                await DisplayAlert(resmgr.GetString("error", ci), resmgr.GetString("choose_file", ci), "ok");
                return;


            }

            if (!File.Exists(path_image2.Text))
            {

                await DisplayAlert(resmgr.GetString("error", ci), path_image.Text + resmgr.GetString("refresh_confirm", ci), "ok");
                return;

            }

            try
            {

                _speechRecongnitionInstance.StartSpeechToText();
            }
            catch (Exception ex)
            {
                recon.Text = ex.Message;
            }

            if (Device.RuntimePlatform == Device.iOS)
            {
                start.IsEnabled = false;
            }


        }








        //public async Task<bool> PurchaseItem(string productId, string payload)
        //{
        //    var billing = CrossInAppBilling.Current;
        //    try
        //    {
        //        var connected = await billing.ConnectAsync(ItemType.Subscription);
        //        if (!connected)
        //        {

        //            await DisplayAlert("خطأ","الانترنت لا يكون متصلا", "ok");
        //            //we are offline or can't connect, don't try to purchase
        //            return false;
        //        }
        //        //check purchases
        //        var purchase = await billing.PurchaseAsync(productId, ItemType.Subscription, payload);
        //        if (purchase == null)
        //        {
        //            //did not purchase
        //        }
        //        else if (purchase.State == PurchaseState.Purchased)
        //        {
        //            //purchased!
        //            //task2();

        //        }
            
        //    } 
            
        //    catch (InAppBillingPurchaseException purchaseEx)
        //    {
        //        //Billing Exception handle this based on the type
        //        Debug.WriteLine("Error: " + purchaseEx);
        //    }
        //    catch (Exception ex)
        //    {
        //        //Something else has gone wrong, log it
        //        Debug.WriteLine("Issue connecting: " + ex);
        //    }
        //    finally
        //    {
        //        await billing.DisconnectAsync();
        //    }
        //    return false;


        //}









    }
}
