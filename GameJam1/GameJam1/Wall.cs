using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam1 {
    class Wall : Tile{
        bool visible = false;
        bool canStandOn = false;

        public override bool CanStandOn {
            get {
                return canStandOn;
            }
        }

        public override bool Visible {
            get {
                return visible;
            }
            set {
                visible = value;
            }
        }

        public override void Draw(Rectangle rect) {
            if (visible) {
                ShapeBatch.Box(rect, Color.White);
            }
        }
    }
}
