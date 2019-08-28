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
        static bool ChengyueStartMatchEnd(string x)
        {
            ChineseChar pre = new ChineseChar(x[0]);
            ChineseChar suf = new ChineseChar(x[x.Length - 1]);
            foreach (var py in pre.Pinyins)
            {
                if (string.IsNullOrEmpty(py)) continue;
                string spy = py.Substring(0, py.Length - 1);

                var sy = suf.Pinyins.Where(r => !string.IsNullOrEmpty(r)).Select(r => r.Substring(0, r.Length - 1));
                if (sy.Contains(spy))
                {
                    return true;
                }
            }
            return false;
        }

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
            

            while (true)
            {
                Console.Write("«Î ‰»Î≥…”Ô £∫ ");
                char ch = Console.ReadLine().Last();
                var chch = new ChineseChar(ch);
                foreach (var py in chch.Pinyins)
                {
                    if (string.IsNullOrEmpty(py)) continue;
                    var noyindiao = py.Substring(0, py.Length - 1);
                    Console.Write(" {0} : ", noyindiao);
                    var lst = (HashSet<string>)indeces[noyindiao];
                    if (lst != null)
                    {
                        foreach (var outp in lst)
                        {
                            Console.Write("{0} ", outp);
                        }
                        Console.WriteLine();
                    }                    
                }
            }

        }
    }
}
