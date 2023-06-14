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
            var resourceManager = new ResourceManager("CodeBlogFitness.CMD.Languages.Messages.ru_ru", typeof(Program).Assembly);

            //Console.WriteLine("Вас приветствует приложение CodeBlogFitness");
            //Console.WriteLine(Languages.Messages.Hello);
            Console.WriteLine(resourceManager.GetString("Hello", culture));

            //Console.WriteLine("Введите имя пользователя");
            //Console.WriteLine(Languages.Messages.EnterName);
            Console.WriteLine(resourceManager.GetString("EnterName", culture));

            var name = Console.ReadLine();

            var userController = new UserController(name);
            var eatingController = new EatingController(userController.CurrentUser);
            var exerciseController = new ExerciseController(userController.CurrentUser);
            if (userController.IsNewUser)
            {
                Console.WriteLine("Введите пол");
                var gender = Console.ReadLine();
                var birthDate = ParseDataTime("дата рождения");
                var weight = ParseDouble("вес");
                var height = ParseDouble("рост");

                userController.SetNewUserData(gender, birthDate, weight, height);
            }

            Console.WriteLine(userController.CurrentUser);
            while (true)
            {
                Console.WriteLine("Что вы хотите сделать?");
                Console.WriteLine("Е - ввести прием пищи");
                Console.WriteLine("A - ввести упражнение");
                Console.WriteLine("Q - выход");
                var key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.E:
                        var foods = EnterEating();
                        eatingController.Add(foods.Food, foods.Weight);

                        foreach (var item in eatingController.Eating.Foods)
                        {
                            Console.WriteLine($"\t{item.Key} - {item.Value}");
                        }
                        break;
                    case ConsoleKey.A:
                        var exe = EmterExercise();
                        //var exercise = new Exercise(exe.Begin, exe.End, exe.Activity, userController.CurrentUser);
                        exerciseController.Add(exe.Activity,exe.Begin, exe.End);

                        foreach(var item in exerciseController.Exercises)
                        {
                            Console.WriteLine($"\t{item.Activity} с {item.Start.ToShortDateString} до {item.Finish.ToShortTimeString}");
                        }
                        break;
                    case ConsoleKey.Q:
                        Environment.Exit(0);
                        break;
                }
                //if (key.Key == ConsoleKey.E)
                //{
                //    var foods = EnterEating();
                //    eatingController.Add(foods.Food, foods.Weight);

                //    foreach (var item in eatingController.Eating.Foods)
                //    {
                //        Console.WriteLine($"\t{item.Key} - {item.Value}");
                //    }

                //}
                Console.ReadLine();
            }           
        }

        private static (DateTime Begin, DateTime End, Activity Activity) EmterExercise()
        {
            Console.WriteLine("Введите название упражнения:");
            var name = Console.ReadLine();

            var energy = ParseDouble("расход энергии в минуту");

            var begin = ParseDataTime("начало упражнения");
            var end = ParseDataTime("окончание упражнения");

            var activity = new Activity(name, energy);
            return (begin, end, activity);
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

        private static DateTime ParseDataTime(string value)
        {
            DateTime birthDate;
            while (true)
            {
                Console.WriteLine($"Введите {value} ");//(dd.mm.yyyy):
                if (DateTime.TryParse(Console.ReadLine(), out birthDate))
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"Неверный формат {value}");
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
