using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using LegendOfDarwin.GameObject;
using LegendOfDarwin.MenuObject;

namespace LegendOfDarwin.Level
{
    public class Level4
    {

        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public GraphicsDevice device;

        private GameState gameState;
        private GameStart gameStart;

        private Darwin darwin;
        private Zombie firstZombie, secondZombie, thirdZombie;
        private Switch firstSwitch;
        private GameBoard board;

        private ZombieTime zTime;
        private ZombieTime zTimeReset; //what zTime should reset to

        private Potion potion;
        private Stairs stairs;

        private BasicObject[] walls;
        private Texture2D wallTex;

        public SpriteFont messageFont;
        public bool keyIsHeldDown = false;
        public bool gameOver = false;
        public bool gameWin = false;

        private int counter;
        private int counterReady;
        public Texture2D gameOverTexture;
        public Texture2D gameWinTexture;

        Vector2 gameOverPosition = Vector2.Zero;


        // things for managing message boxes
        bool messageMode = false;
        int messageModeCounter = 0;
        private MessageBox zombieMessage;
        private MessageBox darwinMessage;
        private MessageBox switchMessage;

        public Song song;
        public Game1 mainGame;

        public Level4(Game1 myMainGame)
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
            gameState.setState(GameState.state.Level);

            board = new GameBoard(new Vector2(33, 25), new Vector2(device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight));
            darwin = new Darwin(board);
            //firstZombie = new Zombie(10, 10, 15, 5, 15, 5, board);
            //secondZombie = new Zombie(10, 16, 15, 5, 15, 5, board);
            //thirdZombie = new Zombie(12, 10, 15, 5, 15, 5, board);

            String zombieString = "This a zombie,\n don't near him \nas a human!!";
            zombieMessage = new MessageBox(board.getPosition(12, 8).X, board.getPosition(10, 10).Y, zombieString);

            String darwinString = "This is darwin,\n move with arrows, \n z to transform, \n a for actions";
            darwinMessage = new MessageBox(board.getPosition(12, 8).X, board.getPosition(10, 10).Y, darwinString);

            String switchString = "This is a switch\n face it and press A\n to see what happens!!";
            switchMessage = new MessageBox(board.getPosition(12, 8).X, board.getPosition(10, 10).Y, switchString);

            stairs = new Stairs(board);

            BasicObject[] removableWalls = setRemovableWallsInLevelOne();

            BasicObject switchSquare = new BasicObject(board);
            switchSquare.X = 5;
            switchSquare.Y = 10;

            firstSwitch = new Switch(switchSquare, board, removableWalls);

            // Initial starting position
            darwin.setGridPosition(2, 20);

            if (board.isGridPositionOpen(darwin))
            {
                board.setGridPositionOccupied(darwin.X, darwin.Y);
                darwin.setPosition(board.getPosition(darwin).X, board.getPosition(darwin).Y);
            }

            // Darwin's lag movement
            counterReady = counter = 5;

            if (board.isGridPositionOpen(13, 2))
            {
                stairs.setGridPosition(13, 2);
                stairs.setDestination(board.getPosition(13, 2));
            }

            zTime = new ZombieTime(board);
            zTimeReset = new ZombieTime(board);

            setPotionPosition(25, 4);

            setWalls();
        }

        private BasicObject[] setRemovableWallsInLevelOne()
        {

            BasicObject s1 = new BasicObject(board);
            s1.X = 28;
            s1.Y = 19;

            BasicObject[] removableWalls = new BasicObject[1] {s1};
            return removableWalls;
        }

        private void setPotionPosition(int x, int y)
        {
            potion = new Potion(board);
            potion.setDestination(board.getPosition(x, y));
            potion.setGridPosition(x, y);
            board.setGridPositionOccupied(x, y);
        }

