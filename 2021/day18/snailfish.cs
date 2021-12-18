Number[] input = File.ReadAllLines("input").Select(Number.Parse).ToArray();

Console.WriteLine(input.Aggregate((a, b) => a + b).Magnitude);
Console.WriteLine(input.SelectMany(a => input.Where(b => a != b).Select(b => (a + b).Magnitude)).Max());

abstract class Number
{
    public Pair? Parent { get; set; }

    public abstract long Magnitude { get; }

    public void Reduce()
    {
        while (ExplodeIfRequired() || SplitIfRequired());
    }

    public abstract bool ExplodeIfRequired(int depth = 0);
    public abstract bool SplitIfRequired();

    public Number Clone() => Parse(this.ToString()!);

    public static Number operator +(Number left, Number right)
    {
        Pair pair = (Pair)new Pair(left.Clone(), right.Clone()).Clone();
        pair.Reduce();
        return pair;
    }

    public static Number Parse(string value)
    {
        return new NumberParser(value).Parse();
    }
}

class Pair : Number
{
    public Number Left { get; set; }
    public Number Right { get; set; }

    public override long Magnitude => Left.Magnitude * 3 + Right.Magnitude * 2;

    public Pair(Number left, Number right)
    {
        Left = left;
        Right = right;

        left.Parent = this;
        right.Parent = this;
    }

    public override bool ExplodeIfRequired(int depth)
    {
        if (depth == 4 && Left is Regular left && Right is Regular right)
        {
            Regular zero = new Regular(0);

            if (left.Left != null)
            {
                left.Left.Value += left.Value;
                left.Left.Right = zero;
                zero.Left = left.Left;
            }
            if (right.Right != null)
            {
                right.Right.Value += right.Value;
                right.Right.Left = zero;
                zero.Right = right.Right;
            }

            if (Parent == null)
            {
                throw new Exception("Cannot explode a number with no parent.");
            }

            zero.Parent = Parent;

            if (Parent.Left == this) Parent.Left = zero;
            else if (Parent.Right == this) Parent.Right = zero;

            return true;
        }

        if (Left.ExplodeIfRequired(depth + 1)) return true;
        return Right.ExplodeIfRequired(depth + 1);
    }

    public override bool SplitIfRequired()
    {
        if (Left.SplitIfRequired()) return true;
        return Right.SplitIfRequired();
    }

    public override string ToString() => $"[{Left},{Right}]";
}

class Regular : Number
{
    public long Value { get; set; }

    public Regular? Left { get; set; }
    public Regular? Right { get; set; }

    public override long Magnitude => Value;

    public Regular(long value)
    {
        Value = value;
    }

    public override bool ExplodeIfRequired(int depth) => false;

    public override bool SplitIfRequired()
    {
        if (Value < 10) return false;

        Regular splitLeft = new Regular(Value / 2);
        Regular splitRight = new Regular(Value / 2 + Value % 2);
        Pair split = new Pair(splitLeft, splitRight);

        split.Parent = Parent;

        splitLeft.Left = Left;
        splitLeft.Right = splitRight;
        splitRight.Left = splitLeft;
        splitRight.Right = Right;
        if (Left != null) Left.Right = splitLeft;
        if (Right != null) Right.Left = splitRight;

        if (Parent == null)
        {
            throw new Exception("Cannot split a number with no parent.");
        }

        if (Parent.Left == this) Parent.Left = split;
        else if (Parent.Right == this) Parent.Right = split;

        return true;
    }

    public override string ToString() => Value.ToString();
}

class NumberParser
{
    private StringReader reader;
    private Regular? previousRegular;

    public NumberParser(string input)
    {
        reader = new StringReader(input);
    }

    public Number Parse()
    {
        if (reader.Peek() == '[')
        {
            reader.Read(); // '['
            Number left = Parse();
            reader.Read(); // ','
            Number right = Parse();
            reader.Read(); // ']'
            return new Pair(left, right);
        }
        else
        {
            Regular regular = new Regular(reader.Read() - '0');
            if (previousRegular != null)
            {
                previousRegular.Right = regular;
                regular.Left = previousRegular;
            }
            previousRegular = regular;
            return regular;
        }
    }
}
