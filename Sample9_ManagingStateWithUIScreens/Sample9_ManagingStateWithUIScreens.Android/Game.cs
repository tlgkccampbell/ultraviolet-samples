using Android.App;
using Android.Content.PM;

namespace Sample9_ManagingStateWithUIScreens
{
    [Activity(Label = "Sample 9 - Managing State with UI Screens", MainLauncher = true, ConfigurationChanges =
        ConfigChanges.Orientation |
        ConfigChanges.ScreenSize |
        ConfigChanges.KeyboardHidden)]
    partial class Game
    { }
}