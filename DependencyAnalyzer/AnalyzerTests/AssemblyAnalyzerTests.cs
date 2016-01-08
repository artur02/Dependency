using System;
using System.Linq;
using System.Reflection;
using Analyzer;
using TestHelpers;
using Xunit;
using FluentAssertions;

namespace AnalyzerTests
{
    public class AssemblyAnalyzerTests : IClassFixture<LoggingFixture>
    {
        public class SimpleMethodTests : IDisposable, IClassFixture<LoggingFixture>
        {

            const string GeneratedAssemblyName = "GeneratedAssemblySimpleMethod.dll";
            readonly AssemblyAnalyzer assemblyAnalyzer;

            public SimpleMethodTests()
            {
                var helper = new Helper();
                helper.GenerateAssemblyAsync(Assembly.GetExecutingAssembly(), "AnalyzerTests.TestCodes", "SimpleMethod.cs", GeneratedAssemblyName);

                
                assemblyAnalyzer = new AssemblyAnalyzer(GeneratedAssemblyName);
            }

            [Fact]
            public void GetTypes()
            {
                this.Log().Debug("Getting types");

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
                    .ShouldBeEquivalentTo(new[] {"System.Int32", "System.Int32", "System.Int32"});
            }

            public void Dispose()
            {
                Helper.DeleteFile(GeneratedAssemblyName);
            }
        }

        public class MethodWithBodyTests : IDisposable
        {

            const string GeneratedAssemblyName = "GeneratedAssemblyMethodWithBody.dll";
            readonly AssemblyAnalyzer assemblyAnalyzer;

            public MethodWithBodyTests()
            {
                var helper = new Helper();
                helper.GenerateAssemblyAsync(Assembly.GetExecutingAssembly(), "AnalyzerTests.TestCodes", "MethodWithBody.cs", GeneratedAssemblyName);


                assemblyAnalyzer = new AssemblyAnalyzer(GeneratedAssemblyName);
            }

            [Fact]
            public void GetTypes()
            {
                var types = assemblyAnalyzer.GetAllTypes();

                types
                    .Select(type => type.FullName)
                    .ShouldBeEquivalentTo(new[] { "<Module>", "Test" });

            }

            [Fact]
            public void GetDependencies()
            {
                var type = assemblyAnalyzer.GetAllTypes().First(t => t.FullName == "Test");

                var dependencies = type.GetDependencies().ToList();

                dependencies
                    .Select(d => d.FullName)
                    .ShouldBeEquivalentTo(new[] { "System.Int32", "System.Int32", "System.Int32", "System.Int32", "System.String" });
            }

            public void Dispose()
            {
                Helper.DeleteFile(GeneratedAssemblyName);
            }
        }
    }
}
