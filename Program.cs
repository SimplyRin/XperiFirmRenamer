using System;
using System.IO;
using System.Text.RegularExpressions;

/**
 * Created by SimplyRin on 2020/11/14.
 * 
 * Copyright (c) 2020 SimplyRin
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
namespace XperiFirmRenamer {

    public class Program {

        public static void Main(String[] args) {
            Console.WriteLine("XperiFirm でダウンロードしたファームウェアのフォルダ名を新しいフォーマットへ変換します。");

            String path = null;
            while (true) {
                Console.Write("パスを入力してください: ");
                path = Console.ReadLine();
                if (path == null || !path.Equals("")) {
                    break;
                }
            }

            String[] files = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
            foreach (String temp0 in files) {
                String name = temp0.Replace(path, "");
                if (name.StartsWith(@"\")) {
                    name = name.Substring(1);
                }
                if (name.Contains(@"\") || name.Split("_").Length != 5 || !name.Contains("-")) {
                    continue;
                }

                String newName = "";
                Boolean nextIsHyphen = false;
                foreach (String temp1 in name.Split("_")) {
                    if (Regex.IsMatch(temp1, "[0-9]{4}[-][0-9]{4}")) {
                        continue;
                    }
                    if (Regex.IsMatch(temp1, "[0-9]{2}[.][0-9]{1}[.][a-zA-Z][.][0-9]{1,2}[.][0-9]{1,3}")) {
                        nextIsHyphen = true;
                    }

                    newName += temp1 + (nextIsHyphen ? "-" : "_");
                    nextIsHyphen = false;
                }
                newName = newName.Substring(0, newName.Length - 1);

                String newPath = path + @"\" + newName;
                if (Directory.Exists(newPath)) {
                    continue;
                }

                String oldPath = path + @"\" + name;

                Console.WriteLine(name + " -> " + newName);
                Directory.Move(oldPath, newPath);
            }
        }
    }
}
