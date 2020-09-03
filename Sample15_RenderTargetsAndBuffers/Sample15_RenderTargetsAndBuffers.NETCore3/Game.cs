using System;
using System.IO;
using Ultraviolet.Graphics;

namespace Sample15_RenderTargetsAndBuffers
{
	partial class Game
    {
        partial void SaveImage(SurfaceSaver surfaceSaver, RenderTarget2D target)
		{
			var filename = $"output-{DateTime.Now:yyyyMMdd-HHmmss}.png";
			var path = filename;

			using (var stream = File.OpenWrite(path))
				surfaceSaver.SaveAsPng(rtarget, stream);

			confirmMsgText = $"Image saved to {filename}";
			confirmMsgOpacity = 1;
		}
    }
}
