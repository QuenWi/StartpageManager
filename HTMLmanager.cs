
namespace HTMLmanager
{
    public class HTMLmanager
    {
        static string pathHTMLfile;
        static string pathTemplates;
        static int counter;

        public static void convertListIntoHTML(List<String> list) {
            setPaths();
            using (StreamWriter streamWriter = new StreamWriter(pathHTMLfile))
            {
                writeFileInStream(streamWriter, pathTemplates + "1_Header.html");
                writeAllElements(streamWriter, list);
                writeFileInStream(streamWriter, pathTemplates + "2_Footer.html");
            }
        }

        public static void setPaths()
        {
            string path = Directory.GetCurrentDirectory();
            path = path.Substring(0, path.LastIndexOf("\\"));
            pathHTMLfile = path + "\\Startpage.html";
            pathTemplates = path + "\\StartpageTemplates\\";
        }

        public static void writeFileInStream(StreamWriter streamWriter, string path)
        {
            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                streamWriter.WriteLine(line);
            }
        }

        public static void writeAllElements(StreamWriter streamWriter, List<String> list)
        {
            string pathHeadline = pathTemplates + "p_Headline.html";
            string pathLink = pathTemplates + "p_Links.html";
            string codeHeadline = File.ReadAllText(pathHeadline);
            string codeLink = File.ReadAllText(pathLink);
            counter = 0;
            foreach(string line in list)
            {
                if (line.StartsWith('#'))
                {
                    string addLine = codeHeadline;
                    string[] elementData = line.Split(' ');
                    addLine = addLine.Replace("{Headline}", cutElementData(elementData[0]));
                    addLine = replaceTimeInString(addLine, elementData[1]);
                    streamWriter.WriteLine(addLine);
                } else if (line.StartsWith('-'))
                {
                    string addLine = codeLink;
                    string[] elementData = line.Split(' ');
                    addLine = addLine.Replace("{Text}", cutElementData(elementData[0]));
                    addLine = addLine.Replace("{Link}", cutElementData(elementData[1]));
                    addLine = replaceTimeInString(addLine, elementData[2]);
                    streamWriter.WriteLine(addLine);
                }
            }
        }

        static string cutElementData(string elementData)
        {
            return elementData.Substring(elementData.IndexOf("\"")+1, elementData.LastIndexOf("\"") - elementData.IndexOf("\"") - 1);
        }

        static string replaceTimeInString(string addLine, string elementDataTime)
        {
            if (elementDataTime.Length > 5)
            {
                elementDataTime = cutElementData(elementDataTime);
                elementDataTime = elementDataTime.Replace(":", ",");
                elementDataTime = elementDataTime.Replace("-", ",");
                addLine = addLine.Replace("{Time}","id=\"timeID" + counter + "\"");
                addLine += " <script>window.onload = blockAtTime(" + elementDataTime + ",\"timeID" + counter + "\");</script>";
                counter++;
            }
            else
            {
                addLine = addLine.Replace(" {Time}", "");
            }
            return addLine;
        }
    }
}
