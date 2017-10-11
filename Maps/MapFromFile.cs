using Horn_War_II.Scenes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horn_War_II.Maps
{
    /// <summary>
    /// Uses a map definition that is stored in a xml file
    /// </summary>
    class MapFromFile : Map
    {
        public MapFromFile(FileInfo MapFile, GameScene Scene) : base(Scene)
        {
            if (!MapFile.Exists) throw new Exception(string.Format("Map definition file '{0}' not found", MapFile.FullName));


        }
    }
}
