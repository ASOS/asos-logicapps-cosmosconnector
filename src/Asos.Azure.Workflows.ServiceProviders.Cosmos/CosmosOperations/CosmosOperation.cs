using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Workflows.ServiceProviders.Abstractions;
using Microsoft.WindowsAzure.ResourceStack.Common.Collections;
using Microsoft.WindowsAzure.ResourceStack.Common.Json;
using Newtonsoft.Json.Linq;

namespace Asos.Azure.Workflows.ServiceProviders.Cosmos.CosmosOperations
{
    public abstract class CosmosOperation
    {
        protected readonly InsensitiveDictionary<JToken> ConnectionParameters;
        protected readonly ServiceOperationRequest ServiceOperationRequest;

        protected CosmosOperation(
            InsensitiveDictionary<JToken> connectionParameters,
            ServiceOperationRequest serviceOperationRequest)
        {
            ConnectionParameters = connectionParameters;
            ServiceOperationRequest = serviceOperationRequest;
        }

        public async Task<ServiceOperationResponse> Invoke()
        {
            try
            {
                return await DoInvocation();
            }
            catch (Exception e)
            {
                throw new ServiceOperationsProviderException(HttpStatusCode.InternalServerError,
                    ServiceOperationsErrorResponseCode.ServiceOperationFailed, e.Message, e);
            }
        }

        protected static ServiceOperationResponse ReturnBadRequestResponse(Exception e)
        {
            return new ServiceOperationResponse(new
            {
                Code = HttpStatusCode.BadRequest.ToString(),
                Message = e.Message
            }.ToJToken(),
                HttpStatusCode.BadRequest);
        }

        protected abstract Task<ServiceOperationResponse> DoInvocation();
    }
}