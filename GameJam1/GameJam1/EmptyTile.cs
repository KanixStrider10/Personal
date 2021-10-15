using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam1 {
    class EmptyTile : Tile{
        bool visible = false;
        bool canStandOn = false;

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

        public override void Draw(Rectangle rect) { }
    }
}
