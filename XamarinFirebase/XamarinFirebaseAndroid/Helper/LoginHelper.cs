using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Facebook;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Facebook.AppEvents;
using Android.Provider;
using Android.Content.PM;
using Android.Graphics;
using System.IO;
using Firebase.Storage;
using Firebase.Auth;
using Android.Util;
using Java.Lang;
using Android.Gms.Tasks;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Auth.Api;
using Android.Gms.Common.Apis;
using XamarinFirebaseAndroid.Activitys;

namespace XamarinFirebaseAndroid.Helper
{
    public class LoginHelper
    {
        public static GoogleApiClient ConfigureGoogleSignIn(Context context)
        {
            //if (context is IOnCompleteListener && context is GoogleApiClient.IConnectionCallbacks && context is GoogleApiClient.IOnConnectionFailedListener)
            if (context is LoginActivity)
            {
                //var onComplete = context as IOnCompleteListener;
                //var goooglConncectionCallBack = context as GoogleApiClient.IConnectionCallbacks;
                //var goooglConncectionFail = context as GoogleApiClient.IOnConnectionFailedListener;

                var LoginContext = (LoginActivity)context;

                GoogleSignInOptions gso = new GoogleSignInOptions
                .Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestIdToken(context.GetString(Resource.String.server_client_id))
                .RequestEmail()
                .Build();

                var _googleApiClient = new GoogleApiClient
                    .Builder(LoginContext)
                    .EnableAutoManage(LoginContext, LoginContext).AddOnConnectionFailedListener(LoginContext)
                    .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
                    .AddConnectionCallbacks(LoginContext).Build();

                return _googleApiClient;
            }

            return null;
        }
    }
}