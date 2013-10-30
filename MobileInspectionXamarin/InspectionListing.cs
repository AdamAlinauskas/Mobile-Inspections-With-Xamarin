using Android.App;
using Android.Widget;
using Android.OS;
using MobileInspection.Core;
using System.Linq;
using MobileInspection.Core.Core;

namespace MobileInspectionXamarin
{
    [Activity(Label = "Mobile Inspections", MainLauncher = true, Icon = "@drawable/icon")]
    public class InspectionListing : Activity
    {
        private ListView listView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Listing);
            listView = FindViewById<ListView>(Resource.InspectionListing.inspectionList);
            CreateStartButton();
        }

        protected override void OnResume()
        {
            base.OnResume();
            var items = new InspectionTask().GetInspectionNames();
            var listAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, items);
            listView.Adapter = listAdapter;
        }

        private void CreateStartButton()
        {
            var startButton = FindViewById<Button>(Resource.InspectionListing.StartInspection);
            startButton.Click += (o, e) => { StartActivity(typeof (Inspection)); };
        }
    }
}

