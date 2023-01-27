using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace HW1
{
    public class AddNumbers
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Enter a collection of numbers in the range [0, 100], separated by spaces: ");
            String[] numbers = Console.ReadLine().Split();
            String[] dist = numbers.Distinct().ToArray(); // remove duplicate
            BST tree = new BST();

            int x = 0; // x counts the number of nodes
            for (int i = 0; i < dist.Length; i++)
            {
                tree.add(Convert.ToInt32(dist[i]));
                x++;
            }
            // calculate the minimum number of levels
            double minimum = Math.Log(x, 2.0);
            int min = Convert.ToInt32(minimum);

            // print tree content and statistic
            Console.Write("Tree contents: ");
            tree.print();
            Console.WriteLine();
            Console.WriteLine("Tree statistics:");
            Console.WriteLine(" Number of nodes: " + x);
            Console.WriteLine(" Number of levels: " + tree.level(tree.getRoot()));
            Console.WriteLine(" Minimum number of levels that a tree with " + x + " nodes could have = " + min);
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }

    class Node
    {
        public int data;
        public Node left;
        public Node right;
    }

    class BST
    {
        private Node root;

        public Node getRoot()
        {
            return root;
        }

        public BST()
        {
            root = null;
        }

        /// <summary>
        /// this method finds the levels of a tree
        /// </summary>
        /// <param name="node"></param>
        /// <returns> the number of levels of the tree </returns>
        public int level(Node node)
        {
            if (node == null)
            {
                return 0;
            }
            else
            {
                int left = level(node.left);
                int right = level(node.right);

                if (left > right)
                {
                    return (left + 1);
                }
                else
                {
                    return (right + 1);
                }
            }
        }

        // this method adds data
        public void add(int data)
        {
            Node node = new Node();
            node.data = data;
            node.left = null;
            node.right = null;
            if (root == null)
            {
                root = node;
            }
            else
            {
                add(root, node);
            }
        }

        // this method adds nodes to the tree
        public void add(Node node, Node newNode)
        {
            if (newNode.data < node.data)
            {
                if (node.left == null)
                    node.left = newNode;
                else
                    add(node.left, newNode);
            }
            else
            {
                if (node.right == null)
                    node.right = newNode;
                else
                    add(node.right, newNode);
            }
        }

        // this method prints node
        public void print(Node node)
        {
            if (node != null)
            {
                print(node.left);
                Console.Write(node.data + " ");
                print(node.right);
            }
        }

        public void print()
        {
            print(root);
        }

    }
}
