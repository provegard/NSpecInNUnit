using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Common;

namespace NSpecInNUnit.Tests.Infra
{
    class TestTrackingWriter : ExtendedTextWriter
    {
        private static TestTrackingWriter _instance;
        private int _testCount = 0;
        private readonly List<string> _output = new List<string>();
        private readonly List<string> _problems = new List<string>();
        private readonly List<string> _failures = new List<string>();
        public override Encoding Encoding => Encoding.UTF8;

        internal IEnumerable<string> Output => _output;
        internal IEnumerable<string> Problems => _problems;
        internal IEnumerable<string> Failures => _failures;
        internal int TestCount => _testCount;

        internal bool HasProblems => _problems.Count > 0;
        internal bool HasFailures => _failures.Count > 0;

        internal TestTrackingWriter()
        {
            _instance = this;
        }
        
        private void CollectOutput(string value)
        {
            //TODO: Lock here?
            _output.Add(value.Trim());
        }
        private void CollectProblem(string value)
        {
            //TODO: Lock here?
            _problems.Add(value.Trim());
        }
        private void CollectFailure(string value)
        {
            //TODO: Lock here?
            _failures.Add(value.Trim());
        }

        private void HandleWrite(ColorStyle style, string value)
        {
            if (style == ColorStyle.Output) 
                CollectOutput(value);
            else if (style == ColorStyle.Error || style == ColorStyle.Warning)
                CollectProblem(value);
            else if (style == ColorStyle.Failure)
                CollectFailure(value);
        }
            
        public override void Write(ColorStyle style, string value)
        {
            HandleWrite(style, value);
        }

        public override void WriteLine(ColorStyle style, string value)
        {
            HandleWrite(style, value);
        }

        public override void WriteLabel(string label, object option)
        {
            if (label.Contains("Test Count"))
            {
                _testCount = int.Parse(option.ToString());
            }
        }

        public override void WriteLabel(string label, object option, ColorStyle valueStyle)
        {
        }

        public override void WriteLabelLine(string label, object option)
        {
        }

        public override void WriteLabelLine(string label, object option, ColorStyle valueStyle)
        {
        }

        internal static void Log(string message)
        {
            if (_instance == null) throw new Exception("No active instance of TestTrackingWriter");
            _instance.CollectOutput(message);
        }
    }
}