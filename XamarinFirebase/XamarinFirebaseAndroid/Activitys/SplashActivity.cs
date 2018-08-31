using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Support.V4;
using Android.Support.V7.App;
using Android.Support.V4.View;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Support.V4.App;
using System.Threading.Tasks;
using Android.Content;
using XamarinFirebaseAndroid;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Java.Util;
using System.Collections.Generic;
using Firebase.Auth;

namespace XamarinFirebaseAndroid.Activitys
{
    [Activity(MainLauncher = true, LaunchMode = LaunchMode.SingleTop, Theme = "@style/AppTheme.NoActionBar", NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.splash_activity);

            Task.Delay(5000);

            AccessToken accessToken = AccessToken.CurrentAccessToken;
            //bool isLoggedIn = accessToken != null && !accessToken.IsExpired;

            var user = FirebaseAuth.Instance.CurrentUser;

            if (user != null)
            {
                ICollection<string> ic = new List<string>();
                ic.Add("public_profile");

                LoginManager.Instance.LogInWithReadPermissions(this, ic);

                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            }
            else
            {
                Intent intent = new Intent(this, typeof(LoginActivity));
                StartActivity(intent);
            }
        }
    }
}