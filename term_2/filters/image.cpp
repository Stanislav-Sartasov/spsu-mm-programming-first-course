#include "image.h"

using namespace std;

void Image::medPix(int z)
{
    unsigned char matrix[9];
    for (unsigned int i = 0; i + 2 < image.len; i += 3)
        for (unsigned int j = 0; j + 2 < image.wid; j += 3)
        {
            unsigned char temp;
            for (unsigned int u = 0; u < 3; ++u)
                for (unsigned int o = 0; o < 3; ++o)
                    matrix[o + 3 * u] = image.bits[i + u][j + o].a[z];
            for (int h = 0; h < 5; ++h)
                for (int g = h + 1; g < 9; ++g)
                    if (matrix[h] > matrix[g])
                    {
                        temp = matrix[h];
                        matrix[h] = matrix[g];
                        matrix[g] = temp;
                    }
            for (unsigned int u = 0; u < 3; ++u)
                for (unsigned int o = 0; o < 3; ++o)
                    image.bits[i + u][j + o].a[z] = matrix[4];
        }
}

struct pix<long> Image::goMatrix(int i, int j)
{
    long long m, n;
    struct pix<long> temp;
    temp.a[0] = 0;
    temp.a[1] = 0;
    temp.a[2] = 0;
    for (int t = -((xMatr - 1) / 2); t <= (xMatr - 1) / 2; ++t)
        for (int y = -((xMatr - 1) / 2); y <= (xMatr - 1) / 2; ++y)
        {
            //m = *(*( matrix +t+1) + y+1);
            m = (long long)i + t;
            n = (long long)j + y;

            if ((m > 0) && (n > 0) && (m < image.len) && (n < image.wid))
            {
                temp.a[0] += image.bits[m][n].a[0] * matrix[t + 1][y + 1];
                temp.a[1] += image.bits[m][n].a[1] * matrix[t + 1][y + 1];
                temp.a[2] += image.bits[m][n].a[2] * matrix[t + 1][y + 1];
            }
        }
    return temp;
}

Image::Image()
{

}

Image::~Image()
{
 for (int i = 0; i < xMatr; ++i)
     delete [] matrix[i];

 if ((palete != NULL) && (palete != nullptr))
     delete [] palete;

 for (long long i = 0; i < image.len; ++i)
     delete [] image.bits[i];

}

void Image::createMatrix(int k)
{
    if (xMatr != 0)
    {
        for (int i = 0; i < xMatr; ++i)
            delete [] matrix[i];
    }
    xMatr = k;
    matrix = new int* [xMatr];
    for (int i = 0; i < xMatr; ++i)
        matrix[i] = new int [xMatr];
}

void Image::openBMP(char *str)
{
    ifstream fIn;
    fIn.open(str, ios::binary);

    fIn.read((char*)&bmpFileH, sizeof(bmpFileH));
    fIn.read((char*)&bmpInfoH, sizeof(bmpInfoH));

    if ((bmpInfoH.biBitCount != 24) && (bmpInfoH.biBitCount != 32))
    {
        return;
    }
    palete = NULL;

    paleteSize = bmpFileH.bfOffBits - sizeof (struct BITMAPFILEHEADER) - sizeof (struct BITMAPINFOHEADER);

    if (paleteSize != 0)
    {
        palete = new char [paleteSize];
        fIn.read((char*)&palete, paleteSize);
        //fread(palete, *paleteSize, 1, fIn);
    }
    //struct pix **bits = (struct pix **)malloc(sizeof (struct pix *) * bmpInfoH.biHeight);
    struct pix<unsigned char> **bits = new struct pix<unsigned char>* [bmpInfoH.biHeight];
    for (unsigned int i = 0; i < bmpInfoH.biHeight; ++i)
        bits[i] = new struct pix<unsigned char> [sizeof (struct pix<unsigned char>) * bmpInfoH.biWidth];
        //bits[i] = (struct pix *)malloc(sizeof (struct pix) * bmpInfoH->biWidth);

    for (unsigned int i = 0; i < bmpInfoH.biHeight; ++i)
        for (unsigned int j = 0; j < bmpInfoH.biWidth; ++j)
            for (int k = 0; k < 3; ++k)
                bits[i][j].a[k] = 0;



   padLine = (4 - (bmpInfoH.biWidth * (bmpInfoH.biBitCount / 8)) % 4) & 3;

   for (unsigned int i = 0; i < bmpInfoH.biHeight; ++i)
   {
      for (unsigned int j = 0; j < bmpInfoH.biWidth; ++j)
      {
          for (int k = 0; k < 3; ++k)
              fIn.read((char*)&bits[i][j].a[k], 1);
              //bits[i][j].a[k] = (unsigned char)getc(fIn);

          if (bmpInfoH.biBitCount == 32)
          {
              fIn.get();
              //getc(fIn);
          }
      }
      for (unsigned int k = 0; k < padLine; k++)
      {
          fIn.get();
          //getc(fIn);
      }
   }
   //image = (struct img*)malloc(sizeof (struct img));
   image.bits = bits;
   image.wid = bmpInfoH.biWidth;
   image.len = bmpInfoH.biHeight;

}

