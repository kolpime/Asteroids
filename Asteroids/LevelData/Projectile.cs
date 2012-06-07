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

namespace Asteroids.LevelData
{
    class Projectile
    {
        public float AmmoAngle=0;
        public Model ammo;
        public bool AmmoFlying;
        public Vector3 AmmoModelPosition =Vector3.Zero;
        public Vector3 AmmoModelDirection = Vector3.Zero;
        public Vector3 AmmoVelocity = Vector3.Zero;
        public Vector3 ammoVelocityAdd = Vector3.Zero;
        public float timer =0.0f;

        public Projectile(Model Bullet, Vector3 position, float rotation, Vector3 velocity, bool AmmoFlying, Vector3 ammoVelocityAdd)
        {
            this.timer = 0.0f;
            this.ammo = Bullet;
            this.AmmoModelPosition = position;
            this.AmmoAngle = rotation;
            this.AmmoVelocity = velocity;
            this.AmmoFlying = AmmoFlying;
            this.ammoVelocityAdd = ammoVelocityAdd;
        }

        public void DrawAmmo(float aspectRatio, Vector3 cameraPosition)
        {
            Matrix[] transforms = new Matrix[ammo.Bones.Count];
            ammo.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in ammo.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationZ(AmmoAngle)
                        * Matrix.CreateTranslation(AmmoModelPosition);

                    effect.View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f),
                        aspectRatio, 1.0f, 10000.0f);
                }
                mesh.Draw();
            }

        }

        
    }
}
