using System.Drawing;
using System.Windows.Forms;

namespace ASA62
{
    public class myButton : Button
    {
        public myButton()
        {
            InitializeComponent();
        }

        public myButton(Size size, string name)
        {
            InitializeComponent();
            MinimumSize = size;
            MaximumSize = size;
            Text = name;
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // myButton
            // 
            FlatAppearance.BorderColor = Color.White;
            FlatAppearance.MouseDownBackColor = Color.Black;
            FlatAppearance.MouseOverBackColor = Color.Black;
            FlatStyle = FlatStyle.Flat;
            Font = new Font("Microsoft Sans Serif", 6.75F, FontStyle.Bold);
            ForeColor = Color.White;
            MaximumSize = new Size(50, 28);
            MinimumSize = new Size(50, 28);
            Size = new Size(50, 28);
            ResumeLayout(false);
        }
    }
}