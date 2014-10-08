/*
 * Copyright 2010-2014 Amazon.com, Inc. or its affiliates. All Rights Reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * A copy of the License is located at
 * 
 *  http://aws.amazon.com/apache2.0
 * 
 * or in the "license" file accompanying this file. This file is distributed
 * on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied. See the License for the specific language governing
 * permissions and limitations under the License.
 */


using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.Model.Internal.MarshallTransformations;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Auth;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.DynamoDBv2
{
    /// <summary>
    /// Implementation for accessing DynamoDB
    ///
    /// Amazon DynamoDB    <b>Overview</b>    
    /// <para>
    /// This is the Amazon DynamoDB API Reference. This guide provides descriptions and samples
    /// of the low-level      DynamoDB API. For information about DynamoDB application development,
    /// go to the <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/">Amazon
    /// DynamoDB Developer Guide</a>.
    /// </para>
    ///     
    /// <para>
    /// Instead of making the requests to the low-level DynamoDB API directly from your application,
    /// we      recommend that you use the AWS Software Development Kits (SDKs). The easy-to-use
    /// libraries in      the AWS SDKs make it unnecessary to call the low-level DynamoDB
    /// API directly from your      application. The libraries take care of request authentication,
    /// serialization, and connection      management. For more information, go to <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/UsingAWSSDK.html">Using
    /// the AWS        SDKs with DynamoDB</a> in the <i>Amazon DynamoDB Developer Guide</i>.
    /// </para>
    ///     
    /// <para>
    /// If you decide to code against the low-level DynamoDB API directly, you will need to
    /// write the      necessary code to authenticate your requests. For more information
    /// on signing your requests,      go to <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/API.html">Using
    /// the DynamoDB API</a> in the      <i>Amazon DynamoDB Developer Guide</i>.
    /// </para>
    ///     
    /// <para>
    /// The following are short descriptions of each low-level API action, organized by function.
    /// </para>
    ///     
    /// <para>
    ///       <b>Managing Tables</b>    
    /// </para>
    ///     
    /// <para>
    ///       <ul>        <li>          
    /// <para>
    /// <i>CreateTable</i> - Creates a table with user-specified provisioned throughput  
    ///          settings. You must designate one attribute as the hash primary key for the
    /// table; you            can optionally designate a second attribute as the range primary
    /// key. DynamoDB creates            indexes on these key attributes for fast data access.
    /// Optionally, you can create one or            more secondary indexes, which provide
    /// fast data access using non-key attributes.
    /// </para>
    ///         </li>        <li>          
    /// <para>
    /// <i>DescribeTable</i> - Returns metadata for a table, such as table size, status, and
    ///            index information.
    /// </para>
    ///         </li>        <li>          
    /// <para>
    /// <i>UpdateTable</i> - Modifies the provisioned throughput settings for a table.   
    ///         Optionally, you can modify the provisioned throughput settings for global
    /// secondary indexes on the            table.
    /// </para>
    ///         </li>        <li>          
    /// <para>
    /// <i>ListTables</i> - Returns a list of all tables associated with the current AWS 
    ///           account and endpoint.
    /// </para>
    ///         </li>        <li>          
    /// <para>
    /// <i>DeleteTable</i> - Deletes a table and all of its indexes.
    /// </para>
    ///         </li>      </ul>    
    /// </para>
    ///     
    /// <para>
    /// For conceptual information about managing tables, go to <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/WorkingWithTables.html">Working
    /// with Tables</a> in the        <i>Amazon DynamoDB Developer Guide</i>.
    /// </para>
    ///     
    /// <para>
    ///       <b>Reading Data</b>    
    /// </para>
    ///     
    /// <para>
    ///       <ul>        <li>          
    /// <para>
    /// <i>GetItem</i> - Returns a set of attributes for the item that has a given primary
    /// key.            By default, <i>GetItem</i> performs an eventually consistent read;
    /// however, applications            can specify a strongly consistent read instead.
    /// </para>
    ///         </li>        <li>          
    /// <para>
    /// <i>BatchGetItem</i> - Performs multiple <i>GetItem</i> requests for data items using
    ///            their primary keys, from one table or multiple tables. The response from
    ///              <i>BatchGetItem</i> has a size limit of 1 MB and returns a maximum of
    ///            100 items. Both eventually consistent and strongly            consistent
    /// reads can be used.
    /// </para>
    ///         </li>        <li>          
    /// <para>
    /// <i>Query</i> - Returns one or more items from a table or a secondary index. You must
    /// provide a            specific hash key value. You can narrow the scope of the query
    /// using comparison            operators against a range key value, or on the index key.
    /// <i>Query</i> supports either            eventual or strong consistency. A single response
    /// has a size limit of            1 MB.
    /// </para>
    ///         </li>        <li>          
    /// <para>
    /// <i>Scan</i> - Reads every item in a table; the result set is eventually consistent.
    /// You            can limit the number of items returned by filtering the data attributes,
    /// using            conditional expressions. <i>Scan</i> can be used to enable ad-hoc
    /// querying of a table            against non-key attributes; however, since this is
    /// a full table scan without using an            index, <i>Scan</i> should not be used
    /// for any application query use case that requires            predictable performance.
    /// </para>
    ///         </li>      </ul>    
    /// </para>
    ///     
    /// <para>
    /// For conceptual information about reading data, go to <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/WorkingWithItems.html">Working
    /// with Items</a> and <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/QueryAndScan.html">Query
    /// and Scan Operations</a> in the        <i>Amazon DynamoDB Developer Guide</i>.
    /// </para>
    ///     
    /// <para>
    ///       <b>Modifying Data</b>    
    /// </para>
    ///     
    /// <para>
    ///       <ul>        <li>          
    /// <para>
    /// <i>PutItem</i> - Creates a new item, or replaces an existing item with a new item
    ///            (including all the attributes). By default, if an item in the table already
    /// exists with            the same primary key, the new item completely replaces the
    /// existing item. You can use            conditional operators to replace an item only
    /// if its attribute values match certain            conditions, or to insert a new item
    /// only if that item doesn't already exist.
    /// </para>
    ///         </li>        <li>          
    /// <para>
    /// <i>UpdateItem</i> - Modifies the attributes of an existing item. You can also use
    ///            conditional operators to perform an update only if the item's attribute
    /// values match            certain conditions.
    /// </para>
    ///         </li>        <li>          
    /// <para>
    /// <i>DeleteItem</i> - Deletes an item in a table by primary key. You can use conditional
    ///            operators to perform a delete an item only if the item's attribute values
    /// match certain            conditions.
    /// </para>
    ///         </li>        <li>          
    /// <para>
    /// <i>BatchWriteItem</i> - Performs multiple <i>PutItem</i> and <i>DeleteItem</i> requests
    ///            across multiple tables in a single request. A failure of any request(s)
    /// in the batch            will not cause the entire <i>BatchWriteItem</i> operation
    /// to fail. Supports batches of            up to 25 items to put or delete, with a maximum
    /// total            request size of 1 MB. 
    /// </para>
    ///         </li>      </ul>    
    /// </para>
    ///     
    /// <para>
    /// For conceptual information about modifying data, go to <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/WorkingWithItems.html">Working
    /// with Items</a> and <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/QueryAndScan.html">Query
    /// and Scan Operations</a> in the        <i>Amazon DynamoDB Developer Guide</i>.
    /// </para>
    /// </summary>
    public partial class AmazonDynamoDBClient : AmazonServiceClient, IAmazonDynamoDB
    {
        
        #region Constructors

        /// <summary>
        /// Constructs AmazonDynamoDBClient with AWS Credentials
        /// </summary>
        /// <param name="credentials">AWS Credentials</param>
        public AmazonDynamoDBClient(AWSCredentials credentials)
            : this(credentials, new AmazonDynamoDBConfig())
        {
        }

        /// <summary>
        /// Constructs AmazonDynamoDBClient with AWS Credentials
        /// </summary>
        /// <param name="credentials">AWS Credentials</param>
        /// <param name="region">The region to connect.</param>
        public AmazonDynamoDBClient(AWSCredentials credentials, RegionEndpoint region)
            : this(credentials, new AmazonDynamoDBConfig{RegionEndpoint = region})
        {
        }

        /// <summary>
        /// Constructs AmazonDynamoDBClient with AWS Credentials and an
        /// AmazonDynamoDBClient Configuration object.
        /// </summary>
        /// <param name="credentials">AWS Credentials</param>
        /// <param name="clientConfig">The AmazonDynamoDBClient Configuration Object</param>
        public AmazonDynamoDBClient(AWSCredentials credentials, AmazonDynamoDBConfig clientConfig)
            : base(credentials, clientConfig)
        {
        }

        /// <summary>
        /// Constructs AmazonDynamoDBClient with AWS Access Key ID and AWS Secret Key
        /// </summary>
        /// <param name="awsAccessKeyId">AWS Access Key ID</param>
        /// <param name="awsSecretAccessKey">AWS Secret Access Key</param>
        public AmazonDynamoDBClient(string awsAccessKeyId, string awsSecretAccessKey)
            : this(awsAccessKeyId, awsSecretAccessKey, new AmazonDynamoDBConfig())
        {
        }

        /// <summary>
        /// Constructs AmazonDynamoDBClient with AWS Access Key ID and AWS Secret Key
        /// </summary>
        /// <param name="awsAccessKeyId">AWS Access Key ID</param>
        /// <param name="awsSecretAccessKey">AWS Secret Access Key</param>
        /// <param name="region">The region to connect.</param>
        public AmazonDynamoDBClient(string awsAccessKeyId, string awsSecretAccessKey, RegionEndpoint region)
            : this(awsAccessKeyId, awsSecretAccessKey, new AmazonDynamoDBConfig() {RegionEndpoint=region})
        {
        }

        /// <summary>
        /// Constructs AmazonDynamoDBClient with AWS Access Key ID, AWS Secret Key and an
        /// AmazonDynamoDBClient Configuration object. 
        /// </summary>
        /// <param name="awsAccessKeyId">AWS Access Key ID</param>
        /// <param name="awsSecretAccessKey">AWS Secret Access Key</param>
        /// <param name="clientConfig">The AmazonDynamoDBClient Configuration Object</param>
        public AmazonDynamoDBClient(string awsAccessKeyId, string awsSecretAccessKey, AmazonDynamoDBConfig clientConfig)
            : base(awsAccessKeyId, awsSecretAccessKey, clientConfig)
        {
        }

        /// <summary>
        /// Constructs AmazonDynamoDBClient with AWS Access Key ID and AWS Secret Key
        /// </summary>
        /// <param name="awsAccessKeyId">AWS Access Key ID</param>
        /// <param name="awsSecretAccessKey">AWS Secret Access Key</param>
        /// <param name="awsSessionToken">AWS Session Token</param>
        public AmazonDynamoDBClient(string awsAccessKeyId, string awsSecretAccessKey, string awsSessionToken)
            : this(awsAccessKeyId, awsSecretAccessKey, awsSessionToken, new AmazonDynamoDBConfig())
        {
        }

        /// <summary>
        /// Constructs AmazonDynamoDBClient with AWS Access Key ID and AWS Secret Key
        /// </summary>
        /// <param name="awsAccessKeyId">AWS Access Key ID</param>
        /// <param name="awsSecretAccessKey">AWS Secret Access Key</param>
        /// <param name="awsSessionToken">AWS Session Token</param>
        /// <param name="region">The region to connect.</param>
        public AmazonDynamoDBClient(string awsAccessKeyId, string awsSecretAccessKey, string awsSessionToken, RegionEndpoint region)
            : this(awsAccessKeyId, awsSecretAccessKey, awsSessionToken, new AmazonDynamoDBConfig{RegionEndpoint = region})
        {
        }

        /// <summary>
        /// Constructs AmazonDynamoDBClient with AWS Access Key ID, AWS Secret Key and an
        /// AmazonDynamoDBClient Configuration object. 
        /// </summary>
        /// <param name="awsAccessKeyId">AWS Access Key ID</param>
        /// <param name="awsSecretAccessKey">AWS Secret Access Key</param>
        /// <param name="awsSessionToken">AWS Session Token</param>
        /// <param name="clientConfig">The AmazonDynamoDBClient Configuration Object</param>
        public AmazonDynamoDBClient(string awsAccessKeyId, string awsSecretAccessKey, string awsSessionToken, AmazonDynamoDBConfig clientConfig)
            : base(awsAccessKeyId, awsSecretAccessKey, awsSessionToken, clientConfig)
        {
        }

        #endregion

        #region Overrides

        protected override AbstractAWSSigner CreateSigner()
        {
            return new AWS4Signer();
        } 

        protected override void CustomizeRuntimePipeline(RuntimePipeline pipeline)
        {
            pipeline.ReplaceHandler<Amazon.Runtime.Internal.RetryHandler>(new Amazon.Runtime.Internal.RetryHandler(new Amazon.DynamoDBv2.Internal.DynamoDBRetryPolicy(this.Config.MaxErrorRetry)));
        }

        #endregion

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #endregion

        
        #region  BatchGetItem

        /// <summary>
        /// The <i>BatchGetItem</i> operation returns the attributes of one or more items from
        /// one or      more tables. You identify requested items by primary key.
        /// 
        ///     
        /// <para>
        /// A single operation can retrieve up to 1 MB of data, which can contain as many    
        ///  as 100 items. <i>BatchGetItem</i> will return a partial result      if the response
        /// size limit is exceeded, the table's provisioned throughput is exceeded, or an    
        ///  internal processing failure occurs. If a partial result is returned, the operation
        /// returns a      value for <i>UnprocessedKeys</i>. You can use this value to retry the
        /// operation starting with      the next item to get.
        /// </para>
        ///     
        /// <para>
        /// For example, if you ask to retrieve 100 items, but each individual item is 50 KB in
        /// size, the      system returns 20 items (1 MB) and an appropriate <i>UnprocessedKeys</i>
        /// value      so you can get the next page of results. If desired, your application can
        /// include its own      logic to assemble the pages of results into one dataset.
        /// </para>
        ///     
        /// <para>
        /// If <i>none</i> of the items can be processed due to insufficient provisioned throughput
        /// on      all of the tables in the request, then <i>BatchGetItem</i> will throw a  
        ///      <i>ProvisionedThroughputExceededException</i>. If <i>at least one</i> of the
        /// items is      successfully processed, then <i>BatchGetItem</i> completes successfully,
        /// while returning the      keys of the unread items in <i>UnprocessedKeys</i>.
        /// </para>
        ///     <important>
        /// <para>
        /// If DynamoDB returns any unprocessed items, you should retry the batch operation on
        /// those items. However, <i>we strongly recommend that you use an exponential backoff
        /// algorithm</i>. If you retry the batch operation immediately, the underlying read or
        /// write requests can still fail due to throttling on the individual tables. If you delay
        /// the batch operation using exponential backoff, the individual requests in the batch
        /// are much more likely to succeed.
        /// </para>
        ///       
        /// <para>
        /// For more information, go to <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ErrorHandling.html#BatchOperations">Batch
        /// Operations and Error Handling</a> in the Amazon DynamoDB Developer Guide.
        /// </para>
        /// </important>    
        /// <para>
        /// By default, <i>BatchGetItem</i> performs eventually consistent reads on every table
        /// in the      request. If you want strongly consistent reads instead, you can set <i>ConsistentRead</i>
        /// to        <code>true</code> for any or all tables.
        /// </para>
        ///     
        /// <para>
        /// In order to minimize response latency, <i>BatchGetItem</i> retrieves items in parallel.
        /// </para>
        ///     
        /// <para>
        /// When designing your application, keep in mind that DynamoDB does not return attributes
        /// in any      particular order. To help parse the response by item, include the primary
        /// key values for the      items in your request in the <i>AttributesToGet</i> parameter.
        /// </para>
        ///     
        /// <para>
        /// If a requested item does not exist, it is not returned in the result. Requests for
        ///      nonexistent items consume the minimum read capacity units according to the type
        /// of read. For      more information, see <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/WorkingWithTables.html#CapacityUnitCalculations">Capacity
        /// Units Calculations</a> in the Amazon DynamoDB Developer Guide.
        /// </para>
        /// </summary>
        /// <param name="requestItems">A map of one or more table names and, for each table, the corresponding primary keys for the   items to retrieve. Each table name can be invoked only once. Each element in the map consists of the following: <ul>   <li>  <i>Keys</i> - An array of primary key attribute values that define specific items in the    table. For each primary key, you must provide <i>all</i> of the key attributes. For    example, with a hash type primary key, you only need to specify the hash attribute. For a    hash-and-range type primary key, you must specify <i>both</i> the hash attribute and the    range attribute.   </li>   <li>  <i>AttributesToGet</i> - One or more attributes to be retrieved from the table. By    default, all attributes are returned. If a specified attribute is not found, it does not    appear in the result.  Note that <i>AttributesToGet</i> has no effect on provisioned throughput consumption.    DynamoDB determines capacity units consumed based on item size, not on the amount of data    that is returned to an application.   </li>   <li>  <i>ConsistentRead</i> - If <code>true</code>, a strongly consistent read is used; if   <code>false</code> (the default), an eventually consistent read is used.   </li> </ul></param>
        /// <param name="returnConsumedCapacity">A property of BatchGetItemRequest used to execute the BatchGetItem service method.</param>
        /// 
        /// <returns>The response from the BatchGetItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="ProvisionedThroughputExceededException">
        /// The request rate is too high, or the request is too large, for the available throughput
        /// to      accommodate. The AWS SDKs automatically retry requests that receive this exception;
        /// therefore,      your request will eventually succeed, unless the request is too large
        /// or your retry queue is      too large to finish. Reduce the frequency of requests
        /// by using the strategies listed in <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ErrorHandling.html#APIRetries">Error
        /// Retries and Exponential Backoff</a>      in the Amazon DynamoDB Developer Guide.
        /// </exception>
        /// <exception cref="ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource may not be
        /// specified      correctly, or its status may not be <code>ACTIVE</code>.
        /// </exception>
        public BatchGetItemResponse BatchGetItem(Dictionary<string, KeysAndAttributes> requestItems, ReturnConsumedCapacity returnConsumedCapacity)
        {
            var request = new BatchGetItemRequest();
            request.RequestItems = requestItems;
            request.ReturnConsumedCapacity = returnConsumedCapacity;
            return BatchGetItem(request);
        }

        /// <summary>
        /// The <i>BatchGetItem</i> operation returns the attributes of one or more items from
        /// one or      more tables. You identify requested items by primary key.
        /// 
        ///     
        /// <para>
        /// A single operation can retrieve up to 1 MB of data, which can contain as many    
        ///  as 100 items. <i>BatchGetItem</i> will return a partial result      if the response
        /// size limit is exceeded, the table's provisioned throughput is exceeded, or an    
        ///  internal processing failure occurs. If a partial result is returned, the operation
        /// returns a      value for <i>UnprocessedKeys</i>. You can use this value to retry the
        /// operation starting with      the next item to get.
        /// </para>
        ///     
        /// <para>
        /// For example, if you ask to retrieve 100 items, but each individual item is 50 KB in
        /// size, the      system returns 20 items (1 MB) and an appropriate <i>UnprocessedKeys</i>
        /// value      so you can get the next page of results. If desired, your application can
        /// include its own      logic to assemble the pages of results into one dataset.
        /// </para>
        ///     
        /// <para>
        /// If <i>none</i> of the items can be processed due to insufficient provisioned throughput
        /// on      all of the tables in the request, then <i>BatchGetItem</i> will throw a  
        ///      <i>ProvisionedThroughputExceededException</i>. If <i>at least one</i> of the
        /// items is      successfully processed, then <i>BatchGetItem</i> completes successfully,
        /// while returning the      keys of the unread items in <i>UnprocessedKeys</i>.
        /// </para>
        ///     <important>
        /// <para>
        /// If DynamoDB returns any unprocessed items, you should retry the batch operation on
        /// those items. However, <i>we strongly recommend that you use an exponential backoff
        /// algorithm</i>. If you retry the batch operation immediately, the underlying read or
        /// write requests can still fail due to throttling on the individual tables. If you delay
        /// the batch operation using exponential backoff, the individual requests in the batch
        /// are much more likely to succeed.
        /// </para>
        ///       
        /// <para>
        /// For more information, go to <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ErrorHandling.html#BatchOperations">Batch
        /// Operations and Error Handling</a> in the Amazon DynamoDB Developer Guide.
        /// </para>
        /// </important>    
        /// <para>
        /// By default, <i>BatchGetItem</i> performs eventually consistent reads on every table
        /// in the      request. If you want strongly consistent reads instead, you can set <i>ConsistentRead</i>
        /// to        <code>true</code> for any or all tables.
        /// </para>
        ///     
        /// <para>
        /// In order to minimize response latency, <i>BatchGetItem</i> retrieves items in parallel.
        /// </para>
        ///     
        /// <para>
        /// When designing your application, keep in mind that DynamoDB does not return attributes
        /// in any      particular order. To help parse the response by item, include the primary
        /// key values for the      items in your request in the <i>AttributesToGet</i> parameter.
        /// </para>
        ///     
        /// <para>
        /// If a requested item does not exist, it is not returned in the result. Requests for
        ///      nonexistent items consume the minimum read capacity units according to the type
        /// of read. For      more information, see <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/WorkingWithTables.html#CapacityUnitCalculations">Capacity
        /// Units Calculations</a> in the Amazon DynamoDB Developer Guide.
        /// </para>
        /// </summary>
        /// <param name="requestItems">A map of one or more table names and, for each table, the corresponding primary keys for the   items to retrieve. Each table name can be invoked only once. Each element in the map consists of the following: <ul>   <li>  <i>Keys</i> - An array of primary key attribute values that define specific items in the    table. For each primary key, you must provide <i>all</i> of the key attributes. For    example, with a hash type primary key, you only need to specify the hash attribute. For a    hash-and-range type primary key, you must specify <i>both</i> the hash attribute and the    range attribute.   </li>   <li>  <i>AttributesToGet</i> - One or more attributes to be retrieved from the table. By    default, all attributes are returned. If a specified attribute is not found, it does not    appear in the result.  Note that <i>AttributesToGet</i> has no effect on provisioned throughput consumption.    DynamoDB determines capacity units consumed based on item size, not on the amount of data    that is returned to an application.   </li>   <li>  <i>ConsistentRead</i> - If <code>true</code>, a strongly consistent read is used; if   <code>false</code> (the default), an eventually consistent read is used.   </li> </ul></param>
        /// 
        /// <returns>The response from the BatchGetItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="ProvisionedThroughputExceededException">
        /// The request rate is too high, or the request is too large, for the available throughput
        /// to      accommodate. The AWS SDKs automatically retry requests that receive this exception;
        /// therefore,      your request will eventually succeed, unless the request is too large
        /// or your retry queue is      too large to finish. Reduce the frequency of requests
        /// by using the strategies listed in <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ErrorHandling.html#APIRetries">Error
        /// Retries and Exponential Backoff</a>      in the Amazon DynamoDB Developer Guide.
        /// </exception>
        /// <exception cref="ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource may not be
        /// specified      correctly, or its status may not be <code>ACTIVE</code>.
        /// </exception>
        public BatchGetItemResponse BatchGetItem(Dictionary<string, KeysAndAttributes> requestItems)
        {
            var request = new BatchGetItemRequest();
            request.RequestItems = requestItems;
            return BatchGetItem(request);
        }

        internal BatchGetItemResponse BatchGetItem(BatchGetItemRequest request)
        {
            var marshaller = new BatchGetItemRequestMarshaller();
            var unmarshaller = BatchGetItemResponseUnmarshaller.Instance;

            return Invoke<BatchGetItemRequest,BatchGetItemResponse>(request, marshaller, unmarshaller);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the BatchGetItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.IAmazonDynamoDB"/>
        /// </summary>
        /// 
        /// <param name="request">Container for the necessary parameters to execute the BatchGetItem operation.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<BatchGetItemResponse> BatchGetItemAsync(BatchGetItemRequest request, System.Threading.CancellationToken cancellationToken = default(CancellationToken))
        {
            var marshaller = new BatchGetItemRequestMarshaller();
            var unmarshaller = BatchGetItemResponseUnmarshaller.Instance;

            return InvokeAsync<BatchGetItemRequest,BatchGetItemResponse>(request, marshaller, 
                unmarshaller, cancellationToken);
        }

        #endregion
        
        #region  BatchWriteItem

        /// <summary>
        /// The <i>BatchWriteItem</i> operation puts or deletes multiple items in one or more
        /// tables. A      single call to <i>BatchWriteItem</i> can write up to 1 MB of data,
        /// which can      comprise as many as 25 put or delete requests. Individual items   
        ///   to be written can be as large as 64 KB.
        /// 
        ///     <note>      
        /// <para>
        /// <i>BatchWriteItem</i> cannot update items. To update items, use the <i>UpdateItem</i>
        ///        API.
        /// </para>
        ///     </note>    
        /// <para>
        /// The individual <i>PutItem</i> and <i>DeleteItem</i> operations specified in      
        ///  <i>BatchWriteItem</i> are atomic; however <i>BatchWriteItem</i> as a whole is not.
        /// If any      requested operations fail because the table's provisioned throughput is
        /// exceeded or an      internal processing failure occurs, the failed operations are
        /// returned in the        <i>UnprocessedItems</i> response parameter. You can investigate
        /// and optionally resend the      requests. Typically, you would call <i>BatchWriteItem</i>
        /// in a loop. Each iteration would      check for unprocessed items and submit a new
        /// <i>BatchWriteItem</i> request with those      unprocessed items until all items have
        /// been processed.
        /// </para>
        ///     
        /// <para>
        /// Note that if <i>none</i> of the items can be processed due to insufficient provisioned
        ///      throughput on all of the tables in the request, then <i>BatchGetItem</i> will
        /// throw a        <i>ProvisionedThroughputExceededException</i>.
        /// </para>
        ///     <important>
        /// <para>
        /// If DynamoDB returns any unprocessed items, you should retry the batch operation on
        /// those items. However, <i>we strongly recommend that you use an exponential backoff
        /// algorithm</i>. If you retry the batch operation immediately, the underlying read or
        /// write requests can still fail due to throttling on the individual tables. If you delay
        /// the batch operation using exponential backoff, the individual requests in the batch
        /// are much more likely to succeed.
        /// </para>
        ///       
        /// <para>
        /// For more information, go to <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ErrorHandling.html#BatchOperations">Batch
        /// Operations and Error Handling</a> in the Amazon DynamoDB Developer Guide.
        /// </para>
        /// </important>        
        /// <para>
        /// With <i>BatchWriteItem</i>, you can efficiently write or delete large amounts of data,
        /// such      as from Amazon Elastic MapReduce (EMR), or copy data from another database
        /// into DynamoDB. In      order to improve performance with these large-scale operations,
        /// <i>BatchWriteItem</i> does not      behave in the same way as individual <i>PutItem</i>
        /// and <i>DeleteItem</i> calls would For      example, you cannot specify conditions
        /// on individual put and delete requests, and        <i>BatchWriteItem</i> does not return
        /// deleted items in the response.
        /// </para>
        ///     
        /// <para>
        /// If you use a programming language that supports concurrency, such as Java, you can
        /// use      threads to write items in parallel. Your application must include the necessary
        /// logic to      manage the threads. With languages that don't support threading, such
        /// as PHP, you must update      or delete the specified items one at a time. In both
        /// situations, <i>BatchWriteItem</i>      provides an alternative where the API performs
        /// the specified put and delete operations in      parallel, giving you the power of
        /// the thread pool approach without having to introduce      complexity into your application.
        /// </para>
        ///     
        /// <para>
        /// Parallel processing reduces latency, but each specified put and delete request consumes
        /// the      same number of write capacity units whether it is processed in parallel or
        /// not. Delete      operations on nonexistent items consume one write capacity unit.
        /// </para>
        ///     
        /// <para>
        /// If one or more of the following is true, DynamoDB rejects the entire batch write operation:
        /// </para>
        ///     <ul>      <li>        
        /// <para>
        /// One or more tables specified in the <i>BatchWriteItem</i> request does not exist.
        /// </para>
        ///       </li>      <li>        
        /// <para>
        /// Primary key attributes specified on an item in the request do not match those in the
        ///          corresponding table's primary key schema.
        /// </para>
        ///       </li>      <li>        
        /// <para>
        /// You try to perform multiple operations on the same item in the same <i>BatchWriteItem</i>
        ///          request. For example, you cannot put and delete the same item in the same
        ///            <i>BatchWriteItem</i> request. 
        /// </para>
        ///       </li>      <li>        
        /// <para>
        /// The total request size exceeds 1 MB.
        /// </para>
        ///       </li>      <li>        
        /// <para>
        /// Any individual item in a batch exceeds 64 KB.
        /// </para>
        ///       </li>    </ul>
        /// </summary>
        /// <param name="requestItems">A map of one or more table names and, for each table, a list of operations to be performed  (<i>DeleteRequest</i> or <i>PutRequest</i>). Each element in the map consists of the   following: <ul>   <li>  <i>DeleteRequest</i> - Perform a <i>DeleteItem</i> operation on the specified item. The    item to be deleted is identified by a <i>Key</i> subelement:  <ul>    <li>   <i>Key</i> - A map of primary key attribute values that uniquely identify the ! item.     Each entry in this map consists of an attribute name and an attribute value. For each     primary key, you must provide <i>all</i> of the key attributes. For example, with a     hash type primary key, you only need to specify the hash attribute. For a     hash-and-range type primary key, you must specify <i>both</i> the hash attribute and     the range attribute.    </li>  </ul>   </li>   <li>  <i>PutRequest</i> - Perform a <i>PutItem</i> operation on the specified item. The item to    be put is identified by an <i>Item</i> subelement:  <ul>    <li>   <i>Item</i> - A map of attributes and their values. Each entry in this map consists     of an attribute name and an attribute value. Attribute values must not be null; string     and binary type attributes must have lengths greater than zero; and set type     attributes must not be empty. Requests that contain empty values will be rejected with     a <i>ValidationException</i>.   If you specify any attributes that are part of an index key, then the data types for     those attributes must match those of the schema in the table's attribute     definition.    </li>  </ul>   </li> </ul></param>
        /// 
        /// <returns>The response from the BatchWriteItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="ItemCollectionSizeLimitExceededException">
        /// An item collection is too large. This exception is only returned for tables that have
        /// one or      more local secondary indexes.
        /// </exception>
        /// <exception cref="ProvisionedThroughputExceededException">
        /// The request rate is too high, or the request is too large, for the available throughput
        /// to      accommodate. The AWS SDKs automatically retry requests that receive this exception;
        /// therefore,      your request will eventually succeed, unless the request is too large
        /// or your retry queue is      too large to finish. Reduce the frequency of requests
        /// by using the strategies listed in <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ErrorHandling.html#APIRetries">Error
        /// Retries and Exponential Backoff</a>      in the Amazon DynamoDB Developer Guide.
        /// </exception>
        /// <exception cref="ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource may not be
        /// specified      correctly, or its status may not be <code>ACTIVE</code>.
        /// </exception>
        public BatchWriteItemResponse BatchWriteItem(Dictionary<string, List<WriteRequest>> requestItems)
        {
            var request = new BatchWriteItemRequest();
            request.RequestItems = requestItems;
            return BatchWriteItem(request);
        }

        internal BatchWriteItemResponse BatchWriteItem(BatchWriteItemRequest request)
        {
            var marshaller = new BatchWriteItemRequestMarshaller();
            var unmarshaller = BatchWriteItemResponseUnmarshaller.Instance;

            return Invoke<BatchWriteItemRequest,BatchWriteItemResponse>(request, marshaller, unmarshaller);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the BatchWriteItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.IAmazonDynamoDB"/>
        /// </summary>
        /// 
        /// <param name="request">Container for the necessary parameters to execute the BatchWriteItem operation.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<BatchWriteItemResponse> BatchWriteItemAsync(BatchWriteItemRequest request, System.Threading.CancellationToken cancellationToken = default(CancellationToken))
        {
            var marshaller = new BatchWriteItemRequestMarshaller();
            var unmarshaller = BatchWriteItemResponseUnmarshaller.Instance;

            return InvokeAsync<BatchWriteItemRequest,BatchWriteItemResponse>(request, marshaller, 
                unmarshaller, cancellationToken);
        }

        #endregion
        
        #region  CreateTable

        /// <summary>
        /// The <i>CreateTable</i> operation adds a new table to your account. In an AWS account,
        /// table      names must be unique within each region. That is, you can have two tables
        /// with same name if      you create the tables in different regions.
        /// 
        ///     
        /// <para>
        /// <i>CreateTable</i> is an asynchronous operation. Upon receiving a <i>CreateTable</i>
        /// request,      DynamoDB immediately returns a response with a <i>TableStatus</i> of
        /// <code>CREATING</code>. After      the table is created, DynamoDB sets the <i>TableStatus</i>
        /// to <code>ACTIVE</code>. You can      perform read and write operations only on an
        /// <code>ACTIVE</code> table. 
        /// </para>
        ///     
        /// <para>
        /// If you want to create multiple tables with secondary indexes on them, you must create
        /// them sequentially.      Only one table with secondary indexes can be in the <code>CREATING</code>
        /// state at any given time.
        /// </para>
        ///     
        /// <para>
        /// You can use the <i>DescribeTable</i> API to check the table status.
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table to create.</param>
        /// <param name="keySchema">Specifies the attributes that make up the primary key for a table or an index. The attributes   in <i>KeySchema</i> must also be defined in the <i>AttributeDefinitions</i> array. For more   information, see <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DataModel.html">Data Model</a> in the   Amazon DynamoDB Developer Guide. Each <i>KeySchemaElement</i> in the array is composed of: <ul>   <li>  <i>AttributeName</i> - The name of this key attribute.   </li>   <li>  <i>KeyType</i> - Determines whether the key attribute is <code>HASH</code> or   <code>RANGE</code>.   </li> </ul> For a primary key that consists of a hash attribute, you must specify exactly one element   with a <i>KeyType</i> of <code>HASH</code>. For a primary key that consists of hash and range attributes, you must specify exactly two   elements, in this order: The first element must have a <i>KeyType</i> of <code>HASH</code>,   and the second element must have a <i>KeyType</i> of <code>RANGE</code>. For more information, see <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/WorkingWithTables.html#WorkingWithTables.primary.key">Specifying the  Primary Key</a> in the Amazon DynamoDB Developer Guide.</param>
        /// <param name="attributeDefinitions">An array of attributes that describe the key schema for the table and indexes.</param>
        /// <param name="provisionedThroughput">A property of CreateTableRequest used to execute the CreateTable service method.</param>
        /// 
        /// <returns>The response from the CreateTable service method, as returned by DynamoDB.</returns>
        /// <exception cref="InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="LimitExceededException">
        /// The number of concurrent table requests (cumulative number of tables in the      
        ///  <code>CREATING</code>, <code>DELETING</code> or <code>UPDATING</code> state) exceeds
        /// the      maximum allowed of 10.
        /// 
        ///     
        /// <para>
        /// Also, for tables with secondary indexes, only one of those tables can be in the <code>CREATING</code>
        ///      state at any point in time. Do not attempt to create more than one such table
        ///      simultaneously.
        /// </para>
        ///     
        /// <para>
        /// The total limit of tables in the <code>ACTIVE</code> state is 250.
        /// </para>
        /// </exception>
        /// <exception cref="ResourceInUseException">
        /// The operation conflicts with the resource's availability. For example, you attempted
        /// to      recreate an existing table, or tried to delete a table currently in the <code>CREATING</code>
        ///      state.
        /// </exception>
        public CreateTableResponse CreateTable(string tableName, List<KeySchemaElement> keySchema, List<AttributeDefinition> attributeDefinitions, ProvisionedThroughput provisionedThroughput)
        {
            var request = new CreateTableRequest();
            request.TableName = tableName;
            request.KeySchema = keySchema;
            request.AttributeDefinitions = attributeDefinitions;
            request.ProvisionedThroughput = provisionedThroughput;
            return CreateTable(request);
        }

        internal CreateTableResponse CreateTable(CreateTableRequest request)
        {
            var marshaller = new CreateTableRequestMarshaller();
            var unmarshaller = CreateTableResponseUnmarshaller.Instance;

            return Invoke<CreateTableRequest,CreateTableResponse>(request, marshaller, unmarshaller);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the CreateTable operation.
        /// <seealso cref="Amazon.DynamoDBv2.IAmazonDynamoDB"/>
        /// </summary>
        /// 
        /// <param name="request">Container for the necessary parameters to execute the CreateTable operation.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<CreateTableResponse> CreateTableAsync(CreateTableRequest request, System.Threading.CancellationToken cancellationToken = default(CancellationToken))
        {
            var marshaller = new CreateTableRequestMarshaller();
            var unmarshaller = CreateTableResponseUnmarshaller.Instance;

            return InvokeAsync<CreateTableRequest,CreateTableResponse>(request, marshaller, 
                unmarshaller, cancellationToken);
        }

        #endregion
        
        #region  DeleteItem

        /// <summary>
        /// Deletes a single item in a table by primary key. You can perform a conditional delete
        ///      operation that deletes the item if it exists, or if it has an expected attribute
        /// value.
        /// 
        ///     
        /// <para>
        /// In addition to deleting an item, you can also return the item's attribute values in
        /// the same      operation, using the <i>ReturnValues</i> parameter.
        /// </para>
        ///     
        /// <para>
        /// Unless you specify conditions, the <i>DeleteItem</i> is an idempotent operation; running
        /// it      multiple times on the same item or attribute does <i>not</i> result in an
        /// error response.
        /// </para>
        ///     
        /// <para>
        /// Conditional deletes are useful for only deleting items if specific conditions are
        /// met. If      those conditions are met, DynamoDB performs the delete. Otherwise, the
        /// item is not deleted. 
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table from which to delete the item.</param>
        /// <param name="key">A map of attribute names to <i>AttributeValue</i> objects, representing the primary key of   the item to delete. For the primary key, you must provide <i>all</i> of the attributes. For example, with a hash   type primary key, you only need to specify the hash attribute. For a hash-and-range type   primary key, you must specify <i>both</i> the hash attribute and the range attribute.</param>
        /// 
        /// <returns>The response from the DeleteItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="ConditionalCheckFailedException">
        /// A condition specified in the operation could not be evaluated.
        /// </exception>
        /// <exception cref="InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="ItemCollectionSizeLimitExceededException">
        /// An item collection is too large. This exception is only returned for tables that have
        /// one or      more local secondary indexes.
        /// </exception>
        /// <exception cref="ProvisionedThroughputExceededException">
        /// The request rate is too high, or the request is too large, for the available throughput
        /// to      accommodate. The AWS SDKs automatically retry requests that receive this exception;
        /// therefore,      your request will eventually succeed, unless the request is too large
        /// or your retry queue is      too large to finish. Reduce the frequency of requests
        /// by using the strategies listed in <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ErrorHandling.html#APIRetries">Error
        /// Retries and Exponential Backoff</a>      in the Amazon DynamoDB Developer Guide.
        /// </exception>
        /// <exception cref="ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource may not be
        /// specified      correctly, or its status may not be <code>ACTIVE</code>.
        /// </exception>
        public DeleteItemResponse DeleteItem(string tableName, Dictionary<string, AttributeValue> key)
        {
            var request = new DeleteItemRequest();
            request.TableName = tableName;
            request.Key = key;
            return DeleteItem(request);
        }

        /// <summary>
        /// Deletes a single item in a table by primary key. You can perform a conditional delete
        ///      operation that deletes the item if it exists, or if it has an expected attribute
        /// value.
        /// 
        ///     
        /// <para>
        /// In addition to deleting an item, you can also return the item's attribute values in
        /// the same      operation, using the <i>ReturnValues</i> parameter.
        /// </para>
        ///     
        /// <para>
        /// Unless you specify conditions, the <i>DeleteItem</i> is an idempotent operation; running
        /// it      multiple times on the same item or attribute does <i>not</i> result in an
        /// error response.
        /// </para>
        ///     
        /// <para>
        /// Conditional deletes are useful for only deleting items if specific conditions are
        /// met. If      those conditions are met, DynamoDB performs the delete. Otherwise, the
        /// item is not deleted. 
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table from which to delete the item.</param>
        /// <param name="key">A map of attribute names to <i>AttributeValue</i> objects, representing the primary key of   the item to delete. For the primary key, you must provide <i>all</i> of the attributes. For example, with a hash   type primary key, you only need to specify the hash attribute. For a hash-and-range type   primary key, you must specify <i>both</i> the hash attribute and the range attribute.</param>
        /// <param name="returnValues">Use <i>ReturnValues</i> if you want to get the item attributes as they appeared before they   were deleted. For <i>DeleteItem</i>, the valid values are: <ul>   <li>  <code>NONE</code> - If <i>ReturnValues</i> is not specified, or if its value is   <code>NONE</code>, then nothing is returned. (This is the default for   <i>ReturnValues</i>.)   </li>   <li>  <code>ALL_OLD</code> - The content of the old item is returned.   </li> </ul></param>
        /// 
        /// <returns>The response from the DeleteItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="ConditionalCheckFailedException">
        /// A condition specified in the operation could not be evaluated.
        /// </exception>
        /// <exception cref="InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="ItemCollectionSizeLimitExceededException">
        /// An item collection is too large. This exception is only returned for tables that have
        /// one or      more local secondary indexes.
        /// </exception>
        /// <exception cref="ProvisionedThroughputExceededException">
        /// The request rate is too high, or the request is too large, for the available throughput
        /// to      accommodate. The AWS SDKs automatically retry requests that receive this exception;
        /// therefore,      your request will eventually succeed, unless the request is too large
        /// or your retry queue is      too large to finish. Reduce the frequency of requests
        /// by using the strategies listed in <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ErrorHandling.html#APIRetries">Error
        /// Retries and Exponential Backoff</a>      in the Amazon DynamoDB Developer Guide.
        /// </exception>
        /// <exception cref="ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource may not be
        /// specified      correctly, or its status may not be <code>ACTIVE</code>.
        /// </exception>
        public DeleteItemResponse DeleteItem(string tableName, Dictionary<string, AttributeValue> key, ReturnValue returnValues)
        {
            var request = new DeleteItemRequest();
            request.TableName = tableName;
            request.Key = key;
            request.ReturnValues = returnValues;
            return DeleteItem(request);
        }

        internal DeleteItemResponse DeleteItem(DeleteItemRequest request)
        {
            var marshaller = new DeleteItemRequestMarshaller();
            var unmarshaller = DeleteItemResponseUnmarshaller.Instance;

            return Invoke<DeleteItemRequest,DeleteItemResponse>(request, marshaller, unmarshaller);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the DeleteItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.IAmazonDynamoDB"/>
        /// </summary>
        /// 
        /// <param name="request">Container for the necessary parameters to execute the DeleteItem operation.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<DeleteItemResponse> DeleteItemAsync(DeleteItemRequest request, System.Threading.CancellationToken cancellationToken = default(CancellationToken))
        {
            var marshaller = new DeleteItemRequestMarshaller();
            var unmarshaller = DeleteItemResponseUnmarshaller.Instance;

            return InvokeAsync<DeleteItemRequest,DeleteItemResponse>(request, marshaller, 
                unmarshaller, cancellationToken);
        }

        #endregion
        
        #region  DeleteTable

        /// <summary>
        /// The <i>DeleteTable</i> operation deletes a table and all of its items. After a   
        ///     <i>DeleteTable</i> request, the specified table is in the <code>DELETING</code>
        /// state until      DynamoDB completes the deletion. If the table is in the <code>ACTIVE</code>
        /// state, you can delete      it. If a table is in <code>CREATING</code> or <code>UPDATING</code>
        /// states, then DynamoDB returns      a <i>ResourceInUseException</i>. If the specified
        /// table does not exist, DynamoDB returns a        <i>ResourceNotFoundException</i>.
        /// If table is already in the <code>DELETING</code> state, no      error is returned.
        /// 
        /// 
        ///     <note>      
        /// <para>
        /// DynamoDB might continue to accept data read and write operations, such as <i>GetItem</i>
        /// and          <i>PutItem</i>, on a table in the <code>DELETING</code> state until the
        /// table deletion is        complete.
        /// </para>
        ///     </note>    
        /// <para>
        /// When you delete a table, any indexes on that table are also deleted.
        /// </para>
        ///     
        /// <para>
        /// Use the <i>DescribeTable</i> API to check the status of the table. 
        /// </para>
        /// </summary>
        /// <param name="tableName"> The name of the table to delete.</param>
        /// 
        /// <returns>The response from the DeleteTable service method, as returned by DynamoDB.</returns>
        /// <exception cref="InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="LimitExceededException">
        /// The number of concurrent table requests (cumulative number of tables in the      
        ///  <code>CREATING</code>, <code>DELETING</code> or <code>UPDATING</code> state) exceeds
        /// the      maximum allowed of 10.
        /// 
        ///     
        /// <para>
        /// Also, for tables with secondary indexes, only one of those tables can be in the <code>CREATING</code>
        ///      state at any point in time. Do not attempt to create more than one such table
        ///      simultaneously.
        /// </para>
        ///     
        /// <para>
        /// The total limit of tables in the <code>ACTIVE</code> state is 250.
        /// </para>
        /// </exception>
        /// <exception cref="ResourceInUseException">
        /// The operation conflicts with the resource's availability. For example, you attempted
        /// to      recreate an existing table, or tried to delete a table currently in the <code>CREATING</code>
        ///      state.
        /// </exception>
        /// <exception cref="ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource may not be
        /// specified      correctly, or its status may not be <code>ACTIVE</code>.
        /// </exception>
        public DeleteTableResponse DeleteTable(string tableName)
        {
            var request = new DeleteTableRequest();
            request.TableName = tableName;
            return DeleteTable(request);
        }

        internal DeleteTableResponse DeleteTable(DeleteTableRequest request)
        {
            var marshaller = new DeleteTableRequestMarshaller();
            var unmarshaller = DeleteTableResponseUnmarshaller.Instance;

            return Invoke<DeleteTableRequest,DeleteTableResponse>(request, marshaller, unmarshaller);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the DeleteTable operation.
        /// <seealso cref="Amazon.DynamoDBv2.IAmazonDynamoDB"/>
        /// </summary>
        /// 
        /// <param name="request">Container for the necessary parameters to execute the DeleteTable operation.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<DeleteTableResponse> DeleteTableAsync(DeleteTableRequest request, System.Threading.CancellationToken cancellationToken = default(CancellationToken))
        {
            var marshaller = new DeleteTableRequestMarshaller();
            var unmarshaller = DeleteTableResponseUnmarshaller.Instance;

            return InvokeAsync<DeleteTableRequest,DeleteTableResponse>(request, marshaller, 
                unmarshaller, cancellationToken);
        }

        #endregion
        
        #region  DescribeTable

        /// <summary>
        /// Returns information about the table, including the current status of the table, when
        /// it was      created, the primary key schema, and any indexes on the table.
        /// </summary>
        /// <param name="tableName"> The name of the table to describe.</param>
        /// 
        /// <returns>The response from the DescribeTable service method, as returned by DynamoDB.</returns>
        /// <exception cref="InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource may not be
        /// specified      correctly, or its status may not be <code>ACTIVE</code>.
        /// </exception>
        public DescribeTableResponse DescribeTable(string tableName)
        {
            var request = new DescribeTableRequest();
            request.TableName = tableName;
            return DescribeTable(request);
        }

        internal DescribeTableResponse DescribeTable(DescribeTableRequest request)
        {
            var marshaller = new DescribeTableRequestMarshaller();
            var unmarshaller = DescribeTableResponseUnmarshaller.Instance;

            return Invoke<DescribeTableRequest,DescribeTableResponse>(request, marshaller, unmarshaller);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the DescribeTable operation.
        /// <seealso cref="Amazon.DynamoDBv2.IAmazonDynamoDB"/>
        /// </summary>
        /// 
        /// <param name="request">Container for the necessary parameters to execute the DescribeTable operation.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<DescribeTableResponse> DescribeTableAsync(DescribeTableRequest request, System.Threading.CancellationToken cancellationToken = default(CancellationToken))
        {
            var marshaller = new DescribeTableRequestMarshaller();
            var unmarshaller = DescribeTableResponseUnmarshaller.Instance;

            return InvokeAsync<DescribeTableRequest,DescribeTableResponse>(request, marshaller, 
                unmarshaller, cancellationToken);
        }

        #endregion
        
        #region  GetItem

        /// <summary>
        /// The <i>GetItem</i> operation returns a set of attributes for the item with the given
        /// primary      key. If there is no matching item, <i>GetItem</i> does not return any
        /// data.
        /// 
        ///     
        /// <para>
        /// <i>GetItem</i> provides an eventually consistent read by default. If your application
        ///      requires a strongly consistent read, set <i>ConsistentRead</i> to <code>true</code>.
        /// Although      a strongly consistent read might take more time than an eventually consistent
        /// read, it always      returns the last updated value.
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table containing the requested item.</param>
        /// <param name="key">A map of attribute names to <i>AttributeValue</i> objects, representing the primary key of   the item to retrieve. For the primary key, you must provide <i>all</i> of the attributes. For example, with a hash   type primary key, you only need to specify the hash attribute. For a hash-and-range type   primary key, you must specify <i>both</i> the hash attribute and the range attribute.</param>
        /// 
        /// <returns>The response from the GetItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="ProvisionedThroughputExceededException">
        /// The request rate is too high, or the request is too large, for the available throughput
        /// to      accommodate. The AWS SDKs automatically retry requests that receive this exception;
        /// therefore,      your request will eventually succeed, unless the request is too large
        /// or your retry queue is      too large to finish. Reduce the frequency of requests
        /// by using the strategies listed in <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ErrorHandling.html#APIRetries">Error
        /// Retries and Exponential Backoff</a>      in the Amazon DynamoDB Developer Guide.
        /// </exception>
        /// <exception cref="ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource may not be
        /// specified      correctly, or its status may not be <code>ACTIVE</code>.
        /// </exception>
        public GetItemResponse GetItem(string tableName, Dictionary<string, AttributeValue> key)
        {
            var request = new GetItemRequest();
            request.TableName = tableName;
            request.Key = key;
            return GetItem(request);
        }

        /// <summary>
        /// The <i>GetItem</i> operation returns a set of attributes for the item with the given
        /// primary      key. If there is no matching item, <i>GetItem</i> does not return any
        /// data.
        /// 
        ///     
        /// <para>
        /// <i>GetItem</i> provides an eventually consistent read by default. If your application
        ///      requires a strongly consistent read, set <i>ConsistentRead</i> to <code>true</code>.
        /// Although      a strongly consistent read might take more time than an eventually consistent
        /// read, it always      returns the last updated value.
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table containing the requested item.</param>
        /// <param name="key">A map of attribute names to <i>AttributeValue</i> objects, representing the primary key of   the item to retrieve. For the primary key, you must provide <i>all</i> of the attributes. For example, with a hash   type primary key, you only need to specify the hash attribute. For a hash-and-range type   primary key, you must specify <i>both</i> the hash attribute and the range attribute.</param>
        /// <param name="consistentRead">If set to <code>true</code>, then the operation uses strongly consistent reads; otherwise, eventually   consistent reads are used.</param>
        /// 
        /// <returns>The response from the GetItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="ProvisionedThroughputExceededException">
        /// The request rate is too high, or the request is too large, for the available throughput
        /// to      accommodate. The AWS SDKs automatically retry requests that receive this exception;
        /// therefore,      your request will eventually succeed, unless the request is too large
        /// or your retry queue is      too large to finish. Reduce the frequency of requests
        /// by using the strategies listed in <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ErrorHandling.html#APIRetries">Error
        /// Retries and Exponential Backoff</a>      in the Amazon DynamoDB Developer Guide.
        /// </exception>
        /// <exception cref="ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource may not be
        /// specified      correctly, or its status may not be <code>ACTIVE</code>.
        /// </exception>
        public GetItemResponse GetItem(string tableName, Dictionary<string, AttributeValue> key, bool consistentRead)
        {
            var request = new GetItemRequest();
            request.TableName = tableName;
            request.Key = key;
            request.ConsistentRead = consistentRead;
            return GetItem(request);
        }

        internal GetItemResponse GetItem(GetItemRequest request)
        {
            var marshaller = new GetItemRequestMarshaller();
            var unmarshaller = GetItemResponseUnmarshaller.Instance;

            return Invoke<GetItemRequest,GetItemResponse>(request, marshaller, unmarshaller);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the GetItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.IAmazonDynamoDB"/>
        /// </summary>
        /// 
        /// <param name="request">Container for the necessary parameters to execute the GetItem operation.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<GetItemResponse> GetItemAsync(GetItemRequest request, System.Threading.CancellationToken cancellationToken = default(CancellationToken))
        {
            var marshaller = new GetItemRequestMarshaller();
            var unmarshaller = GetItemResponseUnmarshaller.Instance;

            return InvokeAsync<GetItemRequest,GetItemResponse>(request, marshaller, 
                unmarshaller, cancellationToken);
        }

        #endregion
        
        #region  ListTables

        internal ListTablesResponse ListTables()
        {
            return ListTables(new ListTablesRequest());
        }
        /// <summary>
        /// Returns an array of table names associated with the current account and endpoint.
        /// The output      from <i>ListTables</i> is paginated, with each page returning a maximum
        /// of 100 table      names.
        /// </summary>
        /// <param name="exclusiveStartTableName">The first table name that this operation will evaluate. Use the value that was returned for  <i>LastEvaluatedTableName</i> in a previous operation, so that you can obtain the next page   of results.</param>
        /// 
        /// <returns>The response from the ListTables service method, as returned by DynamoDB.</returns>
        /// <exception cref="InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        public ListTablesResponse ListTables(string exclusiveStartTableName)
        {
            var request = new ListTablesRequest();
            request.ExclusiveStartTableName = exclusiveStartTableName;
            return ListTables(request);
        }

        /// <summary>
        /// Returns an array of table names associated with the current account and endpoint.
        /// The output      from <i>ListTables</i> is paginated, with each page returning a maximum
        /// of 100 table      names.
        /// </summary>
        /// <param name="exclusiveStartTableName">The first table name that this operation will evaluate. Use the value that was returned for  <i>LastEvaluatedTableName</i> in a previous operation, so that you can obtain the next page   of results.</param>
        /// <param name="limit"> A maximum number of table names to return. If this parameter is not specified, the limit is   100.</param>
        /// 
        /// <returns>The response from the ListTables service method, as returned by DynamoDB.</returns>
        /// <exception cref="InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        public ListTablesResponse ListTables(string exclusiveStartTableName, int limit)
        {
            var request = new ListTablesRequest();
            request.ExclusiveStartTableName = exclusiveStartTableName;
            request.Limit = limit;
            return ListTables(request);
        }

        /// <summary>
        /// Returns an array of table names associated with the current account and endpoint.
        /// The output      from <i>ListTables</i> is paginated, with each page returning a maximum
        /// of 100 table      names.
        /// </summary>
        /// <param name="limit"> A maximum number of table names to return. If this parameter is not specified, the limit is   100.</param>
        /// 
        /// <returns>The response from the ListTables service method, as returned by DynamoDB.</returns>
        /// <exception cref="InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        public ListTablesResponse ListTables(int limit)
        {
            var request = new ListTablesRequest();
            request.Limit = limit;
            return ListTables(request);
        }

        internal ListTablesResponse ListTables(ListTablesRequest request)
        {
            var marshaller = new ListTablesRequestMarshaller();
            var unmarshaller = ListTablesResponseUnmarshaller.Instance;

            return Invoke<ListTablesRequest,ListTablesResponse>(request, marshaller, unmarshaller);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the ListTables operation.
        /// <seealso cref="Amazon.DynamoDBv2.IAmazonDynamoDB"/>
        /// </summary>
        /// 
        /// <param name="request">Container for the necessary parameters to execute the ListTables operation.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<ListTablesResponse> ListTablesAsync(ListTablesRequest request, System.Threading.CancellationToken cancellationToken = default(CancellationToken))
        {
            var marshaller = new ListTablesRequestMarshaller();
            var unmarshaller = ListTablesResponseUnmarshaller.Instance;

            return InvokeAsync<ListTablesRequest,ListTablesResponse>(request, marshaller, 
                unmarshaller, cancellationToken);
        }

        #endregion
        
        #region  PutItem

        /// <summary>
        /// Creates a new item, or replaces an old item with a new item. If an item already exists
        /// in the      specified table with the same primary key, the new item completely replaces
        /// the existing item.      You can perform a conditional put (insert a new item if one
        /// with the specified primary key      doesn't exist), or replace an existing item if
        /// it has certain attribute values. 
        /// 
        ///     
        /// <para>
        /// In addition to putting an item, you can also return the item's attribute values in
        /// the same      operation, using the <i>ReturnValues</i> parameter.
        /// </para>
        ///     
        /// <para>
        /// When you add an item, the primary key attribute(s) are the only required attributes.
        ///      Attribute values cannot be null. String and binary type attributes must have
        /// lengths greater      than zero. Set type attributes cannot be empty. Requests with
        /// empty values will be rejected      with a <i>ValidationException</i>.
        /// </para>
        ///     
        /// <para>
        /// You can request that <i>PutItem</i> return either a copy of the old item (before the
        /// update)      or a copy of the new item (after the update). For more information, see
        /// the        <i>ReturnValues</i> description.
        /// </para>
        ///     <note>      
        /// <para>
        /// To prevent a new item from replacing an existing item, use a conditional put operation
        /// with          <i>ComparisonOperator</i> set to <code>NULL</code> for the primary key
        /// attribute, or attributes.
        /// </para>
        ///     </note>    
        /// <para>
        /// For more information about using this API, see <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/WorkingWithItems.html">Working
        /// with Items</a> in the Amazon DynamoDB Developer Guide.
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table to contain the item.</param>
        /// <param name="item">A map of attribute name/value pairs, one for each attribute. Only the primary key attributes   are required; you can optionally provide other attribute name-value pairs for the item. You must provide <i>all</i> of the attributes for the primary key. For example, with a hash   type primary key, you only need to specify the hash attribute. For a hash-and-range type   primary key, you must specify <i>both</i> the hash attribute and the range attribute. If you specify any attributes that are part of an index key, then the data types for those   attributes must match those of the schema in the table's attribute definition. For more information about primary keys, see <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DataModel.html#DataModelPrimaryKey">Primary Key</a> in the   Amazon DynamoDB Developer Guide. Each element in the <i>Item</i> map is an <i>AttributeValue</i> object.</param>
        /// 
        /// <returns>The response from the PutItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="ConditionalCheckFailedException">
        /// A condition specified in the operation could not be evaluated.
        /// </exception>
        /// <exception cref="InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="ItemCollectionSizeLimitExceededException">
        /// An item collection is too large. This exception is only returned for tables that have
        /// one or      more local secondary indexes.
        /// </exception>
        /// <exception cref="ProvisionedThroughputExceededException">
        /// The request rate is too high, or the request is too large, for the available throughput
        /// to      accommodate. The AWS SDKs automatically retry requests that receive this exception;
        /// therefore,      your request will eventually succeed, unless the request is too large
        /// or your retry queue is      too large to finish. Reduce the frequency of requests
        /// by using the strategies listed in <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ErrorHandling.html#APIRetries">Error
        /// Retries and Exponential Backoff</a>      in the Amazon DynamoDB Developer Guide.
        /// </exception>
        /// <exception cref="ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource may not be
        /// specified      correctly, or its status may not be <code>ACTIVE</code>.
        /// </exception>
        public PutItemResponse PutItem(string tableName, Dictionary<string, AttributeValue> item)
        {
            var request = new PutItemRequest();
            request.TableName = tableName;
            request.Item = item;
            return PutItem(request);
        }

        /// <summary>
        /// Creates a new item, or replaces an old item with a new item. If an item already exists
        /// in the      specified table with the same primary key, the new item completely replaces
        /// the existing item.      You can perform a conditional put (insert a new item if one
        /// with the specified primary key      doesn't exist), or replace an existing item if
        /// it has certain attribute values. 
        /// 
        ///     
        /// <para>
        /// In addition to putting an item, you can also return the item's attribute values in
        /// the same      operation, using the <i>ReturnValues</i> parameter.
        /// </para>
        ///     
        /// <para>
        /// When you add an item, the primary key attribute(s) are the only required attributes.
        ///      Attribute values cannot be null. String and binary type attributes must have
        /// lengths greater      than zero. Set type attributes cannot be empty. Requests with
        /// empty values will be rejected      with a <i>ValidationException</i>.
        /// </para>
        ///     
        /// <para>
        /// You can request that <i>PutItem</i> return either a copy of the old item (before the
        /// update)      or a copy of the new item (after the update). For more information, see
        /// the        <i>ReturnValues</i> description.
        /// </para>
        ///     <note>      
        /// <para>
        /// To prevent a new item from replacing an existing item, use a conditional put operation
        /// with          <i>ComparisonOperator</i> set to <code>NULL</code> for the primary key
        /// attribute, or attributes.
        /// </para>
        ///     </note>    
        /// <para>
        /// For more information about using this API, see <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/WorkingWithItems.html">Working
        /// with Items</a> in the Amazon DynamoDB Developer Guide.
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table to contain the item.</param>
        /// <param name="item">A map of attribute name/value pairs, one for each attribute. Only the primary key attributes   are required; you can optionally provide other attribute name-value pairs for the item. You must provide <i>all</i> of the attributes for the primary key. For example, with a hash   type primary key, you only need to specify the hash attribute. For a hash-and-range type   primary key, you must specify <i>both</i> the hash attribute and the range attribute. If you specify any attributes that are part of an index key, then the data types for those   attributes must match those of the schema in the table's attribute definition. For more information about primary keys, see <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DataModel.html#DataModelPrimaryKey">Primary Key</a> in the   Amazon DynamoDB Developer Guide. Each element in the <i>Item</i> map is an <i>AttributeValue</i> object.</param>
        /// <param name="returnValues">Use <i>ReturnValues</i> if you want to get the item attributes as they appeared before they   were updated with the <i>PutItem</i> request. For <i>PutItem</i>, the valid values are: <ul>   <li>  <code>NONE</code> - If <i>ReturnValues</i> is not specified, or if its value is   <code>NONE</code>, then nothing is returned. (This is the default for   <i>ReturnValues</i>.)   </li>   <li>  <code>ALL_OLD</code> - If <i>PutItem</i> overwrote an attribute name-value pair, then the    content of the old item is returned.   </li> </ul></param>
        /// 
        /// <returns>The response from the PutItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="ConditionalCheckFailedException">
        /// A condition specified in the operation could not be evaluated.
        /// </exception>
        /// <exception cref="InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="ItemCollectionSizeLimitExceededException">
        /// An item collection is too large. This exception is only returned for tables that have
        /// one or      more local secondary indexes.
        /// </exception>
        /// <exception cref="ProvisionedThroughputExceededException">
        /// The request rate is too high, or the request is too large, for the available throughput
        /// to      accommodate. The AWS SDKs automatically retry requests that receive this exception;
        /// therefore,      your request will eventually succeed, unless the request is too large
        /// or your retry queue is      too large to finish. Reduce the frequency of requests
        /// by using the strategies listed in <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ErrorHandling.html#APIRetries">Error
        /// Retries and Exponential Backoff</a>      in the Amazon DynamoDB Developer Guide.
        /// </exception>
        /// <exception cref="ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource may not be
        /// specified      correctly, or its status may not be <code>ACTIVE</code>.
        /// </exception>
        public PutItemResponse PutItem(string tableName, Dictionary<string, AttributeValue> item, ReturnValue returnValues)
        {
            var request = new PutItemRequest();
            request.TableName = tableName;
            request.Item = item;
            request.ReturnValues = returnValues;
            return PutItem(request);
        }

        internal PutItemResponse PutItem(PutItemRequest request)
        {
            var marshaller = new PutItemRequestMarshaller();
            var unmarshaller = PutItemResponseUnmarshaller.Instance;

            return Invoke<PutItemRequest,PutItemResponse>(request, marshaller, unmarshaller);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the PutItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.IAmazonDynamoDB"/>
        /// </summary>
        /// 
        /// <param name="request">Container for the necessary parameters to execute the PutItem operation.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<PutItemResponse> PutItemAsync(PutItemRequest request, System.Threading.CancellationToken cancellationToken = default(CancellationToken))
        {
            var marshaller = new PutItemRequestMarshaller();
            var unmarshaller = PutItemResponseUnmarshaller.Instance;

            return InvokeAsync<PutItemRequest,PutItemResponse>(request, marshaller, 
                unmarshaller, cancellationToken);
        }

        #endregion
        
        #region  Query

        internal QueryResponse Query(QueryRequest request)
        {
            var marshaller = new QueryRequestMarshaller();
            var unmarshaller = QueryResponseUnmarshaller.Instance;

            return Invoke<QueryRequest,QueryResponse>(request, marshaller, unmarshaller);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the Query operation.
        /// <seealso cref="Amazon.DynamoDBv2.IAmazonDynamoDB"/>
        /// </summary>
        /// 
        /// <param name="request">Container for the necessary parameters to execute the Query operation.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<QueryResponse> QueryAsync(QueryRequest request, System.Threading.CancellationToken cancellationToken = default(CancellationToken))
        {
            var marshaller = new QueryRequestMarshaller();
            var unmarshaller = QueryResponseUnmarshaller.Instance;

            return InvokeAsync<QueryRequest,QueryResponse>(request, marshaller, 
                unmarshaller, cancellationToken);
        }

        #endregion
        
        #region  Scan

        /// <summary>
        /// The <i>Scan</i> operation returns one or more items and item attributes by accessing
        /// every      item in the table. To have DynamoDB return fewer items, you can provide
        /// a <i>ScanFilter</i>.
        /// 
        ///     
        /// <para>
        /// If the total number of scanned items exceeds the maximum data set size limit of  
        ///    1 MB, the scan stops and results are returned to the user with a        <i>LastEvaluatedKey</i>
        /// to continue the scan in a subsequent operation. The results also      include the
        /// number of items exceeding the limit. A scan can result in no table data meeting  
        ///    the filter criteria. 
        /// </para>
        ///     
        /// <para>
        /// The result set is eventually consistent. 
        /// </para>
        ///     
        /// <para>
        /// By default, <i>Scan</i> operations proceed sequentially; however, for faster performance
        /// on      large tables, applications can request a parallel <i>Scan</i> by specifying
        /// the <i>Segment</i>      and <i>TotalSegments</i> parameters. For more information,
        /// see <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/QueryAndScan.html#QueryAndScanParallelScan">Parallel
        /// Scan</a> in the      Amazon DynamoDB Developer Guide.
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table containing the requested items.</param>
        /// <param name="attributesToGet">The names of one or more attributes to retrieve.  If no attribute   names are specified, then all attributes will be returned. If   any of the requested attributes are not found, they will not   appear in the result.   Note that <i>AttributesToGet</i> has no effect on provisioned throughput consumption.    DynamoDB determines capacity units consumed based on item size, not on the amount of data that is returned to an application.</param>
        /// 
        /// <returns>The response from the Scan service method, as returned by DynamoDB.</returns>
        /// <exception cref="InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="ProvisionedThroughputExceededException">
        /// The request rate is too high, or the request is too large, for the available throughput
        /// to      accommodate. The AWS SDKs automatically retry requests that receive this exception;
        /// therefore,      your request will eventually succeed, unless the request is too large
        /// or your retry queue is      too large to finish. Reduce the frequency of requests
        /// by using the strategies listed in <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ErrorHandling.html#APIRetries">Error
        /// Retries and Exponential Backoff</a>      in the Amazon DynamoDB Developer Guide.
        /// </exception>
        /// <exception cref="ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource may not be
        /// specified      correctly, or its status may not be <code>ACTIVE</code>.
        /// </exception>
        public ScanResponse Scan(string tableName, List<string> attributesToGet)
        {
            var request = new ScanRequest();
            request.TableName = tableName;
            request.AttributesToGet = attributesToGet;
            return Scan(request);
        }

        /// <summary>
        /// The <i>Scan</i> operation returns one or more items and item attributes by accessing
        /// every      item in the table. To have DynamoDB return fewer items, you can provide
        /// a <i>ScanFilter</i>.
        /// 
        ///     
        /// <para>
        /// If the total number of scanned items exceeds the maximum data set size limit of  
        ///    1 MB, the scan stops and results are returned to the user with a        <i>LastEvaluatedKey</i>
        /// to continue the scan in a subsequent operation. The results also      include the
        /// number of items exceeding the limit. A scan can result in no table data meeting  
        ///    the filter criteria. 
        /// </para>
        ///     
        /// <para>
        /// The result set is eventually consistent. 
        /// </para>
        ///     
        /// <para>
        /// By default, <i>Scan</i> operations proceed sequentially; however, for faster performance
        /// on      large tables, applications can request a parallel <i>Scan</i> by specifying
        /// the <i>Segment</i>      and <i>TotalSegments</i> parameters. For more information,
        /// see <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/QueryAndScan.html#QueryAndScanParallelScan">Parallel
        /// Scan</a> in the      Amazon DynamoDB Developer Guide.
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table containing the requested items.</param>
        /// <param name="scanFilter">Evaluates the scan results and returns only the desired values. If you specify more than one condition in the <i>ScanFilter</i> map, then by default all of   the conditions must evaluate to true. In other words, the conditions are ANDed together. (You   can use the <i>ConditionalOperator</i> parameter to OR the conditions instead. If you do this,   then at least one of the conditions must evaluate to true, rather than all of them.) Each <i>ScanFilter</i> element consists of an attribute name to compare, along with the   following: <ul>   <li>  <i>AttributeValueList</i> - One or more values to evaluate against the supplied    attribute. The number of values in the list depends on the <i>ComparisonOperator</i> being    used.  For type Number, value comparisons are numeric.  String value comparisons for greater than, equals, or less than are based on ASCII    character code values. For example, <code>a</code> is greater than <code>A</code>, and   <code>aa</code> is greater than <code>B</code>. For a list of code values, see <a href="http://en.wikipedia.org/wiki/ASCII#ASCII_printable_characters">http://en.wikipedia.org/wiki/ASCII#ASCII_printable_characters</a>.  For Binary, DynamoDB treats each byte of the binary data as unsigned when it compares binary    values, for example when evaluating query expressions.  For information on specifying data types in JSON, see <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DataFormat.html">JSON Data Format</a> in the Amazon DynamoDB Developer Guide.   </li>   <li><i>ComparisonOperator</i> - A comparator for evaluating attributes. For example,    equals, greater than, less than, etc. The following comparison operators are available:   <code>EQ | NE | LE | LT | GE | GT | NOT_NULL | NULL | CONTAINS | NOT_CONTAINS | BEGINS_WITH | IN | BETWEEN</code> For complete    descriptions of all comparison operators, see <a href="http://docs.aws.amazon.com/amazondynamodb/latest/APIReference/API_Condition.html">Condition</a>.   </li> </ul></param>
        /// 
        /// <returns>The response from the Scan service method, as returned by DynamoDB.</returns>
        /// <exception cref="InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="ProvisionedThroughputExceededException">
        /// The request rate is too high, or the request is too large, for the available throughput
        /// to      accommodate. The AWS SDKs automatically retry requests that receive this exception;
        /// therefore,      your request will eventually succeed, unless the request is too large
        /// or your retry queue is      too large to finish. Reduce the frequency of requests
        /// by using the strategies listed in <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ErrorHandling.html#APIRetries">Error
        /// Retries and Exponential Backoff</a>      in the Amazon DynamoDB Developer Guide.
        /// </exception>
        /// <exception cref="ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource may not be
        /// specified      correctly, or its status may not be <code>ACTIVE</code>.
        /// </exception>
        public ScanResponse Scan(string tableName, Dictionary<string, Condition> scanFilter)
        {
            var request = new ScanRequest();
            request.TableName = tableName;
            request.ScanFilter = scanFilter;
            return Scan(request);
        }

        /// <summary>
        /// The <i>Scan</i> operation returns one or more items and item attributes by accessing
        /// every      item in the table. To have DynamoDB return fewer items, you can provide
        /// a <i>ScanFilter</i>.
        /// 
        ///     
        /// <para>
        /// If the total number of scanned items exceeds the maximum data set size limit of  
        ///    1 MB, the scan stops and results are returned to the user with a        <i>LastEvaluatedKey</i>
        /// to continue the scan in a subsequent operation. The results also      include the
        /// number of items exceeding the limit. A scan can result in no table data meeting  
        ///    the filter criteria. 
        /// </para>
        ///     
        /// <para>
        /// The result set is eventually consistent. 
        /// </para>
        ///     
        /// <para>
        /// By default, <i>Scan</i> operations proceed sequentially; however, for faster performance
        /// on      large tables, applications can request a parallel <i>Scan</i> by specifying
        /// the <i>Segment</i>      and <i>TotalSegments</i> parameters. For more information,
        /// see <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/QueryAndScan.html#QueryAndScanParallelScan">Parallel
        /// Scan</a> in the      Amazon DynamoDB Developer Guide.
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table containing the requested items.</param>
        /// <param name="attributesToGet">The names of one or more attributes to retrieve.  If no attribute   names are specified, then all attributes will be returned. If   any of the requested attributes are not found, they will not   appear in the result.   Note that <i>AttributesToGet</i> has no effect on provisioned throughput consumption.    DynamoDB determines capacity units consumed based on item size, not on the amount of data that is returned to an application.</param>
        /// <param name="scanFilter">Evaluates the scan results and returns only the desired values. If you specify more than one condition in the <i>ScanFilter</i> map, then by default all of   the conditions must evaluate to true. In other words, the conditions are ANDed together. (You   can use the <i>ConditionalOperator</i> parameter to OR the conditions instead. If you do this,   then at least one of the conditions must evaluate to true, rather than all of them.) Each <i>ScanFilter</i> element consists of an attribute name to compare, along with the   following: <ul>   <li>  <i>AttributeValueList</i> - One or more values to evaluate against the supplied    attribute. The number of values in the list depends on the <i>ComparisonOperator</i> being    used.  For type Number, value comparisons are numeric.  String value comparisons for greater than, equals, or less than are based on ASCII    character code values. For example, <code>a</code> is greater than <code>A</code>, and   <code>aa</code> is greater than <code>B</code>. For a list of code values, see <a href="http://en.wikipedia.org/wiki/ASCII#ASCII_printable_characters">http://en.wikipedia.org/wiki/ASCII#ASCII_printable_characters</a>.  For Binary, DynamoDB treats each byte of the binary data as unsigned when it compares binary    values, for example when evaluating query expressions.  For information on specifying data types in JSON, see <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DataFormat.html">JSON Data Format</a> in the Amazon DynamoDB Developer Guide.   </li>   <li><i>ComparisonOperator</i> - A comparator for evaluating attributes. For example,    equals, greater than, less than, etc. The following comparison operators are available:   <code>EQ | NE | LE | LT | GE | GT | NOT_NULL | NULL | CONTAINS | NOT_CONTAINS | BEGINS_WITH | IN | BETWEEN</code> For complete    descriptions of all comparison operators, see <a href="http://docs.aws.amazon.com/amazondynamodb/latest/APIReference/API_Condition.html">Condition</a>.   </li> </ul></param>
        /// 
        /// <returns>The response from the Scan service method, as returned by DynamoDB.</returns>
        /// <exception cref="InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="ProvisionedThroughputExceededException">
        /// The request rate is too high, or the request is too large, for the available throughput
        /// to      accommodate. The AWS SDKs automatically retry requests that receive this exception;
        /// therefore,      your request will eventually succeed, unless the request is too large
        /// or your retry queue is      too large to finish. Reduce the frequency of requests
        /// by using the strategies listed in <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ErrorHandling.html#APIRetries">Error
        /// Retries and Exponential Backoff</a>      in the Amazon DynamoDB Developer Guide.
        /// </exception>
        /// <exception cref="ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource may not be
        /// specified      correctly, or its status may not be <code>ACTIVE</code>.
        /// </exception>
        public ScanResponse Scan(string tableName, List<string> attributesToGet, Dictionary<string, Condition> scanFilter)
        {
            var request = new ScanRequest();
            request.TableName = tableName;
            request.AttributesToGet = attributesToGet;
            request.ScanFilter = scanFilter;
            return Scan(request);
        }

        internal ScanResponse Scan(ScanRequest request)
        {
            var marshaller = new ScanRequestMarshaller();
            var unmarshaller = ScanResponseUnmarshaller.Instance;

            return Invoke<ScanRequest,ScanResponse>(request, marshaller, unmarshaller);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the Scan operation.
        /// <seealso cref="Amazon.DynamoDBv2.IAmazonDynamoDB"/>
        /// </summary>
        /// 
        /// <param name="request">Container for the necessary parameters to execute the Scan operation.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<ScanResponse> ScanAsync(ScanRequest request, System.Threading.CancellationToken cancellationToken = default(CancellationToken))
        {
            var marshaller = new ScanRequestMarshaller();
            var unmarshaller = ScanResponseUnmarshaller.Instance;

            return InvokeAsync<ScanRequest,ScanResponse>(request, marshaller, 
                unmarshaller, cancellationToken);
        }

        #endregion
        
        #region  UpdateItem

        /// <summary>
        /// Edits an existing item's attributes, or inserts a new item if it does not already
        /// exist. You      can put, delete, or add attribute values. You can also perform a conditional
        /// update (insert a      new attribute name-value pair if it doesn't exist, or replace
        /// an existing name-value pair if      it has certain expected attribute values).
        /// 
        ///     
        /// <para>
        /// In addition to updating an item, you can also return the item's attribute values in
        /// the same      operation, using the <i>ReturnValues</i> parameter.
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table containing the item to update. </param>
        /// <param name="key">The primary key of the item to be updated. Each element consists of an attribute name and a   value for that attribute. For the primary key, you must provide <i>all</i> of the attributes. For example, with a hash   type primary key, you only need to specify the hash attribute. For a hash-and-range type   primary key, you must specify <i>both</i> the hash attribute and the range attribute.</param>
        /// <param name="attributeUpdates">The names of attributes to be modified, the action to perform on each, and the new value for   each. If you are updating an attribute that is an index key attribute for any indexes on that   table, the attribute type must match the index key type defined in the  <i>AttributesDefinition</i> of the table description. You can use <i>UpdateItem</i> to   update any non-key attributes. Attribute values cannot be null. String and binary type attributes must have lengths greater   than zero. Set type attributes must not be empty. Requests with empty values will be rejected   with a <i>ValidationException</i>. Each <i>AttributeUpdates</i> element consists of an attribute name to modify, along with the   following: <ul>   <li>  <i>Value</i> - The new value, if applicable, for this attribute.   </li>   <li>  <i>Action</i> - Specifies how to perform the update. Valid values for <i>Action</i> are   <code>PUT</code>, <code>DELETE</code>, and <code>ADD</code>. The behavior depends on    whether the specified primary key already exists in the table.      <b>If an item with the specified <i>Key</i> is found in the table:</b>    <ul>    <li>   <code>PUT</code> - Adds the specified attribute to the item. If the attribute already     exists, it is replaced by the new value.     </li>    <li>   <code>DELETE</code> - If no value is specified, the attribute and its value are     removed from the item. The data type of the specified value must match the existing     value's data type.   If a <i>set</i> of values is specified, then those values are subtracted from the old     set. For example, if the attribute value was the set <code>[a,b,c]</code> and the    <i>DELETE</i> action specified <code>[a,c]</code>, then the final attribute value     would be <code>[b]</code>. Specifying an empty set is an error.    </li>    <li>   <code>ADD</code> - If the attribute does not already exist, then the attribute and     its values are added to the item. If the attribute does exist, then the behavior of    <code>ADD</code> depends on the data type of the attribute:   <ul>     <li>    If the existing attribute is a number, and if <i>Value</i> is also a number, then      the <i>Value</i> is mathematically added to the existing attribute. If     <i>Value</i> is a negative number, then it is subtracted from the existing      attribute.    <note>       If you use <code>ADD</code> to increment or decrement a number value for an     item that doesn't exist before the update, DynamoDB uses 0 as the initial     value.      In addition, if you use <code>ADD</code> to update an existing item, and intend     to increment or decrement an attribute value which does not yet exist, DynamoDB     uses <code>0</code> as the initial value. For example, suppose that the item you     want to update does not yet have an attribute named <i>itemcount</i>, but you     decide to <code>ADD</code> the number <code>3</code> to this attribute anyway,     even though it currently does not exist. DynamoDB will create the <i>itemcount</i>     attribute, set its initial value to <code>0</code>, and finally add       <code>3</code> to it. The result will be a new <i>itemcount</i> attribute in     the item, with a value of <code>3</code>.    </note>     </li>     <li>    If the existing data type is a set, and if the <i>Value</i> is also a set, then      the <i>Value</i> is added to the existing set. (This is a <i>set</i> operation,      not mathematical addition.) For example, if the attribute value was the set     <code>[1,2]</code>, and the <code>ADD</code> action specified <code>[3]</code>,      then the final attribute value would be <code>[1,2,3]</code>. An error occurs if      an Add action is specified for a set attribute and the attribute type specified      does not match the existing set type.     Both sets must have the same primitive data type. For example, if the existing      data type is a set of strings, the <i>Value</i> must also be a set of strings. The      same holds true for number sets and binary sets.     </li>   </ul>   This action is only valid for an existing attribute whose data type is number or is a     set. Do not use <code>ADD</code> for any other data types.    </li>  </ul>      <b>If no item with the specified <i>Key</i> is found:</b>    <ul>    <li>   <code>PUT</code> - DynamoDB creates a new item with the specified primary key, and then     adds the attribute.     </li>    <li>   <code>DELETE</code> - Nothing happens; there is no attribute to delete.    </li>    <li>   <code>ADD</code> - DynamoDB creates an item with the supplied primary key and number (or     set of numbers) for the attribute value. The only data types allowed are number and     number set; no other data types can be specified.    </li>  </ul>   </li> </ul> If you specify any attributes that are part of an index key, then the data types for those   attributes must match those of the schema in the table's attribute definition.</param>
        /// 
        /// <returns>The response from the UpdateItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="ConditionalCheckFailedException">
        /// A condition specified in the operation could not be evaluated.
        /// </exception>
        /// <exception cref="InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="ItemCollectionSizeLimitExceededException">
        /// An item collection is too large. This exception is only returned for tables that have
        /// one or      more local secondary indexes.
        /// </exception>
        /// <exception cref="ProvisionedThroughputExceededException">
        /// The request rate is too high, or the request is too large, for the available throughput
        /// to      accommodate. The AWS SDKs automatically retry requests that receive this exception;
        /// therefore,      your request will eventually succeed, unless the request is too large
        /// or your retry queue is      too large to finish. Reduce the frequency of requests
        /// by using the strategies listed in <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ErrorHandling.html#APIRetries">Error
        /// Retries and Exponential Backoff</a>      in the Amazon DynamoDB Developer Guide.
        /// </exception>
        /// <exception cref="ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource may not be
        /// specified      correctly, or its status may not be <code>ACTIVE</code>.
        /// </exception>
        public UpdateItemResponse UpdateItem(string tableName, Dictionary<string, AttributeValue> key, Dictionary<string, AttributeValueUpdate> attributeUpdates)
        {
            var request = new UpdateItemRequest();
            request.TableName = tableName;
            request.Key = key;
            request.AttributeUpdates = attributeUpdates;
            return UpdateItem(request);
        }

        /// <summary>
        /// Edits an existing item's attributes, or inserts a new item if it does not already
        /// exist. You      can put, delete, or add attribute values. You can also perform a conditional
        /// update (insert a      new attribute name-value pair if it doesn't exist, or replace
        /// an existing name-value pair if      it has certain expected attribute values).
        /// 
        ///     
        /// <para>
        /// In addition to updating an item, you can also return the item's attribute values in
        /// the same      operation, using the <i>ReturnValues</i> parameter.
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table containing the item to update. </param>
        /// <param name="key">The primary key of the item to be updated. Each element consists of an attribute name and a   value for that attribute. For the primary key, you must provide <i>all</i> of the attributes. For example, with a hash   type primary key, you only need to specify the hash attribute. For a hash-and-range type   primary key, you must specify <i>both</i> the hash attribute and the range attribute.</param>
        /// <param name="attributeUpdates">The names of attributes to be modified, the action to perform on each, and the new value for   each. If you are updating an attribute that is an index key attribute for any indexes on that   table, the attribute type must match the index key type defined in the  <i>AttributesDefinition</i> of the table description. You can use <i>UpdateItem</i> to   update any non-key attributes. Attribute values cannot be null. String and binary type attributes must have lengths greater   than zero. Set type attributes must not be empty. Requests with empty values will be rejected   with a <i>ValidationException</i>. Each <i>AttributeUpdates</i> element consists of an attribute name to modify, along with the   following: <ul>   <li>  <i>Value</i> - The new value, if applicable, for this attribute.   </li>   <li>  <i>Action</i> - Specifies how to perform the update. Valid values for <i>Action</i> are   <code>PUT</code>, <code>DELETE</code>, and <code>ADD</code>. The behavior depends on    whether the specified primary key already exists in the table.      <b>If an item with the specified <i>Key</i> is found in the table:</b>    <ul>    <li>   <code>PUT</code> - Adds the specified attribute to the item. If the attribute already     exists, it is replaced by the new value.     </li>    <li>   <code>DELETE</code> - If no value is specified, the attribute and its value are     removed from the item. The data type of the specified value must match the existing     value's data type.   If a <i>set</i> of values is specified, then those values are subtracted from the old     set. For example, if the attribute value was the set <code>[a,b,c]</code> and the    <i>DELETE</i> action specified <code>[a,c]</code>, then the final attribute value     would be <code>[b]</code>. Specifying an empty set is an error.    </li>    <li>   <code>ADD</code> - If the attribute does not already exist, then the attribute and     its values are added to the item. If the attribute does exist, then the behavior of    <code>ADD</code> depends on the data type of the attribute:   <ul>     <li>    If the existing attribute is a number, and if <i>Value</i> is also a number, then      the <i>Value</i> is mathematically added to the existing attribute. If     <i>Value</i> is a negative number, then it is subtracted from the existing      attribute.    <note>       If you use <code>ADD</code> to increment or decrement a number value for an     item that doesn't exist before the update, DynamoDB uses 0 as the initial     value.      In addition, if you use <code>ADD</code> to update an existing item, and intend     to increment or decrement an attribute value which does not yet exist, DynamoDB     uses <code>0</code> as the initial value. For example, suppose that the item you     want to update does not yet have an attribute named <i>itemcount</i>, but you     decide to <code>ADD</code> the number <code>3</code> to this attribute anyway,     even though it currently does not exist. DynamoDB will create the <i>itemcount</i>     attribute, set its initial value to <code>0</code>, and finally add       <code>3</code> to it. The result will be a new <i>itemcount</i> attribute in     the item, with a value of <code>3</code>.    </note>     </li>     <li>    If the existing data type is a set, and if the <i>Value</i> is also a set, then      the <i>Value</i> is added to the existing set. (This is a <i>set</i> operation,      not mathematical addition.) For example, if the attribute value was the set     <code>[1,2]</code>, and the <code>ADD</code> action specified <code>[3]</code>,      then the final attribute value would be <code>[1,2,3]</code>. An error occurs if      an Add action is specified for a set attribute and the attribute type specified      does not match the existing set type.     Both sets must have the same primitive data type. For example, if the existing      data type is a set of strings, the <i>Value</i> must also be a set of strings. The      same holds true for number sets and binary sets.     </li>   </ul>   This action is only valid for an existing attribute whose data type is number or is a     set. Do not use <code>ADD</code> for any other data types.    </li>  </ul>      <b>If no item with the specified <i>Key</i> is found:</b>    <ul>    <li>   <code>PUT</code> - DynamoDB creates a new item with the specified primary key, and then     adds the attribute.     </li>    <li>   <code>DELETE</code> - Nothing happens; there is no attribute to delete.    </li>    <li>   <code>ADD</code> - DynamoDB creates an item with the supplied primary key and number (or     set of numbers) for the attribute value. The only data types allowed are number and     number set; no other data types can be specified.    </li>  </ul>   </li> </ul> If you specify any attributes that are part of an index key, then the data types for those   attributes must match those of the schema in the table's attribute definition.</param>
        /// <param name="returnValues">Use <i>ReturnValues</i> if you want to get the item attributes as they appeared either before   or after they were updated. For <i>UpdateItem</i>, the valid values are: <ul>   <li>  <code>NONE</code> - If <i>ReturnValues</i> is not specified, or if its value is   <code>NONE</code>, then nothing is returned. (This is the default for   <i>ReturnValues</i>.)   </li>   <li>  <code>ALL_OLD</code> - If <i>UpdateItem</i> overwrote an attribute name-value pair, then    the content of the old item is returned.   </li>   <li>  <code>UPDATED_OLD</code> - The old versions of only the updated attributes are    returned.   </li>   <li>  <code>ALL_NEW</code> - All of the attributes of the new version of the item are    returned.   </li>   <li>  <code>UPDATED_NEW</code> - The new versions of only the updated attributes are    returned.   </li> </ul></param>
        /// 
        /// <returns>The response from the UpdateItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="ConditionalCheckFailedException">
        /// A condition specified in the operation could not be evaluated.
        /// </exception>
        /// <exception cref="InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="ItemCollectionSizeLimitExceededException">
        /// An item collection is too large. This exception is only returned for tables that have
        /// one or      more local secondary indexes.
        /// </exception>
        /// <exception cref="ProvisionedThroughputExceededException">
        /// The request rate is too high, or the request is too large, for the available throughput
        /// to      accommodate. The AWS SDKs automatically retry requests that receive this exception;
        /// therefore,      your request will eventually succeed, unless the request is too large
        /// or your retry queue is      too large to finish. Reduce the frequency of requests
        /// by using the strategies listed in <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ErrorHandling.html#APIRetries">Error
        /// Retries and Exponential Backoff</a>      in the Amazon DynamoDB Developer Guide.
        /// </exception>
        /// <exception cref="ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource may not be
        /// specified      correctly, or its status may not be <code>ACTIVE</code>.
        /// </exception>
        public UpdateItemResponse UpdateItem(string tableName, Dictionary<string, AttributeValue> key, Dictionary<string, AttributeValueUpdate> attributeUpdates, ReturnValue returnValues)
        {
            var request = new UpdateItemRequest();
            request.TableName = tableName;
            request.Key = key;
            request.AttributeUpdates = attributeUpdates;
            request.ReturnValues = returnValues;
            return UpdateItem(request);
        }

        internal UpdateItemResponse UpdateItem(UpdateItemRequest request)
        {
            var marshaller = new UpdateItemRequestMarshaller();
            var unmarshaller = UpdateItemResponseUnmarshaller.Instance;

            return Invoke<UpdateItemRequest,UpdateItemResponse>(request, marshaller, unmarshaller);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the UpdateItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.IAmazonDynamoDB"/>
        /// </summary>
        /// 
        /// <param name="request">Container for the necessary parameters to execute the UpdateItem operation.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<UpdateItemResponse> UpdateItemAsync(UpdateItemRequest request, System.Threading.CancellationToken cancellationToken = default(CancellationToken))
        {
            var marshaller = new UpdateItemRequestMarshaller();
            var unmarshaller = UpdateItemResponseUnmarshaller.Instance;

            return InvokeAsync<UpdateItemRequest,UpdateItemResponse>(request, marshaller, 
                unmarshaller, cancellationToken);
        }

        #endregion
        
        #region  UpdateTable

        /// <summary>
        /// Updates the provisioned throughput for the given table. Setting the throughput for
        /// a table      helps you manage performance and is part of the provisioned throughput
        /// feature of DynamoDB.
        /// 
        ///     
        /// <para>
        /// The provisioned throughput values can be upgraded or downgraded based on the maximums
        /// and      minimums listed in the <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Limits.html">Limits</a>
        /// section in the      Amazon DynamoDB Developer Guide.
        /// </para>
        ///     
        /// <para>
        /// The table must be in the <code>ACTIVE</code> state for this operation to succeed.
        ///        <i>UpdateTable</i> is an asynchronous operation; while executing the operation,
        /// the table is      in the <code>UPDATING</code> state. While the table is in the <code>UPDATING</code>
        /// state, the      table still has the provisioned throughput from before the call. The
        /// new provisioned      throughput setting is in effect only when the table returns to
        /// the <code>ACTIVE</code> state      after the <i>UpdateTable</i> operation. 
        /// </para>
        ///     
        /// <para>
        /// You cannot add, modify or delete indexes using <i>UpdateTable</i>. Indexes can only
        /// be      defined at table creation time.
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table to be updated.</param>
        /// <param name="provisionedThroughput">A property of UpdateTableRequest used to execute the UpdateTable service method.</param>
        /// 
        /// <returns>The response from the UpdateTable service method, as returned by DynamoDB.</returns>
        /// <exception cref="InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="LimitExceededException">
        /// The number of concurrent table requests (cumulative number of tables in the      
        ///  <code>CREATING</code>, <code>DELETING</code> or <code>UPDATING</code> state) exceeds
        /// the      maximum allowed of 10.
        /// 
        ///     
        /// <para>
        /// Also, for tables with secondary indexes, only one of those tables can be in the <code>CREATING</code>
        ///      state at any point in time. Do not attempt to create more than one such table
        ///      simultaneously.
        /// </para>
        ///     
        /// <para>
        /// The total limit of tables in the <code>ACTIVE</code> state is 250.
        /// </para>
        /// </exception>
        /// <exception cref="ResourceInUseException">
        /// The operation conflicts with the resource's availability. For example, you attempted
        /// to      recreate an existing table, or tried to delete a table currently in the <code>CREATING</code>
        ///      state.
        /// </exception>
        /// <exception cref="ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource may not be
        /// specified      correctly, or its status may not be <code>ACTIVE</code>.
        /// </exception>
        public UpdateTableResponse UpdateTable(string tableName, ProvisionedThroughput provisionedThroughput)
        {
            var request = new UpdateTableRequest();
            request.TableName = tableName;
            request.ProvisionedThroughput = provisionedThroughput;
            return UpdateTable(request);
        }

        internal UpdateTableResponse UpdateTable(UpdateTableRequest request)
        {
            var marshaller = new UpdateTableRequestMarshaller();
            var unmarshaller = UpdateTableResponseUnmarshaller.Instance;

            return Invoke<UpdateTableRequest,UpdateTableResponse>(request, marshaller, unmarshaller);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the UpdateTable operation.
        /// <seealso cref="Amazon.DynamoDBv2.IAmazonDynamoDB"/>
        /// </summary>
        /// 
        /// <param name="request">Container for the necessary parameters to execute the UpdateTable operation.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<UpdateTableResponse> UpdateTableAsync(UpdateTableRequest request, System.Threading.CancellationToken cancellationToken = default(CancellationToken))
        {
            var marshaller = new UpdateTableRequestMarshaller();
            var unmarshaller = UpdateTableResponseUnmarshaller.Instance;

            return InvokeAsync<UpdateTableRequest,UpdateTableResponse>(request, marshaller, 
                unmarshaller, cancellationToken);
        }

        #endregion
        
    }
}