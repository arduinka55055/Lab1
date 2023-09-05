namespace Lab1;
public class NAND:NodeBase{
    public NAND():base(2,1){
    }
    public override void Compute(){
        _Outputs[0] = !(_Inputs[0] && _Inputs[1]);
    }
}
public class AND:NodeBase{
    public AND():base(2,1){
    }
    public override void Compute(){
        _Outputs[0] = _Inputs[0] && _Inputs[1];
    }
}
public class OR3:NodeBase{
    public OR3():base(3,1){
    }
    public override void Compute(){
        _Outputs[0] = _Inputs[0] || _Inputs[1] || _Inputs[2];
    }
}
public class XOR:NodeBase{
    public XOR():base(2,1){
    }
    public override void Compute(){
        _Outputs[0] = _Inputs[0] ^ _Inputs[1];
    }
}

// 0 is left right, 1 is up down
public class Ablock : NodeBase{
    public Ablock():base(2,2){
    }
    public override void Compute(){
        _Outputs[0] = _Inputs[0] || _Inputs[1];
        _Outputs[1] = _Inputs[0] && _Inputs[1];
    }
    public bool a{
        get => _Outputs[0];
        set => _Inputs[0] = value;
    }
    public bool b{
        get => _Outputs[1];
        set => _Inputs[1] = value;
    }
}

public class Bus : NodeBase{
    public string name;
    public Bus(int size, string name):base(size, size){
        this.name = name;
    }
    public override void Compute(){
        for(int i = 0; i < _Inputs.Length; i++){
            _Outputs[i] = _Inputs[i];
        }
    }
    public void Print(){
        Console.WriteLine($"{name} = {string.Join("", _Outputs.Select(x => x ? "1" : "0"))}");
    }
}