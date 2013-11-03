using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using MobileInspection.Core;
using System.Linq;
using MobileInspection.Core.Core;
using MobileInspection.Core.Dtos;

namespace MobileInspectionXamarin
{
    [Activity(Label = "Mobile Inspections", MainLauncher = true, Icon = "@drawable/icon")]
    public class InspectionListing : Activity
    {
        private ListView listView;
        private List<InspectionListingDto> Inspections;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Listing);
            listView = FindViewById<ListView>(Resource.InspectionListing.inspectionList);
            listView.ItemClick += listView_ItemClick;
            CreateStartButton();
        }

        void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var inspectionToEdit = Inspections[e.Position];
            
            var inspectionIntent = new Intent(this, typeof(Inspection));
            inspectionIntent.PutExtra("isEdit", true);
            inspectionIntent.PutExtra("IdOfInspectionToEdit", inspectionToEdit.Id);
            StartActivity(inspectionIntent);  
        }

        protected override void OnResume()
        {
            base.OnResume();
            Inspections = new InspectionTask().GetInspectionForListing();
            var listAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, Inspections.Select(
                x => x.Title).ToArray());
            listView.Adapter = listAdapter;
        }

        private void CreateStartButton()
        {
            var startButton = FindViewById<Button>(Resource.InspectionListing.StartInspection);
            startButton.Click += (o, e) => { StartActivity(typeof (Inspection)); };
        }
    }
}

