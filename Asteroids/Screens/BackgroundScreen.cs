#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Asteroids.Screens
{

    class BackgroundScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        Texture2D backgroundTexture;

        #endregion

        #region Initialization


        public BackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }


        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            backgroundTexture = content.Load<Texture2D>("Images\\Asteroid");
        }


        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
        }


        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            byte fade = TransitionAlpha;

            spriteBatch.Begin(SpriteBlendMode.None);

            spriteBatch.Draw(backgroundTexture, fullscreen,
                             new Color(fade, fade, fade));

            spriteBatch.End();
        }


        #endregion
    }
}
