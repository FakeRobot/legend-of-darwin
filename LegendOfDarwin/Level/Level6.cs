using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LegendOfDarwin.MenuObject;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using LegendOfDarwin.GameObject;

namespace LegendOfDarwin.Level
{
    class Level6
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public GraphicsDevice device;

        private GameState gameState;
        private GameStart gameStart;

        private GameBoard board;

        private Darwin darwin;
        private ZombieTime zTime;
        private ZombieTime zTimeReset;

        public SpriteFont messageFont;

        public bool gameOver = false;
        public bool gameWin = false;

        public Texture2D gameOverTexture;
        public Texture2D gameWinTexture;

        Vector2 gameOverPosition = Vector2.Zero;

        bool messageMode = false;
        int messageModeCounter = 0;

        public Nursery nurseryOne, nurseryTwo;

        public FatBossZombie fatBossZombie;

        public Song song;
        public Game1 mainGame;

        public Level6(Game1 myMainGame)
        {
            mainGame = myMainGame;
        }


        public void Initialize()
        {
            gameOverPosition.X = 320;
            gameOverPosition.Y = 130;

            device = graphics.GraphicsDevice;

            gameState = new GameState();
            gameStart = new GameStart(device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight);

            board = new GameBoard(new Vector2(33, 25), new Vector2(device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight));
            darwin = new Darwin(board);

            zTime = new ZombieTime(board);

            nurseryOne = new Nursery(board, darwin);
            nurseryTwo = new Nursery(board, darwin);

            fatBossZombie = new FatBossZombie(15, 5, 32, 0, 24, 0, darwin, board);

            setLevelState();
        }


        public void LoadContent()
        {
            messageFont = mainGame.Content.Load<SpriteFont>("TimesNewRoman");

            Texture2D darwinTex = mainGame.Content.Load<Texture2D>("DarwinPic/Darwin");
            Texture2D darwinUpTex = mainGame.Content.Load<Texture2D>("DarwinPic/DarwinUp");
            Texture2D darwinDownTex = mainGame.Content.Load<Texture2D>("DarwinPic/Darwin");
            Texture2D darwinRightTex = mainGame.Content.Load<Texture2D>("DarwinPic/DarwinRight");
            Texture2D darwinLeftTex = mainGame.Content.Load<Texture2D>("DarwinPic/DarwinLeft");
            Texture2D zombieDarwinTex = mainGame.Content.Load<Texture2D>("DarwinPic/ZombieDarwin");
            darwin.LoadContent(graphics.GraphicsDevice, darwinUpTex, darwinDownTex, darwinRightTex, darwinLeftTex, zombieDarwinTex);

            gameOverTexture = mainGame.Content.Load<Texture2D>("gameover");
            gameWinTexture = mainGame.Content.Load<Texture2D>("gamewin");

            board.LoadContent(mainGame.Content.Load<Texture2D>("StaticPic/metal_tile"));
            board.LoadBackgroundContent(mainGame.Content.Load<Texture2D>("StaticPic/side_wall"));

            gameStart.LoadContent(mainGame.Content.Load<Texture2D>("startScreen"));
            zTime.LoadContent(mainGame.Content.Load<Texture2D>("humanities_bar"));

            nurseryOne.LoadContent(mainGame.Content.Load<Texture2D>("StaticPic/nursery"),
                mainGame.Content.Load<Texture2D>("ZombiePic/BabyZombie"),
                mainGame.Content.Load<Texture2D>("explosion_sequence"));
            
            nurseryTwo.LoadContent(mainGame.Content.Load<Texture2D>("StaticPic/nursery"),
                mainGame.Content.Load<Texture2D>("ZombiePic/BabyZombie"),
                mainGame.Content.Load<Texture2D>("explosion_sequence"));

            fatBossZombie.LoadContent(mainGame.Content.Load<Texture2D>("ZombiePic/FatBossZombie"));
        }

        /*
         * Start of the level's state.
         */
        public void setLevelState()
        {
            darwin.setHuman();
            board.setGridPositionOpen(darwin);
            darwin.setGridPosition(15, 22);
            board.setGridPositionOccupied(darwin);

            nurseryOne.reset();
            nurseryOne.setGridPosition(1, 1);
            nurseryOne.setSpawnPoint(2, 4);
            board.setGridPositionOccupied(1, 1);
            board.setGridPositionOccupied(1, 2);
            board.setGridPositionOccupied(1, 3);
            board.setGridPositionOccupied(2, 1);
            board.setGridPositionOccupied(2, 2);
            board.setGridPositionOccupied(2, 3);

            nurseryTwo.reset();
            nurseryTwo.setGridPosition(30, 20);
            nurseryTwo.setSpawnPoint(30, 19);
            board.setGridPositionOccupied(30, 20);
            board.setGridPositionOccupied(30, 21);
            board.setGridPositionOccupied(30, 22);
            board.setGridPositionOccupied(31, 20);
            board.setGridPositionOccupied(31, 21);
            board.setGridPositionOccupied(31, 22);

            fatBossZombie.reset();
            fatBossZombie.setGridPosition(15, 5);
            board.setGridPositionOccupied(15, 5);
            board.setGridPositionOccupied(16, 5);
            board.setGridPositionOccupied(17, 5);
            board.setGridPositionOccupied(15, 6);
            board.setGridPositionOccupied(16, 6);
            board.setGridPositionOccupied(17, 6);
            board.setGridPositionOccupied(15, 7);
            board.setGridPositionOccupied(16, 7);
            board.setGridPositionOccupied(17, 7);

            zTime.reset();

            gameOver = false;
            gameWin = false;
            gameState.setState(GameState.state.Level);
            
        }


