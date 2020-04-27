using System;
using System.ComponentModel;

namespace XMindAPI.Models
{
    /// <summary>
    /// XMindStructure defines the different types of diagrams that can be drawn. Its implemented through the AddCentralTopic() method.
    /// </summary>
    public enum XMindStructure
    {
        [Description("org.xmind.ui.fishbone.rightHeaded")]
        FishboneRightHeaded,
        [Description("org.xmind.ui.fishbone.leftHeaded")]
        FishboneLeftHeaded,
        [Description("org.xmind.ui.spreadsheet")]
        SpreadSheet,
        [Description("org.xmind.ui.map")]
        Map,
        [Description("org.xmind.ui.map.clockwise")]
        MapClockwise,
        [Description("org.xmind.ui.map.anticlockwise")]
        MapAntiClockwise,
        [Description("org.xmind.ui.org-chart.down")]
        OrgChartDown,
        [Description("org.xmind.ui.org-chart.up")]
        OrgChartUp,
        [Description("org.xmind.ui.tree.left")]
        TreeLeft,
        [Description("org.xmind.ui.tree.right")]
        TreeRight,
        [Description("org.xmind.ui.logic.right")]
        LogicRight,
        [Description("org.xmind.ui.logic.left")]
        LogicLeft
    }
}
