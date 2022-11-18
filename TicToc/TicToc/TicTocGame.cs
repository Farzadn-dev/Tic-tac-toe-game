using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace TicToc
{
    public class TicTocGame
    {
        private static bool isPlaying = true;

        const int X = 32;
        const int Y = 14;

        const char H_Wall = '-';
        const char V_Wall = '|';
        static Players WhoPlays;

        static char[,] bord = new char[Y, X];

        public static void SetBored(int x, int y)
        {
            Console.WindowHeight = x + 4;
            Console.WindowWidth = y;
        }

        public static bool CheckEnd(ref List<PlayerMove> playerMoves, out Players winer)
        {
            List<PlayerMove> PlayerX = playerMoves.Where(c => c.Player == Players.X).ToList();
            List<PlayerMove> PlayerO = playerMoves.Where(c => c.Player == Players.O).ToList();
            if (isLine(PlayerX.Select(c => c.MoveIndex).ToArray()))
            {
                winer = Players.X;
                return true;
            }
            if (isLine(PlayerO.Select(c => c.MoveIndex).ToArray()))
            {
                winer = Players.O;
                return true;
            }
            if (isDraw(ref playerMoves))
            {
                winer = Players.Draw;
                return true;
            }
            winer = Players.Draw;
            return false;
        }
        public static bool isLine(params int[] nums)
        {
            if (nums.Any(c => c == 7) && nums.Any(c => c == 8) && nums.Any(c => c == 9))
                return true;

            else if (nums.Any(c => c == 4) && nums.Any(c => c == 5) && nums.Any(c => c == 6))
                return true;

            else if (nums.Any(c => c == 1) && nums.Any(c => c == 2) && nums.Any(c => c == 3))
                return true;

            else if (nums.Any(c => c == 1) && nums.Any(c => c == 4) && nums.Any(c => c == 7))
                return true;

            else if (nums.Any(c => c == 2) && nums.Any(c => c == 5) && nums.Any(c => c == 8))
                return true;

            else if (nums.Any(c => c == 3) && nums.Any(c => c == 6) && nums.Any(c => c == 9))
                return true;

            else if (nums.Any(c => c == 1) && nums.Any(c => c == 5) && nums.Any(c => c == 9))
                return true;

            else if (nums.Any(c => c == 7) && nums.Any(c => c == 5) && nums.Any(c => c == 3))
                return true;

            return false;
        }
        public static bool isDraw(ref List<PlayerMove> playerMoves) => playerMoves.Count >= 9 ? true : false;

        public static void CreateBord(ref char[,] bord)
        {
            for (int y = 0; y < Y - 1; y++)
            {
                for (int x = 0; x < X - 1; x++)
                {
                    if (x == 0 || x == X || x % (X / 3) == 0)
                        bord[y, x] = V_Wall;
                    else if (y == 0 || y == Y || y % (Y / 3) == 0)
                        bord[y, x] = H_Wall;
                    else
                        bord[y, x] = ' ';
                }
            }
        }

        public static Players GetPlayerRandom()
        {
            Random rand = new Random();
            return rand.Next(0, 2) == 0 ? Players.X : Players.O;
        }

        public static void AI_Play()
        {
            Console.Clear();
            WhoPlays = Players.O;
            SetBored(Y, X);
            CreateBord(ref bord);

            Console.WriteLine("Turn: " + WhoPlays.ToString());
            Console.WriteLine("--------|\n");
            ShowBord(ref bord);

            List<PlayerMove> playerMoves = new List<PlayerMove>();
            Players winer;

            while (!CheckEnd(ref playerMoves, out winer))
            {
                var p = Input();
                if (playerMoves.Any(m => m.MoveIndex == p.MoveIndex))
                    continue;

                playerMoves.Add(p);

                var p2 = GetFreeRandomPlace(ref playerMoves);//AI move
                playerMoves.Add(p2);

                Console.Clear();
                UpdateBord(ref bord, p.Player, p.MoveIndex);
                UpdateBord(ref bord, p2.Player, p2.MoveIndex);

                Console.WriteLine("Turn: " + WhoPlays.ToString());
                Console.WriteLine("--------|\n");
                ShowBord(ref bord);
            }

            Console.Clear();
            if (winer == Players.Draw)
            {
                Console.WriteLine("Oops! game is Draw!");
            }
            else
            {
                Console.WriteLine("Congratulation Player " + winer.ToString() + "\nyou are Winer!!");
            }
            Console.WriteLine("Show Bord of \nbefor Game Press 'b'!");
            Console.WriteLine("Press 'a' to Play Again!");
            Console.WriteLine("and Press 'e' to Exit!");

            char c = Console.ReadKey(true).KeyChar;
            switch (c)
            {
                case 'b':
                    {
                        Console.Clear();
                        Console.WriteLine("Winer: " + winer.ToString());
                        Console.WriteLine("--------|\n");
                        ShowBord(ref bord);
                        Console.WriteLine("Press 'a' to Play Again!");
                        Console.WriteLine("and Press 'e' to Exit!");
                        char cc = Console.ReadKey(true).KeyChar;
                        if (cc == 'e')
                            goto case 'e';

                        break;
                    }
                case 'e':
                    {
                        isPlaying = false;
                        break;
                    }
                default:
                    break;
            }
        }

        public static void Play()
        {
            while (isPlaying)
            {
                Console.Clear();
                WhoPlays = GetPlayerRandom();
                SetBored(Y, X);
                CreateBord(ref bord);

                Console.WriteLine("Turn: " + WhoPlays.ToString());
                Console.WriteLine("--------|\n");
                ShowBord(ref bord);

                List<PlayerMove> playerMoves = new List<PlayerMove>();
                Players winer;

                while (!CheckEnd(ref playerMoves, out winer))
                {
                    var p = Input();
                    if (playerMoves.Any(m => m.MoveIndex == p.MoveIndex))
                        continue;

                    playerMoves.Add(p);
                    Console.Clear();
                    UpdateBord(ref bord, p.Player, p.MoveIndex);

                    WhoPlays = p.Player == Players.X ? Players.O : Players.X;

                    Console.WriteLine("Turn: " + WhoPlays.ToString());
                    Console.WriteLine("--------|\n");
                    ShowBord(ref bord);
                }

                Console.Clear();
                if (winer == Players.Draw)
                {
                    Console.WriteLine("Oops! game is Draw!");
                }
                else
                {
                    Console.WriteLine("Congratulation Player " + winer.ToString() + "\nyou are Winer!!");
                }
                Console.WriteLine("Show Bord of \nbefor Game Press 'b'!");
                Console.WriteLine("Press 'a' to Play Again!");
                Console.WriteLine("and Press 'e' to Exit!");

                char c = Console.ReadKey(true).KeyChar;
                switch (c)
                {
                    case 'b':
                        {
                            Console.Clear();
                            Console.WriteLine("Winer: " + winer.ToString());
                            Console.WriteLine("--------|\n");
                            ShowBord(ref bord);
                            Console.WriteLine("Press 'a' to Play Again!");
                            Console.WriteLine("and Press 'e' to Exit!");
                            char cc = Console.ReadKey(true).KeyChar;
                            if (cc == 'e')
                                goto case 'e';

                            break;
                        }
                    case 'e':
                        {
                            isPlaying = false;
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        public static PlayerMove Input()
        {
            PlayerMove move = new PlayerMove();
            while (true)
            {
                char c = Console.ReadKey(true).KeyChar;
                if (c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9')
                {
                    move.Player = WhoPlays;
                    move.MoveIndex = int.Parse(c.ToString());
                    break;
                }
                Console.Clear();
                Console.WriteLine("Please just use these:\n1,2,3,4,5,6,7,8,9");
                Thread.Sleep(3000);
                Console.Clear();
                Console.WriteLine("Turn: " + WhoPlays.ToString());
                Console.WriteLine("--------|\n");
                ShowBord(ref bord);
            }
            return move;
        }

        public static void ShowBord(ref char[,] bord)
        {
            for (int y = 0; y < Y; y++)
            {
                for (int x = 0; x < X; x++)
                {
                    if (bord[y, x] == 'r')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(' ');
                        continue;
                    }
                    else if (bord[y, x] == 'b')
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(' ');
                        continue;
                    }
                    else if (bord[y, x] == 'e')
                    {
                        Console.ResetColor();
                        Console.Write(' ');
                        continue;
                    }

                    Console.Write(bord[y, x]);
                }
                Console.WriteLine("");
            }
        }

        public static void UpdateBord(ref char[,] bord, Players player, int numberOfIndex)
        {
            int x;
            int y;
            GetXAndY(numberOfIndex, out x, out y);

            int countVW = X / 3;
            int countHW = Y / 3;

            x *= countVW;
            x -= 5;

            y *= countHW;
            y -= 2;

            if (player == Players.O)
            {
                bord[y, x] = '_';
                bord[y, x + 1] = ')';
                bord[y, x - 1] = '(';
                bord[y, x - 2] = 'b';//blue color
                bord[y, x + 2] = 'e';//end color
                // (_)
            }
            else if (player == Players.X)
            {
                bord[y, x] = 'X';
                bord[y, x - 1] = 'r';//red color
                bord[y, x + 1] = 'e';//end color
                // X
            }
        }

        private static void GetXAndY(int numberOfIndex, out int x, out int y)
        {
            switch (numberOfIndex)
            {
                case 9:
                    {
                        x = 3;
                        y = 1;
                        break;
                    }
                case 8:
                    {
                        x = 2;
                        y = 1;
                        break;
                    }
                case 7:
                    {
                        x = 1;
                        y = 1;
                        break;
                    }
                case 4:
                    {
                        x = 1;
                        y = 2;
                        break;
                    }
                case 5:
                    {
                        x = 2;
                        y = 2;
                        break;
                    }
                case 6:
                    {
                        x = 3;
                        y = 2;
                        break;
                    }
                case 3:
                    {
                        x = 3;
                        y = 3;
                        break;
                    }
                case 2:
                    {
                        x = 2;
                        y = 3;
                        break;
                    }
                case 1:
                    {
                        x = 1;
                        y = 3;
                        break;
                    }
                default:
                    x = 0;
                    y = 0;
                    break;
            }
        }

        private static PlayerMove GetFreeRandomPlace(ref List<PlayerMove> moves)
        {
            if (moves.Count == 9)
                return new PlayerMove();
            PlayerMove p = new PlayerMove();
            p.Player = WhoPlays == Players.X ? Players.O : Players.X;

            Random r = new Random();
            int index;

            do
            {
               index = r.Next(1,10);
            } while (moves.Any(c=>c.MoveIndex == index));

            p.MoveIndex = index;
            return p;
        }

    }
}
