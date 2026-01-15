using UnityEngine;
using System.Text;
using System.Globalization;
namespace MagmaLabs.Utilities.Strings{
public class Strings : MonoBehaviour
{


public static string CamelCaseToWords(string input)
{
    if (string.IsNullOrEmpty(input))
        return input;

    var sb = new StringBuilder();
    sb.Append(char.ToUpper(input[0]));

    for (int i = 1; i < input.Length; i++)
    {
        char c = input[i];

        // Word boundary: lower -> upper
        if (char.IsUpper(c) && char.IsLower(input[i - 1]))
        {
            sb.Append(' ');
        }

        sb.Append(c);
    }

    return sb.ToString();
}

}

}
