extern __global__ void BitmapToString(unsigned char *input, int width, int height, char *output)
{
    int x = blockDim.x * blockIdx.x + threadIdx.x;
    int y = blockDim.y * blockIdx.y + threadIdx.y;
    int index = y * width + x;

    if (x >= width || y >= height)
        return;

    unsigned char blue = input[index * 3];
    unsigned char green = input[index * 3 + 1];
    output[index] = (char)(blue | (green << 8));
}