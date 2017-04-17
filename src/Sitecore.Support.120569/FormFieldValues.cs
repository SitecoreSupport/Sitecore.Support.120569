using Sitecore.Analytics.Aggregation.Data.Model;
using System;
using System.Security.Cryptography;
using System.Text;
using Sitecore.WFFM.Analytics.Aggregation.Processors.FormFieldValues;

namespace Sitecore.Support.WFFM.Analytics.Aggregation.Processors.FormFieldValues
{
    public class FormFieldValues : Dimension<FormFieldValuesKey, FormFieldValuesValue>
    {
        public long Add(string fieldName, string fieldValue)
        {
            long result;
            using (MD5 mD = MD5.Create())
            {
                long num = BitConverter.ToInt64(mD.ComputeHash(Encoding.UTF8.GetBytes(fieldName + fieldValue)), 0);
                base.Add(new FormFieldValuesKey(num), new FormFieldValuesValue(fieldValue));
                result = num;
            }
            return result;
        }
    }
}