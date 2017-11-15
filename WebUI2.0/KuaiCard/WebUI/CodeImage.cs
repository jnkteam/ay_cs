namespace KuaiCard.WebUI
{
    using KuaiCard.WebComponents;
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    public class CodeImage : Page
    {
        protected HtmlForm form1;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void ProcessRequest(HttpContext context)
        {

            context.Response.Expires = 0;
            context.Response.CacheControl = "no-cache";
            VerifyImage image = new VerifyImage();
            image.Width = 70;
            image.Height = 0x16;
            image.TextLength = 4;
            VerifyImage image2 = image;
            context.Response.ClearContent();
            context.Response.ContentType = "image/jpeg";
            Bitmap bitmap = image2.RenderImage();
            context.Session["CCode"] = image2.Text;
            bitmap.Save(context.Response.OutputStream, ImageFormat.Jpeg);
            bitmap.Dispose();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

