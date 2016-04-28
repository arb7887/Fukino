using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GreatGame
{
    class Map
    {
        private List<Wall> walls;
        private Texture2D wallTexture;
        private CapturePoint cp;
        private List<Vector2> playerSpawnPoints;
        private List<Vector2> enemySpawnPoints;
        private BoundingBox playerSpawn;

        public CapturePoint CP { get { return cp; } set { cp = value; } }
        public Texture2D WallTexture { set { wallTexture = value; } }
        public List<Wall> Walls { get { return walls; } }
        public List<Vector2> PlayerSpawnPoints { get { return playerSpawnPoints; } }
        public List<Vector2> EnemySpawnPoints{ get { return enemySpawnPoints; } }
        public BoundingBox PlayerSpawn { get { return playerSpawn; } }

        public Map()
        {
            walls = new List<Wall>();
        }

        public void LoadMap(String fileName)
        {
            using (Stream inputStream = File.OpenRead(fileName))
            using (StreamReader input = new StreamReader(inputStream))
            {
                int numWalls = int.Parse(input.ReadLine());

                for (int i = 0; i < numWalls; i++)
                {
                    input.ReadLine();
                    int minX = int.Parse(input.ReadLine());
                    int minY = int.Parse(input.ReadLine());

                    int maxX = int.Parse(input.ReadLine());
                    int maxY = int.Parse(input.ReadLine());

                    Wall newWall = new Wall(new BoundingBox(new Vector3(minX, minY, 0), new Vector3(maxX, maxY, 0)), wallTexture);
                    walls.Add(newWall);
                }
            }
            playerSpawn = new BoundingBox(new Vector3(100, 100, 0), new Vector3(600, 500, 0));
            playerSpawnPoints = new List<Vector2>(){ new Vector2(150, 200), new Vector2(225, 200) , new Vector2(300, 200) , new Vector2(375, 200),
                new Vector2(450, 200), new Vector2(525, 200)};
            enemySpawnPoints = new List<Vector2>() { new Vector2(4850, 200), new Vector2(4925, 200), new Vector2(5000, 200) , new Vector2(5075, 200),
                new Vector2(5150, 200), new Vector2(5225, 200)};
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Wall w in walls)
            {
                w.Draw(sb);
            }
            cp.Draw(sb);
        }

        public void checkCapturing(Unit u)
        {
            cp.checkContest(u);
        }
    }
}
