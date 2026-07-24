using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class CameraPhoto : MonoBehaviour
{
    [Header("References")]
    public Camera targetCamera;
    public Image photoDisplay;
    public TMP_Text scoreText;

    [Header("Photo Scoring")]
    public PhotoComparer comparer;
    public TargetPhoto targetPhoto;

    [Header("Capture")]
    public Key captureKey = Key.Space;
    public int width = 512;
    public int height = 512;

    [HideInInspector]
    public Texture2D lastPhoto;

    private bool takingPhoto;

    private void Awake()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    private void Update()
    {
        if (Keyboard.current[captureKey].wasPressedThisFrame && !takingPhoto)
        {
            StartCoroutine(TakePhoto());
        }
    }

    IEnumerator TakePhoto()
    {
        takingPhoto = true;

        yield return new WaitForEndOfFrame();

        RenderTexture rt = new RenderTexture(width, height, 24);

        targetCamera.targetTexture = rt;
        targetCamera.Render();

        RenderTexture.active = rt;

        lastPhoto = new Texture2D(
            width,
            height,
            TextureFormat.RGB24,
            false
        );

        lastPhoto.ReadPixels(
            new Rect(0, 0, width, height),
            0,
            0
        );

        lastPhoto.Apply();

        targetCamera.targetTexture = null;
        RenderTexture.active = null;

        Destroy(rt);


        // Display photo
        if (photoDisplay != null)
        {
            Sprite photoSprite = Sprite.Create(
                lastPhoto,
                new Rect(0, 0, lastPhoto.width, lastPhoto.height),
                new Vector2(0.5f, 0.5f)
            );

            photoDisplay.sprite = photoSprite;
            photoDisplay.preserveAspect = true;
        }


        // Compare photo
        if (comparer != null && targetPhoto != null)
        {
            float score = comparer.CalculateScore(
                lastPhoto,
                targetCamera,
                targetPhoto
            );

            Debug.Log($"Photo Score: {score:F0}/100");

            // Update TextMeshPro score
            if (scoreText != null)
            {
                scoreText.text = $"Score: {score:F0}/100";
            }
        }
        else
        {
            Debug.LogWarning(
                "Missing PhotoComparer or TargetPhoto reference!"
            );
        }


        Debug.Log("Photo taken!");

        takingPhoto = false;
    }
}