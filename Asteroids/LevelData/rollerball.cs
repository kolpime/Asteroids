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
    class rollerball
    {
         public void DrawBall(float aspectRatio, Vector3 cameraPosition, Model ball, float xrotation,float yrotation)
        {
            // assign random number to roid1modelPosition.x or y

            // Copy any parent transforms.
            Matrix[] transforms = new Matrix[ball.Bones.Count];
            ball.CopyAbsoluteBoneTransformsTo(transforms);

            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in ball.Meshes)
            {
                // This is where the mesh orientation is set, as well as our camera and projection.
                foreach (BasicEffect effect in mesh.Effects)
                {

                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] //* Matrix.CreateRotationZ(Roid1modelRotation)
                        * Matrix.CreateRotationX(xrotation) * Matrix.CreateRotationY(yrotation)
                        * Matrix.CreateTranslation(new Vector3(-3650,-2450,0));


                    effect.View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f),
                        aspectRatio, 1.0f, 10000.0f);
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
        }
    }
}
