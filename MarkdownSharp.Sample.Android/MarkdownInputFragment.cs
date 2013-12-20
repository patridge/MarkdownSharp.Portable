using System;
using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace MarkdownSharp.Sample.Android
{
    public class MarkdownChangedEventArgs : EventArgs {
        public string RawMarkdown { get; set; }
    }
    public class MarkdownInputFragment : Fragment
    {
        public event EventHandler<MarkdownChangedEventArgs> MarkdownChanged = delegate { };
        EditText markdownInput;
        public string CurrentMarkdown {
            get {
                return markdownInput.Text;
            }
        }

        string incomingMarkdown;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            RestoreSavedInstanceState(savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragment_input, container, false);
            markdownInput = view.FindViewById<EditText>(Resource.Id.inputEditText);
            markdownInput.Text = incomingMarkdown;
            return view;
        }
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            RestoreSavedInstanceState(savedInstanceState);
        }

        void MarkdownInput_KeyPress(object sender, View.KeyEventArgs e) {
            // Always let input go to EditText view.
            e.Handled = false;

            // TODO: Trigger on every KeyEventActions.Up and throttle consumption of change event (e.g., with Rx).
            if (e.Event.Action == KeyEventActions.Up && e.KeyCode == Keycode.Enter) {
                // MarkdownChanged currently only triggered after a new line.
                MarkdownChanged(this, new MarkdownChangedEventArgs() { RawMarkdown = markdownInput.Text });
            }
        }

        void RestoreSavedInstanceState(Bundle savedInstanceState) {
            if (savedInstanceState != null)
            {
                // Save value until it can be used in OnCreateView.
                incomingMarkdown = savedInstanceState.GetString("current_markdown");
            }
        }
        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutString("current_markdown", markdownInput.Text);
        }
        public override void OnResume()
        {
            base.OnResume();
            markdownInput.KeyPress += MarkdownInput_KeyPress;
        }
        public override void OnPause()
        {
            base.OnPause();
            markdownInput.KeyPress -= MarkdownInput_KeyPress;
        }
    }
}