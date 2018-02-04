namespace NSpecInNUnit.Tests.Infra
{
    internal static class Tracker
    {
        internal static void Log(string msg)
        {
            TestTrackingWriter.Log(msg);
        }
        
    }
}