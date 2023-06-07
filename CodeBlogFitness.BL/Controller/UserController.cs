using CodeBlogFitness.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CodeBlogFitness.BL.Controller
{
    /// <summary>
    /// Контроллер пользователя.
    /// </summary>
    public class UserController
    {
        /// <summary>
        /// Пользователь приложения.
        /// </summary>
        //public List<User> Users { get; }
        public User User { get; }

        /// <summary>
        /// Создание нового контроллера пользователя.
        /// </summary>
        /// <param name="user"></param>
        public UserController(string userName, string genderName, DateTime birthDay, double weight, double height)
        {
            var gender = new Gender(genderName);
            User = new User(userName, gender, birthDay, weight, height);

            //User = user ?? throw new ArgumentNullException("Имя пользователя не может быть пустым", nameof(user));
        }

        /// <summary>
        /// Получить данные пользователя.
        /// </summary>
        public UserController()
        {
            var formatter = new BinaryFormatter();

            using (var fs = new FileStream("users.dat", FileMode.OpenOrCreate))
            {
                if (formatter.Deserialize(fs) is User user)
                {
                    User = user;
                }
                //return formatter.Deserialize(fs) as User;
                // TODO: что делать если пользователя не прочитали?
            }
        }

        //public User User { get; }

        //public bool IsNewUser { get; } = false;

        ///// <summary>
        ///// Создание нового контроллера пользователя.
        ///// </summary>
        ///// <param name="user"></param>
        //public UserController(string userName)
        //{
        //    if (string.IsNullOrWhiteSpace(userName))
        //    {
        //        throw new ArgumentNullException("Имя пользователя не может быть пустым", nameof(userName));
        //    }

        //    Users = GetUsersData();

        //    CurrentUser = Users.SingleOrDefault(u => u.Name == userName);

        //    if (CurrentUser == null)
        //    {
        //        CurrentUser = new User(userName);
        //        Users.Add(CurrentUser);
        //        IsNewUser = true;
        //    }
        //}

        ///// <summary>
        ///// Получить сохраненный список пользователей.
        ///// </summary>
        ///// <returns></returns>
        //private List<User> GetUsersData()
        //{
        //    return Load<User>() ?? new List<User>();
        //}


        //public void SetNewUserData(string genderName, DateTime birthDate, double weight = 1, double height = 1)
        //{
        //    // Проверка

        //    CurrentUser.Gender = new Gender(genderName);
        //    CurrentUser.BirthDate = birthDate;
        //    CurrentUser.Weight = weight;
        //    CurrentUser.Height = height;
        //    Save();
        //}

        /// <summary>
        /// Сохранить данные пользователя.
        /// </summary>
        public void Save()
        {
            var formatter = new BinaryFormatter();

            using(var fs = new FileStream("users.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, User);
            }
        }
        
    }
}
