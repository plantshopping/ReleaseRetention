using System;
using System.Collections.Generic;
using System.Text;

namespace ReleasesRetention
{
    public class ReleaseRetentionResultFormatter
    {
        public string Format(List<ReleaseRetentionResult> result)
        {
            var stringBuilder = new StringBuilder();

            result.ForEach(item => stringBuilder.Append($"{item.ReleaseId} kept because it was the most recently deployed to {item.EnvironmentId}\n"));

            return stringBuilder.ToString();
        }
    }
}
