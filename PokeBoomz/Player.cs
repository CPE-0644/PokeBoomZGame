﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokeBoomz
{
    class Player
    {
        private GraphicsDeviceManager graphicsDevice;
        private SpriteBatch spriteBatch;
        public KeyboardState keyboardState, lastKeyboardState;

        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame, frameCounter;
        private int totalFrames;

        private int scaleSize = 50;

        public bool isThrown = false;
        public double positionX = 0, positionY = 0;
        public float angle = 45, power;
        public Vector2 position, infoPosition, arrowPosition;
        private Texture2D texture, arrow;
        public bool turnLeft, turnRight, startTurn = true, myTurn;

        private float alpha = 1.0f;
        private float pokeballRotation = 0.0f, rotation = 0.0f, arrowRotation = 0.0f;
        private Vector2 origin, arrowOrigin;
        private float scale = 2.0f, arrowScale = 0.8f;
        private SpriteEffects spriteEffect;
        private float zDepth = 0.1f;
        public Rectangle sourceRectangle, destinationRectangle, arrowRectangle;
        private PowerRectangle powerRectangle;

        public int remainedPokeball, pokeballToUse;

        public double accerelate = 1;
        private const float degtorad = (float) Math.PI / 180;

        public List<Pokeball> Pokeballs = new List<Pokeball>(3);
        public List<Pokemon> OwnPokemons = new List<Pokemon>(6);
        public LinkedList<Item> Items = new LinkedList<Item>();

        public Item item1, item2, item3;

        public Pokeball pokeball1, pokeball2, pokeball3;

        public Player(GraphicsDeviceManager graphicsDevice, SpriteBatch spriteBatch)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;
        }

        public void LoadContent(ContentManager Content, String imag, int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
            texture = Content.Load<Texture2D>(imag);

            arrow = Content.Load<Texture2D>("arrow");
            arrowRectangle = new Rectangle(0, 0, arrow.Width, arrow.Height);
            arrowOrigin = new Vector2((arrowPosition.X + arrowRectangle.Width) / 2,
                (arrowPosition.Y + arrowRectangle.Height) / 2);

            origin = new Vector2(0, 0);

            powerRectangle = new PowerRectangle(graphicsDevice, spriteBatch);
            powerRectangle.position.X = 100;
            powerRectangle.position.Y = 100;
            powerRectangle.LoadContent();
            pokeball1 = new Pokeball(graphicsDevice, spriteBatch);
            pokeball2 = new Pokeball(graphicsDevice, spriteBatch);
            pokeball3 = new Pokeball(graphicsDevice, spriteBatch);
            pokeball1.LoadContent(Content, "pokeball/pokeball1");
            pokeball2.LoadContent(Content, "pokeball/pokeball2");
            pokeball3.LoadContent(Content, "pokeball/pokeball3");
            Pokeballs.Add(pokeball1);
            Pokeballs.Add(pokeball2);
            Pokeballs.Add(pokeball3);

            item1 = new Item(graphicsDevice, spriteBatch);
            item2 = new Item(graphicsDevice, spriteBatch);
            item3 = new Item(graphicsDevice, spriteBatch);
            item1.LoadContent(Content, "item/change_wind", 100, 1000);
            item2.LoadContent(Content, "item/freeze", 210, 1000);
            item3.LoadContent(Content, "item/double_size", 320, 1000);

            Items.AddLast(item1);
            Items.AddLast(item2);
            Items.AddLast(item3);

            remainedPokeball = Pokeballs.Count();
        }

        public void Update()
        {
            frameCounter++;
            if (frameCounter % 5 == 0)
            {
                currentFrame++;
                frameCounter = 0;
            }
            if (currentFrame == totalFrames)
                currentFrame = 0;
            keyboardState = Keyboard.GetState();

            infoPosition = new Vector2((float)positionX , (float)positionY - 10);
            

            if (myTurn)
            {
                PlayerMoving();

                if (remainedPokeball > 0)
                {
                    Pokeballs[pokeballToUse].Update(isThrown, pokeballRotation, angle, power, turnLeft);
                    if (!isThrown)
                    {
                        AngleMoving();
                        Powering();
                        PokeballPostitioning();
                    }
                    else
                    {
                        isPokeballCollapsed();
                    }
                }
                else resetTurn();

                foreach (var item in Items)
                {
                    item.Update();
                }

                powerRectangle.Update();
            }
        }

        public void isPokeballCollapsed()
        {
            Pokeball thisPokeball = Pokeballs[pokeballToUse];
            if (thisPokeball.isCollapsed())
            {
                pokeballToUse++;
                remainedPokeball--;
                resetTurn();
            }
        }

        public void resetTurn()
        {
            startTurn = false;
            power = 0;
            powerRectangle.powerGage.X = 0;
            isThrown = false;
            PlayerMoving();
        }

        public void PokeballPostitioning()
        {
            Pokeballs[pokeballToUse].position.X = (float) this.positionX + 10;
            Pokeballs[pokeballToUse].position.Y = (float) this.positionY + texture.Height / 2;
            arrowPosition.X = Pokeballs[pokeballToUse].position.X - Pokeballs[pokeballToUse].drawRec.Width/3
                + (angle * pokeballRotation)/5;
            arrowPosition.Y = Pokeballs[pokeballToUse].position.Y - pokeballRotation * 20;
            if (turnRight)
            {
                Pokeballs[pokeballToUse].position.X = (float) this.positionX + texture.Width/3 + scaleSize - 10;
                arrowPosition.X = Pokeballs[pokeballToUse].position.X + Pokeballs[pokeballToUse].drawRec.Width / 3
                                  + (angle * pokeballRotation) / 5;
                arrowPosition.Y = Pokeballs[pokeballToUse].position.Y + (pokeballRotation) * 20;
            }
        }


        public void Powering()
        {
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                power += 1;
                powerRectangle.powerGage.X = power;
                if (power > 100) power = 0;
//                Console.WriteLine(power);
            }
            if (keyboardState.IsKeyUp(Keys.Space) && power != 0)
            {
                isThrown = true;
                
//                Console.WriteLine("POWERING!!");
            }
        }

        public void PlayerMoving()
        {
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                positionX -= 2;
                turnRight = false;
                turnLeft = true;
                spriteEffect = SpriteEffects.None;
                arrowRotation = angle * degtorad;
                pokeballRotation = angle * degtorad;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                positionX += 2;
                turnLeft = false;
                turnRight = true;
                spriteEffect = SpriteEffects.FlipHorizontally;
                arrowRotation = -angle * degtorad;
                pokeballRotation = -angle * degtorad;
            }
            if (this.positionX < 0)
            {
                this.positionX = 0;
            }
            if (this.positionX > PokeBoomzGame.displayWidth)
            {
                this.positionX = PokeBoomzGame.displayWidth;
            }
        }

        public void AngleMoving()
        {
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                angle += 1;
                if (turnLeft)
                {arrowRotation = angle * degtorad;
                    pokeballRotation = angle * degtorad;
                }
                else if (turnRight)
                {
                    arrowRotation = -angle * degtorad;
                    pokeballRotation = -angle * degtorad;
                }
                if (angle >= 90) angle = 90;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                angle -= 1;

                if (turnLeft)
                {
                    arrowRotation = angle * degtorad;
                    pokeballRotation = angle * degtorad;

                }
                else if (turnRight)
                {
                    arrowRotation = -angle * degtorad;
                    pokeballRotation = -angle * degtorad;

                }
//                Console.WriteLine(angle);
                if (angle <= 0) angle = 0;
            }

            Console.WriteLine(angle);
        }

        public void Draw()
        {
            int width = texture.Width / Columns;
            int height = texture.Height / Rows;
            int row = (int) ((float) currentFrame / (float) Columns);
            int column = currentFrame % Columns;

            if (myTurn)
            {
               foreach (var item in Items)
            {
                item.Draw();
            } 
            }
            

            sourceRectangle = new Rectangle(width * column, height * row, width, height);
            destinationRectangle =
                new Rectangle((int) positionX, (int) positionY, width + scaleSize, height + scaleSize);

            if (remainedPokeball > 0)
            {
                spriteBatch.Draw(arrow, arrowPosition, arrowRectangle, Color.White * alpha, arrowRotation,
                arrowOrigin, arrowScale, spriteEffect, zDepth);
            }
            
            
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White,
                rotation, origin, spriteEffect, zDepth);
            //            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White);
            
            powerRectangle.Draw();
        }
    }
}