using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Asteroids.LevelData;
using Asteroids.Screens;

namespace Asteroids.Screens
{
    class WinGameScreen : MenuScreen
    {
        #region Initialization



        public WinGameScreen()
            : base("YOU WIN!")
        {

            MenuEntry returnToMenuEntry = new MenuEntry("Return To Menu");
            MenuEntry exitGameMenuEntry = new MenuEntry("Exit");

            returnToMenuEntry.Selected += returnToMenuEntrySelected;
            exitGameMenuEntry.Selected += ExitGameMenuEntrySelected;

            MenuEntries.Add(returnToMenuEntry);
            MenuEntries.Add(exitGameMenuEntry);
        }


        #endregion

        #region Handle Input


        void returnToMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                new MainMenuScreen());
        }


        void ExitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to exit?";

            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }


        #endregion

        #region Draw


        public override void Draw(GameTime gameTime)
        {
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            base.Draw(gameTime);
        }


        #endregion
    }
}
