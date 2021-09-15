using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AStarSnake {
    class Snake {
        private Vector2 center;
        private int size;
        private int segSize;
        private Vector2 dxdy;
        private List<Vector2> cords;
        private int score = 0;
        private bool dead = false;
        private Queue<Vector2> path;
        private Texture2D texture;
        private double currentPercentage;
        private Graph currentTargetMap;
        private bool newPercentage = false;
        private bool drawMap = true;

        public Snake(Vector2 c, Vector2 _dxdy, int s, int ss, Texture2D t) {
            center = c;
            dxdy = _dxdy;
            size = s;
            segSize = ss;
            cords = new List<Vector2>();
            path = new Queue<Vector2>();
            texture = t;

            for(int i = 0; i < size; i++) {
                cords.Add(new Vector2(center.X, center.Y + i));
            }
        }

        public Snake(int s, int ss, Texture2D t) {
            size = s;
            segSize = ss;
            texture = t;

            center = new Vector2(16, 16);
            dxdy = new Vector2(0, -1);
            cords = new List<Vector2>();
            path = new Queue<Vector2>();

            for(int i = 0; i < size; i++) {
                cords.Add(new Vector2(center.X, center.Y + i));
            }
        }

        public int Score {
            get {
                return score;
            }
        }

        public double Percentage {
            get {
                return currentPercentage;
            }
        }

        public bool NewPercentage {
            get {
                return newPercentage;
            }
            set {
                newPercentage = value;
            }
        }

        public List<Vector2> Cords {
            get {
                return cords;
            }
        }

        public bool Dead {
            get {
                return dead;
            }
        }

        public bool DrawMap {
            get {
                return drawMap;
            }
            set {
                drawMap = value;
            }
        }

        public void Draw(SpriteBatch sb, SpriteFont font) {
            for(int i = 0; i < size; i++) {
                sb.Draw(texture, new Rectangle((int)cords[i].X * segSize, (int)cords[i].Y * segSize, segSize, segSize), 
                    new Color((int)(255 - (i * (255.0/(size - 1)))), 0, (int)(i * (255.0/(size - 1)))));

                /*if(i == size - 1) {
                    sb.DrawString(font, $"max blue {(i * (255.0/(size - 1)))}", new Vector2 (30 * segSize, segSize), Color.White);
                }*/
            }
        }

        public void MoveAStar(SpriteBatch sb, Food food, SpriteFont font, int frameCounter) {
            if(frameCounter % 1 == 0) {
                FindPath(food, sb);

                Vector2 head = cords[0];
                cords.Insert(0, new Vector2(head.X + dxdy.X, head.Y + dxdy.Y));
                if(cords.Count > size) {
                    cords.RemoveAt(size);
                }

                Eat(food);
            }

            if(currentTargetMap != null && drawMap) {
                currentTargetMap.Draw(sb);
            }
            Draw(sb, font);

            CheckDeath();
            if (dead) {
                Die();
            }
        }

        public void MoveManual(SpriteBatch sb, Vector2 direction, int frameCounter, Food f, SpriteFont font) {
            if(frameCounter % 4 == 0) {
                Vector2 head = cords[0];
                dxdy = direction;
                cords.Insert(0, new Vector2(head.X + dxdy.X, head.Y + dxdy.Y));
                if(cords.Count > size) {
                    cords.RemoveAt(size);
                }

                Eat(f);
            }

            Draw(sb, font);

            CheckDeath();
            if (dead) {
                Die();
            }
        }

        public void Grow() {
            size += 1;
        }

        public void Die() {
            Debug.WriteLine(score);
        }

        public void CheckDeath() {
            if(cords[0].X < 0 || cords[0].X > 32 || cords[0].Y < 0 || cords[0].Y > 32) {
                dead = true;
            }

            for(int i = 1; i < size; i++) {
                if(cords[0].X == cords[i].X && cords[0].Y == cords[i].Y) {
                    dead = true;
                    i = size;
                }
            }
        }

        public void FindPath(Food food, SpriteBatch sb) {
            if(path.Count == 0) {
                Vector2 currentDxDy = dxdy;
                List<Vector2> tempCords = new List<Vector2>();
                for(int i = 0; i < cords.Count; i++) {
                    tempCords.Add(cords[i]);
                }
                
                AStarTree tree = new AStarTree(tempCords, currentDxDy, food, size, segSize, texture);
                path = tree.GetPath();
                currentPercentage = tree.CurrentPercentage;
                newPercentage = true;
                currentTargetMap = tree.Map;
            }

            dxdy = path.Dequeue();
        }

        public void Eat(Food food) {
            if(cords[0].X == food.Cord.X && cords[0].Y == food.Cord.Y) {
                Grow();
                score += 1;
                food.CreatePos();
                cords.Add(new Vector2(-1, -1));
            }
        }
    }
}
