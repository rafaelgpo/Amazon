﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Amazon.S3
{
    public class GetObjectRequest : S3Request
    {
        public GetObjectRequest(string host, string bucketName, string objectName)
            : base(HttpMethod.Get, host, bucketName, objectName)
        {
            CompletionOption = HttpCompletionOption.ResponseHeadersRead;
        }

        public DateTimeOffset? IfModifiedSince
        {
            get => Headers.IfModifiedSince;
            set => Headers.IfModifiedSince = value;
        }


        public string IfNoneMatch
        {
            set
            {
                if (value == null)
                {
                    Headers.IfNoneMatch.Clear();
                }
                else
                {
                    Headers.IfNoneMatch.Add(new EntityTagHeaderValue(value));
                }
            }
        }

        internal void SetCustomerEncryptionKey(ServerSideEncryptionKey key)
        {
            Headers.Add("x-amz-server-side-encryption-customer-algorithm", key.Algorithm);
            Headers.Add("x-amz-server-side-encryption-customer-key",       Convert.ToBase64String(key.Key));
            Headers.Add("x-amz-server-side-encryption-customer-key-MD5",   Convert.ToBase64String(key.KeyMD5));
        }                                                        
                                                                  
        public void SetRange(long? from, long? to)
        {
            Headers.Range = new RangeHeaderValue(from, to);
        }
    }
}