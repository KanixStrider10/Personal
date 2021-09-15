using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace AStarSnake {
    class Graph {
        private List<Vector2> snakeCords;
        private int count;
        private Vector2 root;
        private int[,] mapArray;
        private Texture2D texture;
        private int segSize;

        public Graph(List<Vector2> sc, Texture2D t, int ss) {
            snakeCords = sc;
            count = 0;
            root = snakeCords[0];
            texture = t;
            segSize = ss;
            CreateGraph();
        }

        public Graph(List<Vector2> sc) {
            snakeCords = sc;
            count = 0;
            root = snakeCords[0];
            CreateGraph();
        }

        public int Count {
            get {
                return count;
            }
        }

        public void CreateGraph() {
            mapArray = new int[33, 33];

            for(int i = 0; i < snakeCords.Count; i++) {
                Vector2 cord = snakeCords[i];
                if(cord.X != -1) {
                    mapArray[(int)cord.X, (int)cord.Y] = -1;
                }
            }

            Queue<Vector2> toExpand = new Queue<Vector2>();
            toExpand.Enqueue(root);

            while(toExpand.Count > 0) {
                Vector2 current = toExpand.Dequeue();

                if(current.X != 0) {
                    if(mapArray[(int)current.X - 1, (int)current.Y] != 1 && mapArray[(int)current.X - 1, (int)current.Y] != -1) {
                        mapArray[(int)current.X - 1, (int)current.Y] = 1;
                        toExpand.Enqueue(new Vector2(current.X - 1, current.Y));
                        count ++;
                    }
                }

                if(current.X != 32) {
                    if(mapArray[(int)current.X + 1, (int)current.Y] != 1 && mapArray[(int)current.X + 1, (int)current.Y] != -1) {
                        mapArray[(int)current.X + 1, (int)current.Y] = 1;
                        toExpand.Enqueue(new Vector2(current.X + 1, current.Y));
                        count ++;
                    }
                }

                if(current.Y != 0) {
                    if(mapArray[(int)current.X, (int)current.Y - 1] != 1 && mapArray[(int)current.X, (int)current.Y - 1] != -1) {
                        mapArray[(int)current.X, (int)current.Y - 1] = 1;
                        toExpand.Enqueue(new Vector2(current.X, current.Y - 1));
                        count ++;
                    }
                }

                if(current.Y != 32) {
                    if(mapArray[(int)current.X, (int)current.Y + 1] != 1 && mapArray[(int)current.X, (int)current.Y + 1] != -1) {
                        mapArray[(int)current.X, (int)current.Y + 1] = 1;
                        toExpand.Enqueue(new Vector2(current.X, current.Y + 1));
                        count ++;
                    }
                }
            }

            /*//Start the queue
            start.Visited = true;
            Console.WriteLine(start);
            Queue<Vertex> queue = new Queue<Vertex>();
            queue.Enqueue(start);

            //Search the graph
            while(queue.Count > 0) {
                Vertex n = GetAdjacentUnvisited(queue.Peek().Name);
                if(n == null) {
                    queue.Dequeue();
                }
                else {
                    Console.WriteLine(n);
                    n.Visited = true;
                    queue.Enqueue(n);
                }
            }*/
        }

        public void Draw(SpriteBatch sb) {
            for(int i = 0; i < 33; i++) {
                for(int j = 0; j < 33; j++) {
                    if(mapArray[i, j] == 0) {
                        sb.Draw(texture, new Rectangle(i * segSize, j * segSize, segSize, segSize), Color.OrangeRed);
                    }
                }
            }
        }
    }
}
