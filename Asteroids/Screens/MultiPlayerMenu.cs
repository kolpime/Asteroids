#region Using Statements
using Microsoft.Xna.Framework;
using Asteroids.LevelData;
using Asteroids.Levels;
using Asteroids.Screens;
#endregion

namespace Asteroids.Screens
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class MultiPlayerMenu : MenuScreen
    {
        #region Fields

        MenuEntry ungulateMenuEntry;
        MenuEntry languageMenuEntry;
        MenuEntry frobnicateMenuEntry;
        MenuEntry elfMenuEntry;

        enum Ungulate
        {
            BactrianCamel,
            Dromedary,
            Llama,
        }
        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public MultiPlayerMenu()
            : base("Options")
        {
           
            // Create our menu entries.
            MenuEntry Back = new MenuEntry("\n\n\n\nBack");
            MenuEntry TwoPlayerMenuEntry = new MenuEntry("\n\n\n\n2 Player Game");
            MenuEntry ThreePlayerGameMenuEntry = new MenuEntry("\n\n\n\n3 Player Game");
            MenuEntry FourPlayerGameMenuEntry = new MenuEntry("\n\n\n\n4 Player Game");

            // Hook up menu event handlers.
            TwoPlayerMenuEntry.Selected += TwoGameMenuEntrySelected;
            ThreePlayerGameMenuEntry.Selected += ThreeGameMenuEntrySelected;
            FourPlayerGameMenuEntry.Selected += FourGameSelected;
            Back.Selected += BackSelected;
            

            // Add entries to the menu.
            MenuEntries.Add(TwoPlayerMenuEntry);
            MenuEntries.Add(ThreePlayerGameMenuEntry);
            MenuEntries.Add(FourPlayerGameMenuEntry);
            MenuEntries.Add(Back);
        }


        #endregion

        #region Handle Input


        void TwoGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {

            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                              new MultiPlayer());
            this.ExitScreen();
        }



        void ThreeGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new ThreePlayer());
            this.ExitScreen();
        }


        void FourGameSelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                              new FourPlayer());
            this.ExitScreen();
           
        }


        void BackSelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                new MainMenuScreen());
        }


        #endregion
    }
}
