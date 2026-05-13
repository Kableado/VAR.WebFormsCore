# Development Information for Agents

This document provides essential information for developers and automated agents working on the `VAR.WebFormsCore` project.

## 1. Build and Configuration

### Prerequisites
- .NET 10.0 SDK or higher.
- The project targets `netstandard2.0` (core library) and `net9.0` (AspNetCore integration and tests).

### Building the Project
To build the entire solution, run the following command from the root directory:
```bash
dotnet build VAR.WebFormsCore.slnx
```

### Project Structure
- `VAR.WebFormsCore`: The core library containing WebForms-like controls and logic.
- `VAR.WebFormsCore.AspNetCore`: Integration layer for ASP.NET Core.
- `VAR.WebFormsCore.Tests`: Unit tests for the core library.
- `VAR.WebFormsCore.TestWebApp`: A sample web application for manual testing and demonstration.

## 2. Testing Information

### Running Tests
Tests are implemented using **xUnit**. To run all tests in the solution:
```bash
dotnet test VAR.WebFormsCore.Tests/VAR.WebFormsCore.Tests.csproj
```

### Adding New Tests
1. Create a new test class in the `VAR.WebFormsCore.Tests` project (typically under `Code/`, `Controls/`, or `Pages/`).
2. Use the `[Fact]` attribute for simple tests or `[Theory]` for data-driven tests.
3. Follow the naming convention: `MethodName__Scenario__ExpectedResult`.

### Simple Test Example
Below is a basic example of a unit test for this project:

```csharp
using Xunit;
using VAR.WebFormsCore.Code;

namespace VAR.WebFormsCore.Tests.Code;

public class DemoTest
{
    [Fact]
    public void DemoTest__SimpleAssertion__Success()
    {
        // Arrange
        var value = 1;

        // Act
        var result = value + 1;

        // Assert
        Assert.Equal(2, result);
    }
}
```

## 3. Additional Development Information

### Code Style
- **Naming Conventions**: Use `PascalCase` for classes and methods. Use `_camelCase` for private fields.
- **File Scoped Namespaces**: Use file-scoped namespaces (e.g., `namespace VAR.WebFormsCore.Tests.Code;`) to reduce indentation.
- **Regions**: The codebase sometimes uses `#region` to group related tests or methods.
- **Control Development**: When creating new controls, ensure they inherit from appropriate base classes in `VAR.WebFormsCore.Controls`.
- **Embedded Resources**: JavaScript and CSS files in `VAR.WebFormsCore` are often marked as `EmbeddedResource` to be served by the library.

### Debugging
- You can use `VAR.WebFormsCore.TestWebApp` to debug the library's behavior in a real ASP.NET Core environment.
- Use `Console.WriteLine` or standard logging for debugging during test execution.

## 4. Architecture and Pipeline

### Execution Pipeline
The framework uses a decoupled architecture to handle HTTP requests:
1. **Host Integration**: An adapter (like `VAR.WebFormsCore.AspNetCore`) receives the native HTTP request.
2. **Context Abstraction**: The native request is wrapped in an `IWebContext` implementation (e.g., `AspnetCoreWebContext`).
3. **Routing**: The `GlobalRouter.RouteRequest(IWebContext)` method determines which handler should process the request based on the URL path.
4. **Handler Execution**: An `IHttpHandler` (usually a `Page`) is instantiated and its `ProcessRequest(IWebContext)` method is called to generate the response.

### Key Interfaces
- **`IWebContext`**: Abstraction of the HTTP context (Request, Response, Cookies, Headers). Implementing this allows the core library to run on any HTTP framework (ASP.NET Core, Katana, or even a custom socket-based server).
- **`IHttpHandler`**: Interface for objects that process requests. Both `Page` and resource bundlers implement this.
- **`IGlobalConfig`**: Central configuration for the application (Title, Author, Default Handler, Authentication logic).

### Asset Bundling (`StylesBundler` and `ScriptsBundler`)
The framework includes built-in handlers for CSS and JS bundling:
- **Directories Convention**: By default, they look for files in the `Styles/` and `Scripts/` directories respectively.
- **Internal (Embedded) Resources**: They first bundle files marked as `EmbeddedResource` in the `VAR.WebFormsCore` assembly (located in the `Styles/` and `Scripts/` folders of the core project).
- **External Files**: They then append any files found in the physical `Styles/` and `Scripts/` directories of the running application's content root.
- **Usage**: `PageCommon` automatically includes links to these bundlers in the generated HTML.
