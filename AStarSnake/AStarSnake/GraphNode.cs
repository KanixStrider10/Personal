using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AStarSnake {
    class GraphNode {
        private Vector2 cord;
        private List<GraphNode> children;
        private bool visited = false;

        public GraphNode(Vector2 c) {
            cord = c;
            children = new List<GraphNode>();
        }

        public Vector2 Cord {
            get {
                return cord;
            }
        }

        public List<GraphNode> Children {
            get {
                if(children.Count == 0) {
                    if(cord.X != 0) {
                        children.Add(new GraphNode(new Vector2(cord.X - 1, cord.Y)));
                    }
                    if(cord.X != 32) {
                        children.Add(new GraphNode(new Vector2(cord.X + 1, cord.Y)));
                    }
                    if(cord.Y != 0) {
                        children.Add(new GraphNode(new Vector2(cord.X, cord.Y - 1)));
                    }
                    if(cord.Y != 32) {
                        children.Add(new GraphNode(new Vector2(cord.X, cord.Y + 1)));
                    }
                }
                return children;
            }
        }

        public bool Visited {
            get {
                return visited;
            }
            set {
                visited = value;
            }
        }
    }
}
