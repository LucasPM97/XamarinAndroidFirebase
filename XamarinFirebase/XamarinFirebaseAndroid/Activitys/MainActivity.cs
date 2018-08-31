using System;
using System.Linq;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using XamarinFirebaseAndroid.Fragments;
using XamarinFirebaseAndroid;
using Entities.Security;
using Com.Asksira.BSImagePickerLib;
using Android.Net;
using System.Collections.Generic;
using Firebase.Storage;
using Firebase;
using XamarinFirebaseAndroid.Helper;
using Android.Util;
using Java.IO;
using System.IO;
using Firebase.Auth;
using Android.Gms.Tasks;

namespace XamarinFirebaseAndroid.Activitys
{
    [Activity(Theme = "@style/AppTheme.NoActionBar")]
    public class MainActivity : AppCompatActivity, BSImagePicker.IOnMultiImageSelectedListener, IOnSuccessListener, IOnFailureListener
    {
        private StorageReference storageReference;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(XamarinFirebaseAndroid.Resource.Layout.activity_main);
            storageReference = FirebaseStorage.Instance.Reference;

            FirebaseApp.InitializeApp(this);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(XamarinFirebaseAndroid.Resource.Id.fab);
            fab.SetImageResource(XamarinFirebaseAndroid.Resource.Drawable.baseline_add_white_24);
            fab.Click += FabOnClick;
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            TakePhoto();
        }

        private void TakePhoto()
        {
            var result = CheckImagesPermission();

            if (!result)
            {
                return;
            }
            else
            {
                try
                {
                    BSImagePicker multiSelectionPicker = new BSImagePicker
                    .Builder("com.company.proyect.fileprovider")// Insert here your File Provider
                    .IsMultiSelect() //Set this if you want to use multi selection mode.
                    .SetMultiSelectBarBgColor(Resource.Color.colorPrimary) //Default: #FFFFFF. You can also set it to a translucent color.
                    .SetMultiSelectTextColor(Resource.Color.colorAccent) //Default: #212121(Dark grey). This is the message in the multi-select bottom bar.
                    .SetMultiSelectDoneTextColor(Resource.Color.colorAccent) //Default: #388e3c(Green). This is the color of the "Done" TextView.
                    .SetOverSelectTextColor(Resource.Color.colorAccent) //Default: #b71c1c. This is the color of the message shown when user tries to select more than maximum select count.
                    .DisableOverSelectionMessage() //You can also decide not to show this over select message.
                    .Build();

                    multiSelectionPicker.Show(SupportFragmentManager, "picker");
                }
                catch (Exception ex)
                {
                    string exception = ex.Message;
                }
                //var multiIntent = XamarinFirebaseAndroid.Helper.ImagePickerHelper.GetPickImageIntent(this);

                //StartActivityForResult(multiIntent, (int)Constants.ActionResult.CameraRequestResult);
            }
        }

        public void OnMultiImageSelected(List<Android.Net.Uri> uriList)
        {
            Toast.MakeText(this, "BS LISTENER", ToastLength.Short).Show();

            List<Bitmap> bitmaps = new List<Bitmap>();

            Android.Support.V7.App.AlertDialog.Builder builder = new Android.Support.V7.App.AlertDialog.Builder(this);
            builder.SetTitle("Loading...");
            //View view = getLayoutInflater().inflate(R.layout.progress);

            Dialog dialog = builder.Create();

            try
            {
                foreach (var item in uriList)
                {
                    try
                    {
                        dialog.SetTitle("Loading images " + uriList.IndexOf(item) + "/" + uriList.Count);
                        ImageLoaderHelper.UploadImageFromUri(this, item, storageReference, FirebaseAuth.Instance.CurrentUser);
                    }
                    catch (System.Exception ex)
                    {
                        Log.Debug("ERROR SAVE BITMAP", ex.Message);
                        //Toast.MakeText(this, "Error Uri to Bitmap", ToastLength.Long).Show();
                    }
                }
            }
            catch (System.Exception)
            {
                Toast.MakeText(this, "Ups", ToastLength.Long).Show();
            }
            List<Android.Net.Uri> loadedUriList = new List<Android.Net.Uri>();
            foreach (var item in uriList)
            {
                var uriString = item.ToString();
                //var imageName = uriString.Substring(uriString.LastIndexOf("/") + 1, uriString.Length);
                var uriPath = uriString.Split('/').ToList();

                var imageName = uriPath.LastOrDefault();
            }

            var valid = ImageLoaderHelper.ValidateImage();

            if (valid.IsValid)
            {
                List<string> stringUris = new List<string>();

                try
                {
                    stringUris = uriList.Select(x => x.ToString()).ToList();

                    Bundle bundle = new Bundle();
                    bundle.PutStringArray("UriList", stringUris.ToArray());

                    var frag = new Fragment1();
                    frag.Arguments = bundle;

                    LoadFragment(frag);
                }
                catch (Exception ex)
                {
                    Log.Debug("ERROR URI TO FRAGMENT", ex.Message);
                    //Toast.MakeText(this, "Error Uri to Bitmap", ToastLength.Long).Show();
                }
            }
            else
            {
                Toast.MakeText(this, valid.ErrorMessage, ToastLength.Long).Show();
            }
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            var _result = result;
        }

        public void OnFailure(Java.Lang.Exception e)
        {
            var _exception = e;
        }

        private bool CheckImagesPermission()
        {
            try
            {
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.Camera) != (int)Permission.Granted || ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted || ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != (int)Permission.Granted)
                {
                    ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.Camera, Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage }, (int)Constants.ActionResult.RequestImagesPermission);

                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            if (requestCode == (int)Constants.ActionResult.RequestImagesPermission)
            {
                if (grantResults.Any())
                {
                    bool allGranted = true;
                    foreach (var item in grantResults)
                    {
                        if (item != Permission.Granted)
                        {
                            allGranted = false;
                        }
                    }
                    if (!allGranted)
                    {
                        PermissionRequesDenied();
                    }
                    else
                    {
                        TakePhoto();
                    }
                }
                else
                {
                    PermissionRequesDenied();
                }
            }
        }

        public void PermissionRequesDenied()
        {
            Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(this);
            alert.SetTitle("Attention");
            alert.SetMessage("Accept this permissions");

            Dialog dialog = alert.Create();

            alert.SetPositiveButton("Allow", (senderAlert, args) =>
            {
                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.Camera }, (int)Constants.ActionResult.RequestImagesPermission);
                dialog.Dismiss();
            });

            alert.SetNegativeButton("Cancel", (senderAlert, args) =>
            {
                dialog.Dismiss();
            });
            dialog.Show();
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == (int)Constants.ActionResult.CameraRequestResult && resultCode == Result.Ok)
            {
                Bitmap bitmap = (Bitmap)data.Extras.Get("data");
            }
        }

        private void LoadFragment(Android.Support.V4.App.Fragment fragment)
        {
            SupportFragmentManager.BeginTransaction().Replace(XamarinFirebaseAndroid.Resource.Id.maincontainer, fragment)
                .SetTransition((int)FragmentTransit.FragmentFade)
                .AddToBackStack(null).Commit();
        }
    }
}