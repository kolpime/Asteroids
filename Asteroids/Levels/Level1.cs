using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Asteroids.Levels
{

    public class Level1 : GameScreen
    {
        
        #region Classes
        ContentManager Content;
        CheckPositions check = new CheckPositions();
        CollisionDetection collision = new CollisionDetection();
        Player play;
        SpriteFont defaultFont;
        globe Globe = new globe();

        #endregion

        #region Bools
        public bool hyperbool = true;
        bool EnginePlaying = false;
        public bool soundEnginePlaying = false;
        public bool bulletCollide = false;
        public bool bulletCollide2 = false;
        public bool bulletCollide3 = false;
        public bool dead = false;
        public bool leftclicked = false;
        public bool rightclicked = false;
        public bool thrustclicked = false;
        public bool shootclicked = false;
        bool collide;
        bool paused = false;
        #endregion

        #region Texture2D and Rectangles
        public Texture2D BackGround;                            
        public Texture2D dataScreen;
        public Texture2D panel;
        public Texture2D hypButton;
        public Texture2D hypButtonClk;

        public Rectangle BallRectangle;
        public Rectangle Hyp;

        Rectangle ScoreRectangle = new Rectangle(440, 0, 125, 65);
        Rectangle DataRectangle = new Rectangle(0, 0, 168, 60);

        #endregion

        #region Sound Effects
        public SoundEffect Engine;                              //declare sound effects
        public SoundEffect WeaponFire;                          //
        // 
        public SoundEffect Explosion;
        public SoundEffect HyperSpace;

        SoundEffectInstance EngineInstance;


        #endregion

        #region Constants
        public const int noPlayers = 0;
        public const int NumAsteroid1 = 2;                     //no of asteroids
        public const int NumAsteroid2 = 8;
        public const int NumAsteroid3 = 16;
        public const int noBullets = 1;
        #endregion

        #region object lists
        //Matrix[] asteroidTransforms;

        Asteroid[] asteroid1List = new Asteroid[NumAsteroid1];   //linked list of asteroid data-in draw for each element in the list, pass data and draw
        Asteroid[] asteroid2List = new Asteroid[NumAsteroid2];
        Asteroid[] asteroid3List = new Asteroid[NumAsteroid3];
        Projectile[] BulletList;
        Player[] playerlist = new Player[noPlayers];
        
        #endregion

        #region ModelData
        public Model myModel;
        public Model globeModel;
        public Model Asteroid1;
        public Model Asteroid2;
        public Model Asteroid3;
        public Model ammo;
        #endregion

        #region Vectors
        public Vector3 GlobePosition;
        Vector3 Initial = Vector3.Zero;
        Vector3 cameraPosition = new Vector3(0.0f, 50.0f, 8500.0f);
        Vector3 AP4;
        Vector3 AP3;
        Vector3 AP2;
        Vector3 AP1; 
        #endregion

        #region Floats ints and doubles

        public bool AmmoFlying = false;
        public float AmmoAngle;
        public float globeRotation = 0.0005f;
        public float spawntimer =2500.0f;
        public float spawninterval = 2500.0f;
        public float aspectRatio;
        public int screenWidth;
        public float ballXRotation = 0.0f;
        public float ballYRotation = 0.0f;
        public int screenHeight;
        public float EngineThrust = 0.0f;
        public float timer = 500.0f;
        public int hyperSpace = 1;
        public int asteroidsonscreen = 4;
        float interval = 500.0f;
        public int Score = 0;

        #endregion


        public Level1(int scr)
        {
            Score = scr;
        }

        public override void LoadContent()
        {
            if (Content == null)
                Content = new ContentManager(ScreenManager.Game.Services, "Content");

            #region loaded content

            myModel = Content.Load<Model>("Model\\fighter");
            Asteroid1 = Content.Load<Model>("Model\\Asteroid1");
            Asteroid2 = Content.Load<Model>("Model\\Asteroid2");
            Asteroid3 = Content.Load<Model>("Model\\Asteroid3");
            ammo = Content.Load<Model>("Model\\ammo");
            globeModel = Content.Load<Model>("Model\\globe");

            Explosion = Content.Load<SoundEffect>("Audio\\explosion3");

            dataScreen = Content.Load<Texture2D>("Images\\DataScreen");
            BackGround = Content.Load<Texture2D>("Images\\background");
            panel = Content.Load<Texture2D>("Images\\panel");
            hypButton = Content.Load<Texture2D>("Images\\p1hyp");
            hypButtonClk = Content.Load<Texture2D>("Images\\p1hypclk");

            Engine = Content.Load<SoundEffect>("Audio\\engine_3");
            WeaponFire = Content.Load<SoundEffect>("Audio\\pdp1_fire");

            defaultFont = Content.Load<SpriteFont>("Fonts\\defaultFont");
            HyperSpace = Content.Load<SoundEffect>("Audio\\hax2_fire_alt");

            play = new Player(myModel, Initial, 0.0f, 3);
            Projectile bullet = new Projectile(ammo, Initial, 0.0f, Initial, false, Initial);
            BulletList = new Projectile[noBullets];
            GlobePosition = new Vector3(-2500, -1500, 1050);

            #endregion

            #region Graphic Device and screen elements
            screenWidth = ScreenManager.GraphicsDevice.PresentationParameters.BackBufferWidth;
            screenHeight = ScreenManager.GraphicsDevice.PresentationParameters.BackBufferHeight;

            aspectRatio = ScreenManager.GraphicsDevice.Viewport.AspectRatio;
            #endregion

            #region initialise Asteroids
            AP1.X = -2000;
            AP1.Y = screenHeight;

            AP2.X = -screenWidth;
            AP2.Y = -2000;

            AP3.X = -300;
            AP3.Y = -900;

            AP4.X = -300;
            AP4.Y = -400;

            asteroid1List[0] = new Asteroid(Asteroid1, AP1, 0.005f, true, 5);
            asteroid1List[1] = new Asteroid(Asteroid1, AP2, 0.5f, true, 5);

            asteroid3List[0] = new Asteroid(Asteroid3, AP3, 1.005f, true, 1);
            asteroid3List[1] = new Asteroid(Asteroid3, AP4, 2.005f, true, 1);

            for (int i = 0; i < NumAsteroid2; i++)
                asteroid2List[i] = new Asteroid(Asteroid2, new Vector3(8000, 8000, 0), 0.005f, false, 3);

            for (int i = 2; i < NumAsteroid3; i++)
                asteroid3List[i] = new Asteroid(Asteroid3, new Vector3(8000, 8000, 0), 0.005f, false, 1);


            #endregion

            #region initialise bullet list
            for (int i = 0; i < noBullets; i++)
            {
                BulletList[i] = new Projectile(ammo, new Vector3(5000, 5000, 0), 0.0f, Initial, false, Initial);
            }
            #endregion

            
        }

        public override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            try
            {

                if (paused == false)
                {
                    #region Game

                    base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
                    KeyboardState keyb = Keyboard.GetState();

                    DetectCollisions();

                    #region Asteroids are Defeated
                    if (asteroidsonscreen == 0 || (keyb.IsKeyDown(Keys.N)))
                    {
                        this.Content.Unload();
                        this.ExitScreen();
                        LoadingScreen.Load(ScreenManager, true, null, new textScreen(2,Score));

                    }
                    #endregion

                    #region Player Dead
                    if (dead == true)
                    {
                        this.Content.Unload();
                        this.ExitScreen();
                        LoadingScreen.Load(ScreenManager, false, null, new GameOverBackgroundScreen(), new GameOverScreen());
                    }

                    #endregion

                    timer = Input(gameTime, timer);

                    #region Bullet Timeout
                    for (int j = 0; j <= noBullets - 1; j++)
                    {
                        BulletList[j].AmmoModelPosition += BulletList[j].AmmoVelocity;
                        BulletList[j].timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (BulletList[j].timer > interval)
                        {
                            BulletList[j].timer = 0.0f;
                            BulletList[j].AmmoFlying = false;
                        }

                    }
                    #endregion

                    #region asteroidroll
                    #region Asteroid1
                    for (int j = 0; j <= NumAsteroid1 - 1; j++)
                    {
                        if (asteroid1List[j].isAlive)
                        {
                            asteroid1List[j].rotation += 0.005f;
                            asteroid1List[j].position += new Vector3(3.75f, 3.75f, 0);
                        }
                    }
                    #endregion

                    #region Asteroid2
                    for (int i = 0; i <= NumAsteroid2 - 1; i++)
                    {
                        Vector3 roll = Vector3.Zero;
                        if (i == 1 || i == 3 || i == 5 || i == 7 || i == 9 || i == 11 || i == 13 || i == 15)
                        {
                            roll = new Vector3(-4.75f, 6.75f, 0.0f);
                        }
                        if (i == 0 || i == 2 || i == 4 || i == 6 || i == 8 || i == 10 || i == 12 || i == 14)
                        {
                            roll = new Vector3(6.75f, -4.75f, 0.0f);
                        }

                        if (asteroid2List[i].isAlive)
                        {
                            asteroid2List[i].position += roll;
                            asteroid2List[i].rotation += 0.01f;
                        }
                    }
                    #endregion

                    #region Asteroid3
                    for (int i = 0; i <= NumAsteroid3 - 1; i++)
                    {
                        Vector3 roll = Vector3.Zero;
                        if (i == 1 || i == 3 || i == 5 || i == 7 || i == 9 || i == 11 || i == 13 || i == 15)
                        {
                            roll = new Vector3(-8.75f, 6.75f, 0.0f);
                        }
                        if (i == 0 || i == 2 || i == 4 || i == 6 || i == 8 || i == 10 || i == 12 || i == 14)
                        {
                            roll = new Vector3(9.75f, -6.75f, 0.0f);
                        }

                        if (asteroid3List[i].isAlive)
                        {
                            asteroid3List[i].position += roll;
                            asteroid3List[i].rotation += 0.015f;
                        }
                    }
                    #endregion
                    #endregion

                    globeRotation -= 0.0002f;
                    spawntimer += (float)gameTime.ElapsedGameTime.Milliseconds;


                    #endregion
                }
                if (paused == true)
                {
                    PausedInput();

                }
            }
            catch(Exception ex)
            {
                
            }
        }

        public override void Draw(GameTime gameTime)
        {
            try
            {
                if (paused == false)
                {
                    #region Game
                    ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);
                    ScreenManager.SpriteBatch.Begin();

                    DrawScenery();
                    ScreenManager.spriteBatch.End();

                    DrawItems();

                    #region HUD
                    ScreenManager.spriteBatch.Begin();
                    ScreenManager.spriteBatch.Draw(dataScreen, ScoreRectangle, Color.White);
                    ScreenManager.spriteBatch.Draw(dataScreen, DataRectangle, Color.White);
                    ScreenManager.SpriteBatch.DrawString(defaultFont, "Score", new Vector2(450, 5), Color.Red);
                    ScreenManager.SpriteBatch.DrawString(defaultFont, Score.ToString(), new Vector2(450, 30), Color.Red);

                    ScreenManager.SpriteBatch.DrawString(defaultFont, "Lives", new Vector2(15, 5), Color.Red);
                    ScreenManager.SpriteBatch.DrawString(defaultFont, play.Lives.ToString(), new Vector2(75, 5), Color.Red);

                    ScreenManager.SpriteBatch.DrawString(defaultFont, "HyperSpace", new Vector2(15, 25), Color.Red);
                    ScreenManager.SpriteBatch.DrawString(defaultFont, hyperSpace.ToString(), new Vector2(140, 25), Color.Red);
                    #endregion

                    ScreenManager.spriteBatch.End();

                    Rectangle BallRectangle = new Rectangle(50, screenHeight - 220, 200, 200);
                    Rectangle Hyp = new Rectangle(250, screenHeight - 220, 60, 65);

                    ScreenManager.spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
                    ScreenManager.spriteBatch.Draw(hypButton, Hyp, Color.White);
                    ScreenManager.spriteBatch.Draw(panel, BallRectangle, Color.White);
                    ScreenManager.spriteBatch.End();

                    base.Draw(gameTime);
                    #endregion
                }
                if (paused == true)
                {
                    ScreenManager.spriteBatch.Begin();
                    BackGround = Content.Load<Texture2D>("Images\\Paused");
                    DrawScenery();
                    ScreenManager.spriteBatch.End();
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        /*****************************************************/

        //Draw Game Items
        public void DrawItems()
        {
            #region Globe
            Globe.drawGlobe(aspectRatio, cameraPosition, globeRotation, GlobePosition, globeModel);
            #endregion

            #region Ammo
            for (int j = 0; j <= noBullets - 1; j++)
            {
                if (BulletList[j].AmmoFlying)
                {
                    BulletList[j].DrawAmmo(aspectRatio, cameraPosition);
                }
            };
            #endregion

            #region Player
            // this if statement adds a flicker to the player when their hit
            if ((spawntimer >= spawninterval) || (spawntimer > 100 && spawntimer < 200) || 
                (spawntimer > 100 && spawntimer < 300) || (spawntimer > 500 && spawntimer < 700) ||  
                (spawntimer > 900 && spawntimer < 1100) || (spawntimer > 1300 && spawntimer < 1500) ||
                (spawntimer > 1700 && spawntimer < 1900) ||(spawntimer > 2100 && spawntimer < 2300))
            play.DrawPlayer(aspectRatio, cameraPosition);
            #endregion

            #region Asteroids

            #region Asteroid1
            for (int i = 0; i <= NumAsteroid1 - 1; i++)
            {
                if (asteroid1List[i].isAlive == true)
                    asteroid1List[i].DrawAsteroid(aspectRatio, cameraPosition);
            };
            #endregion


            #region Asteroid2
            for (int i = 0; i <= NumAsteroid2 - 1; i++)
            {
                if (asteroid2List[i].isAlive == true)
                    asteroid2List[i].DrawAsteroid(aspectRatio, cameraPosition);
            };
            #endregion

            #region Asteroid3
            for (int i = 0; i <= NumAsteroid3 - 1; i++)
            {
                if (asteroid3List[i].isAlive == true)
                    asteroid3List[i].DrawAsteroid(aspectRatio, cameraPosition);
            };
            #endregion
            #endregion

            
            
        }

        //Draw Scenery
        public void DrawScenery()
        {
            Rectangle screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
            ScreenManager.spriteBatch.Draw(BackGround, screenRectangle, Color.White);
        }

        //Update Data for model thrust etc...
        protected void UpdateInput()
        {
            KeyboardState currentKeyState = Keyboard.GetState();
            GamePadState gpad1 = GamePad.GetState(PlayerIndex.One);
            // Create some velocity if the right trigger is down.
            Vector3 velocityAdd = Vector3.Zero;

            // Find out what direction we should be thrusting, using rotation.
            velocityAdd.X = -(float)Math.Sin(play.rotation);
            velocityAdd.Y = +(float)Math.Cos(play.rotation);

            if (currentKeyState.IsKeyDown(Keys.Space) || (gpad1.Triggers.Left == 1.0f))
                velocityAdd *= 1.5f;

            // Finally, add this vector to our velocity.
            play.velocity += velocityAdd;



        }

        //Input at the Pause screen
        private void PausedInput()
        {
            KeyboardState keyb = Keyboard.GetState();
            GamePadState gpad1 = GamePad.GetState(PlayerIndex.One);

            if (keyb.IsKeyDown(Keys.Back) || (gpad1.Buttons.Back == ButtonState.Pressed))
            {
                BackGround = Content.Load<Texture2D>("Images\\background");
                paused = false;
            }
            if (keyb.IsKeyDown(Keys.Escape) || (gpad1.Buttons.B == ButtonState.Pressed))
            {
                Content.Unload();
                LoadingScreen.Load(ScreenManager, true, null, new BackgroundScreen(), new MainMenuScreen());
            }

        }

        //Input
        private float Input(GameTime gameTime, float timer)
        {
            KeyboardState keyb = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            GamePadState gpad1 = GamePad.GetState(PlayerIndex.One);
            Vector3 mouseP = Initial;
            Vector3 mouseR = Initial;
            Vector2 mouseC;

            Rectangle BallRectangle = new Rectangle(50, screenHeight - 220, 200, 200);
            Rectangle Hyp = new Rectangle(250, screenHeight - 220, 60, 65);

            #region Mouse
            mouseP.X = mouseState.X;
            mouseP.Y = mouseState.Y;
            mouseC.X = BallRectangle.Center.X;
            mouseC.Y = BallRectangle.Center.Y;


            if ((mouseP.X >= Hyp.Left) && (mouseP.X <= Hyp.Right) && (mouseP.Y >= Hyp.Top) && (mouseP.Y <= Hyp.Bottom) && (mouseState.LeftButton == ButtonState.Pressed))
            {
                hypButton = Content.Load<Texture2D>("Images\\p1hypclk");
                if (hyperbool == true)
                {
                    Random x = new Random();
                    Random y = new Random();
                    play.position.X = x.Next(-2000, 2000);
                    play.position.Y = y.Next(-2000, 2000);
                    hyperSpace = 0;
                    HyperSpace.Play();
                }
                hyperbool = false;
            }

            if ((mouseP.X >= BallRectangle.Left) && (mouseP.X <= BallRectangle.Right) && (mouseP.Y >= BallRectangle.Top) && (mouseP.Y <= BallRectangle.Bottom) && (mouseState.LeftButton == ButtonState.Pressed))
            {
                #region Thrust
                if ((mouseP.Y < mouseC.Y + 20) && (mouseP.X > mouseC.X - 20) && (mouseP.X < mouseC.X + 20))
                {

                    thrustclicked = true;
                    //Play engine sound only when the engine is on.
                    if (!EnginePlaying)
                    {
                        if (EngineInstance == null)
                        {
                            EngineInstance = Engine.Play();
                            EngineInstance.Volume = 1.0f;
                        }
                        else
                            EngineInstance.Resume();
                        EnginePlaying = true;
                    }
                    else if (EnginePlaying)
                    {
                        EngineInstance.Play();
                        EnginePlaying = false;
                    }
                    UpdateInput();

                }
                #endregion

                #region Spin Left
                if (mouseP.X > mouseC.X + 20)
                {

                    play.rotation -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * MathHelper.ToRadians(0.2f);
                    leftclicked = true;

                }
                #endregion
                #region Spin Right
                if (mouseP.X < mouseC.X - 20)
                {

                    play.rotation += (float)gameTime.ElapsedGameTime.TotalMilliseconds * MathHelper.ToRadians(0.2f);
                    leftclicked = true;

                }
                #endregion
                #region Shoot
                if ((mouseP.Y > mouseC.Y - 20) && (mouseP.X > mouseC.X - 20) && (mouseP.X < mouseC.X + 20) && (timer > interval))
                {
                    shootclicked = true;
                    for (int i = 0; i <= noBullets - 1; i++)
                    {

                        if (!BulletList[i].AmmoFlying)
                        {
                            BulletList[i] = new Projectile(ammo, play.position, play.rotation, Initial, true, Initial);
                            BulletList[i].ammoVelocityAdd.X = -(float)Math.Sin(BulletList[i].AmmoAngle);
                            BulletList[i].ammoVelocityAdd.Y = +(float)Math.Cos(BulletList[i].AmmoAngle);
                            BulletList[i].ammoVelocityAdd *= 100;
                            BulletList[i].AmmoVelocity += BulletList[i].ammoVelocityAdd;
                            WeaponFire.Play();
                            break;
                        }
                        timer = 0;
                    }
                }
                #endregion

            }


            #endregion

            #region pause
            if (keyb.IsKeyDown(Keys.P) || (gpad1.Buttons.Start == ButtonState.Pressed))
            {
                paused = true;
            }

            

            #endregion

            #region HyperSpace

            if (keyb.IsKeyDown(Keys.H) || (gpad1.Buttons.Y == ButtonState.Pressed))
            {
                if (hyperSpace == 1)
                {
                    hypButton = Content.Load<Texture2D>("Images\\p1hypclk");
                    hyperbool = false;
                    Random x = new Random();
                    Random y = new Random();
                    play.position.X = x.Next(-2000, 2000);
                    play.position.Y = y.Next(-2000, 2000);
                    hyperSpace = 0;
                    HyperSpace.Play();
                }
            }
            #endregion

            #region Left Right
            if (keyb.IsKeyDown(Keys.Left) || (gpad1.ThumbSticks.Left.X < 0.0f))
            {

                play.rotation += (float)gameTime.ElapsedGameTime.TotalMilliseconds * MathHelper.ToRadians(0.2f);
            }

            if (keyb.IsKeyDown(Keys.Right) || (gpad1.ThumbSticks.Left.X > 0.0f))
            {

                play.rotation -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * MathHelper.ToRadians(0.2f);
            }
            #endregion

            #region Thrust
            if (keyb.IsKeyDown(Keys.Space) || (gpad1.Triggers.Left == 1.0f))
            {

                //Play engine sound only when the engine is on.
                if (!EnginePlaying)
                {
                    if (EngineInstance == null)
                    {
                        EngineInstance = Engine.Play();
                        EngineInstance.Volume = 1.0f;
                    }
                    else
                        EngineInstance.Resume();
                    EnginePlaying = true;
                }
                else if (EnginePlaying)
                {
                    EngineInstance.Play();
                    EnginePlaying = false;
                }


                UpdateInput();
            }
            #endregion

            #region Shoot

            if (((keyb.IsKeyDown(Keys.Enter)) || (gpad1.Triggers.Right == 1.0f)) && (timer > interval))
            {
                for (int i = 0; i <= noBullets - 1; i++)
                {

                    if (!BulletList[i].AmmoFlying)
                    {
                        BulletList[i] = new Projectile(ammo, play.position, play.rotation, Initial, true, Initial);
                        BulletList[i].ammoVelocityAdd.X = -(float)Math.Sin(BulletList[i].AmmoAngle);
                        BulletList[i].ammoVelocityAdd.Y = +(float)Math.Cos(BulletList[i].AmmoAngle);
                        BulletList[i].ammoVelocityAdd *= 100;
                        BulletList[i].AmmoVelocity += BulletList[i].ammoVelocityAdd;
                        WeaponFire.Play();
                        break;
                    }
                    timer = 0;
                }
            }
            #endregion

            #region Position Check And Velocity

            play.position += play.velocity;
            play.velocity *= 0.95f;

            for (int i = 0; i <= NumAsteroid1 - 1; i++)
            {
                asteroid1List[i].position = check.RoidPosition(screenWidth, screenHeight, asteroid1List[i].position);
            }


            for (int i = 0; i <= NumAsteroid2 - 1; i++)
            {
                if (asteroid2List[i].isAlive)
                {
                    asteroid2List[i].position = check.RoidPosition(screenWidth, screenHeight, asteroid2List[i].position);
                }
            }

            for (int i = 0; i <= NumAsteroid3 - 1; i++)
            {
                if (asteroid3List[i].isAlive)
                {
                    asteroid3List[i].position = check.RoidPosition(screenWidth, screenHeight, asteroid3List[i].position);
                }
            }

            play.position = check.Position(screenWidth, screenHeight, play.position);

            #endregion

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            return timer;
        }

        //Detect Collisions for everything
        private void DetectCollisions()
        {
            #region Player vs Asteroids
            if (spawntimer >= spawninterval)
            {
                #region Asteroid1
                for (int i = 0; i <= NumAsteroid1 - 1; i++)
                {

                    collide = collision.playerCollide(myModel, Asteroid1, asteroid1List[i].position, play.position, Content);
                    if (collide == true)
                    {
                        spawntimer = 0;
                        while (collide == true)
                        {
                            Random x = new Random();
                            Random y = new Random();
                            play.position.X = x.Next(-2000, 2000);
                            play.position.Y = y.Next(-2000, 2000);
                            collide = collision.playerCollide(myModel, Asteroid1, asteroid1List[i].position, play.position, Content);
                        }
                        collide = false;
                        play.Lives -= 1;
                        if (play.Lives == 0)
                        {
                            dead = true;
                        }
                    }
                }
                #endregion
            }
            if (spawntimer >= spawninterval)
            {
                #region Asteroid2
                for (int i = 0; i <= NumAsteroid2 - 1; i++)
                {

                    collide = collision.playerCollide(myModel, Asteroid2, asteroid2List[i].position, play.position, Content);
                    if (collide == true)
                    {
                        spawntimer = 0;
                        while (collide == true)
                        {
                            Random x = new Random();
                            Random y = new Random();
                            play.position.X = x.Next(-2000, 2000);
                            play.position.Y = y.Next(-2000, 2000);
                            collide = collision.playerCollide(myModel, Asteroid2, asteroid2List[i].position, play.position, Content);
                        }
                        collide = false;
                        play.Lives -= 1;
                        if (play.Lives == 0)
                        {
                            dead = true;
                        }
                    }
                }
                #endregion
            }
            if (spawntimer >= spawninterval)
            {
                #region Asteroid3
                for (int i = 0; i <= NumAsteroid3 - 1; i++)
                {

                    collide = collision.playerCollide(myModel, Asteroid3, asteroid3List[i].position, play.position, Content);
                    if (collide == true)
                    {
                        spawntimer = 0;
                        while (collide == true)
                        {
                            Random x = new Random();
                            Random y = new Random();
                            play.position.X = x.Next(-2000, 2000);
                            play.position.Y = y.Next(-2000, 2000);
                            collide = collision.playerCollide(myModel, Asteroid3, asteroid3List[i].position, play.position, Content);
                        }
                        collide = false;
                        play.Lives -= 1;
                        if (play.Lives == 0)
                        {
                            dead = true;
                        }
                    }
                }
                #endregion
            }

            #endregion

            #region Asteroids vs Bullets

            #region Asteroid1
            for (int i = 0; i <= NumAsteroid1 - 1; i++)
            {
                bulletCollide = collision.BulletsCollide(ammo, Asteroid1, asteroid1List[i].position, BulletList[0].AmmoModelPosition, Content);
                if (bulletCollide == true)
                {
                    Score += 10;
                    #region Remove bullets
                    bulletCollide = false;
                    BulletList[0].AmmoModelPosition = new Vector3(-5000, -5000f, 0.0f);
                    BulletList[0].AmmoFlying = false;
                    asteroid1List[i].lives -= 1;
                    #endregion

                    #region Out of lives
                    if (asteroid1List[i].lives == 0)
                    {
                        Score += 100;

                        asteroidsonscreen -= 1;
                        Explosion.Play(0.5F);
                        SpawnAsteroids(asteroid1List[i].asteroidModel, asteroid1List[i].position);

                        asteroid1List[i] = new Asteroid(Asteroid1, new Vector3(8000, 8000, 0), 0.0f, false, 0);

                    }
                    #endregion
                }
            }
            #endregion

            #region Asteroid 2
            for (int i = 0; i <= NumAsteroid2 - 1; i++)
            {
                bulletCollide2 = collision.BulletsCollide(ammo, Asteroid2, asteroid2List[i].position, BulletList[0].AmmoModelPosition, Content);
                if (bulletCollide2 == true)
                {
                    Score += 10;
                    #region Remove bullets
                    BulletList[0].AmmoModelPosition = new Vector3(-5000f, -5000f, 0.0f);
                    BulletList[0].AmmoFlying = false;
                    bulletCollide2 = false;
                    asteroid2List[i].lives -= 1;
                    #endregion


                    #region Out of lives
                    if (asteroid2List[i].lives == 0)
                    {
                        Score += 50;

                        Explosion.Play(0.5F);
                        SpawnAsteroids(asteroid2List[i].asteroidModel, asteroid2List[i].position);

                        asteroid2List[i] = new Asteroid(Asteroid2, new Vector3(8000, 8000, 0), 0.0f, false, 0);
                        asteroidsonscreen -= 1;


                    }
                    #endregion
                }
            }
            #endregion

            #region Asteroid3
            for (int i = 0; i <= NumAsteroid3 - 1; i++)
            {
                bulletCollide3 = collision.BulletsCollide(ammo, Asteroid3, asteroid3List[i].position, BulletList[0].AmmoModelPosition, Content);
                if (bulletCollide3 == true)
                {
                    Score += 10;
                    #region Remove bullets
                    BulletList[0].AmmoModelPosition = new Vector3(-5000f, -5000f, 0.0f);
                    BulletList[0].AmmoFlying = false;
                    bulletCollide3 = false;
                    asteroid3List[i].lives -= 1;
                    #endregion


                    #region Out of lives
                    if (asteroid3List[i].lives == 0)
                    {
                        Score += 20;
                        asteroid3List[i] = new Asteroid(Asteroid3, new Vector3(8000, 8000, 0), 0.0f, false, 0);
                        asteroidsonscreen -= 1;
                        //just using asteroidlist1 at the moment, adapt for all models


                        Explosion.Play(0.5F);

                    }
                    #endregion
                }
            }
            #endregion

            #endregion
        }

        //Break up Asteroids
        private void SpawnAsteroids(Model passedModel, Vector3 position)
        {
            #region spawn 2 asteroid2
            if (passedModel == Asteroid1)
            {
                asteroidsonscreen += 2;
                int A2cnt = 0;
                for (int i = 0; i <= NumAsteroid2 - 1; i++)
                {
                    if (asteroid2List[i].isAlive == false)
                    {
                        asteroid2List[i] = new Asteroid(Asteroid2, position, 0.005f, true, 3);
                        A2cnt++;
                        if (A2cnt >= 2)
                            break;
                    }
                }
            }
            #endregion

            #region Spawn 2 asteroid3
            if (passedModel == Asteroid2)
            {
                asteroidsonscreen += 2;
                int A3cnt = 0;
                for (int i = 0; i <= NumAsteroid3 - 1; i++)
                {

                    if (asteroid3List[i].isAlive == false)
                    {
                        asteroid3List[i] = new Asteroid(Asteroid3, position, 0.005f, true, 1);
                        A3cnt++;
                        if (A3cnt >= 2)
                            break;

                    }
                }
            }
            #endregion
        }
    
    }
}
