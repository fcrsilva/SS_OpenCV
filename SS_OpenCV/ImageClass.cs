using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Windows.Forms;
using System.Drawing;

namespace SS_OpenCV
{
    class ImageClass
    {

        /// <summary>
        /// Image Negative using EmguCV library
        /// Slower method
        /// </summary>
        /// <param name="img">Image</param>
        public static void Negative1(Image<Bgr, byte> img)
        {
            Bgr aux;
            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    // emguCV access: slower
                    aux = img[y, x];
                    img[y, x] = new Bgr(255 - aux.Blue, 255 - aux.Green, 255 - aux.Red);
                }
            }
        }

        public static void ConvertToGray(Image<Bgr, byte> img)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte blue, green, red, gray;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            //obtém as 3 componentes
                            blue = dataPtr[0];
                            green = dataPtr[1];
                            red = dataPtr[2];

                            // convert to gray
                            gray = (byte)(((int)blue + green + red) / 3);

                            // store in the image
                            dataPtr[0] = gray;
                            dataPtr[1] = gray;
                            dataPtr[2] = gray;

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
            }
        }

        public static void Negative(Image<Bgr, byte> img)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte blue, green, red, gray;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            //obtém as 3 componentes
                            blue = dataPtr[0];
                            green = dataPtr[1];
                            red = dataPtr[2];

                            // convert to gray
                            gray = (byte)(((int)blue + green + red) / 3);

                            // store in the image
                            dataPtr[0] = (byte)(255 - blue);
                            dataPtr[1] = (byte)(255 - green);
                            dataPtr[2] = (byte)(255 - red);

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
            }
        }

        public static void RedChannel(Image<Bgr, byte> img)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte red;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            //obtém a componente red
                            red = dataPtr[2];

                            // store in the image
                            dataPtr[0] = red;
                            dataPtr[1] = red;
                            dataPtr[2] = red;

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
            }
        }

        public static void BrightContrast(Image<Bgr, byte> img, int bright, double contrast)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte blue, green, red;

                double newblue, newgreen, newred;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            //obtém as 3 componentes
                            blue = dataPtr[0];
                            green = dataPtr[1];
                            red = dataPtr[2];

                            newblue = (blue * contrast + bright);

                            if (newblue > 255)
                                newblue = 255;
                            if (newblue < 0)
                                newblue = 0;

                            newgreen = (green * contrast + bright);

                            if (newgreen > 255)
                                newgreen = 255;
                            if (newgreen < 0)
                                newgreen = 0;

                            newred = (red * contrast + bright);

                            if (newred > 255)
                                newred = 255;
                            if (newred < 0)
                                newred = 0;

                            // store in the image
                            dataPtr[0] = (byte)Math.Round(newblue);
                            dataPtr[1] = (byte)Math.Round(newgreen);
                            dataPtr[2] = (byte)Math.Round(newred);

                    // advance the pointer to the next pixel
                    dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
            }
        }

        public static void Translation(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, int dx, int dy)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right
                MIplImage m = img.MIplImage;
                MIplImage mU = imgCopy.MIplImage;

                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrUndo = (byte*)mU.imageData.ToPointer(); // Pointer to the image

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y, x_orig, y_orig;
                int movx = dx, movy = dy;


                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {

                        for (x = 0; x < width; x++)
                        {

                                x_orig = x - movx;
                                y_orig = y - movy;

                                if (x_orig < width && x_orig >= 0 && y_orig < height && y_orig >= 0)
                                {
                                    dataPtr[0] = (dataPtrUndo + y_orig * m.widthStep + x_orig * nChan)[0];
                                    dataPtr[1] = (dataPtrUndo + y_orig * m.widthStep + x_orig * nChan)[1];
                                    dataPtr[2] = (dataPtrUndo + y_orig * m.widthStep + x_orig * nChan)[2];
                }
                                else
                                {
                                    dataPtr[0] = (byte)0;
                                dataPtr[1] = (byte)0;
                                dataPtr[2] = (byte)0;
                                }
                            

                            // advance the pointer to the next pixel
                            dataPtr += nChan;

                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;

                    }
                }
            }
        }

        public static void Rotation(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float angle)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right
                MIplImage m = img.MIplImage;
                MIplImage mU = imgCopy.MIplImage;

                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrUndo = (byte*)mU.imageData.ToPointer(); // Pointer to the image

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y, x_orig, y_orig;
                //angle = -angle*(float)Math.PI/180;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {

                        for (x = 0; x < width; x++)
                        {
                                x_orig = (int) Math.Round((x - (width/2))*Math.Cos(angle)-((height/2)-y)*Math.Sin(angle)+width/2);
                                y_orig =(int) Math.Round((height/2)-(x-(width/2))*Math.Sin(angle)-((height/2)-y)*Math.Cos(angle));

                            if (x_orig < width && x_orig >= 0 && y_orig < height && y_orig >= 0)
                                {
                                dataPtr[0] = (dataPtrUndo + y_orig * m.widthStep + x_orig * nChan)[0];
                                dataPtr[1] = (dataPtrUndo + y_orig * m.widthStep + x_orig * nChan)[1];
                                dataPtr[2] = (dataPtrUndo + y_orig * m.widthStep + x_orig * nChan)[2];
                            }
                            else
                            {
                                dataPtr[0] = (byte)0;
                                dataPtr[1] = (byte)0;
                                dataPtr[2] = (byte)0;
                            }


                            // advance the pointer to the next pixel
                            dataPtr += nChan;

                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;

                    }
                }
            }
        }

        public static void Scale(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float scaleFactor)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right
                MIplImage m = img.MIplImage;
                MIplImage mU = imgCopy.MIplImage;

                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrUndo = (byte*)mU.imageData.ToPointer(); // Pointer to the image

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y, x_orig, y_orig;


                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {

                        for (x = 0; x < width; x++)
                        {

                            x_orig = (int) (x / scaleFactor);
                            y_orig = (int)(y / scaleFactor);

                            if (x_orig < width && x_orig >= 0 && y_orig < height && y_orig >= 0)
                            {
                                dataPtr[0] = (dataPtrUndo + y_orig * m.widthStep + x_orig * nChan)[0];
                                dataPtr[1] = (dataPtrUndo + y_orig * m.widthStep + x_orig * nChan)[1];
                                dataPtr[2] = (dataPtrUndo + y_orig * m.widthStep + x_orig * nChan)[2];
                            }
                            else
                            {
                                dataPtr[0] = (byte)0;
                                dataPtr[1] = (byte)0;
                                dataPtr[2] = (byte)0;
                            }


                            // advance the pointer to the next pixel
                            dataPtr += nChan;

                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;

                    }
                }
            }
        }

        public static void Scale_point_xy(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float scaleFactor, int centerX, int centerY)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right
                MIplImage m = img.MIplImage;
                MIplImage mU = imgCopy.MIplImage;

                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrUndo = (byte*)mU.imageData.ToPointer(); // Pointer to the image

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y, x_orig, y_orig;


                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {

                        for (x = 0; x < width; x++)
                        {
                            
                            x_orig = (int)Math.Round(((centerX - ((1 / scaleFactor) * width * 0.5)) + ((1 / scaleFactor) * x)));
                            y_orig = (int)Math.Round(((centerY - ((1 / scaleFactor) * height * 0.5)) + ((1 / scaleFactor) * y)));

                            if (x_orig < width && x_orig >= 0 && y_orig < height && y_orig >= 0)
                            {
                                dataPtr[0] = (dataPtrUndo + y_orig * m.widthStep + x_orig * nChan)[0];
                                dataPtr[1] = (dataPtrUndo + y_orig * m.widthStep + x_orig * nChan)[1];
                                dataPtr[2] = (dataPtrUndo + y_orig * m.widthStep + x_orig * nChan)[2];
                            }
                            else
                            {
                               dataPtr[0] = (byte)0;
                               dataPtr[1] = (byte)0;
                               dataPtr[2] = (byte)0;
                            }


                            // advance the pointer to the next pixel
                            dataPtr += nChan;

                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;

                    }
                }
            }
        }

        public static void Mean(Image<Bgr, byte> img, Image<Bgr, byte> imgUndo)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right
                MIplImage m = img.MIplImage;
                MIplImage mU = imgUndo.MIplImage;

                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrUndo = (byte*)mU.imageData.ToPointer(); // Pointer to the image

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;


                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {

                        for (x = 0; x < width; x++)
                        {



                            if (y != 0 && x != 0 && y != height - 1 && x != width - 1)
                            {
                                //obtém as 3 componentesgrab

                                dataPtr[0] = (byte)(((dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] +
                                (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] +
                                (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] +
                                (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] +
                                (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]) / 9);

                                dataPtr[1] = (byte)(((dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] +
                                (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] +
                                (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
                                (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] +
                                (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]) / 9);

                                dataPtr[2] = (byte)(((dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2] +
                                (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] +
                                (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
                                (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] +
                                (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]) / 9);


                            }

                            if (y == 0) //primeira linha
                            {
                                if (x != 0 && x != width) //excluir os cantos
                                {
                                    dataPtr[0] = (byte)((
                                    2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] +
                                    (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]) / 9);

                                    dataPtr[1] = (byte)((
                                    2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] +
                                    (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]) / 9);

                                    dataPtr[2] = (byte)((
                                    2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] +
                                    (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]) / 9);

                                }

                                if (x == 0)//canto sup esq
                                {
                                    dataPtr[0] = (byte)((
                                    4 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                    2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]) / 9);

                                    dataPtr[1] = (byte)((
                                    4 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                    2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]) / 9);

                                    dataPtr[2] = (byte)((
                                    4 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                    2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]) / 9);
                                }

                                if (x == width)//canto sup esq
                                {
                                    dataPtr[0] = (byte)((
                                    4 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] +
                                    2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0]) / 9);

                                    dataPtr[1] = (byte)((
                                    4 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
                                    2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1]) / 9);

                                    dataPtr[2] = (byte)((
                                    4 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
                                    2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2]) / 9);
                                }
                            }

                            if (x == 0) //primeira coluna
                            {
                                if (y != 0 && y != height) //excluir o canto
                                {
                                    dataPtr[0] = (byte)((
                                    2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                    (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] +
                                    2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]) / 9);

                                    dataPtr[1] = (byte)((
                                    2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                    (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] +
                                    2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]) / 9);

                                    dataPtr[2] = (byte)((
                                    2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                    (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] +
                                    2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]) / 9);

                                }
                            }

                            if (y == height) //ultima linha
                            {
                                if (x != 0 && y != height) //excluir os cantos
                                {
                                    dataPtr[0] = (byte)(((dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] +
                                    (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                    (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) / 9);

                                    dataPtr[1] = (byte)(((dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] +
                                    (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                    (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]) / 9);

                                    dataPtr[2] = (byte)((
                                    2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                    (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] +
                                    2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]) / 9);

                                }
                                if (x == 0) //canto inferior esquerdo
                                {
                                    dataPtr[0] = (byte)((
                                    2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                    (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] +
                                    4 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) / 9);

                                    dataPtr[1] = (byte)((
                                    2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                    (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] +
                                    4 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]) / 9);


                                    dataPtr[2] = (byte)((
                                    2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                    (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] +
                                    4 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]) / 9);

                                }
                                if (x == width) //canto inferior direito
                                {
                                    dataPtr[0] = (byte)((
                                    2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                    (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] +
                                    4 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0]) / 9);

                                    dataPtr[1] = (byte)((
                                    2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                    (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] +
                                    4 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1]) / 9);


                                    dataPtr[2] = (byte)((
                                    2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                    (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2] +
                                    4 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2]) / 9);

                                }
                            }
                            if (x == width) //ultima coluna
                            {
                                if (y != 0 && y != height) //excluir os cantos
                                {

                                    dataPtr[0] = (byte)(((dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] +
                                    2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                    (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] +
                                    2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0]) / 9);

                                    dataPtr[1] = (byte)(((dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] +
                                    2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                    (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] +
                                    2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1]) / 9);

                                    dataPtr[2] = (byte)(((dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2] +
                                    2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                    (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] +
                                    2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2]) / 9);
                                }

                            }




                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                            dataPtrUndo += nChan;

                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                        dataPtrUndo += padding;

                    }
                }
            }
        } 

        public static void NonUniform(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float[,] matrix, float matrixWeight)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right
                MIplImage m = img.MIplImage;
                MIplImage mU = imgCopy.MIplImage;

                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrUndo = (byte*)mU.imageData.ToPointer(); // Pointer to the image

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;
                float[] matrixaux = new float[9];
                matrixaux[0] = matrix[0, 0];
                matrixaux[1] = matrix[0, 1];
                matrixaux[2] = matrix[0, 2];
                matrixaux[3] = matrix[1, 0];
                matrixaux[4] = matrix[1, 1];
                matrixaux[0] = matrix[1, 2];
                matrixaux[0] = matrix[2, 0];
                matrixaux[0] = matrix[2, 1];
                matrixaux[0] = matrix[2, 2];

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            if (y != 0 && x != 0 && y != height - 1 && x != width - 1)
                            {
                                //obtém as 3 componentesgrab

                                dataPtr[0] = (byte)((matrixaux[0] * (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] +
                                matrixaux[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                matrixaux[2] * (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] +
                                matrixaux[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] +
                                matrixaux[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                matrixaux[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                matrixaux[6] * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] +
                                matrixaux[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                matrixaux[8] * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]) / matrixWeight);

                                dataPtr[1] = (byte)((matrixaux[0] * (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] +
                                matrixaux[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                matrixaux[2] * (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] +
                                matrixaux[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
                                matrixaux[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                matrixaux[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                matrixaux[6] * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] +
                                matrixaux[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                matrixaux[8] * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]) / matrixWeight);

                                dataPtr[2] = (byte)((matrixaux[0] * (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2] +
                                matrixaux[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                matrixaux[2] * (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] +
                                matrixaux[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
                                matrixaux[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                matrixaux[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                matrixaux[6] * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] +
                                matrixaux[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                matrixaux[8] * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]) / matrixWeight);


                            }

                            if (y == 0) //primeira linha
                            {
                                if (x != 0 && x != width) //excluir os cantos
                                {
                                    dataPtr[0] = (byte)((
                                    matrixaux[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] +
                                    matrixaux[0] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] +
                                    matrixaux[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[1] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                    matrixaux[2] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                    matrixaux[6] * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] +
                                    matrixaux[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[8] * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]) / matrixWeight);

                                    dataPtr[1] = (byte)((
                                    matrixaux[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
                                    matrixaux[0] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
                                    matrixaux[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[1] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                    matrixaux[2] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                    matrixaux[6] * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] +
                                    matrixaux[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[8] * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]) / matrixWeight);

                                    dataPtr[2] = (byte)((
                                    matrixaux[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
                                    matrixaux[0] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
                                    matrixaux[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[1] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                    matrixaux[2] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                    matrixaux[6] * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] +
                                    matrixaux[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[8] * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]) / matrixWeight);

                                }

                                if (x == 0)//canto sup esq
                                {
                                    dataPtr[0] = (byte)((
                                    matrixaux[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[0] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[1] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[2] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                    matrixaux[3] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                    matrixaux[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[6] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[8] * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]) / matrixWeight);

                                    dataPtr[1] = (byte)((
                                    matrixaux[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[0] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[1] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[2] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                    matrixaux[3] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                    matrixaux[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[6] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[8] * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]) / matrixWeight);

                                    dataPtr[2] = (byte)((
                                    matrixaux[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[0] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[1] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[2] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                    matrixaux[3] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                    matrixaux[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[6] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[8] * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]) / matrixWeight);
                                }

                                if (x == width)//canto sup esq
                                {
                                    dataPtr[0] = (byte)((
                                    matrixaux[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[0] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[1] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[2] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] +
                                    matrixaux[5] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] +
                                    matrixaux[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[6] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[8] * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0]) / matrixWeight);

                                    dataPtr[1] = (byte)((
                                     matrixaux[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[0] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[1] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[2] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
                                    matrixaux[5] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
                                    matrixaux[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[6] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[8] * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1]) / matrixWeight);

                                    dataPtr[2] = (byte)((
                                    matrixaux[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[0] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[1] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[2] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
                                    matrixaux[5] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
                                    matrixaux[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[6] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[8] * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2]) / matrixWeight);
                                }
                            }

                            if (x == 0) //primeira coluna
                            {
                                if (y != 0 && y != height) //excluir o canto
                                {
                                    dataPtr[0] = (byte)((
                                    matrixaux[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[0] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[3] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                    matrixaux[2] * (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] +
                                    matrixaux[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[6] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[8] * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]) / matrixWeight);

                                    dataPtr[1] = (byte)((
                                    matrixaux[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[0] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[3] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                    matrixaux[2] * (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] +
                                    matrixaux[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[6] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[8] * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]) / matrixWeight);

                                    dataPtr[2] = (byte)((
                                    matrixaux[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[0] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[3] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                    matrixaux[2] * (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] +
                                    matrixaux[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[6] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[8] * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]) / matrixWeight);

                                }
                            }

                            if (y == height) //ultima linha
                            {
                                if (x != 0 && y != height) //excluir os cantos
                                {
                                    dataPtr[0] = (byte)((
                                    matrixaux[0] * (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] +
                                    matrixaux[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[2] * (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] +
                                    matrixaux[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] +
                                    matrixaux[6] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] +
                                    matrixaux[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[7] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                    matrixaux[8] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) / matrixWeight);

                                    dataPtr[1] = (byte)((
                                    matrixaux[0] * (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] +
                                    matrixaux[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[2] * (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] +
                                    matrixaux[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
                                    matrixaux[6] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
                                    matrixaux[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[7] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                    matrixaux[8] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]) / matrixWeight);

                                    dataPtr[2] = (byte)((
                                   matrixaux[0] * (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2] +
                                    matrixaux[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[2] * (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] +
                                    matrixaux[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
                                    matrixaux[6] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
                                    matrixaux[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[7] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                    matrixaux[8] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]) / matrixWeight);

                                }
                                if (x == 0) //canto inferior esquerdo
                                {
                                    dataPtr[0] = (byte)((
                                    matrixaux[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[0] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[2] * (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] +
                                    matrixaux[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[3] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[6] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[7] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                    matrixaux[8] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) / matrixWeight);

                                    dataPtr[1] = (byte)((
                                    matrixaux[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[0] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[2] * (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] +
                                    matrixaux[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[3] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[6] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[7] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                    matrixaux[8] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]) / matrixWeight);


                                    dataPtr[2] = (byte)((
                                    matrixaux[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[0] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[2] * (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] +
                                    matrixaux[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[3] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[6] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[7] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                    matrixaux[8] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]) / matrixWeight);

                                }
                                if (x == width) //canto inferior direito
                                {
                                    dataPtr[0] = (byte)((
                                    matrixaux[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[2] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[0] * (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] +
                                    matrixaux[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[5] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[6] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[7] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] +
                                    matrixaux[8] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0]) / matrixWeight);

                                    dataPtr[1] = (byte)((
                                    matrixaux[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[2] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[0] * (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] +
                                    matrixaux[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[5] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[6] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[7] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
                                    matrixaux[8] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1]) / matrixWeight);


                                    dataPtr[2] = (byte)((
                                    matrixaux[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[2] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[0] * (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2] +
                                    matrixaux[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[5] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[6] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[7] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
                                    matrixaux[8] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2]) / matrixWeight);

                                }
                            }
                            if (x == width) //ultima coluna
                            {
                                if (y != 0 && y != height) //excluir os cantos
                                {

                                    dataPtr[0] = (byte)((
                                    matrixaux[0] * (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] +
                                    matrixaux[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[2] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] +
                                    matrixaux[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[5] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[6] * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] +
                                    matrixaux[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                    matrixaux[8] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0]) / matrixWeight);

                                    dataPtr[1] = (byte)((
                                    matrixaux[0] * (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] +
                                    matrixaux[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[2] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
                                    matrixaux[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[5] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[6] * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] +
                                    matrixaux[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                    matrixaux[8] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1]) / matrixWeight);

                                    dataPtr[2] = (byte)((
                                    matrixaux[0] * (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2] +
                                    matrixaux[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[2] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
                                    matrixaux[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[5] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[6] * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] +
                                    matrixaux[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                    matrixaux[8] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2]) / matrixWeight);
                                }

                            }




                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                            dataPtrUndo += nChan;

                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                        dataPtrUndo += padding;

                    }
                }
            }
        }//change matrix to 2 indexes

        public static void Median(Image<Bgr, byte> img, Image<Bgr, byte> imgUndo)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right
                MIplImage m = img.MIplImage;
                MIplImage mU = imgUndo.MIplImage;

                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrUndo = (byte*)mU.imageData.ToPointer(); // Pointer to the image

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y, min_dist, dist, index;
                int[] red = new int[9];
                int[] green = new int[9];
                int[] blue = new int[9];


                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {

                        for (x = 0; x < width; x++)
                        {

                            if (y != 0 && x != 0 && y != height - 1 && x != width - 1)
                            {

                                blue[0] = (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0];
                            blue[1] = (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0];
                            blue[2] = (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0];
                            blue[3] = (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0];
                            blue[4] = (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0];
                            blue[5] = (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0];
                            blue[6] = (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0];
                            blue[7] = (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0];
                            blue[8] = (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0];

                            green[0] = (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1];
                            green[1] = (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1];
                            green[2] = (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1];
                            green[3] = (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1];
                            green[4] = (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1];
                            green[5] = (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1];
                            green[6] = (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1];
                            green[7] = (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1];
                            green[8] = (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1];

                            red[0] = (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2];
                            red[1] = (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2];
                            red[2] = (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2];
                            red[3] = (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2];
                            red[4] = (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2];
                            red[5] = (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2];
                            red[6] = (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2];
                            red[7] = (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2];
                            red[8] = (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2];
                            }

                            if (y != 0 && x != 0 && y != height - 1 && x != width - 1)
                            {


                                dist =
                                    Math.Abs(blue[0] - blue[1]) + Math.Abs(red[0] - red[1]) + Math.Abs(green[0] - green[1]) +
                                    Math.Abs(blue[0] - blue[2]) + Math.Abs(red[0] - red[2]) + Math.Abs(green[0] - green[2]) +
                                    Math.Abs(blue[0] - blue[3]) + Math.Abs(red[0] - red[3]) + Math.Abs(green[0] - green[3]) +
                                    Math.Abs(blue[0] - blue[4]) + Math.Abs(red[0] - red[4]) + Math.Abs(green[0] - green[4]) +
                                    Math.Abs(blue[0] - blue[5]) + Math.Abs(red[0] - red[5]) + Math.Abs(green[0] - green[5]) +
                                    Math.Abs(blue[0] - blue[6]) + Math.Abs(red[0] - red[6]) + Math.Abs(green[0] - green[6]) +
                                    Math.Abs(blue[0] - blue[7]) + Math.Abs(red[0] - red[7]) + Math.Abs(green[0] - green[7]) +
                                    Math.Abs(blue[0] - blue[8]) + Math.Abs(red[0] - red[8]) + Math.Abs(green[0] - green[8]);

                                min_dist = dist;
                                index = 0;

                                dist =
                                    Math.Abs(blue[1] - blue[0]) + Math.Abs(red[1] - red[0]) + Math.Abs(green[1] - green[0]) +
                                    Math.Abs(blue[1] - blue[2]) + Math.Abs(red[1] - red[2]) + Math.Abs(green[1] - green[2]) +
                                    Math.Abs(blue[1] - blue[3]) + Math.Abs(red[1] - red[3]) + Math.Abs(green[1] - green[3]) +
                                    Math.Abs(blue[1] - blue[4]) + Math.Abs(red[1] - red[4]) + Math.Abs(green[1] - green[4]) +
                                    Math.Abs(blue[1] - blue[5]) + Math.Abs(red[1] - red[5]) + Math.Abs(green[1] - green[5]) +
                                    Math.Abs(blue[1] - blue[6]) + Math.Abs(red[1] - red[6]) + Math.Abs(green[1] - green[6]) +
                                    Math.Abs(blue[1] - blue[7]) + Math.Abs(red[1] - red[7]) + Math.Abs(green[1] - green[7]) +
                                    Math.Abs(blue[1] - blue[8]) + Math.Abs(red[1] - red[8]) + Math.Abs(green[1] - green[8]);

                                if (dist < min_dist)
                                {
                                    min_dist = dist;
                                    index = 1;
                                }

                                dist =
                                        Math.Abs(blue[2] - blue[0]) + Math.Abs(red[2] - red[0]) + Math.Abs(green[2] - green[0]) +
                                        Math.Abs(blue[2] - blue[1]) + Math.Abs(red[2] - red[1]) + Math.Abs(green[2] - green[1]) +
                                        Math.Abs(blue[2] - blue[3]) + Math.Abs(red[2] - red[3]) + Math.Abs(green[2] - green[3]) +
                                        Math.Abs(blue[2] - blue[4]) + Math.Abs(red[2] - red[4]) + Math.Abs(green[2] - green[4]) +
                                        Math.Abs(blue[2] - blue[5]) + Math.Abs(red[2] - red[5]) + Math.Abs(green[2] - green[5]) +
                                        Math.Abs(blue[2] - blue[6]) + Math.Abs(red[2] - red[6]) + Math.Abs(green[2] - green[6]) +
                                        Math.Abs(blue[2] - blue[7]) + Math.Abs(red[2] - red[7]) + Math.Abs(green[2] - green[7]) +
                                        Math.Abs(blue[2] - blue[8]) + Math.Abs(red[2] - red[8]) + Math.Abs(green[2] - green[8]);

                                if (dist < min_dist)
                                {
                                    min_dist = dist;
                                    index = 2;
                                }

                                dist =
                                        Math.Abs(blue[3] - blue[0]) + Math.Abs(red[3] - red[0]) + Math.Abs(green[2] - green[0]) +
                                        Math.Abs(blue[3] - blue[1]) + Math.Abs(red[3] - red[1]) + Math.Abs(green[2] - green[1]) +
                                        Math.Abs(blue[3] - blue[2]) + Math.Abs(red[3] - red[2]) + Math.Abs(green[2] - green[2]) +
                                        Math.Abs(blue[3] - blue[4]) + Math.Abs(red[3] - red[4]) + Math.Abs(green[2] - green[4]) +
                                        Math.Abs(blue[3] - blue[5]) + Math.Abs(red[3] - red[5]) + Math.Abs(green[2] - green[5]) +
                                        Math.Abs(blue[3] - blue[6]) + Math.Abs(red[3] - red[6]) + Math.Abs(green[2] - green[6]) +
                                        Math.Abs(blue[3] - blue[7]) + Math.Abs(red[3] - red[7]) + Math.Abs(green[2] - green[7]) +
                                        Math.Abs(blue[3] - blue[8]) + Math.Abs(red[3] - red[8]) + Math.Abs(green[2] - green[8]);

                                if (dist < min_dist)
                                {
                                    min_dist = dist;
                                    index = 3;
                                }


                                dist =
                                        Math.Abs(blue[4] - blue[0]) + Math.Abs(red[4] - red[0]) + Math.Abs(green[4] - green[0]) +
                                        Math.Abs(blue[4] - blue[1]) + Math.Abs(red[4] - red[1]) + Math.Abs(green[4] - green[1]) +
                                        Math.Abs(blue[4] - blue[2]) + Math.Abs(red[4] - red[2]) + Math.Abs(green[4] - green[2]) +
                                        Math.Abs(blue[4] - blue[3]) + Math.Abs(red[4] - red[3]) + Math.Abs(green[4] - green[3]) +
                                        Math.Abs(blue[4] - blue[5]) + Math.Abs(red[4] - red[5]) + Math.Abs(green[4] - green[5]) +
                                        Math.Abs(blue[4] - blue[6]) + Math.Abs(red[4] - red[6]) + Math.Abs(green[4] - green[6]) +
                                        Math.Abs(blue[4] - blue[7]) + Math.Abs(red[4] - red[7]) + Math.Abs(green[4] - green[7]) +
                                        Math.Abs(blue[4] - blue[8]) + Math.Abs(red[4] - red[8]) + Math.Abs(green[4] - green[8]);

                                if (dist < min_dist)
                                {
                                    min_dist = dist;
                                    index = 4;
                                }


                                dist =
                                        Math.Abs(blue[5] - blue[0]) + Math.Abs(red[5] - red[0]) + Math.Abs(green[5] - green[0]) +
                                        Math.Abs(blue[5] - blue[1]) + Math.Abs(red[5] - red[1]) + Math.Abs(green[5] - green[1]) +
                                        Math.Abs(blue[5] - blue[2]) + Math.Abs(red[5] - red[2]) + Math.Abs(green[5] - green[2]) +
                                        Math.Abs(blue[5] - blue[3]) + Math.Abs(red[5] - red[3]) + Math.Abs(green[5] - green[3]) +
                                        Math.Abs(blue[5] - blue[4]) + Math.Abs(red[5] - red[4]) + Math.Abs(green[5] - green[4]) +
                                        Math.Abs(blue[5] - blue[6]) + Math.Abs(red[5] - red[6]) + Math.Abs(green[5] - green[6]) +
                                        Math.Abs(blue[5] - blue[7]) + Math.Abs(red[5] - red[7]) + Math.Abs(green[5] - green[7]) +
                                        Math.Abs(blue[5] - blue[8]) + Math.Abs(red[5] - red[8]) + Math.Abs(green[5] - green[8]);

                                if (dist < min_dist)
                                {
                                    min_dist = dist;
                                    index = 5;
                                }

                                dist =
                                        Math.Abs(blue[6] - blue[0]) + Math.Abs(red[6] - red[0]) + Math.Abs(green[6] - green[0]) +
                                        Math.Abs(blue[6] - blue[1]) + Math.Abs(red[6] - red[1]) + Math.Abs(green[6] - green[1]) +
                                        Math.Abs(blue[6] - blue[2]) + Math.Abs(red[6] - red[2]) + Math.Abs(green[6] - green[2]) +
                                        Math.Abs(blue[6] - blue[3]) + Math.Abs(red[6] - red[3]) + Math.Abs(green[6] - green[3]) +
                                        Math.Abs(blue[6] - blue[4]) + Math.Abs(red[6] - red[4]) + Math.Abs(green[6] - green[4]) +
                                        Math.Abs(blue[6] - blue[5]) + Math.Abs(red[6] - red[5]) + Math.Abs(green[6] - green[5]) +
                                        Math.Abs(blue[6] - blue[7]) + Math.Abs(red[6] - red[7]) + Math.Abs(green[6] - green[7]) +
                                        Math.Abs(blue[6] - blue[8]) + Math.Abs(red[6] - red[8]) + Math.Abs(green[6] - green[8]);

                                if (dist < min_dist)
                                {
                                    min_dist = dist;
                                    index = 6;
                                }


                                dist =
                                        Math.Abs(blue[7] - blue[0]) + Math.Abs(red[7] - red[0]) + Math.Abs(green[7] - green[0]) +
                                        Math.Abs(blue[7] - blue[1]) + Math.Abs(red[7] - red[1]) + Math.Abs(green[7] - green[1]) +
                                        Math.Abs(blue[7] - blue[2]) + Math.Abs(red[7] - red[2]) + Math.Abs(green[7] - green[2]) +
                                        Math.Abs(blue[7] - blue[3]) + Math.Abs(red[7] - red[3]) + Math.Abs(green[7] - green[3]) +
                                        Math.Abs(blue[7] - blue[4]) + Math.Abs(red[7] - red[4]) + Math.Abs(green[7] - green[4]) +
                                        Math.Abs(blue[7] - blue[5]) + Math.Abs(red[7] - red[5]) + Math.Abs(green[7] - green[5]) +
                                        Math.Abs(blue[7] - blue[6]) + Math.Abs(red[7] - red[6]) + Math.Abs(green[7] - green[6]) +
                                        Math.Abs(blue[7] - blue[8]) + Math.Abs(red[7] - red[8]) + Math.Abs(green[7] - green[8]);

                                if (dist < min_dist)
                                {
                                    min_dist = dist;
                                    index = 7;
                                }


                                dist =
                                        Math.Abs(blue[8] - blue[0]) + Math.Abs(red[8] - red[0]) + Math.Abs(green[8] - green[0]) +
                                        Math.Abs(blue[8] - blue[1]) + Math.Abs(red[8] - red[1]) + Math.Abs(green[8] - green[1]) +
                                        Math.Abs(blue[8] - blue[2]) + Math.Abs(red[8] - red[2]) + Math.Abs(green[8] - green[2]) +
                                        Math.Abs(blue[8] - blue[3]) + Math.Abs(red[8] - red[3]) + Math.Abs(green[8] - green[3]) +
                                        Math.Abs(blue[8] - blue[4]) + Math.Abs(red[8] - red[4]) + Math.Abs(green[8] - green[4]) +
                                        Math.Abs(blue[8] - blue[5]) + Math.Abs(red[8] - red[5]) + Math.Abs(green[8] - green[5]) +
                                        Math.Abs(blue[8] - blue[6]) + Math.Abs(red[8] - red[6]) + Math.Abs(green[8] - green[6]) +
                                        Math.Abs(blue[8] - blue[7]) + Math.Abs(red[8] - red[7]) + Math.Abs(green[8] - green[7]);

                                if (dist < min_dist)
                                {
                                    min_dist = dist;
                                    index = 8;
                                }

                                dataPtr[0] = (byte)blue[index];
                                dataPtr[2] = (byte)red[index];
                                dataPtr[3] = (byte)green[index];

                            }

                            if (y == 0) //primeira linha
                            {
                                if (x != 0 && x != width) //excluir os cantos
                                {


                                }

                                if (x == 0)//canto sup esq
                                {

                                }

                                if (x == width)//canto sup esq
                                {

                                }
                            }

                            if (x == 0) //primeira coluna
                            {
                                if (y != 0 && y != height) //excluir o canto
                                {


                                }
                            }

                            if (y == height) //ultima linha
                            {
                                if (x != 0 && y != height) //excluir os cantos
                                {


                                }
                                if (x == 0) //canto inferior esquerdo
                                {


                                }
                                if (x == width) //canto inferior direito
                                {

                                }
                            }
                            if (x == width) //ultima coluna
                            {
                                if (y != 0 && y != height) //excluir os cantos
                                {


                                }

                            }




                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                            dataPtrUndo += nChan;

                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                        dataPtrUndo += padding;

                    }
                }
            }
        }

        public static int[,] Histogram_All(Image<Bgr, byte> img)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right
                MIplImage m = img.MIplImage;

                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)

                int x, y;

                int[,] all = new int[4, 256];

                //iniciar os vectores a zero
                //Array.Clear(red_array, 0, red_array.Length);
                //Array.Clear(blue_array, 0, blue_array.Length);
                //Array.Clear(green_array, 0, green_array.Length);
                //Array.Clear(media_array, 0, media_array.Length);

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            byte blue = 0;
                            byte red = 0;
                            byte green = 0;
                            int media = 0;

                            blue = dataPtr[0];
                            red = dataPtr[1];
                            green =dataPtr[2];
                            media = (int) Math.Round((blue + red + green) / 3.0);

                            all[1,(int)Math.Round(blue*1.0)]++;
                            all[2,(int)Math.Round(red*1.0)]++;
                            all[3,(int)Math.Round(green*1.0)]++;
                            all[0,media]++;


                            // advance the pointer to the next pixel
                            dataPtr += nChan;

                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;

                    }
                }
                //Histograma_window Histrograma_form = new Histograma_window(all[0,]);
                //Histrograma_form.ShowDialog();
                return all;
            }
        }

        public static int[,] Histogram_RGB(Image<Bgr, byte> img)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right
                MIplImage m = img.MIplImage;

                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)

                int x, y;

                int[,] all = new int[3, 256];

                //iniciar os vectores a zero
                //Array.Clear(red_array, 0, red_array.Length);
                //Array.Clear(blue_array, 0, blue_array.Length);
                //Array.Clear(green_array, 0, green_array.Length);
                //Array.Clear(media_array, 0, media_array.Length);

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            int blue = 0;
                            int red = 0;
                            int green = 0;

                            blue = dataPtr[0];
                            red = dataPtr[1];
                            green = dataPtr[2];

                            all[0, blue]++;
                            all[1, red]++;
                            all[2, green]++;


                            // advance the pointer to the next pixel
                            dataPtr += nChan;

                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;

                    }
                }
                //Histograma_window Histrograma_form = new Histograma_window(all[0,]);
                //Histrograma_form.ShowDialog();
                return all;
            }
        }

        public static int[] Histogram_Gray(Image<Bgr, byte> img)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right
                MIplImage m = img.MIplImage;

                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)

                int x, y;

                int[] hist = new int[256];

                //iniciar os vectores a zero
                //Array.Clear(red_array, 0, red_array.Length);
                //Array.Clear(blue_array, 0, blue_array.Length);
                //Array.Clear(green_array, 0, green_array.Length);
                //Array.Clear(media_array, 0, media_array.Length);

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            byte blue = 0;
                            byte red = 0;
                            byte green = 0;
                            int media = 0;

                            blue = (dataPtr[0]);
                            red = (dataPtr[1]);
                            green = dataPtr[2];
                            media = (int)Math.Round((blue + red + green) / 3.0);


                            hist[media]++;


                            // advance the pointer to the next pixel
                            dataPtr += nChan;

                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;

                    }
                }
                //Histograma_window Histrograma_form = new Histograma_window(hist);
                //Histrograma_form.ShowDialog();
                return hist;
            }
        }

        public static void Sobel(Image<Bgr, byte> img, Image<Bgr, byte> imgUndo)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right
                MIplImage m = img.MIplImage;
                MIplImage mU = imgUndo.MIplImage;

                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrUndo = (byte*)mU.imageData.ToPointer(); // Pointer to the image

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y,green,blue,red,sobelX,sobelY,sumSobel;


                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            if (y != 0 && x != 0 && y != height - 1 && x != width - 1)
                            {
                                
                                sobelX = (int)Math.Abs(Math.Round((
                                (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] +
                                0.0 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                -1*(dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] +
                                2*(dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] +
                                0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                -2*(dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) +
                                ((dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] +
                                0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                -1*(dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0])));

                                sobelY = (int)Math.Abs(Math.Round((
                                -1*(dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] +
                                -2.0 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                -1*(dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] +
                                0*(dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] +
                                0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                0*(dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) +
                                ((dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] +
                                2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                1*(dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0])));

                                if (sobelX + sobelY > 255)
                                    sumSobel = 255;
                                else
                                    sumSobel = sobelX + sobelY;
                                dataPtr[0] = (byte)sumSobel;

                                sobelX = (int)Math.Abs(Math.Round((
                                (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] +
                                0.0 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                -1*(dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] +
                                2*(dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
                                0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                -2*(dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]) +
                                ((dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] +
                                0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                -1*(dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1])));

                                sobelY = (int)Math.Abs(Math.Round((
                                -1*(dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] +
                                -2.0 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                -1*(dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] +
                                0*(dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
                                0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                0*(dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]) +
                                ((dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] +
                                2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                1*(dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1])));

                                if (sobelX + sobelY > 255)
                                    sumSobel = 255;
                                else
                                    sumSobel = sobelX + sobelY;
                                dataPtr[1] = (byte)sumSobel;

                                sobelX = (int)Math.Abs(Math.Round((
                                (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2] +
                                0.0 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                -1*(dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] +
                                2*(dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
                                0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                -2*(dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]) +
                                ((dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] +
                                0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                -1*(dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2])));

                                sobelY = (int)Math.Abs(Math.Round((
                                -1*(dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2] +
                                -2.0 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                -1*(dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] +
                                0*(dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
                                0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                0*(dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]) +
                                ((dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] +
                                2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                1*(dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2])));

                                if (sobelX + sobelY > 255)
                                    sumSobel = 255;
                                else
                                    sumSobel = sobelX + sobelY;
                                dataPtr[2] = (byte)sumSobel;











                                //blue = (int)Math.Round((
                                //(dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] +
                                //2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                //(dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] -
                                //(dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] -
                                //2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] -
                                //(dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]) +
                                //((dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] +
                                //2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                //(dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0] -
                                //(dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] -
                                //2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] -
                                //(dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0])*1.0);

                                //dataPtr[0] = Math.Abs(blue)>=0 && Math.Abs(blue) <= 255 ? (byte) Math.Abs(blue) : (byte)255;

                                //green = (int)Math.Round((
                                // (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] +
                                //2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
                                //(dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] -
                                //(dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] -
                                //2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] -
                                //(dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]) +
                                //((dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] +
                                //2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                //(dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1] -
                                //(dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] -
                                //2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] -
                                //(dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1]*1.0));

                                //dataPtr[1] = Math.Abs(green) >= 0 && Math.Abs(green) <= 255 ? (byte)Math.Abs(green) : (byte)255;


                                //red = (int)Math.Round((
                                //(dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2] +
                                //2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
                                //(dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] -
                                //(dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] -
                                //2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] -
                                //(dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]) +
                                //((dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] +
                                //2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                //(dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2] -
                                //(dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2] -
                                //2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] -
                                //(dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2]*1.0));

                                //dataPtr[2] = Math.Abs(red) >= 0 && Math.Abs(red) <= 255 ? (byte)Math.Abs(red) : (byte)255;


                            }

                            if (y == 0) //primeira linha
                            {
                                if (x != 0 && x != width) //excluir os cantos
                                {
                                //    blue = (int)Math.Round((
                                //    (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                //    2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                //    (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] -
                                //    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] -
                                //    2 * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] -
                                //    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]) +
                                //    ((dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] +
                                //    2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                //    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0] -
                                //    (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] -
                                //    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] -
                                //    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] * 1.0));

                                //    dataPtr[0] = Math.Abs(blue)>=0 && Math.Abs(blue) <= 255 ? (byte) Math.Abs(blue) : (byte)255;

                                //    green = (int)Math.Round((
                                //    (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                //    2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                //    (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] -
                                //    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] -
                                //    2 * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] -
                                //    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]) +
                                //    ((dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] +
                                //    2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                //    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1] -
                                //    (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] -
                                //    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] -
                                //    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] * 1.0));

                                //dataPtr[1] = Math.Abs(green) >= 0 && Math.Abs(green) <= 255 ? (byte)Math.Abs(green) : (byte)255;

                                //    red = (int)Math.Round((
                                //    (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                //    2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                //    (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] -
                                //    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] -
                                //    2 * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] -
                                //    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]) +
                                //    ((dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] +
                                //    2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                //    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2] -
                                //    (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] -
                                //    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] -
                                //    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2]) * 1.0);

                                //    dataPtr[1] = Math.Abs(red) >= 0 && Math.Abs(red) <= 255 ? (byte)Math.Abs(red) : (byte)255;

                                }

                                if (x == 0)//canto sup esq
                                {
 //                                   blue = (int)Math.Round((
 //                                   (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
 //                                   (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] -
 //                                   2 * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]) +
 //                                   ((dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
 //                                   (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0] -
 //                                   (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] -
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0]) * 1.0);

 //dataPtr[0] = Math.Abs(blue)>=0 && Math.Abs(blue) <= 255 ? (byte) Math.Abs(blue) : (byte)255;

 //                                   green = (int)Math.Round((
 //                                    (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
 //                                   (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] -
 //                                   2 * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]) +
 //                                   ((dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
 //                                   (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1] -
 //                                   (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] -
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1]) * 1.0);

 //                               dataPtr[1] = Math.Abs(green) >= 0 && Math.Abs(green) <= 255 ? (byte)Math.Abs(green) : (byte)255;

 //                                   red = (int)Math.Round((
 //                                    (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
 //                                   (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] -
 //                                   2 * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]) +
 //                                   ((dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
 //                                   (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2] -
 //                                   (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] -
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] * 1.0));

 //                                   dataPtr[1] = Math.Abs(red) >= 0 && Math.Abs(red) <= 255 ? (byte)Math.Abs(red) : (byte)255;
                                }

                                if (x == width)//canto sup dir
                                {
 //                                   blue = (int)Math.Round((
 //                                   (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
 //                                   2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
 //                                   (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] -
 //                                   (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] -
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] -
 //                                   (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) +
 //                                   ((dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] +
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
 //                                   (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] -
 //                                   (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] -
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] -
 //                                   (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0]) * 1.0);

 //dataPtr[0] = Math.Abs(blue)>=0 && Math.Abs(blue) <= 255 ? (byte) Math.Abs(blue) : (byte)255;

 //                                   green = (int)Math.Round((
 //                                  (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
 //                                   2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
 //                                   (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] -
 //                                   (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] -
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] -
 //                                   (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]) +
 //                                   ((dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] +
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
 //                                   (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] -
 //                                   (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] -
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] -
 //                                   (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1]) * 1.0);

 //                               dataPtr[1] = Math.Abs(green) >= 0 && Math.Abs(green) <= 255 ? (byte)Math.Abs(green) : (byte)255;

 //                                   red = (int)Math.Round((
 //                                  (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
 //                                   2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
 //                                   (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] -
 //                                   (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] -
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] -
 //                                   (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]) +
 //                                   ((dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] +
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
 //                                   (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] -
 //                                   (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] -
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] -
 //                                   (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2]) * 1.0);
                                }
                            }

                            if (x == 0) //primeira coluna
                            {
                                if (y != 0 && y != height) //excluir o canto
                                {
 //                                   blue = (int)Math.Round((
 //                                   (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] +
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
 //                                   (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] -
 //                                   2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]) +
 //                                   ((dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
 //                                   (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0] -
 //                                   (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] -
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0]) * 1.0);

 //dataPtr[0] = Math.Abs(blue)>=0 && Math.Abs(blue) <= 255 ? (byte) Math.Abs(blue) : (byte)255;

 //                                   green = (int)Math.Round((
 //                                  (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
 //                                   (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] -
 //                                   2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]) +
 //                                   ((dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
 //                                   (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1] -
 //                                   (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] -
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1]) * 1.0);

 //                               dataPtr[1] = Math.Abs(green) >= 0 && Math.Abs(green) <= 255 ? (byte)Math.Abs(green) : (byte)255;

 //                                   red = (int)Math.Round((
 //                                  (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
 //                                   (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] -
 //                                   2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]) +
 //                                   ((dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
 //                                   (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2] -
 //                                   (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] -
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2]) * 1.0);

 //                                   dataPtr[1] = Math.Abs(red) >= 0 && Math.Abs(red) <= 255 ? (byte)Math.Abs(red) : (byte)255;

                         }
                            }

                            if (y == height) //ultima linha
                            {
                                if (x != 0 && y != height) //excluir os cantos
                                {
 //                                   blue = (int)Math.Round((
 //                                   (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] +
 //                                   2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
 //                                   (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] -
 //                                   2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0]) +
 //                                   ((dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
 //                                   (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] -
 //                                   (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] -
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0]) * 1.0);

 //dataPtr[0] = Math.Abs(blue)>=0 && Math.Abs(blue) <= 255 ? (byte) Math.Abs(blue) : (byte)255;

 //                                   green = (int)Math.Round((
 //                                   (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] +
 //                                   2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
 //                                   (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] -
 //                                   2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1]) +
 //                                   ((dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
 //                                   (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] -
 //                                   (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] -
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] * 1.0));

 //                               dataPtr[1] = Math.Abs(green) >= 0 && Math.Abs(green) <= 255 ? (byte)Math.Abs(green) : (byte)255;

 //                                   red = (int)Math.Round((
 //                                  (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2] +
 //                                   2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
 //                                   (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] -
 //                                   2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2]) +
 //                                   ((dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
 //                                   (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] -
 //                                   (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2] -
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2]) * 1.0);

 //                                   dataPtr[1] = Math.Abs(red) >= 0 && Math.Abs(red) <= 255 ? (byte)Math.Abs(red) : (byte)255;

                                }
                                if (x == 0) //canto inferior esquerdo
                                {
 //                                   blue = (int)Math.Round((
 //                                   (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] +
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
 //                                   (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] -
 //                                   2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0]) +
 //                                   ((dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
 //                                   (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] -
 //                                   (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] -
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0]) * 1.0);

 //dataPtr[0] = Math.Abs(blue)>=0 && Math.Abs(blue) <= 255 ? (byte) Math.Abs(blue) : (byte)255;

 //                                   green = (int)Math.Round((
 //                                   (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
 //                                   (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] -
 //                                   2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1]) +
 //                                   ((dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
 //                                   (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] -
 //                                   (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] -
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1]) * 1.0);

 //                               dataPtr[1] = Math.Abs(green) >= 0 && Math.Abs(green) <= 255 ? (byte)Math.Abs(green) : (byte)255;


 //                                   red = (int)Math.Round((
 //                                   (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
 //                                   (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] -
 //                                   2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2]) +
 //                                   ((dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
 //                                   (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] -
 //                                   (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] -
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2]) * 1.0);

 //                                   dataPtr[1] = Math.Abs(red) >= 0 && Math.Abs(red) <= 255 ? (byte)Math.Abs(red) : (byte)255;

                                }
                                if (x == width) //canto inferior direito
                                {
 //                                   blue = (int)Math.Round((
 //                                   (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] +
 //                                   2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
 //                                   (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] -
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] -
 //                                   (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0]) +
 //                                   ((dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] +
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
 //                                   (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] -
 //                                   (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] -
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] -
 //                                   (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0]) * 1.0);

 //dataPtr[0] = Math.Abs(blue)>=0 && Math.Abs(blue) <= 255 ? (byte) Math.Abs(blue) : (byte)255;

 //                                   green = (int)Math.Round((
 //                                   (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] +
 //                                   2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
 //                                   (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] -
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] -
 //                                   (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1]) +
 //                                   ((dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] +
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
 //                                   (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] -
 //                                   (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] -
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] -
 //                                   (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] * 1.0));

 //                               dataPtr[1] = Math.Abs(green) >= 0 && Math.Abs(green) <= 255 ? (byte)Math.Abs(green) : (byte)255;


 //                                   red = (int)Math.Round((
 //                                   (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2] +
 //                                   2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
 //                                   (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] -
 //                                   (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] -
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] -
 //                                   (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2]) +
 //                                   ((dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] +
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
 //                                   (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] -
 //                                   (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2] -
 //                                   2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] -
 //                                   (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2]) * 1.0);

 //                                   dataPtr[1] = Math.Abs(red) >= 0 && Math.Abs(red) <= 255 ? (byte)Math.Abs(red) : (byte)255;

                                }
                            }
                            if (x == width) //ultima coluna
                            {
                                if (y != 0 && y != height) //excluir os cantos
                                {

                                            //                                   blue = (int)Math.Round((
                                            //                                               (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] +
                                            //                                               2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                            //                                               (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] -
                                            //                                               (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] -
                                            //                                               2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] -
                                            //                                               (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) +
                                            //                                               ((dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] +
                                            //                                               2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                            //                                               (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] -
                                            //                                               (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] -
                                            //                                               2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] -
                                            //                                               (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0]) * 1.0);

                                            //dataPtr[0] = Math.Abs(blue)>=0 && Math.Abs(blue) <= 255 ? (byte) Math.Abs(blue) : (byte)255;

                                            //                                   green = (int)Math.Round((
                                            //                                               (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] +
                                            //                                               2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                            //                                               (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] -
                                            //                                               (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] -
                                            //                                               2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] -
                                            //                                               (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]) +
                                            //                                               ((dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] +
                                            //                                               2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                            //                                               (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] -
                                            //                                               (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] -
                                            //                                               2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] -
                                            //                                               (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1])*1.0);

                                            //                               dataPtr[1] = Math.Abs(green) >= 0 && Math.Abs(green) <= 255 ? (byte)Math.Abs(green) : (byte)255;

                                            //                                   red = (int)Math.Round((
                                            //                                               (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2] +
                                            //                                               2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                            //                                               (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] -
                                            //                                               (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] -
                                            //                                               2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] -
                                            //                                               (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]) +
                                            //                                               ((dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] +
                                            //                                               2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                            //                                               (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] -
                                            //                                               (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2] -
                                            //                                               2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] -
                                            //                                               (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2])*1.0);

                                            //                                   dataPtr[1] = Math.Abs(red) >= 0 && Math.Abs(red) <= 255 ? (byte)Math.Abs(red) : (byte)255;
                                        }

                            }




                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                            dataPtrUndo += nChan;

                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                        dataPtrUndo += padding;

                    }
                }
            }
        }//BORDER

        public static void Diferentiation(Image<Bgr, byte> img, Image<Bgr, byte> imgUndo)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right
                MIplImage m = img.MIplImage;
                MIplImage mU = imgUndo.MIplImage;

                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrUndo = (byte*)mU.imageData.ToPointer(); // Pointer to the image

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y; //new_pixel = 0;


                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {

                            if (y != 0 && x != 0 && y != height - 1 && x != width - 1)
                            {
                                dataPtr[0] = (byte)(
                                            Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] - (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) +
                                            Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] - (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0]));

                                dataPtr[1] = (byte)(
                                            Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] - (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]) +
                                            Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] - (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1]));

                                dataPtr[2] = (byte)(
                                            Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] - (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]) +
                                            Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] - (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2]));
                            }

                            if (y == 0) //primeira linha
                            {
                                if (x == width)//canto sup dir
                                {
                                    dataPtr[0] = (byte)(
                                            Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] - (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0]) +
                                           Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] - (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0]));

                                    dataPtr[1] = (byte)(
                                                Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] - (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1]) +
                                                Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] - (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1]));

                                    dataPtr[2] = (byte)(
                                                Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] - (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2]) +
                                                Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] - (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2]));
                                }
                            }

                            if (x == 0) //primeira coluna
                            {
                                if (y != 0 && y != height) //excluir o canto
                                {
                                    dataPtr[0] = (byte)(Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] + (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0]) +
                                           Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] - (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]));

                                    dataPtr[1] = (byte)(Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] + (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1]) +
                                                Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] - (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]));

                                    dataPtr[2] = (byte)(Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] + (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2]) +
                                                Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] - (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]));

                                }
                            }

                            if (y == height) //ultima linha
                            {
                                if (x != 0 && y != height) //excluir os cantos
                                {
                                    dataPtr[0] = (byte)(Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] + (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0]) +
                                           Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] - (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]));

                                    dataPtr[1] = (byte)(Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] + (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1]) +
                                                Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] - (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]));

                                    dataPtr[2] = (byte)(Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] + (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2]) +
                                                Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] - (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]));

                                }
                                if (x == 0) //canto inferior esquerdo
                                {
                                    dataPtr[0] = (byte)(Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] + (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0]) +
                                            Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] - (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]));

                                    dataPtr[1] = (byte)(Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] + (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1]) +
                                                Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] - (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]));

                                    dataPtr[2] = (byte)(Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] + (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2]) +
                                                Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] - (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]));

                                }
                                if (x == width) //canto inferior direito
                                {
                                    dataPtr[0] = (byte)(Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] + (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0]) +
                                            Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] - (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]));

                                    dataPtr[1] = (byte)(Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] + (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1]) +
                                                Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] - (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]));

                                    dataPtr[2] = (byte)(Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] + (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2]) +
                                                Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] - (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]));

                                }
                            }
                            if (x == width) //ultima coluna
                            {
                                if (y != 0 && y != height) //excluir os cantos
                                {

                                    dataPtr[0] = (byte)(Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] + (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0]) +
                                           Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] - (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]));

                                    dataPtr[1] = (byte)(Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] + (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1]) +
                                                Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] - (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]));

                                    dataPtr[2] = (byte)(Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] + (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2]) +
                                                Math.Abs((byte)(dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] - (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]));
                                }

                            }




                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                            dataPtrUndo += nChan;

                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                        dataPtrUndo += padding;

                    }
                }
            }
        }//border

        public static void ConvertToBW(Image<Bgr, byte> img, int threshold)

        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right
                MIplImage m = img.MIplImage;

                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)

                int x, y;
                int media;


                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            byte blue = dataPtr[0];
                            byte red = dataPtr[1];
                            byte green = dataPtr[2];

                            media = (int)Math.Round(((blue + red + green) / 3.0));

                            if (media <= threshold)
                            {
                                dataPtr[0] = 0;
                                dataPtr[1] = 0;
                                dataPtr[2] = 0;
                            }
                            else
                            {
                                dataPtr[0] = 255;
                                dataPtr[1] = 255;
                                dataPtr[2] = 255;
                            }


                            // advance the pointer to the next pixel
                            dataPtr += nChan;

                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;

                    }
                }

            }
        }

        public static void ConvertToBW_Otsu(Image<Bgr, byte> img)

        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right
                MIplImage m = img.MIplImage;

                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)

                int x, y, t, gray, threshold_out;
                double q1, q2, m1, m2;
                int[] hist = new int[256];
                //double vari1;
                //double vari2;
                double numberPixels = width * height;


                //MessageBox alert = new MessageBox(1);

                //iniciar os vectores a zero
                Array.Clear(hist, 0, hist.Length);

                double largest_vari = 0;
                double vari;


                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            int blue = (int)dataPtr[0];
                            int red = (int)dataPtr[1];
                            int green = (int)dataPtr[2];

                            gray = (int) Math.Round(((blue + red + green) / 3.0));

                            hist[gray]++;



                            // advance the pointer to the next pixel
                            dataPtr += nChan;

                        }
                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;

                    }
                }

                threshold_out = 0;
                vari = 0;
                for (t = 1; t < 255; t++)
                {
                    //System.Diagnostics.Debug.WriteLine("TESTE»»»»»»»:\n");

                    q1 = 0;
                    q2 = 0;
                    m1 = 0;
                    m2 = 0;
                    //vari1 = 0;
                    //vari2 = 0;
                    vari = 0;
                    for (int i = 0; i <= 255; i++)
                    {
                        //System.Diagnostics.Debug.WriteLine("TESTE»»»»»»»\n:" + hist[i]);

                        if (i<= t)
                        {
                            //Console.Write("TESTE»»»»»»»\nq1:");

                            q1 = (q1 + hist[i] / numberPixels);
                            m1 = (m1 + (hist[i] / numberPixels) * i);
                        }
                        if (i > t)
                        {
                            q2 = (q2 + hist[i] / numberPixels);
                            m2 = (m2 + (hist[i] / numberPixels) * i);

                        }

                    }
                    if (q1 == 0)
                        m1 = 0;
                    else
                        m1 = m1 / q1;
                    if (q2 == 0)
                        m2 = 0;
                    else
                        m2 = m2 / q2;

                    //for (int i = 0; i <= 255; i++)
                    //{
                    //    if (i <= t)
                    //    {
                    //        vari1 += Math.Pow((m1 - i), 2) * (hist[i] / numberPixels);
                    //    }
                    //    if (i > t)
                    //    {
                    //        vari2 += Math.Pow((m2 - i), 2) * (hist[i] / numberPixels);
                    //    }
                    //}

                    //vari1 = vari1 / t;
                    //vari2 = vari2 / (256 - t);

                    //vari = q1 * vari1 + q2 * vari2;
                    //Console.Write("VARIACE»»»»»»»»:" + vari + "\n");

                    vari = q1 * q2 * Math.Pow((m2-m1),2);


                    if ( vari > largest_vari)
                    {

                        threshold_out = t;
                        largest_vari = vari;

                    }
                   // Console.Write("VARIACE»»»»»»»»:" + vari+"\n");


                }
                //MessageBox.Show("Chosen Threshold:"+ threshold_out);
                Console.Write("Threshold do metodo de otsu: " + threshold_out+"\n");
                ConvertToBW(img, threshold_out);

            }


        }

        public static void LP_Recognition(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, out Rectangle LP_Location, 
            out Rectangle LP_Chr1, out Rectangle LP_Chr2, out Rectangle LP_Chr3, out Rectangle LP_Chr4, out Rectangle LP_Chr5, 
            out Rectangle LP_Chr6, out string LP_C1, out string LP_C2, out string LP_C3, out string LP_C4, out string LP_C5, 
            out string LP_C6, out string LP_Country, out string LP_Month, out string LP_Year)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right
                MIplImage m = img.MIplImage;
                Image<Bgr, byte> imgcopy, imgCores;
                imgcopy = img.Copy();
                imgCores = img.Copy();
                MIplImage color = imgCores.MIplImage;


                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtr1 = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtr2 = (byte*)color.imageData.ToPointer(); // Pointer to the image



                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y, sobelX, sobelY, sumSobel;
                double windowSum;
                int possibleY = 0;
                int plateY=0;
                int plateX = 0;
                double maxValue = 0;
                double minValue;
                int[] verticalSum = new int[width];
                double[] horizontalSum = new double[height];
                int[] window1 = new int[2];
                int[] window2 = new int[2];



                //ConvertToGray(img);
                imgcopy = img.Copy();
                Mean(img, imgcopy);
                imgcopy = img.Copy();
                Mean(img, imgcopy);
                ConvertToBW_Otsu(img);
                //Negative(img);
                imgcopy = img.Copy();

                Sobel(img, imgcopy);

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            if (y != 0 && x != 0 && y != height - 1 && x != width - 1)
                            {
                                if (dataPtr[0] != (dataPtr + (0) * m.widthStep + (-1) * nChan)[0])//compare with previous value
                                {
                                    horizontalSum[y]++;
                                }
                            }
                            // advance the pointer to the next pixel
                            dataPtr += nChan;

                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;

                    }
                }
                window1[0] = 0;
                window1[1] = 10;
                //for (y = 0; y < height; y++)
                //{
                //    windowSum = 0;
                //    if(y+ window1[1] < height)
                //    for (int i = 0; i< window1[1]; i++)//TODO Remove this loop
                //    {
                //        windowSum += horizontalSum[y + i];
                //    }
                //    if (windowSum > maxValue)
                //    {
                //        maxValue = horizontalSum[y];
                //        plateY = y;
                //    }
                //}
                windowSum = horizontalSum[0] + horizontalSum[1] + horizontalSum[2] + horizontalSum[3] + horizontalSum[4] + horizontalSum[5] +
                    horizontalSum[6] + horizontalSum[7] + horizontalSum[8] + horizontalSum[9];

                for (y = 0; y < height - 11; y++)//
                {
                    windowSum = windowSum - horizontalSum[y] + horizontalSum[y + 10];
                    if (windowSum > maxValue)
                    {
                        maxValue = windowSum;
                        possibleY = y;
                        plateY = y;
                    }
                }



                //plateY = (plateY + plateY + 10) / 2;
                int topY = 0;
                int bottomY = 0;

                windowSum = maxValue;
                minValue = maxValue;
                maxValue = horizontalSum[plateY];


                y = possibleY - 1;
                do
                {
                    y--;
                    topY = y;
                } while (horizontalSum[y] > maxValue / 2.2);



                topY++;
                windowSum = maxValue;
                minValue = maxValue;

                y = possibleY;
                do
                {
                    y++;
                    bottomY = y;
                } while (horizontalSum[y] > maxValue / 2.2);



                maxValue = 0;


                dataPtr1 = (byte*)m.imageData.ToPointer();
                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        if (y != 0 && x != 0 && y != height - 1 && x != width - 1)
                        {
                            if (y >= topY && y <= bottomY)
                            {   
                                if(dataPtr1[0]==255)
                                verticalSum[x]++;

                            }
                        }

                        // advance the pointer to the next pixel
                        dataPtr1 += nChan;

                    }

                    //at the end of the line advance the pointer by the aligment bytes (padding)
                    dataPtr1 += padding;

                }

                topY -= 2;
                bottomY += 2;
                dataPtr1 = (byte*)m.imageData.ToPointer();
                int[] aux = new int[20];
                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        if (y != 0 && x != 0 && y != height - 1 && x != width - 1)
                        {
                            if (y == topY)
                            {
                                if ((dataPtr1 + (-3) * m.widthStep + (-3) * nChan)[0] == 255)
                                    aux[0] = 0;
                                if ((dataPtr1 + (-3) * m.widthStep + (-3) * nChan)[0] == 0)
                                    aux[0] = 1;
                                if ((dataPtr1 + (-2) * m.widthStep + (-3) * nChan)[0] == 255)
                                    aux[1] = 0;
                                if ((dataPtr1 + (-2) * m.widthStep + (-3) * nChan)[0] == 0)
                                    aux[1] = 1;
                                if ((dataPtr1 + (-3) * m.widthStep + (-2) * nChan)[0] == 255)
                                    aux[2] = 0;
                                if ((dataPtr1 + (-3) * m.widthStep + (-2) * nChan)[0] == 0)
                                    aux[2] = 1;
                                if ((dataPtr1 + (0) * m.widthStep + (-3) * nChan)[0] == 255)
                                    aux[3] = 0;
                                if ((dataPtr1 + (0) * m.widthStep + (-3) * nChan)[0] == 0)
                                    aux[3] = 1;
                                if ((dataPtr1 + (-3) * m.widthStep + (0) * nChan)[0] == 255)
                                    aux[4] = 0;
                                if ((dataPtr1 + (-3) * m.widthStep + (0) * nChan)[0] == 0)
                                    aux[4] = 1;
                                if ((dataPtr1 + (0) * m.widthStep + (-2) * nChan)[0] == 255)
                                    aux[8] = 0;
                                if ((dataPtr1 + (0) * m.widthStep + (-2) * nChan)[0] == 0)
                                    aux[8] = 1;


                                if ((dataPtr1 + (1) * m.widthStep + (0) * nChan)[0] == 255)
                                    aux[5] = 1;
                                if ((dataPtr1 + (1) * m.widthStep + (0) * nChan)[0] == 0)
                                    aux[5] = 0;
                                if ((dataPtr1 + (1) * m.widthStep + (1) * nChan)[0] == 255)
                                    aux[6] = 1;
                                if ((dataPtr1 + (1) * m.widthStep + (1) * nChan)[0] == 0)
                                    aux[6] = 0;
                                if ((dataPtr1 + (1) * m.widthStep + (-1) * nChan)[0] == 255)
                                    aux[7] = 1;
                                if ((dataPtr1 + (1) * m.widthStep + (-1) * nChan)[0] == 0)
                                    aux[7] = 0;
                                if ((dataPtr1 + (0) * m.widthStep + (0) * nChan)[0] == 255)
                                    aux[9] = 1;
                                if ((dataPtr1 + (0) * m.widthStep + (0) * nChan)[0] == 0)
                                    aux[9] = 0;



                                verticalSum[x] *= (((aux[5]==1 || aux[6]==1 || aux[7]==1) ? 1 :0) *aux[0] * aux[1] * aux[2] * aux[3]*aux[4]*aux[9] * aux[8]);

                            }

                        }
                        // advance the pointer to the next pixel
                        dataPtr1 += nChan;

                    }

                    //at the end of the line advance the pointer by the aligment bytes (padding)
                    dataPtr1 += padding;

                }


                int maxValue2 = 0;
                for (x = 0; x < width; x++)
                {
                    if (verticalSum[x] > maxValue2)
                    {
                        maxValue2 = verticalSum[x];
                        plateX = x;
                    }
                }

                int scaleFactor = 5;
                int plateH = bottomY - topY;
                int plateW =plateH*scaleFactor;
                int plateL = plateX;
                int plateR = plateL + plateW;

                dataPtr1 = (byte*)m.imageData.ToPointer();
                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        if (y != 0 && x != 0 && y != height - 1 && x != width - 1)
                        {




                        if (y == topY)
                        {


                            (dataPtr1 + (0) * m.widthStep + (0) * nChan)[1] = 255;



                        }


                        if (y == bottomY)
                        {


                            (dataPtr1 + (-1) * m.widthStep + (-1) * nChan)[2] = 255;
                            (dataPtr1 + (-1) * m.widthStep + (0) * nChan)[2] = 255;
                            (dataPtr1 + (-1) * m.widthStep + (1) * nChan)[2] = 255;
                            (dataPtr1 + (0) * m.widthStep + (-1) * nChan)[2] = 255;
                            (dataPtr1 + (0) * m.widthStep + (0) * nChan)[2] = 255;
                            (dataPtr1 + (0) * m.widthStep + (1) * nChan)[2] = 255;
                            (dataPtr1 + (1) * m.widthStep + (-1) * nChan)[2] = 255;
                            (dataPtr1 + (1) * m.widthStep + (0) * nChan)[2] = 255;
                            (dataPtr1 + (1) * m.widthStep + (1) * nChan)[2] = 255;


                        }

                            if (verticalSum[x]!=0)
                            {


                                //s(dataPtr1 + (0) * m.widthStep + (0) * nChan)[2] = 255;



                            }

                            if (x == plateL || x==plateR)
                            {


                                (dataPtr1 + (0) * m.widthStep + (0) * nChan)[2] = 255;
                                (dataPtr1 + (0) * m.widthStep + (0) * nChan)[0] = 255;
                                (dataPtr1 + (0) * m.widthStep + (0) * nChan)[1] = 255;


                            }

                        }
                        // advance the pointer to the next pixel
                        dataPtr1 += nChan;

                    }

                    //at the end of the line advance the pointer by the aligment bytes (padding)
                    dataPtr1 += padding;

                }



                img = imgCores.Copy();


                LP_Location = new Rectangle(plateL, topY, plateW,plateH);
                img = imgCores.Copy();
                img.ROI = LP_Location;
               LP_Chr1 = new Rectangle(1, 2, 3, 4);
            LP_Chr2 = new Rectangle(1, 2, 3, 4);
            LP_Chr3 = new Rectangle(1, 2, 3, 4);
            LP_Chr4 = new Rectangle(1, 2, 3, 4);
            LP_Chr5 = new Rectangle(1, 2, 3, 4);
            LP_Chr6 = new Rectangle(1, 2, 3, 4);
            LP_C1 = "";
            LP_C2 = "";
            LP_C3 = "";
            LP_C4 = "";
            LP_C5 = "";
            LP_C6 = "";
            LP_Country = "";
            LP_Month = "";
            LP_Year = "";


            }
        }



        //TODO:

        //public static void Mean_solutionB(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy) {


        //}

        //public static void Mean_solutionC(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, int size)
        //{


        //}

        //public static void Roberts(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        //{


        //}

        //public static void Rotation_Bilinear(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float angle)
        //{


        //}

        //public static void Scale_Bilinear(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float scaleFactor)
        //{


        //}

        //public static void Scale_point_xy_Bilinear(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float scaleFactor, int centerX, int centerY)
        //{


        //}




    }



    }

