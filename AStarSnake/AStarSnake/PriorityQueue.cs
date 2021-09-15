using System;
using System.Collections.Generic;
using System.Text;

namespace AStarSnake {
    class PriorityQueue {

        private List<AStarNode> queue;

        public PriorityQueue() {
            queue = new List<AStarNode>();
        }

        public AStarNode this[int index] {
            get {
                return queue[index];
            }
        }

        public int Count {
            get {
                return queue.Count;
            }
        }

        public void Enqueue(AStarNode toAdd) {
            bool alreadyInQueue = false;
            for(int i = 0; i < queue.Count; i++) {
                if(toAdd.Cord.X == queue[i].Cord.X && toAdd.Cord.Y == queue[i].Cord.Y) {
                    alreadyInQueue = true;
                    break;
                }
            }

            if (!alreadyInQueue) {
                for(int i = 0; i < queue.Count; i++) {
                    if(toAdd.FCost < queue[i].FCost) {
                        queue.Insert(i, toAdd);
                        alreadyInQueue = true;
                        break;
                    }
                    else if(toAdd.FCost == queue[i].FCost && toAdd.HCost < queue[i].HCost) {
                        queue.Insert(i, toAdd);
                        alreadyInQueue = true;
                        break;
                    }
                }

                if (!alreadyInQueue) {
                    queue.Add(toAdd);
                }
            }
        }

        public AStarNode Dequeue() {
            if(queue.Count > 0) {
                AStarNode output = queue[0];
                queue.RemoveAt(0);
                return output;
            }
            return null;
        }
    }
}
