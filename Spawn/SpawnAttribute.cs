using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horn_War_II.Spawn
{
    /// <summary>
    /// Used to define editor parameters to make GameObjects spawnable in it
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    class SpawnAttribute : Attribute
    {
        /// <summary>
        /// Preview image for editor
        /// </summary>
        public string PreviewImage { get; set; }

        /// <summary>
        /// Multiple instances allowed?
        /// </summary>
        public bool AllowMultiple { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="PreviewImage">Path to preview image for the spawn menu</param>
        /// <param name="AllowMultiple">Allow multiple isntances in the game?</param>
        public SpawnAttribute(string PreviewImage = "Images/NoPreview", bool AllowMultiple = true) : base()
        {
            this.PreviewImage = PreviewImage;
            this.AllowMultiple = AllowMultiple;
        }
    }
}