using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

public class ViewRenderService : IViewRenderService
{
    private readonly IRazorViewEngine _viewEngine;
    private readonly ITempDataProvider _tempDataProvider;
    private readonly IActionContextAccessor _actionContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ViewRenderService(
        IRazorViewEngine viewEngine,
        ITempDataProvider tempDataProvider,
        IActionContextAccessor actionContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        _viewEngine = viewEngine;
        _tempDataProvider = tempDataProvider;
        _actionContextAccessor = actionContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<string> RenderViewToStringAsync(string viewName, object model)
    {
        var actionContext = new ActionContext
        {
            HttpContext = _actionContextAccessor.ActionContext.HttpContext,
            RouteData = _actionContextAccessor.ActionContext.RouteData,
            ActionDescriptor = _actionContextAccessor.ActionContext.ActionDescriptor
        };

        using (var sw = new StringWriter())
        {
            var viewResult = _viewEngine.FindView(actionContext, viewName, false);

            if (viewResult.View == null)
            {
                throw new ArgumentNullException($"{viewName} does not match any available view");
            }

            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                },
                new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                sw,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);

            return sw.ToString();
        }
    }
}
