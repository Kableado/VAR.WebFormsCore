VAR.WebFormsCore
================
[![MIT License](https://img.shields.io/github/license/dotnet/aspnetcore?color=%230b0&style=flat-square)](https://github.com/Kableado/VAR.WebFormsCore/blob/Main/LICENSE.txt)

VAR.WebFormsCore is a web framework based lightly on ASP.Net WebForms. 

## Features
* [x] Components
  * [x] Control
  * [x] HtmlGenericControl, Panel
  * [x] Button
  * [x] Label and LiteralControl
  * [x] HiddenField
  * [x] TextBox and CTextBox
  * [x] HtmlHead, HtmlMeta, HtmlBody and Page
  * [ ] Checkbox and Radiobutton
  * [ ] DataGrid
  * [ ] FileUpload
* [x] Pages
  * [x] Common page
  * [x] Error page
  * [ ] Login page
* [x] Interface 
  * [x] AspnetCore

## Usage
Currently, there are two libraries:
* `VAR.WebFormsCore`: Core implementation of WebFormsCore
* `VAR.WebFormsCore.AspnetCore`: AspnetCore interface

And one test web-application:
* `VAR.WebFormsCore.TestWebApp`: A simple example web-application using WebFormsCore.

## Architecture
VAR.WebFormsCore is designed to be host-agnostic. It uses `IWebContext` to abstract away the underlying HTTP framework.

### Execution Pipeline
1. **Middleware/Adapter**: Receives the HTTP request (e.g., `GlobalRouterMiddleware` in AspNetCore).
2. **Context**: Wraps the request in `IWebContext`.
3. **Router**: `GlobalRouter` finds the appropriate `IHttpHandler` (usually a `Page`).
4. **Handler**: `ProcessRequest` is called to generate the output.

### Asset Bundling
The framework automatically bundles CSS and JS files from both embedded resources in the core library and physical files in the application's `Styles/` and `Scripts/` folders using `StylesBundler` and `ScriptsBundler`.

## Building
An SLNX solution is provided, for usage on Visual Studio, Rider or any editor with Omnisharp.

## Contributing
1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request :D

## Credits
* Valeriano Alfonso Rodriguez.

