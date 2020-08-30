using Android.App;
using Android.Content.PM;

namespace Sample1_CreatingAnApplication
{
    [Activity(Label = "Sample 1 - Creating an Application", MainLauncher = true, ConfigurationChanges =
        ConfigChanges.Orientation |
        ConfigChanges.ScreenSize |
        ConfigChanges.KeyboardHidden)]
    partial class Game
    { }
}