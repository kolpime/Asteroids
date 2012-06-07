using System;
using System.Collections.Generic;
using System.Linq;
using Asteroids.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Asteroids.LevelData
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        
        ScreenManager screenManager;
        public SoundEffect themeMusic;
        bool themePlaying = false;
        SoundEffectInstance themeInstance;


        public Game1()
        {
            
            Content.RootDirectory = "Content";
            themeMusic = Content.Load<SoundEffect>("Audio\\asteroids compx");
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 1000;

            // Create the screen manager component.
            screenManager = new ScreenManager(this);

            Components.Add(screenManager);

            // Activate the first screens.
            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(), null);

            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            Window.Title = "Asteroids";
            this.IsMouseVisible = true;
        }

        #region Draw


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);
            #region Play Theme
            //Play theme sound
            if (!themePlaying)
            {
                if (themeInstance == null)
                {
                    themeInstance = themeMusic.Play(0.5f, 0.0f, 0.0f, true);
                    themeInstance.Volume = 2.0f;
                }
                else
                    themeInstance.Resume();
                themePlaying = true;
            }
            else if (themePlaying)
            {
                themeInstance.Play();
                themePlaying = false;
            }
            #endregion
            // The real drawing happens inside the screen manager component.
            base.Draw(gameTime);
        }


        #endregion
    }
    static class Program
    {
        static void Main()
        {
            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
}
