System.Text.RegularExpressions.Regex numberRegex = new(@"[0-9]+");

List<(long ax, long ay, long bx, long by, long px, long py)> machines = File.ReadAllText("input")
    .Split("\n\n")
    .Select(block => numberRegex
        .Matches(block)
        .Select(x => long.Parse(x.Value))
        .ToArray()
    )
    .Select(data => (
        ax: data[0],
        ay: data[1],
        bx: data[2],
        by: data[3],
        px: data[4],
        py: data[5]
    ))
    .ToList();

// Looks like there is never more than one solution per equation, so we can solve it with Z3.
long SolvePrice((long ax, long ay, long bx, long by, long px, long py) machine)
{
    var context = new Microsoft.Z3.Context();
    var solver = context.MkSolver();

    var a = context.MkIntConst("a");
    var b = context.MkIntConst("b");
    var ax = context.MkInt(machine.ax);
    var ay = context.MkInt(machine.ay);
    var bx = context.MkInt(machine.bx);
    var by = context.MkInt(machine.by);
    var px = context.MkInt(machine.px);
    var py = context.MkInt(machine.py);

    solver.Add(a >= 0);
    solver.Add(b >= 0);
    solver.Add(context.MkEq(context.MkAdd(context.MkMul(ax, a), context.MkMul(bx, b)), px));
    solver.Add(context.MkEq(context.MkAdd(context.MkMul(ay, a), context.MkMul(by, b)), py));

    if (solver.Check() != Microsoft.Z3.Status.SATISFIABLE)
    {
        return 0;
    }

    var model = solver.Model;
    var resultA = long.Parse(model.Eval(a).ToString());
    var resultB = long.Parse(model.Eval(b).ToString());

    return resultA * 3 + resultB;
}

Console.WriteLine(machines.Select(SolvePrice).Sum());

Console.WriteLine(machines.Select(m => (
    m.ax, m.ay, m.bx, m.by,
    px: m.px + 10000000000000L,
    py: m.py + 10000000000000L
)).Select(SolvePrice).Sum());
