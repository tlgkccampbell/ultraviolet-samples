using System;
using System.IO;
using Sample3_RenderingGeometry.Input;
using Ultraviolet;
using Ultraviolet.BASS;
using Ultraviolet.Graphics;
using Ultraviolet.OpenGL;
using Ultraviolet.SDL2;

namespace Sample3_RenderingGeometry
{
    public partial class Game : UltravioletApplication
    {
        public Game()
            : base("Ultraviolet", "Sample 3 - Rendering Geometry")
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
            this.effect = BasicEffect.Create();

            this.vbuffer = VertexBuffer.Create<VertexPositionColor>(3);
            this.vbuffer.SetData(new[]
            {
                new VertexPositionColor(new Vector3(0, 1, 0), Color.Red),
                new VertexPositionColor(new Vector3(1, -1, 0), Color.Lime),
                new VertexPositionColor(new Vector3(-1, -1, 0), Color.Blue),
            });

            this.geometryStream = GeometryStream.Create();
            this.geometryStream.Attach(this.vbuffer);

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

        protected override void OnDrawing(UltravioletTime time)
        {
            var gfx = Ultraviolet.GetGraphics();
            var window = Ultraviolet.GetPlatform().Windows.GetPrimary();
            var aspectRatio = window.DrawableSize.Width / (float)window.DrawableSize.Height;

            effect.World = Matrix.CreateRotationY((float)(2.0 * Math.PI * time.TotalTime.TotalSeconds));
            effect.View = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            effect.Projection = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, aspectRatio, 1f, 1000f);
            effect.VertexColorEnabled = true;

            foreach (var pass in this.effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                gfx.SetRasterizerState(RasterizerState.CullNone);
                gfx.SetGeometryStream(geometryStream);
                gfx.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
            }

            base.OnDrawing(time);
        }

        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                if (this.effect != null)
                    this.effect.Dispose();

                if (this.vbuffer != null)
                    this.vbuffer.Dispose();

                if (this.geometryStream != null)
                    this.geometryStream.Dispose();
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

        private BasicEffect effect;
        private VertexBuffer vbuffer;
        private GeometryStream geometryStream;
    }
}
