namespace OpsWatcher.Notifier;

using Telegram.Bot;

public interface INotifier
{
    Task NotifyAsync(string message);
}

public class TelegramNotifier: INotifier
{
    private readonly TelegramBotClient _bot = new("");

    public async Task NotifyAsync(string message)
    {
        await _bot.SendMessage("", "Hello, World!");
    }
}