using System;
using System.IO;
using Sample5_RenderingSprites.Assets;
using Sample5_RenderingSprites.Input;
using Ultraviolet;
using Ultraviolet.BASS;
using Ultraviolet.Content;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.OpenGL;
using Ultraviolet.SDL2;

namespace Sample5_RenderingSprites
{
    public partial class Game : UltravioletApplication
    {
        public Game()
            : base("Ultraviolet", "Sample 5 - Rendering Sprites")
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

            this.sprite = this.content.Load<Sprite>(GlobalSpriteID.Explosion);
            this.spriteBatch = SpriteBatch.Create();

            this.controller1 = new SpriteAnimationController();
            this.controller2 = new SpriteAnimationController();
            this.controller3 = new SpriteAnimationController();
            this.controller4 = new SpriteAnimationController();

            base.OnLoadingContent();
        }

        protected override void OnUpdating(UltravioletTime time)
        {
            this.sprite.Update(time);

            if (time.TotalTime.TotalMilliseconds > 250 && !controller1.IsPlaying)
            {
                controller1.PlayAnimation(sprite["Explosion"]);
            }
            if (time.TotalTime.TotalMilliseconds > 500 && !controller2.IsPlaying)
            {
                controller2.PlayAnimation(sprite["Explosion"]);
            }
            if (time.TotalTime.TotalMilliseconds > 750 && !controller3.IsPlaying)
            {
                controller3.PlayAnimation(sprite["Explosion"]);
            }
            if (time.TotalTime.TotalMilliseconds > 1000 && !controller4.IsPlaying)
            {
                controller4.PlayAnimation(sprite["Explosion"]);
            }

            controller1.Update(time);
            controller2.Update(time);
            controller3.Update(time);
            controller4.Update(time);

            if (Ultraviolet.GetInput().GetActions().ExitApplication.IsPressed())
            {
                Exit();
            }

            base.OnUpdating(time);
        }

        protected override void OnDrawing(UltravioletTime time)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            spriteBatch.DrawSprite(this.sprite["Explosion"].Controller, new Vector2(32, 32));
            spriteBatch.DrawSprite(controller1, new Vector2(132, 32));
            spriteBatch.DrawSprite(controller2, new Vector2(232, 32));
            spriteBatch.DrawSprite(controller3, new Vector2(332, 32));
            spriteBatch.DrawSprite(controller4, new Vector2(432, 32));

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

            uvContent.Manifests["Global"]["Sprites"].PopulateAssetLibrary(typeof(GlobalSpriteID));
        }

        private ContentManager content;
        private Sprite sprite;
        private SpriteAnimationController controller1;
        private SpriteAnimationController controller2;
        private SpriteAnimationController controller3;
        private SpriteAnimationController controller4;
        private SpriteBatch spriteBatch;
    }
}
