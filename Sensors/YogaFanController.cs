using System;
using global::FanControl.Plugins;

namespace FanControl.LenovoYoga;

internal sealed class YogaFanController : IPluginControlSensor2
{
    private const int MaxRpm = 6000;

    private readonly LenovoWmi _wmi;

    public string Id { get; }

    public string Name { get; }

    public string PairedFanSensorId { get; }

    public float? Value { get; private set; }

    public YogaFanController(
        string id,
        string name,
        string pairedFanSensorId,
        LenovoWmi wmi)
    {
        Id = id;
        Name = name;
        PairedFanSensorId = pairedFanSensorId;
        _wmi = wmi;
    }

    public void Update()
    {
    }

    public void Set(float val)
    {
        if (float.IsNaN(val) || float.IsInfinity(val))
        {
            return;
        }

        val = Math.Clamp(val, 0f, 100f);

        int rpm = (int)Math.Round(
            val * (MaxRpm / 100f));

        rpm = Math.Clamp(rpm, 0, MaxRpm);

        _wmi.SetFanRpm(rpm);

        Value = val;
    }

    public void Reset()
    {
        Value = null;
    }
}
