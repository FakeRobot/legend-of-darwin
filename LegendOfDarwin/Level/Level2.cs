﻿using System;
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


namespace LegendOfDarwin
{
    public class Level2
    {

        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        private GameState gameState;
        private GameStart gameStart;

        private Darwin darwin;
        private Zombie firstZombie, secondZombie, thirdZombie;
        private CannibalZombie cannibalZombie;
        private Switch firstSwitch;
        private Brain brain;
        private GameBoard board;
        public GraphicsDevice device;
        public SpriteFont messageFont;
        public bool keyIsHeldDown = false;
        public bool gameOver = false;
        public bool gameWin = false;

        private int counter;

        private int counterReady;
        public Texture2D gameOverTexture;
        public Texture2D gameWinTexture;

        Vector2 gameOverPosition = Vector2.Zero;
        private Stairs stairs;

        // things for managing message boxes
        bool messageMode = false;
        int messageModeCounter = 0;
        private MessageBox zombieMessage;
        private MessageBox darwinMessage;
        private MessageBox switchMessage;
        private MessageBox brainMessage;

        private ZombieTime zTime;
        private ZombieTime zTimeReset; //what zTime should reset to

        private BasicObject[] walls;
        private Texture2D wallTex;

        public Song song;
        public Game1 mainGame;

        public Level2(Game1 myMainGame)
        {
            mainGame = myMainGame;
        }

        public void Initialize()
        {
            gameOverPosition.X = 320;
            gameOverPosition.Y = 130;

            device = graphics.GraphicsDevice;

            gameState = new GameState();
            gameState.setState(GameState.state.Level);
            gameStart = new GameStart(device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight);

            board = new GameBoard(new Vector2(25, 25), new Vector2(device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight));
            darwin = new Darwin(board);

            firstZombie = new Zombie(10, 9, 15, 5, 15, 5, board);
            //secondZombie = new Zombie(10, 16, 15, 5, 15, 5, board);
            //thirdZombie = new Zombie(16, 10, 15, 5, 15, 5, board);

            List<Zombie> myZombieList= new List<Zombie>();
            myZombieList.Add(firstZombie);
            cannibalZombie = new CannibalZombie(20,5,board.getNumSquaresX()-1,1,board.getNumSquaresY()-1,1,myZombieList,darwin,board);

            String zombieString = "This a zombie,\n don't near him \nas a human!!";
            zombieMessage = new MessageBox(board.getPosition(12, 8).X, board.getPosition(10, 10).Y, zombieString);

            String darwinString = "This is darwin,\n move with arrows, \n z to transform, \n a for actions";
            darwinMessage = new MessageBox(board.getPosition(12, 8).X, board.getPosition(10, 10).Y, darwinString);

            String switchString = "This is a switch\n face it and press A\n to see what happens!!";
            switchMessage = new MessageBox(board.getPosition(12, 8).X, board.getPosition(10, 10).Y, switchString);

            String brainString = "Move the brain as a \nzombie.\n Zombie's like brains!!";
            brainMessage = new MessageBox(board.getPosition(12, 8).X, board.getPosition(10, 10).Y, brainString);

            stairs = new Stairs(board);

            brain = new Brain(board, 10, 18);

            BasicObject[] squares = setRemovableWallsInLevelTwo();

            BasicObject switchSquare = new BasicObject(board);
            switchSquare.X = 10;
            switchSquare.Y = 3;

            firstSwitch = new Switch(switchSquare, board, squares);

            // Initial starting position
            darwin.setGridPosition(5, 5);

            if (board.isGridPositionOpen(darwin))
            {
                board.setGridPositionOccupied(darwin.X, darwin.Y);
                darwin.setPosition(board.getPosition(darwin).X, board.getPosition(darwin).Y);
            }

            // Darwin's lag movement
            counterReady = counter = 5;


            if (board.isGridPositionOpen(21, 20))
            {
                stairs.setGridPosition(21, 20);
                stairs.setDestination(board.getPosition(21, 20));
            }

            zTime = new ZombieTime(board);
            zTimeReset = new ZombieTime(board);

            setWallsInLevelTwo();
        }

