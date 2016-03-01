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
    }
}
