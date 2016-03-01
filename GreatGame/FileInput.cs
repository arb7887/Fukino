using System;
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
        private List<T> list;

        public string FileName { get { return this.fileName; } }

        /// <summary>
        /// This class will have a list of whatever kind of object 
        /// is passed in with the parameters
        /// </summary>
        /// <param name="fileName"></param>
        public FileInput(String fileName)
        {
            this.fileName = fileName;
            this.list = new List<T>();
        }

        // Read file method
        public void Load()
        {

            using (Stream inputStream = File.OpenRead(fileName))
            using (BinaryReader input = new BinaryReader(inputStream))
            {
                //Get number of units and the num of attributes
                int numData = input.ReadInt32();
                int numAttributes = input.ReadInt32();

                for (int i = 0; i < numData; i++)
                {
                    // Read in the data
                    string name = input.ReadString();
                    int health = input.ReadInt32();
                    int speed = input.ReadInt32();
                    int range = input.ReadInt32();
                    int dps = input.ReadInt32();

                    // Make a new unit and add it to the list
                    Unit newUnit = new Unit(name, health, speed, range, dps);
                    list.Add(newUnit);
                }
            }
        }
    }
}
