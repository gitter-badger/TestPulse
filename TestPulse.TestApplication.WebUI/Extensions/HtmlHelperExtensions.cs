using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;


/// <summary>
/// Refer to https://bitbucket.org/forloop/forloop-htmlhelpers
/// </summary>
// ReSharper disable CheckNamespace
public static class HtmlHelperExtensions
// ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Adds a block of script to be rendered out at a later point in the page rendering when
    ///     <see cref="ScriptHtmlHelperExtensions.RenderScripts(System.Web.Mvc.HtmlHelper)" /> is called.
    /// </summary>
    /// <param name="htmlHelper">the <see cref="HtmlHelper" /></param>
    /// <param name="scriptBlock">the block of script to render. The block must not include the &lt;script&gt; tags</param>
    /// <param name="renderOnAjax">
    ///     if set to <c>true</c> and the request is an AJAX request, the script will be written in the response.
    /// </param>
    /// <remarks>
    ///     A call to <see cref="RenderScripts(HtmlHelper)" /> will render all scripts.
    /// </remarks>
    public static void AddScriptBlock(this HtmlHelper htmlHelper, string scriptBlock, bool renderOnAjax = false)
    {
        AddToScriptContext(htmlHelper, context => context.AddScriptBlock(scriptBlock, renderOnAjax));
    }

    /// <summary>
    ///     Adds a block of script to be rendered out at a later point in the page rendering when
    ///     <see cref="RenderScripts(System.Web.Mvc.HtmlHelper)" /> is called.
    /// </summary>
    /// <param name="htmlHelper">the <see cref="HtmlHelper" /></param>
    /// <param name="scriptTemplate">
    ///     the template for the block of script to render. The template must include the &lt;script
    ///     &gt; tags
    /// </param>
    /// <param name="renderOnAjax">
    ///     if set to <c>true</c> and the request is an AJAX request, the script will be written in the response.
    /// </param>
    /// <remarks>
    ///     A call to <see cref="RenderScripts(HtmlHelper)" /> will render all scripts.
    /// </remarks>
    public static void AddScriptBlock(this HtmlHelper htmlHelper, Func<dynamic, HelperResult> scriptTemplate, bool renderOnAjax = false)
    {
        AddToScriptContext(htmlHelper, context => context.AddScriptBlock(scriptTemplate, renderOnAjax));
    }

    /// <summary>
    ///     Adds a script file to be rendered out at a later point in the page rendering when
    ///     <see cref="ScriptHtmlHelperExtensions.RenderScripts(System.Web.Mvc.HtmlHelper)" /> is called.
    /// </summary>
    /// <param name="htmlHelper">the <see cref="HtmlHelper" /></param>
    /// <param name="path">the path to the script file to render later</param>
    /// <param name="renderOnAjax">
    ///     if set to <c>true</c> and the request is an AJAX request, the script will be written in the response.
    /// </param>
    /// <remarks>
    ///     A call to <see cref="RenderScripts(HtmlHelper)" /> will render all scripts.
    /// </remarks>
    public static void AddScriptFile(this HtmlHelper htmlHelper, string path, bool renderOnAjax = false)
    {
        AddToScriptContext(htmlHelper, context => context.AddScriptFile(path, renderOnAjax));
    }

    /// <summary>
    ///     Begins a new <see cref="ScriptContext" />. Used to signal that the scripts inside the
    ///     context belong in the same view, partial view or template
    /// </summary>
    /// <param name="htmlHelper">
    ///     the <see cref="HtmlHelper" />
    /// </param>
    /// <returns>
    ///     a new instance of <see cref="ScriptContext" />
    /// </returns>
    public static ScriptContext BeginScriptContext(this HtmlHelper htmlHelper)
    {
        var context = htmlHelper.ViewContext.HttpContext;
        var scriptContext = new ScriptContext(context, htmlHelper.ViewContext.Writer);
        context.Items[ScriptContext.ScriptContextItem] = scriptContext;
        return scriptContext;
    }

    /// <summary>
    ///     Ends a <see cref="ScriptContext" />.
    /// </summary>
    /// <param name="htmlHelper">
    ///     the <see cref="HtmlHelper" />
    /// </param>
    public static void EndScriptContext(this HtmlHelper htmlHelper)
    {
        var context = htmlHelper.ViewContext.HttpContext;
        var scriptContext = context.Items[ScriptContext.ScriptContextItem] as ScriptContext;

        if (scriptContext != null)
        {
            scriptContext.Dispose();
        }
    }

    /// <summary>
    ///     Renders the scripts out into the view using <see cref="UrlHelper.Content" />
    ///     to generate the paths in the &lt;script&gt; elements for the script files
    /// </summary>
    /// <param name="htmlHelper">
    ///     the <see cref="HtmlHelper" />
    /// </param>
    /// <returns>
    ///     an <see cref="IHtmlString" /> of all of the scripts.
    /// </returns>
    public static IHtmlString RenderScripts(this HtmlHelper htmlHelper)
    {
        return RenderScripts(htmlHelper, ScriptContext.ScriptPathResolver);
    }

    /// <summary>
    ///     Renders the scripts out into the view using the passed <paramref name="scriptPathResolver" /> function
    ///     to generate the &lt;script&gt; elements for the script files.
    /// </summary>
    /// <param name="htmlHelper">
    ///     the <see cref="HtmlHelper" />
    /// </param>
    /// <param name="scriptPathResolver">
    ///     a function that is passed the script paths and is used to generate the markup for
    ///     the script elements
    /// </param>
    /// <returns>
    ///     an <see cref="IHtmlString" /> of all of the scripts.
    /// </returns>
    [Obsolete("Set the scriptPathResolver using the ScriptContext.ScriptPathResolver static property.", false)]
    public static IHtmlString RenderScripts(this HtmlHelper htmlHelper, Func<string[], IHtmlString> scriptPathResolver)
    {
        var scriptContexts =
            htmlHelper.ViewContext.HttpContext.Items[ScriptContext.ScriptContextItems] as Stack<ScriptContext>;

        if (scriptContexts != null)
        {
            var count = scriptContexts.Count;
            var builder = new StringBuilder(scriptContexts.Count);
            var script = new List<string>(scriptContexts.Count);

            for (int i = 0; i < count; i++)
            {
                var scriptContext = scriptContexts.Pop();

                builder.Append(scriptPathResolver(scriptContext._scriptFiles.ToArray()).ToString());
                script.InsertRange(0, scriptContext._scriptBlocks);

                // render out all the scripts in one block on the last loop iteration
                if (i == count - 1 && script.Any())
                {
                    foreach (var s in script)
                    {
                        builder.AppendLine(s);
                    }
                }
            }

            return new HtmlString(builder.ToString());
        }

        return MvcHtmlString.Empty;
    }

    /// <summary>
    ///     Performs an action on the current <see cref="ScriptContext" />
    /// </summary>
    /// <param name="htmlHelper">
    ///     the <see cref="HtmlHelper" />
    /// </param>
    /// <param name="action">the action to perform</param>
    private static void AddToScriptContext(HtmlHelper htmlHelper, Action<ScriptContext> action)
    {
        var scriptContext =
            htmlHelper.ViewContext.HttpContext.Items[ScriptContext.ScriptContextItem] as ScriptContext;

        if (scriptContext == null)
        {
            throw new InvalidOperationException(
                "No ScriptContext in HttpContext.Items. Call Html.BeginScriptContext() to create a ScriptContext.");
        }

        action(scriptContext);
    }
}

