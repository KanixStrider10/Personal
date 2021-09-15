using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace RnadomMapGenerationExperiment {
    enum MapMode {
        Room,
        Map,
        GameOver
    }
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        int sideSize = 115;
        int bufferSize;
        int doorSize = 30;
        double wallChance = .7;
        int playerSize = 30;

        MapMode mapState = MapMode.Map;

        Character player;
        
        Map gameMap;

        KeyboardState pv;
        SpriteFont font;

        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            // TODO: Add your initialization logic here

            bufferSize = (125 - sideSize) / 2;

            _graphics.PreferredBackBufferWidth = (sideSize + (2 * bufferSize)) * 11;
            _graphics.PreferredBackBufferHeight = (sideSize + (2 * bufferSize)) * 11;
            _graphics.ApplyChanges();

            gameMap = new Map(sideSize, bufferSize, doorSize, wallChance, new Random());
            player = new Character(new Vector2((((sideSize + (2 * bufferSize)) * 11) / 2), (((sideSize + (2 * bufferSize)) * 11) / 2)), playerSize);

            Mouse.SetCursor(MouseCursor.Crosshair);

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Arial20");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            if (KeyPressedThisFrame(Keys.R, Keyboard.GetState())) {
                gameMap.Level = 0;
                gameMap.CreateNewMap();
                player = new Character(new Vector2((((sideSize + (2 * bufferSize)) * 11) / 2), (((sideSize + (2 * bufferSize)) * 11) / 2)), playerSize);
                gameMap.RoomScalingFactor = 1;
                mapState = MapMode.Map;
            }
            
            if(KeyPressedThisFrame(Keys.Tab, Keyboard.GetState())) {
                switch (mapState) {
                    case MapMode.Map:
                        gameMap.RoomScalingFactor = 11;
                        mapState = MapMode.Room;
                        break;

                    case MapMode.Room:
                        gameMap.RoomScalingFactor = 1;
                        mapState = MapMode.Map;
                        break;
                }
            }

            if(mapState == MapMode.Room) {
                if(Keyboard.GetState().IsKeyDown(Keys.W)){
                    if(player.Move(new Vector2(0, -1), gameMap.CurrentRoom)) {
                        gameMap.Move(new Vector2(0, -1), player);
                    }
                }
                
                if(Keyboard.GetState().IsKeyDown(Keys.A)){
                    if(player.Move(new Vector2(-1, 0), gameMap.CurrentRoom)) {
                        gameMap.Move(new Vector2(-1, 0), player);
                    }
                }
                
                if(Keyboard.GetState().IsKeyDown(Keys.S)){
                    if(player.Move(new Vector2(0, 1), gameMap.CurrentRoom)) {
                        gameMap.Move(new Vector2(0, 1), player);
                    }
                }
                
                if(Keyboard.GetState().IsKeyDown(Keys.D)){
                    if(player.Move(new Vector2(1, 0), gameMap.CurrentRoom)) {
                        gameMap.Move(new Vector2(1, 0), player);
                    }
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Enter)) {
                    if(gameMap.RoomsCompleted == gameMap.RoomCount - 1) {
                        gameMap.RoomsCompleted += 1;
                    }
                }

                gameMap.MoveBullets();

                if (gameMap.EnemiesTakeDamage(player)) {
                    gameMap.RoomScalingFactor = 1;
                    mapState = MapMode.Map;
                }
            }

            if (!gameMap.CurrentRoom.Completed && mapState == MapMode.Room) {
                Bullet bulletToAdd = player.CreateBullet(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
                if(bulletToAdd != null) {
                    gameMap.AddBullet(bulletToAdd);
                }
                gameMap.EnemiesFire(player);
                if (gameMap.DamagePlayer(player)) {
                    mapState = MapMode.GameOver;
                }
            }

            gameMap.CheckPickups(player);

            pv = Keyboard.GetState();

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            ShapeBatch.Begin(GraphicsDevice);


            if(mapState == MapMode.GameOver) {
                _spriteBatch.DrawString(font, "Game Over!", new Vector2(500, 500), Color.White);
            }
            else {
                gameMap.Draw(_spriteBatch, font, mapState);

                if(mapState == MapMode.Room) {
                    player.Draw();

                    if(gameMap.RoomsCompleted == gameMap.RoomCount - 1) {
                        _spriteBatch.DrawString(font, "Press Enter To Continue", new Vector2(220, 220), Color.White);
                    }
                }
            }

            ShapeBatch.End();
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public bool KeyPressedThisFrame(Keys k, KeyboardState ks) {
            return ks.IsKeyDown(k) && !pv.IsKeyDown(k);
        }
    }
}
