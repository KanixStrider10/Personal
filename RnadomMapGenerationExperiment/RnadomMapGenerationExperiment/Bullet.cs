using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace RnadomMapGenerationExperiment {
    class Bullet {
        private Vector2 position;
        private Vector2 direction;
        private int range;
        private int size;
        private double distanceTravelled;
        private int damage;
        private Color color;

        public Bullet(Vector2 _position, Vector2 _direction, int _range, int _size, int _damage, Color _color) {
            direction = _direction;
            position = _position;
            range = _range;
            size = _size;
            distanceTravelled = 0;
            damage = _damage;
            color = _color;
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

        public int Damage {
            get {
                return damage;
            }
        }

        public bool Move() {
            position += direction / 2;
            distanceTravelled += Math.Sqrt(Math.Pow(direction.X / 2, 2) + Math.Pow(direction.Y / 2, 2));

            return distanceTravelled > range;
        }

        public void Draw() {
            ShapeBatch.Circle(position, size, color);
        }
    }
}
