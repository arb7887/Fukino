﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GreatGame
{
    class FileInput<T>
    {
        // Fields
        private String fileName;
        private List<Unit> unitList;

        public string FileName { get { return this.fileName; } }

        /// <summary>
        /// This class will have a list of whatever kind of object 
        /// is passed in with the parameters
        /// </summary>
        /// <param name="fileName"></param>
        public FileInput(String fileName)
        {
            this.fileName = fileName;
            this.unitList = new List<Unit>();
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
                    int speed = (int.Parse(input.ReadLine()));
                    int range = (int.Parse(input.ReadLine()));
                    int dps = (int.Parse(input.ReadLine()));

                    // Make a new unit and add it to the list
                    Unit newUnit = new Unit(name, health, speed, range, dps);
                    unitList.Add(newUnit);
                }
            }
        }
    }
}
