namespace Verbum
{
    public class App : Application
    {
        protected Window? MainWindow { get; private set; }
        protected IHost? Host { get; private set; }

        protected async override void OnLaunched(LaunchActivatedEventArgs args)
        {
            var builder = this.CreateBuilder(args)
                // Add navigation support for toolkit controls such as TabBar and NavigationView
                .UseToolkitNavigation()
                .Configure(host => host
#if DEBUG
				// Switch to Development environment when running in DEBUG
				.UseEnvironment(Environments.Development)
#endif
                    .ConfigureServices((context, services) =>
                    {
                        // TODO: Register your services
                        //services.AddSingleton<IMyService, MyService>();
                    })
                    .UseNavigation(ReactiveViewModelMappings.ViewModelMappings, RegisterRoutes)
                );
            MainWindow = builder.Window;

            Host = await builder.NavigateAsync<Shell>();
        }

        private static void RegisterRoutes(IViewRegistry views, IRouteRegistry routes)
        {
            views.Register(
                new ViewMap(ViewModel: typeof(ShellModel)),
                new ViewMap<MainPage, MainModel>(),
                new DataViewMap<SecondPage, SecondModel, Entity>()
            );

            routes.Register(
                new RouteMap("", View: views.FindByViewModel<ShellModel>(),
                    Nested: new RouteMap[]
                    {
                    new RouteMap("Main", View: views.FindByViewModel<MainModel>()),
                    new RouteMap("Second", View: views.FindByViewModel<SecondModel>()),
                    }
                )
            );
        }
    }
}