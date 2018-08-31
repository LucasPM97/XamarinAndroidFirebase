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
using Android.Widget;
using Android.Content;
using XamarinFirebaseAndroid;
using Firebase;
using Firebase.Auth;
using Xamarin.Facebook;
using Android.Gms.Tasks;
using Xamarin.Facebook.Login.Widget;
using Xamarin.Facebook.Login;
using Android.Runtime;
using Android.Util;
using Java.Security;
using Android.Gms.Common;
using System;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common.Apis;
using Android.Gms.Auth.Api;
using XamarinFirebaseAndroid.Helper;
using Entities.Security;

namespace XamarinFirebaseAndroid.Activitys
{
    [Activity(Theme = "@style/AppTheme.NoActionBar")]
    public class LoginActivity : AppCompatActivity, IFacebookCallback, IOnCompleteListener, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {
        private SignInButton GoogleButton;
        private LoginButton FacebookButton;

        private GoogleApiClient _googleApiClient;

        private FirebaseAuth _auth;

        private ICallbackManager callbackManager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.login_activity);
            //return base.OnOptionsItemSelected(item);

            FirebaseApp.InitializeApp(this);
            _auth = FirebaseAuth.Instance;

            GoogleButton = FindViewById<SignInButton>(Resource.Id.gButton);
            GoogleButton.Click += (object sender, System.EventArgs e) =>
            {
                var signGoogleintent = Auth.GoogleSignInApi.GetSignInIntent(_googleApiClient);

                StartActivityForResult(signGoogleintent, Constants.ActionResult.GoogleLoginResul);
            };

            FacebookButton = FindViewById<LoginButton>(Resource.Id.fButton);
            FacebookButton.SetReadPermissions("email", "public_profile");

            callbackManager = CallbackManagerFactory.Create();
            FacebookButton.RegisterCallback(callbackManager, this);

            _googleApiClient = LoginHelper.ConfigureGoogleSignIn(this);
        }

        private void handleFacebookAccessToken(AccessToken accessToken)
        {
            //Facebook Credential
            AuthCredential credential = FacebookAuthProvider.GetCredential(accessToken.Token);
            _auth.SignInWithCredential(credential).AddOnCompleteListener(this, this);
        }

        //facebook IFacebookCallback implementation
        public void OnSuccess(Java.Lang.Object p0)
        {
            LoginResult loginResult = p0 as LoginResult;
            handleFacebookAccessToken(loginResult.AccessToken);
        }

        private void FirebaseAuthWithGoogle(GoogleSignInAccount acct)
        {
            //Google Credential
            AuthCredential credential = GoogleAuthProvider.GetCredential(acct.IdToken, null);
            _auth.SignInWithCredential(credential).AddOnCompleteListener(this, this);
        }

        public void OnCancel()
        {
        }

        public void OnError(FacebookException p0)
        {
        }

        //firebase IOnCompleteListener implementation
        public void OnComplete(Task task)
        {
            // Login Completed
            if (task.IsSuccessful)
            {
                FirebaseUser user = _auth.CurrentUser;
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            }
            else
            {
                Toast.MakeText(this, "Authentication failed.", ToastLength.Short).Show();
            }
        }

        protected override void OnStart()
        {
            base.OnStart();
            FirebaseUser currentUser = _auth.CurrentUser;
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            var resultCodeNum = 0;

            if (requestCode == Constants.ActionResult.GoogleLoginResul)
            {
                var GoogleResult = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                HandleSignInResult(GoogleResult);
            }
            else
            {
                switch (resultCode)
                {
                    case Result.Ok:
                        resultCodeNum = -1;
                        break;

                    case Result.Canceled:
                        resultCodeNum = 0;
                        break;

                    case Result.FirstUser:
                        resultCodeNum = 1;
                        break;
                }
            }

            callbackManager.OnActivityResult(requestCode, resultCodeNum, data);
        }

        private void HandleSignInResult(GoogleSignInResult googleResult)
        {
            if (googleResult.IsSuccess)
            {
                try
                {
                    // Google Sign In was successful, authenticate with Firebase
                    GoogleSignInAccount account = googleResult.SignInAccount;
                    FirebaseAuthWithGoogle(account);
                }
                catch (ApiException e)
                {
                }
            }
        }

        public void OnConnected(Bundle connectionHint)
        {
            try
            {
                var obj = connectionHint;
            }
            catch (Exception ex)
            {
                Log.Debug("ERROR", ex.Message);
            }
        }

        public void OnConnectionSuspended(int cause)
        {
            try
            {
                var obj = cause;
            }
            catch (Exception ex)
            {
                Log.Debug("ERROR", ex.Message);
            }
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Connection failed:" + result);
            }
            catch (Exception ex)
            {
                Log.Debug("ERROR", ex.Message);
            }
        }
    }
}