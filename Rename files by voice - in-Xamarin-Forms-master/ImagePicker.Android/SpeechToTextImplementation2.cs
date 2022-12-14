using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Views;
using Android.Widget;
using Plugin.CurrentActivity;
using XFSpeechDemo.Droid;
[assembly: Xamarin.Forms.Dependency(typeof(SpeechToTextImplementation2))]
namespace XFSpeechDemo.Droid
{
    
    public class SpeechToTextImplementation2 : ISpeechToText2
    {
        private readonly int VOICE = 10;
        private Activity _activity;
        public SpeechToTextImplementation2()
        {
            _activity = CrossCurrentActivity.Current.Activity;

        }



      
        public void StartSpeechToText2()
        {
            StartRecordingAndRecognizing2();
        }

        private void StartRecordingAndRecognizing2()
        {
            string rec = global::Android.Content.PM.PackageManager.FeatureMicrophone;
            if (rec == "android.hardware.microphone")
            {
				try
				{
					var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
                voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);


                voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Speak now");

                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
                voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
                voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
                _activity.StartActivityForResult(voiceIntent, VOICE);
				}
				catch(ActivityNotFoundException ex)
                {
                    //String appPackageName = "com.google.android.googlequicksearchbox";
                    try
                    {
                        
                    }
                    catch (ActivityNotFoundException e)
                    {
                    }
                }
                
            }
            else
            {
                throw new Exception("No mic found");
            }
        }


        public void StopSpeechToText2()
        {

        }
    }
}