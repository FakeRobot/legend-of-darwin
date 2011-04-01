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

        Texture2D main;
        KeyboardState ks;
        // for keeping track of what level player is on
        public enum LevelState { Start, Level1, Level2, Level3, Level4, Level5, Level6 };
        LevelState curLevel;

        Level1 level1;
        Level2 level2;
        Level3 level3;
        Level.Level4 level4;
        Level.Level5 level5;
        Level.Level6 level6;

        public Game1()
        {
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

            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            main = Content.Load<Texture2D>("startScreen");

            level1.spriteBatch = spriteBatch;
            level1.song = Content.Load<Song>("thriller");
            level1.LoadContent();

            level2.spriteBatch = spriteBatch;
            level2.song = Content.Load<Song>("thriller");
            level2.LoadContent();

            level3.spriteBatch = spriteBatch;
            level3.song = Content.Load<Song>("thriller");
            level3.LoadContent();

            level4.spriteBatch = spriteBatch;
            level4.song = Content.Load<Song>("thriller");
            level4.LoadContent();

            level5.spriteBatch = spriteBatch;
            level5.song = Content.Load<Song>("thriller");
            level5.LoadContent();

            level6.spriteBatch = spriteBatch;
            level6.song = Content.Load<Song>("thriller");
            level6.LoadContent();
        }

        private void InitializeGraphics()
        {
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 1300;
            graphics.ApplyChanges();
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            if (curLevel == LevelState.Start)
            {
                if (Keyboard.GetState().IsKeyUp(Keys.Enter) && ks.IsKeyDown(Keys.Enter))
                {
                    setCurLevel(LevelState.Level1);
                }
                else
                {
                    ks = Keyboard.GetState();
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
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (curLevel == LevelState.Start)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(main, new Rectangle(0,0,graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferWidth), Color.White);
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
            base.Draw(gameTime);
        }

        public void setCurLevel(LevelState myLevel) 
        {
            curLevel = myLevel;
        }

    }
}
