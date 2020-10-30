using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace brightcast.Models.Dashboard
{
    public class DashboardDataResponse
    {
        public CardStatModel DeliveredMonth { get; set; }
        public CardStatModel DeliveredWeek { get; set; }
        public CardStatModel DeliveredDay { get; set; }
        public CardStatModel ReadMonth { get; set; }
        public CardStatModel ReadWeek { get; set; }
        public CardStatModel ReadDay { get; set; }
        public CardStatModel SubscribersMonth { get; set; }
        public CardStatModel SubscribersWeek { get; set; }
        public CardStatModel SubscribersDay { get; set; }
        public CardStatModel RepliesMonth { get; set; }
        public CardStatModel RepliesWeek { get; set; }
        public CardStatModel RepliesDay { get; set; }
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
