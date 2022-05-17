// --------------------------------
// <copyright file="ImageData.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Reports
{
    using Newtonsoft.Json;

    public class ImageData
    {
        [JsonProperty("File")]
        public string File { get; set; }

        [JsonProperty("Page")]
        public int Page { get; set; }

        [JsonProperty("X")]
        public int X { get; set; }

        [JsonProperty("Y")]
        public int Y { get; set; }

        [JsonProperty("Width")]
        private int width;

        [JsonProperty("Height")]
        private int height;

        [JsonIgnore]
        public int Width
        {
            get
            {
                if(this.width < 1) { return 120; }
                return this.width;
            }
        }

        [JsonIgnore]
        public int Height
        {
            get
            {
                if (this.height < 1) { return 120; }
                return this.height;
            }
        }

        public void SetDimensions(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }
}
