using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Priject2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the event when a file is selected in the open file dialog.
        /// </summary>
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            // Iterate through each file selected in the open file dialog
            foreach (var filename in openFileDialog_LoadTicker.FileNames)
            {
                // Create a new instance of Form_Display, passing the selected file, start date, and end date as parameters
                Form_Display f = new Form_Display(filename, dateTimePicker1.Value, dateTimePicker2.Value); // Create a variable of type Form_Display

                // The following line is commented out, but it would show the new Form_Display instance
                //f.Show(); // Show Form_Display f
            }
        }

        /// <summary>
        /// Handles the click event for the "Load" button to show the open file dialog.
        /// </summary>
        private void button_Load_Click(object sender, EventArgs e)
        {
            // Display the file dialog for the user to select files
            openFileDialog_LoadTicker.ShowDialog();  // Open the open file dialog
        }
    }
}

