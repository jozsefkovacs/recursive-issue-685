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
    public static class WorkAround1Orch
    {
        [FunctionName(nameof(WorkAround1Orch))]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] DurableOrchestrationContext context)
        {
            var outputs = new List<string>();

            outputs.Add(await context.CallActivityAsync<string>(nameof(StringReturnActivity), "Tokyo"));
            var result2 = await context.CallActivityAsync<string>(nameof(Workaround1FixedActivity), "Novi Sad");

            var result = JsonConvert.DeserializeObject<SelfRefClass>(result2, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Objects,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
            });

            outputs.Add(result.Name);

            return outputs;
        }
    }
}