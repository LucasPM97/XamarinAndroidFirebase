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
using XamarinFirebase.Views;
using XamarinFirebaseAndroid;
using Entities;
using Square.Picasso;

namespace XamarinFirebaseAndroid.Adapter

{
    public class ItemRecyclerAdapter : RecyclerView.Adapter
    {
        private Activity _activity;
        private List<DocumentItem> _items;

        public ItemRecyclerAdapter(Activity activity, List<DocumentItem> items)
        {
            _items = items;
            _activity = activity;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            //Setup and inflate your layout here
            var id = Resource.Layout.item_view;
            var itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

            return new ItemView(itemView);
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = _items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as ItemView;
            //holder.Caption.Text = item;
            Picasso.With(_activity).Load(item.documentImageUrl).Into(holder.Image);
        }

        public override int ItemCount => _items.Count;
    }
}