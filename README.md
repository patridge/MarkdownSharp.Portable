MarkdownSharp.Portable
======================

Fork of MarkdownSharp with a focus on making it a portable class library (PCL).

## Why a fork?

I really don't have a good reason to fork except that it looks like making this portable would break some existing functionality and/or execution expectations. I would happily submit this to the original project and point everyone hitting this repo back over there if it is ever the desired direction.

The original [MarkdownSharp](https://code.google.com/p/markdownsharp/) had a couple items that kept it from being usable in a portable class library or being referenced by a Xamarin.iOS/Xamarin.Android project. First, it offered a constructor that pulled settings from `ConfigurationManager.AppSettings` (via the `System.Configuration` namespace). Second, it used `RegexOptions.Compiled` for most regexes. This fork removes the configuration-settings constructor and set all uses of `RegexOptions.Compiled` to `RegexOptions.None`.

## Need compiled regexes?

When switching to `RegexOptions.None`, the setting was extracted to a central variable in the `Markdown` class. If you want to test compilation on a platform that supports it; go find that variable, switch it to `Compiled`, and rebuild.

  private static RegexOptions _defaultRegexOptions = RegexOptions.Compiled;
  
Another option may be to [use pre-compiled regexes](http://msdn.microsoft.com/en-us/magazine/cc163670.aspx#S7), but I haven't poked at how that plays with portability yet.

## Need the original `loadOptionsFromConfigFile` (`ConfigurationManager.AppSettings`) constructor?

Since configuration can still be done by passing in a instance of `MarkdownOptions`, you will need to do the logic the old constructor did for you before creating a new instance of `Markdown`.

    var options = new MarkdownOptions();
    var appSettings = ConfigurationManager.AppSettings;
    foreach (string key in appSettings.Keys)
    {
        switch (key)
        {
            case "Markdown.AutoHyperlink":
                options.AutoHyperlink = Convert.ToBoolean(settings[key]);
                break;
            case "Markdown.AutoNewlines":
                options.AutoNewlines = Convert.ToBoolean(settings[key]);
                break;
            case "Markdown.EmptyElementSuffix":
                options.EmptyElementSuffix = settings[key];
                break;
            case "Markdown.EncodeProblemUrlCharacters":
                options.EncodeProblemUrlCharacters = Convert.ToBoolean(settings[key]);
                break;
            case "Markdown.LinkEmails":
                options.LinkEmails = Convert.ToBoolean(settings[key]);
                break;
            case "Markdown.StrictBoldItalic":
                options.StrictBoldItalic = Convert.ToBoolean(settings[key]);
                break;
        }
    }
    var markdown = new Markdown(options);
