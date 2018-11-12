using libsvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess;
using Amatzia.Controllers;
namespace Amatzia.Utils
{
    public static class MLRecommended
    {
        #region Prop

        public static List<string> X
        {
            get
            {
                return DataTable.Rows.Select(row => row["Text"]).ToList();
            }            
        }

        public static double[] Y
        {
            get
            {
                return DataTable.Rows.Select(row => double.Parse(row["IsRecommended"])).ToArray();
            }
        }

        public static DataTable DataTable
        {
            get
            {
                const String dataFilePath = @"C:\Users\Bar\source\repos\zona\zona\Data.csv";
                var dataTable = DataTable.New.ReadCsv(dataFilePath);
                return dataTable;
            }
        }

        public static IReadOnlyList<string> Vocabulary
        {
            get
            {
                return X.SelectMany(GetWords).Distinct().OrderBy(word => word).ToList();
            }
        }

        public static Dictionary<int,string> Dictionary
        {
            get
            {
                return new Dictionary<int, string> { { -1, "NotRecommended" }, { 1, "Recommended" } };
            }
        }
        #endregion
      
        private static svm_problem CreateProblem(IEnumerable<string> x, double[] y, IReadOnlyList<string> vocabulary)
        {
            var problemBuilder = new TextClassificationProblemBuilder();
            var problem = problemBuilder.CreateProblem(X, Y, Vocabulary);
            return problem;
        }
        
        public static C_SVC CreateModel()
        {        
            var prob = CreateProblem(X, Y, Vocabulary);
            const int C = 1;
            return new C_SVC(prob, KernelHelper.LinearKernel(), C);
        }
        
        public static bool IsRec(string strComm)
        {
            var prob = MLRecommended.CreateNode(strComm);
            var predo = MLRecommended.CreateModel().Predict(prob);

            return Dictionary[(int)predo] == "Recommended";
        }
        //public static void Check()
        //{
        //    CommentController cComment = new CommentController();
        //    const String dataFilePath = @"C:\Users\Bar\source\repos\zona\zona\Data.csv";
        //    var dataTable = DataTable.New.ReadCsv(dataFilePath);
        //    List<string> x = dataTable.Rows.Select(row => row["Text"]).ToList();
        //    double[] y = dataTable.Rows.Select(row => double.Parse(row["IsSunny"])).ToArray();
        //    var vocabulary = x.SelectMany(GetWords).Distinct().OrderBy(word => word).ToList();
          

            
        //    _predictionDictionary = new Dictionary<int, string> { { -1, "NotRecommended" }, { 1, "Recommended" } };

        //    var comments = cComment.GetAllComments();
        //    foreach (var comment in comments)
        //    {
        //        string comm = comment.Content;
        //        if (comm != null)
        //        {
        //            var newprob = TextClassificationProblemBuilder.CreateNode(comm, vocabulary);
        //            var predo = model.Predict(newprob);

        //            bool nIsRec = _predictionDictionary[(int)predo] == "Recommended";
        //        }
        //    }
        //    //do
        //    //{
        //    //    userInput = Console.ReadLine();
        //    //    var newX = TextClassificationProblemBuilder.CreateNode(userInput, vocabulary);

        //    //    var predictedY = model.Predict(newX);
        //    //    Console.WriteLine("The prediction is {0}", _predictionDictionary[(int)predictedY]);
        //    //    Console.WriteLine(new string('=', 50));
        //    //} while (userInput != "quit");

        //}

        //private static void readData()
        //{
        //    const String dataFilePath = @"C:\Users\Bar\source\repos\zona\zona\Data.csv";
        //    var dataTable = DataTable.New.ReadCsv(dataFilePath);
        //    List<string> x = dataTable.Rows.Select(row => row["Text"]).ToList();
        //    double[] y = dataTable.Rows.Select(row => double.Parse(row["IsRecommended"])).ToArray();
        //    var vocabulary = x.SelectMany(GetWords).Distinct().OrderBy(word => word).ToList();

        //}

        private static IEnumerable<string> GetWords(string x)
        {
            return x.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }
        public static svm_node[] CreateNode(string strComm)
        {
           return TextClassificationProblemBuilder.CreateNode(strComm, Vocabulary);
        }
    }

    public class TextClassificationProblemBuilder
    {
        public svm_problem CreateProblem(IEnumerable<string> x, double[] y, IReadOnlyList<string> vocabulary)
        {
            return new svm_problem
            {
                y = y,
                x = x.Select(xVector => CreateNode(xVector, vocabulary)).ToArray(),
                l = y.Length
            };
        }

        public static svm_node[] CreateNode(string x, IReadOnlyList<string> vocabulary)
        {
            var node = new List<svm_node>(vocabulary.Count);

            string[] words = x.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < vocabulary.Count; i++)
            {
                int occurenceCount = words.Count(s => String.Equals(s, vocabulary[i], StringComparison.OrdinalIgnoreCase));
                if (occurenceCount == 0)
                    continue;

                node.Add(new svm_node
                {
                    index = i + 1,
                    value = occurenceCount
                });
            }

            return node.ToArray();
        }
    }
}