//13 юли 2011, вариант 2
using System.Globalization;

namespace PU2011
{
    class Visit
    {
        private int _dayOfWeek;
        private int _bytes;
        private string _pageName;
        private string _username;

        public DateTime TimeOfVisit { get; private set; }
        public int DayOfWeek
        {
            get
            {
                return _dayOfWeek;
            }
            private set
            {
                // value е стойността която се подава от потребителя; същото като set(value) метод, но е запазена дума
                if (value >= 1 && value <= 7)
                {
                    _dayOfWeek = value;
                }
                else
                {                       // $ е интерполация - "текст{променлива}текст"; вместо да имам "текст" + променлива + "текст"
                    throw new Exception($"{value} is not a valid day of the week. Please enter a valid number - [1-7].");
                }
            }
        }
        public int Bytes
        {
            get
            {
                return _bytes;
            }
            private set
            {
                if (value > 0)
                {
                    _bytes = value;
                }
                else
                {
                    throw new Exception("Please enter a valid number (greater than 0).");
                }
            }
        }
        public string PageName
        {
            get
            {
                return _pageName;
            }
            set
            {
                if (value.Length > 50 || String.IsNullOrEmpty(value))
                {
                    throw new Exception("Please enter a valid name for a page (1-50 characters).");
                }
                else
                {
                    _pageName = value;
                }
            }
        }
        public string Username
        {
            get
            {
                return _username;
            }
            private set
            {
                if (value.Length > 20 || String.IsNullOrEmpty(value))
                {
                    throw new Exception("Please enter a valid username (1-20 characters).");
                }
                else
                {
                    _username = value;
                }
            }
        }

        public Visit(DateTime timeOfVisit, int dayOfWeek, int bytes, string pageName, string username)
        {
            TimeOfVisit = timeOfVisit;
            DayOfWeek = dayOfWeek;
            Bytes = bytes;
            PageName = pageName;
            Username = username;
        }

        public override string ToString()
        {
            // (1) Bytes / 1024.0:f3 - .0 за да изчислява с тип double иначе ще получим 3666/1024=3
            //  :f3 форматира до 3тия знак след запетаята
            //                                                                       (1)
            return $"{TimeOfVisit} - {DayOfWeekToString(DayOfWeek)}, {Username}, {Bytes / 1024.0:f3} KB, {PageName}";
        }

        // методът е статичен (и public, иначе винаги private/protected) за да може да го извикам с името на класа по-долу, но не смятам че е добра практика, просто не исках да имам един и същ код два пъти
        public static string DayOfWeekToString(int n)
        {
            switch (n)
            {
                case 1: return "Понеделник";
                case 2: return "Вторник";
                case 3: return "Сряда";
                case 4: return "Четвъртък";
                case 5: return "Петък";
                case 6: return "Събота";
                case 7: return "Неделя";
            }

            return null;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            int n = 0;
            do
            {
                //този ред винаги ще се изпълни поне веднъж, така работи do-while
                n = int.Parse(Console.ReadLine());

                    //проверка дали n отговаря на условието [1-1000]
            } while (n < 1 && n > 1000);

            // винаги когато имаш много данни за едно нещо е най-лесно да ги обединиш в клас (Visit)
            List<Visit> visits = new List<Visit>();

            for (int i = 0; i < n; i++)
            {
                Console.WriteLine("Enter date:");
                //                                  очаква дата във формат "dd.MM.yyyy HH:mm:ss" иначе хвърля грешка
                DateTime date = DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                Console.WriteLine("Enter day of the week:");
                int dayOfWeek = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter bytes:");
                int bytes = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter visited page name:");
                string page = Console.ReadLine();
                Console.WriteLine("Enter username:");
                string username = Console.ReadLine();
                var curr = new Visit(date, dayOfWeek, bytes, page, username);
                visits.Add(curr);
            }
            // тук сортираме листа по дата
            var ordered = visits.OrderBy(x => x.TimeOfVisit).ToList();
            // печатаме го на конзолата, същото е като долния foreach просто е на един ред
            Console.WriteLine(String.Join(Environment.NewLine, ordered.Select(x => x.ToString())));


            Console.WriteLine("Enter username:");
            var usernameFilter = Console.ReadLine();
            // сортираме по условие
            var userFiltered = visits.Where(x => x.Username == usernameFilter).OrderBy(x => x.PageName).ThenByDescending(x => x.Bytes).ToList();
            foreach (var visit in userFiltered)
            {
                Console.WriteLine(visit.ToString());
            }

            // това създава обект от тип IOrderedEnumerable<IGrouping<int,Visit>> или просто списък с ключове дни от седмицата и срещу всеки ключ съответния брой посещения
            // примерно ако извикаш day.Key ще ти върне 1,2,3... какъвто е деня
            // а когато достъпиш методите на day ще забележиш че това е като List<Visit> и следователно си работиш сякаш имаш лист
            var daysOfWeekSorted = visits.GroupBy(x=>x.DayOfWeek).OrderBy(x=>x.Key);
            
            foreach (var day in daysOfWeekSorted)
            {                       //така се извикват статични методи
                Console.WriteLine($"{Visit.DayOfWeekToString(day.Key)}, {day.Count()} посещения, {day.Sum(x=>x.Bytes)} KB");
            }
            

        }
    }
}
