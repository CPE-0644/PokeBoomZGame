using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using PokeBoomZ;

namespace PokeBoomz
{
    public class PokeBoomzGame : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        public static Texture2D dummyTexture;
        private SpriteBatch spriteBatch;
        private KeyboardState keyboardState, lastKeyboardState;
        public Random rand = new Random();

        public static double gravity = 0.5f;
        private int player_role = 1;
        public int millisec = 0, sec = 0, turnTimeLimit = 15;
        public bool timesUp = false;
        public static float windPower;
        public float windDirection;


        public static int displayWidth = 1600, displayHeight = 1200;
        public static int groundY = 895;

        private float alpha = 1.0f, textAlpha = 1.0f, windAlpha = 1.0f;
        private float rotation = 0.0f, textRotation = 0.0f;

        private Vector2 origin = new Vector2(0, 0),
            timerPosition = new Vector2(1450, 1100),
            anglePosition = new Vector2(100, 1100),
            windPosition = new Vector2(750, 50),
            turnPosition = new Vector2(700, 1000),
            powerBarPosition = new Vector2(0, 1050);

        public Rectangle windRectangle, powerBarRectangle;

        public Texture2D card1, card2, wind, powerBar;
        public Vector2 card1Position = new Vector2(10, 10), card2Position = new Vector2(10, 250);
        public float p1Trans, p2Trans;

        private float scale = 2.0f, textScale = 0.8f, timerScale = 2.0f, windScale = 1.0f;
        private SpriteEffects spriteEffect, textEffects, windEffects;
        private float zDepth = 0.1f;

        private Pokemon pokemon1, pokemon2, pokemon3, pokemon4, pokemon5, hittedPokemon;
        private Player player1, player2;
        private Obstacle ground1, block1, bg1;

        private Dictionary<int, Player> Players = new Dictionary<int, Player>();
        private LinkedList<Pokemon> Pokemons = new LinkedList<Pokemon>();
        private LinkedList<Obstacle> Obstacles = new LinkedList<Obstacle>();

        private SpriteFont spriteFont;

        public PokeBoomzGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            IsMouseVisible = true;
            //            displayWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            //            displayHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            displayWidth = 1600;
            displayHeight = 1200;
            graphics.PreferredBackBufferWidth = displayWidth;
            graphics.PreferredBackBufferHeight = displayHeight;

            graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.OrangeRed });

            spriteFont = Content.Load<SpriteFont>("Arial");

            bg1 = new Obstacle(graphics, spriteBatch);
            bg1.LoadContent(Content, "map/bg01", 0, 0);

            ground1 = new Obstacle(graphics, spriteBatch);
            ground1.LoadContent(Content, "map/ground01", 0, groundY);

            block1 = new Obstacle(graphics, spriteBatch);
            block1.LoadContent(Content, "map/block01", 100, 100);

            card1 = Content.Load<Texture2D>("player_card1");
            card2 = Content.Load<Texture2D>("player_card2");

            wind = Content.Load<Texture2D>("wind");
            windRectangle = new Rectangle(0, 0, wind.Width, wind.Height);
            //  Obstacles.AddLast(block1);

            powerBar = Content.Load<Texture2D>("powerBar");
            powerBarRectangle = new Rectangle(0, 0, powerBar.Width, powerBar.Height);

            pokemon1 = new Pokemon(graphics, spriteBatch);
            pokemon1.initStat("Bird", "fly", 10, 10, 10);
            pokemon1.LoadContent(Content, "pokemon/bird_21", 21, 1);

            pokemon2 = new Pokemon(graphics, spriteBatch);
            pokemon2.initStat("Lucia", "fly", 10, 10, 10);
            pokemon2.LoadContent(Content, "pokemon/lucia_18", 19, 1);

            pokemon3 = new Pokemon(graphics, spriteBatch);
            pokemon3.initStat("Ho-oh", "fly", 10, 10, 10);
            pokemon3.LoadContent(Content, "pokemon/ho-on_15", 15, 1);

            pokemon4 = new Pokemon(graphics, spriteBatch);
            pokemon4.initStat("Crab", "normal", 10, 10, 10);
            pokemon4.LoadContent(Content, "pokemon/crab_20", 20, 1);

            pokemon5 = new Pokemon(graphics, spriteBatch);
            pokemon5.initStat("Monkey", "normal", 10, 10, 10);
            pokemon5.LoadContent(Content, "pokemon/monkey_22", 22, 1);

            player1 = new Player(graphics, spriteBatch);
            player1.LoadContent(Content, "character/female/stand",
                "character/female/walk", "character/female/throw",
                "character/female/lose", 1, 3);

            player2 = new Player(graphics, spriteBatch);
            player2.LoadContent(Content, "character/male/stand",
                "character/male/walk", "character/male/throw",
                "character/male/lose", 1, 3);

            player1.myTurn = true;
            player_role = 1;
            p1Trans = 0.8f;
            p2Trans = 0.4f;
            player1.turnRound = 1;
            player2.turnRound = 0;

            Players.Add(1, player1);
            Players.Add(2, player2);

            Pokemons.AddLast(pokemon1);
            Pokemons.AddLast(pokemon2);
            Pokemons.AddLast(pokemon3);
            Pokemons.AddLast(pokemon4);
            Pokemons.AddLast(pokemon5);


            if (Math.Floor(rand.NextDouble() * 2) == 0)
            {
                windDirection = -1;
                windEffects = SpriteEffects.FlipHorizontally;

            }
            else windDirection = 1;
            windPower = windDirection * (float)Math.Floor(rand.NextDouble() * 5.0f);
        }

       

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
             millisec++;
          
           if (millisec % 60 == 0)
            {
                millisec = 0;
                sec++;
            }


           Timeupandswapplayer();
           remainball();          
           Pokemons.Remove(hittedPokemon);
           Typeofpokemon();
           Activetyofplayer();
                        
            if (Pokemons.Count() == 0)
            {
                Exit();
            }
            base.Update(gameTime);
        }

        public void Timeupandswapplayer()
        {
            if (turnTimeLimit - sec == 0)
            {
                timesUp = true;
                sec = 0;
                swapPlayer();
            }
        }

        public void remainball()
        {
            if (player1.remainedPokeball == 0 && player2.remainedPokeball == 0)
            {
                if (player1.allCP > player2.allCP)
                {
                    player2.lose = true;
                }
                else
                {
                    player1.lose = true; 
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            bg1.Draw();
            ground1.Draw();

            foreach (var player in Players)
            {
                player.Value.Draw();
                spriteBatch.DrawString(spriteFont, "Player " + player.Key, player.Value.infoPosition,
                    Color.White * alpha, textRotation, origin, textScale, textEffects, zDepth);
                if (player.Value.remainedPokeball > 0)
                    player.Value.Pokeballs[player.Value.pokeballToUse].Draw();

            }
            foreach (var pokemon in Pokemons)
            {
                pokemon.Draw();
                spriteBatch.DrawString(spriteFont,
                    pokemon.name + "\nCp " + pokemon.cp + "\nChance " + pokemon.chance + "%",
                    pokemon.infoPosition, Color.White * alpha, textRotation, origin, textScale, textEffects, zDepth);
            }
            foreach (var obstacle in Obstacles)
            {
                obstacle.Draw();
            }
            spriteBatch.Draw(powerBar, powerBarPosition, powerBarRectangle, Color.White * alpha,
                rotation, origin, windScale, spriteEffect, zDepth);

            foreach (var player in Players)
            {
                if (player.Value.myTurn)
                {
                    spriteBatch.DrawString(spriteFont,
                        player.Value.angle + "",
                        anglePosition, Color.White * alpha, textRotation, origin, timerScale, textEffects, zDepth);

                    spriteBatch.DrawString(spriteFont,
                        "Turn " + player.Value.turnRound,
                        turnPosition, Color.White * alpha, textRotation, origin, timerScale, textEffects, zDepth);
                }

            }

            spriteBatch.Draw(card1, card1Position, Color.White * p1Trans);
            spriteBatch.Draw(card2, card2Position, Color.White * p2Trans);

            spriteBatch.Draw(wind, windPosition, windRectangle, Color.White * windAlpha,
                rotation, origin, windScale, windEffects, zDepth);

            spriteBatch.DrawString(spriteFont,
                Math.Abs(windPower).ToString(),
                new Vector2(windPosition.X + 50, windPosition.Y + 20),
                Color.White * alpha, textRotation, origin, timerScale, textEffects, zDepth);

            spriteBatch.DrawString(spriteFont,
                "" + (turnTimeLimit - sec),
                timerPosition, Color.White * alpha, textRotation, origin, timerScale, textEffects, zDepth);

            spriteBatch.End();

            //             spriteBatch.Draw(arrow, location, sourceRectangle, Color.White, angle, origin, 1.0f, SpriteEffects.None, 1);
            base.Draw(gameTime);
        }
        protected override void UnloadContent()
        {
        }

        public void Activetyofplayer()
        {
            foreach (var player in Players)
            {
                //                if (player_role == player.Key) 
                Player thisPlayer = player.Value;
                if (thisPlayer.positionY + thisPlayer.destinationRectangle.Height < ground1.position.Y)
                {
                    thisPlayer.positionY += thisPlayer.accerelate;
                    thisPlayer.accerelate += PokeBoomzGame.gravity;
                }
                else
                {
                    thisPlayer.positionY = ground1.position.Y - thisPlayer.destinationRectangle.Height;
                    thisPlayer.accerelate = 0;
                }
                if (thisPlayer.myTurn && thisPlayer.remainedPokeball == 0)
                {
                    swapPlayer();
                }
                player.Value.Update();

                foreach (var pokemon in Pokemons)
                {
                    if (thisPlayer.remainedPokeball > 0 && thisPlayer.isThrown)
                    {
                        Pokeball thisPokeball = thisPlayer.Pokeballs[thisPlayer.pokeballToUse];
                        if (thisPokeball.initialized && thisPokeball.isThrown)
                        {
                            Vector2 lPokeball = new Vector2(thisPokeball.positionCenter.X,
                                thisPokeball.positionCenter.Y);
                            if (lPokeball.X >= pokemon.left && lPokeball.X <= pokemon.right &&
                                lPokeball.Y >= pokemon.top && lPokeball.Y <= pokemon.btm)
                            {
                                thisPokeball.hitPokemon = true;
                                pokemon.isHit = true;
                                hittedPokemon = pokemon;
                                thisPlayer.allCP += pokemon.cp;
                            }
                        }
                    }
                }
            }
        }

        public void Typeofpokemon()
        {
            foreach (var pokemon in Pokemons)
            {
                if (pokemon.type == "fly")
                {
                    if (pokemon.positionY + pokemon.destinationRectangle.Height > ground1.position.Y)
                    {
                        pokemon.positionY = ground1.position.Y - pokemon.destinationRectangle.Height;
                    }
                }
                else
                {
                    if (pokemon.positionY + pokemon.destinationRectangle.Height < ground1.position.Y)
                    {
                        pokemon.positionY += pokemon.accerelate;
                        pokemon.accerelate += PokeBoomzGame.gravity;
                    }
                    else
                    {
                        pokemon.positionY = ground1.position.Y - pokemon.destinationRectangle.Height;
                        pokemon.accerelate = 0;
                    }
                }
                pokemon.Update();
            } 
          

        }

        public void swapPlayer()
        {
            if (player_role == 1 && player2.remainedPokeball > 0)
            {
                player1.myTurn = false;
                player2.myTurn = true;
                player_role = 2;
                p1Trans = 0.4f;
                p2Trans = 0.8f;
                player2.turnRound++;
            }
            else if (player_role == 2 && player1.remainedPokeball > 0)
            {
                player1.myTurn = true;
                player2.myTurn = false;
                player_role = 1;
                p2Trans = 0.4f;
                p1Trans = 0.8f;
                player1.turnRound++;
            }
            Console.WriteLine(player_role);
            sec = 0;
            if (Math.Floor(rand.NextDouble() * 2) == 0)
            {
                windDirection = -1;
                windEffects = SpriteEffects.FlipHorizontally;
            }
            else
            {
                windDirection = 1;
                windEffects = SpriteEffects.None;
            }
            windPower = windDirection * (float)Math.Floor(rand.NextDouble() * 5.0f);
            timesUp = false;
        }
    }
}