using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.Structure;

namespace SS_OpenCV
{
    public partial class MainForm : Form
    {
        Image<Bgr, Byte> img = null; // working image
        Image<Bgr, Byte> imgUndo = null; // undo backup image - UNDO
        string title_bak = "";
        public int[] weighttest = new int[9];
        public int weight_factortest;

        public MainForm()
        {
            InitializeComponent();
            title_bak = Text;
        }

        /// <summary>
        /// Opens a new image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                img = new Image<Bgr, byte>(openFileDialog1.FileName);
                Text = title_bak + " [" +
                        openFileDialog1.FileName.Substring(openFileDialog1.FileName.LastIndexOf("\\") + 1) +
                        "]";
                imgUndo = img.Copy();
                ImageViewer.Image = img.Bitmap;
                ImageViewer.Refresh();
            }
        }

        /// <summary>
        /// Saves an image with a new name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ImageViewer.Image.Save(saveFileDialog1.FileName);
            }
        }

        /// <summary>
        /// Closes the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// restore last undo copy of the working image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imgUndo == null) // verify if the image is already opened
                return; 
            Cursor = Cursors.WaitCursor;
            img = imgUndo.Copy();

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        /// <summary>
        /// Chaneg visualization mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoZoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // zoom
            if (autoZoomToolStripMenuItem.Checked)
            {
                ImageViewer.SizeMode = PictureBoxSizeMode.Zoom;
                ImageViewer.Dock = DockStyle.Fill;
            }
            else // with scroll bars
            {
                ImageViewer.Dock = DockStyle.None;
                ImageViewer.SizeMode = PictureBoxSizeMode.AutoSize;
            }
        }

        /// <summary>
        /// Show authors form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AuthorsForm form = new AuthorsForm();
            form.ShowDialog();
        }


        /// <summary>
        /// Convert the working image to grayscale
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 

            //copy Undo Image
            imgUndo = img.Copy();

            ImageClass.ConvertToGray(img);

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        /// <summary>
        /// Calculate the image negative
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void negativeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 

            //copy Undo Image
            imgUndo = img.Copy();

            ImageClass.Negative(img);

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        private void translationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 

            //copy Undo Image
            imgUndo = img.Copy();

            ImageClass.translation(img,imgUndo);

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        private void meanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 

            //copy Undo Image
            imgUndo = img.Copy();

            ImageClass.meanFilter(img, imgUndo);

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        private void nonUniformToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (img == null) // verify if the image is already opened
                return;

       /*     weighttest[0] =  1;
            weighttest[1] = 1;
            weighttest[2] = 1;
            weighttest[3] = 1;
            weighttest[4] = 1;
            weighttest[5] = 1;
            weighttest[6] = 1;
            weighttest[7] = 1;
            weighttest[8] = 1;
            weight_factortest=9; */


        nonuniform_coeff nonuni = new nonuniform_coeff();
            nonuni.ShowDialog();


            Cursor = Cursors.WaitCursor; // clock cursor 

            //copy Undo Image
            imgUndo = img.Copy();
            

            try
            {
                ImageClass.filNUnif(img, imgUndo, nonuni.weight, nonuni.weight_factor);
            }
            catch { }

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 

        }

        private void filtersToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void medianaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;

    
            Cursor = Cursors.WaitCursor; // clock cursor 

            //copy Undo Image
            imgUndo = img.Copy();


            try
            {
                ImageClass.mediana(img, imgUndo);
            }
            catch { }

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 

        }

        private void manualBinaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // protege de executar a função sem ainda ter aberto a imagem 
                return;

            Cursor = Cursors.WaitCursor; // cursor relogio
            //copy Undo Image
            imgUndo = img.Copy();
            InputBox form = new InputBox("Threshold?");
            form.ShowDialog();
            int threshold = Convert.ToInt32(form.ValueTextBox.Text);
            ImageClass.Manual_binarization(img, imgUndo, threshold);
            ImageViewer.Refresh(); // atualiza imagem no ecrã
            Cursor = Cursors.Default;
        }

        private void sobelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 

            //copy Undo Image
            imgUndo = img.Copy();

            ImageClass.sobel_filter(img, imgUndo);

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // protege de executar a função sem ainda ter aberto a imagem 
                return;

            Cursor = Cursors.WaitCursor; // cursor relogio
            //copy Undo Image
            imgUndo = img.Copy();
            ImageClass.Histogram(img, imgUndo);
            ImageViewer.Refresh(); // atualiza imagem no ecrã
            Cursor = Cursors.Default;
        }

        private void otsuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // protege de executar a função sem ainda ter aberto a imagem 
                return;

            Cursor = Cursors.WaitCursor; // cursor relogio
            //copy Undo Image
            imgUndo = img.Copy();
            ImageClass.Otsu(img, imgUndo);
            ImageViewer.Refresh(); // atualiza imagem no ecrã
            Cursor = Cursors.Default;

        }
    }
}
 