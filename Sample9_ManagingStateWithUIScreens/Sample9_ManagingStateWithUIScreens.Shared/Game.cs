using System;
using System.IO;
using Sample9_ManagingStateWithUIScreens.Assets;
using Sample9_ManagingStateWithUIScreens.Input;
using Sample9_ManagingStateWithUIScreens.UI;
using Sample9_ManagingStateWithUIScreens.UI.Screens;
using Ultraviolet;
using Ultraviolet.BASS;
using Ultraviolet.Content;
using Ultraviolet.OpenGL;
using Ultraviolet.SDL2;

namespace Sample9_ManagingStateWithUIScreens
{
    public partial class Game : UltravioletApplication
    {
        public Game()
            : base("Ultraviolet", "Sample 9 - Managing State with UI Screens")
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

            var screenService = new UIScreenService(content);
            var screen = screenService.Get<SampleScreen1>();
            Ultraviolet.GetUI().GetScreens().Open(screen, TimeSpan.Zero);

            base.OnLoadingContent();
        }

        protected override void OnUpdating(UltravioletTime time)
        {
            if (Ultraviolet.GetInput().GetActions().ExitApplication.IsPressed())
            {
                Exit();
            }

            base.OnUpdating(time);
        }

        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                if (this.content != null)
                    this.content.Dispose();
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

            Ultraviolet.GetContent().Manifests["Global"]["Textures"].PopulateAssetLibrary(typeof(GlobalTextureID));
        }

        private ContentManager content;
    }
}
