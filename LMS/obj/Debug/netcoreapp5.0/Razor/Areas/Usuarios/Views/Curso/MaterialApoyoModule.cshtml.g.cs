#pragma checksum "C:\Users\Oddinx\source\repos\LMS\LMS\Areas\Usuarios\Views\Curso\MaterialApoyoModule.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ee9262f81e6c0423bdae6d65fd8b208be77c73ce"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Usuarios_Views_Curso_MaterialApoyoModule), @"mvc.1.0.view", @"/Areas/Usuarios/Views/Curso/MaterialApoyoModule.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\Oddinx\source\repos\LMS\LMS\Areas\Usuarios\Views\_ViewImports.cshtml"
using LMS;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Oddinx\source\repos\LMS\LMS\Areas\Usuarios\Views\_ViewImports.cshtml"
using LMS.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Oddinx\source\repos\LMS\LMS\Areas\Usuarios\Views\Curso\MaterialApoyoModule.cshtml"
using LMS.Utility;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ee9262f81e6c0423bdae6d65fd8b208be77c73ce", @"/Areas/Usuarios/Views/Curso/MaterialApoyoModule.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a44cc5940732acd06f2b9ec4b09605652596f9b5", @"/Areas/Usuarios/Views/_ViewImports.cshtml")]
    public class Areas_Usuarios_Views_Curso_MaterialApoyoModule : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<LMS.Models.ViewModels.CursoViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("image3"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("data-src", new global::Microsoft.AspNetCore.Html.HtmlString("holder.js/100px225?theme=thumb&amp;bg=55595c&amp;fg=eceeef&amp;text=Thumbnail"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("alt", new global::Microsoft.AspNetCore.Html.HtmlString("Thumbnail [100%x225]"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/images/Folderlogo.png"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("data-holder-rendered", new global::Microsoft.AspNetCore.Html.HtmlString("true"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
            WriteLiteral("\r\n");
#nullable restore
#line 5 "C:\Users\Oddinx\source\repos\LMS\LMS\Areas\Usuarios\Views\Curso\MaterialApoyoModule.cshtml"
  
    ViewData["Title"] = "Material Apoyo";
    //Layout = "~/Views/Shared/_VistaCursoModelo.cshtml";

    Layout = "~/Views/Shared/_Layout2.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n\r\n\r\n\r\n<div class=\"album py-5 bg-light\">\r\n    <div class=\"container\">\r\n\r\n        <div class=\"row\">\r\n\r\n\r\n");
#nullable restore
#line 22 "C:\Users\Oddinx\source\repos\LMS\LMS\Areas\Usuarios\Views\Curso\MaterialApoyoModule.cshtml"
             if (Model.TareasList.Count() > 0)
            {
                foreach (var item in Model.TareasList)
                {




#line default
#line hidden
#nullable disable
            WriteLiteral("                    <div class=\"col-md-4\">\r\n\r\n\r\n                        <div class=\"Container3\">\r\n\r\n                            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagOnly, "ee9262f81e6c0423bdae6d65fd8b208be77c73ce6298", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n\r\n                            <div class=\"overlay3\">\r\n\r\n                                <a");
            BeginWriteAttribute("href", " href=\"", 895, "\"", 961, 1);
#nullable restore
#line 39 "C:\Users\Oddinx\source\repos\LMS\LMS\Areas\Usuarios\Views\Curso\MaterialApoyoModule.cshtml"
WriteAttributeValue("", 902, Url.Action("MaterialApoyo", "Curso", new { Id = item.Id }), 902, 59, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">      <div class=\"text3\">");
#nullable restore
#line 39 "C:\Users\Oddinx\source\repos\LMS\LMS\Areas\Usuarios\Views\Curso\MaterialApoyoModule.cshtml"
                                                                                                                          Write(Html.DisplayFor(m => item.NombreTarea));

#line default
#line hidden
#nullable disable
            WriteLiteral(" </div> </a>\r\n                            </div>\r\n\r\n                            <h4 class=\"title2\" align=\"center\">  ");
#nullable restore
#line 42 "C:\Users\Oddinx\source\repos\LMS\LMS\Areas\Usuarios\Views\Curso\MaterialApoyoModule.cshtml"
                                                           Write(Html.DisplayFor(m => item.NombreTarea));

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\r\n\r\n                        </div>\r\n\r\n                    </div>\r\n");
#nullable restore
#line 47 "C:\Users\Oddinx\source\repos\LMS\LMS\Areas\Usuarios\Views\Curso\MaterialApoyoModule.cshtml"



                }
            }

            else
            {


#line default
#line hidden
#nullable disable
            WriteLiteral("                <div class=\"text-center\">\r\n                    <h1 class=\"display-4\">No hay ningun <br /> material de apoyo disponible</h1>\r\n\r\n                </div>\r\n");
#nullable restore
#line 60 "C:\Users\Oddinx\source\repos\LMS\LMS\Areas\Usuarios\Views\Curso\MaterialApoyoModule.cshtml"

            }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n        </div>\r\n    </div>\r\n</div>\r\n\r\n\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<LMS.Models.ViewModels.CursoViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
