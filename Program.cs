// See https://aka.ms/new-console-template for more information

using System.Reflection.Metadata.Ecma335;
using Lab1;

//measure us time
var sw = new System.Diagnostics.Stopwatch();
sw.Start();



Bus bus1 = new Bus(6, "input");
Bus bus2 = new Bus(6, "output");
Bus bus3 = new Bus(6, "XORs");
Bus bus4 = new Bus(3, "Final");
//make pyramid of A blocks
Ablock[,] ablocks = new Ablock[5,5];


//connect blocks
for(int x=0;x<5;x++){
    for(int y=0;y<x+1;y++){
        ablocks[x,y] = new Ablock();
        Console.WriteLine($"creating block at {x},{y}");
        //columns
        //Console.WriteLine($"placing block at {x},{y}");
        //check element on the left
        string type = "N"; //none
        if(x > 0 && y < x){
            type = "L"; //left
            //Console.WriteLine($"block at {x},{y} has left neighbour");
            ablocks[x-1,y].Connect(ablocks[x,y], 0, 0);
        }
        //check element below
        if(y > 0){
            if(type == "L")
                type = "A"; //All
            else 
                type = "B"; //bottom
            ablocks[x,y-1].Connect(ablocks[x,y], 1, 1);
            //Console.WriteLine($"block at {x},{y} has bottom neighbour");
        }
        Console.WriteLine($"block at {x},{y} is {type}");
    }
}

//connect XORs
XOR[] xors = new XOR[6];
for(int i=0;i<6;i++){
    xors[i] = new XOR();
    int ain_index = i-1;
    int bin_index = i;
    bus2.Connect(xors[i], bin_index, 1); // connect B to XOR
    if(ain_index>=0)
        bus2.Connect(xors[i], ain_index, 0); // connect A to XOR
    //connect xor to bus3
    xors[i].Connect(bus3, 0, i);
}
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
oR3s[0].Connect(bus4, 0, 0);
oR3s[1].Connect(bus4, 0, 1);
oR3s[2].Connect(bus4, 0, 2);


//init bus
bus1.Connect(ablocks[0,0], 0, 0);
bus1.Connect(ablocks[0,0], 1, 1);
bus1.Connect(ablocks[1,0], 2, 1);
bus1.Connect(ablocks[2,0], 3, 1);
bus1.Connect(ablocks[3,0], 4, 1);
bus1.Connect(ablocks[4,0], 5, 1);

//connect bus2 to right out 
ablocks[4,0].Connect(bus2, 0, 0);
ablocks[4,0].Connect(bus2, 0, 1);
ablocks[4,1].Connect(bus2, 0, 2);
ablocks[4,2].Connect(bus2, 0, 3);
ablocks[4,3].Connect(bus2, 0, 4);
ablocks[4,4].Connect(bus2, 0, 5);

Dictionary<byte,byte> real = new Dictionary<byte,byte>();
for(int i=0;i<Math.Pow(2,6);i++){
    Console.Write($"input: {i}, binary: {Convert.ToString(i, 2).PadLeft(6, '0')} >  ");
    //set input
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
        Console.Write(xors[j][0] ? "1" : "0");
    }
    //print ORs
    Console.Write(" OR ");
    for(int j=0;j<3;j++){
        Console.Write(bus4[j] ? "1" : "0");
    }
    real.Add((byte)i, output);
    Console.WriteLine();
}

/*
Console.WriteLine("Side by side:");
for(int i=0;i<Math.Pow(2,6);i++){
    Console.Write(Convert.ToString(i, 2).PadLeft(6, '0'));
    Console.Write(" ");
    Console.Write(Convert.ToString(real[(byte)i], 2).PadLeft(6, '0'));
    Console.Write(" ");
    Console.Write(Convert.ToString(Test.Ideal[(byte)i], 2).PadLeft(6, '0'));
    Console.Write(" ");
    if(real[(byte)i] == Test.Ideal[(byte)i])
        Console.Write("OK");
    else
        Console.Write("\u001b[31mFAIL\u001b[0m");
    Console.WriteLine();
}*/
//measure us time
sw.Stop();
Console.WriteLine($"Elapsed time: {sw.ElapsedMilliseconds}ms");

