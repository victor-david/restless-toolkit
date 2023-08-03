using System.Windows;
using System.Windows.Markup;
/* ThemeInfo(1,2)
* 1. where theme specific resource dictionaries are located (used if a resource is not found in the page, or application resource dictionaries)
* 2. where the generic resource dictionary is located (used if a resource is not found in the page, app, or any theme specific resource dictionaries)
*/
[assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)]

[assembly: XmlnsPrefix("http://schemas.restless.toolkit.com/2021", "toolkit")]
[assembly: XmlnsDefinition("http://schemas.restless.toolkit.com/2021", "Restless.Toolkit.Controls")]
[assembly: XmlnsDefinition("http://schemas.restless.toolkit.com/2021", "Restless.Toolkit.Converters")]
[assembly: XmlnsDefinition("http://schemas.restless.toolkit.com/2021", "Restless.Toolkit.Core")]
[assembly: XmlnsDefinition("http://schemas.restless.toolkit.com/2021", "Restless.Toolkit.Mvvm")]
[assembly: XmlnsDefinition("http://schemas.restless.toolkit.com/2021", "Restless.Toolkit.Resource")]
[assembly: XmlnsDefinition("http://schemas.restless.toolkit.com/2021", "Restless.Toolkit.Utility")]