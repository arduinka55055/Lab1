using System;
using System.Collections.Generic;

namespace Lab1;

class Triangle{
    protected Ablock[,] ablocks;
    Bus bus1, bus2;
    public Triangle(Bus input, Bus output){
        ablocks =  new Ablock[5,5];
        bus1 = input;
        bus2 = output;
        
        /*
            Тут ми поєднуємо блоки в трикутник
            Елементи підключені знизу і зліва
            L - Left, B - Bottom, A - All
            процедурно створюємо блоки і підключаємо їх один до одного
            (краї не підключаємо, бо вони не мають сусідів,вони будуть потім)
        */
        for(int x=0;x<5;x++){
            for(int y=0;y<x+1;y++){
                ablocks[x,y] = new Ablock();
                Console.WriteLine($"creating block at {x},{y}");
                string type = "N"; //none
                //зліва, ікси і y<x для діагоналі
                if(x > 0 && y < x){
                    type = "L"; //left
                    //Console.WriteLine($"block at {x},{y} has left neighbour");
                    ablocks[x-1,y].Connect(ablocks[x,y], 0, 0);
                    ablocks[x-1,y].ConnectedTo.Add($"{x},{y}");
                }
                //знизу є всі, для y>0
                if(y > 0){
                    if(type == "L") type = "A"; else type = "B";
                    ablocks[x,y-1].Connect(ablocks[x,y], 1, 1);
                    ablocks[x,y-1].ConnectedTo.Add($"{x},{y}");
                    //Console.WriteLine($"block at {x},{y} has bottom neighbour");
                }
                Console.WriteLine($"block at {x},{y} is {type}");
            }
        }
        //Верхні краї з'єднуємо з вхідною шиною (формою Г)
        for(int i=0;i<4;i++){
            ablocks[i,i].Connect(ablocks[i+1,i+1], 1, 0);
            ablocks[i,i].ConnectedTo.Add($"{i+1},{i+1}");
        }
        //підключаємо вхід до трикутника
        bus1.Connect(ablocks[0,0], 0, 0);
        bus1.Connect(ablocks[0,0], 1, 1);
        bus1.Connect(ablocks[1,0], 2, 1);
        bus1.Connect(ablocks[2,0], 3, 1);
        bus1.Connect(ablocks[3,0], 4, 1);
        bus1.Connect(ablocks[4,0], 5, 1);

        //підключаємо вихід трикутника до шини XORів
        ablocks[4,0].Connect(bus2, 0, 5);
        ablocks[4,1].Connect(bus2, 0, 4);
        ablocks[4,2].Connect(bus2, 0, 3);
        ablocks[4,3].Connect(bus2, 0, 2);
        ablocks[4,4].Connect(bus2, 0, 1);
        ablocks[4,4].Connect(bus2, 1, 0);

    }
}