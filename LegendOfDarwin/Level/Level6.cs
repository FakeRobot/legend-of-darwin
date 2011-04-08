using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        private int gameOverCounter = 0;

        private SoundEffect deathScreamSound;
        private bool playDeathSound = true;
        

        public Texture2D gameOverTexture;
        public Texture2D gameWinTexture;

        Vector2 gameOverPosition = Vector2.Zero;

        bool messageMode = false;
        int messageModeCounter = 0;

        public Nursery nurseryOne, nurseryTwo;

        public FatBossZombie fatBossZombie;

        //public Song song;
        public Game1 mainGame;

        private BasicObject[] walls;
        private Texture2D wallTex;

        private Stairs stairs;

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

            fatBossZombie = new FatBossZombie(15, 4, 19, 14, 4, 3, darwin, board);
            fatBossZombie.resetGapeMode();
            stairs = new Stairs(board);

            walls = setWallsInLevelSix();

            setLevelState();
            gameState.setState(GameState.state.Start);
        }


        public void LoadContent()
        {
            messageFont = mainGame.Content.Load<SpriteFont>("TimesNewRoman");

            deathScreamSound = mainGame.Content.Load<SoundEffect>("chewScream");
            SoundEffect transformSound = mainGame.Content.Load<SoundEffect>("transform");

            Texture2D darwinTex = mainGame.Content.Load<Texture2D>("DarwinPic/Darwin");
            Texture2D darwinUpTex = mainGame.Content.Load<Texture2D>("DarwinPic/DarwinUp");
            Texture2D darwinDownTex = mainGame.Content.Load<Texture2D>("DarwinPic/Darwin");
            Texture2D darwinRightTex = mainGame.Content.Load<Texture2D>("DarwinPic/DarwinRight");
            Texture2D darwinLeftTex = mainGame.Content.Load<Texture2D>("DarwinPic/DarwinLeft");
            Texture2D zombieDarwinTex = mainGame.Content.Load<Texture2D>("DarwinPic/ZombieDarwin");
            Texture2D deadDarwinTex = mainGame.Content.Load<Texture2D>("DarwinPic/DeadDarwin");
            darwin.LoadContent(graphics.GraphicsDevice, darwinUpTex, darwinDownTex,
                darwinRightTex, darwinLeftTex, zombieDarwinTex, deadDarwinTex, transformSound);

            gameOverTexture = mainGame.Content.Load<Texture2D>("gameover");
            gameWinTexture = mainGame.Content.Load<Texture2D>("gamewin");
            Texture2D menuBarTexture = mainGame.Content.Load<Texture2D>("menubar");

            gameStart.LoadContent(mainGame.Content.Load<Texture2D>("SplashScreens/Level6"));

            board.LoadContent(mainGame.Content.Load<Texture2D>("StaticPic/metal_tile"));
            board.LoadBackgroundContent(mainGame.Content.Load<Texture2D>("StaticPic/side_wall"));
            board.loadMenuBar(menuBarTexture);

            wallTex = mainGame.Content.Load<Texture2D>("StaticPic/side_wall");
            zTime.LoadContent(mainGame.Content.Load<Texture2D>("humanities_bar"));

            SoundEffect babySound = mainGame.Content.Load<SoundEffect>("baby");
            SoundEffect explosionSound = mainGame.Content.Load<SoundEffect>("explosion");
            nurseryOne.LoadContent(mainGame.Content.Load<Texture2D>("StaticPic/nursery-cribs"),
                mainGame.Content.Load<Texture2D>("ZombiePic/BabyZombie"),
                mainGame.Content.Load<Texture2D>("explosion_sequence"),
                babySound, explosionSound);
            
            nurseryTwo.LoadContent(mainGame.Content.Load<Texture2D>("StaticPic/nursery-cribs"),
                mainGame.Content.Load<Texture2D>("ZombiePic/BabyZombie"),
                mainGame.Content.Load<Texture2D>("explosion_sequence"),
                babySound, explosionSound);

            fatBossZombie.LoadContent(mainGame.Content.Load<Texture2D>("ZombiePic/King"),
                mainGame.Content.Load<Texture2D>("explosion_sequence"), explosionSound, mainGame.Content.Load<SoundEffect>("zombie_groan"));

            stairs.LoadContent(mainGame.Content.Load<Texture2D>("StaticPic/stairsUp"),
                mainGame.Content.Load<Texture2D>("StaticPic/stairsDown"), "Down");
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
            fatBossZombie.setGridPosition(15, 4);
            board.setGridPositionOccupied(15, 4);
            board.setGridPositionOccupied(16, 4);
            board.setGridPositionOccupied(17, 4);
            board.setGridPositionOccupied(15, 5);
            board.setGridPositionOccupied(16, 5);
            board.setGridPositionOccupied(17, 5);
            board.setGridPositionOccupied(15, 6);
            board.setGridPositionOccupied(16, 6);
            board.setGridPositionOccupied(17, 6);

            zTime.reset();

            darwin.setDarwinAlive();
            darwin.setHuman();
            playDeathSound = true;
            gameOverCounter = 0;
            stairs.setGridPosition(14, 1);

            gameOver = false;
            gameWin = false;

            Keyboard.GetState();
            
        }

        private BasicObject[] setWallsInLevelSix()
        {
            BasicObject w0 = new BasicObject(board);
            BasicObject w1 = new BasicObject(board);
            BasicObject w2 = new BasicObject(board);
            BasicObject w3 = new BasicObject(board);
            BasicObject w4 = new BasicObject(board);
            BasicObject w5 = new BasicObject(board);
            BasicObject w6 = new BasicObject(board);
            BasicObject w7 = new BasicObject(board);
            BasicObject w8 = new BasicObject(board);
            BasicObject w9 = new BasicObject(board);
            BasicObject w10 = new BasicObject(board);
            BasicObject w11 = new BasicObject(board);

            w0.setGridPosition(13, 1);
            w1.setGridPosition(13, 2);
            w2.setGridPosition(13, 3);
            w3.setGridPosition(13, 4);
            w4.setGridPosition(13, 5);
            w5.setGridPosition(13, 6);
            w6.setGridPosition(20, 1);
            w7.setGridPosition(20, 2);
            w8.setGridPosition(20, 3);
            w9.setGridPosition(20, 4);
            w10.setGridPosition(20, 5);
            w11.setGridPosition(20, 6);

            for (int m = 1; m < 7; m++)
            {
                board.setGridPositionOccupied(13, m);
            }
            for (int m = 1; m < 7; m++)
            {
                board.setGridPositionOccupied(20, m);
            }

            walls = new BasicObject[12] { w0, w1, w2, w3, w4, w5, w6, w7, w8, w9, w10, w11 };
            return walls;
        }

        public void Update(GameTime gameTime)
        {
            switch (gameState.getState())
            {
                case GameState.state.Start:
                    UpdateStartState();
                    break;
                case GameState.state.Level:
                    if (!messageMode && !gameOver)
                        UpdateLevelState(gameTime);
                    else if (messageMode)
                        UpdateMessageMode();
                    else
                    {
                        if (playDeathSound)
                        {
                            deathScreamSound.Play();
                            playDeathSound = false;
                        }

                        darwin.setDarwinDead();
                        darwin.setZombie();
                        UpdateLevelState(gameTime);
                        gameOverCounter++;
                        if (gameOverCounter > 200)
                        {
                            gameState.setState(GameState.state.End);
                            gameOverCounter = 0;
                        }
                    }
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
            checkLevelOver();
            if (darwin.isZombie() && darwin.isDarwinAlive())
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

            KeyboardState ks = Keyboard.GetState();

            if (darwin.isDarwinAlive()){
                checkForExitGame(ks);

                checkForGameOver();
            
                if (fatBossZombie.isZombieAlive())
                    checkForGameOverWithBoss();

                darwin.Update(gameTime, ks, board, darwin.X, darwin.Y);
            }

            if (ks.IsKeyDown(Keys.H) && messageModeCounter > 10)
            {
                messageMode = true;
                messageModeCounter = 0;
            }
            messageModeCounter++;

            if (fatBossZombie.isZombieAlive())
                fatBossZombie.checkForBabyDeaths(nurseryOne, nurseryTwo);

            nurseryOne.Update(gameTime);
            nurseryTwo.Update(gameTime);

            if (fatBossZombie.isZombieAlive())
                fatBossZombie.Update(gameTime);

            updateFirstBabyWaveExplosion();
            updateSecondBabyWaveExplosion();
        }

        private void updateSecondBabyWaveExplosion()
        {
            if (fatBossZombie.explodeSecondWaveOfBabies)
            {
                foreach (BabyZombie baby in nurseryOne.babies)
                {
                    if (baby.Y == fatBossZombie.Y + 4)
                    {
                        if (baby.X == fatBossZombie.X || baby.X == fatBossZombie.X + 1 || baby.X == fatBossZombie.X + 2 || baby.X == fatBossZombie.X + 3 || baby.X == fatBossZombie.X - 1)
                        {
                            baby.exploding = true;
                        }
                    }
                }
                foreach (BabyZombie baby in nurseryTwo.babies)
                {
                    if (baby.Y == fatBossZombie.Y + 4)
                    {
                        if (baby.X == fatBossZombie.X || baby.X == fatBossZombie.X + 1 || baby.X == fatBossZombie.X + 2 || baby.X == fatBossZombie.X + 3 || baby.X == fatBossZombie.X - 1)
                        {
                            baby.exploding = true;
                        }
                    }
                }
                fatBossZombie.explodeSecondWaveOfBabies = false;
            }
        }

        private void updateFirstBabyWaveExplosion()
        {
            if (fatBossZombie.explodeFirstWaveOfBabies)
            {
                foreach (BabyZombie baby in nurseryOne.babies)
                {
                    if (baby.Y == fatBossZombie.Y + 3)
                    {
                        if (baby.X == fatBossZombie.X || baby.X == fatBossZombie.X + 1 || baby.X == fatBossZombie.X + 2)
                        {
                            //add babies beside and behind him
                            baby.exploding = true;
                        }
                    }
                }
                foreach (BabyZombie baby in nurseryTwo.babies)
                {
                    if (baby.Y == fatBossZombie.Y + 3)
                    {
                        if (baby.X == fatBossZombie.X || baby.X == fatBossZombie.X + 1 || baby.X == fatBossZombie.X + 2)
                        {
                            baby.exploding = true;
                        }
                    }
                }
                fatBossZombie.explodeFirstWaveOfBabies = false;
            }
        }

        private void UpdateEndState()
        {
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Q))
            {
                setLevelState();
                mainGame.setCurLevel(Game1.LevelState.Start);
                gameState.setState(GameState.state.Start);
                //mainGame.Exit();
            }
            if (ks.IsKeyDown(Keys.R))
            {
                setLevelState();
                mainGame.DEATH_COUNTER++;
                gameState.setState(GameState.state.Level);
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

            if (ks.IsKeyDown(Keys.Escape))
            {
                mainGame.Exit();
            }
        }

        private void checkForGameOver()
        {
            foreach (BabyZombie b in nurseryOne.babies)
            {
                if (b.exploding && b.isZombieAlive())
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
                if (b.exploding && b.isZombieAlive())
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

        private void checkForGameOverWithBoss()
        {
            if (fatBossZombie.canDarwinBeEaten() && fatBossZombie.isZombieAlive())
            {
                gameOver = true;
                fatBossZombie.eatingDarwin = true;
            }
            else if (fatBossZombie.isInCollision(darwin) && fatBossZombie.isZombieAlive())
            {
                gameOver = true;
                fatBossZombie.eatingDarwin = true;
            }

            else if (darwin.collision && fatBossZombie.isZombieAlive())
            {
                Rectangle rightSideOfDarwin = darwin.destination;
                rightSideOfDarwin.X = rightSideOfDarwin.X + board.getSquareWidth();

                Rectangle leftSideOfDarwin = darwin.destination;
                leftSideOfDarwin.X = leftSideOfDarwin.X - board.getSquareWidth();

                Rectangle onTopOfDarwin = darwin.destination;
                onTopOfDarwin.Y = onTopOfDarwin.Y - board.getSquareLength();

                Rectangle onBottomOfDarwin = darwin.destination;
                onBottomOfDarwin.Y = onBottomOfDarwin.Y + board.getSquareLength();

                if (rightSideOfDarwin == fatBossZombie.destination ||
                    leftSideOfDarwin == fatBossZombie.destination ||
                    onTopOfDarwin == fatBossZombie.destination ||
                    onBottomOfDarwin == fatBossZombie.destination)
                {
                    gameOver = true;
                    fatBossZombie.eatingDarwin = true;
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

            stairs.Draw(spriteBatch);

            if(!fatBossZombie.eatingDarwin)
            {
                darwin.Draw(spriteBatch);
            }
            
            zTime.Draw(spriteBatch);

            nurseryOne.Draw(spriteBatch);
            nurseryTwo.Draw(spriteBatch);

            if (fatBossZombie.isZombieAlive())
                fatBossZombie.Draw(spriteBatch);

            foreach (BasicObject a in walls)
            {
                spriteBatch.Draw(wallTex, board.getPosition(a.X, a.Y), Color.White);
            }

            if (messageMode)
            {
            }

            spriteBatch.DrawString(messageFont, "HUMANITY: ",
                new Vector2(board.getPosition(zTime.X, zTime.Y).X + board.getSquareWidth() * 2, board.getPosition(zTime.X, 24).Y),
                Color.Black);

            spriteBatch.DrawString(messageFont, "DEATH COUNT: " + mainGame.DEATH_COUNTER.ToString(),
                new Vector2(board.getPosition(15, 24).X, board.getPosition(15, 24).Y),
                Color.Black);

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

        private void checkLevelOver()
        {
            if (stairs.isOnTop(darwin))
            {
                mainGame.setCurLevel(Game1.LevelState.End);
            }
        }
    }
}
