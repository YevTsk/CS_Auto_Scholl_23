using System;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        try
        {
            // Вызов функции для обработки файла yupdate.txt и вывод результата
            double errorRatio = ProcessYUpdateFile();
            Console.WriteLine($"Ratio of total entries to error entries: {errorRatio}");
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (DivideByZeroException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (CriticalErrorException ex)
        {
            Console.WriteLine($"Critical Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }

    static double ProcessYUpdateFile()
    {
        // Пути к файлам
        string filePath = "/Users/jeks/Projects/Auto/Auto/bin/Debug/net7.0/yupdate.txt";
        string outputFilePath = "/Users/jeks/Projects/Auto/Auto/bin/Debug/net7.0/errors.txt";
        string criticalErrorKeyword = "CRITICAL ERROR";

        // Переменные для подсчета общего числа записей и записей с ошибками
        int totalEntries = 0;
        int errorEntries = 0;

        try
        {
            // Используем StreamReader для чтения файла yupdate.txt
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    totalEntries++; // Увеличиваем общее количество записей

                    if (line.Contains("ERROR"))
                    {
                        errorEntries++; // Увеличиваем количество записей с ошибками
                        WriteToErrorFile(outputFilePath, line); // Записываем ошибку в файл errors.txt
                    }

                    if (line.Contains(criticalErrorKeyword))
                    {
                        // Если обнаружена запись с CRITICAL ERROR, бросаем исключение
                        throw new CriticalErrorException(line);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing yupdate.txt: {ex.Message}");
        }
        finally
        {
            Console.WriteLine($"Total entries in yupdate.txt: {totalEntries}");
        }

        // Проверяем наличие ошибок для избежания деления на ноль
        if (errorEntries == 0)
        {
            throw new DivideByZeroException("Division by zero error.");
        }

        // Возвращаем отношение общего количества записей к количеству записей с ошибками
        return (double)totalEntries / errorEntries;
    }

    // Функция для записи строки в файл errors.txt
    static void WriteToErrorFile(string filePath, string content)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(content);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to errors.txt: {ex.Message}");
        }
    }
}

// Собственный тип исключения для обработки CRITICAL ERROR
class CriticalErrorException : Exception
{
    public CriticalErrorException(string message) : base(message) { }
}