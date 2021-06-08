using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Sokoban
{
    class Program
    {
        #region StartProgram

        static public void DeserializeFile(out Game game)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Map>));
            List<Map> maps = null;
            using (Stream stream = File.OpenRead("maps.xml"))
            {
                maps = (List<Map>)xmlSerializer.Deserialize(stream);
            }
            List<Level> levels = new List<Level>();
            int i = 1;
            foreach (var map in maps)
            {
                levels.Add(new Level(map, i));
                i++;
            }
            game = new Game(levels);
        }

        #endregion

        #region CheckLogin&DeleteLogin&Rename

        static bool CheckLogin(string path, string name) //Проверка на наличие похожего логина
        {

            using (FileStream fsCheck = new FileStream(path, FileMode.Open))
            {
                using (StreamReader sRCheck = new StreamReader(fsCheck, Encoding.Unicode))
                {
                    while (!sRCheck.EndOfStream)
                    {
                        if (sRCheck.ReadLine() == name)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        static void DeleteLogin(string path, int num)
        {
            List<string> logins = new List<string>();
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sR = new StreamReader(fs, Encoding.Unicode))
                {
                    int i = 1;
                    while (!sR.EndOfStream)
                    {
                        if (i == num)
                        {
                            string tmp = sR.ReadLine();
                            if (Directory.Exists(tmp))
                            {
                                Directory.Delete(tmp, true);
                            }
                        }
                        else
                        {
                            logins.Add(sR.ReadLine());
                        }
                        i++;
                    }
                }
            }
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sW = new StreamWriter(fs, Encoding.Unicode))
                {
                    foreach (var item in logins)
                    {
                        sW.WriteLine(item);
                    }
                }
            }
        }

        static void Rename(string pathUsers, string oldName, string newName)
        {
            List<string> users = new List<string>();
            using (StreamReader sR = new StreamReader(File.OpenRead(pathUsers), Encoding.Unicode))
            {
                string user = String.Empty;
                while (!sR.EndOfStream)
                {
                    user = sR.ReadLine();
                    if (user == oldName)
                    {
                        users.Add(newName);
                        try
                        {
                            Directory.Move(oldName, newName);
                        }
                        catch (Exception)
                        {
                            user = (oldName.Length > 2 ? "1" : oldName + '1');
                            Directory.Move(oldName, user);
                            Directory.Move(user, newName);
                        }
                        continue;
                    }
                    users.Add(user);
                }
            }
            using (FileStream fS = new FileStream(pathUsers, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sW = new StreamWriter(fS, Encoding.Unicode))
                {
                    foreach (var user in users)
                    {
                        sW.WriteLine(user);
                    }
                }
            }
        }

        #endregion

        public static int EnterNum(int min, int max, string Message)
        {
            int write = min;
            bool wrong = false;
            do
            {
                if (wrong) { Console.WriteLine($"Enter right num! {min + 1}-{max}"); wrong = false; }
                try
                {
                    Console.Write($"{Message}: ");
                    write = Int32.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error!");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }
                wrong = true;
            } while (!(min < write && write <= max));

            return write;
        }

        static void Main(string[] args)
        {
            Console.Title = "Sokoban by Dimon1ion";
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Loading.");
            string pathUsers = "users.txt";
            string pathPlayer = String.Empty;
            int writeInt = 0;
            int maxLevel = 1;
            Game game;
            Thread.Sleep(300);
            Console.Write(".");
            DeserializeFile(out game);
            Thread.Sleep(300);
            Console.Write(".");
            Console.Clear();
            while (true) // Авторизация/Регистрация(Меню)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Accounts:");
                Console.ForegroundColor = ConsoleColor.Cyan;
                int usersActions = 1;
                using (FileStream fs = new FileStream(pathUsers, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    using (StreamReader sR = new StreamReader(fs, Encoding.Unicode))
                    {
                        bool oneAcc = false;
                        while (!sR.EndOfStream)
                        {
                            Console.WriteLine(usersActions + ". " + sR.ReadLine());
                            usersActions++;
                            oneAcc = true;
                        }
                        if (!oneAcc)
                        {
                            Console.WriteLine("There is nothing");
                        }
                    }
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nActions:");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{usersActions++}. Create new Account");
                Console.WriteLine($"{usersActions}. Delete Account\n");
                Console.ForegroundColor = ConsoleColor.White;
                writeInt = EnterNum(0, usersActions, "Enter num of action/account");
                if (writeInt == usersActions - 1)                         //Создание нового пользователя
                {
                    Console.Clear();
                    string writeLogin;
                    do
                    {
                        Console.Write("Enter Login: ");
                        writeLogin = Console.ReadLine();

                    } while (writeLogin == String.Empty);
                    if (CheckLogin(pathUsers, writeLogin))
                    {
                        using (FileStream fs = new FileStream(pathUsers, FileMode.Append, FileAccess.Write)) // Создание Файла пользователя
                        {
                            using (StreamWriter sW = new StreamWriter(fs, Encoding.Unicode))
                            {
                                sW.WriteLine(writeLogin);
                                if (!Directory.Exists(writeLogin))
                                {
                                    Directory.CreateDirectory(writeLogin);
                                }
                                using (FileStream fs2 = new FileStream($"{writeLogin}\\levels.dat", FileMode.OpenOrCreate))
                                {
                                    using (BinaryWriter bW = new BinaryWriter(fs2, Encoding.Unicode))
                                    {
                                        bW.Write((int)1);
                                    }
                                }
                            }
                        }
                    }
                    else // Такой логин уже существует
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("This login already exists");
                        Console.ForegroundColor = ConsoleColor.White;
                        Thread.Sleep(1000);
                    }
                    Console.Clear();
                    continue;
                }
                else if (writeInt == usersActions) //Удаление аккаунтов
                {
                    writeInt = EnterNum(0, usersActions - 2, "Enter num of account for delete");
                    DeleteLogin(pathUsers, writeInt);
                    Console.Clear();
                    continue;
                }
                else // Считывание номера уровня, на котором пользователь максимально был
                {
                    using (StreamReader sR = File.OpenText(pathUsers))
                    {
                        for (int a = 1; !sR.EndOfStream; a++)
                        {
                            if (writeInt == a)
                            {
                                pathPlayer = sR.ReadLine();
                                using (FileStream fs = new FileStream($@"{pathPlayer}\levels.dat", FileMode.Open))
                                {
                                    using (BinaryReader bR = new BinaryReader(fs, Encoding.Unicode))
                                    {
                                        maxLevel = bR.Read();
                                    }
                                }
                                break;
                            }
                            sR.ReadLine();
                        }
                    }
                }
                while (true)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Player: {pathPlayer}\n");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"0.Back");
                    Console.WriteLine($"1.Rename Player\n");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    for (int i = 1; i <= maxLevel; i++)
                    {
                        Console.WriteLine($"{i + 1}.Level: {i}");
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                    writeInt = EnterNum(-1, maxLevel + 1, "Enter num for action or active level");
                    switch (writeInt)
                    {
                        case 0:
                            Console.Clear();
                            break;
                        case 1:
                            string writeLogin;
                            do
                            {
                                Console.Write("Enter Login: ");
                                writeLogin = Console.ReadLine();

                            } while (writeLogin == String.Empty);
                            if (CheckLogin(pathUsers, writeLogin))
                            {
                                Rename(pathUsers, pathPlayer, writeLogin);
                                pathPlayer = writeLogin;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("This login already exists");
                                Console.ForegroundColor = ConsoleColor.White;
                                Thread.Sleep(1000);
                            }
                            continue;
                        default:
                            writeInt--;
                            if (1 <= writeInt && writeInt <= maxLevel)
                            {
                                int tmp = game.Play(writeInt);
                                if (tmp > maxLevel)
                                {
                                    maxLevel = tmp;
                                    using (FileStream fs = new FileStream($@"{pathPlayer}\levels.dat", FileMode.Open))
                                    {
                                        using (BinaryWriter bW = new BinaryWriter(fs, Encoding.Unicode))
                                        {
                                            bW.Write((int)maxLevel);
                                        }
                                    }
                                }
                            }
                            continue;
                    }
                    break;
                }
            }
        }
    }
}