        private void setWalls()
        {
            BasicObject w1 = new BasicObject(board);
            w1.setGridPosition(11, 1);
            board.setGridPositionOccupied(11, 1);
            walls = new BasicObject[1] { w1 };
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

            stairs.LoadContent(mainGame.Content.Load<Texture2D>("StaticPic/stairsUp"),
                mainGame.Content.Load<Texture2D>("StaticPic/stairsDown"), "Down");

            firstSwitch.LoadContent(mainGame.Content.Load<Texture2D>("StaticPic/Wall"),
                mainGame.Content.Load<Texture2D>("StaticPic/Switch"));

            board.LoadContent(mainGame.Content.Load<Texture2D>("StaticPic/Level4/metal_tile_medium"));
            board.LoadBackgroundContent(mainGame.Content.Load<Texture2D>("StaticPic/Level4/side_wall_purple"));

            wallTex = mainGame.Content.Load<Texture2D>("StaticPic/Level4/side_wall_purple");

            //darwin.LoadContent(graphics.GraphicsDevice, darwinTex, zombieDarwinTex);

            //firstZombie.LoadContent(mainGame.Content.Load<Texture2D>("ZombiePic/Zombie"));
            //secondZombie.LoadContent(mainGame.Content.Load<Texture2D>("ZombiePic/Zombie"));
            //thirdZombie.LoadContent(mainGame.Content.Load<Texture2D>("ZombiePic/Zombie"));
        
            zombieMessage.LoadContent(mainGame.Content.Load<Texture2D>("messageBox"));
            darwinMessage.LoadContent(mainGame.Content.Load<Texture2D>("messageBox"));
            switchMessage.LoadContent(mainGame.Content.Load<Texture2D>("messageBox"));

            gameStart.LoadContent(mainGame.Content.Load<Texture2D>("startScreen"));
            zTime.LoadContent(mainGame.Content.Load<Texture2D>("humanities_bar"));
            potion.LoadContent(mainGame.Content.Load<Texture2D>("StaticPic/potion"));
        }
            

        //protected override void UnloadContent() { }

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

