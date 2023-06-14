using System;
using CodeBlogFitness.BL.Controller;
using CodeBlogFitness.BL.Model;
using System.Reflection;
using CodeBlogFitness.BL;
using System.Globalization;
using System.Resources;

namespace CodeBlogFitness.CMD // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        // See https://aka.ms/new-console-template for more information

        static void Main()
        {
            //var culture = CultureInfo.CreateSpecificCulture("ru_ru");
            //var culture = CultureInfo.CreateSpecificCulture("en_us");
            var culture = CultureInfo.CurrentCulture;
            var resourceManager = new ResourceManager("CodeBlogFitness.CMD.Languages.Messages.en_us", typeof(Program).Assembly);

            //Console.WriteLine("Вас приветствует приложение CodeBlogFitness");
            //Console.WriteLine(Languages.Messages.Hello);
            Console.WriteLine(resourceManager.GetString("Hello", culture));

            //Console.WriteLine("Введите имя пользователя");
            //Console.WriteLine(Languages.Messages.EnterName);
            Console.WriteLine(resourceManager.GetString("EnterName", culture));

            var name = Console.ReadLine();

            var userController = new UserController(name);
            var eatingController = new EatingController(userController.CurrentUser);
            if (userController.IsNewUser)
            {
                Console.WriteLine("Введите пол");
                var gender = Console.ReadLine();
                var birthDate = ParseDataTime();
                var weight = ParseDouble("вес");
                var height = ParseDouble("рост");

                userController.SetNewUserData(gender, birthDate, weight, height);
            }

            Console.WriteLine(userController.CurrentUser);

            Console.WriteLine("Что вы хотите сделать?");
            Console.WriteLine("Е - ввести прием пищи");
            var key = Console.ReadKey();

            if (key.Key == ConsoleKey.E)
            {
                var foods = EnterEating();
                eatingController.Add(foods.Food, foods.Weight);

                foreach (var item in eatingController.Eating.Foods)
                {
                    Console.WriteLine($"\t{item.Key} - {item.Value}");
                }
                
            }

            Console.ReadLine();
        }

        private static (Food Food, double Weight) EnterEating()
        {
            Console.WriteLine("Введите название продукта:");
            var food = Console.ReadLine();

            var calories = ParseDouble("калорийность");
            var prots = ParseDouble("белки");
            var fats = ParseDouble("жиры");
            var carbs = ParseDouble("углеводы");

            //Console.WriteLine("Введите вес порции:");
            var weight = ParseDouble("вес порции");
            var product = new Food(food, calories, prots, fats, carbs);

            return (Food: product,Weight: weight);
        }

        private static DateTime ParseDataTime()
        {
            DateTime birthDate;
            while (true)
            {
                Console.WriteLine("Введите дату рождения (dd.mm.yyyy):");
                if (DateTime.TryParse(Console.ReadLine(), out birthDate))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Неверный формат даты рождения");
                }
            }

            return birthDate;
        }

        private static double ParseDouble(string name)
        {
            while (true)
            {
                Console.WriteLine($"Введите {name}:");
                if (double.TryParse(Console.ReadLine(), out double value))
                {
                    return value;
                }
                else
                {
                    Console.WriteLine($"Неверный формат поля");
                }
            }
        }


    }
}
