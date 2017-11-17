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
    class UI_HowToPlay : Game
    {
        private GraphicsDeviceManager graphics;
        public bool started = false;
        public MouseState lastMouseState, mouseState;
        private Obstacle bg_h2p, play, back;
        private SpriteBatch spriteBatch;
        private Rectangle block;
        public bool start = false, home= false;
        public UI_HowToPlay()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "How To Play";
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
            bg_h2p = new Obstacle(graphics, spriteBatch);
            bg_h2p.LoadContent(Content, "h2p/bg_h2p", 0, 0);
            play = new Obstacle(graphics, spriteBatch);
            play.LoadContent(Content, "h2p/play", 900, 700);
            back = new Obstacle(graphics, spriteBatch);
            back.LoadContent(Content, "h2p/back", 50, 700);
        }

        protected override void Update(GameTime gameTime)
        {

            Userpressmenu();
            play.Update();
            back.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            bg_h2p.Draw();
            play.Draw();
            back.Draw();
            spriteBatch.End();
            base.Draw(gameTime);

        }
        public void Userpressmenu()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            if (play.isClick())
            {
                Console.WriteLine("START");

                start = true;
                this.Exit();
            }
            if (back.isClick())
            {
                Console.WriteLine("HOME");
                home = true;
                this.Exit();
            }
}
    }
}
