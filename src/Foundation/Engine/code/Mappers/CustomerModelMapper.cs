using System.Collections.Generic;
using System.Linq;
using Hackathon.MLBox.Foundation.Common.Models.Sitecore;
using Sitecore.Processing.Engine.Projection;

namespace Hackathon.MLBox.Foundation.Engine.Mappers
{
    /// <summary>
    /// Mapping customer models
    /// </summary>
    public static class CustomerModelMapper
    {

        public static List<InvoiceItem> MapToCustomers(IReadOnlyList<IDataRow> dataRows)
        {
            return dataRows.Select(data => data.ToPurchaseOutcome()).ToList();
        }


        public static InvoiceItem ToPurchaseOutcome(this IDataRow dataRow)
        {
            var result = new InvoiceItem();
            
            var customerId = dataRow.Schema.Fields.FirstOrDefault(x => x.Name == nameof(InvoiceItem.ContactId));
            if (customerId != null)
                result.ContactId = dataRow.GetGuid(dataRow.Schema.GetFieldIndex(nameof(InvoiceItem.ContactId)));

            var date = dataRow.Schema.Fields.FirstOrDefault(x => x.Name == nameof(InvoiceItem.Timestamp));
            if (date != null)
                result.Timestamp = dataRow.GetDateTime(dataRow.Schema.GetFieldIndex(nameof(InvoiceItem.Timestamp)));

            var price = dataRow.Schema.Fields.FirstOrDefault(x => x.Name == nameof(InvoiceItem.Value));
            if (price != null)
                result.Value = (float) dataRow.GetDouble(dataRow.Schema.GetFieldIndex(nameof(InvoiceItem.Value)));

            return result;
        }
     
    }
}
