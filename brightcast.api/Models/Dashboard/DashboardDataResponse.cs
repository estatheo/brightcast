using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace brightcast.Models.Dashboard
{
    public class DashboardDataResponse
    {
        public CardStatModel Delivered { get; set; }
        public CardStatModel Read { get; set; }
        public CardStatModel NewSubscribers { get; set; }
        public CardStatModel Unsubscribed { get; set; }
        public CardStatModel Replies { get; set; }
    }


    public class CardStatModel
    {
        public int Value { get; set; }
        public float Percentage { get; set; }
        public int ChartPoints { get; set; }
        public int[] ChartValues { get; set; }
        public string[] ChartLabels { get; set; }
    }
}
