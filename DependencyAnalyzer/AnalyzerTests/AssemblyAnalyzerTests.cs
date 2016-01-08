using System.Linq;
using System.Reflection;
using Analyzer;
using TestHelpers;
using Xunit;
using FluentAssertions;

namespace AnalyzerTests
{
    public class AssemblyAnalyzerTests
    {
        public class SimpleMethodTests
        {

            const string GeneratedAssemblyName = "GeneratedAssembly.dll";
            readonly AssemblyAnalyzer assemblyAnalyzer;

            public SimpleMethodTests()
            {
                var helper = new Helper();
                helper.GenerateAssemblyAsync(Assembly.GetExecutingAssembly(), "AnalyzerTests.TestCodes", "a.cs", GeneratedAssemblyName);

                
                assemblyAnalyzer = new AssemblyAnalyzer(GeneratedAssemblyName);
            }

            [Fact]
            public void GetTypes()
            {
                var types = assemblyAnalyzer.GetAllTypes();

                types.Select(type => type.FullName).ShouldBeEquivalentTo(new[] {"<Module>", "Test"});

            }

            [Fact]
            public void GetDependencies()
            {
                var type = assemblyAnalyzer.GetAllTypes().First(t => t.FullName == "Test");

                var dependencies = type.GetDependencies().ToList();

                dependencies
                    .Select(d => d.FullName)
                    .ShouldBeEquivalentTo(new[] {"System.Int32", "System.Int32"});
            }
        }
    }
}
