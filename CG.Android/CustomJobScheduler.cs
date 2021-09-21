using Android.App;
using Android.App.Job;
using Android.Content;
using CG.Dal;
using CG.Providers.Base;
using System.Threading.Tasks;


namespace CG.Droid
{
    [Service(Name = "com.companyname.cg.CustomJobScheduler",
             Permission = "android.permission.BIND_JOB_SERVICE")]
    public class CustomJobScheduler : JobService
    {
        public override bool OnStartJob(JobParameters jobParams)
        {
            Task.Run(() =>
            {
                ContextProvider.Database.SaveItem(new User() { AccessToken = "", Email = "", Password = "", Phone = "", RefreshToken = "", UserName = "" });
                JobFinished(jobParams, false);
            });

            // Return true because of the asynchronous work
            return true;
        }

        public override bool OnStopJob(JobParameters jobParams)
        {
            // we don't want to reschedule the job if it is stopped or cancelled.
            return false;
        }
    }
    public static class JobSchedulerHelpers
    {
        public static JobInfo.Builder CreateJobBuilderUsingJobId<T>(this Context context, int jobId) where T : JobService
        {
            var javaClass = Java.Lang.Class.FromType(typeof(T));
            var componentName = new ComponentName(context, javaClass);
            return new JobInfo.Builder(jobId, componentName);
        }
    }

}
