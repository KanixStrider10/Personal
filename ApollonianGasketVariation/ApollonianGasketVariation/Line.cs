using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ApollonianGasketVariation {
    class Line {
        Vector2 point1;
        Vector2 point2;
        float slope;

        public Line(Vector2 p1, Vector2 p2) {
            point1 = p1;
            point2 = p2;
            slope = (point2.Y - point1.Y)/(point2.X - point1.X);
        }

        public Line(Vector2 p1, float m) {
            point1 = p1;
            slope = m;
            point2 = new Vector2(point1.X + 1, point1.Y + m);
        }

        public Vector2 Point1 {
            get {
                return point1;
            }
            set {
                point1 = value;
                slope = (point2.Y - point1.Y)/(point2.X - point1.X);
            }
        }

        public Vector2 Point2 {
            get {
                return point2;
            }
            set {
                point2 = value;
                slope = (point2.Y - point1.Y)/(point2.X - point1.X);
            }
        }

        public float Slope {
            get {
                return slope;
            }
        }

        public bool ContainsPoint(Vector2 input) {
            float slopeToInput;
            if(input.X >= point1.X) {
                slopeToInput = (input.Y - point1.Y)/(input.X - point1.X);
            }
            else {
                slopeToInput = (point1.Y - input.Y)/(point1.X - input.X);
            }
            
            return slopeToInput == slope;
        }

        public Vector2 getYIntercept() {
            Vector2 output = new Vector2(0, 0);

            output.Y = point1.Y - slope * point1.X;

            return output;
        }

        public void Draw(Color color) {
            Vector2 farthestLeft = new Vector2(0, point1.Y - slope * (point1.X - 0));
            Vector2 farthestRight = new Vector2(800, point1.Y + slope * (800 - point1.X));
            ShapeBatch.Line(farthestLeft, farthestRight, color);
        }
    }
}
