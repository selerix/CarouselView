using System;
using System.IO;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Demo
{
    public class App : Application
    {
        public App()
        {
            // The root page of your application
            MainPage = new NavigationPage(new MainPage());

            //MainPage = new NavigationPage(new MyTabbedPage());

            //MainPage = new NavigationPage(new LeakPage());

            //MainPage = new RootPage();

            //MainPage = new NavigationPage(new HeightIssuePage ());

            //MainPage = new NavigationPage(new Pages.ListViewCarousel());

            //MainPage = new NavigationPage(new Bug129());
            //MainPage = new NavigationPage(new Bug168());

            //Alert objAlert = Alert.CreateDummyAlert();
            //MainPage = new NavigationPage(new Pages.Bug169(objAlert));
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

    [ContentProperty("Source")]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }

            var imageSource = ImageSource.FromStream(() =>
            {
                var assembly = typeof(App).GetTypeInfo().Assembly;
                var ms = new MemoryStream();
                using (var stream = assembly.GetManifestResourceStream(Source))
                {
                    stream.CopyTo(ms);
                }
                ms.Seek(0, SeekOrigin.Begin);
                return ms;
            });

            return imageSource;
        }
    }
}

