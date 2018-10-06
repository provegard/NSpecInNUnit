using System;
using System.Collections.Generic;
using System.Text;
using NSpec.Domain;

namespace NSpecInNUnit
{
    internal static class TestNamer
    {
        internal static string CreateReSharperSafeName(ExampleBase example)
        {
            // ReSharper displays tests with period in their name incorrectly,
            // see: https://youtrack.jetbrains.net/issue/RSRP-469271
            var name = example.FullName();
            return name.TrimEnd('.', ' ').Replace('.', ',');
        }
    }
}
