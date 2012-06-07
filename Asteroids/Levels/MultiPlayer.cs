using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Asteroids.Screens;
using Asteroids.LevelData;

namespace Asteroids.Levels
{
    class MultiPlayer:GameScreen
    {
        #region Classes
        ContentManager Content;
        CheckPositions check = new CheckPositions();
        CollisionDetection collision = new CollisionDetection();
        Player play;
        Player2 play2;
        SpriteFont defaultFont;
        globe Globe = new globe();
        #endregion

        #region Texture2D and Rectangles
        public Texture2D BackGround;                            // 
        public Texture2D dataScreen;
        public Texture2D panel;
        public Texture2D panel2;
        public Texture2D hypButton;
        public Texture2D hypButton2;
        

        Rectangle Hyp;
        Rectangle Hyp2;
        Rectangle BallRectangle;
        Rectangle Ball2Rectangle;

        Rectangle ScoreRectangle = new Rectangle(440, 0, 125, 65);
        Rectangle DataRectangle = new Rectangle(0, 0, 168, 60);
        Rectangle Data2Rectangle = new Rectangle(732, 0, 168, 60);

        #endregion

        #region Sound Effects
        public SoundEffect Engine;                              //declare sound effects
        public SoundEffect WeaponFire;                          //
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
        Projectile[] Bullet2List;
        Player[] playerlist = new Player[noPlayers];
        
        #endregion

        #region ModelData

        public Vector3 globePosition = new Vector3(-2500, -1500, 1050);
        public float globeRotation = 0.0002f;
        public Model myModel;
        public Model Player2;
        public Model Asteroid1;
        public Model Asteroid2;
        public Model Asteroid3;
        public Model ammo;
        public Model globe;
        #endregion

        #region Bools
        public bool AmmoFlying = false;
        public bool bulletCollide = false;
        public bool bulletCollide2 = false;
        public bool bulletCollide3 = false;
        bool EnginePlaying = false;
        public bool soundEnginePlaying = false;
        public bool hyperbool = true;
        public bool hyperbool2 = true;
        public bool paused = false;
        public bool p1dead = false;
        public bool p2dead = false;
        public bool leftclicked = false;
        public bool rightclicked = false;
        public bool thrustclicked = false;
        public bool shootclicked = false;
        bool collide;
        bool pBulletCollide;
        #endregion

        #region Vectors
        Vector3 Initial = Vector3.Zero;
        Vector3 cameraPosition = new Vector3(0.0f, 50.0f, 8500.0f);
        Vector3 AP4;
        Vector3 AP3;
        Vector3 AP2;
        Vector3 AP1;
        Vector3 Player1Start = new Vector3(-2000, 0, 0);
        Vector3 Player2Start = new Vector3(2000, 0, 0); 
        #endregion

        #region Floats Ints and Doubles
        public float spawntimer = 2500.0f;
        public float spawntimer2 = 2500.0f;
        public float spawninterval = 2500.0f;
        public float aspectRatio;
        public int screenWidth;
        public int screenHeight;
        public float EngineThrust = 0.0f;
        public float timer = 500.0f;
        public float timer2 = 500.0f;
        public int hyperSpace = 1;
        public int hyperSpace2 = 1;
        public int asteroidsonscreen = 4;
        float interval = 500.0f;
        #endregion

