using Ultraviolet;
using Ultraviolet.BASS;
using Ultraviolet.OpenGL;
using Ultraviolet.SDL2;

namespace Sample1_CreatingAnApplication
{
    public partial class Game : UltravioletApplication
    {
        public Game()
            : base("Ultraviolet", "Sample 1 - Creating an Application")
        { }

        protected override UltravioletContext OnCreatingUltravioletContext()
        {
            var configuration = new SDL2UltravioletConfiguration();
            configuration.Plugins.Add(new OpenGLGraphicsPlugin());
            configuration.Plugins.Add(new BASSAudioPlugin());

            return new SDL2UltravioletContext(this, configuration);
        }
    }
}
