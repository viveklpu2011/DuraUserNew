using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;
using DuraApp.Core.Helpers;
using NewDuraApp.Helpers;
using Plugin.Multilingual;
using Xamarin.Forms.Xaml;

namespace ShriekSecurity.Helpers
{
	public class TranslateExtension : IMarkupExtension
	{
		const string ResourceId = "NewDuraApp.Resources.AppResources";

		static readonly Lazy<ResourceManager> resmgr =
			new Lazy<ResourceManager>(() => new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly));

		public string Text { get; set; }

		public object ProvideValue(IServiceProvider serviceProvider)
		{
			if (Text == null)
				return "";
			CultureInfo ci = null;
			if (SettingsExtension.CurrentUserCulture != null)
				Thread.CurrentThread.CurrentUICulture = GetCultureFromLanguage.GetCulture(SettingsExtension.CurrentUserCulture);
			else
				ci = CrossMultilingual.Current.CurrentCultureInfo;

			var translation = resmgr.Value.GetString(Text, ci);

			if (translation == null) {

#if DEBUG
				throw new ArgumentException(
					String.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, ResourceId, ci.Name),
					"Text");
#else
                translation = Text; // returns the key, which GETS DISPLAYED TO THE USER  
#endif
			}
			return translation;
		}
	}
}
