using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Views;
using XamarinFirebaseAndroid.Adapter;
using System;
using System.Collections.Generic;
using Entities;

namespace XamarinFirebaseAndroid.Fragments
{
    public class Fragment1 : Fragment
    {
        private RecyclerView recyclerView;
        private RecyclerView.LayoutManager layoutManager;
        private ItemRecyclerAdapter adapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public static Fragment1 NewInstance()
        {
            var frag1 = new Fragment1 { Arguments = new Bundle() };

            return frag1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.fragment1, null);

            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);

            // Create your fragment here
            List<DocumentItem> documentItems = new List<DocumentItem>();

            var uriList = Arguments.GetStringArray("UriList");

            foreach (var item in uriList)
            {
                documentItems.Add(new DocumentItem
                {
                    CreatedDate = DateTime.Now,
                    documentImageUrl = item,
                    Tags = new List<string> { "Document", "test" }
                });
            }

            //Setup RecyclerView
            adapter = new ItemRecyclerAdapter(Activity, documentItems);

            recyclerView.SetAdapter(adapter);

            layoutManager = new GridLayoutManager(Context, 2);
            recyclerView.SetLayoutManager(layoutManager);

            return view;
        }
    }
}