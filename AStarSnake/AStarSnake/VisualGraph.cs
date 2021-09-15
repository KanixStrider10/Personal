using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AStarSnake {
    class VisualGraph {
        List<double> points;
        double largestPoint;
        Vector2 pos;
        int width;
        int height;
        Color color;

        public VisualGraph(int x, int y, int w, int h) {
            pos = new Vector2(x, y);
            width = w;
            height = h;
            points = new List<double>();
            color = Color.Red;
        }

        public VisualGraph(int x, int y, int w, int h, Color c) {
            pos = new Vector2(x, y);
            width = w;
            height = h;
            points = new List<double>();
            color = c;
        }

        public void Add(double data) {
            points.Add(data);
            if(data > largestPoint) {
                largestPoint = data;
            }
            if(points.Count > 50) {
                points.RemoveAt(0);
            }
        }

        public void Reset() {
            points = new List<double>();
        }

        public void Draw() {
            ShapeBatch.Line(pos, new Vector2(pos.X + width, pos.Y), Color.White);
            ShapeBatch.Line(pos, new Vector2(pos.X, pos.Y - height), Color.White);

            double yScaler = height/largestPoint;

            if(points.Count == 1) {
                ShapeBatch.Circle(new Vector2(pos.X, (float)(pos.Y - (points[0] * yScaler))), 3, color);
            }
            else if(points.Count > 1) {
                double xStepSize = width / points.Count;

                Vector2 point1;
                Vector2 point2;
                point1 = new Vector2(pos.X, (float)(pos.Y - (points[0] * yScaler)));
                point2 = point1;

                for(int i = 1; i < points.Count; i++) {
                    point2 = new Vector2((float)(pos.X + (xStepSize * i)), (float)(pos.Y - (points[i] * yScaler)));
                    ShapeBatch.Line(point1, point2, Color.White);
                    ShapeBatch.Circle(point1, 3, color);
                    point1 = point2;
                }

                ShapeBatch.Circle(point2, 3, color);
            }
        }
    }
}
