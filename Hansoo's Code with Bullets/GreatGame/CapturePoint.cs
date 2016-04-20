using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GreatGame
{

    enum Teams { Player, Enemy, Neutral };

    class CapturePoint
    {
        private BoundingBox bounds;
        private Teams controller;
        private int playerTimer;
        private int enemyTimer;
        private int captureTime;
        private bool contested;
        private Teams winner;
        private Teams contester;
        private Texture2D pointTexture;

        public bool Contested { get { return contested; } set { contested = value; } }
        public Teams Contester { get { return contester; } set { contester = value; } }
        public BoundingBox Bounds { get { return bounds; } set { bounds = value; } }
        public Teams Controller { get { return controller; } }
        public Teams Winner { get { return winner; } }
        public int PlayerTimer { get { return playerTimer; } }
        public int EnemyTimer { get { return enemyTimer; } }
        public int CaptureTime { get { return captureTime; } }

        public CapturePoint(int x, int y, int width, int height, Texture2D pt)
        {
            playerTimer = 180;
            enemyTimer = 180;
            captureTime = 5;
            controller = Teams.Neutral;
            winner = Teams.Neutral;
            bounds = new BoundingBox(new Vector3(x,y,0), new Vector3(x+width,y+height,0));
            contested = false;
            contester = Teams.Neutral;
            pointTexture = pt;
        }

        public void Count()
        {
            if (controller == Teams.Neutral)
                return;

            if (controller == Teams.Player)
                playerTimer -= 1;
            else if (controller == Teams.Enemy)
                enemyTimer -= 1;

            if (contested)
                captureTime -= 1;
            else
                captureTime = 5;

            if (playerTimer <= 0 && !contested)
            {
                winner = Teams.Player;
            }
            else if (enemyTimer <= 0 && !contested)
            {
                winner = Teams.Enemy;
            }

            if (captureTime <= 0)
            {
                controller = contester;
            }
        }

        /// <summary>
        /// returns true if unit is withing bounds, sets internal contest logic based on unit Team
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public bool checkContest(Unit u)
        {
            if (u.Bounds.Intersects(bounds))
            {
                if (u.Team == Teams.Enemy && controller == Teams.Player)
                {
                    contested = true;
                    contester = Teams.Enemy;
                }
                if (u.Team == Teams.Player && controller == Teams.Enemy)
                {
                    contested = true;
                    contester = Teams.Player;
                }
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(pointTexture, new Rectangle((int)(bounds.Min.X), (int)(bounds.Min.Y), (int)(bounds.Max.X - bounds.Min.X), (int)(bounds.Max.Y - bounds.Min.Y)), Color.White);
        }
    }
}
