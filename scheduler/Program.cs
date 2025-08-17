// See https://aka.ms/new-console-template for more information


using OpsWatcher.Scheduler;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<ScheduleHostedService>();

var host = builder.Build();
host.Run();