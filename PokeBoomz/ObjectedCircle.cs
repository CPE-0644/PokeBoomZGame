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
    class ObjectedCircle
    {
        private Rectangle rectangle;
        private Texture2D dummyTexture;
        private int accelerate = 0;
        public MouseState mouseState, lastMouseState;
        public int force = 0, _Y = 0, angel = 45;
        public double veloX = 0, veloY = 0;

        public ObjectedCircle(GraphicsDevice graphicsDevice, int X, int Y)
        {
            rectangle = new Rectangle(X, Y, 10, 10);
            dummyTexture = new Texture2D(graphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] {Color.White});
        }

        public void Update()
        {
//            rectangle.X = rectangle.X + Math.veloX;
//            rectangle.Y = this._Y + veloY;
            this._Y = rectangle.Y;
            if(veloY != 0 )
                veloY -= PokeBoomzGame.gravity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(dummyTexture, rectangle, Color.Red);
        }
    }
}