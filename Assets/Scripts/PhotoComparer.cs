using UnityEngine;

public class PhotoComparer : MonoBehaviour
{
    public float CompareImages(Texture2D photo, Texture2D target)
{
    if (photo.width != target.width || photo.height != target.height)
    {
        Debug.LogWarning(
            $"Image sizes differ! Photo: {photo.width}x{photo.height}, Target: {target.width}x{target.height}"
        );

        return 0f;
    }

    Color[] photoPixels = photo.GetPixels();
    Color[] targetPixels = target.GetPixels();

    float totalDifference = 0f;

    for (int i = 0; i < photoPixels.Length; i++)
    {
        float rDifference = Mathf.Abs(photoPixels[i].r - targetPixels[i].r);
        float gDifference = Mathf.Abs(photoPixels[i].g - targetPixels[i].g);
        float bDifference = Mathf.Abs(photoPixels[i].b - targetPixels[i].b);

        totalDifference += rDifference + gDifference + bDifference;
    }

    // Maximum possible difference:
    // each pixel can differ by 3 channels * 1.0
    float maxDifference = photoPixels.Length * 3f;

    float similarity = 1f - (totalDifference / maxDifference);

    return Mathf.Clamp(similarity * 100f, 0f, 100f);
}


    public float CompareCamera(Camera cam, TargetPhoto target)
    {
        float positionScore =
            1f - Mathf.Clamp01(
                Vector3.Distance(
                    cam.transform.position,
                    target.TargetPosition
                ) / 10f
            );


        float rotationScore =
            1f - Mathf.Clamp01(
                Quaternion.Angle(
                    cam.transform.rotation,
                    target.TargetRotation
                ) / 180f
            );


        return ((positionScore + rotationScore) / 2f) * 100f;
    }


    public float CalculateScore(
        Texture2D photo,
        Camera cam,
        TargetPhoto target)
    {
        float imageScore = CompareImages(
            photo,
            target.GetTargetImage()
        );

        float cameraScore = CompareCamera(
            cam,
            target
        );

        return imageScore * 0.7f + cameraScore * 0.3f;
    }
}