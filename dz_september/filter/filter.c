#include "filter.h"

void gray(struct img *image)
{
    unsigned char res;
    for (unsigned int i = 0; i < image->len; ++i)
        for (unsigned int j = 0; j < image->wid; ++j)
        {
            res = (unsigned char)(0.3 * image->bits[i][j].a[0] +  0.5 * image->bits[i][j].a[1] + 0.2 * image->bits[i][j].a[2]) ;
            image->bits[i][j].a[0] = res;
            image->bits[i][j].a[1] = res;
            image->bits[i][j].a[2] = res;
        }
}

void medPix(struct img *image, int z)
{
    unsigned char matrix[9];
    for (unsigned int i = 0; i + 2 < image->len; i += 3)
        for (unsigned int j = 0; j + 2 < image->wid; j += 3)
        {
            unsigned char temp;
            for (unsigned int u = 0; u < 3; ++u)
                for (unsigned int o = 0; o < 3; ++o)
                    matrix[o + 3 * u] = image->bits[i + u][j + o].a[z];
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
                    image->bits[i + u][j + o].a[z] = matrix[4];
        }
}
void median(struct img *image)
{
    medPix(image, 0);
    medPix(image, 1);
    medPix(image, 2);
}

void gauss(struct img *image)
{
    unsigned char matrix[3][3]=\
    {{1, 2, 1},\
     {2, 4, 2},\
     {1, 2, 1}};
    long long m, n;
    long long r = 0, g = 0, b = 0;
    for (long long i = 0; i < image->len; ++i)
        for (long long j = 0; j < image->wid; ++j)
        {
            r = 0;
            g = 0;
            b = 0;
            for (int t = -1; t < 2; ++t)
                for (int y = -1; y < 2; ++y)
                {
                    m = i + t;
                    n = j + y;

                    if ((m > 0) && (n > 0) && (m < image->len) && (n < image->wid))
                    {
                        r += image->bits[m][n].a[0] * matrix[t + 1][y + 1];
                        g += image->bits[m][n].a[1] * matrix[t + 1][y + 1];
                        b += image->bits[m][n].a[2] * matrix[t + 1][y + 1];
                    }
                }
            image->bits[i][j].a[0] = (unsigned char)(r / 16);
            image->bits[i][j].a[1] = (unsigned char)(g / 16);
            image->bits[i][j].a[2] = (unsigned char)(b / 16);
        }
}

void sobel(struct img *image, int matrix[3][3])
{
    long long m, n;
    int r = 0, g = 0, b = 0;
    struct pix **bits = (struct pix **)malloc(sizeof (struct pix *) * image->len);
    for (unsigned int i = 0; i < image->len; ++i)
        bits[i] = (struct pix *)malloc(sizeof (struct pix) * image->wid);
    for (long long i = 1; i < image->len - 1; ++i)
    {
        for (long long j = 1; j < image->wid - 1; ++j)
        {
            r = 0;
            g = 0;
            b = 0;
            for (int t = -1; t < 2; ++t)
                for (int y = -1; y < 2; ++y)
                {
                    m = i + t;
                    n = j + y;


                        r += image->bits[m][n].a[0] * matrix[t + 1][y + 1];
                        g += image->bits[m][n].a[1] * matrix[t + 1][y + 1];
                        b += image->bits[m][n].a[2] * matrix[t + 1][y + 1];

                }

            if (r < 0)
                {
                    r = 0;
                }
                else if (r > 255)
                {
                    r = 255;
                }

                if (g < 0)
                {
                    g = 0;
                }
                else if (g > 255)
                {
                    g = 255;
                }

                if (b < 0)
                {
                    b = 0;
                }
                else if (b > 255)
                {
                    b = 255;
                }

            bits[i][j].a[0] = (unsigned char)(r);
            bits[i][j].a[1] = (unsigned char)(g);
            bits[i][j].a[2] = (unsigned char)(b);
        }
    }

    for (unsigned int i = 0; i < image->len; ++i)
        free(image->bits[i]);
    free(image->bits);
    image->bits = bits;
}

void multisobel(struct img *image)
{
    struct img next;
    next.len = image->len;
    next.wid = image->wid;
    next.bits = (struct pix **)malloc(sizeof (struct pix *) * next.len);
    for (unsigned int i = 0; i < next.len; ++i)
        next.bits[i] = (struct pix *)malloc(sizeof (struct pix) * next.wid);
    for (unsigned int i = 0; i < next.len; ++i)
        for (unsigned int j = 0; j < next.wid; ++j)
            for (int u = 0; u < 3; ++u)
                next.bits[i][j].a[u] = image->bits[i][j].a[u];

    int matrix[3][3]=\
    {{1, 2, 1},\
     {0, 0, 0},\
     {-1, -2, -1}};
    sobel(image, matrix);

    int secMatrix[3][3]=\
    {{-1, 0, 1},\
     {-2, 0, 2},\
     {-1, 0, 1}};
    sobel(&next, secMatrix);
    long long z = 0;
    for (unsigned int i = 0; i < next.len; ++i)
        for (unsigned int j = 0; j < next.wid; ++j)
            for (int u = 0; u < 3; ++u)
            {
                z = (long long)trunc(sqrt(image->bits[i][j].a[u] * image->bits[i][j].a[u] + next.bits[i][j].a[u] * next.bits[i][j].a[u]));
                if (z > 255)
                    z = 255;
                else
                    if (z < 0)
                        z = 0;
                image->bits[i][j].a[u] = (unsigned char)z;
            }

    for (unsigned int i = 0; i < next.len; ++i)
        free(next.bits[i]);
    free(next.bits);

}

