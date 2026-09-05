# FanControl LenovoYoga 7
This plugin allows you to control the fan of your **Lenovo Yoga 7**. Thanks to it, you can finally say goodbye to all the noise your computer was making.

## Supported Devices
The plugin is expected to work on Lenovo Yoga 7 models using the same Lenovo fan interface. _But the main tests were performed on the **16AGP11 (83TF)** model_. It should therefore work on other compatible Yoga 7 models.

## Requirements
- Obviously, a Lenovo Yoga 7
- Windows
- Fan Control available from https://getfancontrol.com/ or the [FanControl.Releases GitHub repository](https://github.com/Rem0o/FanControl.Releases)
- Run Fan Control as administrator to access the Lenovo WMI interface

## How it works?
This plugin uses Lenovo's **LENOVO_OTHER_METHOD** WMI interface to read and control the fan speed. Fan Control's existing temperature sensors can be used to create fan curves.

## Installation
1. Dowload the [lastest](https://github.com/Oneweys/FanControl.LenovoYoga-7/releases/latest/download/FanControl.LenovoYoga.zip), or browse all [Releases](https://github.com/Oneweys/FanControl.LenovoYoga-7/releases).
2. Extract the downloaded ZIP file.
3. Start Fan Control as **administrator**, click **Settings**, then **Plugins**, and select `FanControl.LenovoYoga.dll`.
4. Go back to **Home** and click **Calibrate**. Fan Control will automatically test the fan.
5. Enjoy the silence!

---
# Other Information

## Lenovo Background Services
The application **Lenovo Vantage**, which monitors the system and may initially control the fan, **does not** interfere with the plugin's fan control.

## Firmware Limitation
Due to the laptop's firmware, the fan may refuse to stop completely when the CPU/system temperature is already **around or above 50°C**. The fan can still be fully controlled, but under these conditions, the firmware imposes a minimum fan speed of around 2300–2700 RPM, even when 0% is requested. Once the temperature drops sufficiently, the fan can return to 0 RPM normally.

## Notes
The plugin has been tested on Windows on the Lenovo Yoga 7 2-in-1 16AGP11 (83TF). Other Yoga 7 models using the same Lenovo fan interface are expected to work, but have not been individually tested.

Development was assisted by AI tools for debugging and research.
