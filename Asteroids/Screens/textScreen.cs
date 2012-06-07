#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Asteroids.Levels;
#endregion

namespace Asteroids.Screens
{
    class textScreen : GameScreen
    {
    
        string usageText ="";
        string message = "";
        int lvl;
        int score;
        public event EventHandler<PlayerIndexEventArgs> Accepted;
        public event EventHandler<PlayerIndexEventArgs> Cancelled;


        #region Initialization

        public textScreen(int level, int scre)
        {
            score = scre;
            usageText = "";
            lvl = level;

            if (level == 1)
            {
                usageText += "Asteroids are hurtling towards earth.\n\n";
                usageText += "With the planets defence shields down,\n";
                usageText += "The U.N. send the next best thing\n";
                usageText += "You in a rocket with self renewing lazer canons.\n";
                usageText += "NASA say they've never seen anything like it!\n";
                usageText += "Get your trigger finger ready.\n\n";
                usageText += "The asteroids seem to be coming from one source.\n";
                usageText += "Keep them at bay while NASA's gets to the bottom of it.\n";
                usageText += "Don't worry - back ups on the way.\n\n";
                usageText += "                                Press fire to continue";
            }
            else
            {
                if (level == 2)
                {
                    usageText += "Excellent work pilot\n\n";
                    usageText += "But the radar! More Asteroids are hurtling towards you.\n\n";
                    usageText += "Back up was destroyed, its all down to you.\n";
                    usageText += "Keep blasting, you're our only hope.\n\n";
                    usageText += "                                Press fire to continue";
                }
                else
                {
                    if (level == 3)
                    {
                        usageText += "Radar has picked up several large objects not far from you.\n";
                        usageText += "looks like they're located inside the asteroid field.\n\n";
                        usageText +="Keep shooting until you get a visual.\n\n";
                        usageText += "                                Press fire to continue";
                    }
                }
            }



            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

        }


        #endregion

        #region Handle Input

        public override void HandleInput(InputState input)
        {
            PlayerIndex playerIndex;

            if (input.IsMenuSelect(ControllingPlayer, out playerIndex))
            {
                if (Accepted != null)
                    Accepted(this, new PlayerIndexEventArgs(playerIndex));

                if (lvl == 1)
                {
                    LoadingScreen.Load(ScreenManager, true, playerIndex,
                                   new Level1(score));
                    this.ExitScreen();
                }
                else
                {
                    if (lvl == 2)
                    {
                        LoadingScreen.Load(ScreenManager, true, playerIndex,
                                   new Level2(score));
                        this.ExitScreen();
                    }
                    else
                    {
                        if (lvl == 3)
                        {
                            LoadingScreen.Load(ScreenManager, true, playerIndex,
                                   new Level3(score));
                            this.ExitScreen();
                        }
                    }
                }
            }

        }


        #endregion

        #region Draw

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;


            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            Color color = new Color(255, 255, 255, TransitionAlpha);

            spriteBatch.Begin();

            spriteBatch.DrawString(font, usageText, new Vector2(100,100), color);

            spriteBatch.End();
        }


        #endregion
    }
}