/// <summary>
/// A context in which to add references to script files and blocks of script
/// to be rendered to the view at a later point.
/// </summary>
public class ScriptContext : IDisposable
{
    internal const string ScriptContextItem = "ScriptContext";
    internal const string ScriptContextItems = "ScriptContexts";
    private static HttpContextBase _context;

    private static Func<string[], IHtmlString> _scriptPathResolver = paths =>
    {
        var builder = new StringBuilder(paths.Length);
        foreach (var path in paths)
        {
            builder.AppendLine(
                "<script type='text/javascript' src='" + UrlHelper.GenerateContentUrl(path, Context) + "'></script>");
        }

        return new HtmlString(builder.ToString());
    };

    private readonly HttpContextBase _httpContext;
    private readonly TextWriter _writer;
    private readonly bool _isAjaxRequest;
    internal readonly IList<string> _scriptBlocks = new List<string>();
    internal readonly HashSet<string> _scriptFiles = new HashSet<string>();

    /// <summary>
    ///     Initializes a new instance of the <see cref="ScriptContext" /> class.
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    /// <param name="writer"></param>
    /// <exception cref="System.ArgumentNullException">httpContext</exception>
    public ScriptContext(HttpContextBase httpContext, TextWriter writer)
    {
        if (httpContext == null)
        {
            throw new ArgumentNullException("httpContext");
        }

        if (writer == null)
        {
            throw new ArgumentNullException("writer");
        }

        _httpContext = httpContext;
        _writer = writer;
        _isAjaxRequest = _httpContext.Request.IsAjaxRequest();
    }

