using System;
using System.Collections.Generic;
using System.Linq;
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
    class Asteroid
    {
        public Vector3 position = Vector3.Zero;
        public Vector3 direction = Vector3.Zero;
        public float rotation = 0;
        public bool isAlive = true;
        public int lives = 5;
        public int score = 0;

        public Model asteroidModel;

        public Asteroid(Model asteroidModel, Vector3 position,  float rotation, bool isAlive, int lives)
        {
            this.asteroidModel = asteroidModel;
            this.position = position;
            this.rotation = rotation;
            this.isAlive = isAlive;
            this.lives = lives;
        }      


        public void DrawAsteroid(float aspectRatio, Vector3 cameraPosition)
        {
            Matrix[] transforms = new Matrix[asteroidModel.Bones.Count];
            asteroidModel.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in asteroidModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {

                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] 
                        * Matrix.CreateRotationX(rotation) * Matrix.CreateRotationY(rotation)
                        * Matrix.CreateTranslation(position);


                    effect.View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f),
                        aspectRatio, 1.0f, 10000.0f);
                }
                mesh.Draw();
            }
        }
    }
}
