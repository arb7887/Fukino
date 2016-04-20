using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GreatGame
{

    enum Control { Player, Enemy, Neutral };

    class ControlPoint
    {
        private Rectangle bounds;
        private Control controller;
        private int playerTimer;
        private int enemyTimer;
        private int captureTime;
        private bool contested;
        private Control winner;
        private Control contester;

        public bool Contested { get { return contested; } set { contested = value; } }
        public Control Contester { get { return contester; } set { contester = value; } }
        public Rectangle Bounds { get { return bounds; } set { bounds = value; } }
        public Control Controller { get { return controller; } }
        public Control Winner { get { return winner; } }
        public int PlayerTimer { get { return playerTimer; } }
        public int EnemyTimer { get { return enemyTimer; } }
        public int CaptureTime { get { return captureTime; } }

        public ControlPoint(int x, int y, int width, int height)
        {
            playerTimer = 180;
            enemyTimer = 180;
            captureTime = 5;
            controller = Control.Neutral;
            winner = Control.Neutral;
            bounds = new Rectangle(x, y, width, height);
            contested = false;
            contester = Control.Neutral;
        }

        public void Count()
        {
            if (controller == Control.Neutral)
                return;

            if (controller == Control.Player)
                playerTimer -= 1;
            else if (controller == Control.Enemy)
                enemyTimer -= 1;

            if (contested)
                captureTime -= 1;
            else
                captureTime = 5;

            if(playerTimer <= 0 && !contested)
            {
                winner = Control.Player;
            }
            else if(enemyTimer <= 0 && !contested)
            {
                winner = Control.Enemy;
            }

            if(captureTime <= 0)
            {
                controller = contester;
            }
        }
    }
}
