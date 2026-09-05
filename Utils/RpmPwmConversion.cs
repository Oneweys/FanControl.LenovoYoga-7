using System;

namespace FanControl.LenovoYoga;

internal static class RpmPwmConversion
{
    public const int MinRpm = 1500;
    public const int MaxRpm = 6000;

    public static int PwmToRpm(float pwm)
    {
        pwm = Math.Clamp(pwm, 0f, 100f);

        if (pwm <= 0f)
        {
            return 0;
        }

        return (int)Math.Round(
            MinRpm +
            ((MaxRpm - MinRpm) * (pwm / 100f)));
    }

    public static float RpmToPwm(float rpm)
    {
        rpm = Math.Clamp(rpm, 0f, MaxRpm);

        if (rpm <= 0f)
        {
            return 0f;
        }

        if (rpm <= MinRpm)
        {
            return 0f;
        }

        return ((rpm - MinRpm) / (MaxRpm - MinRpm)) * 100f;
    }
}
