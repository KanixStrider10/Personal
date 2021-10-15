using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GameJam1 {
    /* Theme:
     * If you don't see it, it doesn't exist
     * 
     * 6hr time limit
     * 
     * 10/15/21
     */
    enum GameState {
        LevelSelect,
        InGame
    }
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardState pv;
        private SpriteFont levelFont;
        private SpriteFont titleFont;
        private SpriteFont textFont;

        private string title = "[TITLE]";

        private Map levelMap;
        private int tileSize = 80;
        private int numTilesInRow = 11;

        private int numLevels = 10;
        private List<LevelSelectArea> levelSelectAreas;

        private GameState gameState = GameState.LevelSelect;

        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            _graphics.PreferredBackBufferWidth = tileSize * numTilesInRow;
            _graphics.PreferredBackBufferHeight = tileSize * numTilesInRow;
            _graphics.ApplyChanges();

            levelSelectAreas = new List<LevelSelectArea>();
            for(int i = 0; i < numLevels; i++) {
                levelSelectAreas.Add(new LevelSelectArea(new Rectangle(tileSize * (3 + (i % 5)), tileSize * (5 + (i / 5)), tileSize, tileSize), i + 1));
            }

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            textFont = Content.Load<SpriteFont>("arial12");
            levelFont = Content.Load<SpriteFont>("arial40");
            titleFont = Content.Load<SpriteFont>("arial60");
        }

        protected override void Update(GameTime gameTime) {
            if (PressedThisFrame(Keyboard.GetState(), Keys.Escape)) {
                if(gameState == GameState.InGame) {
                    gameState = GameState.LevelSelect;
                }
                else {
                    Exit();
                }
            }

            if(gameState == GameState.InGame) {
                levelMap.FindVisibleTiles();

                if(PressedThisFrame(Keyboard.GetState(), Keys.W)){
                    levelMap.MovePlayer(new Vector2(0, -1));
                }

                if(PressedThisFrame(Keyboard.GetState(), Keys.A)){
                    levelMap.MovePlayer(new Vector2(-1, 0));
                }

                if(PressedThisFrame(Keyboard.GetState(), Keys.S)){
                    levelMap.MovePlayer(new Vector2(0, 1));
                }

                if(PressedThisFrame(Keyboard.GetState(), Keys.D)){
                    levelMap.MovePlayer(new Vector2(1, 0));
                } 

                if(PressedThisFrame(Keyboard.GetState(), Keys.Enter) && levelMap.LevelCompleted()) {
                    levelSelectAreas[levelMap.LevelNum].LevelCompleted = true;
                    if(levelMap.LevelNum != numLevels - 1) {
                        levelMap = new Map(levelMap.LevelNum + 1);
                    }
                    else {
                        gameState = GameState.LevelSelect;
                    }
                }
            }

            if(gameState == GameState.LevelSelect) {
                if(Mouse.GetState().LeftButton == ButtonState.Pressed) {
                    for(int i = 0; i < levelSelectAreas.Count; i++) {
                        if(levelSelectAreas[i].Rect.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y))){
                            if(i == 0) {
                                levelMap = new Map(i);
                                gameState = GameState.InGame;
                                break;
                            }
                            else if(levelSelectAreas[i].LevelCompleted || levelSelectAreas[i - 1].LevelCompleted) {
                                levelMap = new Map(i);
                                gameState = GameState.InGame;
                                break;
                            }
                        }
                    }
                }
            }

            pv = Keyboard.GetState();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            ShapeBatch.Begin(GraphicsDevice);
            _spriteBatch.Begin();


            if(gameState == GameState.InGame) {
                levelMap.Draw(tileSize, numTilesInRow);
                if(levelMap.LevelNum == 0) {
                    _spriteBatch.DrawString(textFont,"Complete Each Level By Seeing All Targets Then Pressing Enter", new Vector2(50, 50), Color.White);
                }
                if(levelMap.LevelNum == 2) {
                    _spriteBatch.DrawString(textFont,"Some Levels Have More Than One Target", new Vector2(50, 50), Color.White);
                }
                if(levelMap.LevelNum == 3) {
                    _spriteBatch.DrawString(textFont,"Mirrors Will Reflect Your Sightline Around Corners", new Vector2(50, 50), Color.White);
                }
                if(levelMap.LevelNum == 4) {
                    _spriteBatch.DrawString(textFont,"Walking Into Mirrors Will Rotate Them", new Vector2(50, 50), Color.White);
                }
            }

            if(gameState == GameState.LevelSelect) {
                Vector2 titleStringLength = titleFont.MeasureString(title);
                _spriteBatch.DrawString(titleFont, title, new Vector2((GraphicsDevice.Viewport.Width / 2) - (titleStringLength.X / 2), (tileSize * 2.5f) - (titleStringLength.Y / 2)), Color.White);
                for(int i = 0; i < levelSelectAreas.Count; i++) {
                    if(i == 0) {
                        levelSelectAreas[i].Draw(levelFont, _spriteBatch, true);
                    }
                    else if(levelSelectAreas[i - 1].LevelCompleted) {
                        levelSelectAreas[i].Draw(levelFont, _spriteBatch, true);
                    }
                    else {
                        levelSelectAreas[i].Draw(levelFont, _spriteBatch, false);
                    }
                }
            }

            ShapeBatch.End();
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public bool PressedThisFrame(KeyboardState currentState, Keys keyToCheck) {
            return !pv.IsKeyDown(keyToCheck) && currentState.IsKeyDown(keyToCheck);
        }
    }
}
