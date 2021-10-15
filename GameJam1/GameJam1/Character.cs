using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam1 {
    class Character {
        Vector2 pos;
        int radiusSize;

        public Character(Vector2 _pos, int _radiusSize) {
            pos = _pos;
            radiusSize = _radiusSize;
        }

        public Vector2 Pos {
            get {
                return pos;
            }
            set {
                pos = value;
            }
        }

        public void Draw(Vector2 drawPos) {
            ShapeBatch.Circle(drawPos, radiusSize, Color.White);
        }
    }
}
