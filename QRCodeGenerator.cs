using UnityEngine;
using ZXing;
using ZXing.QrCode;
using ZXing.Common;
using System.Text;
using Newtonsoft.Json;

namespace QRCodeGenerator
{
    public static class QRCodeWriter
    {
        /// <summary>
        /// JSON ObjectをQRコードに変換してTexture2Dを返す
        /// </summary>
        /// <param name="jsonObject">QRコードに変換するJSONオブジェクト</param>
        /// <param name="width">QRコードの幅（デフォルト: 256px）</param>
        /// <param name="height">QRコードの高さ（デフォルト: 256px）</param>
        /// <param name="margin">QRコードのマージン（デフォルト: 1）</param>
        /// <returns>QRコードのTexture2D</returns>
        public static Texture2D GenerateQRCodeFromJson(object jsonObject, int width = 256, int height = 256, int margin = 1)
        {
            try
            {
                // JSONオブジェクトを文字列にシリアライズ
                string jsonString = JsonConvert.SerializeObject(jsonObject, Formatting.None);

                return GenerateQRCodeFromString(jsonString, width, height, margin);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"QRコード生成エラー: {e.Message}");
                return CreateErrorTexture(width, height);
            }
        }

        /// <summary>
        /// JSON文字列をQRコードに変換してTexture2Dを返す
        /// </summary>
        /// <param name="jsonString">QRコードに変換するJSON文字列</param>
        /// <param name="width">QRコードの幅（デフォルト: 256px）</param>
        /// <param name="height">QRコードの高さ（デフォルト: 256px）</param>
        /// <param name="margin">QRコードのマージン（デフォルト: 1）</param>
        /// <returns>QRコードのTexture2D</returns>
        public static Texture2D GenerateQRCodeFromString(string jsonString, int width = 256, int height = 256, int margin = 1)
        {
            try
            {
                // QRコードライターの設定
                var writer = new BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new QrCodeEncodingOptions
                    {
                        Height = height,
                        Width = width,
                        Margin = margin,
                        CharacterSet = "UTF-8"
                    }
                };

                // QRコードを生成（Color32配列として）
                var pixelData = writer.Write(jsonString);

                // Texture2Dを作成
                Texture2D qrTexture = new Texture2D(width, height, TextureFormat.RGB24, false);

                // Color32配列をColor配列に変換
                Color[] colors = new Color[pixelData.Length];
                for (int i = 0; i < pixelData.Length; i++)
                {
                    colors[i] = pixelData[i];
                }

                // テクスチャにピクセルデータを適用
                qrTexture.SetPixels(colors);
                qrTexture.Apply();

                return qrTexture;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"QRコード生成エラー: {e.Message}");
                return CreateErrorTexture(width, height);
            }
        }

        /// <summary>
        /// エラー時に表示する赤いテクスチャを作成
        /// </summary>
        /// <param name="width">テクスチャの幅</param>
        /// <param name="height">テクスチャの高さ</param>
        /// <returns>赤いテクスチャ</returns>
        private static Texture2D CreateErrorTexture(int width, int height)
        {
            Texture2D errorTexture = new Texture2D(width, height, TextureFormat.RGB24, false);
            Color[] colors = new Color[width * height];

            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = Color.red;
            }

            errorTexture.SetPixels(colors);
            errorTexture.Apply();

            return errorTexture;
        }
    }
}