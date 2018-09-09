using System;

namespace Sucrose.Net.Utils
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var buckeList = new SortedBucket<int, string>(2, SortMode.Asc);

            buckeList.Add(5, "five");
            buckeList.Add(4, "4");
            buckeList.Add(789, "5");
            buckeList.Add(3, "8");

            var page = 1;
            while (buckeList != null)
            {
                Console.WriteLine("Page: " + page++);
                foreach (var item in buckeList)
                {
                    Console.WriteLine(item.Key + " " + item.Value);
                }

                buckeList = buckeList.Next;
            }
        }
    }
}
