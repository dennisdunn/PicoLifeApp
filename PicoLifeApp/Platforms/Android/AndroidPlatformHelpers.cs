
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace PicoLife.Helpers;

public class DroidPlatformHelpers
{
    public static async Task<PermissionStatus> CheckAndRequestBluetoothPermissions()
    {
        PermissionStatus status;
        if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.S)
        {
            status = await CheckStatusAsync<BluetoothSPermission>();

            if (status == PermissionStatus.Granted)
                return status;

            status = await RequestAsync<BluetoothSPermission>();
        }
        else
        {
            status = await CheckStatusAsync<LocationWhenInUse>();

            if (status == PermissionStatus.Granted)
                return status;


            if (ShouldShowRationale<LocationWhenInUse>())
            {
                await Application.Current.MainPage.DisplayAlert("Permission required", "Location permission is required for bluetooth scanning. We do not store or use your location at all.", "OK");
            }

            status = await RequestAsync<LocationWhenInUse>();
        }
        return status;
    }
}

public class BluetoothSPermission : BasePlatformPermission
{
    public override (string androidPermission, bool isRuntime)[] RequiredPermissions => new List<(string androidPermission, bool isRuntime)>
    {
        (Android.Manifest.Permission.Bluetooth, true),
        (Android.Manifest.Permission.BluetoothAdmin, true),
        (Android.Manifest.Permission.BluetoothScan, true),
        (Android.Manifest.Permission.BluetoothConnect, true),
        (Android.Manifest.Permission.AccessFineLocation, true)
    }.ToArray();
}