using arb;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using t9n.api.jsonConverter;
using t9n.api.model;

namespace t9n.api.Controllers
{
    [Authorize]
    [Route("api/resource")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
        private readonly ILogger<ResourceController> _logger;

        public ResourceController(ILogger<ResourceController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{domain}")]
        /// <summary>
        /// Retrieve all resrouces for a given domain
        /// </summary>
        /// <param name="domain">name of the domain of the expected resources</param>
        [ProducesResponseType(typeof(ArbResourceCollection),200)]
        public IActionResult GetAllResources(string domain)
        {
            ArbResourceCollection result = new ArbResourceCollection
            {
                Locale = "en",
                Context = domain,
                ArbEntries = new List<ArbResourceEntry>
                {
                    new ArbResourceEntry(Guid.NewGuid())
                    {
                        Id="title",
                        Value="Genome Master",
                        Type=new ArbReourceEntryAttribute("type") {AttributeValue="text"},
                        Context=new ArbReourceEntryAttribute("context"){AttributeValue="login screen"},
                        Description=new ArbReourceEntryAttribute("description"){AttributeValue="Name of the application"},
                        Source_text=new ArbReourceEntryAttribute("source_text"){AttributeValue="Genome Master"}
                    },
                    new ArbResourceEntry(Guid.NewGuid())
                    {
                        Id="test_plural",
                        Value="{age,plural,=1{Tu as {age} an.}other{Tu as {age} ans.}}"
                    },
                }
            };
            return Ok(result);
        }


        [HttpPost( "arb" )]
        public IActionResult PostArbFile(IFormFile formFile)
        {
            if ( formFile == null || formFile.Length == 0 )
            {
                return StatusCode( 400 );
            }

            var stringBuilder = new StringBuilder();
            using ( var reader = new StreamReader( formFile.OpenReadStream() ) )
            {
                while ( reader.Peek() >= 0 )
                {
                    stringBuilder.AppendLine( reader.ReadLine() );
                }
            }
            string json = stringBuilder.ToString();
            var options = new JsonSerializerOptions();
            options.Converters.Add( new ArbResourceCollectionJsonConverter() );
            ArbResourceCollection arbResourceCollection = JsonSerializer.Deserialize<ArbResourceCollection>( json, options );
            return Ok(new ApiMessage(200,"File uploaded", "" ) { Value=arbResourceCollection});
        }

      
    }
}
