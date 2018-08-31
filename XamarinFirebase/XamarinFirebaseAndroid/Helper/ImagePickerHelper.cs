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

namespace XamarinFirebaseAndroid.Helper
{
    public class ImagePickerHelper
    {
        public static Intent GetPickImageIntent(Context context)
        {
            Intent chooserIntent = null;

            List<Intent> intentList = new List<Intent>();

            Intent pickIntent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
            Intent takePhotoIntent = new Intent(MediaStore.ActionImageCapture);
            takePhotoIntent.PutExtra("return-data", true);
            intentList = AddIntentsToList(context, intentList, pickIntent);
            intentList = AddIntentsToList(context, intentList, takePhotoIntent);

            if (intentList.Count > 0)
            {
                chooserIntent = Intent.CreateChooser(intentList.FirstOrDefault(), "Select");
                chooserIntent.PutExtra(Intent.ExtraInitialIntents, intentList.ToArray());
            }

            return chooserIntent;
        }

        private static List<Intent> AddIntentsToList(Context context, List<Intent> list, Intent intent)
        {
            List<ResolveInfo> resInfo = context.PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly).ToList();
            foreach (ResolveInfo resolveInfo in resInfo)
            {
                String packageName = resolveInfo.ActivityInfo.PackageName;
                Intent targetedIntent = new Intent(intent);
                targetedIntent.SetPackage(packageName);
                list.Add(targetedIntent);
            }
            return list;
        }

        internal static object getPickImageIntent()
        {
            throw new NotImplementedException();
        }
    }
}