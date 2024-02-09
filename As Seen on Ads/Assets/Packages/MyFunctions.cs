using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCalc;


    public class MyFunctions : MonoBehaviour
    {
        public static string FormatExpression(string raw, float input)
        {
            // For convenience sake, convert sin, cos and tan
            raw = raw.Replace("sin(", "Sin((3.14592/180)*");
            raw = raw.Replace("cos(", "Cos((3.14592/180)*");
            raw = raw.Replace("tan(", "Tan((3.14592/180)*");

            // Replace "x" with the actual value of x
            raw = raw.Replace("HP", input.ToString());

            // Convert || to Abs()
            // use a bool to switch between "Abs(" and ")"
            List<char> rawChar = new List<char>();
            bool endAbs = false;
            foreach (char c in raw)
            {
                if (c == '|')
                {
                    rawChar.AddRange(!endAbs ? "Abs(" : ")");
                    endAbs = !endAbs ? true : false;
                }
                else rawChar.Add(c);
            }

            raw = new string(rawChar.ToArray());
            return raw;
        }

        public static float EvaluateExpression(string expression)
        {
            // Create an expression evaluator
            Expression expr = new Expression(expression);

            float result = Mathf.Epsilon; // A small value to handle potential floating-point precision issues
            try
            {
                result = float.Parse(expr.Evaluate().ToString());
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error evaluating expression: " + e.Message);
            }

            return result;
        }
    }

