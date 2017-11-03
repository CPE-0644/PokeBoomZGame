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
    class SpecialRectangle : Game
    {
        private Rectangle rectangle;
        private Texture2D dummyTexture;
        private int accelerate = 0;
        public MouseState mouseState, lastMouseState;
        private int angel = 45;

        public SpecialRectangle(GraphicsDevice graphicsDevice, int X, int Y)
        {
            rectangle = new Rectangle(X, Y, 100, 100);
            dummyTexture = new Texture2D(graphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] {Color.White});
        }

        public void Update()
        {
            if (rectangle.Y + rectangle.Height < PokeBoomzGame.displayHeight / 2)
            {
                rectangle.Y += accelerate;
                accelerate += 1;
            }
            else
            {
                rectangle.Y = PokeBoomzGame.displayHeight / 2 - rectangle.Height - 1;
                accelerate = 0;
            }
            lastMouseState = mouseState;
            mouseState = Mouse.GetState();
            if (lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
                if (mouseState.X >= rectangle.X && mouseState.X <= rectangle.X + rectangle.Width &&
                    mouseState.Y >= rectangle.Y && mouseState.Y <= rectangle.Y + rectangle.Height)
                    Console.WriteLine("THIS IS IN THE BOX");
                else Console.WriteLine("OUTSITE THE BOX");
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                rectangle.Y = rectangle.Y - 10;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                rectangle.X = rectangle.X + 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                rectangle.X = rectangle.X - 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                angel += 1;
                if (angel >= 90) angel = 90;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                angel -= 1;
                if (angel <= 0) angel = 0;
            }
           
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(dummyTexture, rectangle, Color.White);
            
        }
    }
}