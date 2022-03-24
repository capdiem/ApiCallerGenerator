# MasaApiCallerGenerator

Automatically generate the source code of Caller.

> Only web apis written by [MASA.Contrib.Service.MinimalAPIs](https://github.com/masastack/MASA.Contrib) is supported.

## Get Started

MasaApiCallerGenerator can be installed using the Nuget package manager or the dotnet CLI.

```shell
dotnet add package MasaApiCallerGenerator
```
 
Update csproj in your WebApi project:
```xml
<PropertyGroup>
    <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
    <!-- Change here: the path of caller project -->
    <CompilerGeneratedFilesOutputPath>$(SolutionDir)\MasaApiCaller</CompilerGeneratedFilesOutputPath>
</PropertyGroup>

<PropertyGroup>
    <!-- Change here: the name of caller: TestCaller here -->
    <MasaApiCaller_Name>Test</MasaApiCaller_Name>
    <!-- Change here: the applicationUrl -->
    <MasaApiCaller_BaseAddress>http://localhost:5177</MasaApiCaller_BaseAddress>
</PropertyGroup>

<ItemGroup>
    <CompilerVisibleProperty Include="MasaApiCaller_Name" />
    <CompilerVisibleProperty Include="MasaApiCaller_BaseAddress" />
</ItemGroup>

<ItemGroup Condition="'$(Configuration)' == 'Debug'">
    <PackageReference Include="Masa.Utils.Caller.HttpClient" Version="0.0.0" /> <!-- change version -->
    <ProjectReference Include="MasaApiCallerGenerator" Version="0.0.0" /> <!-- change version -->
</ItemGroup>
```

## TODOs
- [x] get, post, put, delete
- [x] ignore \[FromService\]
- [ ] ~~give priority to \[FromQuery\] and \[FromBody\]~~
- [ ] fine with BindAsync and TryParse?
- [ ] {id}
- [ ] masa.utils.caller.daprClient
