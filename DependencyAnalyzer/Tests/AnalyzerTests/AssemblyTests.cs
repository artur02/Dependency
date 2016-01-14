using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Analyzer;
using FluentAssertions;
using NUnit.Framework;
using TestHelpers;


namespace AnalyzerTests
{
    [TestFixture(TestOf = typeof(Assembly))]
    public class AssemblyTests
    {
       
        [TestFixture]
        public class WhenIGetTypesDefinedinAnAssembly
        {
            [TestCase("SimpleMethod.cs", "GeneratedAssemblySimpleMethod.dll", new[] { "<Module>", "Test" })]
            [TestCase("MethodWithBody.cs", "GeneratedAssemblyMethodWithBody.dll", new[] { "<Module>", "Test" })]
            public void ThenIShouldGetAllTypesDefined(string fileToBeCompiled, string generatedAssemblyName, IEnumerable<string> expectedTypeNames)
            {
                var assembly = GenerateAndLoadAssembly(fileToBeCompiled, generatedAssemblyName);

                var types = assembly.GetTypes();

                types
                    .Select(type => type.FullName)
                    .ShouldBeEquivalentTo(expectedTypeNames);

                DeleteFileIfExists(generatedAssemblyName);
            }
        }

        [TestFixture]
        public class WhenIGetAllDependenciesForAType
        {
            [TestCase("SimpleMethod.cs", "GeneratedAssemblySimpleMethod.dll", new[] { "System.Int32", "System.Object" })]
            [TestCase("MethodWithBody.cs", "GeneratedAssemblyMethodWithBody.dll", new[] { "System.Int32", "System.Object", "System.String", "System.SByte" })]
            public void ThenAllDependenciesShouldBeReturned(string fileToBeCompiled, string generatedAssemblyName, IEnumerable<string> expectedTypeNames)
            {
                var assembly = GenerateAndLoadAssembly(fileToBeCompiled, generatedAssemblyName);
                Assume.That(assembly, Is.Not.Null);
                var type = assembly.GetTypes().First(t => t.FullName == "Test");

                var dependencies = type.GetReferencedTypes().ToList();

                dependencies
                    .Select(d => d.Key.FullName)
                    .ShouldBeEquivalentTo(expectedTypeNames);

                DeleteFileIfExists(generatedAssemblyName);
            }
        }

        [TestFixture]
        public class WhenIGetAssembliesReferencedDirectlyByTheAnalyzedAssembly
        {
            [TestCase("SimpleMethod.cs", "GeneratedAssemblySimpleMethod.dll", new[] { "CommonLanguageRuntimeLibrary" })]
            [TestCase("MethodWithBody.cs", "GeneratedAssemblyMethodWithBody.dll", new[] { "CommonLanguageRuntimeLibrary" })]
            public void ThenAllReferencedAssembliesShouldBeEnumerated(string fileToBeCompiled, string generatedAssemblyName, IEnumerable<string> expectedAssemblies)
            {
                var assembly = GenerateAndLoadAssembly(fileToBeCompiled, generatedAssemblyName);
                Assume.That(assembly, Is.Not.Null);
                var references = assembly.References;

                references
                    .Select(a => a.Name)
                    .ShouldBeEquivalentTo(expectedAssemblies );

                DeleteFileIfExists(generatedAssemblyName);
            }
        }

        [TestFixture]
        public class WhenIWantToCheckWhetherAFileIsAnAssembly
        {
            [TestCase("SimpleMethod.cs", "GeneratedAssemblySimpleMethod.dll", true)]
            [TestCase("MethodWithBody.cs", "GeneratedAssemblyMethodWithBody.dll", true)]
            public void ThenADotNetAssemblyShouldBeRecognized(string fileToBeCompiled, string generatedAssemblyName, bool expectedResult)
            {
                var assembly = GenerateAndLoadAssembly(fileToBeCompiled, generatedAssemblyName);
                Assume.That(assembly, Is.Not.Null);
                Exception exception;
                var result = Assembly.IsAssembly(generatedAssemblyName, out exception);

                result.Should().Be(expectedResult);
            }
        }















        protected static Assembly GenerateAndLoadAssembly(string fileName, string generatedAssemblyName)
        {
            DeleteFileIfExists(generatedAssemblyName);

            var helper = new Helper();
            helper.GenerateAssemblyAsync(System.Reflection.Assembly.GetExecutingAssembly(),
                "AnalyzerTests.TestCodes", fileName, generatedAssemblyName);
            var assembly = new Assembly(generatedAssemblyName, new ComponentCache());

            return assembly;
        }

        protected static void DeleteFileIfExists(string path)
        {
            var file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();
            }
        }
    }
}