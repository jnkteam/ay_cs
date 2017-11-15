namespace KuaiCard.WebComponents
{
    using System;
    using System.IO;
    using System.Web.UI.WebControls;

    public class FileUploadHelper
    {
        public static bool IsAllowedExtension(FileUpload hifile)
        {
            if (!hifile.HasFile)
            {
                return false;
            }
            FileStream input = new FileStream(hifile.PostedFile.FileName, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(input);
            string str = "";
            try
            {
                str = reader.ReadByte().ToString();
                str = str + reader.ReadByte().ToString();
            }
            catch
            {
            }
            reader.Close();
            input.Close();
            return ((str == "255216") || (str == "7173"));
        }
    }
}

