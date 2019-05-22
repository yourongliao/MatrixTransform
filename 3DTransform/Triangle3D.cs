using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

using System.Drawing.Drawing2D;

namespace _3DTransform
{
    class Triangle3D
    {

        private Vector4 a, b, c;

        public Vector4 A, B, C;

        private float dot;

        private bool cullBack;

        public Triangle3D() { }

        public Triangle3D(Vector4 a,Vector4 b,Vector4 c)
        {
            this.A = this.a= a;

            this.B = this.b = b;

            this.C = this.c = c;
        }

        public void Transform(Matrix4x4 m)
        {
            this.a = m.Mul(this.A);

            this.b = m.Mul(this.B);

            this.c = m.Mul(this.C);
        }

        public void CalculateLighting(Matrix4x4 _Object2World,Vector4 L)
        {
            this.Transform(_Object2World);

            Vector4 U = this.b - this.a;

            Vector4 V = this.c - this.a;

            Vector4 normal= U.Cross(V);

            dot = normal.Normalized.Dot(L.Normalized);

            dot = Math.Max(0, dot);

            Vector4 E = new Vector4(0, 0, -1, 0);

            cullBack = normal.Normalized.Dot(E) < 0 ? true : false;
        }

        public void Draw(Graphics g,bool isLine)
        {
            //Pen pen = new Pen(Color.Red, 2);
            //g.TranslateTransform(300, 300);
            if (isLine)
                g.DrawLines(new Pen(Color.Red, 2), this.Get2DPointFArr());
            else
            {
                if (!cullBack)
                {

                    GraphicsPath path = new GraphicsPath();

                    path.AddLines(this.Get2DPointFArr());

                    int r = (int)(200 * dot) + 55;

                    Color color = Color.FromArgb(r, r, r);

                    Brush br = new SolidBrush(color);

                    g.FillPath(br, path);
                }

            }
            //g.DrawLines(pen,this.Get2DPointFArr());


        }

        private PointF Get2DPointF(Vector4 v)
        {
            PointF p = new PointF();

            p.X = (float)(v.x / v.w);

            p.Y = -(float)(v.y / v.w);

            return p;
        }

        private PointF[] Get2DPointFArr()
        {
            PointF[] arr = new PointF[4];

            arr[0] = Get2DPointF(this.a);

            arr[1] = Get2DPointF(this.b);

            arr[2] = Get2DPointF(this.c);

            arr[3] = arr[0];

            return arr;
        }

       
    }
}