    /// <summary>
    /// Gets or sets the resolver used to resolve paths to script files.
    /// </summary>
    /// <value>
    /// The script path resolver.
    /// </value>
    public static Func<string[], IHtmlString> ScriptPathResolver
    {
        get { return _scriptPathResolver; }
        set { _scriptPathResolver = value; }
    }

    /// <summary>
    /// Gets or sets the context.
    /// </summary>
    /// <value>
    /// The context.
    /// </value>
    internal static HttpContextBase Context
    {
        get { return _context ?? new HttpContextWrapper(HttpContext.Current); }
        set { _context = value; }
    }

    /// <summary>
    ///     Adds a block of script to be rendered out at a later point in the page rendering when
    ///     <see cref="ScriptHtmlHelperExtensions.RenderScripts(System.Web.Mvc.HtmlHelper)" /> is called.
    /// </summary>
    /// <param name="scriptBlock">the block of script to render. The block must not include the &lt;script&gt; tags</param>
    /// <param name="renderOnAjax">
    ///     if set to <c>true</c> and the request is an AJAX request, the script will be written in the response.
    /// </param>
    /// <remarks>
    ///     A call to <see cref="ScriptHtmlHelperExtensions.RenderScripts(HtmlHelper)" /> will render all scripts.
    /// </remarks>
    public void AddScriptBlock(string scriptBlock, bool renderOnAjax = false)
    {
        if (_isAjaxRequest)
        {
            if (renderOnAjax)
            {
                _scriptBlocks.Add("<script type='text/javascript'>" + scriptBlock + "</script>");
            }
        }
        else
        {
            _scriptBlocks.Add("<script type='text/javascript'>" + scriptBlock + "</script>");
        }
    }

    /// <summary>
    ///     Adds a block of script to be rendered out at a later point in the page rendering when
    ///     <see cref="ScriptHtmlHelperExtensions.RenderScripts(System.Web.Mvc.HtmlHelper)" /> is called.
    /// </summary>
    /// <param name="scriptTemplate">
    ///     the template for the block of script to render. The template must include the &lt;script
    ///     &gt; tags
    /// </param>
    /// <param name="renderOnAjax">
    ///     if set to <c>true</c> and the request is an AJAX request, the script will be written in the response.
    /// </param>
    /// <remarks>
    ///     A call to <see cref="ScriptHtmlHelperExtensions.RenderScripts(HtmlHelper)" /> will render all scripts.
    /// </remarks>
    public void AddScriptBlock(Func<dynamic, HelperResult> scriptTemplate, bool renderOnAjax = false)
    {
        if (_isAjaxRequest)
        {
            if (renderOnAjax)
            {
                _scriptBlocks.Add(scriptTemplate(null).ToString());
            }
        }
        else
        {
            _scriptBlocks.Add(scriptTemplate(null).ToString());
        }
    }

    /// <summary>
    ///     Adds a script file to be rendered out at a later point in the page rendering when
    ///     <see cref="ScriptHtmlHelperExtensions.RenderScripts(System.Web.Mvc.HtmlHelper)" /> is called.
    /// </summary>
    /// <param name="path">the path to the script file to render later</param>
    /// <param name="renderOnAjax">
    ///     if set to <c>true</c> and the request is an AJAX request, the script will be written in the response.
    /// </param>
    /// <remarks>
    ///     A call to <see cref="ScriptHtmlHelperExtensions.RenderScripts(HtmlHelper)" /> will render all scripts.
    /// </remarks>
    public void AddScriptFile(string path, bool renderOnAjax = false)
    {
        if (_isAjaxRequest)
        {
            if (renderOnAjax)
            {
                _scriptFiles.Add(path);
            }
        }
        else
        {
            _scriptFiles.Add(path);
        }
    }

    /// <summary>
    ///     Pushes the <see cref="ScriptContext" /> onto the stack in <see cref="HttpContext.Items" />
    /// </summary>
    public void Dispose()
    {
        if (_isAjaxRequest)
        {
            var builder = new StringBuilder(_scriptFiles.Count + _scriptBlocks.Count);
            builder.Append(ScriptPathResolver(_scriptFiles.ToArray()));

            foreach (var scriptBlock in _scriptBlocks)
            {
                builder.AppendLine(scriptBlock);
            }

            _writer.Write(builder.ToString());
            return;
        }

        var items = _httpContext.Items;
        var scriptContexts = items[ScriptContextItems] as Stack<ScriptContext> ?? new Stack<ScriptContext>();

        // remove any script files already the same as the ones we're about to add
        foreach (var scriptContext in scriptContexts)
        {
            scriptContext._scriptFiles.ExceptWith(_scriptFiles);
        }

        scriptContexts.Push(this);
        items[ScriptContextItems] = scriptContexts;
    }
}