        private BasicObject[] setRemovableWallsInLevelTwo()
        {
            //later add an x and y to the constructor
            BasicObject s1 = new BasicObject(board);
            s1.X = 20;
            s1.Y = 19;

            BasicObject s2 = new BasicObject(board);
            s2.X = 20;
            s2.Y = 20;

            BasicObject s3 = new BasicObject(board);
            s3.X = 20;
            s3.Y = 21;

            BasicObject s4 = new BasicObject(board);
            s4.X = 20;
            s4.Y = 22;


            BasicObject[] squares = new BasicObject[4] { s1, s2, s3, s4};
            return squares;
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

            Texture2D zombieTex = mainGame.Content.Load<Texture2D>("ZombiePic/Zombie");
            Texture2D cannibalTex = mainGame.Content.Load<Texture2D>("ZombiePic/CannibalZombie");
            Texture2D messagePic = mainGame.Content.Load<Texture2D>("messageBox");

            Texture2D basicGridTex = mainGame.Content.Load<Texture2D>("StaticPic/Level2/metal_tile_dark");
            Texture2D basicMenuTex = mainGame.Content.Load<Texture2D>("StaticPic/Level2/side_wall_green");

            Texture2D basicStairUpTex = mainGame.Content.Load<Texture2D>("StaticPic/stairsUp");
            Texture2D basicStairDownTex = mainGame.Content.Load<Texture2D>("StaticPic/stairsDown");

            Texture2D removableWallTex = mainGame.Content.Load<Texture2D>("StaticPic/Wall");
            Texture2D switchTex = mainGame.Content.Load<Texture2D>("StaticPic/Switch");

            Texture2D brainTex = mainGame.Content.Load<Texture2D>("brain");

            wallTex = mainGame.Content.Load<Texture2D>("StaticPic/Level2/side_wall_green");

            gameOverTexture = mainGame.Content.Load<Texture2D>("gameover");
            gameWinTexture = mainGame.Content.Load<Texture2D>("gamewin");

            stairs.LoadContent(basicStairUpTex, basicStairDownTex, "Down");

            firstSwitch.LoadContent(removableWallTex, switchTex);

            brain.LoadContent(brainTex);

            // Test
            board.LoadContent(basicGridTex);
            board.LoadBackgroundContent(basicMenuTex);

            //darwin.LoadContent(graphics.GraphicsDevice, darwinTex, zombieDarwinTex);
            darwin.LoadContent(graphics.GraphicsDevice, darwinUpTex, darwinDownTex, darwinRightTex, darwinLeftTex, zombieDarwinTex);
            firstZombie.LoadContent(zombieTex);
            cannibalZombie.LoadContent(cannibalTex);
            //secondZombie.LoadContent(zombieTex);
            //thirdZombie.LoadContent(zombieTex);
            zombieMessage.LoadContent(messagePic);
            darwinMessage.LoadContent(messagePic);
            switchMessage.LoadContent(messagePic);
            brainMessage.LoadContent(messagePic);

            gameStart.LoadContent(mainGame.Content.Load<Texture2D>("startScreen"));
            
            zTime.LoadContent(mainGame.Content.Load<Texture2D>("humanities_bar")); 
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

            zombieMessage.pointToSquare(firstZombie.X, firstZombie.Y, board);
            darwinMessage.pointToSquare(darwin.X, darwin.Y, board);
            switchMessage.pointToSquare(firstSwitch.X, firstSwitch.Y, board);
            brainMessage.pointToSquare(brain.X, brain.Y, board);
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
                MediaPlayer.Play(song);
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
            }

            stairs.Update(gameTime, darwin);

            firstZombie.setPictureSize(board.getSquareWidth(), board.getSquareLength());
            firstZombie.Update(gameTime, darwin, brain);

            cannibalZombie.setPictureSize(board.getSquareWidth(), board.getSquareLength());
            cannibalZombie.Update(gameTime, darwin);

            firstSwitch.Update(gameTime, ks, darwin);

            brain.Update(gameTime, ks, darwin);

            if (!darwin.isZombie())
                checkForGameOver(firstZombie);
            if (darwin.isZombie())
                checkForGameOver(cannibalZombie);

            //checkForGameWin();
            checkForSwitchToLevelThree();

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
                

                board.setGridPositionOpen(darwin);
                darwin.setGridPosition(2, 2);
                
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

