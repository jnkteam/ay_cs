using KuaiCard.BLL.User;
using KuaiCard.Model.User;
using KuaiCardLib.Web;
using System;
using System.Web.UI;

public class business_User_idimg : Page
{
    public usersIdImageInfo _ItemInfo = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if ((this.show == "on") && (this.ItemInfo != null))
        {
            base.Response.Clear();
            base.Response.ContentType = this.ItemInfo.ptype;
            base.Response.OutputStream.Write(this.ItemInfo.image_on, 0, this.ItemInfo.filesize.Value);
            base.Response.End();
        }
        if ((this.show == "down") && (this.ItemInfo != null))
        {
            base.Response.Clear();
            base.Response.ContentType = this.ItemInfo.ptype;
            base.Response.OutputStream.Write(this.ItemInfo.image_down, 0, this.ItemInfo.filesize1.Value);
            base.Response.End();
        }
    }

    public usersIdImageInfo ItemInfo
    {
        get
        {
            if (this._ItemInfo == null)
            {
                if (this.ItemInfoId > 0)
                {
                    this._ItemInfo = new usersIdImage().Get(this.ItemInfoId);
                }
                else
                {
                    this._ItemInfo = new usersIdImageInfo();
                }
            }
            return this._ItemInfo;
        }
    }

    public int ItemInfoId
    {
        get
        {
            return WebBase.GetQueryStringInt32("id", 0);
        }
    }

    public string show
    {
        get
        {
            return WebBase.GetQueryStringString("show", "");
        }
    }
}

