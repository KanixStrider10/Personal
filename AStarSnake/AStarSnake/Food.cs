using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace AStarSnake {
    class Food {
        private Vector2 center;
        private int size;
        private Random rng;
        private Texture2D texture;

        public Food(int s, Random _rng, Texture2D t) {
            size = s;
            texture = t;
            rng = _rng;
            CreatePos();
        }

        public Vector2 Cord {
            get {
                return center;
            }
        }

        public void CreatePos() {
            center = new Vector2((int)rng.Next(0, 33), (int)rng.Next(0, 33));
        }

        public void Draw(SpriteBatch sb) {
            sb.Draw(texture, new Rectangle((int)center.X * size, (int)center.Y * size, size, size), new Color(0, 255, 0));
        }
    }
}
