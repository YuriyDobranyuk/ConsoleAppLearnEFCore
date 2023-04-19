using ConsoleAppLearnEFCore.Interface;

namespace ConsoleAppLearnEFCore.Menu
{
    public class BaseMenu
    {
        public IServiceLibrary _serviceLibrary;
        
        public BaseMenu() { }

        public BaseMenu(IServiceLibrary serviceLibrary) 
        {
            _serviceLibrary = serviceLibrary;
        }

        public void DisplayTitle(string title)
        {
            Console.WriteLine(new string('-', 20));
            Console.WriteLine($"{title}");
            Console.WriteLine(new string('-', 20));
        }

        public int GetUserSelection()
        {
            Console.WriteLine(new string('-', 10));
            Console.WriteLine($"Enter number, please:");
            if (!int.TryParse(Console.ReadLine(), out var number))
                GetUserSelection();
            Console.WriteLine();
            return number;
        }

        public void EnterKeyForContinueWork()
        {
            Console.WriteLine();
            Console.WriteLine("For continue enter key \"enter\"");
            Console.ReadLine();
        }

        public bool ConfirmDeleteItem(string objectName)
        {
            Console.WriteLine($"For confirm delete {objectName}, enter number 1.");
            Console.WriteLine($"For continue enter anywhere key");
            var result = false;
            var selection = GetUserSelection();
            if (selection == 1) result = true;
            return result;
        }

        public string EnterPropertyValue(string propertyName, string objectName, bool required)
        {
            Console.WriteLine(new string('_', 10));
            Console.WriteLine($"Enter {propertyName} {objectName}, please:");
            var enterName = Console.ReadLine();
            if (string.IsNullOrEmpty(enterName) && required) EnterPropertyValue(propertyName, objectName, required);
            return enterName;
        }
        public int EnterIntPropertyValue(string nameParam)
        {
            Console.WriteLine(new string('*', 10));
            Console.WriteLine($"Enter {nameParam}, please:");
            var value = 0;
            if (!(int.TryParse(Console.ReadLine(), out value) && value > 0)) EnterIntPropertyValue(nameParam);
            return value;
        }

        public string ChoosePosition(string objectName)
        {
            Console.WriteLine(new string('*', 10));
            Console.WriteLine($"Enter the {objectName} number separated by a comma, please:");
            return Console.ReadLine();
        }

        public List<int> MakeListPositions(string positionString, int maxCountPosition = 0)
        {
            var arrayPositions = positionString.Split(new char[] { ',' });
            var listPositions = new List<int>();
            var num = 0;
            foreach (var position in arrayPositions)
            {
                if (int.TryParse(position, out num) && num > 0 && num <= maxCountPosition)
                {
                    listPositions.Add(num);
                }
            }
            return listPositions;
        }

        public List<T> MakeListChooses<T>(List<int> positions, IList<T>? elements)
        {
            var chooseElements = new List<T>();
            T chooseElement;
            foreach (var position in positions)
            {
                chooseElement = elements.ElementAtOrDefault(position - 1);
                if(chooseElement != null) chooseElements.Add(chooseElement);
            }
            return chooseElements;
        }

        public bool ChooseEditOrNotParams(string nameParams)
        {
            int number;
            var isEdit = false;
            Console.WriteLine($"If you want edit \"{nameParams}\", enter number 1, please:");
            if (int.TryParse(Console.ReadLine(), out number) && number == 1) isEdit = true;
            return isEdit;
        }
        
    }
}
