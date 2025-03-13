using Common;
using ReactiveUI;

namespace AvaloniaApplication.ViewModels;

public class MainViewModel : ViewModelBase
{
    private string? scanResult;
    public string? ScanResult
    {
        get => scanResult;
        set => this.RaiseAndSetIfChanged(ref scanResult, value);
    }

    private string scanButtonText= "Start scanning";
    public string ScanButtonText
    {
        get => scanButtonText;
        set => this.RaiseAndSetIfChanged(ref scanButtonText, value);
    }

    bool nowScanning = false;
    public void StartScan()
    {
        void scanResult(string? result)
        {
            if (result is null)
            {
                return;
            }
            ScanResult = result;
            ScanButtonText = "Start scanning";
            ScanHelper.StopScanning?.Invoke();
        }

        this.nowScanning = !this.nowScanning;

        if (this.nowScanning)
        {
            ScanButtonText = "Stop scanning";

            ScanHelper.ScanResult = scanResult;
            ScanHelper.StartScanning?.Invoke();
        }
        else
        {
            ScanButtonText = "Start scanning";
            ScanHelper.StopScanning?.Invoke();
        }
    }
}
