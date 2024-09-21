using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ShopApp.Classes
{
    /// <summary>
    /// Класс шаблонов сообщений
    /// </summary>
    public class MessagesClass
    {
        /// <summary>
        /// Сообщение с информацией
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public MessageBoxResult ShowInfo(string info)
        {
            return MessageBox.Show(info, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        /// <summary>
        /// Сообщение с ошибкой
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public MessageBoxResult ShowError(string error)
        {
            return MessageBox.Show(error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        /// <summary>
        /// Сообщение с предупреждением
        /// </summary>
        /// <param name="warning"></param>
        /// <returns></returns>
        public MessageBoxResult ShowWarning(string warning)
        {
            return MessageBox.Show(warning, "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        /// <summary>
        /// Сообщение с вопросом
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public MessageBoxResult ShowQuestion(string question)
        {
            return MessageBox.Show(question, "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question);
        }
        public MessageBoxResult ShowNoConnection()
        {
            return MessageBox.Show("Потеряно соединение с базой", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
