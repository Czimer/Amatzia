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
                const String dataFilePath = @"\Data.csv";
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
            try
            {
                var problemBuilder = new TextClassificationProblemBuilder();
                var problem = problemBuilder.CreateProblem(X, Y, Vocabulary);
                return problem;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        public static C_SVC CreateModel()
        {
            try
            {
                var prob = CreateProblem(X, Y, Vocabulary);
                const int C = 1;
                return new C_SVC(prob, KernelHelper.LinearKernel(), C);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool IsRec(string strComm)
        {
            try
            {
                var prob = MLRecommended.CreateNode(strComm);
                var predo = MLRecommended.CreateModel().Predict(prob);

                return Dictionary[(int)predo] == "Recommended";
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static IEnumerable<string> GetWords(string x)
        {
            try
            {
                return x.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static svm_node[] CreateNode(string strComm)
        {
            try
            {
                return TextClassificationProblemBuilder.CreateNode(strComm, Vocabulary);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    public class TextClassificationProblemBuilder
    {
        public svm_problem CreateProblem(IEnumerable<string> x, double[] y, IReadOnlyList<string> vocabulary)
        {
            try
            {
                return new svm_problem
                {
                    y = y,
                    x = x.Select(xVector => CreateNode(xVector, vocabulary)).ToArray(),
                    l = y.Length
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static svm_node[] CreateNode(string x, IReadOnlyList<string> vocabulary)
        {
            try
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
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}