void Image::closeBMP(char *str)
{
    ofstream fOut;
    fOut.open(str, ios::binary);
    fOut.write((char*)&bmpFileH, sizeof (bmpFileH));
    fOut.write((char*)&bmpInfoH, sizeof (bmpInfoH));
    //fwrite(&bmpFileH, sizeof (bmpFileH), 1, fOut);
    //fwrite(&bmpInfoH, sizeof (bmpInfoH), 1, fOut);

        if (paleteSize != 0)
        {
            fOut.write(palete, paleteSize);
            //fwrite(palete, paleteSize, 1, fOut);
        }

        for (unsigned int i = 0; i < bmpInfoH.biHeight; ++i)
        {
            for (unsigned int j = 0; j < bmpInfoH.biWidth; ++j)
            {
                for (int k = 0; k < 3; ++k)
                    fOut.write((char*)&(image.bits[i][j].a[k]), 1);
                    //fwrite(&(image->bits[i][j].a[k]), 1, 1, fOut);

                if (bmpInfoH.biBitCount == 32)
                {
                    fOut.put(0);
                    //putc(0, fOut);
                }
            }

            for (unsigned k = 0; k < padLine; ++k)
            {
                fOut.put(0);
                //putc(0, fOut);
            }
        }
        return;
}

void Image::gary()
{
    unsigned char res;
    for (unsigned int i = 0; i < image.len; ++i)
        for (unsigned int j = 0; j < image.wid; ++j)
        {
            res = (unsigned char)(0.3 * image.bits[i][j].a[0] +  0.5 * image.bits[i][j].a[1] + 0.2 * image.bits[i][j].a[2]) ;
            image.bits[i][j].a[0] = res;
            image.bits[i][j].a[1] = res;
            image.bits[i][j].a[2] = res;
        }
}

void Image::median()
{
    medPix(0);
    medPix(1);
    medPix(2);
}

void Image::gauss()
{
    if (xMatr != 3)
        createMatrix(3);
    matrix[0][0] = 1; matrix[0][1] = 2; matrix[0][2] = 1;
    matrix[1][0] = 2; matrix[1][1] = 4; matrix[1][2] = 2;
    matrix[2][0] = 1; matrix[2][1] = 2; matrix[2][2] = 1;
    struct pix<long> temp;
    temp.a[0] = 0;
    temp.a[1] = 0;
    temp.a[2] = 0;
    for (long long i = 0; i < image.len; ++i)
        for (long long j = 0; j < image.wid; ++j)
        {
            temp = goMatrix(i, j);
            image.bits[i][j].a[0] = (unsigned char)(temp.a[0] / 16);
            image.bits[i][j].a[1] = (unsigned char)(temp.a[1] / 16);
            image.bits[i][j].a[2] = (unsigned char)(temp.a[2] / 16);
        }
}

