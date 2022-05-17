

namespace OpenFrameworkV3.Graph
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Text;

    public class GraphData
    {
        private List<int> barData;
        public string Icon { get; set; }
        public int Percentage { get; set; }
        public GraphType DataType { get; set; }

        public ReadOnlyCollection<int> BarData
        {
            get
            {
                if (this.barData == null)
                {
                    this.barData = new List<int>();
                }

                return new ReadOnlyCollection<int>(this.barData);
            }
        }

        public void AddBarData(int data)
        {
            if (this.barData == null)
            {
                this.barData = new List<int>();
            }

            this.barData.Add(data);
        }
        public string Render(int infoBoxType)
        {
            switch (this.DataType)
            {
                case GraphType.Pie:
                    return RenderPie(infoBoxType);
                case GraphType.Bar:
                    return RenderBar(infoBoxType);
                case GraphType.Icon:
                    return RenderIcon(infoBoxType);
                default:
                    return string.Empty;
            }
        }

        private string RenderPie(int infoBoxType)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                @"
                        <div class=""infobox-progress"">
                            <div class=""easy-pie-chart percentage"" data-percent=""{0}"" data-size=""{1}"" style=""height: {1}px; width: {1}px; line-height: {1}px;"">
                                <span class=""percent"">{0}</span>%
		                        <canvas height=""{1}"" width=""{1}""></canvas>
                            </div>
                        </div>",
                this.Percentage,
                infoBoxType == 0 ? "46" : "39");
        }

        private string RenderBar(int infoBoxType)
        {
            var data = new StringBuilder();
            bool first = true;
            foreach (int value in this.barData)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    data.Append(",");
                }

                data.Append(value);
            }
            return string.Format(
                CultureInfo.InvariantCulture,
                @"
                        <div class=""infobox-chart"">
                            <span class=""sparkline"" data-values=""{0}"">
                                <canvas width=""44"" height=""33"" style=""display: inline-block; width: 44px; height: 33px; vertical-align: top;""></canvas>
                            </span>
                        </div>",
                    data);
        }

        private string RenderIcon(int infoBox)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                @"
                    <div class=""infobox-icon"">
                        <i class=""ace-icon {0}""></i>
                    </div>",
                this.Icon);
        }
    }
}
