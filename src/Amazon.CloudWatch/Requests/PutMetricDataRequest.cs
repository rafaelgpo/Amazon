﻿using System.Collections.ObjectModel;

namespace Amazon.CloudWatch
{
    public class PutMetricDataRequest : Collection<MetricDatum>
    {     
        public string Namespace { get; set; }
        
        public AwsRequest ToParams()
        {
            var parameters = new AwsRequest {
                { "Action", "PutMetricData" }
            };

            if (Namespace != null)
            {
                parameters.Add("Namespace", Namespace);
            }

            for (int i = 0; i < Count; i++)
            {
                var datum = this[i];

                var prefix = "MetricData.member." + (i + 1) + ".";

                parameters.Add(prefix + "MetricName", datum.MetricName);
                parameters.Add(prefix + "Unit",       datum.Unit);
                parameters.Add(prefix + "Value",      datum.Value.ToString());

                if (datum.Dimensions != null)
                {
                    for (int i2 = 0; i < datum.Dimensions.Length; i++)
                    {
                        var dimension = datum.Dimensions[i2];

                        var prefix2 = prefix + "Dimensions.member." + (i2 + 1) + ".";

                        parameters.Add(prefix2 + "Name",  dimension.Name);
                        parameters.Add(prefix2 + "Value", dimension.Name);
                    }
                }            
            }

            return parameters;
        }
    }
}
