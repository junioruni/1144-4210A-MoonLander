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
            
            schiffBildContainer = new SchiffBildContainer(spriteBatch, Content);
            blockBildContainer = new BlockBildContainer(spriteBatch, Content);
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


            List<Rectangle> listeAllerBloecke = new List<Rectangle>();
            listeAllerBloecke.Add(blockBildContainer.HoleRahmenVonBlockBild());
            listeAllerBloecke.Add(blockBild2Container.HoleRahmenVonBlockBild());


            // Update der tastatur an den Schiffs-Container schicken.
            schiffBildContainer.ReagiereAufTasten(tastaturStatus, listeAllerBloecke);


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // Dem Schiffs-Container sagen, dass er das Schiff auf die Leinwand malen soll.
            schiffBildContainer.ZeichneSchiff();
            blockBildContainer.ZeichneBlock();
            blockBild2Container.ZeichneBlock();
            
            #region TODO
            // TODO: Textausgabe
            spriteBatch.Begin();
            spriteBatch.DrawString(bildSchrift, "Punkte: 0", new Vector2(20, 20), Color.White);

            // TODO: Rechteck malen
            spriteBatch.Draw(platzhalterTextur, new Rectangle(10, 10, 5, 300), Color.White);
            spriteBatch.End();
            #endregion

            base.Draw(gameTime);
        }
    }
}