        public void Update(GameTime gameTime)
        {
            switch (gameState.getState())
            {
                case GameState.state.Start:
                    UpdateStartState();
                    break;
                case GameState.state.Level:
                    if (!messageMode)
                        UpdateLevelState(gameTime);
                    else
                        UpdateMessageMode();
                    break;
                case GameState.state.End:
                    UpdateEndState();
                    break;
            }

        }

        private void UpdateStartState()
        {
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Enter))
            {
                //MediaPlayer.Play(song);
                gameState.setState(GameState.state.Level);
            }
        }

        private void UpdateLevelState(GameTime gameTime)
        {
            if (darwin.isZombie())
            {
                if (zTime.isTimedOut())
                {
                    gameOver = true;
                }
                else
                {
                    zTime.Update(gameTime);
                }
            }

            checkForGameOver();

            KeyboardState ks = Keyboard.GetState();

            checkForExitGame(ks);

            darwin.Update(gameTime, ks, board, darwin.X, darwin.Y);

            if (gameOver || gameWin)
            {
                gameState.setState(GameState.state.End);
            }

            if (ks.IsKeyDown(Keys.H) && messageModeCounter > 10)
            {
                messageMode = true;
                messageModeCounter = 0;
            }
            messageModeCounter++;

            nurseryOne.Update(gameTime);
            nurseryTwo.Update(gameTime);

            fatBossZombie.Update(gameTime);
        }

        private void UpdateEndState()
        {
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Q))
            {
                mainGame.Exit();
            }
            if (ks.IsKeyDown(Keys.R))
            {
                setLevelState();
                //MediaPlayer.Stop();
                //MediaPlayer.Play(song);
            }

        }

        private void UpdateMessageMode()
        {
            KeyboardState ks = Keyboard.GetState();
            messageModeCounter++;
            if (ks.IsKeyDown(Keys.H) && messageModeCounter > 10)
            {
                messageMode = false;
                messageModeCounter = 0;
            }
        }

        private void checkForExitGame(KeyboardState ks)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                mainGame.Exit();

            if (ks.IsKeyDown(Keys.Q))
            {
                mainGame.Exit();
            }
        }

        private void checkForGameOver()
        {
            foreach (BabyZombie b in nurseryOne.babies)
            {
                if (b.exploding)
                {
                    if (darwin.isOnTop(b)
                        || darwin.isOnTop(b.X, b.Y - 1)
                        || darwin.isOnTop(b.X - 1, b.Y)
                        || darwin.isOnTop(b.X, b.Y + 1)
                        || darwin.isOnTop(b.X + 1, b.Y))
                    {
                        gameOver = true;
                    }
                }
            }

            foreach (BabyZombie b in nurseryTwo.babies)
            {
                if (b.exploding)
                {
                    if (darwin.isOnTop(b)
                        || darwin.isOnTop(b.X, b.Y - 1)
                        || darwin.isOnTop(b.X - 1, b.Y)
                        || darwin.isOnTop(b.X, b.Y + 1)
                        || darwin.isOnTop(b.X + 1, b.Y))
                    {
                        gameOver = true;
                    }
                }
            }
            
        }

        public void setZTime(ZombieTime mytime)
        {
            zTime = mytime;

            zTimeReset = new ZombieTime(board);
            zTimeReset.reset();
            zTimeReset.setTime(mytime.getTime());
        }

        public void setGameOver(bool game)
        {
            gameOver = game;
        }

        public void Draw(GameTime gameTime)
        {
            switch (gameState.getState())
            {
                case GameState.state.Start:
                    DrawStartState();
                    break;
                case GameState.state.Level:
                    DrawLevelState(gameTime);
                    break;
                case GameState.state.End:
                    DrawEndState();
                    break;
            }

        }

        private void DrawStartState()
        {
            mainGame.GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            gameStart.Draw(spriteBatch);
            spriteBatch.End();
        }

        private void DrawLevelState(GameTime gameTime)
        {
            mainGame.GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            board.Draw(spriteBatch);
            darwin.Draw(spriteBatch);
            zTime.Draw(spriteBatch);

            nurseryOne.Draw(spriteBatch);
            nurseryTwo.Draw(spriteBatch);

            fatBossZombie.Draw(spriteBatch);

            if (messageMode)
            {
            }

            spriteBatch.End();
        }

        private void DrawEndState()
        {
            spriteBatch.Begin();
            if (gameOver)
            {
                gameOverPosition.X = 320;
                gameOverPosition.Y = 130;
                spriteBatch.Draw(gameOverTexture, gameOverPosition, Color.White);
            }

            if (gameWin)
            {
                spriteBatch.Draw(gameWinTexture, gameOverPosition, Color.White);
            }
            spriteBatch.End();
        }
    }
}
