using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace AStarSnake {
    class AStarTree {
        private AStarNode root;
        private Food f;
        private Vector2 currentDxDy;
        private int snakeSize;
        private int segSize;
        private double currentPercentage;
        private Texture2D texture;
        private SpriteBatch sb;
        private Graph map;

        public AStarTree(List<Vector2> startPos, Vector2 _dxdy, Food _f, int _snakeSize, int _segSize, Texture2D t) {
            f = _f;
            currentDxDy = _dxdy;
            snakeSize = _snakeSize;
            segSize = _segSize;
            texture = t;
            List<Vector2> lastMove = new List<Vector2>();
            lastMove.Add(currentDxDy);
            root = new AStarNode(startPos, f, lastMove, startPos[0], snakeSize, segSize, 0);
        }

        public double CurrentPercentage {
            get {
                return currentPercentage;
            }
        }

        public Graph Map {
            get {
                return map;
            }
        }

        public Queue<Vector2> GetPath() {
            List<AStarNode> visited = new List<AStarNode>();
            PriorityQueue toExpand = new PriorityQueue();
            toExpand.Enqueue(root);
            Queue<Vector2> path = new Queue<Vector2>();
            bool goalFound = false;
            List<AStarNode> children;

            while(!goalFound) {
                if(toExpand.Count == 0) {
                    if(visited.Count == 1) {
                        path.Enqueue(currentDxDy);
                        return path;
                    }
                    int deepestDepth = 0;
                    AStarNode deepestNode = null;
                    for(int i = 0; i < visited.Count; i++) {
                        if(visited[i].Depth > deepestDepth) {
                            deepestDepth = visited[i].Depth;
                            deepestNode = visited[i];
                        }
                    }
                    path.Enqueue(deepestNode.PathToNode[1]);
                    return path;
                }

                AStarNode nextToExpand = toExpand.Dequeue();
                visited.Add(nextToExpand);
                children = nextToExpand.CreateChildren();

                for(int i = 0; i < visited.Count; i++) {
                    for(int j = 0; j < children.Count; j++) {
                        if(children[j].Cord == visited[i].Cord) {
                            children.RemoveAt(j);
                            j--;
                        }
                    }
                }

                for(int i = 0; i < children.Count; i++) {
                    if(children[i].Cord.X == f.Cord.X && children[i].Cord.Y == f.Cord.Y) {
                        currentPercentage = CheckPercentage(children[i].CurrentSnakePos);
                        List<Vector2> pathTemp = children[i].PathToNode;
                        for(int j = 1; j < pathTemp.Count; j++) {
                            path.Enqueue(pathTemp[j]);
                        }
                        goalFound = true;
                    }
                    else {
                        toExpand.Enqueue(children[i]);
                    }
                }
            }

            return path;
        }

        public double CheckPercentage(List<Vector2> SnakeCords) {
            map = new Graph(SnakeCords, texture, segSize);
            return map.Count / (1089.0 - SnakeCords.Count);
        }
    }
}
