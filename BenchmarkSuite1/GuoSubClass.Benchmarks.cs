using BenchmarkDotNet.Attributes;
using CompassEx.Guo;
using Microsoft.VSDiagnostics;

namespace CompassEx.Benchmarks
{
    [CPUUsageDiagnoser]
    public class GuoSubClassBenchmarks
    {
        [Benchmark]
        public void GetGuoSub_ByIndex()
        {
            for (int i = 0; i < 8; i++)
            {
                var g = GuoSubClass.GetGuoSub(i, true);
            }
        }

        [Benchmark]
        public void GetGuoSub_ByName()
        {
            foreach (var name in GuoSubClass.BeforeGuoSubNames)
            {
                var g = GuoSubClass.GetGuoSub(name, true);
            }
        }

        [Benchmark]
        public void GetCBeforGuos()
        {
            var g = GuoSubClass.GetGuoSub(0, true);
            var result = g.GetCBeforGuos();
        }

        [Benchmark]
        public void GetC24Hills()
        {
            var g = GuoSubClass.GetGuoSub(0, true);
            var result = g.GetC24Hills();
        }
    }
}