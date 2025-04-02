using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseHeightMapGenerator
{


    public static float[,] GenerateHeightMap(int width, int height, float scale, int seed)
    {
        System.Random rand = new System.Random(seed);
        float[,] heightMap = new float[width,height];
        float[,] mask = new float[width, height];

        float sampleX;
        float sampleY;
        int frequencies = 4;
        float perlintemp = 0f;

        float offsetX;
        float offsetY;
        Vector2[] randomOffsets = new Vector2[frequencies] ; // offsets to be used for each sample to achieve better randomness

        for (int i = 0; i < frequencies; i++)
        {
            offsetX = rand.Next(-10000, 10000);
            offsetY = rand.Next(-10000, 10000);
            randomOffsets[i] = new Vector2(offsetX, offsetY);
        }
        

        

        //scale must not be zero
        if(scale == 0)
        {
            scale = 10^(-5);
        }

        for(int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {

                float amplitude = 1f;
                float noiseHeight = 0f;

                //Example with one frequency
                /*sampleX = i / scale * 13f + randomOffsets[0].x;
                sampleY = j / scale * 13f + randomOffsets[0].y;
                noiseHeight = Mathf.PerlinNoise(sampleX, sampleY);*/

                //Example with a number of frequencies (more detail)
                for (int f = 1; f <= frequencies; f++)
                {
                    sampleX = i / scale * (2^f) + randomOffsets[f-1].x;
                    sampleY = j / scale * (2^f) + randomOffsets[f-1].y;
                    perlintemp = Mathf.PerlinNoise(sampleX, sampleY);
                    noiseHeight += perlintemp * amplitude; //higher frequencies have lower amplitude
                    amplitude *= 0.5f;

                    if (j == 0 && i==0) Debug.Log("NOISE HEIGHT: "+noiseHeight);
                }


                

                if (noiseHeight < 0.65f)
                    heightMap[i, j] = 0.1f * noiseHeight; //creates wider valleys
                else if (noiseHeight > 1f)
                    heightMap[i, j] = 5f * noiseHeight; //creates taller mountains
                else
                    heightMap[i, j] = 1f * noiseHeight; //create bumps on the terrain


                //Create mask
                if (j < 5 || i < 5 || (j > width - 5) || (i > height - 5))
                    mask[i, j] = 10f;
                else if (((j < 80) && (i < 80)))
                    mask[i, j] = -heightMap[i, j] + 0.7f; //Create enough space for the stronghold
                else if (((j > (width / 2 - 20) && (j < (width / 2 + 20))) && (i > (height / 2 - 20) && (i < (height / 2 + 20)))))
                    mask[i, j] = -heightMap[i, j] + 1f;
                else
                    mask[i, j] = 0f;

                heightMap[i, j] = heightMap[i, j] + mask[i, j];

            }
        }


        return heightMap;
    }
    
}
