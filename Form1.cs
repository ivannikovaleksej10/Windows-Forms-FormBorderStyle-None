using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    // Наследуемся от FormShadow
    public partial class Form1 : FormShadow
    {
        public Form1()
        {
            InitializeComponent();

            // Плавное закрытие программы
            async void Exit() { for (Opacity = 1; Opacity > .0; Opacity -= .2) await Task.Delay(7); Close(); }
            ButtonClose.Click += (s, a) => Exit();

            // Красим форму
            FormPaint(Color.FromArgb(44, 57, 67), Color.FromArgb(35, 44, 55));

            // Позволяем таскать за заголовок Label и Panel
            new List<Control> { LabelHead, PanelHead }.ForEach(x =>
            {
                x.MouseDown += (s, a) =>
                {
                    x.Capture = false; Capture = false; Message m = Message.Create(Handle, 0xA1, new IntPtr(2), IntPtr.Zero); base.WndProc(ref m);
                };
            });
        }

        // Красим форму
        public void FormPaint(Color color1, Color color2)
        {
            void OnPaintEventHandler(object s, PaintEventArgs a)
            {
                if (ClientRectangle == Rectangle.Empty)
                    return;

                var lgb = new LinearGradientBrush(ClientRectangle, Color.Empty, Color.Empty, 90);
                var cblend = new ColorBlend { Colors = new[] { color1, color1, color2, color2 }, Positions = new[] { 0, 0.13f, 0.13f, 1 } };

                lgb.InterpolationColors = cblend;
                a.Graphics.FillRectangle(lgb, ClientRectangle);
            }

            Paint -= OnPaintEventHandler;
            Paint += OnPaintEventHandler;

            Invalidate();
        }

        async void Form1_Load(object sender, EventArgs e)
        {
            // Плавный запуск формы
            for (Opacity = 0; Opacity < 1; Opacity += .2) await Task.Delay(10);
        }
    }
}
