using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Поиски_в_ширину_и_глубину
{
    class BFS
    {
        class Node
        {
            public Node previous;
            public int x;
            public int y;
            public string name;
            public bool was;
            public Node(Node previous, int x, int y)
            {
                this.previous = previous;
                this.x = x;
                this.y = y;
                this.name = string.Format("{1}{0}", x + 1, (char)(y + 97));
                this.was = false;
            }

            public override int GetHashCode() => x + y * 8;
            public override bool Equals(object other) => other is Node
                && ((Node)other).x == x && ((Node)other).y == y;
        }

        static void aMain(string[] args)
        {
            StreamReader sr = new StreamReader("in.txt");
            var knight = sr.ReadLine();
            var pawn = sr.ReadLine();
            sr.Close();
            
            var board = new List<Node>();
            var queue = new Queue<Node>();
            for (var i = 0; i < 64; i++)
                board.Add(new Node(null, i % 8, i / 8));
            var startNode = new Node(null,knight[1]-49, knight[0] -97);
            var endNode = new Node(null, pawn[1] - 49, pawn[0] - 97);
            board[startNode.x + startNode.y * 8] = startNode;
            board[endNode.x + endNode.y * 8] = endNode;
            if (inBound(endNode.x + 1, endNode.y - 1))
                board[endNode.x + endNode.y * 8 - 7].was = true;
            if (inBound(endNode.x - 1, endNode.y - 1))
                board[endNode.x + endNode.y * 8 - 9].was = true;
            var currentNode = new Node(null,-1,-1);
            queue.Enqueue(startNode);
            while (queue.Count() > 0)
            {
                currentNode = queue.Dequeue();
                if (currentNode.Equals(endNode))
                    break;
                currentNode.was = true;
                foreach (var n in knightSteps(currentNode, board))
                {
                    n.previous = currentNode;
                    queue.Enqueue(n);
                }
            }

            var answer = new List<String>();
            while(!(currentNode.previous is null))
            {
                answer.Add(currentNode.name);
                currentNode = currentNode.previous;
            }
            answer.Add(currentNode.name);
            answer.Reverse();
            StreamWriter f = new StreamWriter("out.txt");
            f.Write(string.Join("\n",answer));
            f.Close();

        }

        static bool inBound(int x, int y) => 0 <= x && x <= 7 && 0 <= y && y <= 7;

        static Node[] knightSteps(Node n, List<Node> board)
        {
            //int[] d = { -2,-1, 1, 2 };
            //return d.SelectMany(x => d.Where(y => Math.Abs(x) + Math.Abs(y) == 3 && inBound(x + n.x, y + n.y) && !board[x + n.x + (y + n.y) * 8].was)
            //                    .Select(y => board[x + n.x + (y + n.y) * 8])).ToArray();
            var result = new List<Node>();
            knightStep(-1, 2, n, board, result);
            knightStep(1, 2, n, board, result);
            knightStep(2, 1, n, board, result);
            knightStep(2, -1, n, board, result);
            knightStep(1, -2, n, board, result);
            knightStep(-1, -2, n, board, result);
            knightStep(-2, -1, n, board, result);
            knightStep(-2, 1, n, board, result);
            return result.ToArray();
        }

        static void knightStep(int x, int y, Node n, List<Node> board, List<Node> result)
        {
            if (inBound(x + n.x, y + n.y) && !board[x + n.x + (y + n.y) * 8].was)
                result.Add(board[x + n.x + (y + n.y) * 8]);
        }
    }


    
}
