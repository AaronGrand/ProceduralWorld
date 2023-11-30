using UnityEngine;

public static class Noise
{
    public static float Get2DPerlin(int seed, Vector2 position, float offset, float scale)
    {
        position.x += (offset + Data.seed + 0.1f);
        position.y += (offset + Data.seed + 0.1f);

        return Mathf.PerlinNoise(position.x / Data.chunkWidth * scale, position.y / Data.chunkWidth * scale);
    }

    public static bool Get3DPerlin(int seed, Vector3 position, float offset, float scale, float threshold)
    {
        float x = (position.x + offset + offset + Data.seed + 0.1f) * scale;
        float y = (position.y + offset + offset + Data.seed + 0.1f) * scale;
        float z = (position.z + offset + offset + Data.seed + 0.1f) * scale;

        float AB = Mathf.PerlinNoise(x, y);
        float BC = Mathf.PerlinNoise(y, z);
        float AC = Mathf.PerlinNoise(x, z);
        float BA = Mathf.PerlinNoise(y, x);
        float CB = Mathf.PerlinNoise(z, y);
        float CA = Mathf.PerlinNoise(z, x);

        if ((AB + BC + AC + BA + CB + CA) / 6f > threshold)
            return true;
        else
            return false;
    }
}
