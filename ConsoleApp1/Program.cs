using System;

class Program
{
    static void Main()
    {

        string[] names = { "Вода", "Пайки", "Аптечки", "Батареи", "Фильтры" };
        int[] quantities = { 18, 9, 3, 12, 5 };
        int[] minimums = { 10, 8, 5, 6, 4 };

        Console.WriteLine("Товаров на складе: " + names.Length);
        Console.WriteLine("Первый товар: " + names[0]);
        Console.WriteLine("Последний товар: " + names[names.Length - 1]);


        bool running = true;
        while (running)
        {
            PrintMenu();
            Console.Write("Выберите действие: ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int choice))
            {
                if (choice == 1)
                {
                    PrintInventory(names, quantities, minimums);
                }
                else if (choice == 2)
                {

                    Console.Write("Введите название товара: ");
                    string query = Console.ReadLine();
                    if (query == null) query = "";
                    int index = FindProductIndex(names, query);
                    if (index == -1)
                    {
                        Console.WriteLine("Товар не найден");
                    }
                    else
                    {
                        Console.WriteLine(names[index] + " — остаток: " + quantities[index] + " шт.");
                    }
                }
                else if (choice == 3)
                {
                    Console.Write("Название товара: ");
                    string query = Console.ReadLine();
                    if (query == null) query = "";
                    int index = FindProductIndex(names, query);
                    if (index == -1)
                    {
                        Console.WriteLine("Товар не найден");
                    }
                    else
                    {
                        Console.Write("Сколько принять: ");
                        string amountStr = Console.ReadLine();
                        if (int.TryParse(amountStr, out int amount) && amount > 0)
                        {
                            if (ChangeStock(quantities, index, amount))
                            {
                                Console.WriteLine("Новый остаток: " + quantities[index] + " шт.");
                            }
                            else
                            {
                                Console.WriteLine("Операция отклонена");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Нужно целое положительное число");
                        }
                    }
                }
                else if (choice == 4)
                {
                    Console.Write("Название товара: ");
                    string query = Console.ReadLine();
                    if (query == null) query = "";
                    int index = FindProductIndex(names, query);
                    if (index == -1)
                    {
                        Console.WriteLine("Товар не найден");
                    }
                    else
                    {
                        Console.Write("Сколько списать: ");
                        string amountStr = Console.ReadLine();
                        if (int.TryParse(amountStr, out int amount) && amount > 0)
                        {
                            if (ChangeStock(quantities, index, -amount))
                            {
                                Console.WriteLine("Новый остаток: " + quantities[index] + " шт.");
                            }
                            else
                            {
                                Console.WriteLine("Нельзя списать больше, чем есть на складе");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Нужно целое положительное число");
                        }
                    }
                }
                else if (choice == 5)
                {
                    
                    int total = SumStock(quantities);
                    int maxIndex = FindMaxIndex(quantities);
                    int deficits = CountBelowMinimum(quantities, minimums);
                    Console.WriteLine("Общий остаток: " + total);
                    Console.WriteLine("Больше всего: " + names[maxIndex] + " (" + quantities[maxIndex] + " шт.)");
                    Console.WriteLine("Дефицитных позиций: " + deficits);
                }
                else if (choice == 6)
                {
                    
                    PrintDeficits(names, quantities, minimums);
                }
                else if (choice == 0)
                {
                    Console.WriteLine("Выход...");
                    running = false;
                }
                else
                {
                    Console.WriteLine("Неизвестный пункт меню");
                }
            }
            else
            {
                Console.WriteLine("Нужно ввести число!");
            }
        }
    }

    static void PrintMenu()
    {
        Console.WriteLine();
        Console.WriteLine("========= NEON DEPOT =========");
        Console.WriteLine("1. Показать весь склад");
        Console.WriteLine("2. Найти товар");
        Console.WriteLine("3. Принять поставку");
        Console.WriteLine("4. Списать товар");
        Console.WriteLine("5. Статистика склада");
        Console.WriteLine("6. Товары ниже минимума");
        Console.WriteLine("0. Выход");
    }


    static void PrintInventory(string[] names, int[] quantities, int[] minimums)
    {
        for (int i = 0; i < names.Length; i++)
        {
            string status;
            if (quantities[i] < minimums[i])
            {
                status = "ПОПОЛНИТЬ";
            }
            else
            {
                status = "НОРМА";
            }
            Console.WriteLine(i + " | " + names[i] + " | " + quantities[i] + " шт. | мин. " + minimums[i] + " | " + status);
        }
    }


    static int FindProductIndex(string[] names, string query)
    {
        string q = query.Trim();
        for (int i = 0; i < names.Length; i++)
        {
            if (string.Equals(names[i], q, StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }
        return -1;
    }


    static bool ChangeStock(int[] quantities, int index, int delta)
    {
        if (index < 0 || index >= quantities.Length)
        {
            return false;
        }
        int newValue = quantities[index] + delta;
        if (newValue < 0)
        {
            return false;
        }
        quantities[index] = newValue;
        return true;
    }


    static int SumStock(int[] quantities)
    {
        int sum = 0;
        for (int i = 0; i < quantities.Length; i++)
        {
            sum = sum + quantities[i];
        }
        return sum;
    }


    static int FindMaxIndex(int[] quantities)
    {
        int maxIndex = 0;
        for (int i = 1; i < quantities.Length; i++)
        {
            if (quantities[i] > quantities[maxIndex])
            {
                maxIndex = i;
            }
        }
        return maxIndex;
    }

    static int CountBelowMinimum(int[] quantities, int[] minimums)
    {
        int count = 0;
        for (int i = 0; i < quantities.Length; i++)
        {
            if (quantities[i] < minimums[i])
            {
                count++;
            }
        }
        return count;
    }

    static void PrintDeficits(string[] names, int[] quantities, int[] minimums)
    {
        bool hasDeficit = false;
        for (int i = 0; i < names.Length; i++)
        {
            if (quantities[i] < minimums[i])
            {
                int missing = minimums[i] - quantities[i];
                Console.WriteLine(names[i] + " — не хватает " + missing + " шт.");
                hasDeficit = true;
            }
        }
        if (!hasDeficit)
        {
            Console.WriteLine("Все запасы в норме");
        }
    }
}