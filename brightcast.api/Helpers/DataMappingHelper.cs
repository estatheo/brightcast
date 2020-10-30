using System;
using System.Collections.Generic;
using System.Linq;
using brightcast.Entities;
using brightcast.Models.Dashboard;

namespace brightcast.Helpers
{
    public class DataMappingHelper
    {
        private readonly DashboardDataResponse _emptyResponse = new DashboardDataResponse
        {
            DeliveredMonth = new CardStatModel
            {
                Value = 0,
                Percentage = 0,
                ChartPoints = 1,
                ChartLabels = new[] {DateTime.UtcNow.ToString("d")},
                ChartValues = new[] {0}
            },
            DeliveredWeek = new CardStatModel
            {
                Value = 0,
                Percentage = 0,
                ChartPoints = 1,
                ChartLabels = new[] {DateTime.UtcNow.ToString("d")},
                ChartValues = new[] {0}
            },
            DeliveredDay = new CardStatModel
            {
                Value = 0,
                Percentage = 0,
                ChartPoints = 1,
                ChartLabels = new[] {DateTime.UtcNow.ToString("d")},
                ChartValues = new[] {0}
            },
            ReadMonth = new CardStatModel
            {
                Value = 0,
                Percentage = 0,
                ChartPoints = 1,
                ChartLabels = new[] {DateTime.UtcNow.ToString("d")},
                ChartValues = new[] {0}
            },
            ReadWeek = new CardStatModel
            {
                Value = 0,
                Percentage = 0,
                ChartPoints = 1,
                ChartLabels = new[] {DateTime.UtcNow.ToString("d")},
                ChartValues = new[] {0}
            },
            ReadDay = new CardStatModel
            {
                Value = 0,
                Percentage = 0,
                ChartPoints = 1,
                ChartLabels = new[] {DateTime.UtcNow.ToString("d")},
                ChartValues = new[] {0}
            },
            SubscribersMonth = new CardStatModel
            {
                Value = 0,
                Percentage = 0,
                ChartPoints = 1,
                ChartLabels = new[] {DateTime.UtcNow.ToString("d")},
                ChartValues = new[] {0}
            },
            SubscribersWeek = new CardStatModel
            {
                Value = 0,
                Percentage = 0,
                ChartPoints = 1,
                ChartLabels = new[] {DateTime.UtcNow.ToString("d")},
                ChartValues = new[] {0}
            },
            SubscribersDay = new CardStatModel
            {
                Value = 0,
                Percentage = 0,
                ChartPoints = 1,
                ChartLabels = new[] {DateTime.UtcNow.ToString("d")},
                ChartValues = new[] {0}
            },
            RepliesMonth = new CardStatModel
            {
                Value = 0,
                Percentage = 0,
                ChartPoints = 1,
                ChartLabels = new[] {DateTime.UtcNow.ToString("d")},
                ChartValues = new[] {0}
            },
            RepliesWeek = new CardStatModel
            {
                Value = 0,
                Percentage = 0,
                ChartPoints = 1,
                ChartLabels = new[] {DateTime.UtcNow.ToString("d")},
                ChartValues = new[] {0}
            },
            RepliesDay = new CardStatModel
            {
                Value = 0,
                Percentage = 0,
                ChartPoints = 1,
                ChartLabels = new[] {DateTime.UtcNow.ToString("d")},
                ChartValues = new[] {0}
            }
        };

