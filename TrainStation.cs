namespace TrainSwitching.Logic;

using static TrainSwitching.Logic.Constants;

public class TrainStation
{
    public Track[] Tracks { get; }

    public TrainStation()
    {
        Tracks = new Track[10];
        for (var i = 0; i < 10; i++)
        {
            Tracks[i] = new Track();
        }
    }

    /// <summary>
    /// Tries to apply the given operation to the train station.
    /// </summary>
    /// <param name="op">Operation to apply</param>
    /// <returns>Returns true if the operation could be applied, otherwise false</returns>

    public bool TryApplyOperation(SwitchingOperation op)
    {
        if (op.TrackNumber < 1 || op.TrackNumber > 10)
        {
            return false;
        }

        if (op.Direction == Constants.DIRECTION_EAST)
        {
            if (op.TrackNumber == 10 || op.TrackNumber == 9)
            {
                return false;
            }
        }

        if (op.OperationType == OPERATION_REMOVE)
        {
            if (op.NumberOfWagons > Tracks[op.TrackNumber - 1].Wagons.Count)
            {
                return false;
            }
        }

        if (op.OperationType == OPERATION_TRAIN_LEAVE)
        {
            if (Tracks[op.TrackNumber - 1].Wagons.Count == 0)
            {
                return false;
            }

            else if (!Tracks[op.TrackNumber - 1].Wagons.Contains(Constants.WAGON_TYPE_LOCOMOTIVE))
            {
                return false;
            }
        }

        if (op.OperationType == OPERATION_ADD)
        {
            if (op.Direction == Constants.DIRECTION_EAST)
            {
                Tracks[op.TrackNumber - 1].Wagons.Add(op.WagonType!.Value);
            }

            else if (op.Direction == Constants.DIRECTION_WEST)
            {
                Tracks[op.TrackNumber - 1].Wagons.Insert(0, op.WagonType!.Value);
            }
        }

        else if (op.OperationType == OPERATION_REMOVE)
        {
            if (op.Direction == DIRECTION_EAST)
            {
                for (int i = 0; i < op.NumberOfWagons; i++)
                {
                    Tracks[op.TrackNumber - 1].Wagons.RemoveAt(Tracks[op.TrackNumber - 1].Wagons.Count - 1);
                }
            }

            else if (op.Direction == DIRECTION_WEST)
            {
                for (int i = 0; i < op.NumberOfWagons; i++)
                {
                    Tracks[op.TrackNumber - 1].Wagons.RemoveAt(0);
                }
            }
        }

        else if (op.OperationType == OPERATION_TRAIN_LEAVE)
        {
            Tracks[op.TrackNumber - 1].Wagons.Clear();
        }

        return true;
    }

    /// <summary>
    /// Calculates the checksum of the train station.
    /// </summary>
    /// <returns>The calculated checksum</returns>
    /// <remarks>
    /// See readme.md for details on how to calculate the checksum.
    /// </remarks>
    public int CalculateChecksum()
    {
        var sum = 0;

        for (var i = 0; i < Tracks.Length; i++)
        {
            int wagonSum = 0;

            foreach (var wagon in Tracks[i].Wagons)
            {
                wagonSum += wagon switch
                {
                    WAGON_TYPE_PASSENGER => 1,
                    WAGON_TYPE_LOCOMOTIVE => 10,
                    WAGON_TYPE_FREIGHT => 20,
                    WAGON_TYPE_CAR_TRANSPORT => 30,
                    _ => throw new InvalidOperationException("Invalid wagon type"),
                };
            }

            sum += (i + 1) * wagonSum;
        }

        return sum;
    }
}