
string input = File.ReadAllText("input").Trim();

List<MemoryFile> ParseMemory()
{
    List<MemoryFile> memory = [];
    for (int i = 0; i < input.Length; i += 2)
    {
        memory.Add(new MemoryFile {
            Id = i / 2,
            Size = input[i] - '0',
            FreeBlocks = i + 1 < input.Length ? input[i + 1] - '0' : -1
        });
    }
    return memory;
}

void PackMemory(List<MemoryFile> memory)
{
    while (true)
    {
        int freeIndex = memory.FindIndex(file => file.FreeBlocks >= 1);
        if (freeIndex == -1) break;

        MemoryFile freeMemory = memory[freeIndex];
        MemoryFile lastFile = memory[^1];

        if (ReferenceEquals(freeMemory, lastFile)) break;

        long movedBlocks = Math.Min(freeMemory.FreeBlocks, lastFile.Size);

        memory.Insert(freeIndex + 1, new MemoryFile { Id = lastFile.Id, Size = movedBlocks, FreeBlocks = freeMemory.FreeBlocks - movedBlocks });
        freeMemory.FreeBlocks = 0;
        lastFile.Size -= movedBlocks;
        if (lastFile.Size == 0)
        {
            memory.RemoveAt(memory.Count - 1);
        }
    }
}

void PackMemoryButBetter(List<MemoryFile> memory)
{
    for (long id = memory.Max(x => x.Id); id >= 0; id--)
    {
        int currentIndex = memory.FindIndex(file => file.Id == id);
        MemoryFile currentFile = memory[currentIndex];

        int freeIndex = memory.FindIndex(file => file.FreeBlocks >= currentFile.Size);
        if (freeIndex == -1 || freeIndex >= currentIndex)
        {
            continue;
        }
        MemoryFile freeMemory = memory[freeIndex];

        memory[currentIndex - 1].FreeBlocks += currentFile.Size + currentFile.FreeBlocks;
        memory.RemoveAt(currentIndex);
        memory.Insert(freeIndex + 1, new MemoryFile { Id = currentFile.Id, Size = currentFile.Size, FreeBlocks = freeMemory.FreeBlocks - currentFile.Size });
        freeMemory.FreeBlocks = 0;
    }
}

long ComputeChecksum(List<MemoryFile> memory)
{
    long checksum = 0;
    long index = 0;
    foreach (MemoryFile file in memory)
    {
        for (int i = 0; i < file.Size; i++)
        {
            checksum += index * file.Id;
            index++;
        }
        index += file.FreeBlocks;
    }
    return checksum;
}

List<MemoryFile> memory = ParseMemory();
PackMemory(memory);
Console.WriteLine(ComputeChecksum(memory));

memory = ParseMemory();
PackMemoryButBetter(memory);
Console.WriteLine(ComputeChecksum(memory));

record MemoryFile
{
    public long Id { get; set; }
    public long Size { get; set; }
    public long FreeBlocks { get; set; }
}
