using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapTextureGenerator
{


    public static Texture2D GenerateTextureFromHeightMap(float[,] heightMap, int width, int height, Color[] regionColors, Texture2D[] regionTextures)
    {
        Texture2D texture = new Texture2D(width, height);
        Color[] colors = new Color[width*height];

        for(int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                colors[i * width + j] = Color.Lerp(Color.black, Color.white, heightMap[i, j]);

                if (heightMap[i, j] <= 0.7f)
                    colors[i * width + j] = regionColors[0] * regionTextures[1].GetPixel(i, j);
                else if(heightMap[i, j] >= 0.95f) //only mountaintops are brown
                    colors[i * width + j] = regionColors[1] * regionTextures[2].GetPixel(i, j);
                else
                    colors[i * width + j] = regionColors[2] * regionTextures[0].GetPixel(i, j);
            }
        }

        texture.SetPixels(colors);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Point;
        texture.Apply();
        return texture;
    }
    
}