        public override void LoadContent()
        {

            if(Content ==null)
                Content = new ContentManager(ScreenManager.Game.Services,"Content");
           
            #region loaded content
            
            myModel = Content.Load<Model>("Model\\fighter");
            Player2 = Content.Load<Model>("Model\\Player2");
            Asteroid1 = Content.Load<Model>("Model\\Asteroid1");
            Asteroid2 = Content.Load<Model>("Model\\Asteroid2");
            Asteroid3 = Content.Load<Model>("Model\\Asteroid3");
            globe = Content.Load<Model>("Model\\globe");

            Explosion = Content.Load<SoundEffect>("Audio\\explosion3");

            dataScreen = Content.Load<Texture2D>("Images\\DataScreen");
            BackGround = Content.Load<Texture2D>("Images\\background");
            panel      = Content.Load<Texture2D>("Images\\panel");
            panel2 = Content.Load<Texture2D>("Images\\panel2");
            hypButton = Content.Load<Texture2D>("Images\\p1hyp");
            hypButton2 = Content.Load<Texture2D>("Images\\p2hypclk");

            ammo = Content.Load<Model>("Model\\ammo");
            Engine = Content.Load<SoundEffect>("Audio\\engine_3");
            WeaponFire = Content.Load<SoundEffect>("Audio\\pdp1_fire");
            defaultFont = Content.Load<SpriteFont>("Fonts\\defaultFont");
            HyperSpace = Content.Load<SoundEffect>("Audio\\hax2_fire_alt");
           
            play = new Player(myModel, Player1Start, 0.0f,10);
            play2 = new Player2(Player2, Player2Start, 0.0f,10);
           
            Projectile bullet = new Projectile(ammo, Initial, 0.0f, Initial, false, Initial);
            BulletList = new Projectile[noBullets];
            Bullet2List = new Projectile[noBullets];
            
            #endregion

            #region Graphic Device and screen elements
            screenWidth =  ScreenManager.GraphicsDevice.PresentationParameters.BackBufferWidth;
            screenHeight = ScreenManager.GraphicsDevice.PresentationParameters.BackBufferHeight;
            aspectRatio = ScreenManager.GraphicsDevice.Viewport.AspectRatio;
            #endregion

            #region initialise Asteroids
            AP1.X = 0;
            AP1.Y = 0;

            AP2.X = -2500;
            AP2.Y = -2000;

            AP3.X = -1500;
            AP3.Y = -1500;

            AP4.X = -300;
            AP4.Y = 1500;

            asteroid1List[0] = new Asteroid(Asteroid1, AP1, 0.005f, true, 5);
            asteroid1List[1] = new Asteroid(Asteroid1, AP2, 1.5f, true, 5);

            asteroid3List[0] = new Asteroid(Asteroid3, AP3, 0.005f, true, 1);
            asteroid3List[1] = new Asteroid(Asteroid3, AP4, 0.5f, true, 1);
            asteroid3List[2] = new Asteroid(Asteroid3, new Vector3(2500, 1900, 0), 1.25f, true, 1);
            asteroid3List[3] = new Asteroid(Asteroid3, new Vector3(300, 2500, 0), 2.0f, true, 1);
            asteroid3List[4] = new Asteroid(Asteroid3, new Vector3(-2500, 1500, 0), 2.5f, true, 1);
            asteroid3List[5] = new Asteroid(Asteroid3, new Vector3(-500, 200, 0), 3.0f, true, 1);
            asteroid3List[6] = new Asteroid(Asteroid3, new Vector3(-1500, 2000, 0), 3.5f, true, 1);
            asteroid3List[7] = new Asteroid(Asteroid3, new Vector3(1500, -2500, 0), 4.0f, true, 1);

            for (int i = 0; i < NumAsteroid2; i++)
                asteroid2List[i] = new Asteroid(Asteroid2, new Vector3(4000, 4000, 0), 0.005f, false, 3);

            for (int i = 8; i < NumAsteroid3; i++)
                asteroid3List[i] = new Asteroid(Asteroid3, new Vector3(4000, 4000, 0), 0.005f, false, 1);




            #endregion
            #region initialise bullet list
            for (int i = 0; i < noBullets; i++)
            {
                BulletList[i] = new Projectile(ammo, new Vector3(5000, 5000, 0), 0.0f, Initial, false, Initial);
            }
            #endregion

            #region initialise bullet 2 list
            for (int i = 0; i < noBullets; i++)
            {
                Bullet2List[i] = new Projectile(ammo, new Vector3(5000,5000,0), 0.0f, Initial, false,Initial);
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

                    #region Player Dead
                    if (p1dead == true)
                    {
                        LoadingScreen.Load(ScreenManager, false, null, new P2WinBGScreen(), new P2WinGameScreen());
                    }
                    if (p2dead == true)
                    {
                        LoadingScreen.Load(ScreenManager, false, null, new P1WinBGScreen(), new P1WinGameScreen());
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

                    #region Bullet 2 Timeout
                    for (int j = 0; j <= noBullets - 1; j++)
                    {
                        Bullet2List[j].AmmoModelPosition += Bullet2List[j].AmmoVelocity;
                        Bullet2List[j].timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (Bullet2List[j].timer > interval)
                        {
                            Bullet2List[j].timer = 0.0f;
                            Bullet2List[j].AmmoFlying = false;
                        }

                    }
                    #endregion

                    DetectCollisions();

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
                    spawntimer2 += (float)gameTime.ElapsedGameTime.Milliseconds;
                    spawntimer += (float)gameTime.ElapsedGameTime.Milliseconds;
                    #endregion
                }

                if (paused == true)
                {

                    PausedInput();
                }
            }
            catch (Exception ex)
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
                    #region Player 1
                    ScreenManager.spriteBatch.Draw(dataScreen, DataRectangle, Color.White);

                    ScreenManager.SpriteBatch.DrawString(defaultFont, "Lives", new Vector2(15, 5), Color.Red);
                    ScreenManager.SpriteBatch.DrawString(defaultFont, play.Lives.ToString(), new Vector2(75, 5), Color.Red);

                    ScreenManager.SpriteBatch.DrawString(defaultFont, "HyperSpace", new Vector2(15, 25), Color.Red);
                    ScreenManager.SpriteBatch.DrawString(defaultFont, hyperSpace.ToString(), new Vector2(140, 25), Color.Red);
                    #endregion

                    #region Player 2
                    ScreenManager.spriteBatch.Draw(dataScreen, Data2Rectangle, Color.White);

                    ScreenManager.SpriteBatch.DrawString(defaultFont, "Lives", new Vector2(750, 5), Color.Blue);
                    ScreenManager.SpriteBatch.DrawString(defaultFont, play2.Lives.ToString(), new Vector2(825, 5), Color.Blue);

                    ScreenManager.SpriteBatch.DrawString(defaultFont, "HyperSpace", new Vector2(750, 25), Color.Blue);
                    ScreenManager.SpriteBatch.DrawString(defaultFont, hyperSpace2.ToString(), new Vector2(875, 25), Color.Blue);
                    #endregion
                    #endregion

                    ScreenManager.spriteBatch.End();

                    ScreenManager.spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
                    Rectangle Hyp = new Rectangle(250, screenHeight - 220, 60, 65);
                    Rectangle Hyp2 = new Rectangle(screenWidth - 330, screenHeight - 220, 60, 65);


                    Rectangle BallRectangle = new Rectangle(50, screenHeight - 220, 200, 200);
                    Rectangle Ball2Rectangle = new Rectangle(screenWidth - 250, screenHeight - 220, 200, 200);

                    ScreenManager.spriteBatch.Draw(hypButton, Hyp, Color.White);
                    ScreenManager.spriteBatch.Draw(hypButton2, Hyp2, Color.White);

                    ScreenManager.spriteBatch.Draw(panel, BallRectangle, Color.White);
                    ScreenManager.spriteBatch.Draw(panel2, Ball2Rectangle, Color.White);
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
            Globe.drawGlobe(aspectRatio, cameraPosition, globeRotation, globePosition, globe);
            #region Ammo
            for (int j = 0; j <= noBullets - 1; j++)
            {
                if (BulletList[j].AmmoFlying)
                {
                    BulletList[j].DrawAmmo(aspectRatio, cameraPosition);
                }
            };
            #endregion

            #region Ammo
            for (int j = 0; j <= noBullets - 1; j++)
            {
                if (Bullet2List[j].AmmoFlying)
                {
                    Bullet2List[j].DrawAmmo(aspectRatio, cameraPosition);
                }
            };
            #endregion

            #region Player
            // this if statement adds a flicker to the player when their hit
            if ((spawntimer >= spawninterval) || (spawntimer > 100 && spawntimer < 200) ||
                (spawntimer > 100 && spawntimer < 300) || (spawntimer > 500 && spawntimer < 700) ||
                (spawntimer > 900 && spawntimer < 1100) || (spawntimer > 1300 && spawntimer < 1500) ||
                (spawntimer > 1700 && spawntimer < 1900) || (spawntimer > 2100 && spawntimer < 2300))
                play.DrawPlayer(aspectRatio, cameraPosition);
            #endregion

            #region Player
            // this if statement adds a flicker to the player when their hit
            if ((spawntimer2 >= spawninterval) || (spawntimer2 > 100 && spawntimer2 < 200) ||
                (spawntimer2 > 100 && spawntimer2 < 300) || (spawntimer2 > 500 && spawntimer2 < 700) ||
                (spawntimer2 > 900 && spawntimer2 < 1100) || (spawntimer2 > 1300 && spawntimer2 < 1500) ||
                (spawntimer2 > 1700 && spawntimer2 < 1900) || (spawntimer2 > 2100 && spawntimer2 < 2300))
            play2.DrawPlayer(aspectRatio, cameraPosition);
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
            Vector3 velocityAdd = Vector3.Zero;

            // Find out what direction we should be thrusting, using rotation.
            velocityAdd.X = -(float)Math.Sin(play.rotation);
            velocityAdd.Y = +(float)Math.Cos(play.rotation);

            if (currentKeyState.IsKeyDown(Keys.Space) || (gpad1.Triggers.Left == 1.0f))
                velocityAdd *= 1.5f;
            // Finally, add this vector to our velocity.
            play.velocity += velocityAdd;
        }

