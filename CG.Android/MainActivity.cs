using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Views;
using CG.Services;
using Android.App.Job;
using CG.Providers.Base;
using Android.Widget;

namespace CG.Droid
{
    [Activity(Label = "CG", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
    /*
            var job = this.CreateJobBuilderUsingJobId<CustomJobScheduler>(1)
                .SetPeriodic(1000)
                .Build();

            var jobScheduler = (JobScheduler)GetSystemService(JobSchedulerService);
            var jobResult = jobScheduler.Schedule(job);
                
            if (JobScheduler.ResultSuccess == jobResult)
            {

            }
            else
            {
               
            }  
      */  
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}