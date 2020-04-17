using Ultraviolet;
using Ultraviolet.BASS;
using Ultraviolet.OpenGL;

namespace sample1_creatinganapplication
{
    public partial class Game : UltravioletApplication
    {
        public Game()
            : base("Ultraviolet", "Sample 1 - Creating an Application")
        { }

        protected override UltravioletContext OnCreatingUltravioletContext()
        {
            var configuration = new OpenGLUltravioletConfiguration();
            configuration.Plugins.Add(new BASSAudioPlugin());

            return new OpenGLUltravioletContext(this, configuration);
        }
    }
}
