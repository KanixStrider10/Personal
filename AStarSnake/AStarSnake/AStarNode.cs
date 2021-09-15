using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AStarSnake {
    class AStarNode {
        private List<Vector2> currentSnakePos;
        private List<Vector2> pathToNode;
        private List<AStarNode> children;
        private Vector2 currentSnakeHead;
        private Food food;
        private int gCost;
        private int hCost;
        private int fCost;
        private Vector2 cord;
        private int snakeSize;
        private int segSize;
        private int depth;
        private Graph map;
        private double currentPercentage;
        private double percentageThreshold = 0;

        public AStarNode(List<Vector2> cp, Food f, List<Vector2> ptn, Vector2 _cord, int ss, int _segSize, int d) {
            currentSnakePos = cp;
            currentSnakeHead = currentSnakePos[0];
            pathToNode = ptn;
            food = f;
            cord = _cord;
            snakeSize = ss;
            segSize = _segSize;
            depth = d;
            children = new List<AStarNode>();

            //map = new Graph(currentSnakePos);
            //currentPercentage = map.Count / (1089.0 - currentSnakePos.Count);
            currentPercentage = 1;

            F();
        }

        public List<Vector2> CurrentSnakePos {
            get {
                return currentSnakePos;
            }
        }

        public int FCost {
            get {
                return fCost;
            }
        }

        public int HCost {
            get {
                return hCost;
            }
        }

        public int GCost {
            get {
                return gCost;
            }
        }

        public Vector2 Cord {
            get {
                return cord;
            }
        }

        public List<Vector2> PathToNode {
            get {
                return pathToNode;
            }
        }

        public int Depth {
            get {
                return depth;
            }
        }

        public double CurrentPrecentage {
            get {
                return currentPercentage;
            }
        }

        public void F() {
            fCost = G() + H();
        }

        public int H(){
            double squareCost = Math.Pow(currentSnakeHead.X - food.Cord.X, 2) + Math.Pow(currentSnakeHead.Y - food.Cord.Y, 2);
            hCost = (int)Math.Sqrt(squareCost);
            return hCost;
        }

        public int G() {
            gCost = pathToNode.Count * segSize;
            return gCost;
        }

        public Vector2 LastMove() {
            return pathToNode[pathToNode.Count - 1];
        }

        public List<AStarNode> CreateChildren() {
            if(children.Count != 0) {
                return children;
            }

            Vector2 lastMove = LastMove();
            List<Vector2> tempCords;
            List<Vector2> tempMoves;

            if(lastMove.X != 1) {
                tempCords = new List<Vector2>();
                tempMoves = new List<Vector2>();
                for(int i = 0; i < currentSnakePos.Count; i++) {
                    tempCords.Add(currentSnakePos[i]);
                }
                for(int i = 0; i < pathToNode.Count; i++) {
                    tempMoves.Add(pathToNode[i]);
                }

                tempCords.Insert(0, new Vector2(currentSnakeHead.X - 1, currentSnakeHead.Y));
                if(tempCords.Count > snakeSize) {
                    tempCords.RemoveAt(snakeSize);
                }

                if (WillNotDie(tempCords)) {
                    tempMoves.Add(new Vector2(-1, 0));
                    AStarNode tempNode = new AStarNode(tempCords, food, tempMoves, tempCords[0], snakeSize, segSize, depth + 1);
                    if(tempNode.CurrentPrecentage >= percentageThreshold) {
                        children.Add(tempNode);
                    }
                }
            }

            if(lastMove.X != -1) {
                tempCords = new List<Vector2>();
                tempMoves = new List<Vector2>();
                for(int i = 0; i < currentSnakePos.Count; i++) {
                    tempCords.Add(currentSnakePos[i]);
                }
                for(int i = 0; i < pathToNode.Count; i++) {
                    tempMoves.Add(pathToNode[i]);
                }

                tempCords.Insert(0, new Vector2(currentSnakeHead.X + 1, currentSnakeHead.Y));
                if(tempCords.Count > snakeSize) {
                    tempCords.RemoveAt(snakeSize);
                }


                if (WillNotDie(tempCords)) {
                    tempMoves.Add(new Vector2(1, 0));
                    AStarNode tempNode = new AStarNode(tempCords, food, tempMoves, tempCords[0], snakeSize, segSize, depth + 1);
                    if(tempNode.CurrentPrecentage >= percentageThreshold) {
                        children.Add(tempNode);
                    }
                }
            }

            if(lastMove.Y != 1) {
                tempCords = new List<Vector2>();
                tempMoves = new List<Vector2>();
                for(int i = 0; i < currentSnakePos.Count; i++) {
                    tempCords.Add(currentSnakePos[i]);
                }
                for(int i = 0; i < pathToNode.Count; i++) {
                    tempMoves.Add(pathToNode[i]);
                }

                tempCords.Insert(0, new Vector2(currentSnakeHead.X, currentSnakeHead.Y - 1));
                if(tempCords.Count > snakeSize) {
                    tempCords.RemoveAt(snakeSize);
                }


                if (WillNotDie(tempCords)) {
                    tempMoves.Add(new Vector2(0, -1));
                    AStarNode tempNode = new AStarNode(tempCords, food, tempMoves, tempCords[0], snakeSize, segSize, depth + 1);
                    if(tempNode.CurrentPrecentage >= percentageThreshold) {
                        children.Add(tempNode);
                    }
                }
            }

            if(lastMove.Y != -1) {
                tempCords = new List<Vector2>();
                tempMoves = new List<Vector2>();
                for(int i = 0; i < currentSnakePos.Count; i++) {
                    tempCords.Add(currentSnakePos[i]);
                }
                for(int i = 0; i < pathToNode.Count; i++) {
                    tempMoves.Add(pathToNode[i]);
                }

                tempCords.Insert(0, new Vector2(currentSnakeHead.X, currentSnakeHead.Y + 1));
                if(tempCords.Count > snakeSize) {
                    tempCords.RemoveAt(snakeSize);
                }


                if (WillNotDie(tempCords)) {
                    tempMoves.Add(new Vector2(0, 1));
                    AStarNode tempNode = new AStarNode(tempCords, food, tempMoves, tempCords[0], snakeSize, segSize, depth + 1);
                    if(tempNode.CurrentPrecentage >= percentageThreshold) {
                        children.Add(tempNode);
                    }
                }
            }

            return children;
        }

        public bool WillNotDie(List<Vector2> cords) {
            if(cords[0].X < 0 || cords[0].X > 32 || cords[0].Y < 0 || cords[0].Y > 32) {
                return false;
            }

            for(int i = 1; i < snakeSize; i++) {
                if(cords[0].X == cords[i].X && cords[0].Y == cords[i].Y) {
                    return false;
                }
            }

            return true;
        }
    }
}
