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
    class PowerRectangle
    {
        private GraphicsDeviceManager graphicsDevice;
        private SpriteBatch spriteBatch;

        private Rectangle rectangle;
        private Texture2D dummyTexture;
        
        public MouseState mouseState, lastMouseState;

        public Vector2 position, powerGage;

        public PowerRectangle(GraphicsDeviceManager graphicsDevice, SpriteBatch spriteBatch)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;
        }

        public void LoadContent()
        {
//            rectangle = new Rectangle((int) position.X, (int) position.Y, (int) powerGage.X, (int) powerGage.Y);
            rectangle = new Rectangle(300, 1085, 0, 80);
            dummyTexture = PokeBoomzGame.dummyTexture;
        }

        public void Update()
        {
            rectangle.Width = (int) powerGage.X * 10;
        }

        public void Draw()
        {
            spriteBatch.Draw(dummyTexture, rectangle, Color.White);
        }
    }
}