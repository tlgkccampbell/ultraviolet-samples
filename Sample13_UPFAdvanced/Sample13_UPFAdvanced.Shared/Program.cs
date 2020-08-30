using System.Linq;

namespace Sample13_UPFAdvanced
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var flags = GameFlags.None;

            if (args.Contains("-compile:expressions"))
                flags |= GameFlags.CompileExpressions;

            using (var game = new Game(flags))
            {
                game.Run();
            }
        }
    }
}