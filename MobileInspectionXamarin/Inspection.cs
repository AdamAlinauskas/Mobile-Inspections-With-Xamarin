using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Widget;
using Java.IO;
using MobileInspection.Core.Core;
using Environment = Android.OS.Environment;

namespace MobileInspectionXamarin
{
    [Activity(Label = "Inspection")]
    public class Inspection : Activity
    {
        private ImageView imageView;
        File file;
        File dir;
        private bool isEdit;
        private string idOfInspectionToEdit;
        private TextView title;
        private TextView location;
        private Button ok;
        private Button cancel;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Inspection);
            RetriveUiControls();
            WireUpSaveButton();
            WireUpCancel();
            SetUpCamera();
            SetupEditMode();
        }

        private void RetriveUiControls()
        {
            title = FindViewById<TextView>(Resource.Inspection.title);
            location = FindViewById<TextView>(Resource.Inspection.location);
            imageView = FindViewById<ImageView>(Resource.Inspection.imageView1);
            ok = FindViewById<Button>(Resource.Inspection.ok);
            cancel = FindViewById<Button>(Resource.Inspection.cancel);
        }

        private void SetupEditMode()
        {
            isEdit = Intent.GetBooleanExtra("isEdit", false);
            idOfInspectionToEdit = Intent.GetStringExtra("idOfInspectionToEdit");
            if (isEdit)
            {
                var inspectionToEdit = new InspectionTask().GetInspectionBy(idOfInspectionToEdit);
                title.Text = inspectionToEdit.Title;
                location.Text = inspectionToEdit.Location;
                file = new File(inspectionToEdit.FilePath);
                DisplayImage();
            }
        }

        private void SetUpCamera()
        {
            if (IsThereAnAppToTakePictures())
            {
               
                CreateDirectoryForPictures();
                SetTakePictureButton();
            }
        }

        private void WireUpCancel()
        {
            cancel.Click += (o, e) => Finish();
        }

        private void WireUpSaveButton()
        {
            ok.Click += (o, e) => { new InspectionTask().Save(title.Text, location.Text,GetFilePath());Finish();};
        }

        private string GetFilePath()
        {
            return file != null ? file.AbsolutePath : string.Empty;
        }

        private void SetTakePictureButton()
        {
            var takePicture = FindViewById<Button>(Resource.Inspection.takePicture);
           
            takePicture.Click += TakeAPicture;
        }

        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities = PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        private void CreateDirectoryForPictures()
        {
            dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "!Inspection photos");
            if (!dir.Exists())
            {
                dir.Mkdirs();
            }
        }

        private void TakeAPicture(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);

            file = new File(dir, String.Format("inspection_{0}.jpg", Guid.NewGuid()));

            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(file));

            StartActivityForResult(intent, 0);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (resultCode == Result.Canceled)
                return;

            base.OnActivityResult(requestCode, resultCode, data);

            // make it available in the gallery
            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            var contentUri = Android.Net.Uri.FromFile(file);
            mediaScanIntent.SetData(contentUri);
            SendBroadcast(mediaScanIntent);

            // display in ImageView. We will resize the bitmap to fit the display
            // Loading the full sized image will consume to much memory 
            // and cause the application to crash.
            DisplayImage();
        }

        private void DisplayImage()
        {
            int height = imageView.Height;
            int width = Resources.DisplayMetrics.WidthPixels;
            using (Bitmap bitmap = file.Path.LoadAndResizeBitmap(width, height))
            {
                imageView.SetImageBitmap(bitmap);
            }
        }
    }
}