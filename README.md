# Web searcher

The searcher should run on any system supported by .NET Core: Windows, Ubuntu, Mac but is tested only on Windows.
1. .NET Core Framework 2.2 must be installed
2. Empty SQLite database named 'sqlite.db' with created tables must be provided in the program's working directory (available in 'sites' directory)
3. Document corpus must be available on path "../../../sites" relative to the program's working directory
4. There are three operating modes used with following arguments:
	- Indexing: i
	- Querying with index: q Query Words
	- Querying without index: d Query Words
5. Run the project WebSearcher.csproj:
	- with Visual Studio
	- or with .NET CLI "dotnet run WebSearcher.csproj"

