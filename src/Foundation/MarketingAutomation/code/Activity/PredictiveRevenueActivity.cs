using Microsoft.Extensions.Logging;
using Sitecore.Framework.Conditions;
using Sitecore.Marketing.Automation.Activity;
using Sitecore.Marketing.Automation.Activity.Extensions;
using Sitecore.Marketing.Rules;
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
                return this.TruePathKey == null ? new SuccessMove() : new SuccessMove(this.TruePathKey) as ActivityResult;

            return (ActivityResult) new SuccessMove();
        }


        protected bool ShouldMove(IContactProcessingContext context)
        {
           // return this.Condition.Evaluates(this.Services, this._conditionExpressionBuilder, context);
           return false;
        }
    }
}