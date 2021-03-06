using System.Collections.Generic;
using System.Linq;
using Hackathon.MLBox.Foundation.Engine.Mappers;
using Hackathon.MLBox.Foundation.Shared.Models.Metrics;
using Hackathon.MLBox.Foundation.Shared.Models.Sitecore;
using Sitecore.Processing.Engine.Projection;

namespace Hackathon.MLBox.Foundation.Engine.Services
{
    public partial class ForecastService : IForecastService
    {
        private SegmentationService _segmentationService;

        public ForecastService()
        {
            _segmentationService = new SegmentationService();
        }

        public void Train(IReadOnlyList<IDataRow> data)
        {
            var customersData = CustomerModelMapper.MapToCustomers(data);
            var segmentationService =  new SegmentationService();
            
            segmentationService.RebuildSegments(customersData);
            
            //var calculatedScores = segmentationService.RebuildCustomerMetrics(customersData);
            //var businessData = calculatedScores.Select(x => new RFMScore()
            //{
            //    R = x.RFM.R,
            //    F = x.RFM.F,
            //    M = x.RFM.M
            //}).ToList();
        }


        public SegmentType GetSegmentType(int value, ForecastRule rule)
        {
            var segments = _segmentationService.GetSegments();
            if (segments != null)
            {
                switch (rule)
                {
                    case ForecastRule.M:
                        {
                            if (value <= segments.MonetaryQ1)
                                return SegmentType.Low;

                            if (value > segments.MonetaryQ1 && value <= segments.MonetaryQ3)
                                return SegmentType.Medium;

                            if (value > segments.MonetaryQ3)
                                return SegmentType.Hight;
                        }
                        break;

                    case ForecastRule.R:
                        {
                            if (value <= segments.RecencyQ1)
                                return SegmentType.Low;

                            if (value > segments.RecencyQ1 && value <= segments.RecencyQ3)
                                return SegmentType.Medium;

                            if (value > segments.RecencyQ3)
                                return SegmentType.Hight;
                        }
                        break;

                    case ForecastRule.F:
                        {
                            if (value <= segments.FrequencyQ1)
                                return SegmentType.Low;

                            if (value > segments.FrequencyQ1 && value <= segments.FrequencyQ3)
                                return SegmentType.Medium;

                            if (value > segments.FrequencyQ3)
                                return SegmentType.Hight;
                        }
                        break;

                }
            }

            return SegmentType.Unknown;
        }
    }

    public interface IForecastService
    {
        void Train(IReadOnlyList<IDataRow> data);
        SegmentType GetSegmentType(int value, ForecastRule rule);
    }
}
