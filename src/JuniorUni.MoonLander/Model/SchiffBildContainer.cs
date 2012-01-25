using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;

namespace JuniorUni.MoonLander.Model
{
    public class SteuerungsConfig
    {
        public Keys Rauf { get; set; }
        public Keys Runter { get; set; }
        public Keys Links { get; set; }
        public Keys Rechts { get; set; }
    }

    public class SchiffBildContainer
    {
        // Zeichenbrett hierher übergeben, um das Schiff darauf zu malen.
        SpriteBatch spriteBatch;

        // Variable für das Schiff.
        Texture2D schiffsBild;
        List<Texture2D> explosionTextures = new List<Texture2D>(); 

        // Startposition des Schiffs festlegen.
        int x = 100;
        int y = 100;
        int restartX = 0;
        int restartY = 0;

        int explosion = 0;
        int Explosion
        {
            get
            {
                return explosion;
            }
            set
            {
                if ((explosion != 0) && (value == 0))
                {
                    StarteNeu();
                }

                explosion = value;
            }
        }

        bool flying = true;
        public int Landungen { get; set; }
        public double Treibstoff = 100;

        SteuerungsConfig steuerungsConfig;
        GraphicsDeviceManager graphics;

        public SchiffBildContainer(SpriteBatch sb, GraphicsDeviceManager g, ContentManager c, int startX, int startY, SteuerungsConfig config)
        {
            x = startX;
            y = startY;
            restartX = startX;
            restartY = startY;
            steuerungsConfig = config;
            graphics = g;

            spriteBatch = sb;

            // Laden des Schiffs aus dem "Content"-Lager.
            schiffsBild = c.Load<Texture2D>("Spaceship");

            for (var i = 1; i <= 10; i++)
            {
                explosionTextures.Add(c.Load<Texture2D>("Explosion/Spaceship_Explosion" + i));
            }
        }

        public void ReagiereAufTasten(KeyboardState tastaturStatus, List<Rectangle> listeAllerBloecke)
        {
            // Wir nehmen die aktuellen Koordinaten als Basis.
            var newX = x;
            var newY = y;

            // Spezielle Tasten einzeln abfragen und die Koordinaten des Schiffs entsprechend anpassen.
            if (tastaturStatus.IsKeyDown(steuerungsConfig.Rauf))
            {
                newY -= 5;
                Treibstoff -= 0.3d;

                if (Treibstoff <= 0)
                {
                    Explodiere();
                }
            }
            if (tastaturStatus.IsKeyDown(steuerungsConfig.Runter))
                newY += 5;
            if (tastaturStatus.IsKeyDown(steuerungsConfig.Links))
                newX -= 5;
            if (tastaturStatus.IsKeyDown(steuerungsConfig.Rechts))
                newX += 5;

            // Immer einen kleineren Wert als die Steuerung auf y addieren, um eine Schwerkraft zu simulieren.
            newY += 3;


            // Schiffsposition speichern (damit das nicht immer wieder abgefragt wird).
            var aktuelleSchiffsPosition = HoleRahmenVonSchiffBild();

            // Eine Schleife über alle Positionen machen, an denen Blöcke sind.
            foreach (var einzelnerBlock in listeAllerBloecke)
            {
                // Schauen, ob das Schiff gerade mit einem Block kollidiert (ob sich die Rechtecke um die beiden Elemente überschneiden).
                if (aktuelleSchiffsPosition.Intersects(einzelnerBlock))
                {
                    // Der Code hier drin soll nur ausgeführt werden, wenn ich nicht die "hoch" Taste drücke - damit ist das Starten immer möglich.
                    if (!tastaturStatus.IsKeyDown(steuerungsConfig.Rauf))
                    {
                        // Wenn ich nicht zu tief im Block drin bin, dann behalte ich meine alte y-Position. Auf diesem Wege hält das Schiff an.
                        if (aktuelleSchiffsPosition.Bottom - 5 < einzelnerBlock.Top)
                        {
                            if (flying)
                                Landungen++;
                            flying = false;
                            Treibstoff = 100;

                            newY = y;
                        }
                        else
                        {
                            // Wenn ich zu tief im Block bin, während sie sich treffen, dann setze ich das Schiff zurück - es hat geknallt.
                            newX = x;
                            newY = y;

                            Explodiere();
                        }
                    }
                    else
                    {
                        flying = true;
                    }
                }
            }

            if (newY < -50) 
            {
                newY = graphics.PreferredBackBufferHeight + 50;
            }
            else if (newY > graphics.PreferredBackBufferHeight + 60)
            {
                newY = -50;
            }
            if (newX < -50)
            {
                newX = graphics.PreferredBackBufferWidth + 50;
            }
            else if (newX > graphics.PreferredBackBufferWidth + 60)
            {
                newX = -50;
            }

            // Erst am Ende der Update Funktion ändern wir die aktuelle Position.
            x = newX;
            y = newY;
        }

        public void Explodiere()
        {
            if (Explosion == 0)
            {
                Explosion = 1;

                Landungen -= 10;
            }
        }

        public void StarteNeu()
        {
            x = restartX;
            y = restartY;
            Treibstoff = 100;
        }

        public void ZeichneSchiff()
        {
            // Zeichenbox auf; Schiff draufkleben (an den gegebenen Koordinaten); Zeichenbox zu
            spriteBatch.Begin();

            if (Explosion == 0)
            {
                spriteBatch.Draw(schiffsBild, new Vector2(x, y), Color.White);
            }
            else
            {
                spriteBatch.Draw(explosionTextures[Explosion], new Vector2(x, y), Color.White);

                Explosion++;

                if (Explosion >= explosionTextures.Count) 
                    Explosion = 0;
            }

            spriteBatch.End();
        }

        public Rectangle HoleRahmenVonSchiffBild()
        {
            return new Rectangle(x, y, schiffsBild.Width, schiffsBild.Height);
        }
    }
}