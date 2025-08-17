// See https://aka.ms/new-console-template for more information
using OpsWatcher.Notifier;


var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<INotifier, TelegramNotifier>();
builder.Services.AddHostedService<NotifierHostedService>();

var host = builder.Build();
host.Run();
