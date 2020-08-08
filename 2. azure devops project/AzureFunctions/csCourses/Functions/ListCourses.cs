using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using csQA.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace csQA.Functions
{
    public class ListCourses
    {
        private readonly ILogger<ListCourses> _logger;
        private readonly Constants _settings;

        private CosmosClient _cosmosClient; 

        private Container _container;

        public ListCourses(ILogger<ListCourses> logger, IOptions<Constants> options, CosmosClient cosmosClient)
        {
            _logger = logger;
            _settings = options.Value;
            _cosmosClient = cosmosClient;

            _container = _cosmosClient.GetContainer(_settings.CosmosDbDatabaseName,_settings.CosmosDbContainerName);
        }

        [FunctionName("GetCourses")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "courses")] HttpRequest req)
        {

            List<Course> returnValue = new List<Course>();

            QueryDefinition query = new QueryDefinition("SELECT * FROM c");
            FeedIterator<Course> feedIterator = _container.GetItemQueryIterator<Course>(query);

            while (feedIterator.HasMoreResults)
            {
                FeedResponse<Course> response = await feedIterator.ReadNextAsync();
                foreach (var item in response)
                {
                    returnValue.Add(item);
                }
            }

            return new OkObjectResult(returnValue);

        }
    }
}
