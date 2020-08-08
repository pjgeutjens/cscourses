using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using csQA.DTO;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Queue;

namespace csQA.Functions
{
    public class DeleteCourse
    {

        private readonly ILogger<DeleteCourse> _logger;
        private readonly Constants _settings;
        private CosmosClient _cosmosClient;
        private Container _container;

        public DeleteCourse(ILogger<DeleteCourse> logger, IOptions<Constants> options, CosmosClient cosmosClient)
        {
            _logger = logger;
            _settings = options.Value;
            _cosmosClient = cosmosClient;
            _container = _cosmosClient.GetContainer(_settings.CosmosDbDatabaseName,_settings.CosmosDbContainerName);

        }

        [FunctionName("DeleteCourse")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "courses/{courseId}")] HttpRequest req, string courseId)
        {
            ItemResponse<Course> item = await  _container.DeleteItemAsync<Course>(courseId, new PartitionKey("QUESTIONS"));
            
            return new OkObjectResult(item.Resource);

        }
    }
}
