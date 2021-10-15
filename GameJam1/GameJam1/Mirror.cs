using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam1 {
    class Mirror : Tile{
        bool visible = false;
        bool canStandOn = false;
        int direction;
        bool reflectedNorth = false;
        bool reflectedSouth = false;
        bool reflectedEast = false;
        bool reflectedWest = false;

        public Mirror(int _direction) {
            direction = _direction;
        }

        public override bool Visible {
            get {
                return visible;
            }
            set {
                visible = value;
            }
        }

        public override int Direction {
            get {
                return direction;
            }
            set {
                direction = value;
            }
        }

        public override bool CanStandOn {
            get {
                return canStandOn;
            }
        }

        public override bool AlreadyReflected(Vector2 fromSide) {
            bool returnValue = false;;
            if(fromSide.X == 0) {
                if(fromSide.Y == -1) {
                    returnValue = reflectedNorth;
                    reflectedNorth = true;
                }
                else {
                    returnValue = reflectedSouth;
                    reflectedSouth = true;
                }
            }
            else {
                if(fromSide.X == -1) {
                    returnValue = reflectedWest;
                    reflectedWest = true;
                }
                else {
                    returnValue = reflectedEast;
                    reflectedEast = true;
                }
            }
            return returnValue;
        }

        public override void ResetReflected() {
            reflectedNorth = false;
            reflectedSouth = false;
            reflectedEast = false;
            reflectedWest = false;
        }

        public override void Draw(Rectangle rect) {
            if (visible) {
                ShapeBatch.BoxOutline(rect, Color.White);
                if(direction == 0) {
                    ShapeBatch.Line(new Vector2(rect.X, rect.Y), new Vector2(rect.X + rect.Width, rect.Y + rect.Height), Color.White);
                }
                else if(direction == 1) {
                    ShapeBatch.Line(new Vector2(rect.X + rect.Width, rect.Y), new Vector2(rect.X, rect.Y + rect.Height), Color.White);
                }
            }
        }
    }
}
