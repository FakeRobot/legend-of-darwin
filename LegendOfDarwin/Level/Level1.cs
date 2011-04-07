using System;
using System.Collections.Generic;
using System.Linq;
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
    public class Level1
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public GraphicsDevice device;

        private GameState gameState;
        private GameStart gameStart;

        private Darwin darwin;
        private Zombie firstZombie, secondZombie, thirdZombie, fourthZombie, fifthZombie, sixthZombie;
        private Switch firstSwitch;
        private Brain brain;
        private GameBoard board;
        private ZombieTime zTime;
        private Vortex vortex;
        private Potion potion;
        private Stairs stairs;

        private BasicObject[] walls;
        private Texture2D wallTex;

        public SpriteFont messageFont;
        public bool keyIsHeldDown = false;
        public bool gameOver = false;
        public bool gameWin = false;
        private int gameOverCounter = 0;

        private bool playDeathSound = true;
        private SoundEffect deathScreamSound;

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
        private MessageBox brainMessage;

        //public Song song;
        public Game1 mainGame;

        public Level1(Game1 myMainGame)
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
            firstZombie = new Zombie(10, 10, 15, 5, 15, 5, board);
            secondZombie = new Zombie(10, 16, 15, 5, 15, 5, board);
            thirdZombie = new Zombie(12, 10, 15, 5, 15, 5, board);
            fourthZombie = new Zombie(20, 7, 27, 15, 22, 2, board);
            fifthZombie = new Zombie(22, 10, 25, 15, 22, 2, board);
            sixthZombie = new Zombie(21, 4, 25, 15, 15, 2, board);

            String zombieString = "This a zombie,\n don't near him \nas a human!!";
            zombieMessage = new MessageBox(board.getPosition(12, 8).X, board.getPosition(10, 10).Y, zombieString);

            String darwinString = "This is darwin,\n move with arrows, \n z to transform, \n a for actions";
            darwinMessage = new MessageBox(board.getPosition(12, 8).X, board.getPosition(10, 10).Y, darwinString);

            String switchString = "This is a switch\n face it and press A\n to see what happens!!";
            switchMessage = new MessageBox(board.getPosition(12, 8).X, board.getPosition(10, 10).Y, switchString);

            String brainString = "Move the brain as a \nzombie.\n Zombie's like brains!!";
            brainMessage = new MessageBox(board.getPosition(12, 8).X, board.getPosition(10, 10).Y, brainString);

            stairs = new Stairs(board);

            brain = new Brain(board, 3, 3);

            BasicObject[] removableWalls = setRemovableWallsInLevelOne();

            BasicObject switchSquare = new BasicObject(board);
            switchSquare.X = 13;
            switchSquare.Y = 2;

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

            if (board.isGridPositionOpen(21, 20))
            {
                stairs.setGridPosition(27, 21);
                stairs.setDestination(board.getPosition(27, 21));
            }

            zTime = new ZombieTime(board);

            vortex = new Vortex(board, 19, 20);

            setPotionPosition(25, 4);

            setWalls();
        }

        private BasicObject[] setRemovableWallsInLevelOne()
        {

            BasicObject s5 = new BasicObject(board);
            s5.X = 25;
            s5.Y = 19;

            BasicObject s6 = new BasicObject(board);
            s6.X = 26;
            s6.Y = 19;

            BasicObject s7 = new BasicObject(board);
            s7.X = 27;
            s7.Y = 19;

            BasicObject s8 = new BasicObject(board);
            s8.X = 28;
            s8.Y = 19;

            BasicObject s9 = new BasicObject(board);
            s9.X = 29;
            s9.Y = 19;

            BasicObject s10 = new BasicObject(board);
            s10.X = 30;
            s10.Y = 19;

            BasicObject s11 = new BasicObject(board);
            s11.X = 31;
            s11.Y = 19;

            BasicObject[] removableWalls = new BasicObject[7] {s5, s6, s7, s8, s9, s10, s11 };
            return removableWalls;
        }

        private void setPotionPosition(int x, int y)
        {
            potion = new Potion(board);
            potion.setDestination(board.getPosition(x, y));
            potion.setGridPosition(x, y);
        }

        private void setWalls()
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

            w1.setGridPosition(11, 1);
            board.setGridPositionOccupied(11, 1);
            w2.setGridPosition(11, 2);
            board.setGridPositionOccupied(11, 2);
            w3.setGridPosition(11, 3);
            board.setGridPositionOccupied(11, 3);
            w4.setGridPosition(11, 5);
            board.setGridPositionOccupied(11, 5);
            w5.setGridPosition(12, 5);
            board.setGridPositionOccupied(12, 5);
            w6.setGridPosition(15, 1);
            board.setGridPositionOccupied(15, 1);
            w7.setGridPosition(15, 2);
            board.setGridPositionOccupied(15, 2);
            w8.setGridPosition(15, 3);
            board.setGridPositionOccupied(15, 3);
            w9.setGridPosition(11, 4);
            board.setGridPositionOccupied(11, 4);
            w10.setGridPosition(13, 5);
            board.setGridPositionOccupied(13, 5);

            w11.setGridPosition(6, 18);
            board.setGridPositionOccupied(6, 18);
            w12.setGridPosition(6, 19);
            board.setGridPositionOccupied(6, 19);
            w13.setGridPosition(6, 20);
            board.setGridPositionOccupied(6, 20);
            w14.setGridPosition(6, 17);
            board.setGridPositionOccupied(6, 17);
            w15.setGridPosition(6, 16);
            board.setGridPositionOccupied(6, 16);
            w18.setGridPosition(6, 21);
            board.setGridPositionOccupied(6, 21);
            w16.setGridPosition(6, 22);
            board.setGridPositionOccupied(6, 22);
            w17.setGridPosition(6, 15);
            board.setGridPositionOccupied(6, 15);
            w19.setGridPosition(6, 14);
            board.setGridPositionOccupied(6, 14);
            w20.setGridPosition(6, 13);
            board.setGridPositionOccupied(6, 13);
            w21.setGridPosition(6, 12);
            board.setGridPositionOccupied(6, 12);

            w22.setGridPosition(24, 18);
            board.setGridPositionOccupied(24, 18);
            w23.setGridPosition(24, 19);
            board.setGridPositionOccupied(24, 19);
            w24.setGridPosition(24, 20);
            board.setGridPositionOccupied(24, 20);
            w25.setGridPosition(24, 17);
            board.setGridPositionOccupied(24, 17);
            w26.setGridPosition(24, 16);
            board.setGridPositionOccupied(24, 16);
            w27.setGridPosition(24, 21);
            board.setGridPositionOccupied(24, 21);
            w28.setGridPosition(24, 22);
            board.setGridPositionOccupied(24, 22);
            w29.setGridPosition(24, 15);
            board.setGridPositionOccupied(24, 15);
            w30.setGridPosition(24, 14);
            board.setGridPositionOccupied(24, 14);
            w31.setGridPosition(24, 13);
            board.setGridPositionOccupied(24, 13);
            w32.setGridPosition(24, 12);
            board.setGridPositionOccupied(24, 12);

            w33.setGridPosition(15, 4);
            board.setGridPositionOccupied(15, 4);
            w34.setGridPosition(15, 5);
            board.setGridPositionOccupied(15, 5);
            w35.setGridPosition(15, 6);
            board.setGridPositionOccupied(15, 6);
            w36.setGridPosition(15, 7);
            board.setGridPositionOccupied(15, 7);
            w37.setGridPosition(15, 8);
            board.setGridPositionOccupied(15, 8);
            w38.setGridPosition(15, 9);
            board.setGridPositionOccupied(15, 9);
            w39.setGridPosition(15, 10);
            board.setGridPositionOccupied(15, 10);
            w40.setGridPosition(15, 11);
            board.setGridPositionOccupied(15, 11);
            w41.setGridPosition(15, 12);
            board.setGridPositionOccupied(15, 12);
            w42.setGridPosition(15, 13);
            board.setGridPositionOccupied(15, 13);
            w43.setGridPosition(15, 14);
            board.setGridPositionOccupied(15, 14);
            w44.setGridPosition(15, 15);
            board.setGridPositionOccupied(15, 15);
            w45.setGridPosition(15, 16);
            board.setGridPositionOccupied(15, 16);
            w46.setGridPosition(15, 17);
            board.setGridPositionOccupied(15, 17);

            walls = new BasicObject[46] {w1, w2, w3, w4, w5, w6, w7, w8, w9, w10, w11, w12, w13, w14, w15, w16, w17, w18, w19, w20, w21, w22, w23, w24, w25, w26, w27, w28, w29, w30, w31, w32, w33, w34, w35, w36, w37, w38, w39, w40, w41, w42, w43, w44, w45, w46 };
        }

        public void LoadContent()
        {
            messageFont = mainGame.Content.Load<SpriteFont>("TimesNewRoman");
            deathScreamSound = mainGame.Content.Load<SoundEffect>("chewScream");

            Texture2D darwinTex = mainGame.Content.Load<Texture2D>("DarwinPic/Darwin");
            Texture2D darwinUpTex = mainGame.Content.Load<Texture2D>("DarwinPic/DarwinUp");
            Texture2D darwinDownTex = mainGame.Content.Load<Texture2D>("DarwinPic/Darwin");
            Texture2D darwinRightTex = mainGame.Content.Load<Texture2D>("DarwinPic/DarwinRight");
            Texture2D darwinLeftTex = mainGame.Content.Load<Texture2D>("DarwinPic/DarwinLeft");
            Texture2D zombieDarwinTex = mainGame.Content.Load<Texture2D>("DarwinPic/ZombieDarwin");
            Texture2D deadDarwinText = mainGame.Content.Load<Texture2D>("DarwinPic/DeadDarwin");
            SoundEffect transformSound = mainGame.Content.Load<SoundEffect>("transform");

            darwin.LoadContent(graphics.GraphicsDevice, darwinUpTex, darwinDownTex, 
                darwinRightTex, darwinLeftTex, zombieDarwinTex, deadDarwinText, transformSound);
            

            gameOverTexture = mainGame.Content.Load<Texture2D>("gameover");
            gameWinTexture = mainGame.Content.Load<Texture2D>("gamewin");
            Texture2D menuBarTexture = mainGame.Content.Load<Texture2D>("rainbow");

            stairs.LoadContent(mainGame.Content.Load<Texture2D>("StaticPic/stairsUp"),
                mainGame.Content.Load<Texture2D>("StaticPic/stairsDown"), "Down");

            SoundEffect switchSound = mainGame.Content.Load<SoundEffect>("switch");
            firstSwitch.LoadContent(mainGame.Content.Load<Texture2D>("StaticPic/Wall"),
                mainGame.Content.Load<Texture2D>("StaticPic/Switch"), switchSound);

            brain.LoadContent(mainGame.Content.Load<Texture2D>("brain"));

            board.LoadContent(mainGame.Content.Load<Texture2D>("StaticPic/metal_tile"));
            board.LoadBackgroundContent(mainGame.Content.Load<Texture2D>("StaticPic/side_wall"));
            board.loadMenuBar(menuBarTexture);

            wallTex = mainGame.Content.Load<Texture2D>("StaticPic/side_wall");


            //darwin.LoadContent(graphics.GraphicsDevice, darwinTex, zombieDarwinTex);

            firstZombie.LoadContent(mainGame.Content.Load<Texture2D>("ZombiePic/Zombie"));
            secondZombie.LoadContent(mainGame.Content.Load<Texture2D>("ZombiePic/Zombie"));
            thirdZombie.LoadContent(mainGame.Content.Load<Texture2D>("ZombiePic/Zombie"));
            fourthZombie.LoadContent(mainGame.Content.Load<Texture2D>("ZombiePic/Zombie"));
            fifthZombie.LoadContent(mainGame.Content.Load<Texture2D>("ZombiePic/Zombie"));
            sixthZombie.LoadContent(mainGame.Content.Load<Texture2D>("ZombiePic/Zombie"));

            zombieMessage.LoadContent(mainGame.Content.Load<Texture2D>("messageBox"));
            darwinMessage.LoadContent(mainGame.Content.Load<Texture2D>("messageBox"));
            switchMessage.LoadContent(mainGame.Content.Load<Texture2D>("messageBox"));
            brainMessage.LoadContent(mainGame.Content.Load<Texture2D>("messageBox"));

            gameStart.LoadContent(mainGame.Content.Load<Texture2D>("LevelOne"));
            zTime.LoadContent(mainGame.Content.Load<Texture2D>("humanities_bar"));
            vortex.LoadContent(mainGame.Content.Load<Texture2D>("vortex"));
            potion.LoadContent(mainGame.Content.Load<Texture2D>("StaticPic/potion"), mainGame.Content.Load<SoundEffect>("potion"));
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
                //MediaPlayer.Play(song);
                gameState.setState(GameState.state.Level);
            }
        }

        private void UpdateLevelState(GameTime gameTime)
        {
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

            checkForExitGame(ks);

            updateKeyHeldDown(ks);


            if (darwin.isDarwinAlive())
            {
                if (!darwin.isZombie())
                {
                    checkForGameOver(firstZombie);
                    checkForGameOver(secondZombie);
                    checkForGameOver(thirdZombie);
                    checkForGameOver(fourthZombie);
                    checkForGameOver(fifthZombie);
                    checkForGameOver(sixthZombie);
                }
                checkForGameOver(vortex);

                darwin.Update(gameTime, ks, board, darwin.X, darwin.Y);
            }

            stairs.Update(gameTime, darwin);

            firstZombie.Update(gameTime, darwin, brain);
            secondZombie.Update(gameTime, darwin, brain);
            thirdZombie.Update(gameTime, darwin, brain);
            fourthZombie.Update(gameTime, darwin, brain);
            fifthZombie.Update(gameTime, darwin, brain);
            sixthZombie.Update(gameTime, darwin, brain);

            firstSwitch.Update(gameTime, ks, darwin);

            brain.Update(gameTime, ks, darwin);

            vortex.Update(gameTime, ks);
            vortex.CollisionWithZombie(firstZombie);
            vortex.CollisionWithZombie(secondZombie);
            vortex.CollisionWithZombie(thirdZombie);
            vortex.CollisionWithZombie(fourthZombie);
            vortex.CollisionWithZombie(fifthZombie);
            vortex.CollisionWithZombie(sixthZombie);
            vortex.CollisionWithBO(brain, board);

            potion.Update(gameTime, ks, darwin, zTime);
            
            //checkForGameWin();
            checkForSwitchToLevelTwo();

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
                
                gameOver = false;
                gameWin = false;

                board.setGridPositionOpen(darwin);
                darwin.setGridPosition(2, 20);

                zTime.reset();

                board.setGridPositionOpen(firstZombie);
                board.setGridPositionOpen(secondZombie);
                board.setGridPositionOpen(thirdZombie);
                board.setGridPositionOpen(fourthZombie);
                board.setGridPositionOpen(fifthZombie);
                board.setGridPositionOpen(sixthZombie);
                board.setGridPositionOpen(brain);
                board.setGridPositionOpen(potion);

                firstZombie.setGridPosition(10, 10);
                board.setGridPositionOccupied(firstZombie.X, firstZombie.Y);
                firstZombie.setZombieAlive(true);

                secondZombie.setGridPosition(10, 16);
                board.setGridPositionOccupied(secondZombie.X, secondZombie.Y);
                secondZombie.setZombieAlive(true);

                thirdZombie.setGridPosition(12, 10);
                board.setGridPositionOccupied(thirdZombie.X, thirdZombie.Y);
                thirdZombie.setZombieAlive(true);

                fourthZombie.setGridPosition(20, 7);
                board.setGridPositionOccupied(fourthZombie.X, fourthZombie.Y);
                fourthZombie.setZombieAlive(true);

                fifthZombie.setGridPosition(22, 10);
                board.setGridPositionOccupied(fifthZombie.X, fifthZombie.Y);
                fifthZombie.setZombieAlive(true);

                sixthZombie.setGridPosition(22, 10);
                board.setGridPositionOccupied(sixthZombie.X, sixthZombie.Y);
                sixthZombie.setZombieAlive(true);

                potion.setGridPosition(25, 4);

                brain.setGridPosition(3, 3);
                brain.setVisible(true);
                board.setGridPositionOccupied(brain.X, brain.Y);

                potion.reset();
                darwin.setHuman();
                darwin.setDarwinAlive();
                playDeathSound = true;
                gameState.setState(GameState.state.Level);
                //MediaPlayer.Stop();
                //MediaPlayer.Play(song);

                mainGame.DEATH_COUNTER++;
                
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

        private void checkForGameOver(Vortex myVortex)
        {
            if (darwin.isOnTop(myVortex))
            {
                gameOver = true;
            }
        }

        private void checkForGameWin()
        {
            if (darwin.isOnTop(stairs))
            {
                gameWin = true;
            }
        }

        private void checkForSwitchToLevelTwo()
        {
            if (darwin.isOnTop(stairs)) 
            {
                //reset everything dynamic on the level
                //board.setGridPositionOpen(darwin);
                //darwin.setGridPosition(2, 2);

                mainGame.setCurLevel(Game1.LevelState.Level2);
                mainGame.setZTimeLevel(zTime,Game1.LevelState.Level2);
                //board.setGridPositionOpen(firstZombie);
                //board.setGridPositionOpen(secondZombie);
                //board.setGridPositionOpen(thirdZombie);
                //board.setGridPositionOpen(fourthZombie);
                //board.setGridPositionOpen(brain);
                //board.setGridPositionOpen(potion);

                //firstZombie.setGridPosition(10, 10);
                //board.setGridPositionOccupied(firstZombie.X, firstZombie.Y);
                //firstZombie.setZombieAlive(true);

                //secondZombie.setGridPosition(10, 16);
                //board.setGridPositionOccupied(secondZombie.X, secondZombie.Y);
                //secondZombie.setZombieAlive(true);

                //thirdZombie.setGridPosition(16, 10);
                //board.setGridPositionOccupied(thirdZombie.X, thirdZombie.Y);
                //thirdZombie.setZombieAlive(true);

                //thirdZombie.setGridPosition(16, 10);
                //board.setGridPositionOccupied(thirdZombie.X, thirdZombie.Y);
                //thirdZombie.setZombieAlive(true);

                //potion.setGridPosition(20, 4);
                //board.setGridPositionOccupied(potion.X, potion.Y);

                //brain.setGridPosition(2, 18);
                //board.setGridPositionOccupied(brain.X, brain.Y);

                //potion.reset();
                //firstSwitch.turnOn();
                darwin.setHuman();
                gameState.setState(GameState.state.Level);
                gameOver = false;
                gameWin = false;
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
            secondZombie.Draw(spriteBatch);
            thirdZombie.Draw(spriteBatch);
            fourthZombie.Draw(spriteBatch);
            fifthZombie.Draw(spriteBatch);
            sixthZombie.Draw(spriteBatch);
            firstSwitch.Draw(spriteBatch);
            brain.Draw(spriteBatch);
            zTime.Draw(spriteBatch);
            vortex.Draw(spriteBatch);
            potion.Draw(spriteBatch);

            if (messageMode)
            {
                zombieMessage.Draw(spriteBatch, messageFont);
                darwinMessage.Draw(spriteBatch, messageFont);
                brainMessage.Draw(spriteBatch, messageFont);
                //switchMessage.Draw(spriteBatch, messageFont);
            }

            foreach(BasicObject a in walls)
            {
                spriteBatch.Draw(wallTex, board.getPosition(a.X, a.Y), Color.White);
            }

            spriteBatch.DrawString(messageFont, "HUMANITY: ", 
                new Vector2(board.getPosition(zTime.X, zTime.Y).X + board.getSquareWidth()*2, board.getPosition(zTime.X, 24).Y),
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

        
    }
}

