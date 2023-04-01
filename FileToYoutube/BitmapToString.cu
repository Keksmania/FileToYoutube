extern __global__ void BitmapToStringAndFindWhitePixel(unsigned char *input, int width, int height, char *output, int *x_pos, int *y_pos)
{
    int x = blockDim.x * blockIdx.x + threadIdx.x;
    int y = blockDim.y * blockIdx.y + threadIdx.y;
    int index = y * width + x;

    if (x >= width || y >= height)
        return;

    unsigned char blue = input[index * 3];
    unsigned char green = input[index * 3 + 1];
    unsigned char red = input[index * 3 + 2];
    output[index] = (char)(blue | (green << 8));

    // Check for white pixel
    if (red == 255 && green == 255 && blue == 255)
    {
        *x_pos = x;
        *y_pos = y;
        return;
    }
}