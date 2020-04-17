using System;
using System.IO;
using Ultraviolet;
using Ultraviolet.Audio;
using Ultraviolet.BASS;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Graphics.Graphics2D.Text;
using Ultraviolet.Input;
using Ultraviolet.OpenGL;

namespace sample8_playingsoundeffects
{
    public partial class Game : UltravioletApplication
    {
        public Game()
            : base("Ultraviolet", "Sample 8 - Playing Sound Effects")
        { }

        protected override UltravioletContext OnCreatingUltravioletContext()
        {
            var configuration = new OpenGLUltravioletConfiguration();
            configuration.Plugins.Add(new BASSAudioPlugin());

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

            this.spriteFont = this.content.Load<SpriteFont>(GlobalFontID.SegoeUI);
            this.spriteBatch = SpriteBatch.Create();
            this.textRenderer = new TextRenderer();
            this.textLayoutCommands = new TextLayoutCommandStream();

            for (int i = 0; i < this.soundEffectPlayers.Length; i++)
                this.soundEffectPlayers[i] = SoundEffectPlayer.Create();

            this.soundEffect = this.content.Load<SoundEffect>(GlobalSoundEffectID.Explosion);

            base.OnLoadingContent();
        }

        protected override void OnUpdating(UltravioletTime time)
        {
            foreach (var soundEffectPlayer in soundEffectPlayers)
                soundEffectPlayer.Update(time);

            var touch = Ultraviolet.GetInput().GetPrimaryTouchDevice();
            if (touch != null && touch.WasTapped())
            {
                soundEffectPlayers[nextPlayerInSequence].Play(soundEffect);
                nextPlayerInSequence = (nextPlayerInSequence + 1) % soundEffectPlayers.Length;
            }

            var keyboard = Ultraviolet.GetInput().GetKeyboard();

            if (keyboard.IsKeyPressed(Key.D1))
                soundEffectPlayers[0].Play(soundEffect);
            if (keyboard.IsKeyPressed(Key.D2))
                soundEffectPlayers[1].Play(soundEffect);
            if (keyboard.IsKeyPressed(Key.D3))
                soundEffectPlayers[2].Play(soundEffect);
            if (keyboard.IsKeyPressed(Key.D4))
                soundEffectPlayers[3].Play(soundEffect);
            if (keyboard.IsKeyPressed(Key.D5))
                soundEffectPlayers[4].Play(soundEffect);
            if (keyboard.IsKeyPressed(Key.D6))
                soundEffectPlayers[5].Play(soundEffect);
            if (keyboard.IsKeyPressed(Key.D7))
                soundEffectPlayers[6].Play(soundEffect);
            if (keyboard.IsKeyPressed(Key.D8))
                soundEffectPlayers[7].Play(soundEffect);

            if (Ultraviolet.GetInput().GetActions().ExitApplication.IsPressed())
            {
                Exit();
            }

            base.OnUpdating(time);
        }

        protected override void OnDrawing(UltravioletTime time)
        {
            var window = Ultraviolet.GetPlatform().Windows.GetPrimary();
            var width = window.DrawableSize.Width;
            var height = window.DrawableSize.Height;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            var instruction = Ultraviolet.Platform == UltravioletPlatform.Android || Ultraviolet.Platform == UltravioletPlatform.iOS ?
                "|c:FFFFFF00|Tap the screen|c| to activate one of the sound effect players." :
                "Press the |c:FFFFFF00|1-8 number keys|c| to activate one of the sound effect players.";
            var attribution =
                instruction + "\n\n" +
                "\"|c:FFFFFF00|grenade.wav|c|\" by ljudman (http://freesound.org/people/ljudman)\n" +
                "Licensed under Creative Commons: Sampling+\n" +
                "|c:FF808080|http://creativecommons.org/licenses/sampling+/1.0/|c|";

            var settings = new TextLayoutSettings(spriteFont, width, height, TextFlags.AlignMiddle | TextFlags.AlignCenter);
            textRenderer.CalculateLayout(attribution, textLayoutCommands, settings);
            textRenderer.Draw(spriteBatch, textLayoutCommands, Vector2.Zero, Color.White);

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

                for (var i = 0; i < soundEffectPlayers.Length; i++)
                    soundEffectPlayers[i].Dispose();
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
            Ultraviolet.GetContent().Manifests["Global"]["SoundEffects"].PopulateAssetLibrary(typeof(GlobalSoundEffectID));
        }

        private ContentManager content;
        private SpriteFont spriteFont;
        private SpriteBatch spriteBatch;
        private TextRenderer textRenderer;
        private TextLayoutCommandStream textLayoutCommands;
        private SoundEffect soundEffect;
        private readonly SoundEffectPlayer[] soundEffectPlayers = new SoundEffectPlayer[8];
        private Int32 nextPlayerInSequence;
    }
}
