using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FakeAPI.Api;

[ModelBinder(BinderType = typeof(AppNameBinder))]
public class AppName : IdString
{
    public AppName(string name) : base(name) { }
}

public class AppNameBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var modelName = bindingContext.ModelName;
        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

        var value = valueProviderResult.FirstValue;

        if (string.IsNullOrEmpty(value))
        {
            return Task.CompletedTask;
        }

        var model = new AppName(value);
        bindingContext.Result = ModelBindingResult.Success(model);
        return Task.CompletedTask;
    }
}