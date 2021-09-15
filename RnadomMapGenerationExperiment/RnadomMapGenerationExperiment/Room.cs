using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RnadomMapGenerationExperiment {
    class Room {
        private Vector2 index;
        private int sideSize;
        private int sideWidth = 10;
        private int fullSideSize;
        private int bufferSize;
        private int doorSize;
        private bool[] doors;
        private int createdFrom;
        private int depth;
        private bool isCurrentRoom = false;
        private bool completed = false;

        private List<Pickup> pickups;

        public Room(Vector2 _index, int _sideSize, int _bufferSize, int _doorSize, bool north, bool east, bool south, bool west, int _createdFrom, int _depth) {
            index = _index;
            sideSize = _sideSize;
            bufferSize = _bufferSize;
            doorSize = _doorSize;
            fullSideSize = sideSize + (2 * bufferSize);
            createdFrom = _createdFrom;
            depth = _depth;
            doors = new bool[] {north, east, south, west};
            pickups = new List<Pickup>();
        }

        public Vector2 Index {
            get {
                return index;
            }
            set {
                index = value;
            }
        }

        public bool Completed {
            get {
                return completed;
            }
            set {
                completed = value;
            }
        }

        public bool[] Doors {
            get {
                return doors;
            }
            set {
                doors = value;
            }
        }

        public List<Pickup> Pickups {
            get {
                return pickups;
            }
        }

        public int Depth {
            get {
                return depth;
            }
        }

        public bool IsCurrentRoom {
            set {
                isCurrentRoom = value;
            }
        }

        public void Draw(SpriteBatch sb, SpriteFont font, int scalingFactor, Vector2 offset, MapMode mapState) {
            //sb.DrawString(font, "" + depth, new Vector2((index.X * fullSideSize) + bufferSize + 20, (index.Y * fullSideSize) + bufferSize + 20), Color.White);;

            Color drawColor = Color.White;

            if (isCurrentRoom && mapState == MapMode.Map) {
                drawColor = Color.Red;
            }
            //north
            /*if(createdFrom == 0) {
                drawColor = Color.Red;
            }*/
            int scaledFullSideSize = scalingFactor * fullSideSize;
            int scaledSideSize = scalingFactor * sideSize;
            int scaledBufferSize = scalingFactor * bufferSize;
            int scaledDoorSize = scalingFactor * doorSize;
            int scaledSideWidth = scalingFactor * sideWidth;
            int scaledX = (scalingFactor * (int)(index.X + 5));
            int scaledY = (scalingFactor * (int)(index.Y + 5));

            if(mapState == MapMode.Room) {
                scaledX = scalingFactor * (int)(index.X + offset.X);
                scaledY = scalingFactor * (int)(index.Y + offset.Y);
            }

            if (doors[0]) {
                ShapeBatch.Box((scaledX * scaledFullSideSize) + scaledBufferSize, 
                    (scaledY * scaledFullSideSize) + scaledBufferSize, 
                    (scaledSideSize / 2) - (scaledDoorSize / 2), 
                    scaledSideWidth, drawColor);
                ShapeBatch.Box(((scaledX * scaledFullSideSize) + scaledBufferSize + (scaledSideSize / 2) + (scaledDoorSize / 2)), 
                    (scaledY * scaledFullSideSize) + scaledBufferSize, 
                    (scaledSideSize / 2) - (scaledDoorSize / 2), 
                    scaledSideWidth, drawColor);
            }
            else {
                ShapeBatch.Box((scaledX * scaledFullSideSize) + scaledBufferSize, 
                    (scaledY * scaledFullSideSize) + scaledBufferSize, 
                    scaledSideSize, 
                    scaledSideWidth, drawColor);
            }

            /*if(createdFrom == 0) {
                drawColor = Color.White;
            }*/

            //east
            /*if(createdFrom == 1) {
                drawColor = Color.Red;
            }*/

            if (doors[1]) {
                ShapeBatch.Box((scaledX * scaledFullSideSize) + scaledBufferSize + (scaledSideSize - scaledSideWidth), 
                    (scaledY * scaledFullSideSize) + scaledBufferSize, 
                    scaledSideWidth, 
                    (scaledSideSize / 2) - (scaledDoorSize / 2), drawColor);
                ShapeBatch.Box((scaledX * scaledFullSideSize) + scaledBufferSize + (scaledSideSize - scaledSideWidth), 
                    (scaledY * scaledFullSideSize) + scaledBufferSize + (scaledSideSize / 2) + (scaledDoorSize / 2), 
                    scaledSideWidth, 
                    (scaledSideSize / 2) - (scaledDoorSize / 2), drawColor);
            }
            else {
                ShapeBatch.Box((scaledX * scaledFullSideSize) + scaledBufferSize + (scaledSideSize - scaledSideWidth), 
                    (scaledY * scaledFullSideSize) + scaledBufferSize, 
                    scaledSideWidth, 
                    scaledSideSize, drawColor);
            }

            /*if(createdFrom == 1) {
                drawColor = Color.White;
            }*/

            //south
            /*if(createdFrom == 2) {
                drawColor = Color.Red;
            }*/

            if (doors[2]) {
                ShapeBatch.Box((scaledX * scaledFullSideSize) + scaledBufferSize, 
                    (scaledY * scaledFullSideSize) + scaledBufferSize + (scaledSideSize - scaledSideWidth), 
                    (scaledSideSize / 2) - (scaledDoorSize / 2), 
                    scaledSideWidth , drawColor);
                ShapeBatch.Box((scaledX * scaledFullSideSize) + scaledBufferSize + (scaledSideSize / 2) + (scaledDoorSize / 2), 
                    (scaledY * scaledFullSideSize) + scaledBufferSize + (scaledSideSize - scaledSideWidth), 
                    (scaledSideSize / 2) - (scaledDoorSize / 2), 
                    scaledSideWidth , drawColor);
            }
            else {
                ShapeBatch.Box((scaledX * scaledFullSideSize) + scaledBufferSize, 
                    (scaledY * scaledFullSideSize) + scaledBufferSize + (scaledSideSize - scaledSideWidth), 
                    scaledSideSize, 
                    scaledSideWidth, drawColor);
            }

            /*if(createdFrom == 2) {
                drawColor = Color.White;
            }*/

            //west
            /*if(createdFrom == 3) {
                drawColor = Color.Red;
            }*/

            if (doors[3]) {
                ShapeBatch.Box((scaledX * scaledFullSideSize) + scaledBufferSize, 
                    (scaledY * scaledFullSideSize) + scaledBufferSize, 
                    scaledSideWidth, 
                    (scaledSideSize / 2) - (scaledDoorSize / 2), drawColor);
                ShapeBatch.Box((scaledX * scaledFullSideSize) + scaledBufferSize, 
                    (scaledY * scaledFullSideSize) + scaledBufferSize + (scaledSideSize / 2) + (scaledDoorSize / 2), 
                    scaledSideWidth, 
                    (scaledSideSize / 2) - (scaledDoorSize / 2), drawColor);
            }
            else {
                ShapeBatch.Box((scaledX * scaledFullSideSize) + scaledBufferSize, 
                    (scaledY * scaledFullSideSize) + scaledBufferSize, 
                    scaledSideWidth, 
                    scaledSideSize, drawColor);
            }

            /*if(createdFrom == 3) {
                drawColor = Color.White;
            }*/

            if (completed && mapState == MapMode.Room && isCurrentRoom) {
                for(int i = 0; i < pickups.Count; i++) {
                    pickups[i].Draw();
                }
            }
        }

        public void AddPickup(Pickup pickupToAdd) {
            pickups.Add(pickupToAdd);
        }
    }
}
