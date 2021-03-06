using System.Collections.Generic;
using System.Linq;
using Hackathon.MLBox.Foundation.Engine.Services;
using Hackathon.MLBox.Foundation.Shared.Models.DTO;
using Hackathon.MLBox.Foundation.Shared.Models.Metrics;
using Hackathon.MLBox.Foundation.Shared.Models.Sitecore;
using Microsoft.Extensions.Logging;
using Sitecore.Framework.Conditions;
using Sitecore.Marketing.Automation.Activity;
using Sitecore.Marketing.Automation.Activity.Extensions;
using Sitecore.Marketing.Rules;
using Sitecore.XConnect;
using Sitecore.XConnect.Collection.Model;
using Sitecore.XConnect.Segmentation.Conditions.ExpressionBuilder;
using Sitecore.Xdb.MarketingAutomation.Core.Activity;
using Sitecore.Xdb.MarketingAutomation.Core.Processing.Plan;

namespace Hackathon.NaN.MLBox.Foundation.MarketingAutomation.Activity
{
    public class PredictiveRevenueListener : BaseActivity
    {
        public int Days { get; set; }

        public CollectionLifecycleOperation Lifecycle { get; set; }

        protected PredictiveRevenueListener(ILogger<IActivity> logger)
            : this(logger, "true", "false") { }

        /// <summary>
        /// Create a new instance of the <see cref="T:Sitecore.Marketing.Automation.Activity.BaseListener" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="truePathKey">The key of the path to take when the condition evaluates to true.</param>
        /// <param name="falsePathKey">The key of the path to take when the condition evaluates to false.</param>
        protected PredictiveRevenueListener(ILogger<IActivity> logger, string truePathKey, string falsePathKey)
            : base(logger)
        {
            this.TruePathKey = truePathKey;
            this.FalsePathKey = falsePathKey;
        }

        public string TruePathKey { get; }

        public string FalsePathKey { get; }

        public override ActivityResult Invoke(IContactProcessingContext context)
        {
            Condition.Requires(context, nameof(context)).IsNotNull();
            if (this.ShouldMove(context))
            {
                return this.TruePathKey == null ? new SuccessMove("false") : new SuccessMove(this.TruePathKey) as ActivityResult;
            }

            return (ActivityResult) new SuccessMove("true");
        }


        protected bool ShouldMove(IContactProcessingContext context)
        {
            //var timeSeries = ExtractHistory(context.Contact);
            //var timeSeriesService = new TimeseriesService();
            //var latest = timeSeries.Last()?.Value;
            //var predicted = timeSeriesService.PredictNext(timeSeries, Days);


            var forecastService = new ForecastService();
            var contactValue = (int)ExtractTotalMonetary(context.Contact);

            var predictionSegment = forecastService.GetSegmentType(contactValue, ForecastRule.M);

            switch (Lifecycle)
            {
                case CollectionLifecycleOperation.High:
                    return predictionSegment == SegmentType.Hight;
                case CollectionLifecycleOperation.Medium:
                    return predictionSegment == SegmentType.Medium;
                case CollectionLifecycleOperation.Low:
                    return predictionSegment == SegmentType.Low;
            }

           // return this.Condition.Evaluates(this.Services, this._conditionExpressionBuilder, context);
            return false;
        }

        List<TimeSeriesValue> ExtractHistory(Contact contact)
        {
            var data = contact.Interactions.SelectMany(x => x.Events.OfType<Outcome>())
                .Where(x => x.MonetaryValue > 0)
                .Select(x => new TimeSeriesValue
                {
                    Value = (float)x.MonetaryValue,
                    Timestamp = x.Timestamp.Date
                }).OrderBy(x => x.Timestamp).ToList();
            return data;
        }

        float ExtractTotalMonetary(Contact contact)
        {
            var sum = contact.Interactions.SelectMany(x => x.Events.OfType<Outcome>())
                .Where(x => x.MonetaryValue > 0)
                .Select(x => (float)x.MonetaryValue).Sum();
            return sum;
        }

    }
}