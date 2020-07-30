using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using brightcast.Entities;
using brightcast.Models.Dashboard;

namespace brightcast.Helpers
{
    public class DataMappingHelper
    {
        private readonly DashboardDataResponse _emptyResponse = new DashboardDataResponse
        {
            Delivered = new CardStatModel
            {
                Value = 0,
                Percentage = 0,
                ChartPoints = 1,
                ChartLabels = new[] { DateTime.UtcNow.ToString("d") },
                ChartValues = new[] { 0 }
            },
            Read = new CardStatModel
            {
                Value = 0,
                Percentage = 0,
                ChartPoints = 1,
                ChartLabels = new[] { DateTime.UtcNow.ToString("d") },
                ChartValues = new[] { 0 }
            },
            NewSubscribers = new CardStatModel
            {
                Value = 0,
                Percentage = 0,
                ChartPoints = 1,
                ChartLabels = new[] { DateTime.UtcNow.ToString("d") },
                ChartValues = new[] { 0 }
            },
            Unsubscribed = new CardStatModel
            {
                Value = 0,
                Percentage = 0,
                ChartPoints = 1,
                ChartLabels = new[] { DateTime.UtcNow.ToString("d") },
                ChartValues = new[] { 0 }
            },
            Replies = new CardStatModel
            {
                Value = 0,
                Percentage = 0,
                ChartPoints = 1,
                ChartLabels = new[] { DateTime.UtcNow.ToString("d") },
                ChartValues = new[] { 0 }
            }
        };
        public DashboardDataResponse MapStats(List<CampaignMessage> list, List<Contact> contacts, List<ReceiveMessage> replies)
        {
            var result = _emptyResponse;

            if (list.Count(x => x.Status == "delivered") > 0)
            {
                result.Delivered = new CardStatModel
                {
                    Value = list.Count(x => x.Status == "delivered"),
                    Percentage = (list.Count(x => x.Status == "delivered") - list
                    .Where(x => x.Date_Created <= DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date)
                    .Count(x => x.Status == "delivered")) * 100 / (float)list
                    .Where(x => x.Date_Created <= DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date)
                    .Count(x => x.Status == "delivered"),
                    ChartPoints = 7,
                    ChartValues = new[]
                {
                    list.Where(x => x.Date_Created <= DateTime.UtcNow.Subtract(new TimeSpan(6, 0, 0, 0)).Date)
                        .Count(x => x.Status == "delivered"),
                    list.Where(x => x.Date_Created <= DateTime.UtcNow.Subtract(new TimeSpan(5, 0, 0, 0)).Date)
                        .Count(x => x.Status == "delivered"),
                    list.Where(x => x.Date_Created <= DateTime.UtcNow.Subtract(new TimeSpan(4, 0, 0, 0)).Date)
                        .Count(x => x.Status == "delivered"),
                    list.Where(x => x.Date_Created <= DateTime.UtcNow.Subtract(new TimeSpan(3, 0, 0, 0)).Date)
                        .Count(x => x.Status == "delivered"),
                    list.Where(x => x.Date_Created <= DateTime.UtcNow.Subtract(new TimeSpan(2, 0, 0, 0)).Date)
                        .Count(x => x.Status == "delivered"),
                    list.Where(x => x.Date_Created <= DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date)
                        .Count(x => x.Status == "delivered"),
                    list.Count(x => x.Status == "delivered")
                },
                    ChartLabels = new[]
                {
                    DateTime.UtcNow.Subtract(new TimeSpan(6, 0, 0, 0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(5, 0, 0, 0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(4, 0, 0, 0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(3, 0, 0, 0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(2, 0, 0, 0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.DayOfWeek.ToString()
                }
                };
            }

            if (list.Count(x => x.Status == "read") > 0)
            {
                result.Read = new CardStatModel
                {
                    Value = list.Count(x => x.Status == "read"),
                    Percentage = (list.Count(x => x.Status == "read") - list
                                    .Where(x => x.Date_Created <= DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date)
                                    .Count(x => x.Status == "read")) * 100 / (float)list
                                    .Where(x => x.Date_Created <= DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date)
                                    .Count(x => x.Status == "read"),
                    ChartPoints = 7,
                    ChartValues = new[]
                                {
                    list.Where(x => x.Date_Created <= DateTime.UtcNow.Subtract(new TimeSpan(6, 0, 0, 0)).Date)
                        .Count(x => x.Status == "read"),
                    list.Where(x => x.Date_Created <= DateTime.UtcNow.Subtract(new TimeSpan(5, 0, 0, 0)).Date)
                        .Count(x => x.Status == "read"),
                    list.Where(x => x.Date_Created <= DateTime.UtcNow.Subtract(new TimeSpan(4, 0, 0, 0)).Date)
                        .Count(x => x.Status == "read"),
                    list.Where(x => x.Date_Created <= DateTime.UtcNow.Subtract(new TimeSpan(3, 0, 0, 0)).Date)
                        .Count(x => x.Status == "read"),
                    list.Where(x => x.Date_Created <= DateTime.UtcNow.Subtract(new TimeSpan(2, 0, 0, 0)).Date)
                        .Count(x => x.Status == "read"),
                    list.Where(x => x.Date_Created <= DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date)
                        .Count(x => x.Status == "read"),
                    list.Count(x => x.Status == "read")
                },
                    ChartLabels = new[]
                                {
                    DateTime.UtcNow.Subtract(new TimeSpan(6, 0, 0, 0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(5, 0, 0, 0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(4, 0, 0, 0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(3, 0, 0, 0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(2, 0, 0, 0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.DayOfWeek.ToString()
                }
                };
            }

            if (contacts.Count(x => x.Subscribed) > 0)
            {
                result.NewSubscribers = new CardStatModel
                {
                    Value = contacts.Count(x => x.Subscribed),
                    Percentage = contacts.Count(x => x.Subscribed) - contacts
                    .Where(x => x.CreatedAt <= DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date)
                    .Count(x => x.Subscribed),
                    ChartPoints = 7,
                    ChartValues = new[]
                {
                    contacts.Where(x => x.CreatedAt <= DateTime.UtcNow.Subtract(new TimeSpan(6, 0, 0, 0)).Date)
                        .Count(x => x.Subscribed),
                    contacts.Where(x => x.CreatedAt <= DateTime.UtcNow.Subtract(new TimeSpan(5, 0, 0, 0)).Date)
                        .Count(x => x.Subscribed),
                    contacts.Where(x => x.CreatedAt <= DateTime.UtcNow.Subtract(new TimeSpan(4, 0, 0, 0)).Date)
                        .Count(x => x.Subscribed),
                    contacts.Where(x => x.CreatedAt <= DateTime.UtcNow.Subtract(new TimeSpan(3, 0, 0, 0)).Date)
                        .Count(x => x.Subscribed),
                    contacts.Where(x => x.CreatedAt <= DateTime.UtcNow.Subtract(new TimeSpan(2, 0, 0, 0)).Date)
                        .Count(x => x.Subscribed),
                    contacts.Where(x => x.CreatedAt <= DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date)
                        .Count(x => x.Subscribed),
                    contacts.Count(x => x.Subscribed)
                },
                    ChartLabels = new[]
                {
                    DateTime.UtcNow.Subtract(new TimeSpan(6, 0, 0, 0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(5, 0, 0, 0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(4, 0, 0, 0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(3, 0, 0, 0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(2, 0, 0, 0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.DayOfWeek.ToString()
                }
                };
            }

            if (contacts.Count(x => !x.Subscribed) > 0)
            {
                result.Unsubscribed = new CardStatModel
                {
                    Value = contacts.Count(x => !x.Subscribed),
                    Percentage = contacts.Count(x => !x.Subscribed) - contacts
                    .Where(x => x.CreatedAt <= DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date)
                    .Count(x => !x.Subscribed),
                    ChartPoints = 7,
                    ChartValues = new[]
                {
                    contacts.Where(x => x.CreatedAt <= DateTime.UtcNow.Subtract(new TimeSpan(6, 0, 0, 0)).Date)
                        .Count(x => !x.Subscribed),
                    contacts.Where(x => x.CreatedAt <= DateTime.UtcNow.Subtract(new TimeSpan(5, 0, 0, 0)).Date)
                        .Count(x => !x.Subscribed),
                    contacts.Where(x => x.CreatedAt <= DateTime.UtcNow.Subtract(new TimeSpan(4, 0, 0, 0)).Date)
                        .Count(x => !x.Subscribed),
                    contacts.Where(x => x.CreatedAt <= DateTime.UtcNow.Subtract(new TimeSpan(3, 0, 0, 0)).Date)
                        .Count(x => !x.Subscribed),
                    contacts.Where(x => x.CreatedAt <= DateTime.UtcNow.Subtract(new TimeSpan(2, 0, 0, 0)).Date)
                        .Count(x => !x.Subscribed),
                    contacts.Where(x => x.CreatedAt <= DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date)
                        .Count(x => !x.Subscribed),
                    contacts.Count(x => !x.Subscribed)
                },
                    ChartLabels = new[]
                {
                    DateTime.UtcNow.Subtract(new TimeSpan(6, 0, 0, 0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(5, 0, 0, 0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(4, 0, 0, 0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(3, 0, 0, 0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(2, 0, 0, 0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.DayOfWeek.ToString()
                }
                };
            }

            if (replies.Count > 0)
            {
                result.Replies = new CardStatModel
                {
                    Value = replies.Count,
                    Percentage = replies.Count - (replies
                                    .Where(x => x.CreatedAt <= DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date).ToList()
                                    .Count),
                    ChartPoints = 7,
                    ChartValues = new[]
                                {
                    replies.Where(x => x.CreatedAt <= DateTime.UtcNow.Subtract(new TimeSpan(6, 0, 0, 0)).Date).ToList()
                        .Count,
                    replies.Where(x => x.CreatedAt <= DateTime.UtcNow.Subtract(new TimeSpan(5, 0, 0, 0)).Date).ToList()
                        .Count,
                    replies.Where(x => x.CreatedAt <= DateTime.UtcNow.Subtract(new TimeSpan(4, 0, 0, 0)).Date).ToList()
                        .Count,
                    replies.Where(x => x.CreatedAt <= DateTime.UtcNow.Subtract(new TimeSpan(3, 0, 0, 0)).Date).ToList()
                        .Count,
                    replies.Where(x => x.CreatedAt <= DateTime.UtcNow.Subtract(new TimeSpan(2, 0, 0, 0)).Date).ToList()
                        .Count,
                    replies.Where(x => x.CreatedAt <= DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date).ToList()
                        .Count,
                    replies.Count
                },
                    ChartLabels = new[]
                                {
                    DateTime.UtcNow.Subtract(new TimeSpan(6, 0, 0, 0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(5, 0, 0, 0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(4, 0, 0, 0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(3, 0, 0, 0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(2, 0, 0, 0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.DayOfWeek.ToString()
                }
                };
            }

            

            return result;
        }
    }
}
