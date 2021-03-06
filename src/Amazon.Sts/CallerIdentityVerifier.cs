﻿using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Sts
{
    public class CallerIdentityVerifier 
    {
        protected readonly HttpClient httpClient = new HttpClient();

        public async Task<GetCallerIdentityResult> VerifyCallerIdentityAsync(CallerIdentityVerificationParameters token)
        {
            #region Preconditions

            if (token == null)
                throw new ArgumentNullException(nameof(token));

            var uri = new Uri(token.Url);

            if (uri.Scheme != "https")
                throw new ArgumentException("endpoint must be HTTPS. was :" + uri.Scheme);

            // https://sts.us-east-1.amazonaws.com/

            if (!(uri.Host.StartsWith("sts.") && uri.Host.EndsWith(".amazonaws.com")))
                throw new Exception("Must be an STS endpoint: was:" + token.Url);

            #endregion

            var request = new HttpRequestMessage(HttpMethod.Post, token.Url) {
                Content = new StringContent(token.Body, Encoding.UTF8, "application/x-www-form-urlencoded")
            };

            foreach (var header in token.Headers)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value.ToString());
            }

            request.Headers.UserAgent.ParseAdd("Carbon/1.6.0");
            request.Headers.Host = uri.Host;

            // throw new Exception(JsonObject.FromObject(request).ToString(true));

            // Our message should be signed

            using (var response = await httpClient.SendAsync(request).ConfigureAwait(false))
            {
                var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("ERROR:" + response.StatusCode + "/" + responseText);
                }

                return StsResponseHelper<GetCallerIdentityResponse>.ParseXml(responseText).GetCallerIdentityResult;
            }
        }
      
    }
}
