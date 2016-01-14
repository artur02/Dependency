$openCoverPath = "..\packages\OpenCover.4.6.401-rc\tools\OpenCover.Console.exe"
$nUnitPath = "..\packages\NUnit.Console.3.0.1\tools\nunit3-console.exe"
$reportGeneratorPath = "..\packages\ReportGenerator.2.4.0-beta2\tools\ReportGenerator.exe"

$testAssemblies = "AnalyzerTests\Bin\Debug\AnalyzerTests.dll"

$nUnitArgs = "$testAssemblies"

$coverageFilters = "+[*]* -[FluentAssertions*]* -[*]System.Diagnostics.Contracts.*"

Write-Host "[Debug] Creating coverage report directory"

if (!(Test-Path CoverageReport)) {
    New-Item CoverageReport -type directory
}

Write-Host "[Debug] Executing OpenCover with nUnit"
& $openCoverPath -target:$nUnitPath -targetargs:"$nUnitArgs" -register:user -output:CoverageReport/coverage.xml -skipautoprops "-filter:$coverageFilters"

Write-Host "[Debug] Generating coverage report"
& $reportGeneratorPath -reports:CoverageReport/coverage.xml -targetdir:CoverageReport

Write-Host "[Debug] Opening report"
Start-Process .\CoverageReport\index.htm

Write-Host "[Debug] Done."