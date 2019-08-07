using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.International.Converters.PinYinConverter;
using System.Collections;
using Newtonsoft.Json;

namespace ChineseCTop
{
    class Program
    {

        static void Main(string[] args)
        {
            Hashtable result = new Hashtable();

            string[] lines = System.IO.File.ReadAllLines(@"ChengYu5W.txt");
            var chengyus = lines.Select(x => x.Split()[0]).ToList();

            Hashtable indeces = new Hashtable();
            foreach (var cy in chengyus)
            {
                ChineseChar startch = new ChineseChar(cy[0]);
                foreach (var startpinyin in startch.Pinyins)
                {
                    if (string.IsNullOrEmpty(startpinyin)) continue;
                    var start = startpinyin.Substring(0, startpinyin.Length - 1);
                    if (!indeces.Contains(start))
                    {
                        indeces[start] = new HashSet<string>();
                    }
                    ((HashSet<string>)indeces[start]).Add(cy);
                }
            }

            Parallel.ForEach(chengyus, (chengyu) =>
            {
                Console.WriteLine("Processing {0}",chengyu);
                char endchar = chengyu[chengyu.Length - 1];
                try
                {
                    ChineseChar chc = new ChineseChar(endchar);
                    List<string> next = new List<string>();
                    foreach (var pinyin in chc.Pinyins)
                    {
                        if (string.IsNullOrEmpty(pinyin)) continue;
                        var endpy = pinyin.Substring(0, pinyin.Length - 1);

                        next.AddRange(((HashSet<string>)indeces[endpy]));
                    }
                    lock (result)
                    {
                        result[chengyu] = next;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("{0} can not parse, skip....", chengyu);
                }
            });

 
            
            var json = JsonConvert.SerializeObject(result);

            System.IO.File.WriteAllText("alljson.txt", json);
            Console.WriteLine("Done");
            Console.WriteLine("=====================================================================");
            foreach (var x in result.Keys)
            {
                if (((List<string>)(result[x])).Count == 0)
                {
                    Console.WriteLine(x);
                }
            }
            
            Console.ReadLine();

        }
    }
}
