public class Graph
    {
        public int numVertices;
        public bool[,] adjacencyMatrix;
        List<List<int>> maximalMatrix;

        public Graph(int numVertices, bool[,] adjacencyMatrix)
        {
            this.numVertices = numVertices;
            this.adjacencyMatrix = adjacencyMatrix;
            this.maximalMatrix = new List<List<int>>();
        }

        public void print()
        {
            for (int i = 0; i < numVertices; ++i)
            {
                for (int j = 0; j < numVertices; ++j)
                {
                    Console.Write(this.adjacencyMatrix[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public void findAllMaximalCliques(bool[] nodes, bool[] open, List<int> clique)
        {
            // True if the current clique is maximal
            bool nodesAllFalse = true;

            for (int i = 0; i < numVertices; ++i)
            {
                if (nodes[i])
                {
                    nodesAllFalse = false;
                    break;
                }
            }

            // 1. Maximal clique was found
            // 1.1 Create a constraint which form is \sum max_clique <= shiftLimit 
            if (nodesAllFalse)
            {
                foreach (int vertex in clique)
                    Console.Write(vertex + " ");
                Console.WriteLine("\nA maximal clique was found!");
                this.maximalMatrix.Add(clique);
                return;

            }

            // 2. else: may extend the current clique.
            // auxiliary variables: maxVal keep the number of open neighbors of node maxNode
            int maxVal = -1;
            int maxNode = -1;

            // Choose node u with most open neighbors
            for (int u = 0; u < numVertices; ++u)
            {
                if (nodes[u])
                {
                    int tmp_count = 0;
                    for (int i = 0; i < numVertices; ++i)
                    {
                        if (open[i] && adjacencyMatrix[u, i])
                            ++tmp_count;
                    }
                    if (tmp_count > maxVal)
                    {
                        maxVal = tmp_count;
                        maxNode = u;
                    }
                }
            }

            // 3. extUidx corresponds to non-neighbors of u which are still open, i.e. open - adj[u]
            List<int> ExtUIdx = new List<int>();
            for (int i = 0; i < numVertices; ++i)
            {
                if (open[i] && !(adjacencyMatrix[maxNode, i]))
                    ExtUIdx.Add(i);

            }

            // 4. one recursive call for each vertex through which the clique can be extended
            foreach (int q in ExtUIdx)
            {
                // vertex q joins the clique
                open[q] = false;

                // new nodes and open sets restricting to neighbors of current vertex q
                bool[] nodesQ = new bool[this.numVertices];
                bool[] openQ = new bool[this.numVertices];
                for (int i = 0; i < numVertices; ++i)
                {
                    if (nodes[i] && this.adjacencyMatrix[q, i])
                        nodesQ[i] = true;

                    if (open[i] && this.adjacencyMatrix[q, i])
                        openQ[i] = true;
                }
                // Add node q to the current clique
                clique.Add(q);

                // recursive call
                this.findAllMaximalCliques(nodesQ, openQ, clique);
                
                // Go back to the original clique
                clique.RemoveAt(clique.Count() - 1);
            }
        }
    }
