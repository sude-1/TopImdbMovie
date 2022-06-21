using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace TopImdbMovie
{
    public class Program
    {
        static void Main(string[] args)
        {
            ImdbContext imdbContext = new ImdbContext();

            IWebDriver webDriver = new ChromeDriver();

            webDriver.Navigate().GoToUrl("http://www.imdb.com/chart/top");

            ReadOnlyCollection<IWebElement> elements = webDriver.FindElements(By.XPath("//*[@id=\"main\"]/div/span/div/div/div/table/tbody/tr/td[2]/a"));

            ReadOnlyCollection<IWebElement> images = webDriver.FindElements(By.XPath("//*[@id=\"main\"]/div/span/div/div/div/table/tbody/tr/td/a/img"));

            var cmd = new SqlCommand();
            cmd.Connection = imdbContext.Connection();
            for (int i = 0; i < elements.Count; i++)
            {
                var item = elements[i];
                var image = images[i];
                Console.WriteLine(item.Text);

                cmd.CommandText += "insert into Movie(Name,Image) values (@p" + i + ",@pi"+i+");";
                cmd.Parameters.AddWithValue("@p" + i, item.Text);
                cmd.Parameters.AddWithValue("@pi" + i, image.GetAttribute("src"));
            }
           
            webDriver.Close();
            cmd.ExecuteNonQuery();
            imdbContext.Connection().Close();
            Console.WriteLine("Film Adı Giriniz");
            string name = Console.ReadLine();
            int findMovie = 0;
            Console.WriteLine("-----Aranılan Film-----");
            SqlCommand command = new SqlCommand("select *from Movie where Name like '" + name + "'", imdbContext.Connection());
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                findMovie++;
                Console.WriteLine(reader[0] + "\t\t" + reader[1]);
            }
            if (findMovie > 0)
            {
                Console.WriteLine(findMovie + "Film bulundu");
            }
            else
            {
                Console.WriteLine("Film bulunamadı");
            }
            imdbContext.Connection().Close();
        }
    }
}
