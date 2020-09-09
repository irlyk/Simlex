using System;
using System.Collections.Generic;

namespace TestApp
{
    class Program
    {
 

        static void Main(string[] args)
        {
            float[,] table = ReadTable();
            PrintFunc(table);
            float[] result = Calculate(table, true);
            Console.WriteLine("Ответ: " + GetResult(table,result));
            Analis(table, result);
            Console.ReadKey();
        }

        static float[] Calculate(float[,] orig_table, bool output)
        {
            float[,] table = ConvertTable(orig_table);
            if (output)
                PrintTable(table);
            List<int> basis = GetFirstBasis(table, output);
            int mRow, mCol, iter = 0, m = table.GetLength(0), n = table.GetLength(1);
            while (!Check_Resh(table, m, n))
            {
                if (output)
                    Console.WriteLine("Итерация " + ((iter++) + 1));
                mCol = FindMCol(table, n, m);
                mRow = FindMRow(table, mCol, m);
                basis[mRow] = mCol;
                if (output)
                {
                    Console.WriteLine("min row: " + mRow + "; min col: " + mCol + ";");
                    Console.WriteLine();
                    foreach (int i in basis)
                    {
                        Console.WriteLine("Элемент " + i + " под индексом " + basis.IndexOf(i));
                    }
                    Console.WriteLine();
                }
                float[,] new_table = new float[m, n];
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                        new_table[i,j] = table[i,j] - ((table[mRow,j] * table[i,mCol]) / table[mRow,mCol]);
                }
                for (int i = 0; i < m; i++)
                {
                    if (i == mRow)
                        continue;
                    new_table[i,mCol] = -(table[i,mCol] / table[mRow,mCol]);
                }
                for (int i = 0; i < n; i++)
                {
                    if (i == mCol)
                        continue;
                    new_table[mRow,i] = table[mRow,i] / table[mRow,mCol];
                }
                new_table[mRow,mCol] = 1 / table[mRow,mCol];
                table = new_table;
                if (output)
                    PrintTable(table);
            }
            float[] reshenie = new float[orig_table.GetLength(1) - 1];
            for (int i = 0; i < reshenie.GetLength(0); i++)
            {
                int k = basis.IndexOf(i + 1);
                if (k != -1)
                    reshenie[i] = table[k, 0];
                else
                    reshenie[i] = 0;
            }
            if (output)
            {
                Console.WriteLine("Решение:");
                for (int i = 0; i < reshenie.GetLength(0); i++)
                    Console.Write($"X{i + 1} = {reshenie[i]}; \t");
                Console.WriteLine();
            }

            return reshenie;
        }

        static float[] Calculate_(float[,] orig_table, bool output)
        {
            float[,] table = ConvertTable(orig_table);
            if(output)
                PrintTable(table);
            List<int> basis = GetFirstBasis(table, output);
            int mRow, mCol, iter = 0 , m = table.GetLength(0), n = table.GetLength(1);
            while (!Check_Resh(table, m, n))
            {
                if(output)
                    Console.WriteLine("Итерация " + ((iter++) + 1));
                mCol = FindMCol(table, n, m);
                mRow = FindMRow(table, mCol, m);
                basis[mRow] = mCol;
                if (output)
                {
                    Console.WriteLine("min row: " + mRow + "; min col: " + mCol + ";");
                    Console.WriteLine();
                    foreach (int i in basis)
                    {
                        Console.WriteLine("Элемент " + i + " под индексом " + basis.IndexOf(i));
                    }
                    Console.WriteLine();
                }
                float[,] new_table = new float[m, n];
                for (int j = 0; j < n; j++)
                    new_table[mRow, j] = table[mRow, j] / table[mRow, mCol];
                for (int i = 0; i < m; i++)
                {
                    if (i == mRow)
                        continue;

                    for (int j = 0; j < n; j++)
                        new_table[i, j] = table[i, j] - table[i, mCol] * new_table[mRow, j];
                }
                table = new_table;
                if(output)
                    PrintTable(table);
            }
            float[] reshenie = new float[orig_table.GetLength(1) - 1];
            for(int i = 0; i < reshenie.GetLength(0); i++)
            {
                int k = basis.IndexOf(i + 1);
                if (k != -1)
                    reshenie[i] = table[k, 0];
                else
                    reshenie[i] = 0;
            }
            if (output)
            {
                Console.WriteLine("Решение:");
                for (int i = 0; i < reshenie.GetLength(0); i++)
                    Console.Write($"X{i + 1} = {reshenie[i]}; \t");
                Console.WriteLine();
            }

            return reshenie;
        }

