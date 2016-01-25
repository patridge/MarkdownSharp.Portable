using System;

using Xamarin.Forms;

namespace MarkdownSharp.Sample.Forms
{
	public class App : Application
	{
		readonly Markdown markdown = new Markdown();
		string markdownHtml;
		public void UpdatePreview(string rawMarkdown)
		{
			markdownHtml = markdown.Transform(rawMarkdown);
			if (webView != null) {
				webView.Source = new HtmlWebViewSource() {
					Html = markdownHtml,
				};
			}
		}
		Editor markdownEditor;
		WebView webView;
		ContentPage markdownInputPage;
		ContentPage markdownPreviewPage;
		public App()
		{
			markdownEditor = new Editor() {
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};
			markdownEditor.TextChanged += MarkdownEditor_TextChanged;
			markdownInputPage = new ContentPage() {
				Title = "Markdown Input",
				Content = new StackLayout() {
					VerticalOptions = LayoutOptions.FillAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Padding = new OnPlatform<Thickness>() {
						iOS = new Thickness(0, 20, 0, 0),
					},
					Children = {
						markdownEditor,
					},
				},
			};
			webView = new WebView() {
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};
			markdownPreviewPage = new ContentPage() {
				Title = "HTML Preview",
				Content = new StackLayout() {
					VerticalOptions = LayoutOptions.FillAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Children = {
						webView,
					},
				},
			};
			var tabs = new TabbedPage() {
				Children = {
					markdownInputPage,
					markdownPreviewPage,
				},
			};
			MainPage = tabs;
		}

		void MarkdownEditor_TextChanged (object sender, TextChangedEventArgs e)
		{
			UpdatePreview(e.NewTextValue);
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}

