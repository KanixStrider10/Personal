using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam1 {
    class LevelSelectArea {
        Rectangle rect;
        int levelNumber;
        bool levelCompleted = false;

        public LevelSelectArea(Rectangle _rect, int _levelNumber) {
            rect = _rect;
            levelNumber = _levelNumber;
        }

        public int LevelNumber {
            get {
                return levelNumber;
            }
        }

        public bool LevelCompleted {
            get {
                return levelCompleted;
            }
            set {
                levelCompleted = value;
            }
        }

        public Rectangle Rect {
            get {
                return rect;
            }
        }

        public void Draw(SpriteFont font, SpriteBatch sb, bool showLevel) {
            if (showLevel) {
                ShapeBatch.BoxOutline(rect, Color.White);
            }
            else {
                ShapeBatch.Box(rect, Color.White);
            }
            sb.DrawString(font, "" + levelNumber, new Vector2(rect.X, rect.Y), Color.White);
        }
    }
}