        public DashboardDataResponse MapStats(List<CampaignMessage> list, List<Contact> contacts,
            List<ReceiveMessage> replies)
        {
            var result = _emptyResponse;

            if (list.Count(x =>
                (x.Status == "delivered" || x.Status == "read") &&
                x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(56, 0, 0, 0)) &&
                x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(28, 0, 0, 0))) > 0)
            {
                result.DeliveredMonth = new CardStatModel
                {
                    Value = list.Count(x =>
                        (x.Status == "delivered" || x.Status == "read") && x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(28, 0, 0, 0))),
                    Percentage = list.Count(x => (x.Status == "delivered" || x.Status == "read")
                                                 && x.CreatedAt >=
                                                 DateTime.Now.Subtract(new TimeSpan(28, 0, 0, 0)))
                        / list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(56, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(28, 0, 0, 0))) * 100,
                    ChartPoints = 28,
                    ChartValues = new[]
                    {
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(28, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(27, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(27, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(26, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(26, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(25, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(25, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(24, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(24, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(23, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(23, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(22, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(22, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(21, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(21, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(20, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(20, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(19, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(19, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(18, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(18, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(17, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(17, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(16, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(16, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(15, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(15, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(14, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(14, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(13, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(13, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(12, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(12, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(11, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(11, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(10, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(10, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(9, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(9, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(8, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(8, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(6, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(6, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(5, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(5, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(4, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(4, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now)
                    },
                    ChartLabels = new[]
                    {
                        DateTime.UtcNow.Subtract(new TimeSpan(27, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(26, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(25, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(24, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(23, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(22, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(21, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(20, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(19, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(18, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(17, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(16, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(15, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(14, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(13, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(12, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(11, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(10, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(9, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(8, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(7, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(6, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(5, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(4, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(3, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(2, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.DayOfWeek.ToString()
                    }
                };
                
            }

            if (list.Count(x =>
                (x.Status == "delivered" || x.Status == "read") &&
                x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(14, 0, 0, 0)) &&
                x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0))) > 0)
            {
                result.DeliveredWeek = new CardStatModel
                {
                    Value = list.Count(x =>
                        (x.Status == "delivered" || x.Status == "read") && x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0))),
                    Percentage = list.Count(x => (x.Status == "delivered" || x.Status == "read")
                                                 && x.CreatedAt >=
                                                 DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)))
                        / list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(14, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0))) * 100,
                    ChartPoints = 7,
                    ChartValues = new[]
                    {
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(6, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(6, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(5, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(5, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(4, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(4, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0))),
                        list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now)
                    },
                    ChartLabels = new[]
                    {
                        DateTime.UtcNow.Subtract(new TimeSpan(6, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(5, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(4, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(3, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(2, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.ToString("dd/MM")
                    }
                };
                
            }

            if (list.Count(x =>
                (x.Status == "delivered" || x.Status == "read") &&
                x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0)) &&
                x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0))) > 0)
            {
                result.DeliveredDay = new CardStatModel
                {
                    Value = list.Count(x =>
                        (x.Status == "delivered" || x.Status == "read") && x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0))),
                    Percentage = list.Count(x => (x.Status == "delivered" || x.Status == "read")
                                                 && x.CreatedAt >=
                                                 DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)))
                        / list.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0))) * 100,
                    ChartPoints = 24,
                    ChartValues = new[]
                    {
                        list.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 23, 0, 0))),
                        list.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 23, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 22, 0, 0))),
                        list.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 22, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 21, 0, 0))),
                        list.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 21, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 20, 0, 0))),
                        list.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 20, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 19, 0, 0))),
                        list.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 19, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 18, 0, 0))),
                        list.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 18, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 17, 0, 0))),
                        list.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 17, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 16, 0, 0))),
                        list.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 16, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 15, 0, 0))),
                        list.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 15, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 14, 0, 0))),
                        list.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 14, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 13, 0, 0))),
                        list.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 13, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 12, 0, 0))),
                        list.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 12, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 11, 0, 0))),
                        list.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 11, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 10, 0, 0))),
                        list.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 10, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 9, 0, 0))),
                        list.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 9, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 8, 0, 0))),
                        list.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 8, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 7, 0, 0))),
                        list.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 7, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 6, 0, 0))),
                        list.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 6, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 5, 0, 0))),
                        list.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 5, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 4, 0, 0))),
                        list.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 4, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 3, 0, 0))),
                        list.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 3, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 2, 0, 0))),
                        list.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 2, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 1, 0, 0))),
                        list.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 1, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now)
                    },
                    ChartLabels = new[]
                    {
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 23, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 22, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 21, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 20, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 19, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 18, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 17, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 16, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 15, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 14, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 13, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 12, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 11, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 10, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 9, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 8, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 7, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 6, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 5, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 4, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 3, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 2, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 1, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.ToString("HH:mm"),
                    }
                };
            }


            if (list.Count(x =>
                (x.Status == "read") &&
                x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(56, 0, 0, 0)) &&
                x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(28, 0, 0, 0))) > 0)
            {
                result.ReadMonth = new CardStatModel
                {
                    Value = list.Count(x =>
                        x.Status == "read" && x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(28, 0, 0, 0))),
                    Percentage = list.Count(x => (x.Status == "read")
                                                 && x.CreatedAt >=
                                                 DateTime.Now.Subtract(new TimeSpan(28, 0, 0, 0)))
                        / list.Count(x =>
                            (x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(56, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(28, 0, 0, 0))) * 100,
                    ChartPoints = 28,
                    ChartValues = new[]
                    {
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(28, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(27, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(27, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(26, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(26, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(25, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(25, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(24, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(24, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(23, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(23, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(22, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(22, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(21, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(21, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(20, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(20, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(19, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(19, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(18, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(18, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(17, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(17, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(16, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(16, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(15, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(15, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(14, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(14, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(13, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(13, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(12, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(12, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(11, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(11, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(10, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(10, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(9, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(9, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(8, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(8, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(6, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(6, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(5, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(5, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(4, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(4, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now)
                    },
                    ChartLabels = new[]
                    {
                        DateTime.UtcNow.Subtract(new TimeSpan(27, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(26, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(25, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(24, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(23, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(22, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(21, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(20, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(19, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(18, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(17, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(16, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(15, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(14, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(13, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(12, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(11, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(10, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(9, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(8, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(7, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(6, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(5, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(4, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(3, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(2, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.DayOfWeek.ToString()
                    }
                };
                
            }

            if (list.Count(x =>
                (x.Status == "read") &&
                x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(14, 0, 0, 0)) &&
                x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0))) > 0)
            {
                result.ReadWeek = new CardStatModel
                {
                    Value = list.Count(x =>
                        x.Status == "delivered" && x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0))),
                    Percentage = list.Count(x => (x.Status == "read")
                                                 && x.CreatedAt >=
                                                 DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)))
                        / list.Count(x =>
                            (x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(14, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0))) * 100,
                    ChartPoints = 7,
                    ChartValues = new[]
                    {
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(6, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(6, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(5, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(5, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(4, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(4, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0))),
                        list.Count(x =>
                            ( x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now)
                    },
                    ChartLabels = new[]
                    {
                        DateTime.UtcNow.Subtract(new TimeSpan(6, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(5, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(4, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(3, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(2, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.ToString("dd/MM")
                    }
                };
                
            }

            if (list.Count(x =>
                (x.Status == "read") &&
                x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0)) &&
                x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0))) > 0)
            {
                result.ReadDay = new CardStatModel
                {
                    Value = list.Count(x =>
                        x.Status == "read" && x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0))),
                    Percentage = list.Count(x => (x.Status == "read")
                                                 && x.CreatedAt >=
                                                 DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)))
                        / list.Count(x =>
                            (x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0))) * 100,
                    ChartPoints = 24,
                    ChartValues = new[]
                    {
                        list.Count(x => ( x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 23, 0, 0))),
                        list.Count(x => ( x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 23, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 22, 0, 0))),
                        list.Count(x => ( x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 22, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 21, 0, 0))),
                        list.Count(x => ( x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 21, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 20, 0, 0))),
                        list.Count(x => ( x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 20, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 19, 0, 0))),
                        list.Count(x => ( x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 19, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 18, 0, 0))),
                        list.Count(x => ( x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 18, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 17, 0, 0))),
                        list.Count(x => ( x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 17, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 16, 0, 0))),
                        list.Count(x => ( x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 16, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 15, 0, 0))),
                        list.Count(x => ( x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 15, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 14, 0, 0))),
                        list.Count(x => ( x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 14, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 13, 0, 0))),
                        list.Count(x => ( x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 13, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 12, 0, 0))),
                        list.Count(x => ( x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 12, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 11, 0, 0))),
                        list.Count(x => ( x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 11, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 10, 0, 0))),
                        list.Count(x => ( x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 10, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 9, 0, 0))),
                        list.Count(x => ( x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 9, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 8, 0, 0))),
                        list.Count(x => ( x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 8, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 7, 0, 0))),
                        list.Count(x => ( x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 7, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 6, 0, 0))),
                        list.Count(x => ( x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 6, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 5, 0, 0))),
                        list.Count(x => ( x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 5, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 4, 0, 0))),
                        list.Count(x => ( x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 4, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 3, 0, 0))),
                        list.Count(x => ( x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 3, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 2, 0, 0))),
                        list.Count(x => ( x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 2, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 1, 0, 0))),
                        list.Count(x => ( x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 1, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now)
                    },
                    ChartLabels = new[]
                    {
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 23, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 22, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 21, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 20, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 19, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 18, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 17, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 16, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 15, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 14, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 13, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 12, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 11, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 10, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 9, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 8, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 7, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 6, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 5, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 4, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 3, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 2, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 1, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.ToString("HH:mm"),
                    }
                };
            }

            if (replies.Count(x =>
                (x.Status == "delivered" || x.Status == "read") &&
                x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(56, 0, 0, 0)) &&
                x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(28, 0, 0, 0))) > 0)
            {
                result.ReadMonth = new CardStatModel
                {
                    Value = replies.Count(x =>
                        (x.Status == "delivered" || x.Status == "read") && x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(28, 0, 0, 0))),
                    Percentage = replies.Count(x => (x.Status == "delivered" || x.Status == "read")
                                                 && x.CreatedAt >=
                                                 DateTime.Now.Subtract(new TimeSpan(28, 0, 0, 0)))
                        / replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(56, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(28, 0, 0, 0))) * 100,
                    ChartPoints = 28,
                    ChartValues = new[]
                    {
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(28, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(27, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(27, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(26, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(26, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(25, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(25, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(24, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(24, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(23, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(23, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(22, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(22, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(21, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(21, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(20, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(20, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(19, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(19, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(18, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(18, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(17, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(17, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(16, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(16, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(15, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(15, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(14, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(14, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(13, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(13, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(12, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(12, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(11, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(11, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(10, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(10, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(9, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(9, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(8, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(8, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(6, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(6, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(5, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(5, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(4, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(4, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now)
                    },
                    ChartLabels = new[]
                    {
                        DateTime.UtcNow.Subtract(new TimeSpan(27, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(26, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(25, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(24, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(23, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(22, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(21, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(20, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(19, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(18, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(17, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(16, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(15, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(14, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(13, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(12, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(11, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(10, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(9, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(8, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(7, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(6, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(5, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(4, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(3, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(2, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.DayOfWeek.ToString()
                    }
                };

            }

            if (replies.Count(x =>
                (x.Status == "delivered" || x.Status == "read") &&
                x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(14, 0, 0, 0)) &&
                x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0))) > 0)
            {
                result.RepliesWeek = new CardStatModel
                {
                    Value = replies.Count(x =>
                        (x.Status == "delivered" || x.Status == "read") && x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0))),
                    Percentage = replies.Count(x => (x.Status == "delivered" || x.Status == "read")
                                                 && x.CreatedAt >=
                                                 DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)))
                        / replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(14, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0))) * 100,
                    ChartPoints = 7,
                    ChartValues = new[]
                    {
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(6, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(6, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(5, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(5, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(4, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(4, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0))),
                        replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now)
                    },
                    ChartLabels = new[]
                    {
                        DateTime.UtcNow.Subtract(new TimeSpan(6, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(5, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(4, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(3, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(2, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.ToString("dd/MM")
                    }
                };

            }

            if (replies.Count(x =>
                (x.Status == "delivered" || x.Status == "read") &&
                x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0)) &&
                x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0))) > 0)
            {
                result.RepliesDay = new CardStatModel
                {
                    Value = replies.Count(x =>
                        (x.Status == "delivered" || x.Status == "read") && x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0))),
                    Percentage = replies.Count(x => (x.Status == "delivered" || x.Status == "read")
                                                 && x.CreatedAt >=
                                                 DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)))
                        / replies.Count(x =>
                            (x.Status == "delivered" || x.Status == "read") &&
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0))) * 100,
                    ChartPoints = 24,
                    ChartValues = new[]
                    {
                        replies.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 23, 0, 0))),
                        replies.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 23, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 22, 0, 0))),
                        replies.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 22, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 21, 0, 0))),
                        replies.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 21, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 20, 0, 0))),
                        replies.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 20, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 19, 0, 0))),
                        replies.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 19, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 18, 0, 0))),
                        replies.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 18, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 17, 0, 0))),
                        replies.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 17, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 16, 0, 0))),
                        replies.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 16, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 15, 0, 0))),
                        replies.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 15, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 14, 0, 0))),
                        replies.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 14, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 13, 0, 0))),
                        replies.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 13, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 12, 0, 0))),
                        replies.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 12, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 11, 0, 0))),
                        replies.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 11, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 10, 0, 0))),
                        replies.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 10, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 9, 0, 0))),
                        replies.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 9, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 8, 0, 0))),
                        replies.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 8, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 7, 0, 0))),
                        replies.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 7, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 6, 0, 0))),
                        replies.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 6, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 5, 0, 0))),
                        replies.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 5, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 4, 0, 0))),
                        replies.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 4, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 3, 0, 0))),
                        replies.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 3, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 2, 0, 0))),
                        replies.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 2, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 1, 0, 0))),
                        replies.Count(x => (x.Status == "delivered" || x.Status == "read") &&
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 1, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now)
                    },
                    ChartLabels = new[]
                    {
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 23, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 22, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 21, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 20, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 19, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 18, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 17, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 16, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 15, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 14, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 13, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 12, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 11, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 10, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 9, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 8, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 7, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 6, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 5, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 4, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 3, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 2, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 1, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.ToString("HH:mm"),
                    }
                };
            }

            if (contacts.Count(x =>
                
                x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(56, 0, 0, 0)) &&
                x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(28, 0, 0, 0))) > 0)
            {
                result.SubscribersMonth = new CardStatModel
                {
                    Value = contacts.Count(x =>
                         x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(28, 0, 0, 0))),
                    Percentage = contacts.Count(x => 
                                                  x.CreatedAt >=
                                                 DateTime.Now.Subtract(new TimeSpan(28, 0, 0, 0)))
                        / contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(56, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(28, 0, 0, 0))) * 100,
                    ChartPoints = 28,
                    ChartValues = new[]
                    {
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(28, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(27, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(27, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(26, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(26, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(25, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(25, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(24, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(24, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(23, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(23, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(22, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(22, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(21, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(21, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(20, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(20, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(19, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(19, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(18, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(18, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(17, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(17, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(16, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(16, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(15, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(15, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(14, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(14, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(13, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(13, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(12, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(12, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(11, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(11, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(10, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(10, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(9, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(9, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(8, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(8, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(6, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(6, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(5, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(5, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(4, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(4, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now)
                    },
                    ChartLabels = new[]
                    {
                        DateTime.UtcNow.Subtract(new TimeSpan(27, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(26, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(25, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(24, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(23, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(22, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(21, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(20, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(19, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(18, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(17, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(16, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(15, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(14, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(13, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(12, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(11, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(10, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(9, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(8, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(7, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(6, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(5, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(4, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(3, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(2, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.DayOfWeek.ToString()
                    }
                };

            }

            if (contacts.Count(x =>
                
                x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(14, 0, 0, 0)) &&
                x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0))) > 0)
            {
                result.SubscribersWeek = new CardStatModel
                {
                    Value = contacts.Count(x =>
                         x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0))),
                    Percentage = contacts.Count(x => 
                                                  x.CreatedAt >=
                                                 DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)))
                        / contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(14, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0))) * 100,
                    ChartPoints = 7,
                    ChartValues = new[]
                    {
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(6, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(6, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(5, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(5, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(4, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(4, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0))),
                        contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now)
                    },
                    ChartLabels = new[]
                    {
                        DateTime.UtcNow.Subtract(new TimeSpan(6, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(5, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(4, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(3, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(2, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date.ToString("dd/MM"),
                        DateTime.UtcNow.ToString("dd/MM")
                    }
                };

            }

            if (contacts.Count(x =>
                
                x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0)) &&
                x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0))) > 0)
            {
                result.SubscribersDay = new CardStatModel
                {
                    Value = contacts.Count(x =>
                         x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0))),
                    Percentage = contacts.Count(x => 
                                                 x.CreatedAt >=
                                                 DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)))
                        / contacts.Count(x =>
                            
                            x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0)) &&
                            x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0))) * 100,
                    ChartPoints = 24,
                    ChartValues = new[]
                    {
                        contacts.Count(x => 
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 23, 0, 0))),
                        contacts.Count(x => 
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 23, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 22, 0, 0))),
                        contacts.Count(x => 
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 22, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 21, 0, 0))),
                        contacts.Count(x => 
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 21, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 20, 0, 0))),
                        contacts.Count(x => 
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 20, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 19, 0, 0))),
                        contacts.Count(x => 
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 19, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 18, 0, 0))),
                        contacts.Count(x => 
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 18, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 17, 0, 0))),
                        contacts.Count(x => 
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 17, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 16, 0, 0))),
                        contacts.Count(x => 
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 16, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 15, 0, 0))),
                        contacts.Count(x => 
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 15, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 14, 0, 0))),
                        contacts.Count(x => 
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 14, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 13, 0, 0))),
                        contacts.Count(x => 
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 13, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 12, 0, 0))),
                        contacts.Count(x => 
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 12, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 11, 0, 0))),
                        contacts.Count(x => 
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 11, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 10, 0, 0))),
                        contacts.Count(x => 
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 10, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 9, 0, 0))),
                        contacts.Count(x => 
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 9, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 8, 0, 0))),
                        contacts.Count(x => 
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 8, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 7, 0, 0))),
                        contacts.Count(x => 
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 7, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 6, 0, 0))),
                        contacts.Count(x => 
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 6, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 5, 0, 0))),
                        contacts.Count(x => 
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 5, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 4, 0, 0))),
                        contacts.Count(x => 
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 4, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 3, 0, 0))),
                        contacts.Count(x => 
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 3, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 2, 0, 0))),
                        contacts.Count(x => 
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 2, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(0, 1, 0, 0))),
                        contacts.Count(x => 
                                        x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(0, 1, 0, 0)) &&
                                        x.CreatedAt <= DateTime.Now)
                    },
                    ChartLabels = new[]
                    {
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 23, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 22, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 21, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 20, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 19, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 18, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 17, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 16, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 15, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 14, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 13, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 12, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 11, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 10, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 9, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 8, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 7, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 6, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 5, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 4, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 3, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 2, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 1, 0, 0)).Date.ToString("HH:mm"),
                        DateTime.UtcNow.ToString("HH:mm"),
                    }
                };
            }

            return result;
        }
    }
}