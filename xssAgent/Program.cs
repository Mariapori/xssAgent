using System.Reflection;

var tyyppi = args[0] ?? throw new NotImplementedException();
Assembly asm = Assembly.GetAssembly(Type.GetType(tyyppi));

var controlleractionlist = asm?.GetTypes()
        .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
        .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
        .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
        .Select(x => new { Controller = x.DeclaringType?.Name, Action = x.Name, ReturnType = x.ReturnType.Name, Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))) })
        .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();

if(controlleractionlist?.Count > 0)
{
    foreach (var item in controlleractionlist)
    {
        Console.WriteLine($"Testing XSS for {item.Controller}/{item.Action}");
    }
}