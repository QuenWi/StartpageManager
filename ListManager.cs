
using System.Xml.Linq;

namespace ListManager
{
    public class ListManager {
        static List<string> list;

        public static void Main()
        {
            Console.WriteLine("starting...");
            TxtManager.TxtManager.setup();
            list = TxtManager.TxtManager.readTxt();
            menue_main();
        }

        static void menue_main()
        {
            Console.WriteLine("");
            int input = getUserIntegerInput("Choose option:\n1: Show list of all elements.\n2: Convert list to a .html file.\n9: Help/Info");
            if (input == 1)
            {
                menue_list();
            }
            else
            {
                if(input == 2)
                {
                    HTMLmanager.HTMLmanager.convertListIntoHTML(list);
                }
                if (input == 9)
                {
                    Console.WriteLine("coming soon");
                    Console.WriteLine("Inside the list:" 
                        + "\n\"#\": Means, that it will be a headline in the .html file. (#\"Headline Text\" \"Time, when it will be active\")"
                        + "\n\"-\": Means, that it will be a link in the .html file. (-\"Link Title\" \"Link link\" \"Time, when it will be active\")");
                }
                menue_main();
            }
        }

        static void menue_list()
        {
            Console.WriteLine("");
            printList();
            int input = getUserIntegerInput("Choose option:\n1: Go back and save list.\n6: Add element.\n7: Remove element.\n8: Change element.\n9: Move element.");
            if(input == 1)
            {
                TxtManager.TxtManager.writeTxt(list);
                menue_main();
            }
            else
            {
                if(input == 6)
                {
                    Console.WriteLine();
                    addElement();
                }
                else if (input == 7)
                {
                    Console.WriteLine();
                    deleteElement();
                }
                else if (input == 8)
                {
                    Console.WriteLine();
                    changeElementSettings();
                }
                else if (input == 9)
                {
                    Console.WriteLine();
                    moveElement();
                }
                menue_list();
            }
        }

        static int getUserIntegerInput(string menue)
        {
            while (true)
            {
                Console.WriteLine(menue);
                int input;
                try
                {
                    input = int.Parse(Console.ReadLine());
                    return input;
                }
                catch
                {
                    Console.WriteLine("Couldn't detect a number. Please try again.");
                }
            }
        }

        static void printList()
        {
            int counter = 1;
            foreach(string s in list)
            {
                if (s.Length > 0)
                {
                    Console.WriteLine(counter.ToString() + ":\t" + s);
                }
                else
                {
                    Console.WriteLine(counter.ToString() + "\t");
                }
                counter++;
            }
            Console.WriteLine();
        }

        static void addElement()
        {
            string s = getUserWantedElementToAdd();
            addElementInListAtUserWantedPosition(s);
        }

        static void deleteElement()
        {
            while (true)
            {
                int position = getUserIntegerInput("What element in which position should get deleted?:") - 1;
                if (position < list.Count() && position > -1)
                {
                    list.RemoveAt(position);
                    break;
                }
                Console.WriteLine("The position was not available or couldn't be detected.");
            }
        }

        static void changeElementSettings()
        {
            while (true)
            {
                int position = getUserIntegerInput("What element in which position should get changed?:") - 1;
                if (position < list.Count() && position > -1)
                {
                    if (list[position].Length > 0)
                    {
                        if (list[position].StartsWith('#'))
                        {
                            Console.WriteLine();
                            changeElementHeadline(position);
                        }
                        if (list[position].StartsWith('-'))
                        {
                            Console.WriteLine();
                            changeElementLink(position);
                        }
                        break;
                    }
                }
                Console.WriteLine("The position was not available or couldn't be detected.");
            }
        }

        static void moveElement()
        {
            while (true)
            {
                int position = getUserIntegerInput("What element you want to move?:") - 1;
                if (position < list.Count() && position > -1)
                {
                    while (true)
                    {
                        int position2 = getUserIntegerInput("Where you want to move the element to?:") - 1;
                        if (addElementInListAtPosition(list[position], position2))
                        {
                            if (position2 <= position)
                            {
                                position++;
                            }
                            break;
                        }
                    }
                    list.RemoveAt(position);
                    break;
                }
                Console.WriteLine("The position was not available or couldn't be detected.");
            }
        }

