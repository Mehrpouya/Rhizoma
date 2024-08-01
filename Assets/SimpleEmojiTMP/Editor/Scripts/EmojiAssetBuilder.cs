using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore;

namespace Assets.SimpleEmojiTMP.Editor.Scripts
{
    [CreateAssetMenu(fileName = "EmojiAssetBuilder", menuName = "TextMeshPro/Emoji Asset Builder")]
    public class EmojiAssetBuilder : ScriptableObject
    {
        [Header("Input")]
        public Texture2D SheetTexture;
        public TextAsset Config;

        [Header("Ounput")]
        public TMP_SpriteAsset TMPSpriteAsset;
        public Texture2D SpriteAtlas;
        public int CellSize;

        [Header("Settings")]
        public float Scale;
        public float OffsetY;
        public bool GeneratePOTTexture;
        [Tooltip("Exclude subcategories and emojis to optimize the output texture.")]
        public List<string> ExcludeSubcategories;
        public List<string> ExcludeEmojis;

        [HideInInspector] public List<string> Categories;
        [HideInInspector] public List<string> IncludeCategories;

        public void Build()
        {
            var json = JArray.Parse(Config.text);
            var sprites = new List<Color[]>();
            var chars = new List<TMP_SpriteCharacter>();

            TMPSpriteAsset.spriteCharacterTable.Clear();
            TMPSpriteAsset.spriteGlyphTable.Clear();

            for (var i = 0; i < json.Count; i++)
            {
                var charName = json[i]["name"].Value<string>();
                var category = json[i]["category"].Value<string>();
                var subcategory = json[i]["subcategory"].Value<string>();

                if (!IncludeCategories.Contains(category) || ExcludeSubcategories.Contains(subcategory) || ExcludeEmojis.Contains(charName)) continue;

                var unified = json[i]["unified"].Value<string>();

                if (unified.Contains('-'))
                {
                    // https://forum.unity.com/threads/how-to-display-emojis-of-multiple-code-points.1368228/
                    Debug.LogWarning($"Ignoring {charName} ({unified}). Multi-codepoint emojis are not supported in the current version of TMP.");
                    continue;
                }

                var unicode = Convert.ToUInt32(unified, 16);
                var x = json[i]["sheet_x"].Value<int>();
                var y = json[i]["sheet_y"].Value<int>();
                var glyph = new TMP_SpriteGlyph
                {
                    index = (uint) chars.Count,
                    glyphRect = new GlyphRect(CellSize * x, SheetTexture.height - CellSize * (y + 1), CellSize, CellSize),
                    metrics = new GlyphMetrics(CellSize, CellSize, 0, CellSize + OffsetY, CellSize)
                };
                var character = new TMP_SpriteCharacter(unicode, glyph) { name = charName, scale = Scale };
                var pixels = SheetTexture.GetPixels(CellSize * x + 1 + 2 * x, SheetTexture.height - CellSize * (y + 1) - 1 - 2 * y, CellSize, CellSize);
                
                chars.Add(character);
                sprites.Add(pixels);
            }

            if (chars.Count > 0)
            {
                var texture = GeneratePOTTexture ? CreateTexturePOT(chars.Count) : CreateTexture(chars.Count);
                var columns = texture.width / CellSize;

                for (var i = 0; i < chars.Count; i++)
                {
                    var x = i % columns;
                    var y = i / columns;
                    var rect = new GlyphRect(CellSize * x, texture.height - CellSize * (y + 1), CellSize, CellSize);

                    texture.SetPixels(rect.x, rect.y, rect.width, rect.height, sprites[i]);
                    chars[i].glyph.glyphRect = rect;
                    TMPSpriteAsset.spriteCharacterTable.Add(chars[i]);
                    TMPSpriteAsset.spriteGlyphTable.Add((TMP_SpriteGlyph) chars[i].glyph);
					EditorUtility.SetDirty(TMPSpriteAsset);
                }

                TMPSpriteAsset.SortGlyphTable();
                TMPSpriteAsset.UpdateLookupTables();

                var path = AssetDatabase.GetAssetPath(SpriteAtlas);

                File.WriteAllBytes(path, texture.EncodeToPNG());

                var message = $"{chars.Count} emojis packed into '{SpriteAtlas.name}' ({texture.width}x{texture.height} px).";

                if (texture.width > 2048 || texture.height > 2048)
                {
                    message += "\n\nWARNING: Textures larger than 2048x2048 px may be not supported on old devices. Reduce emoji size or amount, or set [Max Size]=4096 for `EmojiTMP` texture to continue.";
                }

                EditorUtility.DisplayDialog("Emoji Asset Builder", message, "OK");
                DestroyImmediate(texture);
                AssetDatabase.Refresh();
            }
            else
            {
                EditorUtility.DisplayDialog("Emoji Asset Builder", $"{chars.Count} emojis packed.", "OK");
            }
        }

        public void RefreshCategories()
        {
            var json = JArray.Parse(Config.text);

            Categories.Clear();

            for (var i = 0; i < json.Count; i++)
            {
                var category = json[i]["category"].Value<string>();

                if (!Categories.Contains(category)) Categories.Add(category);
            }
        }

        private Texture2D CreateTexture(int charCount)
        {
            var width = CellSize * Mathf.CeilToInt(Mathf.Sqrt(charCount));
            var columns = width / CellSize;
            var height = CellSize * Mathf.CeilToInt(charCount / (float) columns);
            var texture = new Texture2D(width, height);

            texture.SetPixels32(new Color32[texture.width * texture.height]);

            return texture;
        }

        private Texture2D CreateTexturePOT(int charCount)
        {
            var width = CellSize * Mathf.CeilToInt(Mathf.Sqrt(charCount));

            for (var p = 8; p <= 11; p++)
            {
                var min = (int) Mathf.Pow(2, p);

                if (width <= min)
                {
                    width = min;
                    break;
                }
            }

            var columns = width / CellSize;
            var height = CellSize * Mathf.CeilToInt(charCount / (float) columns);
            var pow = 11;

            if (charCount * CellSize > Mathf.Pow(2, pow) && charCount * CellSize <= Mathf.Pow(2, pow))
            {
                pow++;
            }

            for (var p = 8; p <= pow; p++)
            {
                var min = (int) Mathf.Pow(2, p);

                if (height <= min)
                {
                    height = min;
                    break;
                }
            }

            var texture = new Texture2D(width, height);

            texture.SetPixels32(new Color32[texture.width * texture.height]);

            return texture;
        }
    }
}