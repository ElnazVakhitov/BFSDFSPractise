using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Поиски_в_ширину_и_глубину
{
    class DFS
    {
        class Node
        {
            public Node[] Neighbours;
            public Color Color;
            public int Number;
            public Node(int number)
            {
                Number = number;
                Color = Color.noncolor;
            }

            public override string ToString() => Number.ToString();
        }

        enum Color
        {
            black,
            white,
            noncolor
        }

        static void Main(string[] args)
        {
            var nodes = ParseInput();
            var stack = new Stack<Node>();
            var flag = false;
            while (stack.Count > 0 || nodes.Any(x => x.Color == Color.noncolor))
            {
                if (stack.Count == 0)
                {
                    var noda = nodes.Where(x => x.Color == Color.noncolor).First();
                    noda.Color = Color.white;
                    stack.Push(noda);
                }
                var currentNode = stack.Pop();
                foreach (var n in currentNode.Neighbours.Where(x => x.Color == Color.noncolor))
                    stack.Push(n);
                foreach (var n in currentNode.Neighbours)
                {
                    if (n.Color == currentNode.Color)
                    {
                        flag = true;
                        break;
                    }
                    n.Color = (Color)(((int)currentNode.Color + 1) % 2);
                }
                if (flag)
                    break;
            }
            StreamWriter f = new StreamWriter("out.txt");
            if (flag)
                f.WriteLine("N");
            else
            {
                f.WriteLine("Y");
                f.WriteLine(string.Join(" ", nodes.Where(x => x.Color == Color.white)
                    .Select(x => x.Number.ToString())) + " 0");
                
                f.WriteLine(string.Join(" ", nodes.Where(x => x.Color == Color.black)
                    .Select(x => x.Number.ToString())));
            }
            f.Close();
        }

        static Node[] ParseInput()
        {
            StreamReader sr = new StreamReader("in.txt");
            var countNode = int.Parse(sr.ReadLine());
            var nodes = new Node[countNode];
            for (var i = 0; i < countNode; i++)
                nodes[i] = new Node(i + 1);
            for (var i = 0; i < countNode; i++)
            {
                var line = sr.ReadLine();
                nodes[i].Neighbours = line.Substring(0, line.Length - 2)
                    .Split(' ')
                    .Select(x => nodes[int.Parse(x) - 1])
                    .ToArray();
            }
            sr.Close();
            return nodes;
        }
    }
}
