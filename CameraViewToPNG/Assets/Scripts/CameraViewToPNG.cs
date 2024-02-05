using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewToPNG : MonoBehaviour
{
    [SerializeField]
    private string _saveFolderPath = "Assets/SavedImages";
    [SerializeField]
    private KeyCode _keyToPrint = KeyCode.P;
    // Start is called before the first frame update
    void Start()
    {
        // Certifique-se de que a pasta de salvamento exista, senão crie-a
        if (!System.IO.Directory.Exists(_saveFolderPath))
            System.IO.Directory.CreateDirectory(_saveFolderPath);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(_keyToPrint))
            CaptureAndSaveCameraView();
    }

    void CaptureAndSaveCameraView()
    {
        // Obtenha a textura da visão da câmera principal
        Texture2D screenshotTexture = CaptureCameraViewToTexture();

        // Encode a textura como um arquivo PNG
        byte[] bytes = screenshotTexture.EncodeToPNG();

        // Salvar o PNG em um arquivo
        string timestamp = System.DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss");
        string savePath = System.IO.Path.Combine(_saveFolderPath, $"Screenshot_{timestamp}.png");
        System.IO.File.WriteAllBytes(savePath, bytes);
        Debug.Log("Print Saved: " + savePath);
    }

    Texture2D CaptureCameraViewToTexture()
    {
        // Obtenha a visão da câmera principal como uma textura
        RenderTexture renderTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 32);
        Camera.main.targetTexture = renderTexture;
        Camera.main.Render();

        // Crie uma textura a partir do RenderTexture
        Texture2D texture = new(Screen.width, Screen.height);
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();

        // Limpe os recursos
        Camera.main.targetTexture = null;
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(renderTexture);

        return texture;

    }
}
