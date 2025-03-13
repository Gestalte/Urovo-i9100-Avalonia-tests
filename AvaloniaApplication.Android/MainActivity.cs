using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Device;
using Android.Runtime;
using Avalonia;
using Avalonia.Android;
using Avalonia.ReactiveUI;
using Common;

namespace AvaloniaApplication.Android;

[Activity(
    Label = "AvaloniaApplication.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        ScanHelper.StartScanning = StartDecode;
        ScanHelper.StopScanning = StopDecode;

        return base.CustomizeAppBuilder(builder)
            .WithInterFont()
            .UseReactiveUI();
    }

    private ScanManager? scanManager;

    protected override void OnResume()
    {
        System.Diagnostics.Debug.WriteLine(nameof(OnResume), "[TRACE]");

        try
        {
            this.scanManager = new ScanManager();
            if (!this.scanManager.ScannerState)
            {
                bool b = this.scanManager.OpenScanner();
                System.Diagnostics.Debug.WriteLine($"Scanner is {(b ? "ON" : "OFF")}", "[TRACE]");
            }
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(nameof(OnResume), "[ERROR]");
            System.Diagnostics.Debug.WriteLine(ex, "[ERROR]");
        }

        base.OnResume();
    }

    protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent? data)
    {
        System.Diagnostics.Debug.WriteLine(nameof(OnActivityResult), "[TRACE]");

        string? scanResult = data?.GetStringExtra("barcode_string");
        System.Diagnostics.Debug.WriteLineIf(scanResult is not null, $"Scan result: {scanResult}", "[TRACE]");
        ScanHelper.ScanResult?.Invoke(scanResult);

        base.OnActivityResult(requestCode, resultCode, data);
    }

    private void StartDecode()
    {
        System.Diagnostics.Debug.WriteLine(nameof(StartDecode), "[TRACE]");

        try
        {
            if (this.scanManager is null)
            {
                this.scanManager = new ScanManager();
                bool b = this.scanManager.OpenScanner();
                System.Diagnostics.Debug.WriteLine($"Scanner is {(b ? "ON" : "OFF")}", "[TRACE]");
            }

            bool result = this.scanManager?.StartDecode() ?? false;
            System.Diagnostics.Debug.WriteLine($"{nameof(StartDecode)} is {result}", "[TRACE]");
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(nameof(StartDecode), "[ERROR]");
            System.Diagnostics.Debug.WriteLine(ex, "[ERROR]");
        }
    }

    private void StopDecode()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine(nameof(StopDecode), "[TRACE]");
            bool result = this.scanManager?.StopDecode() ?? false;
            System.Diagnostics.Debug.WriteLine($"{nameof(StopDecode)} is {result}", "[TRACE]");
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(nameof(StopDecode), "[ERROR]");
            System.Diagnostics.Debug.WriteLine(ex, "[ERROR]");
        }
    }
}
