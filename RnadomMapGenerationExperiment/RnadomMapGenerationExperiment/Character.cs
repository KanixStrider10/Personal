using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace RnadomMapGenerationExperiment {
    class Character {
        private Vector2 position;
        private int sideSize;
        
        private int speed = 4;
        private int bulletSpeed = 10;
        private int bulletSize = 10;
        private int range = 500;
        private int fireRate = 10;
        private int damage = 5;
        private int health;
        private int maxHealth = 100;

        private int bulletTimer = 50;

        private bool damageJustTaken = false;

        private List<Color> pickupColors = new List<Color>();

        
        public Character(Vector2 _position, int _sideSize) {
            position = _position;
            sideSize = _sideSize;
            health = maxHealth;
        }

        public Vector2 Position {
            get {
                return position;
            }
            set {
                position = value;
            }
        }

        public int Size {
            get {
                return sideSize;
            }
        }

        public int BulletSize {
            get {
                return bulletSize;
            }
            set {
                bulletSize = value;
            }
        }

        public int BulletSpeed {
            get {
                return bulletSpeed;
            }
            set {
                bulletSpeed = value;
            }
        }

        public int FireRate {
            get {
                return fireRate;
            }
            set {
                fireRate = value;
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

        public int MaxHealth {
            get {
                return maxHealth;
            }
        }

        public int BulletTimer {
            set {
                bulletTimer = value;
            }
        }

        public int Speed {
            get {
                return speed;
            }
            set {
                speed = value;
            }
        }

        public int Damage {
            get {
                return damage;
            }
            set {
                damage = value;
            }
        }

        public int Range {
            get {
                return range;
            }
            set {
                range = value;
            }
        }

        public List<Color> PickupColors {
            get {
                return pickupColors;
            }
        }

        public bool Move(Vector2 moveToMake, Room currentRoom) {
            position += moveToMake * speed;

            if(position.X < 165) {
                if(position.Y + (sideSize / 2) > 522.5 && position.Y + (sideSize / 2) < 852.5 && currentRoom.Doors[3] && currentRoom.Completed) {
                    position.X = 1180;
                    return true;
                }

                position.X = 165;
            }

            if(position.Y < 165) {
                if(position.X + (sideSize / 2) > 522.5 && position.X + (sideSize / 2) < 852.5 && currentRoom.Doors[0] && currentRoom.Completed) {
                    position.Y = 1180;
                    return true;
                }

                position.Y = 165;
            }

            if(position.X > 1180) {
                if(position.Y + (sideSize / 2) > 522.5 && position.Y + (sideSize / 2) < 852.5 && currentRoom.Doors[1] && currentRoom.Completed) {
                    position.X = 165;
                    return true;
                }

                position.X = 1180;
            }

            if(position.Y > 1180) {
                if(position.X + (sideSize / 2) > 522.5 && position.X + (sideSize / 2) < 852.5 && currentRoom.Doors[2] && currentRoom.Completed) {
                    position.Y = 165;
                    return true;
                }

                position.Y = 1180;
            }

            return false;
        }

        public void Draw() {
            ShapeBatch.CircleOutline(new Vector2(position.X + (sideSize / 2), position.Y + (sideSize / 2)), sideSize / 2, Color.White);

            if (damageJustTaken) {
                ShapeBatch.Box(55, 10, ((float)health / maxHealth) * 1265.0f, 35, Color.Red);
                damageJustTaken = false;
            }
            else{
                ShapeBatch.Box(55, 10, ((float)health / maxHealth) * 1265.0f, 35, Color.White); 
            }

            DrawPickupBar();
        }

        public void DrawPickupBar() {
            for(int i = 0; i < pickupColors.Count; i++) {
                if(pickupColors.Count == 1) {
                    ShapeBatch.Box(10, 55, 35, 1265.0f, pickupColors[i]);
                }
                else if(i == pickupColors.Count - 1) {
                    ShapeBatch.Box(10, 55 + (i * 1265.0f / pickupColors.Count), 35, 1265.0f / pickupColors.Count, pickupColors[i]);
                }
                else {
                    ShapeBatch.Box(10, 55 + (i * 1265.0f / pickupColors.Count), 35, (1265.0f / pickupColors.Count) - 10, pickupColors[i]);
                }
            }
        }

        public Bullet CreateBullet(Vector2 point) {
            if(bulletTimer == 0) {
                Vector2 totalDistanceVector = point - position;

                double totalDistance = Math.Sqrt(Math.Pow(totalDistanceVector.X, 2) + Math.Pow(totalDistanceVector.Y, 2));

                Vector2 direction = new Vector2();
                direction.X = (float)(bulletSpeed / totalDistance) * totalDistanceVector.X;
                direction.Y = (float)(bulletSpeed / totalDistance) * totalDistanceVector.Y;

                bulletTimer = fireRate;
                return new Bullet(new Vector2(position.X + (sideSize / 2), position.Y + (sideSize / 2)), direction, range, bulletSize, damage, Color.White);
            }
            else { 
                bulletTimer -= 1;
                return null;
            }
        }

        public void Reset(int bufferSize, int mapSideSize) {
            position = new Vector2((((mapSideSize + (2 * bufferSize)) * 11) / 2), (((mapSideSize + (2 * bufferSize)) * 11) / 2));
            health += maxHealth / 4;
            if(health > maxHealth) {
                health = maxHealth;
            }
            bulletTimer = 50;
        }

        public void takeDamage(Bullet bullet) {
            damageJustTaken = true;
            health -= bullet.Damage;
        }
    }
}
