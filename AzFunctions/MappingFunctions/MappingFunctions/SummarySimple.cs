using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace MappingFunctions
{
    public static class SummarySimple
    {
        [FunctionName("SummarySimple")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Starting SummarySimple.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            if (string.IsNullOrEmpty(requestBody))
            {
                return new BadRequestResult();
            }

            // Add more error handling
            Catalog.SummaryValueMapMapToSummary mapper = new Catalog.SummaryValueMapMapToSummary();

            Altova.IO.Input booksSource = new Altova.IO.StringInput(requestBody);
            StringBuilder responseMessageBuilder = new StringBuilder();
            Altova.IO.Output target = new Altova.IO.StringOutput(responseMessageBuilder);

            mapper.Run(booksSource, target);


            return new OkObjectResult(responseMessageBuilder.ToString());
        }
    }
}


