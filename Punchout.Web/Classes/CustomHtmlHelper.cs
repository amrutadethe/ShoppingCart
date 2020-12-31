using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Punchout.Web
{
    public static class CustomHtmlHelper
    {
        /// <summary>
        /// Html Image Helper
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="id"></param>
        /// <param name="src"></param>
        /// <param name="alt"></param>
        /// <returns></returns>
        public static MvcHtmlString Image(this HtmlHelper helper, string id, string src,string alt)
        {
            // Below line is used for generate new tag with img  
            var builder = new TagBuilder("img");

            // Below five lines are used for adding atrribute for img tag  
            builder.MergeAttribute("id", id);

            builder.MergeAttribute("src", src);

            builder.MergeAttribute("alt", alt);

            //builder.MergeAttribute("height", height);

            //builder.MergeAttribute("width", width);

            // this method will return MVChtmlstring and Create method will render as selfclosing tag.  

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }

    }
}