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
    public class GetCourseById
    {

        private readonly ILogger<GetCourseById> _logger;
        private readonly Constants _settings;
        private CosmosClient _cosmosClient;
        private Container _container;

        public GetCourseById(ILogger<GetCourseById> logger, IOptions<Constants> options, CosmosClient cosmosClient)
        {
            _logger = logger;
            _settings = options.Value;
            _cosmosClient = cosmosClient;
            _container = _cosmosClient.GetContainer(_settings.CosmosDbDatabaseName,_settings.CosmosDbContainerName);

        }

        [FunctionName("GetCourseById")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", 
                Route = "courses/{courseId}")] HttpRequest req, 
                string courseId)
        {
            Course course = _container.GetItemLinqQueryable<Course>(true)
                .Where(q => q.Id == courseId)
                .AsEnumerable().FirstOrDefault();
            
            return new OkObjectResult(course);

        }
    }
}
