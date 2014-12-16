﻿/*******************************************************************************
 *  Copyright 2008-2013 Amazon.com, Inc. or its affiliates. All Rights Reserved.
 *  Licensed under the Apache License, Version 2.0 (the "License"). You may not use
 *  this file except in compliance with the License. A copy of the License is located at
 *
 *  http://aws.amazon.com/apache2.0
 *
 *  or in the "license" file accompanying this file.
 *  This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
 *  CONDITIONS OF ANY KIND, either express or implied. See the License for the
 *  specific language governing permissions and limitations under the License.
 * *****************************************************************************
 *    __  _    _  ___
 *   (  )( \/\/ )/ __)
 *   /__\ \    / \__ \
 *  (_)(_) \/\/  (___/
 *
 *  AWS SDK for .NET
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Security.Cryptography;
using System.Text;
using Amazon.Runtime;

namespace Amazon.Runtime.Internal.Util
{
    public partial class HashingWrapper : IHashingWrapper
    {
        //private HashAlgorithm _algorithm;
        private void Init(string algorithmName)
        {
            throw new NotImplementedException();
        }

        #region IHashingWrapper Members

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public byte[] ComputeHash(byte[] buffer)
        {
            throw new NotImplementedException();
        }

        public byte[] ComputeHash(Stream stream)
        {
            throw new NotImplementedException();
        }

        public void AppendBlock(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public byte[] AppendLastBlock(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class HashingWrapperMD5 : HashingWrapper
    {
        private static string md5AlgorithmName;// = typeof(MD5).FullName;

        public HashingWrapperMD5()
            : base(md5AlgorithmName)
        { }
    }
}