using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam1 {
    class Tile {
        bool visible = false;
        bool canStandOn = true;

        public virtual bool Visible {
            get {
                return visible;
            }
            set {
                visible = value;
            }
        }

        public virtual int Direction{
            get;
            set;
        }

        public virtual bool AlreadyReflected(Vector2 fromSide) {
            return false;
        }

        public virtual void ResetReflected() {
        }

        public virtual bool CanStandOn {
            get {
                return canStandOn;
            }
        }

        public virtual void Draw(Rectangle rect) {
            if (visible) {
                ShapeBatch.BoxOutline(rect, Color.White);
            }
        }
    }
}
