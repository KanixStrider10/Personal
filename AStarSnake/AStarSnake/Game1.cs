using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace AStarSnake {
    enum GameState {
        Menu,
        Records,
        AStar,
        Manual
    }

    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Snake s;
        private Food f;
        private int segSize = 40;
        private int startingSize = 3;

        private Texture2D snakeTexture;
        private Texture2D foodTexture;
        private SpriteFont font;
        private Random rng;

        private int lastScore = 0;
        private List<int> highScores;

        private GameState gameState = GameState.Menu;

        private Vector2 currentDirection;
        private int frameCounter = 0;

        private VisualGraph percentagesGraph;
        private VisualGraph scoresGraph;

        private bool showGraphs = true;

        private KeyboardState pv;

        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 33 * segSize;
            _graphics.PreferredBackBufferHeight = 33 * segSize;
            _graphics.ApplyChanges();

            rng = new Random();
            highScores = new List<int>();
            highScores.Add(0);
            highScores.Add(0);
            highScores.Add(0);

            percentagesGraph = new VisualGraph(200, 120, 400, 80);
            scoresGraph = new VisualGraph(640, 120, 400, 80, Color.Blue);

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            snakeTexture = Content.Load<Texture2D>("snakeTexture");
            foodTexture = Content.Load<Texture2D>("FoodTexture");
            font = Content.Load<SpriteFont>("Arial12");
            
            // TODO: use this.Content to load your game content here
            s = new Snake(startingSize, segSize, snakeTexture);
            f = new Food(segSize, rng, foodTexture);
            currentDirection = new Vector2(0, -1);
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (gameState) {
                case GameState.Menu:
                    if (Keyboard.GetState().IsKeyDown(Keys.M)) {
                        gameState = GameState.Manual;
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.A)) {
                        gameState = GameState.AStar;
                    }
                    break;

                case GameState.Records:
                    if (Keyboard.GetState().IsKeyDown(Keys.M)) {
                        gameState = GameState.Menu;
                    }
                    break;

                case GameState.AStar:
                    if (KeyPressedThisFrame(Keys.M, Keyboard.GetState())) {
                        if (s.DrawMap) {
                            s.DrawMap = false;
                        }
                        else {
                            s.DrawMap = true;
                        }
                    }
                    if (KeyPressedThisFrame(Keys.G, Keyboard.GetState())) {
                        if (showGraphs) {
                            showGraphs = false;
                        }
                        else {
                            showGraphs = true;
                        }
                    }
                    CheckIfDead();
                    break;

                case GameState.Manual:
                    if(Keyboard.GetState().IsKeyDown(Keys.W)) {
                        if(currentDirection.Y != 1) {
                            currentDirection = new Vector2(0, -1);
                        }
                    }
                    else if(Keyboard.GetState().IsKeyDown(Keys.A)) {
                        if(currentDirection.X != 1) {
                            currentDirection = new Vector2(-1, 0);
                        }
                    }
                    else if(Keyboard.GetState().IsKeyDown(Keys.S)) {
                        if(currentDirection.Y != -1) {
                            currentDirection = new Vector2(0, 1);
                        }
                    }
                    else if(Keyboard.GetState().IsKeyDown(Keys.D)) {
                        if(currentDirection.X != -1) {
                            currentDirection = new Vector2(1, 0);
                        }
                    }
                    CheckIfDead();
                    break;
            }

            pv = Keyboard.GetState();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            ShapeBatch.Begin(GraphicsDevice);

            switch (gameState) {
                case GameState.Menu:
                    _spriteBatch.DrawString(font, "Press M For Manual", new Vector2(segSize, segSize), Color.White);
                    _spriteBatch.DrawString(font, "Press A For A Star", new Vector2(segSize, 2 * segSize), Color.White);
                    break;
                case GameState.AStar:
                    frameCounter += 1;
                    s.MoveAStar(_spriteBatch, f, font, frameCounter);
                    f.Draw(_spriteBatch);

                    if (s.NewPercentage) {
                        percentagesGraph.Add(s.Percentage);
                        s.NewPercentage = false;
                    }
                    if (showGraphs) {
                        percentagesGraph.Draw();
                        scoresGraph.Draw();
                    }

                    _spriteBatch.DrawString(font, "Score: " + s.Score, new Vector2(segSize, segSize), Color.White);
                    _spriteBatch.DrawString(font, "Last Score: " + lastScore, new Vector2(segSize, 2 * segSize), Color.White);
                    _spriteBatch.DrawString(font, $"High Scores: {highScores[0]} {highScores[1]} {highScores[2]}", new Vector2(segSize, 3 * segSize), Color.White);
                    _spriteBatch.DrawString(font, "%: " + s.Percentage * 100, new Vector2(120, 40), Color.White);
                    break;

                case GameState.Manual:
                    frameCounter += 1;
                    s.MoveManual(_spriteBatch, currentDirection, frameCounter, f, font);
                    f.Draw(_spriteBatch);

                    _spriteBatch.DrawString(font, "Score: " + s.Score, new Vector2(segSize, segSize), Color.White);
                    _spriteBatch.DrawString(font, "Last Score: " + lastScore, new Vector2(segSize, 2 * segSize), Color.White);
                    _spriteBatch.DrawString(font, $"High Scores: {highScores[0]} {highScores[1]} {highScores[2]}", new Vector2(segSize, 3 * segSize), Color.White);
                    break;
            }
            _spriteBatch.End();
            ShapeBatch.End();

            base.Draw(gameTime);
        }

        public void CheckIfDead() {
            if (s.Dead) {
                lastScore = s.Score;
                scoresGraph.Add(lastScore);
                s = new Snake(startingSize, segSize, snakeTexture);
                percentagesGraph.Reset();
                currentDirection = new Vector2(0, -1);
                for(int i = 0; i < 3; i++) {
                    if(highScores[i] < lastScore) {
                        highScores.Insert(i, lastScore);
                        highScores.RemoveAt(3);
                        i = 3;
                    }
                }
            } 
        }

        public bool KeyPressedThisFrame(Keys k, KeyboardState ks) {
            return ks.IsKeyDown(k) && !pv.IsKeyDown(k);
        }
    }
}
