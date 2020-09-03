using System;
using System.IO;
using Sample16_CustomTextLayoutCommands.Assets;
using Sample16_CustomTextLayoutCommands.Input;
using Ultraviolet;
using Ultraviolet.BASS;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Graphics.Graphics2D.Text;
using Ultraviolet.OpenGL;
using Ultraviolet.SDL2;

namespace Sample16_CustomTextLayoutCommands
{
    public partial class Game : UltravioletApplication
    {
        public Game()
            : base("Ultraviolet", "Sample 16 - Custom Text Layout Commands")
        { }

        protected override UltravioletContext OnCreatingUltravioletContext()
        {
            var configuration = new SDL2UltravioletConfiguration();
            configuration.Plugins.Add(new OpenGLGraphicsPlugin());
            configuration.Plugins.Add(new BASSAudioPlugin());
            
            return new SDL2UltravioletContext(this, configuration);
        }

        protected override void OnInitialized()
        {
            UsePlatformSpecificFileSource();
            LoadInputBindings();

            base.OnInitialized();
        }

        protected override void OnShutdown()
        {
            SaveInputBindings();

            base.OnShutdown();
        }

        protected override void OnLoadingContent()
        {
            this.content = ContentManager.Create("Content");
            LoadContentManifests();

            this.spriteFont = this.content.Load<SpriteFont>(GlobalFontID.SegoeUI);
            this.spriteBatch = SpriteBatch.Create();

            this.textRenderer = new TextRenderer();
            this.textBlock = new ScrollingTextBlock(textRenderer, spriteFont,
                "Lorem ipsum dolor sit amet,|delay| consectetur adipiscing elit. |delay:200|" +
                "|speed:5|Donec |speed:1|commodo|speed:5| massa in odio dapibus blandit. |delay:200|" +
                "|speed|Etiam quis auctor magna. |delay:200|Mauris varius lacus eu vulputate pretium. |delay:200|" +
                "|speed:100|Cras a turpis id enim luctus laoreet. |delay:200|" +
                "|speed|Praesent sed faucibus nunc,|delay| in vestibulum libero. |delay:200|" +
                "Sed vitae bibendum ex. |delay:200|" +
                "Etiam vel aliquet ipsum,|delay| sit amet scelerisque est.", 256, 256);

            base.OnLoadingContent();
        }

        protected override void OnUpdating(UltravioletTime time)
        {
            var window = Ultraviolet.GetPlatform().Windows.GetPrimary();
            if (window != null)
            {
                this.textBlock.ChangeSize(
                    window.DrawableSize.Width / 2,
                    window.DrawableSize.Height / 2);
                this.textBlock.Update(time);
            }

            if (Ultraviolet.GetInput().GetActions().ResetScrollingText.IsPressed() || (Ultraviolet.GetInput().GetPrimaryTouchDevice()?.WasTapped() ?? false))
                textBlock.Reset();

            if (Ultraviolet.GetInput().GetActions().ExitApplication.IsPressed())
                Exit();

            base.OnUpdating(time);
        }

        protected override void OnDrawing(UltravioletTime time)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            var settings = new TextLayoutSettings(spriteFont, null, null, TextFlags.Standard);
            if (Ultraviolet.Platform == UltravioletPlatform.Android || Ultraviolet.Platform == UltravioletPlatform.iOS)
            {
                textRenderer.Draw(spriteBatch, "Tap the screen to reset the scrolling text.",
                    Vector2.One * 8f, Color.White, settings);
            }
            else
            {
                textRenderer.Draw(spriteBatch, $"Press {Ultraviolet.GetInput().GetActions().ResetScrollingText.Primary} to reset the scrolling text.",
                    Vector2.One * 8f, Color.White, settings);
            }

            var window = Ultraviolet.GetPlatform().Windows.GetPrimary();
            var x = (window.DrawableSize.Width - textBlock.Width.Value) / 2;
            var y = (window.DrawableSize.Height - textBlock.Height.Value) / 2;

            textBlock.Draw(time, spriteBatch, new Vector2(x, y), Color.White);

            spriteBatch.End();

            base.OnDrawing(time);
        }

        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                if (this.content != null)
                    this.content.Dispose();

                if (this.spriteBatch != null)
                    this.spriteBatch.Dispose();
            }
            base.Dispose(disposing);
        }

        private String GetInputBindingsPath()
        {
            return Path.Combine(GetRoamingApplicationSettingsDirectory(), "InputBindings.xml");
        }

        private void LoadInputBindings()
        {
            Ultraviolet.GetInput().GetActions().Load(GetInputBindingsPath(), throwIfNotFound: false);
        }

        private void SaveInputBindings()
        {
            Ultraviolet.GetInput().GetActions().Save(GetInputBindingsPath());
        }

        private void LoadContentManifests()
        {
            var uvContent = Ultraviolet.GetContent();

            var contentManifestFiles = this.content.GetAssetFilePathsInDirectory("Manifests");
            uvContent.Manifests.Load(contentManifestFiles);

            Ultraviolet.GetContent().Manifests["Global"]["Fonts"].PopulateAssetLibrary(typeof(GlobalFontID));
        }

        private ContentManager content;
        private SpriteFont spriteFont;
        private SpriteBatch spriteBatch;
        private TextRenderer textRenderer;
        private ScrollingTextBlock textBlock;
    }
}