                firstZombie.setGridPosition(10, 10);
                darwin.setHuman();
                gameState.setState(GameState.state.Level);
                gameOver = false;
                gameWin = false;
                MediaPlayer.Stop();
                MediaPlayer.Play(song);
            }

        }

        private void checkForExitGame(KeyboardState ks)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                mainGame.Exit();

            if (ks.IsKeyDown(Keys.Q) || ks.IsKeyDown(Keys.Space))
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

        private void checkForSwitchToLevelThree()
        {
            if (darwin.isOnTop(stairs))
            {
                board.setGridPositionOpen(darwin);
                darwin.setGridPosition(2, 2);
                mainGame.setCurLevel(Game1.LevelState.Level3);
                mainGame.setZTimeLevel(zTime, Game1.LevelState.Level3);
                firstZombie.setGridPosition(10, 10);
                darwin.setHuman();
                gameState.setState(GameState.state.Level);
                gameOver = false;
                gameWin = false;
                firstSwitch.turnOn();
            }
        }

        private void checkForGameWin()
        {
            if (darwin.isOnTop(stairs))
            {
                gameWin = true;
            }
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

        public void setZTime(ZombieTime mytime) 
        {
            zTime = mytime;

            zTimeReset = new ZombieTime(board);
            zTimeReset.reset();
            zTimeReset.setTime(mytime.getTime());
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
            firstZombie.Draw(spriteBatch);
            cannibalZombie.Draw(spriteBatch);
            //secondZombie.Draw(spriteBatch);
            //thirdZombie.Draw(spriteBatch);
            firstSwitch.Draw(spriteBatch);
            brain.Draw(spriteBatch);
            zTime.Draw(spriteBatch);

            foreach (BasicObject a in walls)
            {
                spriteBatch.Draw(wallTex, board.getPosition(a.X, a.Y), Color.White);
            }

            if (messageMode)
            {
                zombieMessage.Draw(spriteBatch, messageFont);
                darwinMessage.Draw(spriteBatch, messageFont);
                brainMessage.Draw(spriteBatch, messageFont);
                //switchMessage.Draw(spriteBatch, messageFont);
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

        private void setWallsInLevelTwo()
        {
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
            BasicObject w12 = new BasicObject(board);
            BasicObject w13 = new BasicObject(board);
            BasicObject w14 = new BasicObject(board);
            BasicObject w15 = new BasicObject(board);
            BasicObject w16 = new BasicObject(board);
            BasicObject w17 = new BasicObject(board);
            BasicObject w18 = new BasicObject(board);

            w1.setGridPosition(20, 17);
            w2.setGridPosition(21, 17);
            w3.setGridPosition(22, 17);
            w4.setGridPosition(23, 17);
            w5.setGridPosition(19, 17);
            w6.setGridPosition(18, 17);
            w7.setGridPosition(17, 17);
            w8.setGridPosition(16, 17);
            w9.setGridPosition(15, 17);
            w10.setGridPosition(14, 17);
            w11.setGridPosition(13, 17);
            w12.setGridPosition(12, 17);
            w13.setGridPosition(11, 17);
            w14.setGridPosition(10, 17);
            w15.setGridPosition(9, 17);
            w16.setGridPosition(8, 17);
            w17.setGridPosition(7, 17);
            w18.setGridPosition(6, 17);

            for (int m = 6; m < 24; m++)
            {
                board.setGridPositionOccupied(m, 17);
            }

            BasicObject w19 = new BasicObject(board);
            BasicObject w20 = new BasicObject(board);
            BasicObject w21 = new BasicObject(board);
            BasicObject w22 = new BasicObject(board);
            BasicObject w23 = new BasicObject(board);
            BasicObject w24 = new BasicObject(board);
            BasicObject w25 = new BasicObject(board);
            BasicObject w26 = new BasicObject(board);
            BasicObject w27 = new BasicObject(board);
            BasicObject w28 = new BasicObject(board);
            BasicObject w29 = new BasicObject(board);
            BasicObject w30 = new BasicObject(board);
            BasicObject w31 = new BasicObject(board);
            BasicObject w32 = new BasicObject(board);
            BasicObject w33 = new BasicObject(board);
            BasicObject w34 = new BasicObject(board);
            BasicObject w35 = new BasicObject(board);
            BasicObject w36 = new BasicObject(board);

            w19.setGridPosition(1, 10);
            w20.setGridPosition(2, 10);
            w21.setGridPosition(2, 10);
            w22.setGridPosition(3, 10);
            w23.setGridPosition(4, 10);
            w24.setGridPosition(5, 10);
            w25.setGridPosition(6, 10);
            w26.setGridPosition(7, 10);
            w27.setGridPosition(8, 10);
            w28.setGridPosition(9, 10);
            w29.setGridPosition(10, 10);
            w30.setGridPosition(11, 10);
            w31.setGridPosition(12, 10);
            w32.setGridPosition(13, 10);
            w33.setGridPosition(14, 10);
            w34.setGridPosition(15, 10);
            w35.setGridPosition(16, 10);
            w36.setGridPosition(17, 10);

            for (int n = 1; n < 18; n++)
            {
                board.setGridPositionOccupied(n, 10);
            }

            walls = new BasicObject[36] { w1, w2, w3, w4, w5, w6, w7, w8, w9, w10, w11, 
                w12, w13, w14, w15, w16, w17, w18, w19, w20, w21, w22, w23, w24, w25, 
                w26, w27, w28, w29, w30, w31, w32, w33, w34, w35, w36 };
        }

    }
}