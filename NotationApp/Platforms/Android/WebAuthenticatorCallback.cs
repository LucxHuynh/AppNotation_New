using Android.App;
using Android.Content.PM;
using Android.Content;
using Android.OS;


namespace NotationApp.Platforms.Android
{
    [Activity(Name = "com.cscompany.appnotations.WebAuthenticatorCallback",
          NoHistory = true,
          LaunchMode = LaunchMode.SingleTop,
          Exported = true)]
    [IntentFilter(new[] { Intent.ActionView },
              Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
              DataScheme = "com.cscompany.appnotations", // URI scheme cần khớp
              AutoVerify = true)]
    public class WebAuthenticatorCallbackActivity : Microsoft.Maui.Authentication.WebAuthenticatorCallbackActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
    }
}
