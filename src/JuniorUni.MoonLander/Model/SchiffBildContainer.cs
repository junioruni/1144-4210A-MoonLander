using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace JuniorUni.MoonLander.Model
{
    public class SchiffBildContainer
    {
        // Zeichenbrett hierher übergeben, um das Schiff darauf zu malen.
        SpriteBatch spriteBatch;

        // Variable für das Schiff.
        Texture2D schiffsBild;

        // Startposition des Schiffs festlegen.
        int x = 100;
        int y = 100;

        public SchiffBildContainer(SpriteBatch sb, ContentManager c)
        {
            spriteBatch = sb;

            // Laden des Schiffs aus dem "Content"-Lager.
            schiffsBild = c.Load<Texture2D>("Spaceship");
        }

        public void ReagiereAufTasten(KeyboardState tastaturStatus, Rectangle blockRectangle)
        {
            // Wir nehmen die aktuellen Koordinaten als Basis.
            var newX = x;
            var newY = y;

            // Spezielle Tasten einzeln abfragen und die Koordinaten des Schiffs entsprechend anpassen.
            if (tastaturStatus.IsKeyDown(Keys.Up))
                newY -= 5;
            if (tastaturStatus.IsKeyDown(Keys.Down))
                newY += 5;
            if (tastaturStatus.IsKeyDown(Keys.Left))
                newX -= 5;
            if (tastaturStatus.IsKeyDown(Keys.Right))
                newX += 5;

            // Immer einen kleineren Wert als die Steuerung auf y addieren, um eine Schwerkraft zu simulieren.
            newY += 3;
            
            if (HoleRahmenVonSchiffBild().Intersects(blockRectangle))
            {
                newX = 100;
                newY = 100;
            }

            // Erst am Ende der Update Funktion ändern wir die aktuelle Position.
            x = newX;
            y = newY;
        }

        public void ZeichneSchiff()
        {
            // Zeichenbox auf; Schiff draufkleben (an den gegebenen Koordinaten); Zeichenbox zu
            spriteBatch.Begin();
            spriteBatch.Draw(schiffsBild, new Vector2(x, y), Color.White);
            spriteBatch.End();
        }

        public Rectangle HoleRahmenVonSchiffBild()
        {
            return new Rectangle(x, y, schiffsBild.Width, schiffsBild.Height);
        }
    }
}