void filter(struct img *image, char *str)
{
    if (strcmp(str, "gray") == 0)
    {
        gray(image);
        return;
    }
    if (strcmp(str, "median") == 0)
    {
        median(image);
        return;
    }
    if (strcmp(str, "gauss") == 0)
    {
        gauss(image);
        return;
    }
    if (strcmp(str, "sobelX") == 0)
    {
        int matrix[3][3]=\
        {{1, 2, 1},\
         {0, 0, 0},\
         {-1, -2, -1}};
        sobel(image, matrix);
        return;
    }
    if (strcmp(str, "sobelY") == 0)
    {
        int matrix[3][3]=\
        {{-1, 0, 1},\
         {-2, 0, 2},\
         {-1, 0, 1}};
        sobel(image, matrix);
        return;
    }
    if (strcmp(str, "sobelAll") == 0)
    {
        multisobel(image);
        return;
    }
}

int getReady(FILE *fIn, struct BITMAPFILEHEADER *bmpFileH, struct BITMAPINFOHEADER *bmpInfoH, struct img *image, unsigned int *paleteSize, char *palete, unsigned int *padLine)
{
    fread(bmpFileH, sizeof(*bmpFileH), 1, fIn);
    fread(bmpInfoH, sizeof(*bmpInfoH), 1, fIn);


    if ((bmpInfoH->biBitCount != 24) && (bmpInfoH->biBitCount != 32))
    {
        return -1;
    }
    palete = NULL;

    *paleteSize = bmpFileH->bfOffBits - sizeof (struct BITMAPFILEHEADER) - sizeof (struct BITMAPINFOHEADER);

    if (paleteSize != 0)
    {
        palete = (char *)malloc(*paleteSize);
        fread(palete, *paleteSize, 1, fIn);
    }
    struct pix **bits = (struct pix **)malloc(sizeof (struct pix *) * bmpInfoH->biHeight);
    for (unsigned int i = 0; i < bmpInfoH->biHeight; ++i)
        bits[i] = (struct pix *)malloc(sizeof (struct pix) * bmpInfoH->biWidth);

    for (unsigned int i = 0; i < bmpInfoH->biHeight; ++i)
        for (unsigned int j = 0; j < bmpInfoH->biWidth; ++j)
            for (int k = 0; k < 3; ++k)
                bits[i][j].a[k] = 0;




   *padLine = (4 - (bmpInfoH->biWidth * (bmpInfoH->biBitCount / 8)) % 4) & 3;

   for (unsigned int i = 0; i < bmpInfoH->biHeight; ++i)
   {
      for (unsigned int j = 0; j < bmpInfoH->biWidth; ++j)
      {
          for (int k = 0; k < 3; ++k)
              bits[i][j].a[k] = (unsigned char)getc(fIn);

          if (bmpInfoH->biBitCount == 32)
          {
              getc(fIn);
          }
      }
      for (unsigned int k = 0; k < *padLine; k++)
      {
          getc(fIn);
      }
   }
   //image = (struct img*)malloc(sizeof (struct img));
   image->bits = bits;
   image->wid = bmpInfoH->biWidth;
   image->len = bmpInfoH->biHeight;
   return 0;
}

int getDone(FILE *fOut, struct BITMAPFILEHEADER bmpFileH, struct BITMAPINFOHEADER bmpInfoH, struct img *image, unsigned int paleteSize, char *palete, unsigned int padLine)
{
    fwrite(&bmpFileH, sizeof (bmpFileH), 1, fOut);
    fwrite(&bmpInfoH, sizeof (bmpInfoH), 1, fOut);

        if (paleteSize != 0)
        {
            fwrite(palete, paleteSize, 1, fOut);
        }

        for (unsigned int i = 0; i < bmpInfoH.biHeight; ++i)
        {
            for (unsigned int j = 0; j < bmpInfoH.biWidth; ++j)
            {
                for (int k = 0; k < 3; ++k)
                    fwrite(&(image->bits[i][j].a[k]), 1, 1, fOut);

                if (bmpInfoH.biBitCount == 32)
                {
                    putc(0, fOut);
                }
            }

            for (unsigned k = 0; k < padLine; ++k)
            {
                putc(0, fOut);
            }
        }
        return 0;
}
