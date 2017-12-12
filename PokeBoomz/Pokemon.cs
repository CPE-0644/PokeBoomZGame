using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PokeBoomz
{
    public class Pokemon
    {
        public string name;
        public float cp;
        public Texture2D texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame, frameCounter;
        private int totalFrames;
        private int scaleSize = 50;
        private GraphicsDeviceManager graphicsDevice;
        private SpriteBatch spriteBatch;
        public Rectangle sourceRectangle, destinationRectangle;
        public string type;
        static Random rand = new Random();
        private float alpha = 1.0f, textAlpha = 1.0f;
        private float rotation = 0.0f, textRotation = 0.0f;
        private Vector2 origin = new Vector2(0, 0);
        private float scale = 2.0f, textScale = 0.8f;
        private SpriteEffects spriteEffect, textEffects;
        private float zDepth = 0.1f;
        public float left, top, right, btm;
        public double positionX, positionY;
        public double accerelate = 1;
        public bool isHit = false;     
        public Vector2 infoPosition;
        public float movingCond = 0;
        public int caughtByBallNO = 0;
        public Pokemon()
        {
        }
        public Pokemon(GraphicsDeviceManager graphicsDevice, SpriteBatch spriteBatch)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;
//            Console.WriteLine(positionX);
        }

        public void initStat(string name, string type, float cp)
        {
            this.name = name;
            this.type = type;
            this.cp = cp;
        }
        public void initSide()
        {
            top = (float) positionY;
            left = (float) positionX;
            right = (float) positionX + destinationRectangle.Width;
            btm = (float) positionY + destinationRectangle.Height;
        }     
        public void LoadContent(ContentManager Content, String imag, int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
            texture = Content.Load<Texture2D>(imag);

            positionX = rand.NextDouble() * PokeBoomzGame.displayWidth;
            positionY = rand.NextDouble() * PokeBoomzGame.groundY;
        }

        
        public void Update()
        {           
            infoPosition = new Vector2((float) positionX + 10, (float) positionY + destinationRectangle.Height);
            FrameUpdate();
            if(!isHit) MovingByTypes();         
            edgeChecking();
            initSide();
        }
      
        
        public void Draw()
        {
            int width = texture.Width / Columns;
            int height = texture.Height / Rows;
            int row = (int) ((float) currentFrame / (float) Columns);
            int column = currentFrame % Columns;

            sourceRectangle = new Rectangle(width * column, height * row, width, height);
            destinationRectangle =
                new Rectangle((int) positionX, (int) positionY, width + scaleSize, height + scaleSize);

            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White,
                rotation, origin, spriteEffect, zDepth);

            //            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White);
        }

        public void MovingByTypes()
        {
            if (type == "fly")
            {
                if (currentFrame < totalFrames / 2)
                {
                    moveLeft();
                    if (movingCond == 0)
                        moveUp();
                    else if (movingCond == 1) moveDown();
                }
                else
                {
                    moveRight();
                    if (movingCond == 0)
                        moveUp();
                    else if (movingCond == 1) moveDown();
                }
            }
            else
            {
                if (movingCond == 0)
                    moveLeft();
                else if (movingCond == 1) moveRight();
            }
        }

        public void FrameUpdate()
        {
            frameCounter++;
            if (frameCounter % 10 == 0)
            {
                currentFrame++;
                frameCounter = 0;
            }
            if (currentFrame == totalFrames)
            {
                currentFrame = 0;
                movingCond = (float)Math.Floor(rand.NextDouble() * 3);
            }
        }
        public void edgeChecking()
        {
            if (this.positionX < 0)
            {
                this.positionX = 0;
            }
            if (this.positionX + destinationRectangle.Width > PokeBoomzGame.displayWidth)
            {
                this.positionX = PokeBoomzGame.displayWidth - destinationRectangle.Width;
            }
            if (this.positionY < 0) this.positionY = 0;
        }
        public void moveUp()
        {
            this.positionY -= 1 * PokeBoomzGame.moveFactor;
        }

        public void moveDown()
        {
            this.positionY += 1 * PokeBoomzGame.moveFactor;
        }

        public void moveLeft()
        {
            this.positionX -= 1 * PokeBoomzGame.moveFactor;
            spriteEffect = SpriteEffects.None;
        }

        public void moveRight()
        {
            this.positionX += 1 * PokeBoomzGame.moveFactor;
            spriteEffect = SpriteEffects.FlipHorizontally;
        }
    }

}