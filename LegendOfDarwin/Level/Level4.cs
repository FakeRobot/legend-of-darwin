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
        private CongaLeaderZombie leaderZombie;
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

            Vector2[] myPath = new Vector2[4]; 
            myPath[0] = new Vector2(7,7);
            myPath[1] = new Vector2(7,19);
            myPath[2] = new Vector2(20,19);
            myPath[3] = new Vector2(20,7);

            leaderZombie = new CongaLeaderZombie(7,7,board.getNumSquaresX(),0,board.getNumSquaresY(),0,myPath,darwin,board);

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
            darwin.setGridPosition(16, 22);

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

            setWallsInLevelFour();
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

            leaderZombie.LoadContent(mainGame.Content.Load<Texture2D>("ZombiePic/CongaLeaderZombie"));
        
            zombieMessage.LoadContent(mainGame.Content.Load<Texture2D>("messageBox"));
            darwinMessage.LoadContent(mainGame.Content.Load<Texture2D>("messageBox"));
            switchMessage.LoadContent(mainGame.Content.Load<Texture2D>("messageBox"));

            gameStart.LoadContent(mainGame.Content.Load<Texture2D>("startScreen"));
            zTime.LoadContent(mainGame.Content.Load<Texture2D>("humanities_bar"));
            potion.LoadContent(mainGame.Content.Load<Texture2D>("StaticPic/potion"));

            
        }

        private void setWallsInLevelFour()
        {
            //lower left area wall
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
            BasicObject w9p = new BasicObject(board);
            BasicObject w10 = new BasicObject(board);
            BasicObject w11 = new BasicObject(board);
            BasicObject w12 = new BasicObject(board);
            BasicObject w13 = new BasicObject(board);
            BasicObject w14 = new BasicObject(board);
            BasicObject w15 = new BasicObject(board);
            BasicObject w16 = new BasicObject(board);
            BasicObject w17 = new BasicObject(board);
            BasicObject w18 = new BasicObject(board);
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
            BasicObject w37 = new BasicObject(board);
            BasicObject w38 = new BasicObject(board);
            BasicObject w39 = new BasicObject(board);
            BasicObject w40 = new BasicObject(board);
            BasicObject w41 = new BasicObject(board);
            BasicObject w42 = new BasicObject(board);
            BasicObject w43 = new BasicObject(board);
            BasicObject w44 = new BasicObject(board);
            BasicObject w45 = new BasicObject(board);
            BasicObject w46 = new BasicObject(board);
            BasicObject w47 = new BasicObject(board);
            BasicObject w48 = new BasicObject(board);
            BasicObject w49 = new BasicObject(board);
            BasicObject w50 = new BasicObject(board);
            BasicObject w51 = new BasicObject(board);
            BasicObject w52 = new BasicObject(board);
            BasicObject w53 = new BasicObject(board);
            BasicObject w54 = new BasicObject(board);
            BasicObject w55 = new BasicObject(board);
            BasicObject w56 = new BasicObject(board);
            BasicObject w57 = new BasicObject(board);
            BasicObject w58 = new BasicObject(board);
            BasicObject w59 = new BasicObject(board);
            BasicObject w60 = new BasicObject(board);
            BasicObject w61 = new BasicObject(board);
            BasicObject w62 = new BasicObject(board);
            BasicObject w63 = new BasicObject(board);
            BasicObject w64 = new BasicObject(board);
            BasicObject w65 = new BasicObject(board);
            BasicObject w66 = new BasicObject(board);
            BasicObject w67 = new BasicObject(board);
            BasicObject w68 = new BasicObject(board);
            BasicObject w69 = new BasicObject(board);
            BasicObject w70 = new BasicObject(board);
            BasicObject w71 = new BasicObject(board);
            BasicObject w72 = new BasicObject(board);
            BasicObject w73 = new BasicObject(board);
            BasicObject w74 = new BasicObject(board);
            BasicObject w75 = new BasicObject(board);
            BasicObject w76 = new BasicObject(board);
            BasicObject w77 = new BasicObject(board);
            BasicObject w78 = new BasicObject(board);
            BasicObject w79 = new BasicObject(board);
            BasicObject w80 = new BasicObject(board);
            BasicObject w81 = new BasicObject(board);
            BasicObject w82 = new BasicObject(board);
            BasicObject w83 = new BasicObject(board);
            BasicObject w84 = new BasicObject(board);
            BasicObject w85 = new BasicObject(board);

            // first lower row
            w0.setGridPosition(1, 22);
            w1.setGridPosition(2, 22);
            w2.setGridPosition(3, 22);
            w3.setGridPosition(4, 22);
            w4.setGridPosition(5, 22);
            w5.setGridPosition(6, 22);
            w6.setGridPosition(7, 22);
            w7.setGridPosition(8, 22);
            w8.setGridPosition(9, 22);
            w9p.setGridPosition(10,22);
            w9.setGridPosition(11, 22);
            w10.setGridPosition(12, 22);
            w11.setGridPosition(13, 22);
            w12.setGridPosition(14, 22);
            w13.setGridPosition(18, 22);
            w14.setGridPosition(19, 22);
            w15.setGridPosition(20, 22);
            w16.setGridPosition(21, 22);
            w17.setGridPosition(22, 22);
            w18.setGridPosition(23, 22);
            w19.setGridPosition(24, 22);
            w20.setGridPosition(25, 22);
            w21.setGridPosition(26, 22);
            w22.setGridPosition(27, 22);
            w23.setGridPosition(28, 22);
            w24.setGridPosition(29, 22);
            w25.setGridPosition(30, 22);
            w26.setGridPosition(31, 22);
            w27.setGridPosition(32, 22);

            for (int m = 1; m < 15; m++)
            {
                board.setGridPositionOccupied(m, 22);
            }
            for (int m = 18; m < 33; m++)
            {
                board.setGridPositionOccupied(m, 22);
            }

            //second lower row
            w28.setGridPosition(1, 21);
            w29.setGridPosition(2, 21);
            w30.setGridPosition(3, 21);
            w31.setGridPosition(4, 21);
            w32.setGridPosition(5, 21);
            w33.setGridPosition(6, 21);
            w34.setGridPosition(7, 21);
            w35.setGridPosition(8, 21);
            w36.setGridPosition(9, 21);
            w37.setGridPosition(10, 21);
            w38.setGridPosition(11, 21);
            w39.setGridPosition(12, 21);
            w40.setGridPosition(13, 21);
            w41.setGridPosition(14, 21);
            w42.setGridPosition(18, 21);
            w43.setGridPosition(19, 21);
            w44.setGridPosition(20, 21);
            w45.setGridPosition(21, 21);
            w46.setGridPosition(22, 21);
            w47.setGridPosition(23, 21);
            w48.setGridPosition(24, 21);
            w49.setGridPosition(25, 21);
            w50.setGridPosition(26, 21);
            w51.setGridPosition(27, 21);
            w52.setGridPosition(28, 21);
            w53.setGridPosition(29, 21);
            w54.setGridPosition(30, 21);
            w55.setGridPosition(31, 21);
            w56.setGridPosition(32, 21);

            for (int m = 1; m < 15; m++)
            {
                board.setGridPositionOccupied(m, 21);
            }
            for (int m = 18; m < 33; m++)
            {
                board.setGridPositionOccupied(m, 21);
            }

            //third lower row
            w57.setGridPosition(1, 20);
            w58.setGridPosition(2, 20);
            w59.setGridPosition(3, 20);
            w60.setGridPosition(4, 20);
            w61.setGridPosition(5, 20);
            w62.setGridPosition(6, 20);
            w63.setGridPosition(7, 20);
            w64.setGridPosition(8, 20);
            w65.setGridPosition(9, 20);
            w66.setGridPosition(10, 20);
            w67.setGridPosition(11, 20);
            w68.setGridPosition(12, 20);
            w69.setGridPosition(13, 20);
            w70.setGridPosition(14, 20);
            w71.setGridPosition(18, 20);
            w72.setGridPosition(19, 20);
            w73.setGridPosition(20, 20);
            w74.setGridPosition(21, 20);
            w75.setGridPosition(22, 20);
            w76.setGridPosition(23, 20);
            w77.setGridPosition(24, 20);
            w78.setGridPosition(25, 20);
            w79.setGridPosition(26, 20);
            w80.setGridPosition(27, 20);
            w81.setGridPosition(28, 20);
            w82.setGridPosition(29, 20);
            w83.setGridPosition(30, 20);
            w84.setGridPosition(31, 20);
            w85.setGridPosition(32, 20);

            for (int m = 1; m < 15; m++)
            {
                board.setGridPositionOccupied(m, 20);
            }
            for (int m = 18; m < 33; m++)
            {
                board.setGridPositionOccupied(m, 20);
            }

            ////upper long horizontal wall
            //BasicObject w19 = new BasicObject(board);
            //BasicObject w20 = new BasicObject(board);
            //BasicObject w21 = new BasicObject(board);
            //BasicObject w22 = new BasicObject(board);
            //BasicObject w23 = new BasicObject(board);
            //BasicObject w24 = new BasicObject(board);
            //BasicObject w25 = new BasicObject(board);
            //BasicObject w26 = new BasicObject(board);
            //BasicObject w27 = new BasicObject(board);
            //BasicObject w28 = new BasicObject(board);
            //BasicObject w29 = new BasicObject(board);
            //BasicObject w30 = new BasicObject(board);
            //BasicObject w31 = new BasicObject(board);
            //BasicObject w32 = new BasicObject(board);
            //BasicObject w33 = new BasicObject(board);
            //BasicObject w34 = new BasicObject(board);
            //BasicObject w35 = new BasicObject(board);
            //BasicObject w36 = new BasicObject(board);
            //BasicObject w48 = new BasicObject(board);
            //BasicObject w49 = new BasicObject(board);
            //BasicObject w50 = new BasicObject(board);
            //BasicObject w51 = new BasicObject(board);
            //BasicObject w52 = new BasicObject(board);

            //w19.setGridPosition(1, 10);
            //w20.setGridPosition(2, 10);
            //w21.setGridPosition(2, 10);
            //w22.setGridPosition(3, 10);
            //w23.setGridPosition(4, 10);
            //w24.setGridPosition(5, 10);
            //w25.setGridPosition(6, 10);
            //w26.setGridPosition(7, 10);
            //w27.setGridPosition(8, 10);
            //w28.setGridPosition(9, 10);
            //w29.setGridPosition(10, 10);
            //w30.setGridPosition(11, 10);
            //w31.setGridPosition(12, 10);
            //w32.setGridPosition(13, 10);
            //w33.setGridPosition(14, 10);
            //w34.setGridPosition(15, 10);
            //w48.setGridPosition(16, 10);
            //w49.setGridPosition(17, 10);
            //w50.setGridPosition(18, 10);
            //w51.setGridPosition(19, 10);
            //w52.setGridPosition(20, 10);

            //for (int n = 1; n < 21; n++)
            //{
            //    board.setGridPositionOccupied(n, 10);
            //}

            ////short vertical wall
            //BasicObject w37 = new BasicObject(board);
            //BasicObject w38 = new BasicObject(board);
            //BasicObject w39 = new BasicObject(board);
            //BasicObject w40 = new BasicObject(board);
            //BasicObject w41 = new BasicObject(board);
            //BasicObject w42 = new BasicObject(board);

            //w37.setGridPosition(26, 1);
            //w38.setGridPosition(26, 2);
            //w39.setGridPosition(26, 3);
            //w40.setGridPosition(26, 4);
            //w41.setGridPosition(26, 5);
            //w42.setGridPosition(26, 6);

            //for (int k = 1; k < 7; k++)
            //{
            //    board.setGridPositionOccupied(26, k);
            //}

            walls = new BasicObject[87] {w0, w1, w2, w3, w4, w5, w6, w7, w8, w9, w9p,w10, w11, 
                w12, w13, w14, w15, w16, w17, w18, 
                 w19, w20, w21, w22, w23, w24, w25,w26,w27,
            w28, w29, w30, w31, w32, w33, w34, w35, w36, w37, w38,w39, w40, 
                w41, w42, w43, w44, w45, w46, w47, 
                 w48, w49, w50, w51, w52, w53, w54,w55,w56,
            w57, w58, w59, w60, w61, w62, w63, w64, w65, w66, w67,w68, w69, 
                w70, w71, w72, w73, w74, w75, w76, 
                 w77, w78, w79, w80, w81, w82, w83,w84,w85};
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

            if (!darwin.isZombie())
            {
                checkForGameOver(leaderZombie);
            }

            darwin.Update(gameTime, ks, board, darwin.X, darwin.Y);

            stairs.Update(gameTime, darwin);

            //firstZombie.Update(gameTime, darwin);
            //secondZombie.Update(gameTime, darwin);
            //thirdZombie.Update(gameTime, darwin);
            leaderZombie.Update(gameTime, darwin);

            firstSwitch.Update(gameTime, ks, darwin);

            potion.Update(gameTime, ks, darwin, zTime);

            

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
                darwin.setGridPosition(16, 20);

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

                leaderZombie.Reset(7, 7);

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
            leaderZombie.Draw(spriteBatch);

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




