using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Views;
using EnigmaRampageAndroidUI.Utils;
using Microcharts;
using Microcharts.Droid;
using SkiaSharp;

namespace EnigmaRampageAndroidUI.Fragments
{
    public class PlayTimeTabFragment : Android.Support.V4.App.Fragment
    {
        private Activity mActivity;
        private List<Entry> mEntries;
        private ChartView mBarChart, mPointChart, mLineChart, mDonutChart, mRadialGaugeChart, mRadarChart;

        /// <summary>
        /// Override OnCreateView method
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            mActivity = Activity as Activity;
            View playTimeFragment = inflater.Inflate(Resource.Layout.fragment_playtime_analysis, container, false);

            mBarChart = playTimeFragment.FindViewById<ChartView>(Resource.Id.playtimeBarChart);
            mPointChart = playTimeFragment.FindViewById<ChartView>(Resource.Id.playtimePointChart);
            mLineChart = playTimeFragment.FindViewById<ChartView>(Resource.Id.playtimeLineChart);
            mDonutChart = playTimeFragment.FindViewById<ChartView>(Resource.Id.playtimeDonutChart);
            mRadialGaugeChart = playTimeFragment.FindViewById<ChartView>(Resource.Id.playtimeRadialGaugeChart);
            mRadarChart = playTimeFragment.FindViewById<ChartView>(Resource.Id.playtimeRadarChart);

            ChartDataRetriever.OnPlayTimeDataComplete += ChartDataRetriever_OnPlayTimeDataComplete;

            return playTimeFragment;
        }

        /// <summary>
        /// Triggers when the chart data is fully loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChartDataRetriever_OnPlayTimeDataComplete(object sender, ChartDataRetriever.OnPlayTimeEventArgs e)
        {
            List<int> playTimeValues = new List<int>();
            List<TimeSpan> playTimes = new List<TimeSpan>();

            playTimes = e.PlayTime;

            if (playTimes != null)
            {
                playTimeValues.Add((from time in playTimes where time < new TimeSpan(1, 0, 0) select time).Count());
                playTimeValues.Add((from time in playTimes where time >= new TimeSpan(1, 0, 0) && time < new TimeSpan(5, 0, 0) select time).Count());
                playTimeValues.Add((from time in playTimes where time >= new TimeSpan(5, 0, 0) && time < new TimeSpan(20, 0, 0) select time).Count());
                playTimeValues.Add((from time in playTimes where time >= new TimeSpan(20, 0, 0) && time < new TimeSpan(100, 0, 0) select time).Count());
                playTimeValues.Add((from time in playTimes where time >= new TimeSpan(100, 0, 0) select time).Count());
            }
            else
            {
                string error = "There was a problem retrieving the record.";
                mActivity.RunOnUiThread(() =>
                {
                    AlertGenerator.ShowError(error, mActivity);
                });
            }

            DataEntries(playTimeValues);
        }

        /// <summary>
        /// Method for drawing the charts
        /// </summary>
        private void DrawCharts()
        {
            // Initialize the charts
            var barChart = new BarChart() { Entries = mEntries };
            mBarChart.Chart = barChart;
            var pointChart = new PointChart() { Entries = mEntries };
            mPointChart.Chart = pointChart;
            var lineChart = new LineChart() { Entries = mEntries };
            mLineChart.Chart = lineChart;
            var donutChart = new DonutChart() { Entries = mEntries };
            mDonutChart.Chart = donutChart;
            var radialGaugeChart = new RadialGaugeChart() { Entries = mEntries };
            mRadialGaugeChart.Chart = radialGaugeChart;
            var radarChart = new RadarChart() { Entries = mEntries };
            mRadarChart.Chart = radarChart;
        }

        /// <summary>
        /// Initialize the data entries
        /// </summary>
        private void DataEntries(List<int> playTimeValues)
        {
            mEntries = new List<Entry>();
            List<int> playTimes = new List<int>();
            playTimes = playTimeValues;

            mEntries = new List<Entry>() {
                new Entry(playTimes[0])
                {
                    Label = "<1 hr",
                    ValueLabel = playTimes[0].ToString(),
                    Color = SKColor.Parse("#266489")
                },
                new Entry(playTimes[1])
                {
                    Label = "1-5 hrs",
                    ValueLabel = playTimes[1].ToString(),
                    Color = SKColor.Parse("#3498db")
                },
                new Entry(playTimes[2])
                {
                    Label = "5-20 hrs",
                    ValueLabel = playTimes[2].ToString(),
                    Color = SKColor.Parse("#00a766")
                },
                new Entry(playTimes[3])
                {
                    Label = "20-100 hrs",
                    ValueLabel = playTimes[3].ToString(),
                    Color = SKColor.Parse("#f78b1f")
                },
                new Entry(playTimes[4])
                {
                    Label = ">100",
                    ValueLabel = playTimes[4].ToString(),
                    Color = SKColor.Parse("#ee1a40")
                }
            };

            DrawCharts();
        }
    }
}