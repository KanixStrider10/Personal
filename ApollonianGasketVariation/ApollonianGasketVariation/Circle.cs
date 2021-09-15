using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ApollonianGasketVariation {
    public class Circle {
        Vector2 center;
        float radius;

        public Circle(Vector2 c, float r) {
            center = c;
            radius = r;
        }

        public Vector2 Center {
            get {
                return center;
            }
            set {
                center = value;
            }
        }

        public float CenterX {
            get {
                return center.X;
            }
            set {
                center.X = value;
            }
        }

        public float CenterY {
            get {
                return center.Y;
            }
            set {
                center.Y = value;
            }
        }

        public float Radius {
            get {
                return radius;
            }
            set {
                radius = value;
            }
        }

        public void Draw(Color color) {
            ShapeBatch.CircleOutline(center, radius, color);
        }
    }
}
