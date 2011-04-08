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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D main, main2, end;
        KeyboardState ks, ks2, ksEnd;
        // for keeping track of what level player is on
        public enum LevelState { Start, Start2, Level1, Level2, Level3, Level4, Level5, Level6, End };
        LevelState curLevel;

        Level1 level1;
        Level2 level2;
        Level3 level3;
        Level.Level4 level4;
        Level.Level5 level5;
        Level.Level6 level6;

        Song song;

        public int DEATH_COUNTER;

        public Game1()
        {
            DEATH_COUNTER = 0;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            curLevel = LevelState.Start;

            level1 = new Level1(this);
            level2 = new Level2(this);
            level3 = new Level3(this);
            level4 = new Level.Level4(this);
            level5 = new Level.Level5(this);
            level6 = new Level.Level6(this);
        }

        public void setZTimeLevel(MenuObject.ZombieTime mytime,LevelState myLevel)
        {
            if (myLevel == LevelState.Level2) 
            {
                level2.setZTime(mytime);
            }
            else if (myLevel == LevelState.Level3)
            {
                level3.setZTime(mytime);
            }
            else if (myLevel == LevelState.Level4) 
            {
                level4.setZTime(mytime);
            }
            else if (myLevel == LevelState.Level5)
            {
                level5.setZTime(mytime);
            }
            else if (myLevel == LevelState.Level6)
            {
                level6.setZTime(mytime);
            }
        }

        protected override void Initialize()
        {
            
            level1.graphics = graphics;
            level2.graphics = graphics;
            level3.graphics = graphics;
            level4.graphics = graphics;
            level5.graphics = graphics;
            level6.graphics = graphics;

            InitializeGraphics();
            level1.Initialize();
            level2.Initialize();
            level3.Initialize();
            level4.Initialize();
            level5.Initialize();
            level6.Initialize();

            MediaPlayer.IsRepeating = true;

            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            main = Content.Load<Texture2D>("startScreen");
            main2 = Content.Load<Texture2D>("SplashScreens/Intro");
            end = Content.Load<Texture2D>("SplashScreens/Final");
            song = Content.Load<Song>("Thriller8bit");

            level1.spriteBatch = spriteBatch;
            level1.LoadContent();

            level2.spriteBatch = spriteBatch;
            level2.LoadContent();

            level3.spriteBatch = spriteBatch;
            level3.LoadContent();

            level4.spriteBatch = spriteBatch;
            level4.LoadContent();

            level5.spriteBatch = spriteBatch;
            level5.LoadContent();

            level6.spriteBatch = spriteBatch;
            level6.LoadContent();
        }

        private void InitializeGraphics()
        {
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1024;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            if (curLevel == LevelState.Start)
            {
                if (Keyboard.GetState().IsKeyUp(Keys.Enter) && ks.IsKeyDown(Keys.Enter))
                {
                    DEATH_COUNTER = 0;
                    setCurLevel(LevelState.Start2);
                    ks = Keyboard.GetState();
                }
                else
                {
                    ks = Keyboard.GetState();
                }
            }
            else if(curLevel == LevelState.Start2)
            {
                if (Keyboard.GetState().IsKeyUp(Keys.Enter) && ks2.IsKeyDown(Keys.Enter))
                {
                    MediaPlayer.Play(song);
                    setCurLevel(LevelState.Level1);
                    ks2 = Keyboard.GetState();
                }
                else
                {
                    ks2 = Keyboard.GetState();
                }
            }
            else if (curLevel == LevelState.Level1)
                level1.Update(gameTime);
            else if (curLevel == LevelState.Level2)
                level2.Update(gameTime);
            else if (curLevel == LevelState.Level3)
                level3.Update(gameTime);
            else if (curLevel == LevelState.Level4)
                level4.Update(gameTime);
            else if (curLevel == LevelState.Level5)
                level5.Update(gameTime);
            else if (curLevel == LevelState.Level6)
                level6.Update(gameTime);
            else if (curLevel == LevelState.End)
            {
                if (Keyboard.GetState().IsKeyUp(Keys.Enter) && ksEnd.IsKeyDown(Keys.Enter))
                {
                    setCurLevel(LevelState.Start);
                }
                else
                {
                    ksEnd = Keyboard.GetState();
                }

                this.DEATH_COUNTER = 0;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (curLevel == LevelState.Start)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(main, new Rectangle(0,0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                spriteBatch.End();
            }
            else if (curLevel == LevelState.Start2)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(main2, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                spriteBatch.End();
            }
            else if (curLevel == LevelState.Level1)
                level1.Draw(gameTime);
            else if (curLevel == LevelState.Level2)
                level2.Draw(gameTime);
            else if (curLevel == LevelState.Level3)
                level3.Draw(gameTime);
            else if (curLevel == LevelState.Level4)
                level4.Draw(gameTime);
            else if (curLevel == LevelState.Level5)
                level5.Draw(gameTime);
            else if (curLevel == LevelState.Level6)
                level6.Draw(gameTime);
            else if (curLevel == LevelState.End)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(end, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferWidth), Color.White);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        public void setCurLevel(LevelState myLevel) 
        {
            curLevel = myLevel;
        }

    }
}