        //Update Data for model 2 thrust etc...
        protected void UpdateInput2()
        {
            KeyboardState currentKeyState = Keyboard.GetState();
            GamePadState gpad2 = GamePad.GetState(PlayerIndex.Two);
            Vector3 velocityAdd = Vector3.Zero;

            // Find out what direction we should be thrusting, using rotation.
            velocityAdd.X = -(float)Math.Sin(play2.rotation);
            velocityAdd.Y = +(float)Math.Cos(play2.rotation);

            if (currentKeyState.IsKeyDown(Keys.W) || (gpad2.Triggers.Left == 1.0f))
                velocityAdd *= 1.5f;
            // Finally, add this vector to our velocity.
            play2.velocity += velocityAdd;
        }

        //Input at the Pause screen
        private void PausedInput()
        {
            KeyboardState keyb = Keyboard.GetState();
            GamePadState gpad1 = GamePad.GetState(PlayerIndex.One);
            GamePadState gpad2 = GamePad.GetState(PlayerIndex.Two);

            if (keyb.IsKeyDown(Keys.Back) || (gpad1.Buttons.Back == ButtonState.Pressed)||(gpad2.Buttons.Back==ButtonState.Pressed))
            {
                BackGround = Content.Load<Texture2D>("Images\\background");
                paused = false;
            }
            if (keyb.IsKeyDown(Keys.Escape) || (gpad1.Buttons.B == ButtonState.Pressed) || (gpad2.Buttons.B == ButtonState.Pressed))
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
            GamePadState gpad2 = GamePad.GetState(PlayerIndex.Two);

            Vector3 mouseP = Initial;
            Vector3 mouseR = Initial;
            Vector2 mouseC;
            Vector2 mouseC2;

            Rectangle Hyp = new Rectangle(250, screenHeight - 220, 60, 65);
            Rectangle Hyp2 = new Rectangle(screenWidth - 330, screenHeight - 220, 60, 65);
            Rectangle BallRectangle = new Rectangle(50, screenHeight - 220, 200, 200);
            Rectangle Ball2Rectangle = new Rectangle(screenWidth - 250, screenHeight - 220, 200, 200);

            mouseP.X = mouseState.X;
            mouseP.Y = mouseState.Y;
            mouseC.X = BallRectangle.Center.X;
            mouseC.Y = BallRectangle.Center.Y;
            mouseC2.X = Ball2Rectangle.Center.X;
            mouseC2.Y = Ball2Rectangle.Center.Y;

            #region Mouse1
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

            #region Mouse2

            if ((mouseP.X >= Hyp2.Left) && (mouseP.X <= Hyp2.Right) && (mouseP.Y >= Hyp2.Top) && (mouseP.Y <= Hyp2.Bottom) && (mouseState.LeftButton == ButtonState.Pressed))
            {
                hypButton2 = Content.Load<Texture2D>("Images\\p2hyp");
                if (hyperbool2 == true)
                {
                    Random x = new Random();
                    Random y = new Random();
                    play2.position.X = x.Next(-2000, 2000);
                    play2.position.Y = y.Next(-2000, 2000);

                    hyperSpace2 = 0;
                    HyperSpace.Play();
                }
                hyperbool2 = false;
            }

            if ((mouseP.X >= Ball2Rectangle.Left) && (mouseP.X <= Ball2Rectangle.Right) && (mouseP.Y >= Ball2Rectangle.Top) && (mouseP.Y <= Ball2Rectangle.Bottom) && (mouseState.LeftButton == ButtonState.Pressed))
            {
                #region Thrust
                if ((mouseP.Y < mouseC2.Y + 20) && (mouseP.X > mouseC2.X - 20) && (mouseP.X < mouseC2.X + 20))
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
                    UpdateInput2();

                }
                #endregion

                #region Spin Left
                if (mouseP.X > mouseC2.X + 20)
                {
                    
                    play2.rotation -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * MathHelper.ToRadians(0.2f);
                    leftclicked = true;

                }
                #endregion

                #region Spin Right
                if (mouseP.X < mouseC2.X - 20)
                {
                    
                    play2.rotation += (float)gameTime.ElapsedGameTime.TotalMilliseconds * MathHelper.ToRadians(0.2f);
                    leftclicked = true;

                }
                #endregion
                #region Shoot
                if ((mouseP.Y > mouseC2.Y - 20) && (mouseP.X > mouseC2.X - 20) && (mouseP.X < mouseC2.X + 20) && (timer2 > interval))
                {
                    shootclicked = true;
                    for (int i = 0; i <= noBullets - 1; i++)
                    {

                        if (!Bullet2List[i].AmmoFlying)
                        {
                            Bullet2List[i] = new Projectile(ammo, play2.position, play2.rotation, Initial, true, Initial);
                            Bullet2List[i].ammoVelocityAdd.X = -(float)Math.Sin(Bullet2List[i].AmmoAngle);
                            Bullet2List[i].ammoVelocityAdd.Y = +(float)Math.Cos(Bullet2List[i].AmmoAngle);
                            Bullet2List[i].ammoVelocityAdd *= 100;
                            Bullet2List[i].AmmoVelocity += Bullet2List[i].ammoVelocityAdd;
                            WeaponFire.Play();
                            break;
                        }
                        timer2 = 0;
                    }
                }
                #endregion

            }

