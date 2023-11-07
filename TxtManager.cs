
namespace TxtManager
{
    public class TxtManager
    {
        public static string txtPath;

        public static void setup()
        {
            txtPath = Directory.GetCurrentDirectory();
            txtPath = txtPath.Substring(0, txtPath.LastIndexOf("\\"));
            txtPath += "\\StartpageTemplates" + "\\List.txt";
        }

        public static List<String> readTxt()
        {
            if (File.Exists(txtPath))
            {
                return File.ReadAllLines(txtPath).ToList();
            }
            else
            {
                Console.WriteLine("Mistake. The following file wasn't found: " + txtPath);
                Console.ReadLine();
                Environment.Exit(0);
                return null;
            }
        }

        public static void writeTxt(List<String> txt)
        {
            File.WriteAllLines(txtPath, txt);
        }
    }
}