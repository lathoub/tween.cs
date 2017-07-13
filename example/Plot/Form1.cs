using System;
using System.Collections;
using System.Windows.Forms;
using Tween;

namespace Plot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private double _time;

        private void Form1_Load(object sender, EventArgs e)
        {
            var position = new Hashtable { { "x", 0.0 } };

            var tween = new Tween.Tween(position)
                .To(new Hashtable { { "x", 100.0 }}, 2000)
                .Easing(Easing.Bounce.InOut);

            tween.Start();

            tween.Updated += (o, args) =>
            {
                var y = (double) args.Obj["x"];
                chart.Series["Series"].Points.AddXY(_time, y);
            };

            for (double i = 0; i < 2000; i = i + 10)
            {
                _time = i;
                tween.Update(_time);
            }
        }
    }
}
