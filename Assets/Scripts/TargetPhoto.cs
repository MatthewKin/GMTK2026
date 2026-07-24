using UnityEngine;

public class TargetPhoto : MonoBehaviour
{
    public Texture2D targetImage;

    [Header("Resize Settings")]
    public int width = 512;
    public int height = 512;

    private Texture2D resizedImage;

    public Vector3 TargetPosition
    {
        get { return transform.position; }
    }

    public Quaternion TargetRotation
    {
        get { return transform.rotation; }
    }

    public Texture2D GetTargetImage()
    {
        if (resizedImage == null && targetImage != null)
        {
            resizedImage = ResizeTexture(targetImage, width, height);
        }

        return resizedImage;
    }


    private Texture2D ResizeTexture(Texture2D source, int newWidth, int newHeight)
    {
        RenderTexture rt = new RenderTexture(
            newWidth,
            newHeight,
            24
        );

        Graphics.Blit(source, rt);

        RenderTexture.active = rt;

        Texture2D result = new Texture2D(
            newWidth,
            newHeight,
            TextureFormat.RGB24,
            false
        );

        result.ReadPixels(
            new Rect(0, 0, newWidth, newHeight),
            0,
            0
        );

        result.Apply();

        RenderTexture.active = null;
        Destroy(rt);

        return result;
    }
}