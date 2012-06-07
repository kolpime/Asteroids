using Microsoft.Xna.Framework;
using Asteroids.Levels;


namespace Asteroids.Screens
{

    class MainMenuScreen : MenuScreen
    {
        #region Initialization


        public MainMenuScreen()
            : base("")
        {
            // Create our menu entries.
            MenuEntry playSingleGameMenuEntry = new MenuEntry("\n\n\n\nSingle Player");
            MenuEntry playMultiGameMenuEntry = new MenuEntry("\n\n\n\nMultiplayer");
            MenuEntry Controls = new MenuEntry("\n\n\n\nControls");
            MenuEntry optionsMenuEntry = new MenuEntry("\n\n\n\nAbout");
            MenuEntry exitMenuEntry = new MenuEntry("\n\n\n\nExit");

            // Hook up menu event handlers.
            playSingleGameMenuEntry.Selected += playSingleGameMenuEntrySelected;
            playMultiGameMenuEntry.Selected += playMultiGameMenuEntrySelected;
            Controls.Selected += ControlsSelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(playSingleGameMenuEntry);
            MenuEntries.Add(playMultiGameMenuEntry);
            MenuEntries.Add(Controls);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }


        #endregion

        #region Handle Input


        void playSingleGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {

            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new textScreen(1,0));
            this.ExitScreen();
        }



        void playMultiGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                new MultiPlayerMenu());
        }


        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new MessageBoxScreen("      Stephen Shaw"+"\n"+"Stephen.Shaw85@Gmail.Com",false),e.PlayerIndex);
        }


        void ControlsSelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ControlsScreen(null,true), e.PlayerIndex);
        }


        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit?";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }



        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }


        #endregion
    }
}