        static string getUserWantedElementToAdd()
        {
            string s = "";
            string input;
            while (true)
            {
                Console.WriteLine("What you want to add?:\n1: Headline\n2: Link\n9: Empty line in list");
                input = Console.ReadLine();
                if (input == "1")
                {
                    s += "#";
                    Console.WriteLine("What you want the headline to say?:");
                    input = Console.ReadLine().Replace(" ", "_");
                    s += "\"" + input + "\" ";
                    s += getUserInputTime();
                    break;
                }
                else if (input == "2")
                {
                    s += "-";
                    Console.WriteLine("What you want the link to say?:");
                    input = Console.ReadLine().Replace(" ", "_");
                    s += "\"" + input + "\" ";
                    Console.WriteLine("What you want the link to link to?:");
                    input = Console.ReadLine().Replace(" ", "_");
                    s += "\"" + input + "\" ";
                    s += getUserInputTime();
                    break;
                }
                else if (input == "9")
                {
                    break;
                }
                Console.WriteLine("The number couldn't be detected.");
            }
            return s;
        }

        static string getUserInputTime()
        {
            string input;
            while (true)
            {
                Console.WriteLine("Should it have time restrictions? (Example: \"11:00-14:00\").\nIf not, just press enter without input.:");
                input = Console.ReadLine();
                if (input.Length >= 7 && input.Length <= 11)
                {
                    int[] numbers = new int[4];
                    try{
                        numbers[0] = int.Parse(input.Split("-")[0].Split(":")[0]);
                        numbers[1] = int.Parse(input.Split("-")[0].Split(":")[1]);
                        numbers[2] = int.Parse(input.Split("-")[1].Split(":")[0]);
                        numbers[3] = int.Parse(input.Split("-")[1].Split(":")[1]);
                        if (numbers[0] > -1 && numbers[0] < 24 && numbers[2] > -1 && numbers[2] < 24
                        && numbers[1] > -1 && numbers[1] < 60 && numbers[3] > -1 && numbers[3] < 60)
                        {
                            return "\"" + input + "\" ";
                            break;
                        }
                        else
                        {
                            Console.WriteLine("The timeinterval couldn't be detected!");
                        }
                    }
                    catch
                    {
                        Console.WriteLine("The timeinterval couldn't be detected!");
                    }
                }
                else
                {
                    return "\"\"";
                    break;
                }
            }
        }

        static void addElementInListAtUserWantedPosition(string s)
        {
            while (true)
            {
                int position = getUserIntegerInput("What position should the element be in?:") - 1;
                if(addElementInListAtPosition(s, position))
                {
                    break;
                }
            }
        }

        static bool addElementInListAtPosition(string s, int position)
        {
            if (position == list.Count())
            {
                list.Add(s);
                return true;
            }
            else if (position < list.Count() && position > -1)
            {
                List<string> newlist = new List<string>();
                for (int i = 0; i < list.Count(); i++)
                {
                    if (i == position)
                    {
                        newlist.Add(s);
                        i--;
                        position = -1;
                    }
                    else
                    {
                        newlist.Add(list[i]);
                    }
                }
                list = newlist;
                return true;
            }
            Console.WriteLine("The position was not available or couldn't be detected.");
            return false;
        }

        static void changeElementHeadline(int position)
        {
            Console.WriteLine("Given is the Headline: ");
            Console.WriteLine(list[position].Substring(1));
            int choice = getUserIntegerInput("What part you want to change ? (1 - 2), (0: stop changing element):");
            if(choice > 0 && choice < 3)
            {
                string[] stringParts = list[position].Split(" ");
                if (choice == 1)
                {
                    stringParts[0] = "#" + getUserChangingTo();
                }
                else if (choice == 2)
                {
                    stringParts[1] = getUserInputTime();
                }
                list[position] = stringParts[0] + " " + stringParts[1];
            }
            if(choice != 0)
            {
                changeElementHeadline(position);
            }
        }

        static void changeElementLink(int position)
        {
            Console.WriteLine("Given is the Link: ");
            Console.WriteLine(list[position].Substring(1));
            Console.WriteLine("What part you want to change? (1-3): ");
            int choice = int.Parse(Console.ReadLine());
            if (choice > 0 && choice < 4)
            {
                string[] stringParts = list[position].Split(" ");
                if (choice == 1)
                {
                    stringParts[0] = "-" + getUserChangingTo();
                }
                else if (choice == 2)
                {
                    stringParts[1] = getUserChangingTo();
                }
                else if (choice == 3)
                {
                    stringParts[2] = getUserInputTime();
                }
                list[position] = stringParts[0] + " " + stringParts[1] + " " + stringParts[2];
            }
            if (choice != 0)
            {
                changeElementLink(position);
            }
        }

        static string getUserChangingTo()
        {
            Console.WriteLine("What you want to change it into: ");
            string userInput = Console.ReadLine().Replace(" ", "_");
            return "\"" + userInput +"\"";
        }
    }
}