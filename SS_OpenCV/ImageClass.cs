using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Windows.Forms;

namespace SS_OpenCV
{
    class ImageClass
    {

        /// <summary>
        /// Image Negative using EmguCV library
        /// Slower method
        /// </summary>
        /// <param name="img">Image</param>
        internal static void Negative(Image<Bgr, byte> img)
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


        /// <summary>
        /// Convert to gray
        /// Direct access to memory
        /// </summary>
        /// <param name="img">image</param>
        internal static void ConvertToGray(Image<Bgr, byte> img)
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

        internal static void translation(Image<Bgr, byte> img, Image<Bgr, byte> imgUndo)
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
                int x, y, x_orig, y_orig;
                int movx = -900, movy = -900;


                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {

                        for (x = 0; x < width; x++)
                        {

                            if (x < movx || width + movx < x || y < movy || height + movy < y)
                            {
                                dataPtr[0] = (byte)0;
                                dataPtr[1] = (byte)0;
                                dataPtr[2] = (byte)0;
                            }

                            else
                            {
                                x_orig = x - movx;
                                y_orig = y - movy;

                                if (x_orig < width && x_orig >= 0 && y_orig < height && y_orig >= 0)
                                {
                                    //obtém as 3 componentes
                                    dataPtr[0] = (byte)(dataPtrUndo + y_orig * m.widthStep + x_orig * nChan)[0];
                                    dataPtr[1] = (byte)(dataPtrUndo + y_orig * m.widthStep + x_orig * nChan)[1];
                                    dataPtr[2] = (byte)(dataPtrUndo + y_orig * m.widthStep + x_orig * nChan)[2];
                                }
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

        internal static void meanFilter(Image<Bgr, byte> img, Image<Bgr, byte> imgUndo)
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

        internal static void filNUnif(Image<Bgr, byte> img, Image<Bgr, byte> imgUndo, int[] weight, int weight_factor)
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

                                dataPtr[0] = (byte)((weight[0] * (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] +
                                weight[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                weight[2] * (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] +
                                weight[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] +
                                weight[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                weight[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                weight[6] * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] +
                                weight[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                weight[8] * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]) / weight_factor);

                                dataPtr[1] = (byte)((weight[0] * (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] +
                                weight[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                weight[2] * (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] +
                                weight[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
                                weight[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                weight[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                weight[6] * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] +
                                weight[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                weight[8] * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]) / weight_factor);

                                dataPtr[2] = (byte)((weight[0] * (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2] +
                                weight[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                weight[2] * (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] +
                                weight[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
                                weight[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                weight[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                weight[6] * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] +
                                weight[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                weight[8] * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]) / weight_factor);


                            }

                            if (y == 0) //primeira linha
                            {
                                if (x != 0 && x != width) //excluir os cantos
                                {
                                    dataPtr[0] = (byte)((
                                    weight[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] +
                                    weight[0] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] +
                                    weight[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    weight[1] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    weight[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                    weight[2] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                    weight[6] * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] +
                                    weight[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                    weight[8] * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]) / weight_factor);

                                    dataPtr[1] = (byte)((
                                    weight[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
                                    weight[0] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
                                    weight[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    weight[1] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    weight[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                    weight[2] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                    weight[6] * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] +
                                    weight[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                    weight[8] * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]) / weight_factor);

                                    dataPtr[2] = (byte)((
                                    weight[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
                                    weight[0] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
                                    weight[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    weight[1] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    weight[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                    weight[2] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                    weight[6] * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] +
                                    weight[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                    weight[8] * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]) / weight_factor);

                                }

                                if (x == 0)//canto sup esq
                                {
                                    dataPtr[0] = (byte)((
                                    weight[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    weight[0] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    weight[1] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    weight[2] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    weight[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                    weight[3] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                    weight[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                    weight[6] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                    weight[8] * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]) / weight_factor);

                                    dataPtr[1] = (byte)((
                                    weight[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    weight[0] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    weight[1] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    weight[2] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    weight[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                    weight[3] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                    weight[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                    weight[6] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                    weight[8] * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]) / weight_factor);

                                    dataPtr[2] = (byte)((
                                    weight[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    weight[0] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    weight[1] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    weight[2] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    weight[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                    weight[3] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                    weight[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                    weight[6] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                    weight[8] * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]) / weight_factor);
                                }

                                if (x == width)//canto sup esq
                                {
                                    dataPtr[0] = (byte)((
                                    weight[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    weight[0] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    weight[1] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    weight[2] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    weight[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] +
                                    weight[5] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] +
                                    weight[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                    weight[6] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                    weight[8] * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0]) / weight_factor);

                                    dataPtr[1] = (byte)((
                                     weight[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    weight[0] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    weight[1] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    weight[2] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    weight[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
                                    weight[5] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
                                    weight[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                    weight[6] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                    weight[8] * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1]) / weight_factor);

                                    dataPtr[2] = (byte)((
                                    weight[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    weight[0] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    weight[1] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    weight[2] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    weight[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
                                    weight[5] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
                                    weight[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                    weight[6] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                    weight[8] * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2]) / weight_factor);
                                }
                            }

                            if (x == 0) //primeira coluna
                            {
                                if (y != 0 && y != height) //excluir o canto
                                {
                                    dataPtr[0] = (byte)((
                                    weight[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                    weight[0] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                    weight[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    weight[3] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    weight[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                    weight[2] * (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] +
                                    weight[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                    weight[6] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                    weight[8] * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]) / weight_factor);

                                    dataPtr[1] = (byte)((
                                    weight[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                    weight[0] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                    weight[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    weight[3] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    weight[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                    weight[2] * (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] +
                                    weight[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                    weight[6] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                    weight[8] * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]) / weight_factor);

                                    dataPtr[2] = (byte)((
                                    weight[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                    weight[0] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                    weight[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    weight[3] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    weight[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                    weight[2] * (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] +
                                    weight[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                    weight[6] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                    weight[8] * (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]) / weight_factor);

                                }
                            }

                            if (y == height) //ultima linha
                            {
                                if (x != 0 && y != height) //excluir os cantos
                                {
                                    dataPtr[0] = (byte)((
                                    weight[0] * (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] +
                                    weight[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                    weight[2] * (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] +
                                    weight[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] +
                                    weight[6] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] +
                                    weight[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    weight[7] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    weight[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                    weight[8] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) / weight_factor);

                                    dataPtr[1] = (byte)((
                                    weight[0] * (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] +
                                    weight[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                    weight[2] * (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] +
                                    weight[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
                                    weight[6] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
                                    weight[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    weight[7] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    weight[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                    weight[8] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]) / weight_factor);

                                    dataPtr[2] = (byte)((
                                   weight[0] * (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2] +
                                    weight[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                    weight[2] * (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] +
                                    weight[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
                                    weight[6] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
                                    weight[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    weight[7] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    weight[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                    weight[8] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]) / weight_factor);

                                }
                                if (x == 0) //canto inferior esquerdo
                                {
                                    dataPtr[0] = (byte)((
                                    weight[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                    weight[0] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                    weight[2] * (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] +
                                    weight[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    weight[3] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    weight[6] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    weight[7] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    weight[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                    weight[8] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) / weight_factor);

                                    dataPtr[1] = (byte)((
                                    weight[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                    weight[0] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                    weight[2] * (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] +
                                    weight[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    weight[3] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    weight[6] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    weight[7] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    weight[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                    weight[8] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]) / weight_factor);


                                    dataPtr[2] = (byte)((
                                    weight[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                    weight[0] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                    weight[2] * (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] +
                                    weight[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    weight[3] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    weight[6] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    weight[7] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    weight[5] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                    weight[8] * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]) / weight_factor);

                                }
                                if (x == width) //canto inferior direito
                                {
                                    dataPtr[0] = (byte)((
                                    weight[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                    weight[2] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                    weight[0] * (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] +
                                    weight[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    weight[5] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    weight[6] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    weight[7] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    weight[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] +
                                    weight[8] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0]) / weight_factor);

                                    dataPtr[1] = (byte)((
                                    weight[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                    weight[2] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                    weight[0] * (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] +
                                    weight[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    weight[5] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    weight[6] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    weight[7] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    weight[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
                                    weight[8] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1]) / weight_factor);


                                    dataPtr[2] = (byte)((
                                    weight[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                    weight[2] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                    weight[0] * (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2] +
                                    weight[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    weight[5] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    weight[6] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    weight[7] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    weight[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
                                    weight[8] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2]) / weight_factor);

                                }
                            }
                            if (x == width) //ultima coluna
                            {
                                if (y != 0 && y != height) //excluir os cantos
                                {

                                    dataPtr[0] = (byte)((
                                    weight[0] * (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] +
                                    weight[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                    weight[2] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                    weight[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] +
                                    weight[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    weight[5] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    weight[6] * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] +
                                    weight[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] +
                                    weight[8] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0]) / weight_factor);

                                    dataPtr[1] = (byte)((
                                    weight[0] * (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] +
                                    weight[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                    weight[2] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                    weight[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
                                    weight[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    weight[5] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    weight[6] * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] +
                                    weight[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] +
                                    weight[8] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1]) / weight_factor);

                                    dataPtr[2] = (byte)((
                                    weight[0] * (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2] +
                                    weight[1] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                    weight[2] * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                    weight[3] * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
                                    weight[4] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    weight[5] * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    weight[6] * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] +
                                    weight[7] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] +
                                    weight[8] * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2]) / weight_factor);
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

        internal static void mediana(Image<Bgr, byte> img, Image<Bgr, byte> imgUndo)
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

        internal static void Histogram(Image<Bgr, byte> img, Image<Bgr, byte> imgUndo)
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

                int[] blue_array = new int[256];
                int[] red_array = new int[256];
                int[] green_array = new int[256];
                int[] media_array = new int[256];

                //iniciar os vectores a zero
                Array.Clear(red_array, 0, red_array.Length);
                Array.Clear(blue_array, 0, blue_array.Length);
                Array.Clear(green_array, 0, green_array.Length);
                Array.Clear(media_array, 0, media_array.Length);

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            int blue = 0;
                            int red = 0;
                            int green = 0;
                            int media = 0;

                            blue = (int)(dataPtr[0]);
                            red = (int)(dataPtr[1]);
                            green = (int)(dataPtr[2]);
                            media = (blue + red + green) / 3;

                            blue_array[blue]++;
                            red_array[red]++;
                            green_array[green]++;
                            media_array[media]++;


                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                            dataPtrUndo += nChan;

                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                        dataPtrUndo += padding;

                    }
                }
                Histograma_window Histrograma_form = new Histograma_window(media_array);
                Histrograma_form.ShowDialog();
            }
        }

        internal static void sobel_filter(Image<Bgr, byte> img, Image<Bgr, byte> imgUndo)
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

                                dataPtr[0] = (byte)((
                                (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] +
                                2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] -
                                (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] -
                                2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] -
                                (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]) +
                                ((dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] +
                                2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0] -
                                (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] -
                                2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] -
                                (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0]));

                                dataPtr[1] = (byte)((
                                 (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] +
                                2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
                                (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] -
                                (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] -
                                2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] -
                                (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]) +
                                ((dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] +
                                2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1] -
                                (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] -
                                2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] -
                                (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1]));

                                dataPtr[2] = (byte)((
                                (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2] +
                                2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
                                (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] -
                                (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] -
                                2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] -
                                (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]) +
                                ((dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] +
                                2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2] -
                                (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2] -
                                2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] -
                                (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2]));

                            }

                            if (y == 0) //primeira linha
                            {
                                if (x != 0 && x != width) //excluir os cantos
                                {
                                    dataPtr[0] = (byte)((
                                    (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                    2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                    (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] -
                                    2 * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] -
                                    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]) +
                                    ((dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0] -
                                    (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] -
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0]));

                                    dataPtr[1] = (byte)((
                                    (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                    2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                    (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] -
                                    2 * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] -
                                    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]) +
                                    ((dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1] -
                                    (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] -
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1]));

                                    dataPtr[2] = (byte)((
                                    (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                    2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                    (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] -
                                    2 * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] -
                                    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]) +
                                    ((dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2] -
                                    (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] -
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2]));

                                }

                                if (x == 0)//canto sup esq
                                {
                                    dataPtr[0] = (byte)((
                                    (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] -
                                    2 * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] -
                                    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]) +
                                    ((dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0] -
                                    (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] -
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0]));

                                    dataPtr[1] = (byte)((
                                     (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] -
                                    2 * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] -
                                    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]) +
                                    ((dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1] -
                                    (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] -
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1]));

                                    dataPtr[2] = (byte)((
                                     (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] -
                                    2 * (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] -
                                    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]) +
                                    ((dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2] -
                                    (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] -
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2]));
                                }

                                if (x == width)//canto sup dir
                                {
                                    dataPtr[0] = (byte)((
                                    (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                    2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                    (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] -
                                    (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] -
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] -
                                    (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) +
                                    ((dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                    (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] -
                                    (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] -
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] -
                                    (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0]));

                                    dataPtr[1] = (byte)((
                                   (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                    2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                    (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] -
                                    (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] -
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] -
                                    (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]) +
                                    ((dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                    (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] -
                                    (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] -
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] -
                                    (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1]));

                                    dataPtr[2] = (byte)((
                                   (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                    2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                    (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] -
                                    (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] -
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] -
                                    (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]) +
                                    ((dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                    (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] -
                                    (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] -
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] -
                                    (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2]));
                                }
                            }

                            if (x == 0) //primeira coluna
                            {
                                if (y != 0 && y != height) //excluir o canto
                                {
                                    dataPtr[0] = (byte)((
                                    (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] -
                                    2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] -
                                    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0]) +
                                    ((dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[0] -
                                    (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] -
                                    2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0]));

                                    dataPtr[1] = (byte)((
                                   (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] -
                                    2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] -
                                    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1]) +
                                    ((dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[1] -
                                    (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] -
                                    2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1]));

                                    dataPtr[2] = (byte)((
                                   (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] -
                                    2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] -
                                    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2]) +
                                    ((dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                    (dataPtrUndo + (1) * m.widthStep + (1) * nChan)[2] -
                                    (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] -
                                    2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2]));

                                }
                            }

                            if (y == height) //ultima linha
                            {
                                if (x != 0 && y != height) //excluir os cantos
                                {
                                    dataPtr[0] = (byte)((
                                    (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] +
                                    2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                    (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] -
                                    2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] -
                                    (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0]) +
                                    ((dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] -
                                    (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] -
                                    2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0]));

                                    dataPtr[1] = (byte)((
                                    (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] +
                                    2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                    (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] -
                                    2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] -
                                    (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1]) +
                                    ((dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] -
                                    (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] -
                                    2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1]));

                                    dataPtr[2] = (byte)((
                                   (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2] +
                                    2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                    (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] -
                                    2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] -
                                    (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2]) +
                                    ((dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] -
                                    (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2] -
                                    2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2]));

                                }
                                if (x == 0) //canto inferior esquerdo
                                {
                                    dataPtr[0] = (byte)((
                                    (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] -
                                    2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] -
                                    (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0]) +
                                    ((dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[0] -
                                    (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] -
                                    2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0]));

                                    dataPtr[1] = (byte)((
                                    (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] -
                                    2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] -
                                    (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1]) +
                                    ((dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[1] -
                                    (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] -
                                    2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1]));


                                    dataPtr[2] = (byte)((
                                    (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] -
                                    2 * (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] -
                                    (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2]) +
                                    ((dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    (dataPtrUndo + (1) * m.widthStep + (0) * nChan)[2] -
                                    (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] -
                                    2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2]));

                                }
                                if (x == width) //canto inferior direito
                                {
                                    dataPtr[0] = (byte)((
                                    (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] +
                                    2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                    (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] -
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] -
                                    (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0]) +
                                    ((dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] +
                                    (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] -
                                    (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] -
                                    2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] -
                                    (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0]));

                                    dataPtr[1] = (byte)((
                                    (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] +
                                    2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                    (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] -
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] -
                                    (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1]) +
                                    ((dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] +
                                    (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] -
                                    (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] -
                                    2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] -
                                    (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1]));


                                    dataPtr[2] = (byte)((
                                    (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2] +
                                    2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                    (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] -
                                    (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] -
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] -
                                    (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2]) +
                                    ((dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] +
                                    2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] +
                                    (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] -
                                    (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2] -
                                    2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] -
                                    (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2]));

                                }
                            }
                            if (x == width) //ultima coluna
                            {
                                if (y != 0 && y != height) //excluir os cantos
                                {

                                    dataPtr[0] = (byte)((
                                                (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] +
                                                2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[0] +
                                                (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] -
                                                (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[0] -
                                                2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[0] -
                                                (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0]) +
                                                ((dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[0] +
                                                2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] +
                                                (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[0] -
                                                (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[0] -
                                                2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0] -
                                                (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[0]));

                                    dataPtr[1] = (byte)((
                                                (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] +
                                                2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[1] +
                                                (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] -
                                                (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[1] -
                                                2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[1] -
                                                (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1]) +
                                                ((dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[1] +
                                                2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] +
                                                (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[1] -
                                                (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[1] -
                                                2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1] -
                                                (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[1]));

                                    dataPtr[2] = (byte)((
                                                (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2] +
                                                2 * (dataPtrUndo + (-1) * m.widthStep + (0) * nChan)[2] +
                                                (dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] -
                                                (dataPtrUndo + (1) * m.widthStep + (-1) * nChan)[2] -
                                                2 * (dataPtrUndo + (0) * m.widthStep + (0) * nChan)[2] -
                                                (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2]) +
                                                ((dataPtrUndo + (-1) * m.widthStep + (1) * nChan)[2] +
                                                2 * (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] +
                                                (dataPtrUndo + (0) * m.widthStep + (1) * nChan)[2] -
                                                (dataPtrUndo + (-1) * m.widthStep + (-1) * nChan)[2] -
                                                2 * (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2] -
                                                (dataPtrUndo + (0) * m.widthStep + (-1) * nChan)[2]));
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

        internal static void diferential_filter(Image<Bgr, byte> img, Image<Bgr, byte> imgUndo)
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

        internal static void Manual_binarization(Image<Bgr, byte> img, Image<Bgr, byte> imgUndo, int threshold)

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
                byte media;


                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            int blue = (int)dataPtr[0];
                            int red = (int)dataPtr[1];
                            int green = (int)dataPtr[2];

                            media = (byte)((blue + red + green) / 3);

                            if (media < threshold)
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
                            dataPtrUndo += nChan;

                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                        dataPtrUndo += padding;

                    }
                }

            }
        }

        internal static void Otsu(Image<Bgr, byte> img, Image<Bgr, byte> imgUndo)

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

                int x, y, threshold, gray, threshold_out;
                double q1, q2, m1, m2;
                int[] hist = new int[256];
                //MessageBox alert = new MessageBox(1);

                //iniciar os vectores a zero
                Array.Clear(hist, 0, hist.Length);

                double vari = 0;


                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            int blue = (int)dataPtr[0];
                            int red = (int)dataPtr[1];
                            int green = (int)dataPtr[2];

                            gray = ((blue + red + green) / 3);

                            hist[gray]++;



                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                            dataPtrUndo += nChan;

                        }
                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                        dataPtrUndo += padding;

                    }
                }

                threshold_out = 0;

                for (threshold = 0; threshold < 255; threshold++)
                {
                    q1 = 0;
                    q2 = 0;
                    m1 = 0;
                    m2 = 0;
                    for (int i = 0; i == 255; i++)
                    {
                        Console.Write("TESTE»»»»»»»\n:" + hist[i] );

                        if (hist[i] <= threshold)
                        {
                            Console.Write("TESTE»»»»»»»\nq1:");

                           q1 = q1 + hist[i] / 256;
                            m1 = m1 + (hist[i] / 256) * i;

                        }
                        if (hist[i] > threshold)
                        {
                            q2 = q2 + hist[i] / 256;
                            m2 = m2 + (hist[i] / 256) * i;

                        }

                    }
                    m1 = m1 / q1;
                    m2 = m2 / q2;
                    //Console.Write("TESTE»»»»»»»\nq1:" +q1+"\nq2:"+q2+ "\nm1:" +m1+ "\nm2:" + m2);

                    if (q1 * q2 * Math.Pow((m1 - m2), 2) < vari)
                    {
                        threshold_out = threshold;
                        vari = q1 * q2 * Math.Pow((m1 - m2), 2);

                    }


                }
                //MessageBox.Show("Chosen Threshold:"+ threshold_out);
                //Console.Write("TESTE»»»»»»»»" + threshold_out);
                Manual_binarization(img, imgUndo, threshold_out);

            }


        }

    }



}

