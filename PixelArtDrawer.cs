using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class PixelArtDrawer : MonoBehaviour
{
    public Color paintColor = Color.black;
    Texture2D texture;

    public async void Save()
    {
        byte[] bytes = texture.EncodeToPNG();
        var dirPath = Application.dataPath + "/Resources/Sprites/Backgrounds/";
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        int filename = gameObject.GetHashCode();
        await File.WriteAllBytesAsync(dirPath + filename + ".png", bytes);

        AssetDatabase.Refresh();
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Backgrounds/" + filename);
        texture = GetComponent<SpriteRenderer>().sprite.texture;
        print("Sprite saved.");
    }

    public void NewTexture()
    {
        
        if (texture.width == (int)Mathf.Round(GetComponent<SpriteRenderer>().size.x * 12) && texture.height == (int)Mathf.Round(GetComponent<SpriteRenderer>().size.y))
        {
            //if same size, just clear the texture

            var fillColorArray = texture.GetPixels();
            for (var i = 0; i < fillColorArray.Length; ++i)
            {
                fillColorArray[i] = Color.clear;
            }
            texture.SetPixels(fillColorArray);
            texture.Apply();
        }
        else
        {
            //if size changed, create new texture

            Destroy(texture);
            texture = new Texture2D((int)Mathf.Round(GetComponent<SpriteRenderer>().size.x * 12), (int)Mathf.Round(GetComponent<SpriteRenderer>().size.y * 12), TextureFormat.RGBA32, false, true);
            Sprite sprite = Sprite.Create(texture,
                                          new Rect(0, 0, (int)Mathf.Round(GetComponent<SpriteRenderer>().size.x * 12), (int)Mathf.Round(GetComponent<SpriteRenderer>().size.y * 12)), Vector2.one / 2, 12);
            texture.filterMode = FilterMode.Point;
            print("New sprite created.");
            GetComponent<SpriteRenderer>().sprite = sprite;

            var fillColorArray = texture.GetPixels();
            for (var i = 0; i < fillColorArray.Length; ++i)
            {
                fillColorArray[i] = Color.clear;

            }
            texture.SetPixels(fillColorArray);

            texture.Apply();

            Save();
        }
    }

    public void DrawPixel(Vector2 pos, bool delete = false)
    {
        Vector2 relativePos = (Vector2)transform.position - pos;

        Vector2 targetPos = new Vector2(-relativePos.x + GetComponent<SpriteRenderer>().size.x / 2,
                                        -relativePos.y + GetComponent<SpriteRenderer>().size.y / 2);
        targetPos = targetPos * 12;
        texture.SetPixel((int)(targetPos.x), (int)(targetPos.y), delete? Color.clear : paintColor);
        texture.Apply(false);
    }
}
