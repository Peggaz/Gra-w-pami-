using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace Game {
    class Program {
        static void Main(string[] args) {
            GameRemamber g = new GameRemamber();
            Thread.Sleep(50000);
        }
    }

    enum isHiden{
        hiden,
        show,
        par_swhow
    }

    class GameRemamber {
        ///wybierz poziom trudnosci i na jego podstawie stworz stany
        private static int NR = 4, NC = 4;
        private State S;
        public GameRemamber() {
            try {
                if (Level().Equals("H")) {
                    NR = NC = 8;
                }
                S = new State(NR, NC);//do zmiany
                ///stworzenie gry
                S.MakeState(FileToArray());
                while(true){
                    ///wypisanie planszy
                    S.PrintBoard();
                    ///input -> wejscie uzytkownika
                    int[] answer = ReadAnswer();
                    Console.WriteLine("debug " + answer[0] + answer[1]);//debug
                    ///sprawdzenie czy jest prawidlowe
                    if (!IsOKIN(answer)) {
                        continue;
                    }
                    ///odkryci wybranej karty
                    ///sprawdzenie czy jest odkryta jego para
                    S.ShowCard(answer);
                    ///sprawdzenie czy jest to wygrana
                    if (S.IsWin()) {
                        S.PrintBoard();
                        Console.WriteLine("Gratulacje wygrałeś naszą grę, ćwicz pamięć dalej ;)");//debug
                        break;
                    }
                }
            }
            catch {
                Console.WriteLine("przykro mi wystąpił błąd, spróbuj jeszcze raz lub odezwij się do mnie po łatkę ;)");//debug
            }
        }

        private bool IsOKIN(int[] i) {
            if (i[0] >= NR && i[1] >= NC)
                return false;
            return true;
        }

        int[] ReadAnswer() {
            string line = Console.ReadLine();
            int[] ret = { 0,0};
            switch (line.Substring(0, 1)) {
                case "A":
                    ret[0] = 0;
                    break;
                case "B":
                    ret[0] = 1;
                    break;
                case "C":
                    ret[0] = 2;
                    break;
                case "D":
                    ret[0] = 3;
                    break;
                case "E":
                    ret[0] = 4;
                    break;
                case "F":
                    ret[0] = 5;
                    break;
                case "G":
                    ret[0] = 6;
                    break;
                case "H":
                    ret[0] = 7;
                    break;
            }
            ret[1] = int.Parse(line.Substring(1, 1));
            ret[1]--;
            return ret;
        }

        List<String> FileToArray() {
            List<String> Words = new List<string>();
            try {
                using (var file = new StreamReader("Words.txt")) {
                    while (file.Peek() >= 0) {
                        string str;
                        str = file.ReadLine();
                        Words.Add(str);

                    }
                    return Words;
                }

            }
            catch (IOException e) {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                return null;
            }
        }


        string Level() {
            try {
                Console.WriteLine("Wybierz poziom trudności ('[E] Easy' or '[H] Hard'):");
                string level = Console.ReadLine();
                return level;
            }
            catch (IOException e) {
                Console.WriteLine("Incorrect data. Enter 'Easy' or 'Hard'.");
                Console.WriteLine(e.Message);
                return null;

            }
        }

    }

    class Field {
        public Field(String s) {
            hiden = isHiden.hiden;
            word = s;
        }
        public isHiden hiden {
            get; set;
        }
        public string word {
            get; set;
        }

    }
    class State {
        //ukryke słowo
        public Field[,] Board;
        public int NR, NC;//liczba wierszy i liczba kolumn
        public State(int nr, int nc) {
            NR = nr; NC = nc;
            Board = new Field[NR,NC];
        } 
        
        public State(State S) {
            Board = new Field[NR,NC];
            for (int i = 0; i < NR; i++)
                for (int j = 0; j < NC; j++)
                    Board[i, j] = S.Board[i,j];

        }

        public void Hello() {
            Console.WriteLine("Hello world");
        }

        public void MakeState(List<String> ss) {//do randomizacji
            int count_words = NR * NC / 2;

            List<String> local_words = new List<String>();
            List<int> local_int = new List<int>();
            foreach(String iterator in ss) {
                if (count_words <= 0)
                    break;
                local_words.Add(iterator);
                local_int.Add(2);
                --count_words;
            }
            var random = new Random();
            
            for (int i = 0; i < NR; i++) {
                for (int j = 0; j < NC; j++) {
                    String str;
                    while (true) {
                        int id = random.Next(0, local_words.Count());
                        if (local_int[id] > 0) {
                            str = local_words[id];
                            local_int[id]--;
                            break;
                        }

                    }
                    Board[i, j] = new Field(str);
                }
            }
        }

        public void PrintBoard() {
            String s = "";
            for (int j = 1; j < NC+1; j++) {
                Console.Write("  " + j);//do zmiany
            }
            for (int i = 0; i < NR; i++) {
                switch (i) {
                    case 0:
                        Console.Write("\n" + "A ");//do zmiany
                        break;
                    case 1:
                        Console.Write("\n" + "B ");//do zmiany
                        break;
                    case 2:
                        Console.Write("\n" + "C ");//do zmiany
                        break;
                    case 3:
                        Console.Write("\n" + "D ");//do zmiany
                        break;
                    case 4:
                        Console.Write("\n" + "E ");//do zmiany
                        break;
                    case 5:
                        Console.Write("\n" + "F ");//do zmiany
                        break;
                    case 6:
                        Console.Write("\n" + "G ");//do zmiany
                        break;
                    case 7:
                        Console.Write("\n" + "H ");//do zmiany
                        break;
                }
                
                for (int j = 0; j < NC; j++) {
                    if (Board[i, j].hiden != isHiden.hiden) {
                        Console.Write(Board[i, j].word + " ");
                    }
                    else {
                        Console.Write("X  ");
                    }
                }
            }
            Console.Write("\n");

        }
        
        public void ShowCard(int[]xy) {
            Console.Clear();
            int[] old_show = {0, 0};
            bool is_show = false;
            for (int i = 0; i < NR; i++) {
                for (int j = 0; j < NC; j++) {
                    if (Board[i, j].hiden == isHiden.show) {
                        is_show = true;
                        old_show[0] = i;
                        old_show[1] = j;
                        if ((i != xy[0] || j != xy[1]) && Board[xy[0], xy[1]].word.Equals(Board[i, j].word)) {
                            Board[xy[0], xy[1]].hiden = isHiden.par_swhow;
                            Board[i, j].hiden = isHiden.par_swhow;
                        } else {
                            Board[i, j].hiden = isHiden.hiden;
                        }
                        break;
                    }
                }
                if (is_show) {
                    break;
                }
            }
            if(Board[xy[0], xy[1]].hiden != isHiden.par_swhow)
                Board[xy[0], xy[1]].hiden = isHiden.show;
        }

        public bool IsWin() {
            for (int i = 0; i < NR; i++) {
                for (int j = 0; j < NC; j++) {
                    if (Board[i, j].hiden == isHiden.hiden)
                        return false;
                }
            }
            return true;
        }
    }        
}
