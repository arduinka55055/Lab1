// See https://aka.ms/new-console-template for more information

using System.Reflection.Metadata.Ecma335;
using Lab1;

var sw = new System.Diagnostics.Stopwatch();
//зробимо 4 шини, вхід, вихід трикутника, вихід XORів, результат ORів
Bus bus1 = new Bus(6, "input");
Bus bus2 = new Bus(6, "output");
Bus bus3 = new Bus(6, "XORs");
Bus bus4 = new Bus(3, "Final");

Console.WriteLine(@"Якщо ви хочете трикутник напишіть 1, інакше Return");
var ask = Console.ReadLine();
if(ask == "1")
    new Triangle(bus1,bus2);
else 
    new Pyramid(bus1,bus2);


/*
    XORи поєднуються зі зсувом відносно шини. а перший це заземлення
    отже є канали i, та i-1 
*/
XOR[] xors = new XOR[6];
for(int i=0;i<6;i++){
    xors[i] = new XOR();
    int ain_index = i-1;
    int bin_index = i;
    bus2.Connect(xors[i], bin_index, 1); // connect B to XOR
    if(ain_index>=0)
        bus2.Connect(xors[i], ain_index, 0); // connect A to XOR, -1 is ground

    Console.WriteLine($"connecting XOR {i} to bus2[{ain_index}] and bus2[{bin_index}]");
    //connect xor to bus3
    xors[i].Connect(bus3, 0, i);
}
/*
    ORи не мають детермінованого порядку, 
    тому ми їх створюємо і підключаємо
*/
OR3[] oR3s = new OR3[3];
for(int i=0;i<3;i++){
    oR3s[i] = new OR3();
}
bus3.Connect(oR3s[0], 0, 0);
bus3.Connect(oR3s[0], 1, 1);
bus3.Connect(oR3s[0], 2, 2);
//
bus3.Connect(oR3s[1], 0, 0);
bus3.Connect(oR3s[1], 4, 1);
bus3.Connect(oR3s[1], 3, 2);
//
bus3.Connect(oR3s[2], 3, 0);
bus3.Connect(oR3s[2], 1, 1);
bus3.Connect(oR3s[2], 5, 2);
//CONNECT FINAL OUTPUT
oR3s[2].Connect(bus4, 0, 0);
oR3s[1].Connect(bus4, 0, 1);
oR3s[0].Connect(bus4, 0, 2);




Console.WriteLine(@"Виберіть варіант вхідних даних:
1. ручний
2. випадковий
3. всі можливі
");
int choice = int.Parse(Console.ReadLine());

if(choice == 1){
    Console.WriteLine("Введіть 6 бітів вхідних даних (наприклад 111111)");
    string input = Console.ReadLine();
    for(int i=0;i<6;i++){
        bus1[i] = input[i] == '1';
    }
    sw.Start();
}


if(choice == 2){
    Random rnd = new Random();
    for(int i=0;i<6;i++){
        bus1[i] = rnd.Next(0,2) == 1;
    }
    sw.Start();
}

if(choice != 3){
    Console.WriteLine($"Вхідні дані: {string.Join("", bus1.Outputs.Select(x => x ? "1" : "0"))}");
    Console.WriteLine($"Дані шини трикутника: {string.Join("", bus2.Outputs.Select(x => x ? "1" : "0"))}");
    Console.WriteLine($"Дані шини XORів: {string.Join("", bus3.Outputs.Select(x => x ? "1" : "0"))}");
    Console.WriteLine($"Дані шини ORів: {string.Join("", bus4.Outputs.Select(x => x ? "1" : "0"))}");

    sw.Stop();
    Console.WriteLine($"я впорався за: {sw.ElapsedMilliseconds}ms");
    return;
}



for(int i=0;i<Math.Pow(2,6);i++){
    Console.Write($"input: {i}, binary: {Convert.ToString(i, 2).PadLeft(6, '0')} >  ");
    for(int j=0;j<6;j++){
        bus1[j] = (i & (1 << j)) != 0;
    }
    //print output
    byte output = (byte)((bus2[0] ? 1 : 0) + (bus2[1] ? 2 : 0) + (bus2[2] ? 4 : 0) + (bus2[3] ? 8 : 0) + (bus2[4] ? 16 : 0) + (bus2[5] ? 32 : 0));
    for(int j=0;j<6;j++){
        Console.Write(bus2[j] ? "1" : "0");
    }
    //print XORs
    Console.Write(" XOR ");
    for(int j=0;j<6;j++){
        Console.Write(xors[5-j][0] ? "1" : "0");
    }
    //print ORs
    Console.Write(" OR ");
    for(int j=0;j<3;j++){
        Console.Write(bus4[j] ? "1" : "0");
    }
    Console.Write(" /// ");
    Solution sol = Test.solve2(i);
    bool tri_equal = true;//sol.triag.SequenceEqual(bus3.Outputs.Reverse());
    bool xor_equal = true;//sol.xorr.SequenceEqual(bus3.Outputs);
    bool or_equal = sol.orr.SequenceEqual(bus4.Outputs);
    if( tri_equal && xor_equal && or_equal){
        Console.Write("\u001b[32m OK \u001b[0m ");
    }else{
        
        Console.Write(xor_equal ? "\u001b[32m" : "\u001b[31m");
        for(int j=0;j<6;j++){
            Console.Write(sol.triag[j] ? "1" : "0");
        }
        Console.Write(" XOR ");
        for(int j=0;j<6;j++){
            Console.Write(sol.xorr[j] ? "1" : "0");
        }
        Console.Write(" OR ");
        for(int j=0;j<3;j++)
            Console.Write(sol.orr[j] ? "1" : "0");
    }
    Console.WriteLine();
}


//measure us time
sw.Stop();
Console.WriteLine($"Elapsed time: {sw.ElapsedMilliseconds}ms");

