using System;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace ChatApplication.Functions;

public static class OpenXmlExtensions
{
    public static string InnerTextWithBreaks(this OpenXmlElement element)
    {
        var stringBuilder = new StringBuilder();
        
        foreach (var child in element.ChildElements)
        {
            if (child is Text text)
            {
                stringBuilder.Append(text.Text);
            }
            else if (child is Break lineBreak)
            {
                if (lineBreak.Type != null && lineBreak.Type == BreakValues.TextWrapping)
                {
                    stringBuilder.Append(Environment.NewLine);
                }
            }
            else
            {
                stringBuilder.Append(InnerTextWithBreaks(child));
            }
        }
        
        return stringBuilder.ToString();
    }
}