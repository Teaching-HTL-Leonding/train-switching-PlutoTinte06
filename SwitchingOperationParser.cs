namespace TrainSwitching.Logic;

public static class SwitchingOperationParser
{
    /// <summary>
    /// Parses a line of input into a <see cref="SwitchingOperation"/>.
    /// </summary>
    /// <param name="inputLine">Line to parse. See readme.md for details</param>
    /// <returns>The parsed switching operation</returns>

    public static SwitchingOperation Parse(string inputLine)
    {
        var parts = inputLine.Split(' ');
        var trackNumber = int.Parse(parts[2].Trim(','));
        var direction = parts.Last() switch
        {
            "East" => 0,
            "West" => 1,
            _ => throw new InvalidOperationException("Invalid direction")
        };

        if (parts[3] == "add")
        {
            var wagonType = parts[4] switch
            {
                "Passenger" => 0,
                "Locomotive" => 1,
                "Freight" => 2,
                "Car" => 3,
                _ => throw new InvalidOperationException("Invalid wagon type")
            };

            return new SwitchingOperation
            {
                TrackNumber = trackNumber,
                Direction = direction,
                OperationType = 1,
                WagonType = wagonType
            };
        }

        if (parts[3] == "remove")
        {
            var count = int.Parse(parts[4]);
            return new SwitchingOperation
            {
                TrackNumber = trackNumber,
                Direction = direction,
                OperationType = -1,
                NumberOfWagons = count
            };
        }

        if (parts[3] == "train")
        {
            return new SwitchingOperation
            {
                TrackNumber = trackNumber,
                Direction = direction,
                OperationType = 0
            };
        }

        return new SwitchingOperation();
    }
}