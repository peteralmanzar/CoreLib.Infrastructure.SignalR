using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CoreLib.Infrastructure.SignalR
{
    public abstract class baseSignalRSelfHostedService
    {
        #region Properties
        public string[] URLs { get; }
        #endregion

        #region Members
        protected IWebHost _webHost;

        protected virtual string _defaultURL => "http://localhost:8080";
        #endregion

        #region Events
        public event EventHandler Started;
        public event EventHandler Stopped;
        #endregion

        #region Constructors
        public baseSignalRSelfHostedService(params string[] urls)
        {
            #region Guards
            if (urls == null || urls.Length < 1) urls = new string[] { _defaultURL };
            #endregion

            URLs = urls;
        }
        #endregion

        #region Public Functions
        public void Start()
        {
            _webHost = WebHost.CreateDefaultBuilder()
                .ConfigureServices(addSignalRSevice)
                .ConfigureServices(registerDependencies)
                .Configure(configureSignalR)
                .Start(URLs);
            Started?.Invoke(this, EventArgs.Empty);
        }

        public void Stop()
        {
            _webHost.StopAsync();
            Stopped?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Private Functions
        private void addSignalRSevice(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSignalR();
        }

        protected abstract void registerDependencies(IServiceCollection serviceCollection);

        private void configureSignalR(IApplicationBuilder app)
        {
            #region Guards
            if (app == null) throw new ArgumentNullException(nameof(app));
            #endregion

            app.UseSignalR(mapHubs);
        }

        protected abstract void mapHubs(HubRouteBuilder routes);
        #endregion

    }
}
