using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

namespace SS_OpenCV
{
    class ImageClass
    {

        /// <summary>
        /// Image Negative using EmguCV library
        /// Slower method
        /// </summary>
        /// <param name="img">Image</param>
        /// 
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
                float[,] matrix = new float[3, 3];
                matrix[0, 0] = 1;
                matrix[0, 1] = 1;
                matrix[0, 2] = 1;
                matrix[1, 0] = 1;
                matrix[1, 1] = 1;
                matrix[1, 2] = 1;
                matrix[2, 0] = 1;
                matrix[2, 1] = 1;
                matrix[2, 2] = 1;
                float matrixWeight = 9;
                NonUniform(img, imgUndo, matrix,matrixWeight);
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
                int x, y,red,green,blue;
                float[] matrixaux = new float[9];
                matrixaux[0] = matrix[0, 0];
                matrixaux[1] = matrix[0, 1];
                matrixaux[2] = matrix[0, 2];
                matrixaux[3] = matrix[1, 0];
                matrixaux[4] = matrix[1, 1];
                matrixaux[5] = matrix[1, 2];
                matrixaux[6] = matrix[2, 0];
                matrixaux[7] = matrix[2, 1];
                matrixaux[8] = matrix[2, 2];

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            if (y != 0 && x != 0 && y != height - 1 && x != width - 1)
                            {
                                //obtém as 3 componentesgrab

                                blue = (int)Math.Abs(Math.Round((matrixaux[0] * (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] +
                                matrixaux[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                matrixaux[2] * (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] +
                                matrixaux[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] +
                                matrixaux[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                matrixaux[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                matrixaux[6] * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] +
                                matrixaux[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                matrixaux[8] * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]) / matrixWeight));

                                if (blue > 255)
                                    blue = 255;
                                dataPtr[0] = (byte)blue;


                                green = (int)Math.Abs(Math.Round((matrixaux[0] * (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] +
                                matrixaux[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                matrixaux[2] * (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] +
                                matrixaux[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
                                matrixaux[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                matrixaux[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                matrixaux[6] * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] +
                                matrixaux[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                matrixaux[8] * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]) / matrixWeight));

                                if (green > 255)
                                    green = 255;
                                dataPtr[1] = (byte)green;

                                red = (int)Math.Abs(Math.Round((matrixaux[0] * (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2] +
                                matrixaux[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                matrixaux[2] * (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] +
                                matrixaux[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
                                matrixaux[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                matrixaux[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                matrixaux[6] * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] +
                                matrixaux[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                matrixaux[8] * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]) / matrixWeight));

                                if (red > 255)
                                    red = 255;
                                dataPtr[2] = (byte)red;


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
        }

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

                            blue[0] = (int)(1.0*(dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0]);
                            blue[1] = (int)(1.0*(dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0]);
                            blue[2] = (int)(1.0 * (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0]);
                            blue[3] = (int)(1.0 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0]);
                            blue[4] = (int)(1.0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0]);
                            blue[5] = (int)(1.0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]);
                            blue[6] = (int)(1.0 * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0]);
                            blue[7] = (int)(1.0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0]);
                            blue[8] = (int)(1.0 *(dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]);

                            green[0] = (int)(1.0 * (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1]);
                            green[1] = (int)(1.0 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1]);
                            green[2] = (int)(1.0 * (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1]);
                            green[3] = (int)(1.0 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1]);
                            green[4] = (int)(1.0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1]);
                            green[5] = (int)(1.0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]);
                            green[6] = (int)(1.0 * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1]);
                            green[7] = (int)(1.0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1]);
                            green[8] = (int)(1.0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]);

                            red[0] = (int)(1.0 * (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2]);
                            red[1] = (int)(1.0 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2]);
                            red[2] = (int)(1.0 * (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2]);
                            red[3] = (int)(1.0 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2]);
                            red[4] = (int)(1.0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2]);
                            red[5] = (int)(1.0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]);
                            red[6] = (int)(1.0 * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2]);
                            red[7] = (int)(1.0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2]);
                            red[8] = (int)(1.0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]);
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
                                        Math.Abs(blue[3] - blue[0]) + Math.Abs(red[3] - red[0]) + Math.Abs(green[3] - green[0]) +
                                        Math.Abs(blue[3] - blue[1]) + Math.Abs(red[3] - red[1]) + Math.Abs(green[3] - green[1]) +
                                        Math.Abs(blue[3] - blue[2]) + Math.Abs(red[3] - red[2]) + Math.Abs(green[3] - green[2]) +
                                        Math.Abs(blue[3] - blue[4]) + Math.Abs(red[3] - red[4]) + Math.Abs(green[3] - green[4]) +
                                        Math.Abs(blue[3] - blue[5]) + Math.Abs(red[3] - red[5]) + Math.Abs(green[3] - green[5]) +
                                        Math.Abs(blue[3] - blue[6]) + Math.Abs(red[3] - red[6]) + Math.Abs(green[3] - green[6]) +
                                        Math.Abs(blue[3] - blue[7]) + Math.Abs(red[3] - red[7]) + Math.Abs(green[3] - green[7]) +
                                        Math.Abs(blue[3] - blue[8]) + Math.Abs(red[3] - red[8]) + Math.Abs(green[3] - green[8]);

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
                                dataPtr[1] = (byte)green[index];

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
        }//not working

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






                            }

                            if (y == 0) //primeira linha
                            {
                                if (x != 0 && x != width) //excluir os cantos
                                {
                               
                                }

                                if (x == 0)//canto sup esq
                                {
 //                                  
                                }

                                if (x == width)//canto sup dir
                                {
 //                                  
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
 //                                 
                                }
                                if (x == 0) //canto inferior esquerdo
                                {
 //                                   
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
        }

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
                //Console.Write("Threshold do metodo de otsu: " + threshold_out+"\n");
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

                Image<Bgr, Byte> imgChar1 = null;
                Image<Bgr, Byte> imgChar2 = null;
                Image<Bgr, Byte> imgChar3 = null;
                Image<Bgr, Byte> imgChar4 = null;
                Image<Bgr, Byte> imgChar5 = null;
                Image<Bgr, Byte> imgChar6 = null;
                Image<Bgr, Byte> imgPlate = null;
                Image<Bgr, Byte> imgUndo = null;
                Image<Bgr, Byte> imgauxiliar = img.Copy();
                imgUndo = img.Copy();
                imgPlate = img.Copy();


                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right
                MIplImage m = img.MIplImage;


                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtr1 = (byte*)m.imageData.ToPointer(); // Pointer to the image



                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;
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

                Mean(img, imgCopy);
                imgCopy = img.Copy();
                Mean(img, imgCopy);

                imgCopy = img.Copy();
                ConvertToBW_Otsu(img);
                //Negative(img);

                imgCopy = img.Copy();
                Sobel(img, imgCopy);

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

                            (dataPtr1 + (0) * m.widthStep + (0) * nChan)[2] = 255;



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

             
            LP_Location = new Rectangle(plateL, topY, plateW,plateH);

                //LOCALIZAÇÃO DOS CARACTERES

                Image<Bgr, Byte> imgaux = imgUndo.Copy();
                imgPlate.ROI = LP_Location;
                imgaux = imgPlate.Copy();
                imgUndo = imgaux.Copy();
                ImageClass.CharLoc(imgaux, imgUndo, plateL, topY, out LP_Chr1, out LP_Chr2, out LP_Chr3, out LP_Chr4, out LP_Chr5, out LP_Chr6);
                img.Draw(LP_Chr1, new Bgr (Color.Red), 1);
                img.Draw(LP_Chr2, new Bgr (Color.Red), 1);
                img.Draw(LP_Chr3, new Bgr (Color.Red), 1);
                img.Draw(LP_Chr4, new Bgr (Color.Red), 1);
                img.Draw(LP_Chr5, new Bgr (Color.Red), 1);
                img.Draw(LP_Chr6, new Bgr (Color.Red), 1);
                imgChar1 = imgauxiliar.Copy();
                imgChar1.ROI = LP_Chr1;
                imgChar2 = imgauxiliar.Copy();
                imgChar2.ROI = LP_Chr2;
                imgChar3 = imgauxiliar.Copy();
                imgChar3.ROI = LP_Chr3;
                imgChar4 = imgauxiliar.Copy();
                imgChar4.ROI = LP_Chr4;
                imgChar5 = imgauxiliar.Copy();
                imgChar5.ROI = LP_Chr5;
                imgChar6 = imgauxiliar.Copy();
                imgChar6.ROI = LP_Chr6;

                char car1;
                char car2;
                char car3;
                char car4;
                char car5;
                char car6;

                ImageClass.CharIden(imgChar1, out car1);
                ImageClass.CharIden(imgChar2, out car2);
                ImageClass.CharIden(imgChar3, out car3);
                ImageClass.CharIden(imgChar4, out car4);
                ImageClass.CharIden(imgChar5, out car5);
                ImageClass.CharIden(imgChar6, out car6);
                

            LP_C1 = car1.ToString();
            LP_C2 = car2.ToString();
            LP_C3 = car3.ToString();
            LP_C4 = car4.ToString();
             LP_C5 = car5.ToString();
             LP_C6 = car6.ToString();
             LP_Country = "";
            LP_Month = "";
            LP_Year = "";

               
            }
        }

        public static void CharLoc(Image<Bgr, byte> img, Image<Bgr, byte> imgUndo,int offsetx ,int offsety,out Rectangle LP_Chr1, out Rectangle LP_Chr2, out Rectangle LP_Chr3, out Rectangle LP_Chr4, out Rectangle LP_Chr5, out Rectangle LP_Chr6)
        {//binarizacao e dpeois mudanças de 0 para 1 e ta feito
            unsafe
            {
                MIplImage m = img.MIplImage;
            byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
            byte* dataPtr1 = (byte*)m.imageData.ToPointer(); // Pointer to the image

            int width = img.Width;
            int height = img.Height;
            int nChan = m.nChannels; // number of channels - 3
            int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
            int x, y;
                int[] verticalSum = new int[width];
                int[] verticalSum1 = new int[width];
                int[] horizontalSum = new int[height];
                int[] gapSum = new int[width];
                float[,] matrix = new float[3,3];
                //Mean(img, imgUndo);
                //imgUndo = img.Copy();
                matrix[0, 0] =1;
                matrix[0, 1] =2;
                matrix[0, 2] =1;
                matrix[1, 0] =2;
                matrix[1, 1] =4;
                matrix[1, 2] =2;
                matrix[2, 0] =1;
                matrix[2, 1] =2;
                matrix[2, 2] = 1;
                    



                NonUniform(img, imgUndo,matrix,32);
                ConvertToBW_Otsu(img);

                dataPtr = (byte*)m.imageData.ToPointer();
            for (y = 0; y < height; y++)
            {
                for (x = 0; x < width; x++)
                {
                    if (y != 0 && x != 0 && y != height - 1 && x != width - 1)
                    {
                            if (dataPtr[0] != (dataPtr + (-1) * m.widthStep + (0) * nChan)[0])
                                verticalSum[x]++;
                            if (dataPtr[0]==0)
                                verticalSum1[x]++;

                        }

                    // advance the pointer to the next pixel
                    dataPtr += nChan;

                }

                //at the end of the line advance the pointer by the aligment bytes (padding)
                dataPtr += padding;

            }

                dataPtr = (byte*)m.imageData.ToPointer();
                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        if (y != 0 && x != 0 && y != height - 1 && x != width - 1)
                        {
                            if (dataPtr[0] != (dataPtr + (0) * m.widthStep + (-1) * nChan)[0])
                                horizontalSum[y]++;
                            //if (dataPtr[0] == 0)
                            //verticalSum1[x]++;

                        }

                        // advance the pointer to the next pixel
                        dataPtr += nChan;

                    }

                    //at the end of the line advance the pointer by the aligment bytes (padding)
                    dataPtr += padding;

                }



                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                       
                            if (verticalSum[x]<=1 || verticalSum1[x]>=0.90* height|| verticalSum1[x] <= 0.1 * height)
                            {
                                dataPtr1[0] = 254;
                                dataPtr1[1] = 0;
                                dataPtr1[2] = 0;

                            }

                       
                        // advance the pointer to the next pixel
                        dataPtr1 += nChan;

                    }

                    //at the end of the line advance the pointer by the aligment bytes (padding)
                    dataPtr1 += padding;

                }


                dataPtr1 = (byte*)m.imageData.ToPointer();
                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                     
                            if (horizontalSum[y] <= 5 )
                            {
                                dataPtr1[0] = 254;
                                dataPtr1[1] = 0;
                                dataPtr1[2] = 0;

                            }

                        // advance the pointer to the next pixel
                        dataPtr1 += nChan;

                    }

                    //at the end of the line advance the pointer by the aligment bytes (padding)
                    dataPtr1 += padding;

                }

                dataPtr = (byte*)m.imageData.ToPointer();
                int x1=0,y1=0;
                Boolean aux1 = true,aux2=true;

                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        if (y != 0 && x != 0 && y != height - 1 && x != width - 1)
                        {
                            if (dataPtr[0] != 254&&(y1==0||y1==y))
                            {
                                if ((dataPtr + (0) * m.widthStep + (-1) * nChan)[0] == 254)
                                x1 = x;
                                gapSum[x1]++;
                                y1 = y;
                           
                            }
                        }
                        // advance the pointer to the next pixel
                        dataPtr += nChan;

                    }

                    //at the end of the line advance the pointer by the aligment bytes (padding)
                    dataPtr += padding;

                }

         

                int maxValue0 = gapSum.Max();
                int maxIndex0 = gapSum.ToList().IndexOf(maxValue0);
                gapSum[maxIndex0] = -6;


                int maxValue1 = gapSum.Max();
                int maxIndex1 = gapSum.ToList().IndexOf(maxValue1);
                gapSum[maxIndex1] = -1;


                int maxValue2 = gapSum.Max();
                int maxIndex2 = gapSum.ToList().IndexOf(maxValue2);
                gapSum[maxIndex2] = -2;


                int maxValue3 = gapSum.Max();
                int maxIndex3 = gapSum.ToList().IndexOf(maxValue3);
                gapSum[maxIndex3] = -3;


                int maxValue4 = gapSum.Max();
                int maxIndex4 = gapSum.ToList().IndexOf(maxValue4);
                gapSum[maxIndex4] = -4;


                int maxValue5 = gapSum.Max();
                int maxIndex5 = gapSum.ToList().IndexOf(maxValue5);
                gapSum[maxIndex5] = -5;

                dataPtr1 = (byte*)m.imageData.ToPointer();
                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {

                        if (x == maxIndex1|| x == maxIndex2|| x == maxIndex3 ||x == maxIndex4 || x == maxIndex5 || x == maxIndex0 )
                        {
                            dataPtr1[0] = 0;
                            dataPtr1[1] = 0;
                            dataPtr1[2] = 255;

                        }

                        // advance the pointer to the next pixel
                        dataPtr1 += nChan;

                    }

                    //at the end of the line advance the pointer by the aligment bytes (padding)
                    dataPtr1 += padding;

                }


                dataPtr1 = (byte*)m.imageData.ToPointer();
                int charsheight = 0;
                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {

                        if (dataPtr[0] != 254&&x== maxIndex0&&y>y1)
                        {
                            charsheight++;
                        }

                        // advance the pointer to the next pixel
                        dataPtr1 += nChan;

                    }

                    //at the end of the line advance the pointer by the aligment bytes (padding)
                    dataPtr1 += padding;

                }
                charsheight-=1;

                int[] chars = new int[6];
                chars[0] = maxIndex0;
                chars[1] = maxIndex1;
                chars[2] = maxIndex2;
                chars[3] = maxIndex3;
                chars[4] = maxIndex4;
                chars[5] = maxIndex5;

                Array.Sort(chars);
                LP_Chr1 = new Rectangle();
                switch (gapSum[chars[0]])
                    {
                        case -6:
                        LP_Chr1 = new Rectangle(chars[0]+offsetx, y1 + offsety, maxValue0, charsheight);
                        break;
                        case -1:
                        LP_Chr1 = new Rectangle(chars[0] + offsetx, y1 + offsety, maxValue1, charsheight);
                        break;
                        case -2:
                        LP_Chr1 = new Rectangle(chars[0] + offsetx, y1 + offsety, maxValue2, charsheight);
                        break;
                        case -3:
                        LP_Chr1 = new Rectangle(chars[0] + offsetx, y1 + offsety, maxValue3, charsheight);
                        break;
                        case -4:
                        LP_Chr1 = new Rectangle(chars[0] + offsetx, y1 + offsety, maxValue4, charsheight);
                        break;
                        case -5:
                        LP_Chr1 = new Rectangle(chars[0] + offsetx, y1 + offsety, maxValue5, charsheight);
                        break;

                    default:
                            break;
                    }
                LP_Chr2 = new Rectangle();
                switch (gapSum[chars[1]])
                {
                    case -6:
                        LP_Chr2 = new Rectangle(chars[1] + offsetx, y1 + offsety, maxValue0, charsheight);
                        break;
                    case -1:
                        LP_Chr2 = new Rectangle(chars[1] + offsetx, y1 + offsety, maxValue1, charsheight);
                        break;
                    case -2:
                        LP_Chr2 = new Rectangle(chars[1] + offsetx, y1 + offsety, maxValue2, charsheight);
                        break;
                    case -3:
                        LP_Chr2 = new Rectangle(chars[1] + offsetx, y1 + offsety, maxValue3, charsheight);
                        break;
                    case -4:
                        LP_Chr2 = new Rectangle(chars[1] + offsetx, y1 + offsety, maxValue4, charsheight);
                        break;
                    case -5:
                        LP_Chr2 = new Rectangle(chars[1] + offsetx, y1 + offsety, maxValue5, charsheight);
                        break;

                    default:
                        break;
                }

                LP_Chr3 = new Rectangle();
                switch (gapSum[chars[2]])
                {
                    case -6:
                        LP_Chr3 = new Rectangle(chars[2] + offsetx, y1 + offsety, maxValue0, charsheight);
                        break;
                    case -1:
                        LP_Chr3 = new Rectangle(chars[2] + offsetx, y1 + offsety, maxValue1, charsheight);
                        break;
                    case -2:
                        LP_Chr3 = new Rectangle(chars[2] + offsetx, y1 + offsety, maxValue2, charsheight);
                        break;
                    case -3:
                        LP_Chr3 = new Rectangle(chars[2] + offsetx, y1 + offsety, maxValue3, charsheight);
                        break;
                    case -4:
                        LP_Chr3 = new Rectangle(chars[2] + offsetx, y1 + offsety, maxValue4, charsheight);
                        break;
                    case -5:
                        LP_Chr3 = new Rectangle(chars[2] + offsetx, y1 + offsety, maxValue5, charsheight);
                        break;

                    default:
                        break;
                }
                LP_Chr4 = new Rectangle();
                switch (gapSum[chars[3]])
                {
                    case -6:
                        LP_Chr4 = new Rectangle(chars[3] + offsetx, y1 + offsety, maxValue0, charsheight);
                        break;
                    case -1:
                        LP_Chr4 = new Rectangle(chars[3] + offsetx, y1 + offsety, maxValue1, charsheight);
                        break;
                    case -2:
                        LP_Chr4 = new Rectangle(chars[3] + offsetx, y1 + offsety, maxValue2, charsheight);
                        break;
                    case -3:
                        LP_Chr4 = new Rectangle(chars[3] + offsetx, y1 + offsety, maxValue3, charsheight);
                        break;
                    case -4:
                        LP_Chr4 = new Rectangle(chars[3] + offsetx, y1 + offsety, maxValue4, charsheight);
                        break;
                    case -5:
                        LP_Chr4 = new Rectangle(chars[3] + offsetx, y1 + offsety, maxValue5, charsheight);
                        break;

                    default:
                        break;
                }
                LP_Chr5 = new Rectangle();
                switch (gapSum[chars[4]])
                {
                    case -6:
                        LP_Chr5 = new Rectangle(chars[4] + offsetx, y1 + offsety, maxValue0, charsheight);
                        break;
                    case -1:
                        LP_Chr5 = new Rectangle(chars[4] + offsetx, y1 + offsety, maxValue1, charsheight);
                        break;
                    case -2:
                        LP_Chr5 = new Rectangle(chars[4] + offsetx, y1 + offsety, maxValue2, charsheight);
                        break;
                    case -3:
                        LP_Chr5 = new Rectangle(chars[4] + offsetx, y1 + offsety, maxValue3, charsheight);
                        break;
                    case -4:
                        LP_Chr5 = new Rectangle(chars[4] + offsetx, y1 + offsety, maxValue4, charsheight);
                        break;
                    case -5:
                        LP_Chr5 = new Rectangle(chars[4] + offsetx, y1 + offsety, maxValue5, charsheight);
                        break;

                    default:
                        break;
                }
                LP_Chr6 = new Rectangle();
                switch (gapSum[chars[5]])
                {
                    case -6:
                        LP_Chr6 = new Rectangle(chars[5] + offsetx, y1 + offsety, maxValue0, charsheight);
                        break;
                    case -1:
                        LP_Chr6 = new Rectangle(chars[5] + offsetx, y1 + offsety, maxValue1, charsheight);
                        break;
                    case -2:
                        LP_Chr6 = new Rectangle(chars[5] + offsetx, y1 + offsety, maxValue2, charsheight);
                        break;
                    case -3:
                        LP_Chr6 = new Rectangle(chars[5] + offsetx, y1 + offsety, maxValue3, charsheight);
                        break;
                    case -4:
                        LP_Chr6 = new Rectangle(chars[5] + offsetx, y1 + offsety, maxValue4, charsheight);
                        break;
                    case -5:
                        LP_Chr6 = new Rectangle(chars[5] + offsetx, y1+offsety, maxValue5, charsheight);
                        break;

                    default:
                        break;
                }





            }

        }

        public static void CharIden(Image<Bgr, byte> lp1, out char char1 )
        {
            int[] absdiff = new int[91];
            Image<Bgr, Byte> A = new Image<Bgr, Byte>("..\\..\\\\BD\\A.bmp");
            Image<Bgr, Byte> B = new Image<Bgr, Byte>("..\\..\\BD\\B.bmp");
            Image<Bgr, Byte> C = new Image<Bgr, Byte>("..\\..\\BD\\C.bmp");
            Image<Bgr, Byte> D = new Image<Bgr, Byte>("..\\..\\BD\\D.bmp");
            Image<Bgr, Byte> E = new Image<Bgr, Byte>("..\\..\\BD\\E.bmp");
            Image<Bgr, Byte> F = new Image<Bgr, Byte>("..\\..\\BD\\F.bmp");
            Image<Bgr, Byte> G = new Image<Bgr, Byte>("..\\..\\BD\\G.bmp");
            Image<Bgr, Byte> H = new Image<Bgr, Byte>("..\\..\\BD\\H.bmp");
            Image<Bgr, Byte> I = new Image<Bgr, Byte>("..\\..\\BD\\I.bmp");
            Image<Bgr, Byte> J = new Image<Bgr, Byte>("..\\..\\BD\\J.bmp");
            Image<Bgr, Byte> K = new Image<Bgr, Byte>("..\\..\\BD\\K.bmp");
            Image<Bgr, Byte> L = new Image<Bgr, Byte>("..\\..\\BD\\L.bmp");
            Image<Bgr, Byte> M = new Image<Bgr, Byte>("..\\..\\BD\\M.bmp");
            Image<Bgr, Byte> N = new Image<Bgr, Byte>("..\\..\\BD\\N.bmp");
            Image<Bgr, Byte> O = new Image<Bgr, Byte>("..\\..\\BD\\O.bmp");
            Image<Bgr, Byte> P = new Image<Bgr, Byte>("..\\..\\BD\\P.bmp");
            Image<Bgr, Byte> Q = new Image<Bgr, Byte>("..\\..\\BD\\Q.bmp");
            Image<Bgr, Byte> R = new Image<Bgr, Byte>("..\\..\\BD\\R.bmp");
            Image<Bgr, Byte> S = new Image<Bgr, Byte>("..\\..\\BD\\S.bmp");
            Image<Bgr, Byte> T = new Image<Bgr, Byte>("..\\..\\BD\\T.bmp");
            Image<Bgr, Byte> U = new Image<Bgr, Byte>("..\\..\\BD\\U.bmp");
            Image<Bgr, Byte> V = new Image<Bgr, Byte>("..\\..\\BD\\V.bmp");
            Image<Bgr, Byte> X = new Image<Bgr, Byte>("..\\..\\BD\\X.bmp");
            Image<Bgr, Byte> Z = new Image<Bgr, Byte>("..\\..\\BD\\Z.bmp");
            Image<Bgr, Byte> UM = new Image<Bgr, Byte>("..\\..\\BD\\1.bmp");
            Image<Bgr, Byte> DOIS = new Image<Bgr, Byte>("..\\..\\BD\\2.bmp");
            Image<Bgr, Byte> TRES = new Image<Bgr, Byte>("..\\..\\BD\\3.bmp");
            Image<Bgr, Byte> QUATRO = new Image<Bgr, Byte>("..\\..\\BD\\4.bmp");
            Image<Bgr, Byte> CINCO = new Image<Bgr, Byte>("..\\..\\BD\\5.bmp");
            Image<Bgr, Byte> SEIS = new Image<Bgr, Byte>("..\\..\\BD\\6.bmp");
            Image<Bgr, Byte> SETE = new Image<Bgr, Byte>("..\\..\\BD\\7.bmp");
            Image<Bgr, Byte> OITO = new Image<Bgr, Byte>("..\\..\\BD\\8.bmp");
            Image<Bgr, Byte> NOVE = new Image<Bgr, Byte>("..\\..\\BD\\9.bmp");
            Image<Bgr, Byte> ZERO = new Image<Bgr, Byte>("..\\..\\BD\\0.bmp");



            absdiff[65] = Compare(lp1, A);
            absdiff[66] = Compare(lp1, B);
            absdiff[67] = Compare(lp1, C);
            absdiff[68] = Compare(lp1, D);
            absdiff[69] = Compare(lp1, E);
            absdiff[70] = Compare(lp1, F);
            absdiff[71] = Compare(lp1, G);
            absdiff[72] = Compare(lp1, H);
            absdiff[73] = Compare(lp1, I);
            absdiff[74] = Compare(lp1, J);
            absdiff[75] = Compare(lp1, K);
            absdiff[76] = Compare(lp1, L);
            absdiff[77] = Compare(lp1, M);
            absdiff[78] = Compare(lp1, N);
            absdiff[79] = Compare(lp1, O);
            absdiff[80] = Compare(lp1, P);
            absdiff[81] = Compare(lp1, Q);
            absdiff[82] = Compare(lp1, R);
            absdiff[83] = Compare(lp1, S);
            absdiff[84] = Compare(lp1, T);
            absdiff[85] = Compare(lp1, U);
            absdiff[86] = Compare(lp1, V);
            absdiff[88] = Compare(lp1, X);
            absdiff[90] = Compare(lp1, Z);
            absdiff[49] = Compare(lp1, UM);
            absdiff[50] = Compare(lp1, DOIS);
            absdiff[51] = Compare(lp1, TRES);
            absdiff[52] = Compare(lp1, QUATRO);
            absdiff[53] = Compare(lp1, CINCO);
            absdiff[54] = Compare(lp1, SEIS);
            absdiff[55] = Compare(lp1, SETE);
            absdiff[56] = Compare(lp1, OITO);
            absdiff[57] = Compare(lp1, NOVE);
            absdiff[48] = Compare(lp1, ZERO);


            int max=absdiff.Max();
            int maxindex = absdiff.ToList().IndexOf(max);
            //Console.Write(((char)maxindex));
            //Console.Write(maxindex);
            char1 = (char)maxindex;






    }

        public static int Compare(Image<Bgr, byte> imag1, Image<Bgr, byte> imag2)
        {
            Image<Bgr, Byte> img01 = imag1.Copy();
            Image<Bgr, Byte> img02 = imag2.Copy();


            Image<Bgr, Byte> img1 =img01.Resize(64, 64, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
            Image<Bgr, Byte> img2 =img02.Resize(64, 64, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
            MIplImage m1 = img1.MIplImage;
            MIplImage m2 = img2.MIplImage;
            ConvertToBW_Otsu(img1);
            ConvertToBW_Otsu (img2);

            //using (img1)
            //{
            //    //Show the image
            //    CvInvoke.cvShowImage("img1", img1.Ptr);
            //    //Wait for the key pressing event
            //    CvInvoke.cvWaitKey(0);
            //    //Destory the window
            //    CvInvoke.cvDestroyWindow("img1");
            //}

            //using (img2)
            //{
            //    //Show the image
            //    CvInvoke.cvShowImage("img2", img2.Ptr);
            //    //Wait for the key pressing event
            //    CvInvoke.cvWaitKey(0);
            //    //Destory the window
            //    CvInvoke.cvDestroyWindow("img2");
            //}

            int width1 = img1.Width,height1 =img1.Height;
            int x,y,result=0;
            int nChan = m1.nChannels; // number of channels - 3
            int padding = m1.widthStep - m1.nChannels * m1.width; // alinhament bytes (padding)

            unsafe
            {
                byte* dataPtr1 = (byte*)m1.imageData.ToPointer(); // Pointer to the image
                byte* dataPtr2 = (byte*)m2.imageData.ToPointer(); // Pointer to the image
                for (y = 0; y < height1; y++)
                {
                    for (x = 0; x < width1; x++)
                    {

                        if (dataPtr1[0] == dataPtr2[0])
                            result++;




                        // advance the pointer to the next pixel
                        dataPtr1 += nChan;
                        dataPtr2 += nChan;


                    }

                    //at the end of the line advance the pointer by the aligment bytes (padding)
                    dataPtr1 += padding;
                    dataPtr2 += padding;

                }



            }
            //Console.Write(result+"\n");
            return (result);
        }

        public static void RobertsGabi(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
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
                int x, y, green, blue, red, robX = 0, robY = 0, sumRob;


                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            if (y != 0 && x != 0 && y != height - 1 && x != width - 1)
                            {

                                robX = (int)Math.Abs(Math.Round((
                                        1.0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                        0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) +
                                        0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                        -1 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]));

                                robY = (int)Math.Abs(Math.Round((
                                        0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                        1 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) +
                                        -1.0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                        0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]));

                                if (robX + robY > 255)
                                    sumRob = 255;
                                else
                                    sumRob = robX + robY;
                                dataPtr[0] = (byte)sumRob;

                                robX = (int)Math.Abs(Math.Round((
                                        1.0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                        0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]) +
                                        0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                        -1 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]));

                                robY = (int)Math.Abs(Math.Round((
                                        0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                        1 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]) +
                                        -1.0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                        0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]));

                                if (robX + robY > 255)
                                    sumRob = 255;
                                else
                                    sumRob = robX + robY;
                                dataPtr[1] = (byte)sumRob;

                                robX = (int)Math.Abs(Math.Round((
                                         1.0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                         0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]) +
                                         0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                         -1 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]));

                                robY = (int)Math.Abs(Math.Round((
                                        0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                        1 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]) +
                                        -1.0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                        0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]));

                                if (robX + robY > 255)
                                    sumRob = 255;
                                else
                                    sumRob = robX + robY;
                                dataPtr[2] = (byte)sumRob;

                            }

                            if (y == 0) //primeira linha
                            {
                                if (x != width && x != 0) //excluindo os cantos
                                {
                                    robX = (int)Math.Abs(Math.Round((
                                           1.0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                           0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) +
                                           -1 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                           0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]));

                                    robY = (int)Math.Abs(Math.Round((
                                            0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                            1 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) +
                                            -1.0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                            0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]));

                                    if (robX + robY > 255)
                                        sumRob = 255;
                                    else
                                        sumRob = robX + robY;
                                    dataPtr[0] = (byte)sumRob;

                                    robX = (int)Math.Abs(Math.Round((
                                               1.0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                               0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]) +
                                               -1 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                               0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]));

                                    robY = (int)Math.Abs(Math.Round((
                                             0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                            1 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]) +
                                            -1.0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                            0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]));

                                    if (robX + robY > 255)
                                        sumRob = 255;
                                    else
                                        sumRob = robX + robY;
                                    dataPtr[1] = (byte)sumRob;

                                    robX = (int)Math.Abs(Math.Round((
                                               1.0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                               0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]) +
                                               -1 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                               0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]));

                                    robY = (int)Math.Abs(Math.Round((
                                            0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                            1 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]) +
                                            -1.0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                            0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]));

                                    if (robX + robY > 255)
                                        sumRob = 255;
                                    else
                                        sumRob = robX + robY;
                                    dataPtr[2] = (byte)sumRob;



                                }
                                if (x == 0)
                                {
                                    robX = (int)Math.Abs(Math.Round((
                                               1.0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                               0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) +
                                               -1 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                               0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]));

                                    robY = (int)Math.Abs(Math.Round((
                                            1 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                            0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) +
                                            -1.0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                            0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]));

                                    if (robX + robY > 255)
                                        sumRob = 255;
                                    else
                                        sumRob = robX + robY;
                                    dataPtr[0] = (byte)sumRob;

                                    robX = (int)Math.Abs(Math.Round((
                                               1.0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                               0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]) +
                                               -1 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                               0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]));

                                    robY = (int)Math.Abs(Math.Round((
                                            1 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                            0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]) +
                                            -1.0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                            0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]));

                                    if (robX + robY > 255)
                                        sumRob = 255;
                                    else
                                        sumRob = robX + robY;
                                    dataPtr[1] = (byte)sumRob;

                                    robX = (int)Math.Abs(Math.Round((
                                               1.0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                               0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]) +
                                               -1 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                               0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]));

                                    robY = (int)Math.Abs(Math.Round((
                                            1 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                            0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]) +
                                            -1.0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                            0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]));

                                    if (robX + robY > 255)
                                        sumRob = 255;
                                    else
                                        sumRob = robX + robY;
                                    dataPtr[2] = (byte)sumRob;

                                }
                            }
                            if (x == 0)//primeira coluna
                            {
                                if (y != 0 && y != height)
                                {
                                    robX = (int)Math.Abs(Math.Round((
                                               1.0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                               0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) +
                                               -1 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                               0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]));

                                    robY = (int)Math.Abs(Math.Round((
                                            1 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                            0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) +
                                            -1.0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                            0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]));

                                    if (robX + robY > 255)
                                        sumRob = 255;
                                    else
                                        sumRob = robX + robY;
                                    dataPtr[0] = (byte)sumRob;

                                    robX = (int)Math.Abs(Math.Round((
                                               1.0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                               0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]) +
                                               -1 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                               0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]));

                                    robY = (int)Math.Abs(Math.Round((
                                            1 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                            0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]) +
                                            -1.0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                            0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]));

                                    if (robX + robY > 255)
                                        sumRob = 255;
                                    else
                                        sumRob = robX + robY;
                                    dataPtr[1] = (byte)sumRob;

                                    robX = (int)Math.Abs(Math.Round((
                                               1.0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                               0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]) +
                                               -1 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                               0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]));

                                    robY = (int)Math.Abs(Math.Round((
                                            1 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                            0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]) +
                                            -1.0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                            0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]));

                                    if (robX + robY > 255)
                                        sumRob = 255;
                                    else
                                        sumRob = robX + robY;
                                    dataPtr[2] = (byte)sumRob;

                                }
                            }
                            if (y == height) //ultima linha
                            {
                                if (x != 0 && y != height) //excluir os cantos
                                {
                                    robX = (int)Math.Abs(Math.Round((
                                              0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                              0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) +
                                              1.0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                              -1 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]));

                                    robY = (int)Math.Abs(Math.Round((
                                            0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                            0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) +
                                            1.0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                            -1 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]));

                                    if (robX + robY > 255)
                                        sumRob = 255;
                                    else
                                        sumRob = robX + robY;
                                    dataPtr[0] = (byte)sumRob;

                                    robX = (int)Math.Abs(Math.Round((
                                               0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                               0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) +
                                               1.0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                               -1 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]));

                                    robY = (int)Math.Abs(Math.Round((
                                            0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                            0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]) +
                                            1.0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                            -1 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]));

                                    if (robX + robY > 255)
                                        sumRob = 255;
                                    else
                                        sumRob = robX + robY;
                                    dataPtr[1] = (byte)sumRob;

                                    robX = (int)Math.Abs(Math.Round((
                                              0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                              0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]) +
                                              1.0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                              -1 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]));

                                    robY = (int)Math.Abs(Math.Round((
                                            0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                            0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]) +
                                            1.0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                            -1 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]));

                                    if (robX + robY > 255)
                                        sumRob = 255;
                                    else
                                        sumRob = robX + robY;
                                    dataPtr[2] = (byte)sumRob;
                                }
                                if (x == 0) //canto inferior esquerdo
                                {
                                    robX = (int)Math.Abs(Math.Round((
                                              0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                              0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) +
                                              1.0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                              -1 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0]));

                                    robY = (int)Math.Abs(Math.Round((
                                            0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                            0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) +
                                            1.0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                            -1 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0]));

                                    if (robX + robY > 255)
                                        sumRob = 255;
                                    else
                                        sumRob = robX + robY;
                                    dataPtr[0] = (byte)sumRob;

                                    robX = (int)Math.Abs(Math.Round((
                                               0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                               0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) +
                                               1.0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                               -1 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0]));

                                    robY = (int)Math.Abs(Math.Round((
                                            0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                            0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]) +
                                            1.0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                            -1 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1]));

                                    if (robX + robY > 255)
                                        sumRob = 255;
                                    else
                                        sumRob = robX + robY;
                                    dataPtr[1] = (byte)sumRob;

                                    robX = (int)Math.Abs(Math.Round((
                                              0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                              0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]) +
                                              1.0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                              -1 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2]));

                                    robY = (int)Math.Abs(Math.Round((
                                            0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                            0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]) +
                                            1.0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                            -1 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2]));

                                    if (robX + robY > 255)
                                        sumRob = 255;
                                    else
                                        sumRob = robX + robY;
                                    dataPtr[2] = (byte)sumRob;
                                }
                                if (x == width) //canto inferior direito
                                {
                                    robX = (int)Math.Abs(Math.Round((
                                              0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                              0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) +
                                              1.0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0] +
                                              -1 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]));

                                    robY = (int)Math.Abs(Math.Round((
                                            0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                            0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) +
                                            1.0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0] +
                                            -1 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]));

                                    if (robX + robY > 255)
                                        sumRob = 255;
                                    else
                                        sumRob = robX + robY;
                                    dataPtr[0] = (byte)sumRob;

                                    robX = (int)Math.Abs(Math.Round((
                                               0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                               0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) +
                                               1.0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0] +
                                               -1 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]));

                                    robY = (int)Math.Abs(Math.Round((
                                            0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                            0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]) +
                                            1.0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1] +
                                            -1 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]));

                                    if (robX + robY > 255)
                                        sumRob = 255;
                                    else
                                        sumRob = robX + robY;
                                    dataPtr[1] = (byte)sumRob;

                                    robX = (int)Math.Abs(Math.Round((
                                              0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                              0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]) +
                                              1.0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2] +
                                              -1 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]));

                                    robY = (int)Math.Abs(Math.Round((
                                            0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                            0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]) +
                                            1.0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2] +
                                            -1 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]));

                                    if (robX + robY > 255)
                                        sumRob = 255;
                                    else
                                        sumRob = robX + robY;
                                    dataPtr[2] = (byte)sumRob;

                                }
                            }
                            if (x == width) //ultima coluna
                            {
                                if (y != 0 && y != height) //excluir os cantos
                                {
                                    robX = (int)Math.Abs(Math.Round((
                                             0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                             1.0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) +
                                             0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0] +
                                             -1 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]));

                                    robY = (int)Math.Abs(Math.Round((
                                            0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                            1.0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) +
                                            0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0] +
                                            -1 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]));

                                    if (robX + robY > 255)
                                        sumRob = 255;
                                    else
                                        sumRob = robX + robY;
                                    dataPtr[0] = (byte)sumRob;

                                    robX = (int)Math.Abs(Math.Round((
                                               0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                               1.0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) +
                                               0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0] +
                                               -1 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]));

                                    robY = (int)Math.Abs(Math.Round((
                                            0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                            1.0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]) +
                                            0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1] +
                                            -1 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]));

                                    if (robX + robY > 255)
                                        sumRob = 255;
                                    else
                                        sumRob = robX + robY;
                                    dataPtr[1] = (byte)sumRob;

                                    robX = (int)Math.Abs(Math.Round((
                                              0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                              1.0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]) +
                                              0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2] +
                                              -1 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]));

                                    robY = (int)Math.Abs(Math.Round((
                                            0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                            1.0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]) +
                                            0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2] +
                                            -1 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]));

                                    if (robX + robY > 255)
                                        sumRob = 255;
                                    else
                                        sumRob = robX + robY;
                                    dataPtr[2] = (byte)sumRob;
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

        public static void Roberts(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
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
                int x, y, green, blue, red, robX=0, robY=0, sumRob;


                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            if (y != 0 && x != 0 && y != height - 1 && x != width - 1)
                            {

                                robX = (int)Math.Abs(Math.Round((
                                1.0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) +
                                0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                -1 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]));

                                robY = (int)Math.Abs(Math.Round((
                                0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                1 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) +
                                -1.0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]));

                                if (robX + robY > 255)
                                    sumRob = 255;
                                else
                                    sumRob = robX + robY;
                                dataPtr[0] = (byte)sumRob;

                    robX = (int)Math.Abs(Math.Round((
                    1.0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                    0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]) +
                    0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                    -1 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]));

                    robY = (int)Math.Abs(Math.Round((
                    0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                    1 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]) +
                    -1.0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                    0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]));

                    if (robX + robY > 255)
                                    sumRob = 255;
                                else
                                    sumRob = robX + robY;
                                dataPtr[1] = (byte)sumRob;

                                robX = (int)Math.Abs(Math.Round((
                                 1.0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                 0 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]) +
                                 0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                 -1 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]));

                                robY = (int)Math.Abs(Math.Round((
                                0 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                1 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]) +
                                -1.0 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                0 * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]));

                                if (robX + robY > 255)
                                    sumRob = 255;
                                else
                                    sumRob = robX + robY;
                                dataPtr[2] = (byte)sumRob;

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

        //TODO:

        //public static void Mean_solutionB(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy) {


        //}

        //public static void Mean_solutionC(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, int size)
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

