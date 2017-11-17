using System;

namespace PokeBoomz
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var game = new HomeGame();
            using (game)
                game.Run();

            if (game.h2p)
            {
                var h2pGame = new UI_HowToPlay();
                h2pGame.Run();
                if (h2pGame.start) game.start = true;
                if (h2pGame.home) Main();
            }

            if (game.start)
            {
                var mainGame = new PokeBoomzGame();
                mainGame.Run();
            }

        }
    }
#endif
}
