using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using Catalog;

namespace MappingFunctions
{
    public static class SummaryAdvanced
    {
        [FunctionName("SummaryAdvanced")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Starting SummaryAdvanced.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            
            if (string.IsNullOrEmpty(requestBody))
            {
                return new BadRequestResult();
            }

            // TODO: Add more error handling
            
            // Create the mapper object
            SummaryLookupMapToCatalog_Summary_Schema mapper = new SummaryLookupMapToCatalog_Summary_Schema();
            
            // Sources 
            Altova.IO.Input booksSource = new Altova.IO.StringInput( requestBody);
            Altova.IO.Input shelfsSource = new Altova.IO.StringInput(MappingFunctions.Properties.Resources.Shelfs);
            
            // Mapping result 
            StringBuilder responseMessageBuilder = new StringBuilder();
            Altova.IO.Output Catalog_Summary_SchemaTarget = new Altova.IO.StringOutput(responseMessageBuilder);

            // Execute transformation
            mapper.Run(booksSource, shelfsSource, Catalog_Summary_SchemaTarget);

            
            return new OkObjectResult(responseMessageBuilder.ToString());
        }
    }
}
