using Microsoft.AspNetCore.SignalR.Client;
using SharedClassLibrary;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PlayerSide.Models
{
    public class TestViewModel : INotifyPropertyChanged
    {
        private HubConnection hubConnection;

        public ICommand SendMessageCommand { get; set; }

        public bool IsConnected =>
            hubConnection.State == HubConnectionState.Connected;

        #region MessageProperties
        public string UserName { get; set; } = " ";
        public string SheetId { get; set; } = " ";
        public string LastModified { get; set; } = " ";
        public string SessionKey { get; set; } = " ";

        public string[] MessagePropertyList =
        {
            nameof(UserName),
            nameof(SheetId),
            nameof(LastModified),
            nameof(SessionKey),
        };
        #endregion

        public TestViewModel()
        {
        }

        public async Task Initialise()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl($"{Constants.RestUrl}/filehub", options => options.Headers.Add("Authorization", $"Basic {Globals.RService.AuthHeader}"))
                .Build();

            hubConnection.On<FileUpdateMessage>("ReceiveMessage", msg =>
            {
                try
                {
                    Console.WriteLine($"Received message {msg.LastModified}");
                    Console.WriteLine($"From {msg.UserName}");
                    foreach (string m in MessagePropertyList)
                    {
                        OnPropertyChanged(m);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to receive message");
                    Console.WriteLine(ex.Message);
                }
            });

            await hubConnection.StartAsync();
            await Send();
        }

        public async Task Send()
        {
            FileUpdateMessage msg = new()
            {
                UserName = UserName,
                SheetId = SheetId,
                SessionKey = SessionKey,
                LastModified = LastModified
            };

            Console.WriteLine($"Sending message for user {UserName}");

            await hubConnection.SendAsync("SendMessage", msg);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}