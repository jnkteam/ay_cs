namespace OriginalStudio.WebComponents
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Text;

    public class VerifyImage
    {
        private BackgroundNoiseLevel _backgroundNoise = BackgroundNoiseLevel.Low;
        private string _fontFamilyName = "";
        private FontWarpFactor _fontWarp = FontWarpFactor.Low;
        private string _fontWhitelist = "arial;arial black;comic sans ms;courier new;estrangelo edessa;franklin gothic medium;georgia;lucida console;lucida sans unicode;mangal;microsoft sans serif;palatino linotype;sylfaen;tahoma;times new roman;trebuchet ms;verdana";
        private DateTime _generatedAt;
        private string _guid;
        private int _height = 50;
        private LineNoiseLevel _lineNoise = LineNoiseLevel.None;
        private Random _rand = new Random();
        private Color[] _randomcolor;
        private string _randomText;
        private string _randomTextChars = "ACDEFGHJKLNPQRTUVXYZ2346789";
        private int _randomTextLength = 5;
        private int _width = 180;
        private static string[] static_RandomFontFamily_ff;

        public VerifyImage()
        {
            this._randomText = this.GenerateRandomText();
            this._generatedAt = DateTime.Now;
            this._randomcolor = new Color[] { Color.Red, Color.Green, Color.Blue, Color.Black, Color.Purple, Color.Orange, Color.BlueViolet };
            this._guid = Guid.NewGuid().ToString();
        }

        private void AddLine(Graphics graphics1, Rectangle rect)
        {
            int num = 0;
            float width = 0f;
            int num3 = 0;
            switch (this._lineNoise)
            {
                case LineNoiseLevel.None:
                    return;

                case LineNoiseLevel.Low:
                    num = 4;
                    width = Convert.ToSingle((double) (((double) this._height) / 31.25));
                    num3 = 1;
                    break;

                case LineNoiseLevel.Medium:
                    num = 5;
                    width = Convert.ToSingle((double) (((double) this._height) / 27.7777));
                    num3 = 1;
                    break;

                case LineNoiseLevel.High:
                    num = 3;
                    width = Convert.ToSingle((int) (this._height / 0x19));
                    num3 = 2;
                    break;

                case LineNoiseLevel.Extreme:
                    num = 3;
                    width = Convert.ToSingle((double) (((double) this._height) / 22.7272));
                    num3 = 3;
                    break;
            }
            PointF[] points = new PointF[num + 1];
            Pen pen = new Pen(Color.DarkBlue, width);
            for (int i = 1; i <= num3; i++)
            {
                for (int j = 0; j <= num; j++)
                {
                    points[j] = this.RandomPoint(rect);
                }
                graphics1.DrawCurve(pen, points, 1.75f);
            }
            pen.Dispose();
        }

        private void AddNoise(Graphics graphics1, Rectangle rect)
        {
            int num = 0;
            int num2 = 0;
            switch (this._backgroundNoise)
            {
                case BackgroundNoiseLevel.None:
                    return;

                case BackgroundNoiseLevel.Low:
                    num = 30;
                    num2 = 40;
                    break;

                case BackgroundNoiseLevel.Medium:
                    num = 0x12;
                    num2 = 40;
                    break;

                case BackgroundNoiseLevel.High:
                    num = 0x10;
                    num2 = 0x27;
                    break;

                case BackgroundNoiseLevel.Extreme:
                    num = 12;
                    num2 = 0x26;
                    break;
            }
            SolidBrush brush = new SolidBrush(Color.DarkBlue);
            int maxValue = Convert.ToInt32((int) (Math.Max(rect.Width, rect.Height) / num2));
            for (int i = 0; i <= Convert.ToInt32((int) ((rect.Width * rect.Height) / num)); i++)
            {
                graphics1.FillEllipse(brush, this._rand.Next(rect.Width), this._rand.Next(rect.Height), this._rand.Next(maxValue), this._rand.Next(maxValue));
            }
            brush.Dispose();
        }

        private Bitmap GenerateImagePrivate()
        {
            System.Drawing.Font f = null;
            Bitmap image = new Bitmap(this._width, this._height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(image);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, this._width, this._height);
            Brush brush = new SolidBrush(Color.White);
            graphics.FillRectangle(brush, rect);
            int num = 0;
            double num2 = this._width / this._randomTextLength;
            foreach (char ch in this._randomText)
            {
                f = this.GetFont();
                Rectangle r = new Rectangle(Convert.ToInt32((double) (num * num2)), 0, Convert.ToInt32(num2), this._height);
                GraphicsPath textPath = this.TextPath(ch.ToString(), f, r);
                this.WarpText(textPath, r);
                brush = new SolidBrush(this.GetRandomColor());
                graphics.FillPath(brush, textPath);
                num++;
            }
            this.AddNoise(graphics, rect);
            this.AddLine(graphics, rect);
            f.Dispose();
            brush.Dispose();
            graphics.Dispose();
            return image;
        }

        public string GenerateRandomText()
        {
            StringBuilder builder = new StringBuilder(this._randomTextLength);
            int length = this._randomTextChars.Length;
            for (int i = 0; i <= (this._randomTextLength - 1); i++)
            {
                builder.Append(this._randomTextChars.Substring(this._rand.Next(length), 1));
            }
            return builder.ToString();
        }

        private System.Drawing.Font GetFont()
        {
            float emSize = 0f;
            string familyName = this._fontFamilyName;
            if (familyName == "")
            {
                familyName = this.RandomFontFamily();
            }
            switch (this.FontWarp)
            {
                case FontWarpFactor.None:
                    emSize = Convert.ToInt32((double) (this._height * 0.7));
                    break;

                case FontWarpFactor.Low:
                    emSize = Convert.ToInt32((double) (this._height * 0.8));
                    break;

                case FontWarpFactor.Medium:
                    emSize = Convert.ToInt32((double) (this._height * 0.85));
                    break;

                case FontWarpFactor.High:
                    emSize = Convert.ToInt32((double) (this._height * 0.9));
                    break;

                case FontWarpFactor.Extreme:
                    emSize = Convert.ToInt32((double) (this._height * 0.95));
                    break;
            }
            return new System.Drawing.Font(familyName, emSize, FontStyle.Bold);
        }

        private Color GetRandomColor()
        {
            return this.RandomColor[this._rand.Next(0, this.RandomColor.Length)];
        }

        private string RandomFontFamily()
        {
            if (static_RandomFontFamily_ff == null)
            {
                static_RandomFontFamily_ff = this._fontWhitelist.Split(new char[] { ';' });
            }
            return static_RandomFontFamily_ff[this._rand.Next(0, static_RandomFontFamily_ff.Length)];
        }

        private PointF RandomPoint(Rectangle rect)
        {
            return this.RandomPoint(rect.Left, rect.Width, rect.Top, rect.Bottom);
        }

        private PointF RandomPoint(int xmin, int xmax, int ymin, int ymax)
        {
            return new PointF((float) this._rand.Next(xmin, xmax), (float) this._rand.Next(ymin, ymax));
        }

        public Bitmap RenderImage()
        {
            return this.GenerateImagePrivate();
        }

        private GraphicsPath TextPath(string s, System.Drawing.Font f, Rectangle r)
        {
            StringFormat format2 = new StringFormat();
            format2.Alignment = StringAlignment.Near;
            format2.LineAlignment = StringAlignment.Near;
            StringFormat format = format2;
            GraphicsPath path = new GraphicsPath();
            path.AddString(s, f.FontFamily, (int) f.Style, f.Size, r, format);
            return path;
        }

        private void WarpText(GraphicsPath textPath, Rectangle rect)
        {
            float num = 0f;
            float num2 = 0f;
            switch (this._fontWarp)
            {
                case FontWarpFactor.None:
                    return;

                case FontWarpFactor.Low:
                    num = 6f;
                    num2 = 1f;
                    break;

                case FontWarpFactor.Medium:
                    num = 5f;
                    num2 = 1.3f;
                    break;

                case FontWarpFactor.High:
                    num = 4.5f;
                    num2 = 1.4f;
                    break;

                case FontWarpFactor.Extreme:
                    num = 4f;
                    num2 = 1.5f;
                    break;
            }
            RectangleF srcRect = new RectangleF(Convert.ToSingle(rect.Left), 0f, Convert.ToSingle(rect.Width), (float) rect.Height);
            int num3 = Convert.ToInt32((float) (((float) rect.Height) / num));
            int num4 = Convert.ToInt32((float) (((float) rect.Width) / num));
            int xmin = rect.Left - Convert.ToInt32((float) (num4 * num2));
            int ymin = rect.Top - Convert.ToInt32((float) (num3 * num2));
            int xmax = (rect.Left + rect.Width) + Convert.ToInt32((float) (num4 * num2));
            int ymax = (rect.Top + rect.Height) + Convert.ToInt32((float) (num3 * num2));
            if (xmin < 0)
            {
                xmin = 0;
            }
            if (ymin < 0)
            {
                ymin = 0;
            }
            if (xmax > this.Width)
            {
                xmax = this.Width;
            }
            if (ymax > this.Height)
            {
                ymax = this.Height;
            }
            PointF[] destPoints = new PointF[] { this.RandomPoint(xmin, xmin + num4, ymin, ymin + num3), this.RandomPoint(xmax - num4, xmax, ymin, ymin + num3), this.RandomPoint(xmin, xmin + num4, ymax - num3, ymax), this.RandomPoint(xmax - num4, xmax, ymax - num3, ymax) };
            Matrix matrix = new Matrix();
            matrix.Translate(0f, 0f);
            textPath.Warp(destPoints, srcRect, matrix, WarpMode.Perspective, 0f);
        }

        public BackgroundNoiseLevel BackgroundNoise
        {
            get
            {
                return this._backgroundNoise;
            }
            set
            {
                this._backgroundNoise = value;
            }
        }

        public string Font
        {
            get
            {
                return this._fontFamilyName;
            }
            set
            {
                try
                {
                    System.Drawing.Font font = new System.Drawing.Font(value, 12f);
                    this._fontFamilyName = value;
                    font.Dispose();
                }
                catch (Exception)
                {
                    this._fontFamilyName = FontFamily.GenericSerif.Name;
                }
            }
        }

        public FontWarpFactor FontWarp
        {
            get
            {
                return this._fontWarp;
            }
            set
            {
                this._fontWarp = value;
            }
        }

        public string FontWhitelist
        {
            get
            {
                return this._fontWhitelist;
            }
            set
            {
                this._fontWhitelist = value;
            }
        }

        public int Height
        {
            get
            {
                return this._height;
            }
            set
            {
                this._height = value;
            }
        }

        public LineNoiseLevel LineNoise
        {
            get
            {
                return this._lineNoise;
            }
            set
            {
                this._lineNoise = value;
            }
        }

        public Color[] RandomColor
        {
            get
            {
                return this._randomcolor;
            }
            set
            {
                this._randomcolor = value;
            }
        }

        public DateTime RenderedAt
        {
            get
            {
                return this._generatedAt;
            }
        }

        public string Text
        {
            get
            {
                return this._randomText;
            }
        }

        public string TextChars
        {
            get
            {
                return this._randomTextChars;
            }
            set
            {
                this._randomTextChars = value;
                this._randomText = this.GenerateRandomText();
            }
        }

        public int TextLength
        {
            get
            {
                return this._randomTextLength;
            }
            set
            {
                this._randomTextLength = value;
                this._randomText = this.GenerateRandomText();
            }
        }

        public string UniqueId
        {
            get
            {
                return this._guid;
            }
        }

        public int Width
        {
            get
            {
                return this._width;
            }
            set
            {
                this._width = value;
            }
        }

        public enum BackgroundNoiseLevel
        {
            None,
            Low,
            Medium,
            High,
            Extreme
        }

        public enum FontWarpFactor
        {
            None,
            Low,
            Medium,
            High,
            Extreme
        }

        public enum LineNoiseLevel
        {
            None,
            Low,
            Medium,
            High,
            Extreme
        }
    }
}

