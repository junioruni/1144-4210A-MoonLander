using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using JuniorUni.MoonLander.Model;

namespace JuniorUni.MoonLander
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SchiffBildContainer schiffBildContainer;
        SchiffBildContainer schiffBild2Container;
        BlockBildContainer blockBildContainer;
        BlockBildContainer blockBild2Container;

        SpriteFont bildSchrift;
        Texture2D platzhalterTextur;

        #region Laden von Daten und setzen von Variablen
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }
        #endregion

        protected override void LoadContent()
        {
            // Zeichenbrett, dass wir zur Ausgabe der 2D-Grafiken verwenden.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            schiffBildContainer = new SchiffBildContainer(spriteBatch, graphics, Content, 100, 100, new SteuerungsConfig() { Links = Keys.A, Rauf = Keys.W, Rechts = Keys.D, Runter = Keys.S  });
            schiffBild2Container = new SchiffBildContainer(spriteBatch, graphics, Content, 600, 100, new SteuerungsConfig() { Links = Keys.Left, Rauf = Keys.Up, Rechts = Keys.Right, Runter = Keys.Down });
            blockBildContainer = new BlockBildContainer(spriteBatch, Content, 300, 300);
            blockBild2Container = new BlockBildContainer(spriteBatch, Content, 500, 200);

            #region TODO
            bildSchrift = Content.Load<SpriteFont>("Courier New");

            platzhalterTextur = new Texture2D(GraphicsDevice, 1, 1);
            platzhalterTextur.SetData(new Color[] { Color.White });
            #endregion
        }

        #region Aufraeumen nach dem Ende
        protected override void UnloadContent()
        {
        }
        #endregion

        protected override void Update(GameTime gameTime)
        {
            // Tastatur fragen, was gerade alles gedrückt wird.
            var tastaturStatus = Keyboard.GetState();


            // Ich erstelle eine Liste von Rechtecken und packe die Koordinaten meiner beiden Blöcke in die Liste, um dem SchiffsContainer sie zum Vergleichen zu übergeben.
            List<Rectangle> listeAllerBloecke = new List<Rectangle>();
            listeAllerBloecke.Add(blockBildContainer.HoleRahmenVonBlockBild());
            listeAllerBloecke.Add(blockBild2Container.HoleRahmenVonBlockBild());


            // Update der tastatur an den Schiffs-Container schicken.
            schiffBildContainer.ReagiereAufTasten(tastaturStatus, listeAllerBloecke);
            schiffBild2Container.ReagiereAufTasten(tastaturStatus, listeAllerBloecke);


            if (schiffBildContainer.HoleRahmenVonSchiffBild().Intersects(schiffBild2Container.HoleRahmenVonSchiffBild()))
            {
                schiffBildContainer.Explodiere();
                schiffBild2Container.Explodiere();
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // Dem Schiffs-Container sagen, dass er das Schiff auf die Leinwand malen soll.
            schiffBildContainer.ZeichneSchiff();
            schiffBild2Container.ZeichneSchiff();
            blockBildContainer.ZeichneBlock();
            blockBild2Container.ZeichneBlock();
            
            #region TODO
            // TODO: Textausgabe
            spriteBatch.Begin();
            spriteBatch.DrawString(bildSchrift, "Punkte: " + schiffBildContainer.Landungen + " | " + schiffBild2Container.Landungen, new Vector2(20, 20), Color.White);

            // TODO: Rechteck malen
            spriteBatch.Draw(platzhalterTextur, new Rectangle(3, 30, 10, 304), Color.White);
            spriteBatch.Draw(platzhalterTextur, new Rectangle(5, 32, 6, (int)Math.Floor(300 / 100 * schiffBildContainer.Treibstoff)), Color.Green);

            spriteBatch.Draw(platzhalterTextur, new Rectangle(graphics.PreferredBackBufferWidth - 20, 30, 10, 304), Color.White);
            spriteBatch.Draw(platzhalterTextur, new Rectangle(graphics.PreferredBackBufferWidth - 18, 32, 6, (int)Math.Floor(300 / 100 * schiffBild2Container.Treibstoff)), Color.Green);
            spriteBatch.End();
            #endregion

            base.Draw(gameTime);
        }
    }
}
