namespace Lab1;

public interface Node{
    public void Connect(Node node, int src, int dst);
    public void Write(bool value, int index);
    public bool Read(int index);
    //virtual array operator
    public bool this[int index]{
        get => Read(index);
        set => Write(value, index);
    }
    public abstract void Compute();
}
public class SignalChangeEventArgs:EventArgs{
    public bool Value{get;set;}
    public int Index{get;set;}
    public Node? Node{get;set;}
}

public class NodeBase:Node {
    protected bool[] _Inputs;
    protected bool[] _Outputs;
    public List<string> ConnectedTo = new List<string>();
    private bool isCold = true;
    protected event Action<SignalChangeEventArgs> OnOutputChanged;

    public NodeBase(int inputCount, int outputCount){
        _Inputs = new bool[inputCount];
        _Outputs = new bool[outputCount];
    }
    public bool this[int index]{
        get => Read(index);
        set => Write(value, index);
    }
    public virtual bool Read(int index){
        if(index >= _Outputs.Length){
            throw new IndexOutOfRangeException();
        }
        return _Outputs[index];
    }
    public virtual void Write(bool value, int index){
        if(index >= _Inputs.Length){
            throw new IndexOutOfRangeException();
        }
        _Inputs[index] = value;
        bool[] prev = _Outputs.Clone() as bool[];
        Compute();
        if(Enumerable.SequenceEqual(prev, _Outputs) && true){
            return; //buggy for child nodes
            if(isCold)
                isCold = false;//init child nodes
            else
                return;//no change
        }
        for(int i = 0; i < _Outputs.Length; i++){
            OnOutputChanged?.Invoke(new SignalChangeEventArgs{Value = _Outputs[i], Index = i, Node = this});
        }
    }
    public void Connect(Node node, int src, int dst){
        OnOutputChanged += signalArgs => {
            if(signalArgs.Index == src){
                node.Write(signalArgs.Value, dst);
                //Console.WriteLine($"\u001b[32mSignal changed at {signalArgs.Node} {signalArgs.Index} to {signalArgs.Value}");
            };
        };
    }
    public virtual void Compute(){}
}



public class LogicMonitor:Node {
    public void Connect(Node node, int src, int dst){
        throw new NotSupportedException();
    }
    public void Write(bool value, int index){
        Console.WriteLine($"Signal changed to {value} at {index}");
    }
    public void Compute(){
        throw new NotSupportedException();
    }
    public bool Read(int index){
        throw new NotSupportedException();
    }
}
