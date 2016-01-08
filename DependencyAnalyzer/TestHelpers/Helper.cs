using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace TestHelpers
{
    public class Helper
    {
        public async Task GenerateAssemblyAsync( string code, string assemblyName)
        {
            var tree = SyntaxFactory.ParseSyntaxTree(code);
            var compilation = CSharpCompilation.Create(assemblyName,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary), 
                syntaxTrees: new[] {tree},
                references: new[] {MetadataReference.CreateFromFile(typeof (object).Assembly.Location)});

            using (var stream = new FileStream(assemblyName, FileMode.Create))
            {
                compilation.Emit(stream);
                stream.Flush(true);
            }
        }

        public async Task GenerateAssemblyAsync(Assembly callerAssembly, string resourceNamespace, string resourceName, string generatedAssemblyName)
        {
            if (File.Exists(generatedAssemblyName))
            {
                File.Delete(generatedAssemblyName);
            }


            var text = await GetEmbeddedResourceAsync(callerAssembly, resourceNamespace, resourceName);
            var helper = new Helper();
            await helper.GenerateAssemblyAsync(text, generatedAssemblyName);
        }

        public static async Task<string> GetEmbeddedResourceAsync(Assembly assembly, string @namespace, string resourceName)
        {
            using (var reader = new StreamReader(assembly.GetManifestResourceStream($"{@namespace}.{resourceName}")))
            {
                return reader.ReadToEnd();
            }
        }

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
