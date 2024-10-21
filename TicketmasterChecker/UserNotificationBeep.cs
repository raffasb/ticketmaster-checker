namespace TicketmasterChecker;

internal class UserNotificationBeep : IUserNotification
{
    private readonly int _beepFrequencyInMilliseconds;
    private readonly int _beepDurationInMilliseconds;

    public UserNotificationBeep(int beepFrequencyInMilliseconds, int beepDurationInMilliseconds)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan<int>(beepFrequencyInMilliseconds, 37, nameof(beepFrequencyInMilliseconds));
        ArgumentOutOfRangeException.ThrowIfGreaterThan<int>(beepFrequencyInMilliseconds, 32_767, nameof(beepFrequencyInMilliseconds));

        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual<int>(beepDurationInMilliseconds, 0, nameof(beepDurationInMilliseconds));

        _beepFrequencyInMilliseconds = beepFrequencyInMilliseconds;
        _beepDurationInMilliseconds = beepDurationInMilliseconds;
    }

    public void Notify()
    {
        if (!OperatingSystem.IsWindows())
        {
            throw new PlatformNotSupportedException("Beep is only supported on Windows.");
        }

        Console.Beep(_beepFrequencyInMilliseconds, _beepDurationInMilliseconds);
    }
}
