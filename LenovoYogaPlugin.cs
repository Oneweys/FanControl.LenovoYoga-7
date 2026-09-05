using global::FanControl.Plugins;

namespace FanControl.LenovoYoga;

public sealed class LenovoYogaPlugin : IPlugin2
{
    private const string FanSensorId = "LenovoYoga.Fan.RPM";
    private const string FanControlId = "LenovoYoga.Fan.Control";

    private LenovoWmi? _wmi;
    private YogaFanSensor? _fanSensor;
    private YogaFanController? _fanController;

    public string Name => "Lenovo Yoga Fan";

    public void Initialize()
    {
        if (!LenovoWmi.IsSupportedYoga())
        {
            return;
        }

        _wmi = new LenovoWmi();
    }

    public void Load(IPluginSensorsContainer container)
    {
        if (_wmi is null)
        {
            return;
        }

        _fanSensor = new YogaFanSensor(
            FanSensorId,
            "Yoga Fan",
            _wmi);

        _fanController = new YogaFanController(
            FanControlId,
            "Yoga Fan Control",
            FanSensorId,
            _wmi);

        container.FanSensors.Add(_fanSensor);
        container.ControlSensors.Add(_fanController);
    }

    public void Update()
    {
        _fanSensor?.Update();
    }

    public void Close()
    {
        _fanSensor = null;
        _fanController = null;

        _wmi?.Dispose();
        _wmi = null;
    }
}

internal sealed class YogaFanSensor : IPluginSensor
{
    private readonly LenovoWmi _wmi;

    public string Id { get; }

    public string Name { get; }

    public float? Value { get; private set; }

    public YogaFanSensor(
        string id,
        string name,
        LenovoWmi wmi)
    {
        Id = id;
        Name = name;
        _wmi = wmi;

        // IMPORTANT:
        // Read the RPM immediately so the sensor has a valid value
        // before FanControl starts its automatic pairing test.
        Update();
    }
    public void Update()
    {
        try
        {
            Value = _wmi.GetFanRpm();
        }
        catch (Exception ex)
        {
            System.IO.File.AppendAllText(
                @"C:\Temp\yogafan_debug.log",
                $"{DateTime.Now:HH:mm:ss.fff} Update failed: {ex}\n");
        }
    }
}