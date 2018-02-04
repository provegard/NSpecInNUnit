using System.Collections.Generic;
using NSpec.Domain;
using NSpec.Domain.Formatters;

namespace NSpecInNUnit
{
    /// <summary>
    /// An NSpec formatter that doesn't do anything.
    /// </summary>
    internal class NoopFormatter : IFormatter
    {
        public void Write(ContextCollection contexts)
        {
        }

        public IDictionary<string, string> Options { get; set; }
    }
}