            #endregion


            #region pause
            if (keyb.IsKeyDown(Keys.P)||(gpad1.Buttons.Start==ButtonState.Pressed)||(gpad2.Buttons.Start==ButtonState.Pressed))
            {
               paused=true;
            }
            #endregion

            #region HyperSpace player 1 and player 2
            if (keyb.IsKeyDown(Keys.H)|| (gpad1.Buttons.Y == ButtonState.Pressed))
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


            if (keyb.IsKeyDown(Keys.G)|| (gpad2.Buttons.Y == ButtonState.Pressed))
            {
                if (hyperSpace2 == 1)
                {
                    hypButton2 = Content.Load<Texture2D>("Images\\p2hyp");
                    hyperbool2 = false;
                    Random x = new Random();
                    Random y = new Random();
                    play2.position.X = x.Next(-2000, 2000);
                    play2.position.Y = y.Next(-2000, 2000);

                    hyperSpace2 = 0;
                    HyperSpace.Play();
                }
            }
            #endregion

            #region Left Right + 'A' 'D'
            #region Player1
            if (keyb.IsKeyDown(Keys.Left) || (gpad1.ThumbSticks.Left.X < -0.0f))
            {
                
                play.rotation += (float)gameTime.ElapsedGameTime.TotalMilliseconds * MathHelper.ToRadians(0.2f);
            }

