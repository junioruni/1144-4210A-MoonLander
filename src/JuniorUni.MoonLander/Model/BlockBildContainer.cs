using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JuniorUni.MoonLander.Model
{
    public class BlockBildContainer
    {
        // Zeichenbrett hierher übergeben, um den Block darauf zu malen.
        SpriteBatch spriteBatch;

        // Variable für das Schiff.
        Texture2D blockBild;

        // Startposition des Blocks festlegen.
        int x = 300;
        int y = 300;

        public BlockBildContainer(SpriteBatch sb, ContentManager c, int startX, int startY)
        {
            x = startX;
            y = startY;

            spriteBatch = sb;

            // Laden des Blocks aus dem "Content"-Lager.
            blockBild = c.Load<Texture2D>("DirtBlock");
        }

        public BlockBildContainer(SpriteBatch sb, ContentManager c) : this(sb, c, 300, 300)
        {
        }

        public void ReagiereAufTasten(KeyboardState tastaturStatus)
        {
        }

        public void ZeichneBlock()
        {
            // Zeichenbox auf; Block draufkleben (an den gegebenen Koordinaten); Zeichenbox zu
            spriteBatch.Begin();
            spriteBatch.Draw(blockBild, new Vector2(x, y), Color.White);
            spriteBatch.End();
        }

        public Rectangle HoleRahmenVonBlockBild()
        {
            return new Rectangle(x, y, blockBild.Width, blockBild.Height);
        }
    }
}
