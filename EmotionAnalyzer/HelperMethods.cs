using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
namespace SentEmo
{
    class HelperMethods
    {
        public static string low_registry(string s)
        {
            string new_s = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] >= 'A' && s[i] <= 'Z') new_s += (char)('a' + s[i] - 'A');
                else new_s += s[i];
            }
            return new_s;
        }

        public static void CreateResourceInFileSystem(string name)
        {
            using (var fileStream = File.Create(name))
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SentEmo.ModelsAndData." + name))
                {
                    stream.CopyTo(fileStream);
                }
            }
        }

        public static Dictionary<string, bool> import_positive_words()
        {
            StreamReader input;
            var positive_words = new Dictionary<string, bool>(); //here will be placed positive words
            try
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SentEmo.ModelsAndData." + StaticResourcePaths.positive_path))
                {
                    input = new StreamReader(stream);
                    while (true)
                    {
                        string line = input.ReadLine();
                        if (line == null) break;
                        if (line.Length < 2) continue;
                        if (!positive_words.ContainsKey(line))
                            positive_words.Add(line, true);
                    }
                    input.Close();
                    return positive_words;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Positive Words format is not correct", e);
            }
        }
        public static Dictionary<string, bool> import_negative_words()
        {
            System.IO.StreamReader input;
            var negative_words = new Dictionary<string, bool>(); //here will be placed negative words
            try
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SentEmo.ModelsAndData." + StaticResourcePaths. negative_path))
                {
                    input = new System.IO.StreamReader(stream);
                    while (true)
                    {
                        string line = input.ReadLine();
                        if (line == null) break;
                        if (line.Length < 2) continue;
                        if (!negative_words.ContainsKey(line)) negative_words.Add(line, true);
                    }
                    input.Close();
                    return negative_words;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static Dictionary<string, int[]> import_emotion_words()
        {
            System.IO.StreamReader input;
            try
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SentEmo.ModelsAndData." + StaticResourcePaths. emotions_path))
                {
                    input = new System.IO.StreamReader(stream);
                    var emotion_words = new Dictionary<string, int[]>(); //here will be placed imported words
                    while (true)
                    {
                        string line = input.ReadLine();
                        if (line == null) break;
                        if (line.Length < 2) continue;
                        string[] arr = line.Split(' ');
                        int[] val = new int[7];
                        val[0] = arr[1][0] - '0'; //anger
                        val[1] = arr[2][0] - '0'; //disgust
                        val[2] = arr[3][0] - '0'; //fear
                        val[3] = arr[4][0] - '0'; // joy
                        val[4] = arr[7][0] - '0'; //sadness
                        val[5] = arr[5][0] - '0'; //negative
                        val[6] = arr[6][0] - '0'; //positive
                        if (!emotion_words.ContainsKey(arr[0]))
                        {


                            emotion_words.Add(arr[0], val);

                        }
                    }
                    input.Close();
                    return emotion_words;
                }
            }
            catch (Exception e)
            {
                throw new Exception("EMotion Words format is not correct", e);
            }

        }
        public static Dictionary<string, int> import_inclusion_words()
        {
            System.IO.StreamReader input;
            try
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SentEmo.ModelsAndData." +StaticResourcePaths.inclusion_path))
                {
                    input = new System.IO.StreamReader(stream);
                    var inclusion_words = new Dictionary<string, int>(); //here will be placed imported words
                    while (true)
                    {
                        string line = input.ReadLine();
                        if (line == null) break;
                        if (line.Length < 2) continue;
                        string[] arr = line.Split(' ');
                        int val = 1;
                        if (arr.Count() > 1) val = arr[1][0] - '0';
                        if (!inclusion_words.ContainsKey(arr[0]))
                            inclusion_words.Add(arr[0], val);
                    }
                    input.Close();
                    return inclusion_words;
                }
            }
            catch (Exception e)
            {
                throw new Exception("EMotion Words format is not correct", e);
            }
        }
        public static Dictionary<string, int> import_exclusion_words()
        {
            System.IO.StreamReader input;
            try
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SentEmo.ModelsAndData." + StaticResourcePaths.exclusion_path))
                {
                    input = new System.IO.StreamReader(stream);
                    var exclusion_words = new Dictionary<string, int>(); //here will be placed imported words
                    while (true)
                    {
                        string line = input.ReadLine();
                        if (line == null) break;
                        if (line.Length < 2) continue;
                        string[] arr = line.Split(' ');
                        int val = 1;
                        if (arr.Count() > 1) val = arr[1][0] - '0';
                        if (!exclusion_words.ContainsKey(arr[0]))
                            exclusion_words.Add(arr[0], val);
                    }
                    input.Close();
                    return exclusion_words;
                }
            }
            catch (Exception e)
            {
                throw new Exception("EMotion Words format is not correct", e);
            }
        }
    }
}
