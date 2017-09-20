﻿using System;
using System.Collections.Generic;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Droid.Views.Attributes;
using SharedElement.Official.Core.ViewModels;
using SharedElement.Official.Droid.Adapters;

namespace SharedElement.Official.Droid.Views
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register(DroidConstants.SharedElement_Views_Namespace + nameof(ListFragment))]
    public class ListFragment : MvxFragment<ListViewModel>, IMvxOverridePresentationAttribute
    {
        public MvxBasePresentationAttribute PresentationAttribute()
        {
            return new MvxFragmentPresentationAttribute(typeof(MainViewModel), Resource.Id.content_frame)
            {

            };
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.fragment_list, null);

            var recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.my_recycler_view);
            if (recyclerView != null)
            {
                recyclerView.HasFixedSize = true;
                var layoutManager = new LinearLayoutManager(Activity);
                recyclerView.SetLayoutManager(layoutManager);

                var adapter = new SelectedItemRecyclerAdapter(BindingContext as IMvxAndroidBindingContext);
                adapter.OnItemClick += Adapter_OnItemClick;
                recyclerView.Adapter = adapter;
            }

            return view;
        }

        public override void OnResume()
        {
            base.OnResume();
            (Activity as AppCompatActivity)?.SupportActionBar.SetDisplayHomeAsUpEnabled(false);
        }

        private void Adapter_OnItemClick(object sender, SelectedItemRecyclerAdapter.SelectedItemEventArgs e)
        {
            Toast.MakeText(Activity, $"Selected item {e.Position + 1}", ToastLength.Short)
                .Show();

            if (Attribute.GetCustomAttribute(GetType(), typeof(MvxFragmentPresentationAttribute)) is MvxFragmentPresentationAttribute attribute)
            {
                if (attribute.SharedElements == null)
                    attribute.SharedElements = new Dictionary<string, View>();

                ImageView itemLogo = e.View.FindViewById<ImageView>(Resource.Id.img_logo);
                attribute.SharedElements.Add(Activity.Resources.GetString(Resource.String.transition_list_item_icon), itemLogo);

                TextView itemName = e.View.FindViewById<TextView>(Resource.Id.txt_name);
                attribute.SharedElements.Add(Activity.Resources.GetString(Resource.String.transition_list_item_name), itemName);
            }

            ViewModel.SelectItemExecution(e.DataContext as ListItemViewModel);
        }
    }
}
