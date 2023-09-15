using System;
using System.Collections.Generic;

namespace Lab1;

class Pyramid{
    protected Ablock[,] ablocks;
    Bus bus1, bus2;
    public Ablock a = new Ablock();
    public Ablock b = new Ablock();
    public Ablock c = new Ablock();
    public Pyramid(Bus input, Bus output){
        ablocks =  new Ablock[3,3];
        bus1 = input;
        bus2 = output;
        
        //ініціялізація наперед
        for(int x=0;x<3;x++)
            for(int y=0;y<3;y++)
                ablocks[x,y] = new Ablock();
        /*
            Тут ми поєднуємо блоки вершини піраміди. верхня ножка іде наверх а нижня на низ
            процедурно створюємо блоки і підключаємо їх один до одного
            (краї підключаємо напряму в 0 (прямі лінії))
        */
        for(int x=0;x<3;x++){
            for(int y=0;y<3;y++){
                Console.WriteLine($"creating block at {x},{y}");
                if(x>0){
                    int yileft = Math.Max(y-1,0); //вище або 0 (стеля)
                    Console.WriteLine($"block at {x},{y} A connected to {x-1},{yileft}" + ((y==0)?"a-a":"b-a"));
                    if(y==0)
                        ablocks[x-1,yileft].Connect(ablocks[x,y], 0, 0);
                    else
                        ablocks[x-1,yileft].Connect(ablocks[x,y], 1, 0);
                    ablocks[x-1,yileft].ConnectedTo.Add($"{x},{y}"+ ((y==0)?"a-a":"b-a"));
                    
                    int ydleft = Math.Min(y+1,2); // нижче або 2 (підлога)
                    if(y==2)
                        ablocks[x-1,ydleft].Connect(ablocks[x,y], 1, 1);
                    else
                        ablocks[x-1,ydleft].Connect(ablocks[x,y], 0, 1);
                    ablocks[x-1,ydleft].ConnectedTo.Add($"{x},{y}"+ ((y==2)?"b-b":"a-b"));
                    Console.WriteLine($"block at {x},{y} B connected to {x-1},{ydleft}" + ((y==2)?"b-b":"a-b"));
                }
            }
        }
        //ручне зʼєднання вершини
        //
        ablocks[2,0].Connect(a, 1, 0);//b до а верх
        ablocks[2,1].Connect(a, 0, 1);//a до b верх
        ablocks[2,1].Connect(b, 1, 0);//b до a низ
        ablocks[2,2].Connect(b, 0, 1);//a до b низ
        //
        a.Connect(c, 1, 0);//b до a верш
        b.Connect(c, 0, 1);//a до b верш

        //підключаємо вхід до піраміди
        bus1.Connect(ablocks[0,0], 0, 0);
        bus1.Connect(ablocks[0,0], 1, 1);
        bus1.Connect(ablocks[0,1], 2, 0);
        bus1.Connect(ablocks[0,1], 3, 1);
        bus1.Connect(ablocks[0,2], 4, 0);
        bus1.Connect(ablocks[0,2], 5, 1);

        //підключаємо вихід піраміди до шини 
        ablocks[2,0].Connect(bus2, 0, 5); //a
        a.Connect(bus2, 0, 4);//a
        c.Connect(bus2, 0, 3);//a
        c.Connect(bus2, 1, 2);//b
        b.Connect(bus2, 1, 1);//b
        ablocks[2,2].Connect(bus2, 1, 0);//b

    }
}