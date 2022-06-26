using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class TextCreator
{
    public static GameObject CreateText(Transform canvas_transform, float x, float y, float sx, float sy, string text_to_print, int font_size, Color text_color) {
        GameObject UItextGO = new GameObject(text_to_print);
        UItextGO.transform.SetParent(canvas_transform);

        RectTransform trans = UItextGO.AddComponent<RectTransform>();
        trans.anchoredPosition = new Vector2(x, y);
        trans.localScale = new Vector3(1, 1, 1);
        trans.sizeDelta = new Vector2(sx, sy);

        Text text = UItextGO.AddComponent<Text>();
        Font ArialFont = (Font)Resources.Load("Fonts/Roboto-Regular");
        text.font = ArialFont;
        text.text = text_to_print;
        text.fontSize = font_size;
        text.color = text_color;

        return UItextGO;
    }

    public static string addReturnText(string text) {

        // add \n in the text 
        /*while (text.Length > 0) {
            int len = 25;

                if (text.Length < len) {
                    len = text.Length;
                }

                newText = newText + text.Substring(0, len) + '\n';
                text = text.Substring(len, text.Length - len);
            }*/

        string newText = "";
        string[] words = text.Split(' ');
        int sizeWords = 0;

        for (int i = 0; i < words.Length; i++) {
            sizeWords = sizeWords + words[i].Length;
            newText = newText + words[i] + ' ';
            if (sizeWords > 20) {
                newText = newText + '\n';
                sizeWords = 0;
            }
        }

        newText.Remove(newText.Length - 1);
        return newText;
    }
}
