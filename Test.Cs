using System.Collections.Generic;
namespace Lab1;
public class Test{
    private static Dictionary<byte,byte> _ideal = new Dictionary<byte,byte>();
    private static bool _isCold = true;
    public static Dictionary<byte,byte> Ideal {
        get{
            if(_isCold){
                _isCold = false;
                Init();
            }
            return _ideal;
        }
    }
    public static void Init(){
        for(int i=0;i<Math.Pow(2,6);i++){
            int[] a = new int[6];
            a = Convert.ToString(i, 2).PadLeft(6, '0').Select(x => int.Parse(x.ToString())).ToArray(); 
            int[,] b = new int[5,5];
            b[4,0] = a[0] | a[1];
            b[4,1] = b[4,0] | a[2];
            b[4,2] = b[4,1] | a[3];
            b[4,3] = b[4,2] | a[4];
            b[4,4] = b[4,3] | a[5];
            b[3,1] = (Convert.ToBoolean(a[0]) && Convert.ToBoolean(a[1])) || (Convert.ToBoolean(b[4,0]) && Convert.ToBoolean(a[2])) ? 1 : 0;
            b[3,2] = b[3,1] | (b[4,1] & a[3]);
            b[3,3] = b[3,2] | (b[4,2] & a[4]);
            b[3,4] = b[3,3] | (b[4,3] & a[5]);
            b[2,2] = (Convert.ToBoolean(a[0]) && Convert.ToBoolean(a[1])) && (Convert.ToBoolean(b[4,0]) && Convert.ToBoolean(a[2])) || (Convert.ToBoolean(b[3,1]) && (Convert.ToBoolean(b[4,1]) &&
            Convert.ToBoolean(a[3]))) ? 1 : 0;
            b[2,3] = b[2,2] | ((b[3,2] & (b[4,2] & a[4])));
            b[2,4] = b[2,3] | (b[3,3] & (b[4,3] & a[5]));
            b[1,3] = ((Convert.ToBoolean(a[0]) && Convert.ToBoolean(a[1])) && (Convert.ToBoolean(b[4,0]) && Convert.ToBoolean(a[2])) && ((Convert.ToBoolean(b[4,1]) && Convert.ToBoolean(a[3])))) ||
            ((Convert.ToBoolean(b[4,2]) && Convert.ToBoolean(a[4])) && Convert.ToBoolean(b[3,2])) ? 1 : 0;
            b[1,4] = b[1,3] | (b[2,3] & (b[3,3] & (b[4,3] & a[5])));
            b[0,4] = ((Convert.ToBoolean(a[0]) && Convert.ToBoolean(a[1])) && (Convert.ToBoolean(b[4,0]) && Convert.ToBoolean(a[2])) && (Convert.ToBoolean(b[4,1]) && Convert.ToBoolean(a[3])) &&
            (Convert.ToBoolean(b[4,2]) && Convert.ToBoolean(a[4]))) || (Convert.ToBoolean(b[4,3]) && Convert.ToBoolean(a[5]) && Convert.ToBoolean(b[3,3]) && Convert.ToBoolean(b[2,3]) && Convert.ToBoolean(b[1,3])) ? 1 : 0;
            //output
            byte c = (byte)(b[0,4] | b[1,4] << 1 | b[2,4] << 2 | b[3,4] << 3 | b[4,4] << 4);
            _ideal.Add((byte)i, c);
        }
    }
}