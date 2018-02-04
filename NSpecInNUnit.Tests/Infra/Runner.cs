using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnitLite;

namespace NSpecInNUnit.Tests.Infra
{
    static class Runner
    {
        internal static IEnumerable<string> CollectOuputFrom<T>(params string[] exampleNames)
        {
            return CollectOuputFrom(typeof(T), exampleNames);
        }

        internal static IEnumerable<string> CollectOuputFrom(Type typeToRun, params string[] exampleNames)
        {
            var assembly = typeToRun.Assembly;
            var runner = new AutoRun(assembly);
//            var arg = $"/test:{typeToRun.FullName}";
            var arg = $"/where:class == {typeToRun.FullName}";
            if (exampleNames.Length > 0)
            {
                //TODO: Get rid of last dot!
                var parts = exampleNames.Select(n => $"test =~ /.*{Regex.Escape(n)}\\.$/");
                var oredParts = string.Join(" || ", parts);
                arg += $" && ({oredParts})";
            }
            // Minimize output sent to the writer
            // Single worker and fixed seed to make the results stable/consistent
            var writer = new TestTrackingWriter();
            runner.Execute(new[] {arg, "/noresult", "/noheader", "/nocolor", "/labels:on", "/workers:1", "/seed:1"}, writer, Console.In);
            if (writer.HasProblems)
            {
                var problemDesc = string.Join(Environment.NewLine, writer.Problems);
                throw new Exception(problemDesc);
            }

            if (writer.HasFailures)
            {
                var failureDesc = string.Join(Environment.NewLine, writer.Failures);
                throw new Exception(failureDesc);                
            }
            if (writer.TestCount == 0)
            {
                throw new Exception("No tests run");
            }
            return writer.Output;
        }
    }
}