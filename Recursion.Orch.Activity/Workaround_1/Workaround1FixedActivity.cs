using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Recursion.Orch.Activity
{
    public static class Workaround1FixedActivity
    {
        [FunctionName(nameof(Workaround1FixedActivity))]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Saying hello to {name}.");

            var result = new SelfRefClass()
            {
                Desc = "desc",
                Name = name,
                OtherProp = "something else",
                Parent = new SelfRefClass
                {
                    Desc = "desc 2",
                    Name = name + " Parent",
                    OtherProp = "something",
                }
            };
            result.Parent.Parent = result;

            string json = JsonConvert.SerializeObject(result, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Objects,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
            });

            return json;
        }
    }
}