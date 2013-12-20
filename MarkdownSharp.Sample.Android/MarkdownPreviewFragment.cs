using System;
using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Webkit;

namespace MarkdownSharp.Sample.Android
{
	public class MarkdownPreviewFragment : Fragment
	{
		readonly Markdown markdown = new Markdown();
		string markdownHtml;
		WebView webView;

		public void UpdatePreview(string rawMarkdown)
		{
			markdownHtml = markdown.Transform(rawMarkdown);
			if (webView != null) {
				webView.LoadData(markdownHtml, "text/html", null);
			}
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
            var view = inflater.Inflate(Resource.Layout.fragment_preview, container, false);
            webView = view.FindViewById<WebView>(Resource.Id.previewWebView);
            if (savedInstanceState != null)
            {
                string markdownInput = savedInstanceState.GetString("current_markdown");
                UpdatePreview(markdownInput);
            }
			return view;
		}
	}
}