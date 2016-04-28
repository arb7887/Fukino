using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

namespace GreatGame
{
    class FileInput
    {
        // Fields
        private String fileName;
        private String fileNameTexture;
        private List<Unit> unitList;
        private List<String> textureList;

        public string FileName { get { return this.fileName; } }
        public String FileNameTexture { get { return this.fileNameTexture; } }

        public List<Unit> UnitList { get { return this.unitList; } }
        public List<String> TextureList { get { return this.textureList; } }

        public int ListCount { get { return unitList.Count; } }

        /// <summary>
        /// This class will have a list of whatever kind of object 
        /// is passed in with the parameters
        /// </summary>
        /// <param name="fileName"></param>
        public FileInput(String fileName, String fileNameTextures)
        {
            this.fileName = fileName;
            this.fileNameTexture = fileNameTextures;
            this.unitList = new List<Unit>();
            this.textureList = new List<String>();
        }

        // Read file method
        public void LoadUnit()
        {
            using (Stream inputStream = File.OpenRead(fileName))
            using (StreamReader input = new StreamReader(inputStream))
            {
                //Get number of units and the num of attributes
                int numData = (int.Parse(input.ReadLine()));
                int numAttributes = (int.Parse(input.ReadLine()));

                for (int i = 0; i < numData; i++)
                {
                    // Read in the data
                    string name = input.ReadLine();
                    int health = (int.Parse(input.ReadLine()));
                    double speed = (double.Parse(input.ReadLine()));
                    int range = (int.Parse(input.ReadLine()));
                    int dps = (int.Parse(input.ReadLine()));
                    double rateOfFire = (double.Parse(input.ReadLine()));

                    // Make a new unit and add it to the list
                    Unit newUnit = new Unit(name, health, speed, range, dps, rateOfFire, i);
                    unitList.Add(newUnit);
                }
            }
        }

        public void LoadTextures()
        {
            using (Stream inputStream = File.OpenRead(fileNameTexture))
            using (StreamReader input = new StreamReader(inputStream))
            {
                int numData = (int.Parse(input.ReadLine()));

                for(int i = 0; i < numData; i++)
                {
                    String textureName = input.ReadLine();
                    textureList.Add(textureName);
                }
            }
        }

        /// <summary>
        /// Overrides the ToString in order to return a string that has all of the names of the units...
        /// This is kinda useless a little bit but we shall see
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            String names = "";
            for(int i = 0; i < unitList.Count; i++)
            {
                names += " " + unitList[i].Name;
            }
            return names;
        }
    }
}
