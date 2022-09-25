using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
namespace PortalWeb.Intermediate
{
    public sealed class KeyValueModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext ctx)
        {
            var name = ctx.ValueProvider.GetValue("Name");
            var value = ctx.ValueProvider.GetValue("Value");
            if(name == ValueProviderResult.None || value == ValueProviderResult.None)
            {
                ctx.Result = ModelBindingResult.Failed();
            }
            KeyValuePair<string, string> pair = new KeyValuePair<string, string>(name.FirstValue, value.FirstValue);
            ctx.Result = ModelBindingResult.Success(pair);
            await Task.Yield();
        }
    }
}
