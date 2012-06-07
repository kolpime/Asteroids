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
    class CheckPositions
    {
        public Vector3 Position(int screenWidth, int screenHeight, Vector3 modelPosition)
        {
            float screenw = screenWidth * 3.75f;
            float screenh = screenHeight * 3.75f;

            if (modelPosition.X > screenw)
            {
                modelPosition.X = 0 - screenw;
            }

            if (modelPosition.X < -screenw)
            {
                modelPosition.X = screenw;
            }

            if (modelPosition.Y > screenh)
            {
                modelPosition.Y = -(screenh-375);
            }

            if (modelPosition.Y < -(screenh-375))
            {
                modelPosition.Y = screenh;

            }
            return modelPosition;
        }

        public bool ammoPosition(int screenWidth, int screenHeight, Vector3 AmmoModelPosition, bool AmmoFlying)
        {
            float screenw = screenWidth * 3.75f;
            float screenh = screenHeight * 3.75f;

            if (AmmoModelPosition.X > screenw)
            {
                AmmoFlying = false;
            }

            else if (AmmoModelPosition.X < -screenw)
            {
                AmmoFlying = false;
            }

            else if (AmmoModelPosition.Y > screenh)
            {
                AmmoFlying = false;
            }

            else if (AmmoModelPosition.Y < -(screenh - 375))
            {
                AmmoFlying = false;

            }
            else
                AmmoFlying = true;
            return AmmoFlying;
        }

        public Vector3 RoidPosition(int screenWidth, int screenHeight, Vector3 Roid1modelPosition)
        {
            float screenw = screenWidth * 3.75f;
            float screenh = screenHeight * 3.75f;

            if (Roid1modelPosition.X > screenw)
            {
                Roid1modelPosition.X = 0 - screenw;
            }

            if (Roid1modelPosition.X < -screenw)
            {
                Roid1modelPosition.X = screenw;
            }

            if (Roid1modelPosition.Y > screenh)
            {
                Roid1modelPosition.Y = -(screenh - 400);
            }

            if (Roid1modelPosition.Y < -(screenh - 400))
            {
                Roid1modelPosition.Y = screenh;

            }
            return Roid1modelPosition;
        }

        public bool UFOPosition(int screenWidth, int screenHeight, Vector3 UFOPosition, bool UFOFlying)
        {
            float screenw = screenWidth * 3.75f;
            float screenh = screenHeight * 3.75f;

            if (UFOPosition.X > screenw)
            {
                UFOFlying = false;
            }
            else
            UFOFlying = true;
            return UFOFlying;
        }

    }
}
