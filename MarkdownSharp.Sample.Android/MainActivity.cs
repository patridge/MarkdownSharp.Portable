using Android.OS;
using Android.Support.V4.App;
using Xamarin.ActionbarSherlockBinding.App;
using Activity = Android.App.ActivityAttribute;
using Tab = Xamarin.ActionbarSherlockBinding.App.ActionBar.Tab;
using SherlockActionBar = Xamarin.ActionbarSherlockBinding.App.ActionBar;
using Debug = System.Diagnostics.Debug;

namespace MarkdownSharp.Sample.Android
{
    [Activity(Icon = "@drawable/icon", Label = "@string/input_label", MainLauncher = true, Theme = "@android:style/Theme.NoTitleBar")]
    public class MainActivity : SherlockFragmentActivity, SherlockActionBar.ITabListener
    {
        Tab inputTab;
        Tab previewTab;
        MarkdownInputFragment inputFragment;
        MarkdownPreviewFragment previewFragment;

        protected override void OnCreate(Bundle bundle)
        {
            SetTheme(Resource.Style.Theme_Sherlock);
            SupportActionBar.SetDisplayShowHomeEnabled(false);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetDisplayUseLogoEnabled(false);
            SupportActionBar.NavigationMode = SherlockActionBar.NavigationModeTabs;
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.activity_main);

            inputFragment = (MarkdownInputFragment)SupportFragmentManager.FindFragmentByTag("input");
            previewFragment = (MarkdownPreviewFragment)SupportFragmentManager.FindFragmentByTag("preview");
            if (inputFragment == null || previewFragment == null) {
                // Only add fragments the first time.
                inputFragment = new MarkdownInputFragment();
                previewFragment = new MarkdownPreviewFragment();
                inputFragment.MarkdownChanged += (sender, e) => {
                    previewFragment.UpdatePreview(e.RawMarkdown);
                };
                SupportFragmentManager.BeginTransaction()
                    .Add(Resource.Id.fragmentContainer, previewFragment, "preview")
                    .Hide(previewFragment)
                    .Add(Resource.Id.fragmentContainer, inputFragment, "input")
                    .Commit();
            }

            // NOTE: Doesn't maintain previously selected tab on activity restart (e.g., rotate).
            inputTab = SupportActionBar.NewTab()
                .SetText(Resource.String.input_label)
                .SetTabListener(this);
            SupportActionBar.AddTab(inputTab);
            previewTab = SupportActionBar.NewTab()
                .SetText(Resource.String.preview_label)
				.SetTabListener(this);
            SupportActionBar.AddTab(previewTab);
        }

        public void OnTabSelected(SherlockActionBar.Tab tab, FragmentTransaction ft)
        {
            if (tab == inputTab) {
                ft.Hide(previewFragment);
                ft.Show(inputFragment);
            } else if (tab == previewTab) {
                // Get latest markdown. Only needed because MarkdownChanged not triggered until new line.
                previewFragment.UpdatePreview(inputFragment.CurrentMarkdown);
                ft.Hide(inputFragment);
                ft.Show(previewFragment);
            }
        }

        public void OnTabReselected(SherlockActionBar.Tab tab, FragmentTransaction ft) { }
        public void OnTabUnselected(SherlockActionBar.Tab tab, FragmentTransaction ft) { }
    }
}