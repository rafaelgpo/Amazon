﻿using System;
using System.Threading.Tasks;

namespace Amazon
{
    using Helpers;

    // AccessKey

    public class AwsCredential : IAwsCredential
    {
        public AwsCredential(string accessKeyId, string secretAccessKey)
        {
            AccessKeyId     = accessKeyId     ?? throw new ArgumentNullException(nameof(accessKeyId));
            SecretAccessKey = secretAccessKey ?? throw new ArgumentNullException(nameof(secretAccessKey));
        }

        // 16 - 32 characters
        public string AccessKeyId { get; }

        public string SecretAccessKey { get; }

        public string SecurityToken => null;

        // {id}:{secret}

        public static AwsCredential Parse(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var parts = text.Split(Seperators.Colon);

            if (parts.Length != 2)
            {
                throw new Exception("AccessKeyId & SecretAccessKey should be seperated be ':'");
            }

            return new AwsCredential(parts[0], parts[1]);
        }

        #region Renewable

        public bool ShouldRenew => false;

        public Task<bool> RenewAsync()
        {
            throw new NotImplementedException("AwsCredential is not renewable");
        }

        #endregion
    }
}