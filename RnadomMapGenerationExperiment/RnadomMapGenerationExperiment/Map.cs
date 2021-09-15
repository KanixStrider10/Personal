using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace RnadomMapGenerationExperiment {
    class Map {
        private List<Room> rooms;
        private int sideSize;
        private int bufferSize;
        private int doorSize;
        private double wallChance;
        private Random rng;

        private List<Bullet> bulletsOnBoard;
        private List<Enemy> enemiesOnBoard;
        private List<Bullet> enemyBulletsOnBoard;

        private int roomScalingFactor = 1;

        private int roomsCompleted = 0;

        private int level = 0;

        private Vector2 currentRoomIndex;

        public Map(int _sideSize, int _bufferSize, int _doorSize, double _wallChance, Random _rng) {
            sideSize = _sideSize;
            bufferSize = _bufferSize;
            doorSize = _doorSize;
            wallChance = _wallChance;
            rng = _rng;

            CreateNewMap();
        }

        public int RoomScalingFactor {
            get {
                return roomScalingFactor;
            }
            set {
                roomScalingFactor = value;
            }
        }

        public Room CurrentRoom {
            get {
                int index = GetIndexOfRoomWithIndex(currentRoomIndex);
                return rooms[index];
            }
        }

        public int RoomCount {
            get {
                return rooms.Count;
            }
        }

        public int RoomsCompleted {
            get {
                return roomsCompleted;
            }
            set {
                roomsCompleted = value;
            }
        }

        public int Level {
            get {
                return level;
            }
            set {
                level = value;
            }
        }

        public void CreateNewMap() {
            level += 1;

            Reset();

            Queue<Room> toExpand = new Queue<Room>();
            List<Vector2> visitedIndexes = new List<Vector2>();

            Room root = CreateNewRoom(new Vector2(0, 0), -1, 0);
            toExpand.Enqueue(root);
            rooms.Add(root);
            rooms[0].IsCurrentRoom = true;
            visitedIndexes.Add(new Vector2(0, 0));
            

            while(toExpand.Count > 0) {
                Room expanding = toExpand.Dequeue();
                bool[] doorsToExpand = expanding.Doors;
                int depth = expanding.Depth + 1;

                //north
                if (doorsToExpand[0]) {
                    bool roomNotVisited = true;
                    for(int i = 0; i < visitedIndexes.Count; i++) {
                        if(new Vector2(expanding.Index.X, expanding.Index.Y - 1).Equals(visitedIndexes[i])) {
                            roomNotVisited = false;
                            i = visitedIndexes.Count;
                        }
                    }
                    if(roomNotVisited){
                        Room newRoom = CreateNewRoom(new Vector2(expanding.Index.X, expanding.Index.Y - 1), 2, depth);
                        visitedIndexes.Add(newRoom.Index);
                        toExpand.Enqueue(newRoom);
                        rooms.Add(newRoom);
                    }
                }

                //east
                if (doorsToExpand[1]) {
                    bool roomNotVisited = true;
                    for(int i = 0; i < visitedIndexes.Count; i++) {
                        if(new Vector2(expanding.Index.X + 1, expanding.Index.Y).Equals(visitedIndexes[i])) {
                            roomNotVisited = false;
                            i = visitedIndexes.Count;
                        }
                    }
                    if(roomNotVisited){
                        Room newRoom = CreateNewRoom(new Vector2(expanding.Index.X + 1, expanding.Index.Y), 3, depth);
                        visitedIndexes.Add(newRoom.Index);
                        toExpand.Enqueue(newRoom);
                        rooms.Add(newRoom);
                    }
                }

                //south
                if (doorsToExpand[2]) {              
                    bool roomNotVisited = true;
                    for(int i = 0; i < visitedIndexes.Count; i++) {
                        if(new Vector2(expanding.Index.X, expanding.Index.Y + 1).Equals(visitedIndexes[i])) {
                            roomNotVisited = false;
                            i = visitedIndexes.Count;
                        }
                    }
                    if(roomNotVisited){
                        Room newRoom = CreateNewRoom(new Vector2(expanding.Index.X, expanding.Index.Y + 1), 0, depth);
                        visitedIndexes.Add(newRoom.Index);
                        toExpand.Enqueue(newRoom);
                        rooms.Add(newRoom);
                    }
                }

                //west
                if (doorsToExpand[3]) {
                    bool roomNotVisited = true;
                    for(int i = 0; i < visitedIndexes.Count; i++) {
                        if(new Vector2(expanding.Index.X - 1, expanding.Index.Y).Equals(visitedIndexes[i])) {
                            roomNotVisited = false;
                            i = visitedIndexes.Count;
                        }
                    }
                    if(roomNotVisited){
                        Room newRoom = CreateNewRoom(new Vector2(expanding.Index.X - 1, expanding.Index.Y), 1, depth);
                        visitedIndexes.Add(newRoom.Index);
                        toExpand.Enqueue(newRoom);
                        rooms.Add(newRoom);
                    }
                }
            }

            for(int i = 0; i < level; i++) {
                int roomToAddPickup = rng.Next(1, rooms.Count);
                int pickupType = rng.Next(3);
                switch (pickupType) {
                    case 0:
                        rooms[roomToAddPickup].AddPickup(new HealthPickup(new Vector2((1375 / 2) + (rooms[roomToAddPickup].Pickups.Count * 50), 1375 / 2)));
                        break;
                    case 1:
                        rooms[roomToAddPickup].AddPickup(new DamagePickup(new Vector2((1375 / 2) + (rooms[roomToAddPickup].Pickups.Count * 50), 1375 / 2)));
                        break;
                    case 2:
                        rooms[roomToAddPickup].AddPickup(new FireRatePickup(new Vector2((1375 / 2) + (rooms[roomToAddPickup].Pickups.Count * 50), 1375 / 2)));
                        break;
                    default:
                        rooms[roomToAddPickup].AddPickup(new DamagePickup(new Vector2((1375 / 2) + (rooms[roomToAddPickup].Pickups.Count * 50), 1375 / 2)));
                        break;
                } 
            }
        }

        public Room CreateNewRoom(Vector2 Index, int CreatedFrom, int Depth) {
            bool north = false;
            bool east = false;
            bool south = false;
            bool west = false;

            bool northFound = false;
            bool eastFound = false;
            bool southFound = false;
            bool westFound = false;

            //First Room
            if(CreatedFrom == -1) {
                north = CreateRoomChance();
                east = CreateRoomChance();
                south = CreateRoomChance();
                west = CreateRoomChance();

                //Force one opening if none were made
                if(!north && !east && !south && !west) {
                    int roomToOpen = rng.Next(4);
                    if(roomToOpen == 0) {
                        north = true;
                    }
                    else if(roomToOpen == 1) {
                        east = true;
                    }
                    else if(roomToOpen == 2) {
                        south = true;
                    }
                    else if(roomToOpen == 3) {
                        west = true;
                    }
                }

                return new Room(Index, sideSize, bufferSize, doorSize, north, east, south, west, CreatedFrom, Depth);
            }

            //Created From The North Side
            if(CreatedFrom == 0) {
                north = true;

                if(Index.X == 5) {
                    east = false;
                    eastFound = true;
                }

                if(Index.Y == 5) {
                    south = false;
                    southFound = true;
                }

                if(Index.X == -5) {
                    west = false;
                    westFound = true;
                }

                for(int i = 0; i < rooms.Count; i++) {
                    //Check East
                    if(!eastFound && rooms[i].Index.X == Index.X + 1 && rooms[i].Index.Y == Index.Y) {
                        if (rooms[i].Doors[3]) {
                            east = true;
                        }
                        else {
                            east = false;
                        }

                        eastFound = true;
                    }

                    //Check South
                    if(!southFound && rooms[i].Index.X == Index.X && rooms[i].Index.Y == Index.Y + 1) {
                        if (rooms[i].Doors[0]) {
                            south = true;
                        }
                        else {
                            south = false;
                        }

                        southFound = true;
                    }

                    //Check West
                    if(!westFound && rooms[i].Index.X == Index.X - 1 && rooms[i].Index.Y == Index.Y) {
                        if (rooms[i].Doors[1]) {
                            west = true;
                        }
                        else {
                            west = false;
                        }

                        westFound = true;
                    }
                }

                if (!eastFound) {
                    east = CreateRoomChance();
                }

                if (!southFound) {
                    south = CreateRoomChance();
                }

                if (!westFound) {
                    west = CreateRoomChance();
                }

                return new Room(Index, sideSize, bufferSize, doorSize, north, east, south, west, CreatedFrom, Depth);
            }

            //Created From The East Side
            if(CreatedFrom == 1) {
                east = true;

                if(Index.Y == -5) {
                    north = false;
                    northFound = true;
                }

                if(Index.Y == 5) {
                    south = false;
                    southFound = true;
                }

                if(Index.X == -5) {
                    west = false;
                    westFound = true;
                }

                for(int i = 0; i < rooms.Count; i++) {
                    //Check North
                    if(!northFound && rooms[i].Index.X == Index.X && rooms[i].Index.Y == Index.Y - 1) {
                        if (rooms[i].Doors[2]) {
                            north = true;
                        }
                        else {
                            north = false;
                        }

                        northFound = true;
                    }

                    //Check South
                    if(!southFound && rooms[i].Index.X == Index.X && rooms[i].Index.Y == Index.Y + 1) {
                        if (rooms[i].Doors[0]) {
                            south = true;
                        }
                        else {
                            south = false;
                        }

                        southFound = true;
                    }

                    //Check West
                    if(!westFound && rooms[i].Index.X == Index.X - 1 && rooms[i].Index.Y == Index.Y) {
                        if (rooms[i].Doors[1]) {
                            west = true;
                        }
                        else {
                            west = false;
                        }

                        westFound = true;
                    }
                }

                if (!northFound) {
                    north = CreateRoomChance();
                }

                if (!southFound) {
                    south = CreateRoomChance();
                }

                if (!westFound) {
                    west = CreateRoomChance();
                }

                return new Room(Index, sideSize, bufferSize, doorSize, north, east, south, west, CreatedFrom, Depth);
            }

            //Created From The South Side
            if(CreatedFrom == 2) {
                south = true;

                if(Index.X == 5) {
                    east = false;
                    eastFound = true;
                }

                if(Index.Y == -5) {
                    north = false;
                    northFound = true;
                }

                if(Index.X == -5) {
                    west = false;
                    westFound = true;
                }

                for(int i = 0; i < rooms.Count; i++) {
                    //Check East
                    if(!eastFound && rooms[i].Index.X == Index.X + 1 && rooms[i].Index.Y == Index.Y) {
                        if (rooms[i].Doors[3]) {
                            east = true;
                        }
                        else {
                            east = false;
                        }

                        eastFound = true;
                    }

                    //Check North
                    if(!northFound && rooms[i].Index.X == Index.X && rooms[i].Index.Y == Index.Y - 1) {
                        if (rooms[i].Doors[2]) {
                            north = true;
                        }
                        else {
                            north = false;
                        }

                        northFound = true;
                    }

                    //Check West
                    if(!westFound &&rooms[i].Index.X == Index.X - 1 && rooms[i].Index.Y == Index.Y) {
                        if (rooms[i].Doors[1]) {
                            west = true;
                        }
                        else {
                            west = false;
                        }

                        westFound = true;
                    }
                }

                if (!eastFound) {
                    east = CreateRoomChance();
                }

                if (!northFound) {
                    north = CreateRoomChance();
                }

                if (!westFound) {
                    west = CreateRoomChance();
                }

                return new Room(Index, sideSize, bufferSize, doorSize, north, east, south, west, CreatedFrom, Depth);
            }

            //Created From The West Side
            if(CreatedFrom == 3) {
                west = true;

                if(Index.X == 5) {
                    east = false;
                    eastFound = true;
                }

                if(Index.Y == 5) {
                    south = false;
                    southFound = true;
                }

                if(Index.Y == -5) {
                    north = false;
                    northFound = true;
                }

                for(int i = 0; i < rooms.Count; i++) {
                    //Check East
                    if(!eastFound && rooms[i].Index.X == Index.X + 1 && rooms[i].Index.Y == Index.Y) {
                        if (rooms[i].Doors[3]) {
                            east = true;
                        }
                        else {
                            east = false;
                        }

                        eastFound = true;
                    }

                    //Check South
                    if(!southFound && rooms[i].Index.X == Index.X && rooms[i].Index.Y == Index.Y + 1) {
                        if (rooms[i].Doors[0]) {
                            south = true;
                        }
                        else {
                            south = false;
                        }

                        southFound = true;
                    }

                    //Check North
                    if(!northFound && rooms[i].Index.X == Index.X && rooms[i].Index.Y == Index.Y - 1) {
                        if (rooms[i].Doors[2]) {
                            north = true;
                        }
                        else {
                            north = false;
                        }

                        northFound = true;
                    }
                }

                if (!eastFound) {
                    east = CreateRoomChance();
                }

                if (!southFound) {
                    south = CreateRoomChance();
                }

                if (!northFound) {
                    north = CreateRoomChance();
                }

                return new Room(Index, sideSize, bufferSize, doorSize, north, east, south, west, CreatedFrom, Depth);
            }

            return null;
        }

        public bool CreateRoomChance() {
            double chance = rng.NextDouble();
            return chance >= wallChance;
        }

        public void Draw(SpriteBatch sb, SpriteFont font, MapMode mapState) {
            Vector2 offset = new Vector2(0, 0) - currentRoomIndex;

            if(mapState == MapMode.Room) {
                DrawEnemies();
                DrawBullets();

                ShapeBatch.Box(0, 0, 1375, 55, Color.Black);
                ShapeBatch.Box(0, 0, 55, 1375, Color.Black);
                ShapeBatch.Box(0, 1320, 1375, 55, Color.Black);
                ShapeBatch.Box(1320, 0, 55, 1375, Color.Black);

                ShapeBatch.Box(522, 55, 330, 110, Color.Black);
                ShapeBatch.Box(55, 522, 110, 330, Color.Black);
                ShapeBatch.Box(522, 1210, 330, 110, Color.Black);
                ShapeBatch.Box(1210, 522, 110, 330, Color.Black);
            }

            ShapeBatch.Box(55, 1330, ((float)(roomsCompleted) / (rooms.Count - 1)) * 1265.0f, 35, Color.White); 

            for(int i = 0; i < rooms.Count; i++) {
                rooms[i].Draw(sb, font, roomScalingFactor, offset, mapState);
            }
        }

        public void Move(Vector2 moveToMake, Character player) {
            int indexOfAttemptedMove = GetIndexOfRoomWithIndex(currentRoomIndex + moveToMake);
            if(indexOfAttemptedMove != -1) {
                bool validMove = false;

                if(moveToMake.Equals(new Vector2(0, -1))) {
                    validMove = rooms[indexOfAttemptedMove].Doors[2];
                }
                else if(moveToMake.Equals(new Vector2(1, 0))) {
                    validMove = rooms[indexOfAttemptedMove].Doors[3];
                }
                else if(moveToMake.Equals(new Vector2(0, 1))) {
                    validMove = rooms[indexOfAttemptedMove].Doors[0];
                }
                else if(moveToMake.Equals(new Vector2(-1, 0))) {
                    validMove = rooms[indexOfAttemptedMove].Doors[1];
                }

                if (validMove) {
                    rooms[GetIndexOfRoomWithIndex(currentRoomIndex)].IsCurrentRoom = false;
                    rooms[GetIndexOfRoomWithIndex(currentRoomIndex += moveToMake)].IsCurrentRoom = true;
                    ClearBullets();
                    player.BulletTimer = 50;
                    if (!rooms[GetIndexOfRoomWithIndex(currentRoomIndex)].Completed) {
                        CreateEnemies();
                    }
                }
            }
        }

        public int GetIndexOfRoomWithIndex(Vector2 Index) {
            for(int i = 0; i < rooms.Count; i++) {
                if (Index.Equals(rooms[i].Index)) {
                    return i;
                }
            }
            return -1;
        }

        public void AddBullet(Bullet bulletToAdd) {
            bulletsOnBoard.Add(bulletToAdd);
        }

        public void MoveBullets() {
            for(int i = 0; i < bulletsOnBoard.Count; i++) {
                if (bulletsOnBoard[i].Move()) {
                    bulletsOnBoard.RemoveAt(i);
                    i--;
                }
            }

            for(int i = 0; i < enemyBulletsOnBoard.Count; i++) {
                if (enemyBulletsOnBoard[i].Move()) {
                    enemyBulletsOnBoard.RemoveAt(i);
                    i--;
                }
            }
        }

        public void DrawBullets() {
            foreach(Bullet i in bulletsOnBoard) {
                i.Draw();
            }

            foreach(Bullet i in enemyBulletsOnBoard) {
                i.Draw();
            }
        }

        public void ClearBullets() {
            bulletsOnBoard = new List<Bullet>();
            enemyBulletsOnBoard = new List<Bullet>();
        }

        public void CreateEnemies() {
            enemiesOnBoard = new List<Enemy>();

            if(currentRoomIndex == new Vector2(0, 0)) {
                return;
            }

            int numEnemies = rng.Next(4, 8);
            for(int i = 0; i < numEnemies; i++) {
                Vector2 EnemyPos = new Vector2(rng.Next(175, 1170), rng.Next(175, 1170));
                enemiesOnBoard.Add(new Enemy(EnemyPos, 20, 30));
            }
        }

        public void DrawEnemies() {
            foreach(Enemy i in enemiesOnBoard) {
                i.Draw();
            }
        }

        public bool EnemiesTakeDamage(Character player) {
            for(int i = 0; i < enemiesOnBoard.Count; i++) {
                Enemy e = enemiesOnBoard[i];
                for(int j = 0; j < bulletsOnBoard.Count; j++) {
                    Bullet b = bulletsOnBoard[j];
                    double distanceBetweenCentersSquared = Math.Pow(e.Position.X - b.Position.X, 2) + Math.Pow(e.Position.Y - b.Position.Y, 2);
                    double radiiDistanceSquared = Math.Pow((e.Size / 2) + b.Size, 2);
                    if(distanceBetweenCentersSquared <= radiiDistanceSquared) {
                        e.Health -= b.Damage;
                        bulletsOnBoard.RemoveAt(j);
                        j--;
                        if(e.Health <= 0) {
                            enemiesOnBoard.RemoveAt(i);
                            i--;
                            j = bulletsOnBoard.Count;
                        }
                    }
                }
            }

            if(enemiesOnBoard.Count == 0 && CurrentRoom.Completed != true) {
                CurrentRoom.Completed = true;
                if(currentRoomIndex != new Vector2(0, 0)) {
                    roomsCompleted += 1;
                } 
            }

            if (roomsCompleted == rooms.Count) {
                CreateNewMap();
                player.Reset(bufferSize, sideSize);
                return true;
            }

            return false;
        }

        public void EnemiesFire(Character player) {
            foreach(Enemy e in enemiesOnBoard) {
                Bullet bulletToAdd = e.CreateBullet(player.Position);
                if(bulletToAdd != null) {
                    enemyBulletsOnBoard.Add(bulletToAdd);
                }
            }
        }

        public bool DamagePlayer(Character player) {
            for(int i = 0; i < enemyBulletsOnBoard.Count; i++) {
                if(player.Health <= 0) {
                    return true;
                }
                Bullet b = enemyBulletsOnBoard[i];
                double distanceBetweenCentersSquared = Math.Pow(player.Position.X - b.Position.X, 2) + Math.Pow(player.Position.Y - b.Position.Y, 2);
                double radiiDistanceSquared = Math.Pow((player.Size / 2) + b.Size, 2);
                if(distanceBetweenCentersSquared <= radiiDistanceSquared) {
                    player.takeDamage(b);
                    enemyBulletsOnBoard.RemoveAt(i);
                    i--;
                }
            }
            return false;
        }

        public void Reset() {
            enemiesOnBoard = new List<Enemy>();
            roomsCompleted = 0;
            ClearBullets();

            rooms = new List<Room>();
            currentRoomIndex = new Vector2(0, 0);
        }

        public void CheckPickups(Character player) {
            if (!CurrentRoom.Completed) {
                return;
            }
            for(int i = 0; i < CurrentRoom.Pickups.Count; i++) {
                if (CurrentRoom.Pickups[i].CheckCollision(player)) {
                    CurrentRoom.Pickups.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
