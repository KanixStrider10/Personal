using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace GameJam1 {
    class Map {
        List<List<Tile>> map;
        int levelNum;
        Character player;
        int numGoals = 0;

        public Map(int _levelNum) {
            map = new List<List<Tile>>();
            levelNum = _levelNum;
            ReadLevelFromFile();
        }

        public int LevelNum {
            get {
                return levelNum;
            }
        }

        private void ReadLevelFromFile() {
            try {
                //Create StreamReader
                StreamReader input = new StreamReader("..\\..\\..\\Levels.txt");
                string line = "";
                string[] splitLine;

                //Move past notes
                while (line != "^^^^^") {
                    line = input.ReadLine();
                }

                //Go to the correct level in the file
                while(line != String.Format(">{0}", levelNum)) {
                    line = input.ReadLine();
                }

                line = input.ReadLine();
                splitLine = line.Split(' ');
                int levelWidth = Int32.Parse(splitLine[0]);
                int levelHeight = Int32.Parse(splitLine[1]);

                for(int i = 0; i < levelHeight; i++) {
                    line = input.ReadLine();
                    splitLine = line.Split(' ');
                    List<Tile> newRow = new List<Tile>();;
                    for(int j = 0; j < levelWidth; j++) {
                        if (splitLine[j].Equals("t")) {
                            newRow.Add(new Tile());
                        }
                        if (splitLine[j].Equals("e")) {
                            newRow.Add(new EmptyTile());
                        }
                        if (splitLine[j].Equals("w")) {
                            newRow.Add(new Wall());
                        }
                        if (splitLine[j].Equals("0")) {
                            newRow.Add(new Mirror(0));
                        }
                        if (splitLine[j].Equals("1")) {
                            newRow.Add(new Mirror(1));
                        }
                        if (splitLine[j].Equals("g")) {
                            newRow.Add(new Goal());
                            numGoals += 1;
                        }
                        if (splitLine[j].Equals("p")) {
                            newRow.Add(new Tile());
                            player = new Character(new Vector2(j, i), 35);
                        }
                    }
                    map.Add(newRow);
                }

                //Get each line of the level, and split it into individual tiles
                input.Close();
            }
            catch (Exception e) {
                Debug.WriteLine(e.Message);
            }
        }

        //Find all tiles visible to the player
        public void FindVisibleTiles() {
            ResetVisible();
            //Set current tile visible
            map[(int)player.Pos.Y][(int)player.Pos.X].Visible = true;

            //check North
            bool checking = true;
            int counter = 1;
            while (checking) {
                if((int)player.Pos.Y - counter < 0) {
                    checking = false;
                    break;
                }

                map[(int)player.Pos.Y - counter][(int)player.Pos.X].Visible = true;

                if(map[(int)player.Pos.Y - counter][(int)player.Pos.X] is Mirror) {
                    FindMirrorVisibles(new Vector2(player.Pos.X, player.Pos.Y - counter), new Vector2(0, -1));
                    checking = false;
                    break;
                }

                if(map[(int)player.Pos.Y - counter][(int)player.Pos.X] is Wall) {
                    checking = false;
                    break;
                }
                counter++;
            }

            //check East
            checking = true;
            counter = 1;
            while (checking) {
                if((int)player.Pos.X + counter >= map[0].Count) {
                    checking = false;
                    break;
                }

                map[(int)player.Pos.Y][(int)player.Pos.X + counter].Visible = true;

                if(map[(int)player.Pos.Y][(int)player.Pos.X + counter] is Mirror) {
                    FindMirrorVisibles(new Vector2(player.Pos.X + counter, player.Pos.Y), new Vector2(1, 0));
                    checking = false;
                    break;
                }

                if(map[(int)player.Pos.Y][(int)player.Pos.X + counter] is Wall) {
                    checking = false;
                    break;
                }
                counter++;
            }

            //check South
            checking = true;
            counter = 1;
            while (checking) {
                if((int)player.Pos.Y + counter >= map.Count) {
                    checking = false;
                    break;
                }

                map[(int)player.Pos.Y + counter][(int)player.Pos.X].Visible = true;

                if(map[(int)player.Pos.Y + counter][(int)player.Pos.X] is Mirror) {
                    FindMirrorVisibles(new Vector2(player.Pos.X, player.Pos.Y + counter), new Vector2(0, 1));
                    checking = false;
                    break;
                }

                if(map[(int)player.Pos.Y + counter][(int)player.Pos.X] is Wall) {
                    checking = false;
                    break;
                }
                counter++;
            }

            //check West
            checking = true;
            counter = 1;
            while (checking) {
                if((int)player.Pos.X - counter < 0) {
                    checking = false;
                    break;
                }

                map[(int)player.Pos.Y][(int)player.Pos.X - counter].Visible = true;

                if(map[(int)player.Pos.Y][(int)player.Pos.X - counter] is Mirror) {
                    FindMirrorVisibles(new Vector2(player.Pos.X - counter, player.Pos.Y), new Vector2(-1, 0));
                    checking = false;
                    break;
                }

                if(map[(int)player.Pos.Y][(int)player.Pos.X - counter] is Wall) {
                    checking = false;
                    break;
                }
                counter++;
            }
        }

        //Find all of the tiles that should be visible given a certain mirror
        public void FindMirrorVisibles(Vector2 mirrorPos, Vector2 visibleFrom) {
            if(map[(int)mirrorPos.Y][(int)mirrorPos.X].AlreadyReflected(visibleFrom)) {
                return;
            }

            bool checking = true;
            int counter = 1;
            //viewed going north
            if(visibleFrom.X == 0 && visibleFrom.Y == -1) {
                //reflect west
                if(map[(int)mirrorPos.Y][(int)mirrorPos.X].Direction == 0) {
                    while (checking) {
                        if((int)mirrorPos.X - counter < 0) {
                            checking = false;
                            break;
                        }

                        map[(int)mirrorPos.Y][(int)mirrorPos.X - counter].Visible = true;

                        if(map[(int)mirrorPos.Y][(int)mirrorPos.X - counter] is Mirror) {
                            FindMirrorVisibles(new Vector2(mirrorPos.X - counter, mirrorPos.Y), new Vector2(-1, 0));
                            checking = false;
                            break;
                        }

                        if(map[(int)mirrorPos.Y][(int)mirrorPos.X - counter] is Wall) {
                            checking = false;
                            break;
                        }
                        counter++;
                    }
                }

                //reflect east
                if(map[(int)mirrorPos.Y][(int)mirrorPos.X].Direction == 1) {
                    while (checking) {
                        if((int)mirrorPos.X + counter >= map[0].Count) {
                            checking = false;
                            break;
                        }

                        map[(int)mirrorPos.Y][(int)mirrorPos.X + counter].Visible = true;

                        if(map[(int)mirrorPos.Y][(int)mirrorPos.X + counter] is Mirror) {
                            FindMirrorVisibles(new Vector2(mirrorPos.X + counter, mirrorPos.Y), new Vector2(1, 0));
                            checking = false;
                            break;
                        }

                        if(map[(int)mirrorPos.Y][(int)mirrorPos.X + counter] is Wall) {
                            checking = false;
                            break;
                        }
                        counter++;
                    }
                }
            }
            //viewed going south
            else if(visibleFrom.X == 0 && visibleFrom.Y == 1) {
                //reflect east
                if(map[(int)mirrorPos.Y][(int)mirrorPos.X].Direction == 0) {
                    while (checking) {
                        if((int)mirrorPos.X + counter >= map[0].Count) {
                            checking = false;
                            break;
                        }

                        map[(int)mirrorPos.Y][(int)mirrorPos.X + counter].Visible = true;

                        if(map[(int)mirrorPos.Y][(int)mirrorPos.X + counter] is Mirror) {
                            FindMirrorVisibles(new Vector2(mirrorPos.X + counter, mirrorPos.Y), new Vector2(1, 0));
                            checking = false;
                            break;
                        }

                        if(map[(int)mirrorPos.Y][(int)mirrorPos.X + counter] is Wall) {
                            checking = false;
                            break;
                        }
                        counter++;
                    }
                }

                //reflect west
                if(map[(int)mirrorPos.Y][(int)mirrorPos.X].Direction == 1) {
                    while (checking) {
                        if((int)mirrorPos.X - counter < 0) {
                            checking = false;
                            break;
                        }

                        map[(int)mirrorPos.Y][(int)mirrorPos.X - counter].Visible = true;

                        if(map[(int)mirrorPos.Y][(int)mirrorPos.X - counter] is Mirror) {
                            FindMirrorVisibles(new Vector2(mirrorPos.X - counter, mirrorPos.Y), new Vector2(-1, 0));
                            checking = false;
                            break;
                        }

                        if(map[(int)mirrorPos.Y][(int)mirrorPos.X - counter] is Wall) {
                            checking = false;
                            break;
                        }
                        counter++;
                    }
                }
            }
            //viewed going west
            else if(visibleFrom.X == -1 && visibleFrom.Y == 0) {
                //reflect north
                if(map[(int)mirrorPos.Y][(int)mirrorPos.X].Direction == 0) {
                    while (checking) {
                        if((int)mirrorPos.Y - counter < 0) {
                            checking = false;
                            break;
                        }

                        map[(int)mirrorPos.Y - counter][(int)mirrorPos.X].Visible = true;

                        if(map[(int)mirrorPos.Y - counter][(int)mirrorPos.X] is Mirror) {
                            FindMirrorVisibles(new Vector2(mirrorPos.X, mirrorPos.Y - counter), new Vector2(0, -1));
                            checking = false;
                            break;
                        }

                        if(map[(int)mirrorPos.Y - counter][(int)mirrorPos.X] is Wall) {
                            checking = false;
                            break;
                        }
                        counter++;
                    }
                }

                //reflect south
                if(map[(int)mirrorPos.Y][(int)mirrorPos.X].Direction == 1) {
                    while (checking) {
                        if((int)mirrorPos.Y + counter >= map.Count) {
                            checking = false;
                            break;
                        }

                        map[(int)mirrorPos.Y + counter][(int)mirrorPos.X].Visible = true;

                        if(map[(int)mirrorPos.Y + counter][(int)mirrorPos.X] is Mirror) {
                            FindMirrorVisibles(new Vector2(mirrorPos.X, mirrorPos.Y + counter), new Vector2(0, 1));
                            checking = false;
                            break;
                        }

                        if(map[(int)mirrorPos.Y + counter][(int)mirrorPos.X] is Wall) {
                            checking = false;
                            break;
                        }
                        counter++;
                    }
                }
            }
            //viewed going east
            else if(visibleFrom.X == 1 && visibleFrom.Y == 0) {
                //reflect south
                if(map[(int)mirrorPos.Y][(int)mirrorPos.X].Direction == 0) {
                    while (checking) {
                        if((int)mirrorPos.Y + counter >= map.Count) {
                            checking = false;
                            break;
                        }

                        map[(int)mirrorPos.Y + counter][(int)mirrorPos.X].Visible = true;

                        if(map[(int)mirrorPos.Y + counter][(int)mirrorPos.X] is Mirror) {
                            FindMirrorVisibles(new Vector2(mirrorPos.X, mirrorPos.Y + counter), new Vector2(0, 1));
                            checking = false;
                            break;
                        }

                        if(map[(int)mirrorPos.Y + counter][(int)mirrorPos.X] is Wall) {
                            checking = false;
                            break;
                        }
                        counter++;
                    }
                }

                //reflect north
                if(map[(int)mirrorPos.Y][(int)mirrorPos.X].Direction == 1) {
                    while (checking) {
                        if((int)mirrorPos.Y - counter < 0) {
                            checking = false;
                            break;
                        }

                        map[(int)mirrorPos.Y - counter][(int)mirrorPos.X].Visible = true;

                        if(map[(int)mirrorPos.Y - counter][(int)mirrorPos.X] is Mirror) {
                            FindMirrorVisibles(new Vector2(mirrorPos.X, mirrorPos.Y - counter), new Vector2(0, -1));
                            checking = false;
                            break;
                        }

                        if(map[(int)mirrorPos.Y - counter][(int)mirrorPos.X] is Wall) {
                            checking = false;
                            break;
                        }
                        counter++;
                    }
                }
            }
        }

        public void ResetVisible() {
            foreach(List<Tile> i in map) {
                foreach(Tile t in i) {
                    t.Visible = false;
                    t.ResetReflected();
                }
            }
        }

        public void MovePlayer(Vector2 move) {
            Vector2 attemptPos = player.Pos + move;
            if(attemptPos.X >= 0 && attemptPos.X < map[0].Count && attemptPos.Y >= 0 && attemptPos.Y < map.Count) {
                if(map[(int)attemptPos.Y][(int)attemptPos.X].CanStandOn) {
                    player.Pos += move;
                }
                else if(map[(int)attemptPos.Y][(int)attemptPos.X] is Mirror) {
                    if(map[(int)attemptPos.Y][(int)attemptPos.X].Direction == 0) {
                        map[(int)attemptPos.Y][(int)attemptPos.X].Direction = 1;
                    }
                    else {
                        map[(int)attemptPos.Y][(int)attemptPos.X].Direction = 0;
                    }
                }
            }
        }

        public bool LevelCompleted() {
            int numGoalsVisible = 0;
            for(int i = 0; i < map.Count; i++) {
                for(int j = 0; j < map[0].Count; j++) {
                    if(map[i][j] is Goal && map[i][j].Visible) {
                        numGoalsVisible += 1;
                        if(numGoalsVisible == numGoals) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void Draw(int tileSize, int numTilesInRow) {
            //Map cords work like map[y][x] = tile at (x, y)
            int leftBuffer = (numTilesInRow - map[0].Count) / 2;
            int topBuffer = (numTilesInRow - map.Count) / 2;
            for(int i = 0; i < map.Count; i++) {
                for(int j = 0; j < map[i].Count; j++) {
                    map[i][j].Draw(new Rectangle((leftBuffer + j) * tileSize, (topBuffer + i) * tileSize, tileSize, tileSize));
                }
            }

            Vector2 playerDrawPos = new Vector2();
            playerDrawPos.X = tileSize * (leftBuffer + player.Pos.X) + (tileSize / 2);
            playerDrawPos.Y = tileSize * (topBuffer + player.Pos.Y) + (tileSize / 2);
            player.Draw(playerDrawPos);
        }
    }
}
