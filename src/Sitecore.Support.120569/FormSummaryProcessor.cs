using Sitecore.Analytics.Aggregation.Pipeline;
using Sitecore.Analytics.Model;
using Sitecore.Diagnostics;
using Sitecore.WFFM.Abstractions.Analytics;
using System.Collections.Generic;
using System.Linq;
using Sitecore.WFFM.Analytics.Aggregation.Processors.FormSummary;

namespace Sitecore.Support.WFFM.Analytics.Aggregation.Processors.FormSummary
{
    public class FormSummaryProcessor : AggregationProcessor
    {
        protected override void OnProcess(AggregationPipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            VisitData visit = args.Context.Visit;
            if (visit.Pages != null)
            {
                foreach (PageData data2 in visit.Pages)
                {
                    if (data2.PageEvents != null)
                    {
                        Sitecore.WFFM.Analytics.Aggregation.Processors.FormSummary.FormSummary fact = null;
                        foreach (PageEventData data3 in data2.PageEvents)
                        {
                            if ((data3.PageEventDefinitionId == IDs.FormSubmitSuccessEventId) && data3.CustomValues.ContainsKey(Sitecore.WFFM.Abstractions.Analytics.Constants.WffmKey))
                            {
                                List<FieldData> list = data3.CustomValues[Sitecore.WFFM.Abstractions.Analytics.Constants.WffmKey] as List<FieldData>;
                                if (list == null)
                                {
                                    IEnumerable<FieldData> collection = data3.CustomValues[Sitecore.WFFM.Abstractions.Analytics.Constants.WffmKey] as List<FieldData>;
                                    if (collection == null)
                                    {
                                        continue;
                                    }
                                    list = new List<FieldData>(collection);
                                }
                                foreach (FieldData data4 in list)
                                {
                                    foreach (string str in ((data4.Values != null) && (data4.Values.Count > 0)) ? data4.Values : (!string.IsNullOrEmpty(data4.Value) ? Enumerable.Repeat<string>(data4.Value, 1) : Enumerable.Repeat<string>(string.Empty, 1)))
                                    {
                                        FormSummaryKey key = new FormSummaryKey
                                        {
                                            FormId = data3.ItemId,
                                            FieldId = data4.FieldId,
                                            FieldName = data4.FieldName,
                                            FieldValueId = args.GetDimension<Sitecore.Support.WFFM.Analytics.Aggregation.Processors.FormFieldValues.FormFieldValues>().Add(data4.FieldName, str)
                                        };
                                        FormSummaryValue value2 = new FormSummaryValue
                                        {
                                            Count = 1
                                        };
                                        if (fact == null)
                                        {
                                            fact = args.GetFact<Sitecore.WFFM.Analytics.Aggregation.Processors.FormSummary.FormSummary>();
                                        }
                                        fact.Emit(key, value2);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}