// --------------------------------
// <copyright file="ImageFormat.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFramework.ItemManager.List
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Newtonsoft.Json;

    /// <summary>Implements image format in list</summary>
    public sealed class ImageFormat
    {
        /// <summary>Extension permitted for image</summary>
        [JsonProperty("Extensions")]
        private readonly object[] extensions;

        /// <summary>Gets or sets the maximum length of image</summary>
        [JsonProperty("MaxLength")]
        public long MaxLength { get; set; }

        /// <summary>Gets or sets the image size</summary>
        [JsonProperty("Size")]
        public int Size { get; set; }

        /// <summary>Gets or sets the image miniature</summary>
        [JsonProperty("Thumbnail")]
        public int Thumbnail { get; set; }

        /// <summary>Gets or sets the image of "no image"</summary>
        [JsonProperty("NoImage")]
        public string NoImage { get; set; }

        /// <summary>Gets the lists of permitted extensions</summary>
        [JsonIgnore]
        public ReadOnlyCollection<string> Extensions
        {
            get
            {
                var res = new List<string>();
                for (int x = 0; x < this.extensions.Length; x++)
                {
                    res.Add(this.extensions[x] as string);
                }

                return new ReadOnlyCollection<string>(res);
            }
        }
    }
}