void Image::sobelY()
{
    if (xMatr != 3)
        createMatrix(3);
    matrix[0][0] = -1; matrix[0][1] = 0; matrix[0][2] = 1;
    matrix[1][0] = -2; matrix[1][1] = 0; matrix[1][2] = 2;
    matrix[2][0] = -1; matrix[2][1] = 0; matrix[2][2] = 1;

    sobel();
}
void Image::sobelX()
{
    if (xMatr != 3)
        createMatrix(3);
    matrix[0][0] = 1;   matrix[0][1] = 2;   matrix[0][2] = 1;
    matrix[1][0] = 0;   matrix[1][1] = 0;   matrix[1][2] = 0;
    matrix[2][0] = -1;  matrix[2][1] = -2;  matrix[2][2] = -1;

    sobel();
}

void Image::sobelAll()
{
    struct img next;
    next.len = image.len;
    next.wid = image.wid;
    next.bits = new struct pix<unsigned char>* [next.len];
    for (unsigned int i = 0; i < next.len; ++i)
        next.bits[i] = new struct pix<unsigned char> [next.wid];
    for (unsigned int i = 0; i < next.len; ++i)
        for (unsigned int j = 0; j < next.wid; ++j)
            for (int u = 0; u < 3; ++u)
                next.bits[i][j].a[u] = image.bits[i][j].a[u];

    sobelX();

    struct pix<unsigned char> **temp = image.bits;
    image.bits = next.bits;
    next.bits = temp;

    sobelY();

    long long z = 0;
    for (unsigned int i = 0; i < next.len; ++i)
        for (unsigned int j = 0; j < next.wid; ++j)
            for (int u = 0; u < 3; ++u)
            {
                z = (long long)trunc(sqrt(image.bits[i][j].a[u] * image.bits[i][j].a[u] + next.bits[i][j].a[u] * next.bits[i][j].a[u]));
                z = z > 255 ? 255 : z;
                z = z < 0 ? 0 : z;
                image.bits[i][j].a[u] = (unsigned char)z;
            }

    for (unsigned int i = 0; i < next.len; ++i)
        delete [] next.bits[i];
}

void Image::sobel()
{
    struct pix<long> temp;
    temp.a[0] = 0;
    temp.a[1] = 0;
    temp.a[2] = 0;
    struct pix<unsigned char> **bits = new struct pix<unsigned char>* [image.len];
    for (unsigned int i = 0; i < image.len; ++i)
        bits[i] = new struct pix<unsigned char> [image.wid];

    for (unsigned int i = 0; i < image.len; ++i)
        for (unsigned int j = 0; j < image.wid; ++j)
            for (int u = 0; u < 3; ++u)
                bits[i][j].a[u] = 0;

    for (long long i = 1; i < image.len - 1; ++i)
    {
        for (long long j = 1; j < image.wid - 1; ++j)
        {
            temp = goMatrix(i, j);

            temp.a[0] = temp.a[0] > 255 ? 255 : temp.a[0];
            temp.a[0] = temp.a[0] < 0 ? 0 : temp.a[0];
            temp.a[1] = temp.a[1] > 255 ? 255 : temp.a[1];
            temp.a[1] = temp.a[1] < 0 ? 0 : temp.a[1];
            temp.a[2] = temp.a[2] > 255 ? 255 : temp.a[2];
            temp.a[2] = temp.a[2] < 0 ? 0 : temp.a[2];

            bits[i][j].a[0] = (unsigned char)(temp.a[0]);
            bits[i][j].a[1] = (unsigned char)(temp.a[1]);
            bits[i][j].a[2] = (unsigned char)(temp.a[2]);
        }
    }

    for (unsigned int i = 0; i < image.len; ++i)
        delete [] image.bits[i];
    //free(image.bits);
    image.bits = bits;
}
