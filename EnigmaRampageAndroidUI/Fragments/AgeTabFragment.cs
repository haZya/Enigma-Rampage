using Android.App;
using Android.OS;
using Android.Views;
using EnigmaRampageAndroidUI.Utils;
using Microcharts;
using Microcharts.Droid;
using SkiaSharp;
using System.Collections.Generic;
using System.Linq;

namespace EnigmaRampageAndroidUI.Fragments
{
    /// <summary>
    /// Contains the events of AgeTab Fragment
    /// </summary>
    public class AgeTabFragment : Android.Support.V4.App.Fragment
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
            View ageFragment = inflater.Inflate(Resource.Layout.fragment_age_analysis, container, false);

            mBarChart = ageFragment.FindViewById<ChartView>(Resource.Id.ageBarChart);
            mPointChart = ageFragment.FindViewById<ChartView>(Resource.Id.agePointChart);
            mLineChart = ageFragment.FindViewById<ChartView>(Resource.Id.ageLineChart);
            mDonutChart = ageFragment.FindViewById<ChartView>(Resource.Id.ageDonutChart);
            mRadialGaugeChart = ageFragment.FindViewById<ChartView>(Resource.Id.ageRadialGaugeChart);
            mRadarChart = ageFragment.FindViewById<ChartView>(Resource.Id.ageRadarChart);

            ChartDataRetriever.OnAgeDataComplete += ChartDataRetriever_OnAgeDataComplete;

            return ageFragment;
        }

        /// <summary>
        /// Triggers when the chart data is fully loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChartDataRetriever_OnAgeDataComplete(object sender, ChartDataRetriever.OnAgeEventArgs e)
        {
            List<int> ageValues = new List<int>();
            List<int> ageGroups = new List<int>();

            ageGroups = e.Age;

            if (ageGroups != null)
            {
                ageValues.Add((from age in ageGroups where age < 15 select age).Count());
                ageValues.Add((from age in ageGroups where age >= 15 && age < 25 select age).Count());
                ageValues.Add((from age in ageGroups where age >= 25 && age < 55 select age).Count());
                ageValues.Add((from age in ageGroups where age >= 55 && age < 65 select age).Count());
                ageValues.Add((from age in ageGroups where age >= 65 select age).Count());
            }
            else
            {
                string error = "There was a problem retrieving the record.";
                mActivity.RunOnUiThread(() =>
                {
                    AlertGenerator.ShowError(error, mActivity);
                });
            }

            DataEntries(ageValues);
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
        private void DataEntries(List<int> ageValues)
        {
            mEntries = new List<Entry>();
            List<int> ageGroups = new List<int>();
            ageGroups = ageValues;

            mEntries = new List<Entry>() {
                new Entry(ageGroups[0])
                {
                    Label = "Children",
                    ValueLabel = ageGroups[0].ToString(),
                    Color = SKColor.Parse("#266489")
                },
                new Entry(ageGroups[1])
                {
                    Label = "Youth",
                    ValueLabel = ageGroups[1].ToString(),
                    Color = SKColor.Parse("#3498db")
                },
                new Entry(ageGroups[2])
                {
                    Label = "Adults",
                    ValueLabel = ageGroups[2].ToString(),
                    Color = SKColor.Parse("#00a766")
                },
                new Entry(ageGroups[3])
                {
                    Label = "Seniors",
                    ValueLabel = ageGroups[3].ToString(),
                    Color = SKColor.Parse("#f78b1f")
                },
                new Entry(ageGroups[4])
                {
                    Label = "Seniors",
                    ValueLabel = ageGroups[4].ToString(),
                    Color = SKColor.Parse("#ee1a40")
                }
            };

            DrawCharts();
        }
    }
}