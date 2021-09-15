using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace RnadomMapGenerationExperiment {
    abstract class Pickup {

        /*Pickup Types
         *Health: Red
         *Damage: Green
         *FireRate: Whitish
         *Speed: Yellow
         *Range: Purple
         *BulletSpeed: Light Blue
         *BulletSize: Dark Blue
         */

        private Color color;
        private Vector2 position;

        public Pickup(Vector2 _position) {
            position = _position;
        }

        public bool CheckCollision(Character player) {
            double distanceBetweenCentersSquared = Math.Pow(position.X - player.Position.X, 2) + Math.Pow(position.Y - player.Position.Y, 2);
            double radiiDistanceSquared = Math.Pow((player.Size / 2) + 20, 2);
            if(distanceBetweenCentersSquared < radiiDistanceSquared) {
                UsePickup(player);
                return true;
            }
            return false;
        }

        abstract public void UsePickup(Character player);

        abstract public void Draw();
            
    }
}
