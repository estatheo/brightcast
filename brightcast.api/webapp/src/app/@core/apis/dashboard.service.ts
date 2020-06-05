import { Injectable } from '@angular/core';
import { PeriodsService } from '../mock/periods.service';
import { OrdersChart, OrdersChartData } from '../data/orders-chart';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';

@Injectable()
export class DashboardService extends OrdersChartData {

  apiURL: string = environment.apiUrl;

  private year = [
    '2012',
    '2013',
    '2014',
    '2015',
    '2016',
    '2017',
    '2018',
    '2019',
    '2020',
  ];

  private data = { };

  constructor(private period: PeriodsService, private httpClient: HttpClient) {
    super();
      this.getDataForWeekPeriod();
      // month: this.getDataForMonthPeriod(),
      // year: this.getDataForYearPeriod(),
    
  }

  private getDataForWeekPeriod() {
    
    this.httpClient.get(`${this.apiURL}/dashboard/data`).subscribe(data => {
      this.data = {
        week: {
          delivered: {
            chartLabel: data['Delivered']['ChartLabels'],
            linesData: data['Delivered']['ChartValues'],
            value: data['Delivered']['Value'],
            percentage: data['Delivered']['Percentage']
          },      
          read: {
            chartLabel: data['Read']['ChartLabels'],
            linesData: data['Read']['ChartValues'],
            value: data['Read']['Value'],
            percentage: data['Read']['Percentage']
          },
          newSubscribers: {
            chartLabel: data['NewSubscribers']['ChartLabels'],
            linesData: data['NewSubscribers']['ChartValues'],
            value: data['NewSubscribers']['Value'],
            percentage: data['NewSubscribers']['Percentage']
          },
          unsubscribed: {
            chartLabel: data['Unsubscribed']['ChartLabels'],
            linesData: data['Unsubscribed']['ChartValues'],
            value: data['Unsubscribed']['Value'],
            percentage: data['Unsubscribed']['Percentage']
          },
          replies: {
            chartLabel: data['Replies']['ChartLabels'],
            linesData: data['Replies']['ChartValues'],
            value: data['Replies']['Value'],
            percentage: data['Replies']['Percentage']
          }
        }      
      }  
    });
  }

  // private getDataForMonthPeriod(): OrdersChart {
  //   return {
  //     chartLabel: this.getDataLabels(47, this.period.getMonths()),
  //     linesData: [
  //       [
  //         5, 63, 113, 156, 194, 225,
  //         250, 270, 283, 289, 290,
  //         286, 277, 264, 244, 220,
  //         194, 171, 157, 151, 150,
  //         152, 155, 160, 166, 170,
  //         167, 153, 135, 115, 97,
  //         82, 71, 64, 63, 62, 61,
  //         62, 65, 73, 84, 102,
  //         127, 159, 203, 259, 333,
  //       ],
  //     ],
  //   };
  // }

  // private getDataForYearPeriod(): OrdersChart {
  //   return {
  //     chartLabel: this.getDataLabels(42, this.year),
  //     linesData: [
  //       [
  //         190, 269, 327, 366, 389, 398,
  //         396, 387, 375, 359, 343, 327,
  //         312, 298, 286, 276, 270, 268,
  //         265, 258, 247, 234, 220, 204,
  //         188, 172, 157, 142, 128, 116,
  //         106, 99, 95, 94, 92, 89, 84,
  //         77, 69, 60, 49, 36, 22,
  //       ],
  //       [
  //         265, 307, 337, 359, 375, 386,
  //         393, 397, 399, 397, 390, 379,
  //         365, 347, 326, 305, 282, 261,
  //         241, 223, 208, 197, 190, 187,
  //         185, 181, 172, 160, 145, 126,
  //         105, 82, 60, 40, 26, 19, 22,
  //         43, 82, 141, 220, 321,
  //       ],
  //       [
  //         9, 165, 236, 258, 244, 206,
  //         186, 189, 209, 239, 273, 307,
  //         339, 365, 385, 396, 398, 385,
  //         351, 300, 255, 221, 197, 181,
  //         170, 164, 162, 161, 159, 154,
  //         146, 135, 122, 108, 96, 87,
  //         83, 82, 82, 82, 82, 82, 82,
  //       ],
  //     ],
  //   };
  // }

  getDataLabels(nPoints: number, labelsArray: string[]): string[] {
    const labelsArrayLength = labelsArray.length;
    const step = Math.round(nPoints / labelsArrayLength);

    return Array.from(Array(nPoints)).map((item, index) => {
      const dataIndex = Math.round(index / step);

      return index % step === 0 ? labelsArray[dataIndex] : '';
    });
  }

  getOrdersChartData(period: string): OrdersChart {
    return this.data[period];
  }
}
