using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam1 {
    class Goal : Tile {
        bool visible = false;
        bool canStandOn = true;
        int radiusSize = 35;

        public override bool Visible {
            get {
                return visible;
            }
            set {
                visible = value;
            }
        }

        public override bool CanStandOn {
            get {
                return canStandOn;
            }
        }

        public override void Draw(Rectangle rect) {
            if (visible) {
                ShapeBatch.BoxOutline(rect, Color.White);
                ShapeBatch.Circle(new Vector2(rect.X + (rect.Width / 2), rect.Y + (rect.Height / 2)), radiusSize, Color.Red);
            }
        }
    }
}
