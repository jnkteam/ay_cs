using System;
using System.Text.RegularExpressions;

namespace OriginalStudio.Lib.Utils
{
	public class TypeParse
	{
		public static bool IsDouble(object Expression)
		{
			return Expression != null && Regex.IsMatch(Expression.ToString(), "^([0-9])[0-9]*(\\.\\w*)?$");
		}

		public static bool IsNumeric(object Expression)
		{
			bool result;
			if (Expression != null)
			{
				string input = Expression.ToString();
				if (input.Length > 0 && input.Length <= 11 && Regex.IsMatch(input, "^[-]?[0-9]*[.]?[0-9]*$") && (input.Length < 10 || (input.Length == 10 && input[0] == '1') || (input.Length == 11 && input[0] == '-' && input[1] == '1')))
				{
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}

		public static bool IsNumericArray(string[] strNumber)
		{
			bool result;
			if (strNumber == null || strNumber.Length < 1)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < strNumber.Length; i++)
				{
					object Expression = strNumber[i];
					if (!TypeParse.IsNumeric(Expression))
					{
						result = false;
						return result;
					}
				}
				result = true;
			}
			return result;
		}

		public static bool StrToBool(object Expression, bool defValue)
		{
			bool result;
			if (Expression != null)
			{
				if (string.Compare(Expression.ToString(), "true", true) == 0)
				{
					result = true;
					return result;
				}
				if (string.Compare(Expression.ToString(), "false", true) == 0)
				{
					result = false;
					return result;
				}
			}
			result = defValue;
			return result;
		}

		public static float StrToFloat(object strValue, float defValue)
		{
			float result;
			if (strValue == null || strValue.ToString().Length > 10)
			{
				result = defValue;
			}
			else
			{
				float num = defValue;
				if (strValue != null && Regex.IsMatch(strValue.ToString(), "^([-]|[0-9])[0-9]*(\\.\\w*)?$"))
				{
					num = Convert.ToSingle(strValue);
				}
				result = num;
			}
			return result;
		}

        public static decimal StrToDecimal(object strValue, float defValue)
        {
            try
            {
                return Convert.ToDecimal(strValue);
            }
            catch
            {
                return Convert.ToDecimal(defValue);
            }
        }

        public static int StrToInt(object expression, int defValue)
		{
            //int result;
            //if (expression != null)
            //{
            //    string input = expression.ToString();
            //    if (input.Length > 0 && input.Length <= 11 && Regex.IsMatch(input, "^[-]?[0-9]*$") && (input.Length < 10 || (input.Length == 10 && input[0] == '1') || (input.Length == 11 && input[0] == '-' && input[1] == '1')))
            //    {
            //        result = Convert.ToInt32(input);
            //        return result;
            //    }
            //}
            //result = defValue;
            //return result;

            try
            {
                int result = 0;
                if (Int32.TryParse(expression.ToString(), out result))
                    return result;
                else
                    return defValue;
            }
            catch
            {
                return defValue;
            }
		}

		public static int[] StringToIntArray(string idList, int defValue)
		{
			int[] result;
			if (string.IsNullOrEmpty(idList))
			{
				result = null;
			}
			else
			{
				string[] strArray = Utils.SplitString(idList, ",");
				int[] numArray = new int[strArray.Length];
				for (int index = 0; index < strArray.Length; index++)
				{
					numArray[index] = TypeParse.StrToInt(strArray[index], defValue);
				}
				result = numArray;
			}
			return result;
		}

		public static int ObjectToInt(object expression)
		{
			return TypeParse.ObjectToInt(expression, 0);
		}

		public static int ObjectToInt(object expression, int defValue)
		{
			int result;
			if (expression != null)
			{
				result = TypeParse.StrToInt(expression.ToString(), defValue);
			}
			else
			{
				result = defValue;
			}
			return result;
		}

        public static long StrToLong(object expression, long defValue)
        {
            try
            {
                long result = 0;
                if (long.TryParse(expression.ToString(), out result))
                    return result;
                else
                    return defValue;
            }
            catch
            {
                return defValue;
            }
        }

        public static DateTime StrToDateTime(object expression, DateTime defValue)
        {
            try
            {
                DateTime result = DateTime.Now;
                if (DateTime.TryParse(expression.ToString(), out result))
                    return result;
                else
                    return defValue;
            }
            catch
            {
                return defValue;
            }
        }
    }
}
