using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;
using SharedClassLibrary;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
            SendMessageCommand = new Command(async () => await Send());
        }

        public async Task Initialise()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7055/filehub")
                .Build();

            hubConnection.On<FileUpdateMessage>("ReceiveMessage", msg =>
            {
                try
                {
                    Console.WriteLine($"Received message {msg.Message}");
                    Console.WriteLine($"From {msg.UserName}");
                    UpdateProperties(msg);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to receive message");
                    Console.WriteLine(ex.Message);
                }
            });

            await hubConnection.StartAsync();
        }

        public async Task Send()
        {
            var msg = new FileUpdateMessage
            {
                UserName = UserName,
                Message = Message,
                UserId = userId,
                AvatarId = avatarId
            };

            Console.WriteLine($"Sending message for user {userId}");

            await hubConnection.SendAsync("SendMessage", msg);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateProperties(FileUpdateMessage message)
        {
            Message4Text = message.Message;
            Message4Username = message.UserName;
            Message4Avatar = $"avatar{message.AvatarId}.png";
            Message4IsMe = message.UserId == userId;
            Message4IsNotMe = message.UserId != userId;

            MessagePropertyList.ForEach(m => OnPropertyChanged(m));

            Console.WriteLine("Added message:");
            Console.WriteLine($"From: {Message4Username}");
            Console.WriteLine($"Message: {Message4Text}");
            Console.WriteLine($"Is me: {Message4IsMe}");
        }
    }
}