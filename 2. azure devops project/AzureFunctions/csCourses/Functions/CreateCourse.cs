 using System;
using System.IO;
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
 using Newtonsoft.Json;

namespace csQA.Functions
{
    public class CreateCourse
    {
        private readonly ILogger _logger;
        private readonly Constants _settings;
        private CosmosClient _cosmosClient;

        private Database _database;
        private Container _container;

        public CreateCourse(
            ILogger<CreateCourse> logger,
            IOptions<Constants> options,
            CosmosClient cosmosClient)
        {
            _logger = logger;
            _settings = options.Value;
            _cosmosClient = cosmosClient;

            _database = _cosmosClient.GetDatabase(_settings.CosmosDbDatabaseName);
            _container = _database.GetContainer(_settings.CosmosDbContainerName);
        }

        [FunctionName("CreateCourse")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "courses")] HttpRequest req)
        {
            IActionResult returnValue = null;

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                var input = JsonConvert.DeserializeObject<Course>(requestBody);

                var course = new Course
                {
                    Id = Guid.NewGuid().ToString(),
                    PartitionKey = "COURSES",
                    Title = input.Title,
                    Description = input.Description
                };

                ItemResponse<Course> item =
                    await _container.CreateItemAsync(course, new PartitionKey("COURSES"));
              
                _logger.LogInformation("Item inserted");
                _logger.LogInformation($"This query cost: {item.RequestCharge} RU/s");
                
                returnValue = new OkObjectResult(course);

            }
            catch (Exception e)
            {
                _logger.LogError($"Could not insert item. Exception thrown: {e.Message}");
                returnValue = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return returnValue;

        }
    }
}
