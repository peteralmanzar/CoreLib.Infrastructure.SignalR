using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace CoreLib.Infrastructure.SignalR
{
    public abstract class baseSignalRClientService
    {
        #region Properties
        public string URL { get; private set; }
        #endregion

        #region Members
        protected HubConnection _connection { get; set; }
        #endregion

        #region Events
        public event EventHandler Connected;
        protected virtual void OnConnected()
        {
            //TODO: need to implement OnConnected
            Connected?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler Disconnected;
        protected virtual void OnDisconnected()
        {
            //TODO: need to implement OnDisconnected
            Disconnected?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Constructors
        public baseSignalRClientService(string url)
        {
            URL = url;

            initConnection();
            subscribeToServerRequests();
        }
        #endregion

        #region Public Functions
        public virtual async Task Connect()
        {
            //TODO: RignalR attempt to reconnect on disconnects
            await _connection.StartAsync();
        }
        public virtual async Task Disconnect()
        {
            await _connection.StopAsync();
        }

        public void Dispose()
        {
            Disconnect().GetAwaiter().GetResult();
        }
        #endregion

        #region Private Functions
        private void initConnection()
        {
            _connection = new HubConnectionBuilder().WithUrl(URL).Build();
            _connection.Closed += _connection_Closed;
        }

        private Task _connection_Closed(Exception arg)
        {
            OnDisconnected();
            return Task.CompletedTask;
        }

        protected virtual void subscribeToServerRequests()
        {
            #region Guards
            if (_connection == null) throw new ArgumentNullException(nameof(_connection));
            #endregion

            _connection.On(nameof(OnConnected), OnConnected);
            _connection.On(nameof(OnDisconnected), OnDisconnected);
        }

        protected static void subscribeToCustomServerRequest(HubConnection connection, string methodName, Action action)
        {
            #region Guards
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (string.IsNullOrEmpty(methodName)) throw new ArgumentNullException(nameof(methodName));
            if (action == null) throw new ArgumentNullException(nameof(action));
            #endregion

            connection.On(methodName, action);
        }
        protected static void subscribeToCustomServerRequest<T>(HubConnection connection, string methodName, Action<T> action)
        {
            #region Guards
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (string.IsNullOrEmpty(methodName)) throw new ArgumentNullException(nameof(methodName));
            if (action == null) throw new ArgumentNullException(nameof(action));
            #endregion

            connection.On(methodName, action);
        }

        protected static async Task tryConnect(HubConnection connection)
        {
            #region Guards
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            #endregion

            if (connection.State != HubConnectionState.Connected) await connection.StartAsync();
        }
        #endregion
    }
}
