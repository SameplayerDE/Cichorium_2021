using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Cichorium.Data;
using Cichorium.Objects;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using tainicom.Aether.Physics2D.Dynamics;
using System.Security.Cryptography;
using Cichorium.Objects.ScreenObjects;
using System.Collections.Generic;
using System.Text;

namespace Cichorium.Managers
{
    public static class SaveFileManager
    {

        public static PlayerData Data;
        public static SkillTreeData TreeData;

        public static PlayerData Load()
        {

            if (File.Exists("version.txt"))
            {
                using (Stream stream = File.OpenRead("version.txt"))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string json = sr.ReadToEnd();

                        if (json.Equals(Cichorium.Version))
                        {

                        }
                        else
                        {
                            stream.Close();
                            sr.Close();
                            using (StreamWriter sw = new StreamWriter("version.txt"))
                            {
                                sw.Write(Cichorium.Version);
                                sw.Close();
                            }
                            File.Delete("playerData.json");
                        }

                    }
                }
            }
            else
            {
                /**using (StreamWriter sw = new StreamWriter("version.txt"))
                {
                    sw.Write(Cichorium.Version);
                    sw.Close();
                }**/
                File.Delete("playerData.json");
            }

            if (!File.Exists("playerData.json"))
            {
                Data = new PlayerData();
                Data.Region = "A";
                Data.Health = 1;
                Data.HealthScale = 2;
                Data.SkillPoints = 100;
                Data.Speed = 48f;

                SkillTreeData skillTreeData = new SkillTreeData();
                skillTreeData.Skills = new List<SkillData>()
                {
                    {new SkillData(0, "Naturtalent", "star", new string[] {"von Faehigkeiten", "Ermoeglicht das Erlernen"}, SkillAction.NONE, null, false, false, Color.Orange, 0, 0)},
                    {new SkillData(1, "Hornhaut", "shield", new string[] {"Reduziert den erlittenen Schaden"}, SkillAction.NONE, null, true, false, Color.Orange, 1, 0)},
                    {new SkillData(2, "Tasche", "bag", new string[] {"Ermoeglicht es Gegenstaende zu tragen"}, SkillAction.NONE, null, true, false, Color.Orange, 1, 0)},
                    {new SkillData(3, "gutes Herz I", "heart", new string[] {"Erhoet die maximale Anzahl der Herzen"}, SkillAction.INCREASE_HEALTH_SCALE, SkillUnlockCondition.SINGLE, true, false, Color.Orange, 1, 0, 2)},
                    {new SkillData(4, "Herz aus Hornhaut", "heart", new string[] {"Erhoet die maximale Anzahl der Herzen"}, SkillAction.INCREASE_HEALTH_SCALE, SkillUnlockCondition.MULTIPLE, true, false, Color.Orange, 1, 3, 0, 1)},
                    
                };

                Data.SkillTree = skillTreeData;
                Save(false);
            }

            using (Stream stream = File.OpenRead("playerData.json"))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    string json = decrypt(sr.ReadToEnd());
                    //string json = sr.ReadToEnd();

                    Data = JsonConvert.DeserializeObject<PlayerData>(json);
                    TreeData = Data.SkillTree;

                    return Data;
                }
            }

        }

        public static void Save(bool region)
        {



            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
            serializer.NullValueHandling = NullValueHandling.Ignore;

            if (region)
                Data.Region = Cichorium.SimulationManager.CurrentRegion.Title;

            using (StreamWriter sw = new StreamWriter("playerData.json"))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, Data);
                    
                    writer.Close();
                }
                sw.Close();
            }

            

            string text;

            using (Stream stream = File.OpenRead("playerData.json"))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    text = sr.ReadToEnd();
                }
            }


            encrypt(text);

        }

        private static void encrypt(string text)
        {
            using (var aes = new AesManaged())
            {
                byte[] key = Encoding.ASCII.GetBytes("Oktay ist sexifs");
                aes.Key = key;
                aes.Mode = CipherMode.ECB;

                byte[] encrypted;
                ICryptoTransform crypto = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    // Create crypto stream using the CryptoStream class. This class is the key to encryption    
                    // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream    
                    // to encrypt    
                    using (CryptoStream cs = new CryptoStream(ms, crypto, CryptoStreamMode.Write))
                    {
                        // Create StreamWriter and write data to a stream    
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(text);
                        encrypted = ms.ToArray();

                        File.WriteAllText("playerData.json", Convert.ToBase64String(encrypted));
                    }
                }


            }
        }

        private static string decrypt(string text)
        {
            using (var aes = new AesManaged())
            {
                byte[] key = Encoding.ASCII.GetBytes("Oktay ist sexifs");
                aes.Key = key;
                aes.Mode = CipherMode.ECB;

                byte[] bytes = Convert.FromBase64String(text);

                using (var decryptor = aes.CreateDecryptor())
                {
                    byte[] result = decryptor.TransformFinalBlock(bytes, 0, bytes.Length);
                    string plain = Encoding.ASCII.GetString(result);
                    return plain;
                }
            }
        }

    }
}
