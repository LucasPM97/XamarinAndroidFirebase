using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using XamarinFirebaseAndroid;

namespace XamarinFirebase.Views
{
    public class ItemView : RecyclerView.ViewHolder
    {
        public ImageView Image { get; private set; }
        public TextView Caption { get; private set; }

        public ItemView(View itemView) : base(itemView)
        {
            Image = ItemView.FindViewById<ImageView>(Resource.Id.imageView);
            Caption = ItemView.FindViewById<TextView>(Resource.Id.textView);
        }
    }
}