        static void Analis(float[,] orig_table, float[] res)
        {
            Console.WriteLine();
            Console.WriteLine("Старт Анализа:");
            Console.WriteLine("Введите шаг");
            string str = Console.ReadLine();
            float s = (float)(Convert.ToDouble(str));
            for (int i = 0; i < orig_table.GetLength(0) - 1; i++)
            {
                float[,] table = CopyTable(orig_table);
                bool check = true;
                table[i, 0] += s;
                float[] new_res = Calculate(table, false);
                table[i, 0] -= s;
                if (!CheckRes(new_res, res))
                {
                    Console.WriteLine($"Ресурс {i + 1} дефицитный");
                    check = true;

                }
                else
                {
                    Console.WriteLine($"Ресурс {i + 1} не дефицитный");
                    check = false;
                }
                int st = 0, maxiter = 100000000;
                float delta = 0, old_res = GetResult(orig_table, res);
                while (st < maxiter)
                {
                    st +=1;
                    if (check)
                    {
                        table[i, 0] += s;
                        float[] res_in = Calculate(table, false);
                        float res1 =GetResult(table, res_in);
                        if (st % 100000 == 0)
                        {
                            Console.Write('*');
                            //    Console.WriteLine("i = " + st + " Значений функции со сдвигом " + delta + " F2 = " + res1 +" \t Предыдущие значение F1 = " + old_res);
                        }
                        if (res1<=old_res)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Значений функции с дельтой F = " + old_res);
                            Console.WriteLine($"Дельта ресурса {i + 1} = {delta}");
                            break;
                        }
                        old_res = res1; 
                    }
                    else
                    {
                        table[i, 0] -= s;
                        float[] res_in = Calculate(table, false);
                        float res1 = GetResult(orig_table, res);
                        float res2 = GetResult(table, res_in);
                        if (st % 100000 == 0)
                        {
                            Console.Write('*');
                            //    Console.WriteLine("i = " + st + " Значение функции со сдвигом "+ delta + " F1 = " + res2 + " \t Значение функции изначальной таблицы F2 = " + res1);
                        }
                        if (res2 != res1)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Значений функции с дельтой F1 = " + res2 + "; Новое значение функции F2 = " + res1);
                            Console.WriteLine($"Дельта ресурса {i + 1} = {-delta}");
                            break;
                        }
                    }
                    delta += s;
                }
                if (st >= maxiter)
                    Console.WriteLine("Ресурс можно изменять до бесконечности");
                Console.WriteLine();
            }
            Console.WriteLine("Конец анализа");
        }

        static bool CheckRes(float[] new_res, float[] old_res)
        {
            bool check = true;
            for(int i = 0; i < old_res.GetLength(0); i++)
                if (old_res[i] != new_res[i])
                {
                    check = false;
                    break;
                }
            return check;
        }

        static float[,] CopyTable(float[,] table)
        {
            float[,] cop_table = new float[table.GetLength(0), table.GetLength(1)];
            for (int i = 0; i < table.GetLength(0); i++)
                for (int j = 0; j < table.GetLength(1); j++)
                    cop_table[i, j] = table[i, j];
            return cop_table;
        }

        static float GetResult(float[,] table, float[] result)
        {
            float res = 0;
            int n = table.GetLength(0) - 1;
            for (int i = 0; i < result.GetLength(0); i++)
            {
                res += table[n, i + 1] * result[i];
            }
            res += table[n, 0];
            return res;
        }
        static int FindMRow(float[,] table, int mCol, int m)
        {
            int mainRow = 0;
            for (int i = 0; i < m - 1; i++)
                if (table[i, mCol] > 0)
                {
                    mainRow = i;
                    break;
                }
            for (int i = mainRow + 1; i < m - 1; i++)
                if ((table[i, mCol] > 0) && ((table[i, 0] / table[i, mCol]) < (table[mainRow, 0] / table[mainRow, mCol])))
                    mainRow = i;
            return mainRow;
        }

        static int FindMCol(float[,] table, int n, int m)
        {
            int mCol = 1;
            for (int j = 2; j < n; j++)
                if (table[m - 1, j] < table[m - 1, mCol])
                    mCol = j;
            return mCol;
        }

        static float[,] ConvertTable(float[,] old_table)
        {
            int m = old_table.GetLength(0), n = old_table.GetLength(1);
            float[,] new_table = new float[m, n + m - 1];
            for (int i = 0; i < new_table.GetLength(0); i++)
            {
                for (int j = 0; j < new_table.GetLength(1); j++)
                {
                    if (j < n)
                        new_table[i, j] = old_table[i, j];
                    else
                        new_table[i, j] = 0;
                }
                if((n + i) < new_table.GetLength(1))
                {
                    new_table[i, n + i] = 1;
                }
                if( i == m - 1)
                {
                    for(int j = 0; j < new_table.GetLength(1); j++)
                    {
                        new_table[i, j] = 0 - new_table[i, j];
                    }
                }
            }
            return new_table;
        }

        static bool Check_Resh(float[,] table, int m, int n)
        {
            bool flag = true;
            for (int j = 1; j < n; j++)
            {
                if (table[m - 1, j] < 0)
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }

        static List<int> GetFirstBasis(float[,] table, bool output)
        {
            List<int> basis = new List<int>();
            int m = table.GetLength(0) - 1;
            if(output)
                Console.WriteLine("Базисные переменные:");
            for (int i = 1; i < table.GetLength(1); i++)
            {
                if (table[m, i] < 0) continue;
                basis.Add(i);
                if(output)
                    Console.Write(i + "\t");
            }
            if(output)
                Console.WriteLine();
            return basis;
        }

        static void PrintFunc(float[,] table)
        {
            Console.WriteLine();
            Console.Write("F = ");
            int k = table.GetLength(0) - 1; 
            for (int i = 1; i < table.GetLength(1); i++)
            {
                if (table[k, i] >= 0 && i > 1)
                    Console.Write($"+{table[k, i]}X{i} ");
                else
                    Console.Write($"{table[k, i]}X{i} ");
            }
            Console.WriteLine();
        }
        static void PrintTable(float[,] table)
        {
            Console.WriteLine();
            for (int i = 0; i < table.GetLength(1); i++)
            {
                if (i == 0)
                    Console.Write("b\t");
                else
                    Console.Write($"X{i}\t");
            }
            Console.WriteLine();
            for (int i = 0; i < table.GetLength(0); i++)
            {
                for(int j = 0; j < table.GetLength(1); j++)
                {
                    Console.Write(table[i,j] + "\t");
                }
                if (i == table.GetLength(0) - 1)
                    Console.Write(" - F(X)");
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static float[,] ReadTable()
        {
            Console.WriteLine("Введите количество переменных n и количество ограничений m:");
            string[] str = Console.ReadLine().Split(new char[] { ' ', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            int n = Convert.ToInt32(str[0]), m = Convert.ToInt32(str[1]);
            Console.WriteLine("Введите ограничения в виде: b1 x11 x22 ... x1n");
            Console.WriteLine("                            ....... ");
            Console.WriteLine("                            bm xm1 xm2 ... xmn");
            Console.WriteLine("                            0  c1  c2  ... cn ");
            float[,] table = new float[m + 1, n + 1];
            for (int i = 0; i < table.GetLength(0); i++)
            {
                str = Console.ReadLine().Split(new char[] { ' ', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < str.Length; j++)
                {
                    if (j < table.GetLength(1))
                        table[i, j] = (float)Convert.ToDouble(str[j]);
                    else
                    {
                        Console.WriteLine("ERROR: Введёная строка слишком большая");
                        Environment.Exit(1);
                    }
                }
            }
            return table;
        } 
    }
}
