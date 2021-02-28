using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GLASTIK
{
    public class TextureManager : IDisposable
    {
        protected ContentManager content;
        protected Dictionary<ushort, Texture2D> textures = new();
        protected Dictionary<ushort, bool> missingTextureLogged = new();
        protected string textureFile;

        public TextureManager(ContentManager content)
        {
            this.content = content ?? throw new ArgumentNullException(nameof(content));
        }

        public void Dispose()
        {
            content.Unload();
        }

        public Texture2D GetTexture(ushort index)
        {
            if (textures.ContainsKey(index) == false)
            {
                if (missingTextureLogged.ContainsKey(index) == false)
                {
                    missingTextureLogged[index] = true;
                    GameConsole.GameConsole.Log.LogWarning($"Texture requested at {index} - not specified in file {textureFile}.");
                }

                return null;
            }

            return textures[index];
        }

        public void PreloadTextures(string textureFile)
        {
            this.textureFile = textureFile;

            // Ensure file exists
            if (!File.Exists(textureFile))
            {
                throw new FileNotFoundException("Could not find texture file!", textureFile);
            }

            // Open file
            using FileStream fs = File.OpenRead(textureFile);
            using StreamReader stream = new(fs);

            string line;
            uint lineNum = 0;

            // Read file
            while ((line = stream.ReadLine()) != null)
            {
                lineNum++;

                int commentIndex = line.IndexOf('#');

                if (commentIndex != -1)
                {
                    line = line.Substring(0, commentIndex);
                }

                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                else
                {

                    string[] parts = line.Split('=', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();

                    if (ushort.TryParse(parts[0], out ushort result) == false)
                    {
                        GameConsole.GameConsole.Log.LogError($"Invalid index {parts[0]} in texture file {textureFile} at line {lineNum}.");
                    }
                    else
                    {
                        try
                        {
                            Texture2D tex = content.Load<Texture2D>(parts[1]);
                            if (textures.ContainsKey(result)) GameConsole.GameConsole.Log.LogWarning($"Overwriting texture {textures[result].Name} at index {result} with texture {parts[1]}.");
                            textures[result] = tex;
                            GameConsole.GameConsole.Log.LogDebug(0, $"Loaded texture {parts[1]} at index {result}.");
                        }
                        catch (Exception)
                        {
                            GameConsole.GameConsole.Log.LogError($"Failed to load texture {parts[1]}. (from {textureFile} at line {lineNum})");
                        }
                    }

                }
            }
        }
    }
}
