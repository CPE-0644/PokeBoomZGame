using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokeBoomz
{
    class HomeGame : Game 
    {
        private GraphicsDeviceManager graphics;
        public bool started = false;
        public MouseState lastMouseState, mouseState;
        private Obstacle startgame,newgame,howtoplay,exit,cursor;
        private SpriteBatch spriteBatch;
        private Rectangle block;
        public bool start = false;
        public bool h2p = false;
        public HomeGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "WELCOME";
        }
     


        protected override void Initialize()
        {
            base.Initialize();
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 900;
            graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            startgame = new Obstacle(graphics,spriteBatch);
            startgame.LoadContent(Content,"home/bg00",0,0);
            newgame = new Obstacle(graphics,spriteBatch);
            newgame.LoadContent(Content, "home/ng", 450, 535);
           howtoplay = new Obstacle(graphics,spriteBatch);
            howtoplay.LoadContent(Content, "home/h2p", 450, 630);
             exit = new Obstacle(graphics,spriteBatch);
             exit.LoadContent(Content, "home/exit", 450, 730);
            //cursor = new Obstacle(graphics,spriteBatch);
            //cursor.LoadContent(Content, "home/select_cursor",0 ,0);
        }

        protected override void Update(GameTime gameTime)
        {
            lastMouseState = mouseState;
            mouseState = Mouse.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();

            }
            newgame.togglePosition();
            howtoplay.togglePosition();
            exit.togglePosition();
            check();
            newgame.Update();
            howtoplay.Update();
            exit.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            startgame.Draw();
            newgame.Draw();
            howtoplay.Draw();
            exit.Draw();
            spriteBatch.End();
            base.Draw(gameTime);
           
        }

        public void check()
        {
            if (newgame.isClick())
            {
                start = true;
                this.Exit();
            }

            if (howtoplay.isClick())
            {
                h2p = true;
                this.Exit();
            }

            if (exit.isClick())
            {
                start = false;
                h2p = false;
                this.Exit();
            }
           
        }
    }
    }

