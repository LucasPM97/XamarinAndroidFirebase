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

namespace XamarinFirebaseAndroid.Helper
{
    public class ImageLoaderHelper
    {
        private Dialog progressDialog;

        public IntPtr Handle => throw new NotImplementedException();

        public static Bitmap GetLocalBitmap(Context context, Android.Net.Uri uri)
        {
            Android.Graphics.Bitmap mBitmap = null;
            mBitmap = Android.Provider.MediaStore.Images.Media.GetBitmap(context.ContentResolver, uri);
            return mBitmap;
        }

        public static ImageResponse ValidateImage()
        {
            var response = new ImageResponse();

            response.IsValid = true;

            return response;
        }

        public static ImageSaver GetImageSaver(Context context, Android.Net.Uri uri)
        {
            var imageSaver = new ImageSaver();

            var bitmap = ImageLoaderHelper.GetLocalBitmap(context, uri);

            if (bitmap != null)
            {
                imageSaver.Bitmap = bitmap;

                var uriString = uri.ToString();

                imageSaver.ImageName = uriString.Substring(uriString.LastIndexOf("/") + 1, uriString.Length);

                return imageSaver;
            }

            return null;
        }

        public ImageResponse UploadImageFromBytes(Context context, List<Android.Net.Uri> uriList, StorageReference storageReference, FirebaseUser user)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(context);
            builder.SetTitle("Loading...");
            //View view = getLayoutInflater().inflate(R.layout.progress);

            progressDialog = builder.Create();

            try
            {
                foreach (var item in uriList)
                {
                    progressDialog.SetTitle("Loading images " + uriList.IndexOf(item) + "/" + uriList.Count);
                    var imageSaver = ImageLoaderHelper.GetImageSaver(context, item);

                    if (imageSaver != null)
                    {
                        if (imageSaver.Bitmap != null)
                        {
                            try
                            {
                                MemoryStream stream = new MemoryStream();
                                imageSaver.Bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
                                byte[] byteArray = stream.GetBuffer();

                                //File Path in Firebase Storage
                                var image = storageReference.Child("Images/" + user.Uid + "/" + imageSaver.ImageName);
                            }
                            catch (System.Exception ex)
                            {
                                Log.Debug("ERROR SAVE BITMAP", ex.Message);
                                //Toast.MakeText(this, "Error Uri to Bitmap", ToastLength.Long).Show();
                            }
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                Toast.MakeText(context, "Ups", ToastLength.Long).Show();
            }
            return null;
        }

        public static void LoadImages(Context context, Android.Net.Uri uri, StorageReference storageReference, FirebaseUser user)
        {
            var uriString = uri.ToString();
            //var imageName = uriString.Substring(uriString.LastIndexOf("/") + 1, uriString.Length);
            var uriPath = uriString.Split('/').ToList();

            var imageName = uriPath.LastOrDefault();

            //File Path in Firebase Storage
            var image = storageReference.Child("Images/" + user.Uid + "/" + imageName);

            if (context is IOnSuccessListener && context is IOnFailureListener)
            {
                var onSucces = context as IOnSuccessListener;
                var onFailure = context as IOnFailureListener;
                image.GetFile(uri).AddOnSuccessListener(onSucces).AddOnFailureListener(onFailure);
            }
        }

        public static void UploadImageFromUri(Context context, Android.Net.Uri uri, StorageReference storageReference, FirebaseUser user)
        {
            var uriString = uri.ToString();
            //var imageName = uriString.Substring(uriString.LastIndexOf("/") + 1, uriString.Length);
            var uriPath = uriString.Split('/').ToList();

            var imageName = uriPath.LastOrDefault();

            //File Path in Firebase Storage
            var image = storageReference.Child("Images/" + user.Uid + "/" + imageName);

            if (context is IOnSuccessListener && context is IOnFailureListener)
            {
                var onSucces = context as IOnSuccessListener;
                var onFailure = context as IOnFailureListener;
                image.PutFile(uri).AddOnSuccessListener(onSucces).AddOnFailureListener(onFailure);
            }
        }

        public static void GetUriFromFirebase(Context context, Android.Net.Uri uri, StorageReference storageReference, FirebaseUser user)
        {
            var uriString = uri.ToString();
            //var imageName = uriString.Substring(uriString.LastIndexOf("/") + 1, uriString.Length);
            var uriPath = uriString.Split('/').ToList();

            var imageName = uriPath.LastOrDefault();

            var image = storageReference.Child("Images/" + user.Uid + "/" + imageName);

            if (context is IOnSuccessListener && context is IOnFailureListener)
            {
                var onSucces = context as IOnSuccessListener;
                var onFailure = context as IOnFailureListener;
                image.PutFile(uri).AddOnSuccessListener(onSucces).AddOnFailureListener(onFailure);
            }
        }

        public class ImageResponse
        {
            public bool Success { get; set; }
            public bool IsValid { get; set; }
            public string ErrorMessage { get; set; }
        }

        public class ImageSaver
        {
            public Android.Net.Uri Uri { get; set; }
            public Bitmap Bitmap { get; set; }
            public string ImageName { get; set; }
        }
    }
}