        private void UpdateMessageMode()
        {
            KeyboardState ks = Keyboard.GetState();
            messageModeCounter++;

            //zombieMessage.pointToSquare(firstZombie.X, firstZombie.Y, board);
            darwinMessage.pointToSquare(darwin.X, darwin.Y, board);
            switchMessage.pointToSquare(firstSwitch.X, firstSwitch.Y, board);

            if (ks.IsKeyDown(Keys.H) && messageModeCounter > 10)
            {
                messageMode = false;
                messageModeCounter = 0;
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

            KeyboardState ks = Keyboard.GetState();

            checkForExitGame(ks);

            updateKeyHeldDown(ks);

            /*
            if (keyIsHeldDown)
            {
                if (counter > counterReady)
                {
                    darwin.Update(gameTime, ks, board, darwin.X, darwin.Y);
                    counter = 0;
                }
                else
                {
                    counter++;
                }
            }
            else
            {
                darwin.Update(gameTime, ks, board, darwin.X, darwin.Y);
            }*/

            darwin.Update(gameTime, ks, board, darwin.X, darwin.Y);

            stairs.Update(gameTime, darwin);

            //firstZombie.Update(gameTime, darwin);
            //secondZombie.Update(gameTime, darwin);
            //thirdZombie.Update(gameTime, darwin);

            firstSwitch.Update(gameTime, ks, darwin);

            potion.Update(gameTime, ks, darwin, zTime);

            if (!darwin.isZombie())
            {
                //checkForGameOver(firstZombie);
                //checkForGameOver(secondZombie);
                //checkForGameOver(thirdZombie);
            }

            //checkForGameWin();
            checkForSwitchToLevelFive();

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
                
                
                if (gameWin)
                {
                    zTime.reset();
                    mainGame.setCurLevel(Game1.LevelState.Level1);
                }
                else if (gameOver)
                {
                    zTime = new ZombieTime(board);
                    zTime.reset();
                    zTime.LoadContent(mainGame.Content.Load<Texture2D>("humanities_bar"));
                    zTime.setTime(zTimeReset.getTime());
                }

                board.setGridPositionOpen(darwin);
                darwin.setGridPosition(2, 20);

                gameOver = false;
                gameWin = false;

                //board.setGridPositionOpen(firstZombie);
                //board.setGridPositionOpen(secondZombie);
                //board.setGridPositionOpen(thirdZombie);
                board.setGridPositionOpen(potion);

                //firstZombie.setGridPosition(10, 10);
                //board.setGridPositionOccupied(firstZombie.X, firstZombie.Y);
                //firstZombie.setZombieAlive(true);

                //secondZombie.setGridPosition(10, 16);
                //board.setGridPositionOccupied(secondZombie.X, secondZombie.Y);
                //secondZombie.setZombieAlive(true);

                //thirdZombie.setGridPosition(12, 10);
                //board.setGridPositionOccupied(thirdZombie.X, thirdZombie.Y);
                //thirdZombie.setZombieAlive(true);

                potion.setGridPosition(25, 4);
                board.setGridPositionOccupied(potion.X, potion.Y);

                potion.reset();
                darwin.setHuman();
                gameState.setState(GameState.state.Level);
                MediaPlayer.Stop();
                MediaPlayer.Play(song);
                
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

        private void checkForGameOver(Zombie myZombie)
        {
            if (darwin.isOnTop(myZombie))
            {
                gameOver = true;
            }

            if (darwin.collision)
            {
                Rectangle rightSideOfDarwin = darwin.destination;
                rightSideOfDarwin.X = rightSideOfDarwin.X + board.getSquareWidth();

                Rectangle leftSideOfDarwin = darwin.destination;
                leftSideOfDarwin.X = leftSideOfDarwin.X - board.getSquareWidth();

                Rectangle onTopOfDarwin = darwin.destination;
                onTopOfDarwin.Y = onTopOfDarwin.Y - board.getSquareLength();

                Rectangle onBottomOfDarwin = darwin.destination;
                onBottomOfDarwin.Y = onBottomOfDarwin.Y + board.getSquareLength();


                if (rightSideOfDarwin == myZombie.destination ||
                    leftSideOfDarwin == myZombie.destination ||
                    onTopOfDarwin == myZombie.destination ||
                    onBottomOfDarwin == myZombie.destination)
                {
                    gameOver = true;
                }
            }
        }

        private void checkForGameWin()
        {
            if (darwin.isOnTop(stairs))
            {
                gameWin = true;
            }
        }

        private void checkForSwitchToLevelFive()
        {
            if (darwin.isOnTop(stairs)) 
            {
                mainGame.setCurLevel(Game1.LevelState.Level5);
                mainGame.setZTimeLevel(zTime,Game1.LevelState.Level5);

                darwin.setHuman();
                gameState.setState(GameState.state.Level);
                gameOver = false;
                gameWin = false;
            }
        }

        public void setZTime(ZombieTime mytime)
        {
            zTime = mytime;

            zTimeReset = new ZombieTime(board);
            zTimeReset.reset();
            zTimeReset.setTime(mytime.getTime());
        }

        private void updateKeyHeldDown(KeyboardState ks)
        {
            if (ks.IsKeyUp(Keys.Right) && ks.IsKeyUp(Keys.Left) && ks.IsKeyUp(Keys.Up) && ks.IsKeyUp(Keys.Down))
            {
                keyIsHeldDown = false;
            }
            else
            {
                keyIsHeldDown = true;
            }
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

            darwin.Draw(spriteBatch);
            //firstZombie.Draw(spriteBatch);
            //secondZombie.Draw(spriteBatch);
            //thirdZombie.Draw(spriteBatch);

            firstSwitch.Draw(spriteBatch);
            zTime.Draw(spriteBatch);
            potion.Draw(spriteBatch);

            if (messageMode)
            {
                zombieMessage.Draw(spriteBatch, messageFont);
                darwinMessage.Draw(spriteBatch, messageFont);
                //switchMessage.Draw(spriteBatch, messageFont);
            }

            foreach(BasicObject a in walls)
            {
                spriteBatch.Draw(wallTex, board.getPosition(a.X, a.Y), Color.White);
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




