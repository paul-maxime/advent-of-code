string[] map = File.ReadAllLines("input");

int mapHeight = map.Length;
int mapWidth = map[0].Length;

int ComputeEnergy(string[] map, (int x, int y, int dx, int dy) start)
{
    Queue<(int x, int y, int dx, int dy)> beams = [];
    beams.Enqueue(start);

    HashSet<(int x, int y, int dx, int dy)> visited = [];
    HashSet<(int x, int y)> energized = [];

    void EnqueueBeam(int x, int y, int dx, int dy)
    {
        if (!visited.Add((x, y, dx, dy))) return;

        x += dx;
        y += dy;

        if (x < 0 || x >= mapWidth) return;
        if (y < 0 || y >= mapHeight) return;

        beams.Enqueue((x, y, dx, dy));
    }

    while (beams.Count > 0)
    {
       var beam = beams.Dequeue();
        energized.Add((beam.x, beam.y));

        switch (map[beam.y][beam.x])
        {
            case '.':
                EnqueueBeam(beam.x, beam.y, beam.dx, beam.dy);
                break;
            case '-':
                if (beam.dx != 0)
                {
                    EnqueueBeam(beam.x, beam.y, beam.dx, beam.dy);
                }
                else
                {
                    EnqueueBeam(beam.x, beam.y, -1, 0);
                    EnqueueBeam(beam.x, beam.y, 1, 0);
                }
                break;
            case '|':
                if (beam.dy != 0)
                {
                    EnqueueBeam(beam.x, beam.y, beam.dx, beam.dy);
                }
                else
                {
                    EnqueueBeam(beam.x, beam.y, 0, -1);
                    EnqueueBeam(beam.x, beam.y, 0, 1);
                }
                break;
            case '/':
                if (beam.dx == 1) EnqueueBeam(beam.x, beam.y, 0, -1); // right -> up
                if (beam.dx == -1) EnqueueBeam(beam.x, beam.y, 0, 1); // left -> down
                if (beam.dy == 1) EnqueueBeam(beam.x, beam.y, -1, 0); // down -> left
                if (beam.dy == -1) EnqueueBeam(beam.x, beam.y, 1, 0); // up -> right
                break;
            case '\\':
                if (beam.dx == 1) EnqueueBeam(beam.x, beam.y, 0, 1); // right -> down
                if (beam.dx == -1) EnqueueBeam(beam.x, beam.y, 0, -1); // left -> up
                if (beam.dy == 1) EnqueueBeam(beam.x, beam.y, 1, 0); // down -> right
                if (beam.dy == -1) EnqueueBeam(beam.x, beam.y, -1, 0); // up -> left
                break;
        }
    }

    return energized.Count;
}

Console.WriteLine(ComputeEnergy(map, (0, 0, 1, 0)));

int maximumEnergy = Math.Max(
    Enumerable.Range(0, mapWidth).Select(x => Math.Max(
        ComputeEnergy(map, (x, 0, 0, 1)),
        ComputeEnergy(map, (x, mapHeight - 1, 0, -1))
    )).Max(),
    Enumerable.Range(0, mapHeight).Select(y => Math.Max(
        ComputeEnergy(map, (0, y, 1, 0)),
        ComputeEnergy(map, (mapWidth - 1, y, -1, 0))
    )).Max()
);

Console.WriteLine(maximumEnergy);
