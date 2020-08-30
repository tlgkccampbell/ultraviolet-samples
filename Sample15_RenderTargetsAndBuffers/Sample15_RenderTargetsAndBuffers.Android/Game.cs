using System;
using System.IO;
using Android.App;
using Android.Content.PM;
using Android.Media;
using Android.Webkit;
using Ultraviolet.Graphics;
using AndroidEnvironment = Android.OS.Environment;

namespace Sample15_RenderTargetsAndBuffers
{
	[Activity(Label = "Sample 15 - Render Targets and Buffers", MainLauncher = true, ConfigurationChanges =
        ConfigChanges.Orientation |
        ConfigChanges.ScreenSize |
        ConfigChanges.KeyboardHidden)]
    partial class Game
    {
        partial void SaveImage(SurfaceSaver surfaceSaver, RenderTarget2D target)
		{
			var filename = $"output-{DateTime.Now:yyyyMMdd-HHmmss}.png";

			var dir = AndroidEnvironment.GetExternalStoragePublicDirectory(
				AndroidEnvironment.DirectoryPictures).AbsolutePath;
			var path = Path.Combine(dir, filename);

			using (var stream = File.OpenWrite(path))
				surfaceSaver.SaveAsPng(rtarget, stream);

			MediaScannerConnection.ScanFile(ApplicationContext, new String[] { path },
				new String[] { MimeTypeMap.Singleton.GetMimeTypeFromExtension("png") }, null);

			confirmMsgText = $"Image saved to photo gallery";
			confirmMsgOpacity = 1;
		}
    }
}