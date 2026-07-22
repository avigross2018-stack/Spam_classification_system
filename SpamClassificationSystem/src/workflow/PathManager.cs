using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneFleetDataProcessing.src
{
    //A department that manages the routes
    public class PathManager
    {
        private string _basePath;

        //A function that returns the location of the project
        public PathManager()
        {
            _basePath = Directory.GetCurrentDirectory();
        }
        //A function that returns the position of the correct input.
        public string getInputPath(string filename)
        {
            return Path.Combine(_basePath, "input", filename);
        }
        //A function that returns the location of the output dir
        private string getOutputDirPath()
        {
            return (Path.Combine(_basePath, "output"));
        }
        //A function that returns the location of the output file.
        public string getOutputPath(string filename)
        {
            if (!Directory.Exists(getOutputDirPath()))
            {
                Directory.CreateDirectory(getOutputDirPath());
            }
            return (Path.Combine(_basePath, "output", filename));
        }
    }
}
