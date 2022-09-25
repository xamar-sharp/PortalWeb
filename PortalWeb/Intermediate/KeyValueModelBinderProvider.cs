using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
namespace PortalWeb.Intermediate
{
    public sealed class KeyValueModelBinderProvider:IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext ctx)
        {
            if(ctx.Metadata.ModelType != typeof(KeyValuePair<string, string>))
            {
                return new SimpleTypeModelBinder(ctx.Metadata.ModelType, new LoggerFactory());
            }
            return new KeyValueModelBinder();
        }
    }
}
