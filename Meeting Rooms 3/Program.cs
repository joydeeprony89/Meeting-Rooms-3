using System.Reflection.Metadata.Ecma335;

namespace Meeting_Rooms_3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // https://www.youtube.com/watch?v=2VLwjvODQbA
            int n = 3;
            var meetings = new int[5][] { [1, 20], [2, 10], [3, 5], [4, 9], [6, 8] };
            var sol = new Solution();
            var ans = sol.MostBooked(n, meetings);
            Console.WriteLine(ans);
        }
    }

    public class Solution
    {
        public class Room
        {
            public int endingAt;
            public int roomno;
            public Room(int endingAt, int roomno)
            {
                this.endingAt = endingAt;
                this.roomno = roomno;
            }
        }
        public class MyComparer : IComparer<(int, int)>
        {
            public int Compare((int, int) a, (int, int) b)
            {
                if (a.Item1 == b.Item1)
                {
                    return a.Item2 - b.Item2;
                }
                else return a.Item1 - b.Item1;
            }
        }
        public int MostBooked(int n, int[][] meetings)
        {
            var count = new List<int>(new int[n]);
            var used = new PriorityQueue<(int endingAt, int roomno), (int endingAt, int roomno)>(new MyComparer());
            var available = new PriorityQueue<int, int>();
            for (int i = 0; i < n; i++)
            {
                available.Enqueue(i, i);
            }

            Array.Sort(meetings, (a, b) => a[0].CompareTo(b[0]));

            foreach (var meeting in meetings)
            {
                var start = meeting[0];
                var end = meeting[1];

                while (used.Count > 0 && start >= used.Peek().endingAt)
                {
                    var (_, usedroom) = used.Dequeue();
                    available.Enqueue(usedroom, usedroom);
                }

                if (available.Count == 0)
                {
                    var (endindAt, room) = used.Dequeue();
                    end = endindAt + (end - start);
                    available.Enqueue(room, room);
                }

                var availableroom = available.Dequeue();
                used.Enqueue((end, availableroom), (end, availableroom));
                count[availableroom]++;
            }

            var max = count.Max();
            var roomno = count.IndexOf(max);
            return roomno;
        }
    }
}