            if (keyb.IsKeyDown(Keys.Right) || (gpad1.ThumbSticks.Left.X > 0.0f))
            {
                
                play.rotation -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * MathHelper.ToRadians(0.2f);
            } 
            #endregion

            #region Player 2
            if (keyb.IsKeyDown(Keys.A) || (gpad2.ThumbSticks.Left.X < 0.0f))
            {
                
                play2.rotation += (float)gameTime.ElapsedGameTime.TotalMilliseconds * MathHelper.ToRadians(0.2f);
            }
            if (keyb.IsKeyDown(Keys.D) || (gpad2.ThumbSticks.Left.X > 0.0f))
            {
                
                play2.rotation -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * MathHelper.ToRadians(0.2f);
            } 
            #endregion
            #endregion

            #region Thrust and 'W'
            if (keyb.IsKeyDown(Keys.Space)|| (gpad1.Triggers.Left == 1.0f))
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
            if (keyb.IsKeyDown(Keys.W)|| (gpad2.Triggers.Left == 1.0f))
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
                UpdateInput2();
            }

            #endregion

            #region Shoot(enter) + shoot (F)

            if (((keyb.IsKeyDown(Keys.Enter))|| (gpad1.Triggers.Right == 1.0f)) && (timer > interval))
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
            if (((keyb.IsKeyDown(Keys.F))|| (gpad2.Triggers.Right == 1.0f)) && (timer2 > interval))
            {
                for (int i = 0; i <= noBullets - 1; i++)
                {

                    if (!Bullet2List[i].AmmoFlying)
                    {
                        Bullet2List[i] = new Projectile(ammo, play2.position, play2.rotation, Initial, true, Initial);
                        Bullet2List[i].ammoVelocityAdd.X = -(float)Math.Sin(Bullet2List[i].AmmoAngle);
                        Bullet2List[i].ammoVelocityAdd.Y = +(float)Math.Cos(Bullet2List[i].AmmoAngle);
                        Bullet2List[i].ammoVelocityAdd *= 100;
                        Bullet2List[i].AmmoVelocity += Bullet2List[i].ammoVelocityAdd;
                        WeaponFire.Play();
                        break;
                    }
                    timer2 = 0;
                }
            }
            #endregion

