using System;
using System.IO;
using sample13_upfadvanced.Input;
using sample13_upfadvanced.UI;
using sample13_upfadvanced.UI.Screens;
using Ultraviolet;
using Ultraviolet.BASS;
using Ultraviolet.Content;
using Ultraviolet.FreeType2;
using Ultraviolet.OpenGL;
using Ultraviolet.Presentation;
using Ultraviolet.Presentation.Styles;

namespace sample13_upfadvanced
{
    public partial class Game : UltravioletApplication
    {
        public Game()
            : this(GameFlags.None)
        { }

        public Game(GameFlags flags)
            : base("Ultraviolet", "Sample 13 - UPF (Advanced)")
        {
            this.flags = flags;
        }

        protected override UltravioletContext OnCreatingUltravioletContext()
        {
            var configuration = new OpenGLUltravioletConfiguration();
            configuration.EnableServiceMode = ShouldRunInServiceMode();
            configuration.WatchViewFilesForChanges = ShouldDynamicallyReloadContent();
            configuration.Plugins.Add(new BASSAudioPlugin());
            configuration.Plugins.Add(new FreeTypeFontPlugin());
            configuration.Plugins.Add(new PresentationFoundationPlugin());

            return new OpenGLUltravioletContext(this, configuration);
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
            LoadPresentation();

            if (Ultraviolet.IsRunningInServiceMode)
            {
                CompileBindingExpressions();
                Environment.Exit(0);
            }
            else
            {
                this.screenService = new UIScreenService(content);

                var screen = screenService.Get<GameMenuScreen>();
                Ultraviolet.GetUI().GetScreens().Open(screen);
            }

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
                if (this.screenService != null)
                    this.screenService.Dispose();

                if (this.globalStyleSheet != null)
                    this.globalStyleSheet.Dispose();

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
        }

        private void LoadPresentation()
        {
            var upf = Ultraviolet.GetUI().GetPresentationFoundation();
            upf.RegisterKnownTypes(GetType().Assembly);

            if (!ShouldRunInServiceMode())
            {
                globalStyleSheet = GlobalStyleSheet.Create();
                globalStyleSheet.Append(content, "UI/DefaultUIStyles");
                upf.SetGlobalStyleSheet(globalStyleSheet);

                CompileBindingExpressions();
                upf.LoadCompiledExpressions();
            }
        }

        private Boolean ShouldRunInServiceMode()
        {
            return (flags & GameFlags.CompileExpressions) == GameFlags.CompileExpressions;
        }

        private Boolean ShouldCompileBindingExpressions()
        {
#if DEBUG
            return true;
#else
            return (flags & GameFlags.CompileExpressions) == GameFlags.CompiledExpressions || System.Diagnostics.Debugger.IsAttached;
#endif
        }

        private Boolean ShouldDynamicallyReloadContent()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }

        private void CompileBindingExpressions()
        {
            if (!ShouldCompileBindingExpressions())
                return;

            var compilationFlags = CompileExpressionsFlags.None;
            if ((this.flags & GameFlags.CompileExpressions) == GameFlags.CompileExpressions)
            {
                compilationFlags |= CompileExpressionsFlags.ResolveContentFiles;
                compilationFlags |= CompileExpressionsFlags.IgnoreCache;
            }

            var upf = Ultraviolet.GetUI().GetPresentationFoundation();
            upf.CompileExpressionsIfSupported("Content", compilationFlags);
        }

        // The global content manager.  Manages any content that should remain loaded for the duration of the game's execution.
        private ContentManager content;

        // State values.
        private readonly GameFlags flags;
        private GlobalStyleSheet globalStyleSheet;
        private UIScreenService screenService;
    }
}
