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
    public class Level5
    {

        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public GraphicsDevice device;

        private GameState gameState;
        private GameStart gameStart;

        private Darwin darwin;
        private GameBoard board;

        private ZombieTime zTime;
        private ZombieTime zTimeReset; //what zTime should reset to

        private Potion potion;
        private Stairs stairs;

        private PyroZombie northZombie, southZombie, eastZombie, westZombie;

        private LinkedList<Flame> flames;

        private Box[] boxes;
        private BasicObject[] spotsForPattern;
        private BoxPattern pattern;
        private Texture2D sparkleTex;

        private Snake snake, snake2, snake3, snake4;

        private BasicObject[] walls;
        private Texture2D wallTex;

        // These are the holes
        private Vortex[] vortexes;

        public SpriteFont messageFont;
        public bool keyIsHeldDown = false;
        public bool gameOver = false;
        public bool gameWin = false;
        private int gameOverCounter = 0;
        private bool fellDownPit = false;
        private int fellDownCounter = 0;
        private bool playDeathSound = true;

        private int counter;
        private int counterReady;
        public Texture2D gameOverTexture;
        public Texture2D gameWinTexture;

        Vector2 gameOverPosition = Vector2.Zero;

        bool messageMode = false;
        int messageModeCounter = 0;
        private MessageBox zombieMessage;
        private MessageBox darwinMessage;

        //public Song song;
        public SoundEffect revealStairsSound;
        public bool playSound = true;

        public Game1 mainGame;

        public Level5(Game1 myMainGame)
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

            String zombieString = "This a zombie,\n don't near him \nas a human!!";
            zombieMessage = new MessageBox(board.getPosition(12, 8).X, board.getPosition(10, 10).Y, zombieString);

            String darwinString = "This is darwin,\n move with arrows, \n z to transform, \n a for actions";
            darwinMessage = new MessageBox(board.getPosition(12, 8).X, board.getPosition(10, 10).Y, darwinString);

            stairs = new Stairs(board);
            if (board.isGridPositionOpen(5, 18))
            {
                stairs.setGridPosition(5, 18);
            }

            // Initial starting position
            darwin.setGridPosition(6, 18);
            if (board.isGridPositionOpen(darwin))
            {
                board.setGridPositionOccupied(darwin.X, darwin.Y);
                darwin.setPosition(board.getPosition(darwin).X, board.getPosition(darwin).Y);
            }

            // Darwin's lag movement
            counterReady = counter = 5;

            zTime = new ZombieTime(board);
            zTimeReset = new ZombieTime(board);

            setPotionPosition(27, 5);

            setBoxes();
            setBoxPattern();
            pattern = new BoxPattern(board, spotsForPattern);

            setVortexes();

            setWalls();

            snake = new Snake(10, 9, 27, 5, 18, 5, board);
            snake2 = new Snake(8, 15, 27, 5, 18, 5, board);
            snake3 = new Snake(21, 9, 27, 5, 18, 5, board);
            snake4 = new Snake(21, 15, 27, 5, 18, 5, board);

            northZombie = new PyroZombie(15, 3, 25, 4, 3, 3, board);
            northZombie.setGridPosition(15, 3);
            northZombie.setCurrentPatrolPoint(new Vector2(27, 3));
            northZombie.setNextPatrolPoint(new Vector2(5, 3));
            
            southZombie = new PyroZombie(15, 20, 25, 4, 20, 20, board);
            southZombie.setGridPosition(15, 20);
            southZombie.setCurrentPatrolPoint(new Vector2(5, 20));
            southZombie.setNextPatrolPoint(new Vector2(27, 20));
           
            eastZombie = new PyroZombie(29, 11, 29, 29, 19, 4, board);
            eastZombie.setGridPosition(29, 11);
            eastZombie.setCurrentPatrolPoint(new Vector2(29, 17));
            eastZombie.setNextPatrolPoint(new Vector2(29, 6));

            westZombie = new PyroZombie(3, 11, 3, 3, 19, 4, board);
            westZombie.setGridPosition(3, 11);
            westZombie.setCurrentPatrolPoint(new Vector2(3, 5));
            westZombie.setNextPatrolPoint(new Vector2(3, 18));

            flames = new LinkedList<Flame>();

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

            w1.setGridPosition(11, 1);
            board.setGridPositionOccupied(11, 1);
 
            walls = new BasicObject[1] { w1 };
        }

        private void setBoxes()
        {
            Box b1 = new Box(board, 7, 8);
            Box b2 = new Box(board, 13, 8);
            Box b3 = new Box(board, 19, 8);
            Box b4 = new Box(board, 25, 8);
            Box b5 = new Box(board, 10, 10);
            Box b6 = new Box(board, 16, 10);
            Box b7 = new Box(board, 22, 10);

            Box b8 = new Box(board, 7, 13);
            Box b9 = new Box(board, 8, 13);
            Box b10 = new Box(board, 9, 13);
            Box b11 = new Box(board, 10, 13);
            Box b12 = new Box(board, 11, 13);
            Box b13 = new Box(board, 12, 13);
            Box b14 = new Box(board, 13, 13);

            boxes = new Box[14] { b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, b13, b14 };
        }

        private void resetBoxes()
        {
            foreach (Box b in boxes)
            {
                board.setGridPositionOpen(b);
            }

            boxes[0].setGridPosition(7, 8);
            boxes[1].setGridPosition(13, 8);
            boxes[2].setGridPosition(19, 8);
            boxes[3].setGridPosition(25, 8);
            boxes[4].setGridPosition(10, 10);
            boxes[5].setGridPosition(16, 10);
            boxes[6].setGridPosition(22, 10);

            boxes[7].setGridPosition(7, 13);
            boxes[8].setGridPosition(8, 13);
            boxes[9].setGridPosition(9, 13);
            boxes[10].setGridPosition(10, 13);
            boxes[11].setGridPosition(11, 13);
            boxes[12].setGridPosition(12, 13);
            boxes[13].setGridPosition(13, 13);

            foreach (Box b in boxes)
            {
                board.setGridPositionOccupied(b);
                b.setVisible(true);
            }
        }

        private void setBoxPattern()
        {
            BasicObject bo1 = new BasicObject(board);
                bo1.setGridPosition(7, 8);
            BasicObject bo2 = new BasicObject(board);
                bo2.setGridPosition(13, 8);
            BasicObject bo3 = new BasicObject(board);
                bo3.setGridPosition(19, 8);
            BasicObject bo4 = new BasicObject(board);
                bo4.setGridPosition(25, 8);

            BasicObject bo5 = new BasicObject(board);
                bo5.setGridPosition(7, 15);
            BasicObject bo6 = new BasicObject(board);
                bo6.setGridPosition(13, 15);
            BasicObject bo7 = new BasicObject(board);
                bo7.setGridPosition(19, 15);
            BasicObject bo8 = new BasicObject(board);
                bo8.setGridPosition(25, 15);

            BasicObject bo9 = new BasicObject(board);
                bo9.setGridPosition(10, 10);
            BasicObject bo10 = new BasicObject(board);
                bo10.setGridPosition(16, 10);
            BasicObject bo11 = new BasicObject(board);
                bo11.setGridPosition(22, 10);

            BasicObject bo12 = new BasicObject(board);
                bo12.setGridPosition(10, 13);
            BasicObject bo13 = new BasicObject(board);
                bo13.setGridPosition(16, 13);
            BasicObject bo14 = new BasicObject(board);
                bo14.setGridPosition(22, 13);

            spotsForPattern = new BasicObject[14] { bo1, bo2, bo3, bo4, bo5, bo6, bo7, bo8, bo9, bo10, bo11, bo12, bo13, bo14 };
        }

        private void setVortexes()
        {
            Vortex vn1 = new Vortex(board, 4, 4);
            Vortex vn2 = new Vortex(board, 5, 4);
            Vortex vn3 = new Vortex(board, 6, 4);
            Vortex vn4 = new Vortex(board, 7, 4);
            Vortex vn5 = new Vortex(board, 8, 4);
            Vortex vn6 = new Vortex(board, 9, 4);
            Vortex vn7 = new Vortex(board, 10, 4);
            Vortex vn8 = new Vortex(board, 11, 4);
            Vortex vn9 = new Vortex(board, 12, 4);
            Vortex vn10 = new Vortex(board, 13, 4);
            Vortex vn11 = new Vortex(board, 14, 4);
            Vortex vn12 = new Vortex(board, 15, 4);
            Vortex vn13 = new Vortex(board, 16, 4);
            Vortex vn14 = new Vortex(board, 17, 4);
            Vortex vn15 = new Vortex(board, 18, 4);
            Vortex vn16 = new Vortex(board, 19, 4);
            Vortex vn17 = new Vortex(board, 20, 4);
            Vortex vn18 = new Vortex(board, 21, 4);
            Vortex vn19 = new Vortex(board, 22, 4);
            Vortex vn20 = new Vortex(board, 23, 4);
            Vortex vn21 = new Vortex(board, 24, 4);
            Vortex vn22 = new Vortex(board, 25, 4);
            Vortex vn23 = new Vortex(board, 26, 4);
            Vortex vn24 = new Vortex(board, 27, 4);
            Vortex vn25 = new Vortex(board, 28, 4);

            Vortex vs1 = new Vortex(board, 4, 19);
            Vortex vs2 = new Vortex(board, 5, 19);
            Vortex vs3 = new Vortex(board, 6, 19);
            Vortex vs4 = new Vortex(board, 7, 19);
            Vortex vs5 = new Vortex(board, 8, 19);
            Vortex vs6 = new Vortex(board, 9, 19);
            Vortex vs7 = new Vortex(board, 10, 19);
            Vortex vs8 = new Vortex(board, 11, 19);
            Vortex vs9 = new Vortex(board, 12, 19);
            Vortex vs10 = new Vortex(board, 13, 19);
            Vortex vs11 = new Vortex(board, 14, 19);
            Vortex vs12 = new Vortex(board, 15, 19);
            Vortex vs13 = new Vortex(board, 16, 19);
            Vortex vs14 = new Vortex(board, 17, 19);
            Vortex vs15 = new Vortex(board, 18, 19);
            Vortex vs16 = new Vortex(board, 19, 19);
            Vortex vs17 = new Vortex(board, 20, 19);
            Vortex vs18 = new Vortex(board, 21, 19);
            Vortex vs19 = new Vortex(board, 22, 19);
            Vortex vs20 = new Vortex(board, 23, 19);
            Vortex vs21 = new Vortex(board, 24, 19);
            Vortex vs22 = new Vortex(board, 25, 19);
            Vortex vs23 = new Vortex(board, 26, 19);
            Vortex vs24 = new Vortex(board, 27, 19);
            Vortex vs25 = new Vortex(board, 28, 19);

            Vortex ve1 = new Vortex(board, 28, 5);
            Vortex ve2 = new Vortex(board, 28, 6);
            Vortex ve3 = new Vortex(board, 28, 7);
            Vortex ve4 = new Vortex(board, 28, 8);
            Vortex ve5 = new Vortex(board, 28, 9);
            Vortex ve6 = new Vortex(board, 28, 10);
            Vortex ve7 = new Vortex(board, 28, 11);
            Vortex ve8 = new Vortex(board, 28, 12);
            Vortex ve9 = new Vortex(board, 28, 13);
            Vortex ve10 = new Vortex(board, 28, 14);
            Vortex ve11 = new Vortex(board, 28, 15);
            Vortex ve12 = new Vortex(board, 28, 16);
            Vortex ve13 = new Vortex(board, 28, 17);
            Vortex ve14 = new Vortex(board, 28, 18);

            Vortex vw1 = new Vortex(board, 4, 5);
            Vortex vw2 = new Vortex(board, 4, 6);
            Vortex vw3 = new Vortex(board, 4, 7);
            Vortex vw4 = new Vortex(board, 4, 8);
            Vortex vw5 = new Vortex(board, 4, 9);
            Vortex vw6 = new Vortex(board, 4, 10);
            Vortex vw7 = new Vortex(board, 4, 11);
            Vortex vw8 = new Vortex(board, 4, 12);
            Vortex vw9 = new Vortex(board, 4, 13);
            Vortex vw10 = new Vortex(board, 4, 14);
            Vortex vw11 = new Vortex(board, 4, 15);
            Vortex vw12 = new Vortex(board, 4, 16);
            Vortex vw13 = new Vortex(board, 4, 17);
            Vortex vw14 = new Vortex(board, 4, 18);

            vortexes = new Vortex[78] { vn1, vn2, vn3, vn4, vn5, vn6, vn7, vn8, vn9, vn10, vn11, vn12, vn13, vn14, vn15, vn16, vn17, vn18, vn19, vn20, vn21, vn22, vn23, vn24, vn25,
                                        vs1, vs2, vs3, vs4, vs5, vs6, vs7, vs8, vs9, vs10, vs11, vs12, vs13, vs14, vs15, vs16, vs17, vs18, vs19, vs20, vs21, vs22, vs23, vs24, vs25,
                                        ve1, ve2, ve3, ve4, ve5, ve6, ve7, ve8, ve9, ve10, ve11, ve12, ve13, ve14,
                                        vw1, vw2, vw3, vw4, vw5, vw6, vw7, vw8, vw9, vw10, vw11, vw12, vw13, vw14 };
        }

        public void LoadContent()
        {
            messageFont = mainGame.Content.Load<SpriteFont>("TimesNewRoman");

            // load the sound
            revealStairsSound = mainGame.Content.Load<SoundEffect>("reveal");


            Texture2D darwinTex = mainGame.Content.Load<Texture2D>("DarwinPic/Darwin");
            Texture2D darwinUpTex = mainGame.Content.Load<Texture2D>("DarwinPic/DarwinUp");
            Texture2D darwinDownTex = mainGame.Content.Load<Texture2D>("DarwinPic/Darwin");
            Texture2D darwinRightTex = mainGame.Content.Load<Texture2D>("DarwinPic/DarwinRight");
            Texture2D darwinLeftTex = mainGame.Content.Load<Texture2D>("DarwinPic/DarwinLeft");
            Texture2D zombieDarwinTex = mainGame.Content.Load<Texture2D>("DarwinPic/ZombieDarwin");
            Texture2D deadDarwinTex = mainGame.Content.Load<Texture2D>("DarwinPic/DeadDarwin");

            //load a sparkly overlay for the stairs
            sparkleTex = mainGame.Content.Load<Texture2D>("sparkle");

            darwin.LoadContent(graphics.GraphicsDevice, darwinUpTex, darwinDownTex, 
                darwinRightTex, darwinLeftTex, zombieDarwinTex,deadDarwinTex);

            gameOverTexture = mainGame.Content.Load<Texture2D>("gameover");
            gameWinTexture = mainGame.Content.Load<Texture2D>("gamewin");

            stairs.LoadContent(mainGame.Content.Load<Texture2D>("StaticPic/stairsUp"),
                mainGame.Content.Load<Texture2D>("StaticPic/stairsDown"), "Down");

            board.LoadContent(mainGame.Content.Load<Texture2D>("StaticPic/Level3/metal_tile_light"));
            board.LoadBackgroundContent(mainGame.Content.Load<Texture2D>("StaticPic/Level3/side_wall_yellow"));

            wallTex = mainGame.Content.Load<Texture2D>("StaticPic/Level3/side_wall_yellow");

            zombieMessage.LoadContent(mainGame.Content.Load<Texture2D>("messageBox"));
            darwinMessage.LoadContent(mainGame.Content.Load<Texture2D>("messageBox"));

            gameStart.LoadContent(mainGame.Content.Load<Texture2D>("LevelFive"));
            zTime.LoadContent(mainGame.Content.Load<Texture2D>("humanities_bar"));
            potion.LoadContent(mainGame.Content.Load<Texture2D>("StaticPic/potion"));

            foreach (Box b in boxes)
            {
                b.LoadContent(mainGame.Content.Load<Texture2D>("box"));
            }

            foreach (Vortex v in vortexes)
            {
                v.LoadContent(mainGame.Content.Load<Texture2D>("hole"));
            }

            pattern.LoadContent(mainGame.Content.Load<Texture2D>("boxPattern"));

            Texture2D snakeTexture = mainGame.Content.Load<Texture2D>("ZombiePic/snake_strip");
            snake.LoadContent(snakeTexture);
            snake2.LoadContent(snakeTexture);
            snake3.LoadContent(snakeTexture);
            snake4.LoadContent(snakeTexture);

            Texture2D pyroZombieTex = mainGame.Content.Load<Texture2D>("ZombiePic/FlamethrowerZombie");

            //second sprite is for when the flame is flaming
            northZombie.LoadContent(pyroZombieTex, pyroZombieTex);
            southZombie.LoadContent(pyroZombieTex, pyroZombieTex);
            eastZombie.LoadContent(pyroZombieTex, pyroZombieTex);
            westZombie.LoadContent(pyroZombieTex, pyroZombieTex);
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
                        // deals with logic for showing the player that they died
                        if (playDeathSound)
                        {
                            revealStairsSound.Play();
                            playDeathSound = false;
                        }

                        fellDownCounter++;
                        if (fellDownPit && darwin.destination.Width>0 && darwin.destination.Height>0 && fellDownCounter>5)
                        {
                            darwin.destination.Width -= (int)((float)board.getSquareWidth() * 0.1);
                            darwin.destination.Height -= (int)((float)board.getSquareLength() * 0.1);
                            fellDownCounter = 0;
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

            darwinMessage.pointToSquare(darwin.X, darwin.Y, board);

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
                foreach (Flame flame in flames)
                {
                    this.checkForFlameDeath(flame, darwin);
                }

                darwin.Update(gameTime, ks, board, darwin.X, darwin.Y);
            }
            
            stairs.Update(gameTime, darwin);
            potion.Update(gameTime, ks, darwin, zTime);

            foreach (Box b in boxes)
            {
                b.Update(gameTime, ks, darwin);
            }

            foreach (Vortex v in vortexes)
            {
                v.Update(gameTime, ks);
                // maybe update this so that boxes can't go in holes
                foreach (Box b in boxes)
                {
                    v.CollisionWithBO(b, board);
                }
            }

            northZombie.Update(darwin);
            southZombie.Update(darwin);
            eastZombie.Update(darwin);
            westZombie.Update(darwin);

            if (!darwin.isZombie())
            {
                if (!northZombie.isPatrolling())
                {
                    if (darwin.X == northZombie.X)
                    {
                        flames.AddLast(new Flame(board, northZombie.X, northZombie.Y + 1));
                        flames.AddLast(new Flame(board, northZombie.X, northZombie.Y + 2));
                        flames.AddLast(new Flame(board, northZombie.X, northZombie.Y + 3));
                    }
                }
                else if (!southZombie.isPatrolling())
                {
                    if (darwin.X == southZombie.X)
                    {
                        flames.AddLast(new Flame(board, southZombie.X, southZombie.Y - 1));
                        flames.AddLast(new Flame(board, southZombie.X, southZombie.Y - 2));
                        flames.AddLast(new Flame(board, southZombie.X, southZombie.Y - 3));
                    }
                }
                else if (!eastZombie.isPatrolling())
                {
                    if (darwin.Y == eastZombie.Y)
                    {
                        flames.AddLast(new Flame(board, eastZombie.X - 1, eastZombie.Y));
                        flames.AddLast(new Flame(board, eastZombie.X - 2, eastZombie.Y));
                        flames.AddLast(new Flame(board, eastZombie.X - 3, eastZombie.Y));
                    }
                }
                else if (!westZombie.isPatrolling())
                {
                    if (darwin.Y == westZombie.Y)
                    {
                        flames.AddLast(new Flame(board, westZombie.X + 1, westZombie.Y));
                        flames.AddLast(new Flame(board, westZombie.X + 2, westZombie.Y));
                        flames.AddLast(new Flame(board, westZombie.X + 3, westZombie.Y));
                    }
                }
            }
                        

            foreach (Flame flame in flames)
            {
                flame.Update();

                if (!flame.isAlive())
                {
                    //flames.Remove(flame);
                }
            }

            if (darwin.isDarwinAlive())
            {
                if (snake.isZombieAlive())
                {
                    updateSnakeCollision(snake, darwin, gameTime);
                }
                if (snake2.isZombieAlive())
                {
                    updateSnakeCollision(snake2, darwin, gameTime);
                }
                if (snake3.isZombieAlive())
                {
                    updateSnakeCollision(snake3, darwin, gameTime);
                }
                if (snake4.isZombieAlive())
                {
                    updateSnakeCollision(snake4, darwin, gameTime);
                }
            }

            foreach (Vortex v in vortexes)
            {
                if (darwin.isOnTop(v))
                {
                    fellDownPit = true;
                    gameOver = true;
                }
            }

            checkForGameWin();
            checkForSwitchToLevelSix();

            if (ks.IsKeyDown(Keys.H) && messageModeCounter > 10)
            {
                messageMode = true;
                messageModeCounter = 0;
            }
            messageModeCounter++;
        }

        private void checkForFlameDeath(Flame flame, Darwin darwin)
        {
            if(darwin.isOnTop(flame))
            {
                this.gameOver = true;
            }
        }

        private void updateSnakeCollision(Snake snake, Darwin darwin, GameTime gameTime)
        {
            if (!snake.isSnakeInPit())
            {
                snake.setZombieAlive(false);
            }
            else
            {
                snake.Update(gameTime, darwin, flames);

                if (snake.lineOfSight & snake.allowedToWalk)
                {
                    if (snake.lineOfSightDirection.Equals(LegendOfDarwin.GameObject.Snake.Direction.Up))
                    {
                        checkForDarwinAboveSnake(snake, darwin);
                    }
                    if (snake.lineOfSightDirection.Equals(LegendOfDarwin.GameObject.Snake.Direction.Down))
                    {
                        checkForDarwinBelowSnake(snake, darwin);
                    }
                    if (snake.lineOfSightDirection.Equals(LegendOfDarwin.GameObject.Snake.Direction.Right))
                    {
                        checkForDarwinRightOfSnake(snake, darwin);
                    }
                    if (snake.lineOfSightDirection.Equals(LegendOfDarwin.GameObject.Snake.Direction.Left))
                    {
                        checkForDarwinLeftOfSnake(snake, darwin);
                    }
                }
            }
        }

        private void checkForDarwinAboveSnake(Snake snake, Darwin darwin)
        {
            if (snake.isDarwinDirectlyAboveSnake(darwin) && board.isGridPositionOpen(darwin.X, darwin.Y - 1))
            {
                snake.pushDarwinUp(darwin);
            }
            else if (snake.isDarwinDirectlyAboveSnake(darwin) && !board.isGridPositionOpen(darwin.X, darwin.Y - 1))
            {
                snake.backOffDown();
            }
            else
            {
                if (board.isGridPositionOpen(snake.X, snake.Y - 1))
                {
                    snake.MoveUp();
                }
                else
                {
                    snake.backOffDown();
                }
            }
        }

        private void checkForDarwinBelowSnake(Snake snake, Darwin darwin)
        {
            if (snake.isDarwinDirectlyBelowSnake(darwin) && board.isGridPositionOpen(darwin.X, darwin.Y + 1))
            {
                snake.pushDarwinDown(darwin);
            }
            else if (snake.isDarwinDirectlyBelowSnake(darwin) && !board.isGridPositionOpen(darwin.X, darwin.Y + 1))
            {
                snake.backOffUp();
            }
            else
            {
                if (board.isGridPositionOpen(snake.X, snake.Y + 1))
                {
                    snake.MoveDown();
                }
                else
                {
                    snake.backOffUp();
                }
            }
        }

        private void checkForDarwinRightOfSnake(Snake snake, Darwin darwin)
        {
            if (snake.isDarwinDirectlyRightOfSnake(darwin) && board.isGridPositionOpen(darwin.X + 1, darwin.Y))
            {
                snake.pushDarwinRight(darwin);
            }
            else if (snake.isDarwinDirectlyRightOfSnake(darwin) && !board.isGridPositionOpen(darwin.X + 1, darwin.Y))
            {
                snake.backOffLeft();
            }
            else
            {
                if (board.isGridPositionOpen(snake.X + 1, snake.Y))
                {
                    snake.MoveRight();
                }
                else
                {
                    snake.backOffLeft();
                }
            }
        }

        private void checkForDarwinLeftOfSnake(Snake snake, Darwin darwin)
        {
            if (snake.isDarwinDirectlyLeftOfSnake(darwin) && board.isGridPositionOpen(darwin.X - 1, darwin.Y))
            {
                snake.pushDarwinLeft(darwin);
            }
            else if(snake.isDarwinDirectlyLeftOfSnake(darwin) && !board.isGridPositionOpen(darwin.X - 1, darwin.Y))
            {
                snake.backOffRight();
            }
            else
            {
                if (board.isGridPositionOpen(snake.X - 1, snake.Y))
                {
                    snake.MoveLeft();
                }
                else
                {
                    snake.backOffRight();
                }
            }
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
                darwin.setGridPosition(6, 18);

                gameOver = false;
                gameWin = false;

                board.setGridPositionOpen(potion);

                potion.setGridPosition(27, 5);

                potion.reset();
                darwin.setHuman();
                darwin.setDarwinAlive();
                gameOverCounter = 0;
                gameState.setState(GameState.state.Level);
                playSound = true;
                playDeathSound = true;
                //MediaPlayer.Stop();
                //MediaPlayer.Play(song);

                northZombie.setGridPosition(15, 3);
                northZombie.setCurrentPatrolPoint(new Vector2(27, 3));
                northZombie.setNextPatrolPoint(new Vector2(5, 3));
                northZombie.patrolling = true;

                southZombie.setGridPosition(15, 20);
                southZombie.setCurrentPatrolPoint(new Vector2(5, 20));
                southZombie.setNextPatrolPoint(new Vector2(27, 20));
                southZombie.patrolling = true;
                
                eastZombie.setGridPosition(29, 11);
                eastZombie.setCurrentPatrolPoint(new Vector2(29, 17));
                eastZombie.setNextPatrolPoint(new Vector2(29, 6));
                eastZombie.patrolling = true;

                westZombie.setGridPosition(3, 11);
                westZombie.setCurrentPatrolPoint(new Vector2(3, 6));
                westZombie.setNextPatrolPoint(new Vector2(3, 17));
                westZombie.patrolling = true;

                flames = new LinkedList<Flame>();

                board.setGridPositionOpen(snake);
                board.setGridPositionOpen(snake2);
                board.setGridPositionOpen(snake3);
                board.setGridPositionOpen(snake4);

                snake.setGridPosition(10, 9);
                snake2.setGridPosition(8, 15);
                snake3.setGridPosition(21, 9);
                snake4.setGridPosition(21, 15);

                // reset the boxes
                resetBoxes();

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
            if (myZombie.isOnTop(darwin))
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
            // if the pattern is complete, show the stairs and win
            if (pattern.isComplete(board, boxes))
            {
                Console.Out.WriteLine("thinks the pattern is complete");
                if (darwin.isOnTop(stairs))
                {
                    gameWin = true;
                }
            }
        }

        private void checkForSwitchToLevelSix()
        {
            if (gameWin)
            {
                mainGame.setCurLevel(Game1.LevelState.Level6);
                mainGame.setZTimeLevel(zTime, Game1.LevelState.Level6);

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

            pattern.Draw(spriteBatch);

            foreach (Vortex v in vortexes)
            {
                v.Draw(spriteBatch);
            }

            darwin.Draw(spriteBatch);

            zTime.Draw(spriteBatch);
            potion.Draw(spriteBatch);

            snake.Draw(spriteBatch);
            snake2.Draw(spriteBatch);
            snake3.Draw(spriteBatch);
            snake4.Draw(spriteBatch);

            northZombie.Draw(spriteBatch);
            southZombie.Draw(spriteBatch);
            eastZombie.Draw(spriteBatch);
            westZombie.Draw(spriteBatch);

            foreach (Flame flame in flames)
            {
                flame.Draw(spriteBatch, mainGame.Content.Load<Texture2D>("flame"));
            }

            if (messageMode)
            {
                zombieMessage.Draw(spriteBatch, messageFont);
                darwinMessage.Draw(spriteBatch, messageFont);
            }

            foreach (BasicObject a in walls)
            {
                spriteBatch.Draw(wallTex, board.getPosition(a.X, a.Y), Color.White);
            }

            foreach (Box b in boxes)
            {
                b.Draw(spriteBatch);
            }

            // only show the stairs after the pattern is complete
            if (pattern.isComplete(board, boxes))
            {
                if (pattern.shouldSparkle())
                {
                    //PUT THE SOUND HERE
                    if (playSound)
                    {
                        revealStairsSound.Play();
                        playSound = false;
                    }
                    Console.Out.WriteLine("here");

                    stairs.Draw(spriteBatch);
                    Rectangle rect = stairs.destination;
                    Rectangle source = new Rectangle(0, 0, 100, 100);
                    if (pattern.getSparkleCount() >= 0 && pattern.getSparkleCount() < 25)
                    {
                        rect.Height = 100;
                        rect.Width = 100;
                        rect.X -= 30;
                        rect.Y -= 50;

                        spriteBatch.Draw(sparkleTex, rect, source, Color.White);
                    }
                    else if (pattern.getSparkleCount() >= 25 && pattern.getSparkleCount() < 50)
                    {
                        rect.Height = 100;
                        rect.Width = 100;
                        rect.X -= 30;
                        rect.Y -= 50;

                        source.X = 100;

                        spriteBatch.Draw(sparkleTex, rect, source, Color.White);
                    }
                    else if (pattern.getSparkleCount() >= 50)
                    {
                        rect.Height = 100;
                        rect.Width = 100;
                        rect.X -= 30;
                        rect.Y -= 50;

                        source.X = 200;

                        spriteBatch.Draw(sparkleTex, rect, source, Color.White);
                    }
                }
                else
                {
                    stairs.Draw(spriteBatch);
                }
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

    }
}




