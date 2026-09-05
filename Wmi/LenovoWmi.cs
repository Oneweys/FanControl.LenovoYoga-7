using System;
using System.Management;

namespace FanControl.LenovoYoga;

internal sealed class LenovoWmi : IDisposable
{
    private const string WmiNamespace = @"root\WMI";
    private const string WmiClass = "LENOVO_OTHER_METHOD";

    // Feature Lenovo découvert et validé sur le Yoga 7 2-in-1 16AGP11.
    private const int FanFeatureId = unchecked((int)0x04030001);

    private const int MaxRpm = 6000;

    private ManagementObject? _methodObject;

    public LenovoWmi()
    {
        _methodObject = FindMethodObject();

        if (_methodObject == null)
        {
            throw new InvalidOperationException(
                "LENOVO_OTHER_METHOD was not found or is not accessible.");
        }
    }

    public int GetFanRpm()
    {
        EnsureConnected();

        using ManagementBaseObject inParams =
            _methodObject!.GetMethodParameters("GetFeatureValue");
        inParams["IDs"] = FanFeatureId;

        using ManagementBaseObject? outParams =
            _methodObject.InvokeMethod("GetFeatureValue", inParams, null);

        if (outParams == null)
        {
            throw new InvalidOperationException(
                "GetFeatureValue returned null.");
        }

        object? value = outParams["value"];

        if (value == null)
        {
            throw new InvalidOperationException(
                "GetFeatureValue returned no 'value' field.");
        }

        return Convert.ToInt32(value);
    }

    public void SetFanRpm(int rpm)
    {
        EnsureConnected();

        rpm = Math.Clamp(rpm, 0, MaxRpm);

        using ManagementBaseObject inParams =
            _methodObject!.GetMethodParameters("SetFeatureValue");
        inParams["IDs"] = FanFeatureId;
        inParams["value"] = (uint)rpm;

        _methodObject.InvokeMethod("SetFeatureValue", inParams, null);
    }

    private static ManagementObject? FindMethodObject()
    {
        using var searcher = new ManagementObjectSearcher(
            WmiNamespace,
            $"SELECT * FROM {WmiClass}");

        using ManagementObjectCollection collection = searcher.Get();

        foreach (ManagementObject obj in collection)
        {
            try
            {
                if (obj["Active"] is bool active && !active)
                {
                    obj.Dispose();
                    continue;
                }
            }
            catch
            {
                // Si Active n'est pas accessible, on essaie quand même l'objet.
            }

            obj.Get();

            return obj;
        }

        return null;
    }

    private void EnsureConnected()
    {
        if (_methodObject == null)
        {
            throw new ObjectDisposedException(nameof(LenovoWmi));
        }

        try
        {
            _methodObject.Get();
        }
        catch
        {
            _methodObject.Dispose();

            _methodObject = FindMethodObject();

            if (_methodObject == null)
            {
                throw new InvalidOperationException(
                    "LENOVO_OTHER_METHOD is no longer accessible.");
            }
        }
    }

    public static bool IsSupportedYoga()
    {
        try
        {
            using var searcher = new ManagementObjectSearcher(
                @"root\CIMV2",
                "SELECT Manufacturer, Model, SystemSKUNumber " +
                "FROM Win32_ComputerSystem");

            using ManagementObjectCollection collection = searcher.Get();

            foreach (ManagementObject obj in collection)
            {
                string manufacturer =
                    Convert.ToString(obj["Manufacturer"]) ?? string.Empty;

                string model =
                    Convert.ToString(obj["Model"]) ?? string.Empty;

                string sku =
                    Convert.ToString(obj["SystemSKUNumber"]) ?? string.Empty;

                bool isLenovo =
                    manufacturer.Contains(
                        "LENOVO",
                        StringComparison.OrdinalIgnoreCase);

                bool isSupportedModel =
                    model.Contains(
                        "83TF",
                        StringComparison.OrdinalIgnoreCase)
                    ||
                    sku.Contains(
                        "Yoga 7 2-in-1 16AGP11",
                        StringComparison.OrdinalIgnoreCase);

                return isLenovo && isSupportedModel;
            }
        }
        catch
        {
            return false;
        }

        return false;
    }

    public void Dispose()
    {
        _methodObject?.Dispose();
        _methodObject = null;
    }
}
