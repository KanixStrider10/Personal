using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace RnadomMapGenerationExperiment {
    class Enemy {
        private Vector2 position;
        private int health;
        private int size;

        private int bulletSpeed = 10;
        private int bulletTimer = 50;
        private int fireRate = 10;
        private int range = 2000;
        private int bulletSize = 10;
        private int damage = 5;

        public Enemy(Vector2 _position, int _health, int _size) {
            position = _position;
            health = _health;
            size = _size;
        }

        public Vector2 Position {
            get {
                return position;
            }
        }

        public int Size {
            get {
                return size;
            }
        }

        public int Health {
            get {
                return health;
            }
            set {
                health = value;
            }    
        }

        public void Draw() {
            ShapeBatch.CircleOutline(position, size / 2, Color.Red);
        }

        public Bullet CreateBullet(Vector2 point) {
            if(bulletTimer == 0) {
                Vector2 totalDistanceVector = point - position;

                double totalDistance = Math.Sqrt(Math.Pow(totalDistanceVector.X, 2) + Math.Pow(totalDistanceVector.Y, 2));

                Vector2 direction = new Vector2();
                direction.X = (float)(bulletSpeed / totalDistance) * totalDistanceVector.X;
                direction.Y = (float)(bulletSpeed / totalDistance) * totalDistanceVector.Y;

                bulletTimer = fireRate;
                return new Bullet(new Vector2(position.X, position.Y), direction, range, bulletSize, damage, Color.Red);
            }
            else { 
                bulletTimer -= 1;
                return null;
            }
        }
    }
}
