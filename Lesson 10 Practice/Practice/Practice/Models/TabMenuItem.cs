using System;
using System.Windows.Controls;

namespace Practice.Models
{
    public class TabMenuItem : TabItem
    {
        public string Id { get; set; }

        public Type ViewType { get; set; }

        public string Icon { get; set; }
    }
}
