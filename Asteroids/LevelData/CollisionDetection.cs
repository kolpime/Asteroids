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
using Asteroids.Levels;
using Asteroids.Screens;

namespace Asteroids.LevelData
{
    class CollisionDetection : GameScreen
    {
        public const float AsteroidBoundingSphereScale = 2.0f;  //200% size
        public const float ShipBoundingSphereScale = 1.5f;  //50% size

        SoundEffect soundExplosion3;

        public bool playerCollide(Model player, Model asteroid, Vector3 playerPosition, Vector3 asteroidPosition, ContentManager Content)
        {
            bool collide = false;
            soundExplosion3 = Content.Load<SoundEffect>("Audio\\explosion3");
            BoundingSphere shipSphere = new BoundingSphere(playerPosition, player.Meshes[0].BoundingSphere.Radius * ShipBoundingSphereScale);
            
            BoundingSphere b = new BoundingSphere(asteroidPosition, asteroid.Meshes[0].BoundingSphere.Radius* 2.5f);

    
            if (b.Intersects(shipSphere))
            {
                soundExplosion3.Play();
                collide = true;
            }

            return collide;
        }

        public bool BulletsCollide(Model Bullet, Model asteroid, Vector3 BulletPosition, Vector3 asteroidPosition, ContentManager Content)
        {
            bool collide = false;
            soundExplosion3 = Content.Load<SoundEffect>("Audio\\explosion1");
            BoundingSphere BulletSphereSphere = new BoundingSphere(BulletPosition, Bullet.Meshes[0].BoundingSphere.Radius * ShipBoundingSphereScale);

            BoundingSphere b = new BoundingSphere(asteroidPosition, asteroid.Meshes[0].BoundingSphere.Radius * 2.5f);

            if (b.Intersects(BulletSphereSphere))
            {
                soundExplosion3.Play();
                collide = true;
            }

            return collide;
        }

        public bool PlayersvPlayer(Model p1, Model p2, Vector3 p1Position, Vector3 p2Position, ContentManager Content)
        {
            bool collide = false;
            soundExplosion3 = Content.Load<SoundEffect>("Audio\\explosion3");
            BoundingSphere p1Sphere = new BoundingSphere(p1Position, p1.Meshes[0].BoundingSphere.Radius * ShipBoundingSphereScale);

            BoundingSphere b = new BoundingSphere(p2Position, p2.Meshes[0].BoundingSphere.Radius * 2.5f);

            if (b.Intersects(p1Sphere))
            {
                soundExplosion3.Play();
                collide = true;
            }

            return collide;
        }

        public bool pBulletsCollide(Model Bullet, Model myModel, Vector3 BulletPosition, Vector3 ModelPosition, ContentManager Content)
        {
            bool collide = false;
            soundExplosion3 = Content.Load<SoundEffect>("Audio\\explosion3");
            BoundingSphere BulletSphereSphere = new BoundingSphere(BulletPosition, Bullet.Meshes[0].BoundingSphere.Radius * ShipBoundingSphereScale);

            BoundingSphere b = new BoundingSphere(ModelPosition, myModel.Meshes[0].BoundingSphere.Radius * 2.5f);

            if (b.Intersects(BulletSphereSphere))
            {
                soundExplosion3.Play();
                collide = true;
            }

            return collide;
        }
    }
}
