namespace Common
{
    public static class ScanHelper
    {
        public static Action? StartScanning { get; set; }
        public static Action? StopScanning { get; set; }

        public static Action<string?>? ScanResult { get; set; }
    }
}