            #region Position Check And Velocity

            play.position += play.velocity;
            play.velocity *= 0.95f;
            play2.position += play2.velocity;
            play2.velocity *= 0.95f;

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
            play2.position = check.Position(screenWidth, screenHeight, play2.position);

            #endregion

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            timer2 += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            return timer;
        }

        //Detect Collisions for everything
        private void DetectCollisions()
        {
            if (spawntimer >= spawninterval)
            {
                #region Players vs Players

                collide = collision.PlayersvPlayer(myModel, Player2, play.position, play2.position, Content);
                if (collide == true)
                {
                    spawntimer = 0;
                    spawntimer2 = 0;
                    Random x = new Random();
                    Random y = new Random();
                    play.position.X = x.Next(-2000, 2000);
                    play.position.Y = y.Next(-2000, 2000);


                    play2.position.X = x.Next(-2000, 2000);
                    play2.position.Y = y.Next(-2000, 2000);
                    collide = false;
                    play.Lives -= 1;
                    play2.Lives -= 1;
                    if (play.Lives == 0)
                    {
                        p1dead = true;
                    }
                    if (play2.Lives == 0)
                    {
                        p2dead = true;
                    }
                }

                #endregion
            }

            #region Player vs Asteroids
                #region Player1
                
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
                                p1dead = true;
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
                                collide = collision.playerCollide(myModel, Asteroid3, asteroid3List[i].position, play.position, Content);
                            }
                            collide = false;
                            play.Lives -= 1;
                            if (play.Lives == 0)
                            {
                                p1dead = true;
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
                                p1dead = true;
                            }
                            
                        }
                    }
                    #endregion
                }

                #endregion

                #region Player2
                if (spawntimer2 >= spawninterval)
                {
                    #region Asteroid1
                    for (int i = 0; i <= NumAsteroid1 - 1; i++)
                    {

                        collide = collision.playerCollide(Player2, Asteroid1, asteroid1List[i].position, play2.position, Content);
                        if (collide == true)
                        {
                            spawntimer2 = 0;
                            Random x = new Random();
                            Random y = new Random();
                            play2.position.X = x.Next(-2000, 2000);
                            play2.position.Y = y.Next(-2000, 2000);
                            collide = false;
                            play2.Lives -= 1;
                            if (play2.Lives == 0)
                            {
                                p2dead = true;
                            }
                        }
                    }
                    #endregion
                }

                if (spawntimer2 >= spawninterval)
                {
                    #region Asteroid2
                    for (int i = 0; i <= NumAsteroid2 - 1; i++)
                    {

                        collide = collision.playerCollide(Player2, Asteroid2, asteroid2List[i].position, play2.position, Content);
                        if (collide == true)
                        {
                            spawntimer2 = 0;
                            Random x = new Random();
                            Random y = new Random();
                            play2.position.X = x.Next(-2000, 2000);
                            play2.position.Y = y.Next(-2000, 2000);
                            collide = false;
                            play2.Lives -= 1;
                            if (play2.Lives == 0)
                            {
                                p2dead = true;
                            }
                        }
                    }
                    #endregion
                }

                if (spawntimer2 >= spawninterval)
                {
                    #region Asteroid3
                    for (int i = 0; i <= NumAsteroid3 - 1; i++)
                    {

                        collide = collision.playerCollide(Player2, Asteroid3, asteroid3List[i].position, play2.position, Content);
                        if (collide == true)
                        {
                            spawntimer2 = 0;
                            Random x = new Random();
                            Random y = new Random();
                            play2.position.X = x.Next(-2000, 2000);
                            play2.position.Y = y.Next(-2000, 2000);
                            collide = false;
                            play2.Lives -= 1;
                            if (play2.Lives == 0)
                            {
                                p2dead = true;
                            }
                        }
                    }
                    #endregion
                }
                #endregion

            #endregion

            #region Asteroids vs Bullets

                #region Player1
                #region Asteroid1
                for (int i = 0; i <= NumAsteroid1 - 1; i++)
                {
                    bulletCollide = collision.BulletsCollide(ammo, Asteroid1, asteroid1List[i].position, BulletList[0].AmmoModelPosition, Content);
                    if (bulletCollide == true)
                    {
                        #region Remove bullets
                        bulletCollide = false;
                        BulletList[0].AmmoModelPosition = new Vector3(5000, 5000f, 0.0f);
                        BulletList[0].AmmoFlying = false;
                        asteroid1List[i].lives -= 1;
                        #endregion

                        #region Out of lives
                        if (asteroid1List[i].lives == 0)
                        {
                            SpawnAsteroids(asteroid1List[i].asteroidModel, asteroid1List[i].position);

                            asteroid1List[i] = new Asteroid(Asteroid1, new Vector3(8000, 8000, 0), 0.0f, false, 0);

                            asteroidsonscreen -= 1;
                            Explosion.Play(0.5F);

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

                        #region Remove bullets
                        BulletList[0].AmmoModelPosition = new Vector3(5000f, 5000f, 0.0f);
                        BulletList[0].AmmoFlying = false;
                        bulletCollide2 = false;
                        asteroid2List[i].lives -= 1;
                        #endregion


                        #region Out of lives
                        if (asteroid2List[i].lives == 0)
                        {
                            SpawnAsteroids(asteroid2List[i].asteroidModel, asteroid2List[i].position);

                            asteroid2List[i] = new Asteroid(Asteroid2, new Vector3(8000, 8000, 0), 0.0f, false, 0);
                            asteroidsonscreen -= 1;



                            Explosion.Play(0.5F);

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
                        #region Remove bullets
                        BulletList[0].AmmoModelPosition = new Vector3(5000f, 5000f, 0.0f);
                        BulletList[0].AmmoFlying = false;
                        bulletCollide3 = false;
                        asteroid3List[i].lives -= 1;
                        #endregion


                        #region Out of lives
                        if (asteroid3List[i].lives == 0)
                        {

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

                #region Player2
                #region Asteroid1
                for (int i = 0; i <= NumAsteroid1 - 1; i++)
                {
                    bulletCollide = collision.BulletsCollide(ammo, Asteroid1, asteroid1List[i].position, Bullet2List[0].AmmoModelPosition, Content);
                    if (bulletCollide == true)
                    {
                        #region Remove bullets
                        bulletCollide = false;
                        Bullet2List[0].AmmoModelPosition = new Vector3(5000, 5000f, 0.0f);
                        Bullet2List[0].AmmoFlying = false;
                        asteroid1List[i].lives -= 1;
                        #endregion

                        #region Out of lives
                        if (asteroid1List[i].lives == 0)
                        {
                            SpawnAsteroids(asteroid1List[i].asteroidModel, asteroid1List[i].position);

                            asteroid1List[i] = new Asteroid(Asteroid1, new Vector3(8000, 8000, 0), 0.0f, false, 0);

                            asteroidsonscreen -= 1;
                            Explosion.Play(0.5F);

                        }
                        #endregion
                    }
                }
                #endregion

                #region Asteroid 2
                for (int i = 0; i <= NumAsteroid2 - 1; i++)
                {
                    bulletCollide2 = collision.BulletsCollide(ammo, Asteroid2, asteroid2List[i].position, Bullet2List[0].AmmoModelPosition, Content);
                    if (bulletCollide2 == true)
                    {
                        #region Remove bullets
                        Bullet2List[0].AmmoModelPosition = new Vector3(5000f, 5000f, 0.0f);
                        Bullet2List[0].AmmoFlying = false;
                        bulletCollide2 = false;
                        asteroid2List[i].lives -= 1;
                        #endregion


                        #region Out of lives
                        if (asteroid2List[i].lives == 0)
                        {
                            SpawnAsteroids(asteroid2List[i].asteroidModel, asteroid2List[i].position);

                            asteroid2List[i] = new Asteroid(Asteroid2, new Vector3(8000, 8000, 0), 0.0f, false, 0);
                            asteroidsonscreen -= 1;



                            Explosion.Play(0.5F);

                        }
                        #endregion
                    }
                }
                #endregion

                #region Asteroid3
                for (int i = 0; i <= NumAsteroid3 - 1; i++)
                {
                    bulletCollide3 = collision.BulletsCollide(ammo, Asteroid3, asteroid3List[i].position, Bullet2List[0].AmmoModelPosition, Content);
                    if (bulletCollide3 == true)
                    {
                        #region Remove bullets
                        Bullet2List[0].AmmoModelPosition = new Vector3(5000f, 5000f, 0.0f);
                        Bullet2List[0].AmmoFlying = false;
                        bulletCollide3 = false;
                        asteroid3List[i].lives -= 1;
                        #endregion


                        #region Out of lives
                        if (asteroid3List[i].lives == 0)
                        {
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

            #endregion

                if (spawntimer >= spawninterval)
                {
                    #region player vs player2 bullets

                    for (int i = 0; i <= noBullets - 1; i++)
                    {
                        pBulletCollide = collision.pBulletsCollide(ammo, myModel, play.position, Bullet2List[0].AmmoModelPosition, Content);
                        if (pBulletCollide == true)
                        {
                            spawntimer = 0;
                            #region Remove bullets
                            bulletCollide = false;
                            Bullet2List[0].AmmoModelPosition = new Vector3(5000, 5000f, 0.0f);
                            Bullet2List[0].AmmoFlying = false;
                            #endregion

                            Random x = new Random();
                            Random y = new Random();
                            play.position.X = x.Next(-2000, 2000);
                            play.position.Y = y.Next(-2000, 2000);
                            collide = false;
                            play.Lives -= 1;

                            if (play.Lives == 0)
                            {
                                p1dead = true;
                            }
                        }
                    }
                    #endregion
                }
            
            if (spawntimer2 >= spawninterval)
                {
                    #region player2 vs player bullets

                    for (int i = 0; i <= noBullets - 1; i++)
                    {
                        pBulletCollide = collision.pBulletsCollide(ammo, Player2, play2.position, BulletList[0].AmmoModelPosition, Content);
                        if (pBulletCollide == true)
                        {
                            spawntimer2 = 0;
                            #region Remove bullets
                            bulletCollide = false;
                            BulletList[0].AmmoModelPosition = new Vector3(5000, 5000f, 0.0f);
                            BulletList[0].AmmoFlying = false;
                            #endregion

                            Random x = new Random();
                            Random y = new Random();
                            play2.position.X = x.Next(-2000, 2000);
                            play2.position.Y = y.Next(-2000, 2000);

                            collide = false;
                            play2.Lives -= 1;

                            if (play2.Lives == 0)
                            {
                                p2dead = true;
                            }
                        }
                    }


                    #endregion
                }
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
                        if (A2cnt == 2)
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
                        if (A3cnt == 2)
                            break;

                    }
                }
            }
            #endregion
        } 
        
    }
}
    
      