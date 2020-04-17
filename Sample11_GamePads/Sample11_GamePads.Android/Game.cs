using Android.App;
using Android.Content.PM;

namespace sample11_gamepads
{
    [Activity(Label = "Sample 11 - Game Pads", MainLauncher = true, ConfigurationChanges =
        ConfigChanges.Orientation |
        ConfigChanges.ScreenSize |
        ConfigChanges.KeyboardHidden)]
    partial class Game
    { }
}