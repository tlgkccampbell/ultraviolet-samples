using Android.App;
using Android.Content.PM;

namespace sample1_creatinganapplication
{
    [Activity(Label = "Sample 1 - Creating an Application", MainLauncher = true, ConfigurationChanges =
        ConfigChanges.Orientation |
        ConfigChanges.ScreenSize |
        ConfigChanges.KeyboardHidden)]
    partial class Game
    { }
}