using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Recursion.Orch.Activity
{
    public static class BonusIssueOrch
    {
        [FunctionName(nameof(BonusIssueOrch))]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] DurableOrchestrationContext context)
        {
            var outputs = new List<string>();

            outputs.Add(await context.CallActivityAsync<string>(nameof(StringReturnActivity), "Tokyo"));
            var result2 = await context.CallActivityAsync<string>(nameof(RecursiveClassActivityReturningString), "Novi Sad");
            outputs.Add(result2);

            return outputs;
        }
    }
}