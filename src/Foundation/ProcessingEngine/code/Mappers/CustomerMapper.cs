using System.Collections.Generic;
using System.Linq;
using Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Models;
using Sitecore.Processing.Engine.Projection;

namespace Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Mappers
{
    public static class CustomerMapper
    {

        public static List<PurchaseInvoice> MapToCustomers(IReadOnlyList<IDataRow> dataRows)
        {
            return dataRows.Select(data => data.ToPurchaseOutcome()).ToList();
        }

        public static PurchaseInvoice ToPurchaseOutcome(this IDataRow dataRow)
        {
            var result = new PurchaseInvoice();
            var customerId = dataRow.Schema.Fields.FirstOrDefault(x => x.Name == nameof(PurchaseInvoice.ContactId));
            if (customerId != null)
            {
                result.ContactId = dataRow.GetGuid(dataRow.Schema.GetFieldIndex(nameof(PurchaseInvoice.ContactId)));
            }

            var date = dataRow.Schema.Fields.FirstOrDefault(x => x.Name == nameof(PurchaseInvoice.Timestamp));
            if (date != null)
            {
                result.Timestamp = dataRow.GetDateTime(dataRow.Schema.GetFieldIndex(nameof(PurchaseInvoice.Timestamp)));
            }

            var price = dataRow.Schema.Fields.FirstOrDefault(x => x.Name == nameof(PurchaseInvoice.Value));
            if (price != null)
            {
                result.Value = (float) dataRow.GetDouble(dataRow.Schema.GetFieldIndex(nameof(PurchaseInvoice.Value)));
            }

            return result;
        }
     
    }
}
