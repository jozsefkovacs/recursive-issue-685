using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Recursion.Orch.Activity
{
    public static class Orchestration
    {
        [FunctionName(nameof(Orchestration))]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] DurableOrchestrationContext context)
        {
            var outputs = new List<string>();

            outputs.Add(await context.CallActivityAsync<string>(nameof(StringReturnActivity), "Tokyo")); // This will pass always
            var result2 = await context.CallActivityAsync<SelfRefClass>(nameof(RecursiveClassActivity), "Novi Sad");
            outputs.Add(result2.Name);
            outputs.Add(result2.Parent?.Name);

            return outputs;
        }
    }
}