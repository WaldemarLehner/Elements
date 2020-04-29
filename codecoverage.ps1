# HOW TO USE

# Add "coverlet.collector" through Nuget to the Test Project
# Add "JunitXml.TestLogger" version 2.1.15 through Nuget to the Test Project
# Add "Microsoft.NET.Test.Sdk" to Test Project
# Change relative path of $testProjectDll to your test dll
$testProjectDll = ".\ComputergrafikSpiel.Test\bin\Debug\ComputergrafikSpiel.Test.dll"

dotnet --version | Write-Output 
"Adding ReportGenerator Tool to .\reportgenerator"

dotnet tool install dotnet-reportgenerator-globaltool --tool-path reportgenerator --verbosity minimal

# Get Nuget Packages if neccesary
dotnet restore

# Remove old build files and rebuild
dotnet clean
dotnet build

# Go to reportgenerator directory, delete old output, create new one
Remove-Item .\reportgenerator\output -Recurse -ErrorAction Ignore
Remove-Item .\reportgenerator\report -Recurse -ErrorAction Ignore
# Reroute to $null to supress dir output
mkdir .\reportgenerator\output      > $null
mkdir .\reportgenerator\output\html > $null
mkdir .\reportgenerator\output\text > $null
mkdir .\reportgenerator\report      > $null

"Starting dotnet test" 
# this is %HOMEPATH%, It is needed to access XPlat(cobertura) in the .nuget directory
$userDir = Get-Variable HOME -ValueOnly
# run vstest, log result using junit format, collect data using XPlat (cobertura) and export results to "report" 
dotnet vstest $testProjectDll --ResultsDirectory:".\reportgenerator\report"  --logger:"junit;LogFileName=.\reportgenerator\report\TestResult.xml"  --collect:"XPlat Code Coverage" /testadapterpath:"$userDir\.nuget\packages\coverlet.collector\1.2.0\build\netstandard1.0"

"Creating Reports" 
.\reportgenerator\reportgenerator.exe "-reports:.\reportgenerator\report\*\coverage.cobertura.xml" "-targetdir:.\reportgenerator\output/text" -reportTypes:TextSummary
.\reportgenerator\reportgenerator.exe "-reports:.\reportgenerator\report\*\coverage.cobertura.xml" "-targetdir:.\reportgenerator\output/html" -reportTypes:Html

# Open the Index Page of the HTML Summary
.\reportgenerator\output\html\index.htm
Write-Output "`n`n`n`n"

# Print the Text Summary to Console
"########### Summary ###########"
Get-Content ".\reportgenerator\output\text\Summary.txt"